﻿using System;
using System.Collections.Generic;
using System.Linq;
using Nabu.Models;
using Nabu.Services;
using Xamarin.Forms;

namespace Nabu.ViewModels
{
	public class SessionViewModel : BaseViewModel
	{
		public static Unit Unit { get; set; }

		public Vocabulary Vocabulary => Unit?.Vocabulary ?? new Vocabulary();
		public Mode Mode => Unit?.Mode ?? new Mode();
		public string Lection => Unit?.Lection;

		public double Correct { get; set; } = 0.3;
		public double Wrong { get; set; } = 0.56;
		public bool PlaySound { get; set; } = true;
		public IDictionary<string, int> Repetitions { get; set; }
		public Word[] LectionWords { get; set; }
		private Random _random;

		public bool IsMode1 => Mode.Id == 0;
		public bool IsMode2 => Mode.Id == 1;
		public bool IsMode3 => Mode.Id == 2;

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
					.SelectMany(WordsViewModel.AsWordObj)
					.ToArray();
				LectionWords = possible;
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
	}
}