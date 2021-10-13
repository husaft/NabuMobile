using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Nabu.Models;
using Nabu.Views;
using Xamarin.Forms;

namespace Nabu.ViewModels
{
	public class StartViewModel : BaseViewModel
	{
		public ObservableCollection<Vocabulary> Vocabularies { get; }
		public int VocabularyIndex { get; set; } = -2;
		public ObservableCollection<Mode> Modes { get; }
		public int ModeIndex { get; set; } = -2;
		public ObservableCollection<string> Lections { get; }
		public int LectionIndex { get; set; } = -2;

		public Command LoadItemsCommand { get; }

		public Command GoCommand { get; }

		public StartViewModel()
		{
			Title = nameof(Nabu);
			Vocabularies = new ObservableCollection<Vocabulary>();
			Modes = new ObservableCollection<Mode>();
			Lections = new ObservableCollection<string>();
			LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

			GoCommand = new Command(OnGo, CanGo);
		}

		private bool CanGo(object arg) => VocabularyIndex >= 0 && ModeIndex >= 0 && LectionIndex >= 0;

		private async void OnGo(object obj)
		{
			var unit = new Unit
			{
				Vocabulary = Vocabularies[VocabularyIndex],
				Mode = Modes[ModeIndex],
				Lection = Lections[LectionIndex]
			};
			SessionViewModel.Unit = unit;
			await Shell.Current.GoToAsync($"//{nameof(SessionPage)}");
		}

		private async Task ExecuteLoadItemsCommand()
		{
			IsBusy = true;
			try
			{
				Vocabularies.Clear();
				Modes.Clear();
				Lections.Clear();
				var items = await DataStore.GetItemsAsync(true);
				foreach (var item in items)
				{
					var voc = item.Vocabulary;
					Vocabularies.Add(voc);
					foreach (var mode in voc.Modes)
						Modes.Add(mode);
					foreach (var lection in voc.Src.Labels)
						Lections.Add(lection);
				}
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
	}
}