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
		private int _vocabularyIndex = -2;
		private int _modeIndex = -2;
		private int _lectionIndex = -2;
		public ObservableCollection<Vocabulary> Vocabularies { get; }

		public int VocabularyIndex
		{
			get => _vocabularyIndex; set { _vocabularyIndex = value; OnPropertyChanged(); }
		}

		public ObservableCollection<Mode> Modes { get; }

		public int ModeIndex
		{
			get => _modeIndex; set { _modeIndex = value; OnPropertyChanged(); }
		}

		public ObservableCollection<string> Lections { get; }

		public int LectionIndex
		{
			get => _lectionIndex; set { _lectionIndex = value; OnPropertyChanged(); }
		}

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
				VocabularyIndex = VocabularyIndex,
				Mode = Modes[ModeIndex],
				ModeIndex = ModeIndex,
				Lection = Lections[LectionIndex],
				LectionIndex = LectionIndex
			};
			Helpers.Environment.UpdateUnit(this, unit);
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
			var previous = Helpers.Environment.GetIfSet(this);
			if (previous != null)
			{
				VocabularyIndex = previous.VocabularyIndex;
				ModeIndex = previous.ModeIndex;
				LectionIndex = previous.LectionIndex;
			}
			IsBusy = false;
		}
	}
}