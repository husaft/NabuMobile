using System.Collections.Generic;
using System.Linq;
using Nabu.Models;
using Nabu.Services;

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

		public SessionViewModel()
		{
			Title = "Session";
			Repetitions = new SortedDictionary<string, int>();
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
	}
}