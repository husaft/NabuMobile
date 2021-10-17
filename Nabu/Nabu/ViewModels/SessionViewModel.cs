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
			}
		}

		public Vocabulary Vocabulary => Unit?.Vocabulary ?? new Vocabulary();
		public Mode Mode => Unit?.Mode ?? new Mode();
		public string Lection => Unit?.Lection;

		public double Correct => CorrectCount / ((LectionWords?.Count ?? 1) * MaxRepetitions);
		public double Wrong => WrongCount / ((LectionWords?.Count ?? 1) * MaxRepetitions);
		public bool PlaySound { get; set; } = true;
		public IDictionary<string, int> Repetitions { get; set; }
		private const double MaxRepetitions = 2.0;
		public List<Word> LectionWords { get; set; }
		private Random _random;

		public bool IsMode1 => Mode.Id == 0;
		public bool IsMode2 => Mode.Id == 1;
		public bool IsMode3 => Mode.Id == 2;

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

		private int WrongCount;
		private int CorrectCount;
		private string _currentInput;
		private Word _currentWord;

		private void OnOk()
		{
			bool isCorrect;
			if (IsMode1)
				isCorrect = CompareText(CurrentWord.Language2, CurrentInput);
			else if (IsMode2)
				isCorrect = CompareText(CurrentWord.Language1, CurrentInput);
			else if (IsMode3)
				isCorrect = CompareText(CurrentWord.Transcription, CurrentInput);
			else
				return;
			if (isCorrect)
				CorrectCount++;
			else
				WrongCount++;
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
			WrongCount++;
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
			NextWord();
		}

		private void NextWord()
		{
			CurrentInput = null;
			if (LectionWords == null)
			{
				OnAppearing();
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
	}
}