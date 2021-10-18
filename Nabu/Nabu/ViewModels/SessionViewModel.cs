using System;
using System.Collections.Generic;
using System.Linq;
using Nabu.Models;
using Nabu.Services;
using Xamarin.Forms;

namespace Nabu.ViewModels
{
	public class SessionViewModel : BaseViewModel, IUpdateable
	{
		private Unit Unit
		{
			get => Helpers.Environment.GetIfSet(this);
			set
			{
				OnPropertyChanged(nameof(Vocabulary));
				OnPropertyChanged(nameof(Mode));
				OnPropertyChanged(nameof(Lection));
				OnPropertyChanged(nameof(IsMode1));
				OnPropertyChanged(nameof(IsMode2));
				OnPropertyChanged(nameof(IsMode3));
				OnPropertyChanged(nameof(HasStillWords));
				OnPropertyChanged(nameof(LectionWordCount));
			}
		}

		public Vocabulary Vocabulary => Unit?.Vocabulary ?? new Vocabulary();
		public Mode Mode => Unit?.Mode ?? new Mode();
		public string Lection => Unit?.Lection;

		public double Correct => CorrectCount / (LectionWordCount * MaxRepetitions);
		public double Wrong => WrongCount / (LectionWordCount * MaxRepetitions);
		public bool PlaySound { get; set; } = true;
		public IDictionary<string, int> Repetitions { get; set; }
		private const double MaxRepetitions = 2.0;
		public List<Word> LectionWords { get; set; }
		private Random _random;

		public bool IsMode1 => Mode.Id == 0 && HasStillWords;
		public bool IsMode2 => Mode.Id == 1 && HasStillWords;
		public bool IsMode3 => Mode.Id == 2 && HasStillWords;

		public bool HasStillWords => LectionWords?.Count >= 1 || CurrentWord?.Transcription != null;

		public Word CurrentWord
		{
			get => _currentWord; set { _currentWord = value; OnPropertyChanged(); }
		}

		public string CurrentInput
		{
			get => _currentInput; set { _currentInput = value; OnPropertyChanged(); }
		}

		public Command OkCommand { get; }
		public Command SolveCommand { get; }

		public SessionViewModel()
		{
			_random = new Random();
			Title = "Session";
			Repetitions = new SortedDictionary<string, int>();

			OkCommand = new Command(OnOk);
			SolveCommand = new Command(OnSolve);

			ResetView();
		}

		public void OnAppearing()
		{
			IsBusy = true;
			if (Vocabulary?.Src != null)
			{
				((JsonDataStore)DataStore).LoadWords(Vocabulary);
				var words = Vocabulary.Src.Words;
				var l = Lection;
				var possible = words.Where(w => w.Length >= 2 &&
												w[1].Replace(".", string.Empty) == l)
					.SelectMany(w => WordsViewModel.AsWordObj(w, split: false));
				LectionWords = new List<Word>(possible);
				LectionWordCount = LectionWords.Count;
			}
			IsBusy = false;
		}

		private static bool CompareText(string first, string second)
		{
			first = first?.Trim('/').Trim();
			second = second?.Trim('/').Trim();
			if (first == second)
				return true;
			if (StringComparer.InvariantCultureIgnoreCase.Compare(first, second) == 0)
				return true;
			return false;
		}

		private int LectionWordCount;
		private int WrongCount;
		private int CorrectCount;
		private bool Solved;
		private string _currentInput;
		private Word _currentWord;

		private void OnOk()
		{
			bool isCorrect;
			if (IsMode1)
				isCorrect = new[] { CurrentWord.Language2 }.Concat(CurrentWord.Language2Array)
					.Any(w => CompareText(w, CurrentInput));
			else if (IsMode2)
				isCorrect = CompareText(CurrentWord.Language1, CurrentInput);
			else if (IsMode3)
				isCorrect = CompareText(CurrentWord.Transcription, CurrentInput);
			else
			{
				Unit = null;
				return;
			}
			if (!Solved && isCorrect)
				CorrectCount++;
			else
				WrongCount++;
			NextWord();
		}

		private void OnSolve()
		{
			if (IsMode1)
				CurrentInput = CurrentWord.Language2;
			else if (IsMode2)
				CurrentInput = CurrentWord.Language1;
			else if (IsMode3)
				CurrentInput = CurrentWord.Transcription;
			else
				return;
			Solved = true;
		}

		public void Update(object instance)
		{
			ResetView();
			Unit = (Unit)instance;
		}

		private void ResetView()
		{
			WrongCount = 0;
			CorrectCount = 0;
			LectionWordCount = 0;
			LectionWords = null;
			Repetitions.Clear();
			NextWord();
		}

		private void NextWord()
		{
			ShowProgress();
			CurrentInput = null;
			Solved = false;
			if (LectionWords == null)
			{
				OnAppearing();
			}
			if (LectionWords.Count == 0)
			{
				CurrentWord = null;
				Unit = null;
				return;
			}
			var newIndex = _random.Next(0, LectionWords.Count);
			var newWord = LectionWords[newIndex];
			var key = newWord.Transcription;
			Repetitions.TryGetValue(key, out var currentRepeat);
			currentRepeat++;
			if (currentRepeat >= (int)MaxRepetitions)
				LectionWords.RemoveAt(newIndex);
			Repetitions[key] = currentRepeat;
			CurrentWord = newWord;
		}

		private void ShowProgress()
		{
			OnPropertyChanged(nameof(Correct));
			OnPropertyChanged(nameof(Wrong));
		}
	}
}