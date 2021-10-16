﻿using System;
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
						foreach (var word in AsWordObj(entry))
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

		internal static IEnumerable<Word> AsWordObj(string[] entry)
		{
			if (entry.Length != 5)
				yield break;
			var lang2 = entry[3].Trim();
			if (string.IsNullOrWhiteSpace(lang2))
				yield break;
			foreach (var part in lang2.Split(new[] { ", ", "," }, StringSplitOptions.None))
				yield return new Word
				{
					Sound = entry[4],
					Lection = entry[1],
					Language1 = entry[0],
					Language2 = part.Trim(),
					Transcription = entry[2]
				};
		}
	}
}