using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Nabu.Models;
using Nabu.Services;
using Xamarin.Forms;

namespace Nabu.ViewModels
{
	public class WordsViewModel : BaseViewModel
	{
		public ObservableCollection<Word> Words { get; }
		public Command LoadItemsCommand { get; }

		public Word[] AllWords;
		public readonly int MaxNumber = 75;

		public WordsViewModel()
		{
			Title = "Words";
			Words = new ObservableCollection<Word>();
			LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
		}

		private async Task ExecuteLoadItemsCommand()
		{
			IsBusy = true;
			try
			{
				Words.Clear();
				var temp = new List<Word>();
				var items = await DataStore.GetItemsAsync(true);
				foreach (var item in items)
				{
					((JsonDataStore)DataStore).LoadWords(item);
					foreach (var entry in item.Vocabulary.Src.Words)
						foreach (var word in AsWordObj(entry, split: true))
							temp.Add(word);
				}
				AllWords = temp.OrderBy(t => t.Language2).ToArray();
				foreach (var word in AllWords.Take(MaxNumber))
					Words.Add(word);
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex);
			}
			finally
			{
				IsBusy = false;
			}
		}

		public void OnAppearing()
		{
			IsBusy = true;
		}

		private static string[] SplitWords(string text)
		{
			const StringSplitOptions cmp = StringSplitOptions.RemoveEmptyEntries;
			var parts = text
				.Split(new[] { ", ", "," }, cmp)
				.Select(p => p.Trim())
				.Where(p => !string.IsNullOrWhiteSpace(p));
			return parts.ToArray();
		}

		internal static IEnumerable<Word> AsWordObj(string[] entry, bool split)
		{
			if (entry.Length != 5)
				yield break;
			var lang2 = entry[3].Trim();
			if (string.IsNullOrWhiteSpace(lang2))
				yield break;
			foreach (var part in split ? SplitWords(lang2) : new[] { lang2 })
				yield return new Word
				{
					Sound = entry[4].Trim(),
					Lection = entry[1].Trim(),
					Language1 = entry[0].Trim(),
					Language2 = part.Trim(),
					Language2Array = split ? null : SplitWords(part),
					Transcription = entry[2].Trim()
				};
		}
	}
}