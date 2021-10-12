using System.Collections.Generic;
using System.Linq;
using Nabu.Helpers;
using Nabu.Models;
using Nabu.ViewModels;
using Xamarin.Forms;

namespace Nabu.Views
{
	public partial class WordsPage : ContentPage
	{
		private WordsViewModel _viewModel;

		public WordsPage()
		{
			InitializeComponent();

			BindingContext = _viewModel = new WordsViewModel();
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();
			_viewModel.OnAppearing();
		}

		private IEnumerable<Word> _all;

		private void InputView_OnTextChanged(object sender, TextChangedEventArgs e)
		{
			if (_all == null)
			{
				_all = dictView.ItemsSource.Cast<Word>();
			}
			if (string.IsNullOrWhiteSpace(e.NewTextValue))
			{
				dictView.ItemsSource = _all;
				return;
			}
			var text = e.NewTextValue;
			dictView.ItemsSource = _all.Where(x =>
				x.Language1.Contains(text, TextHelper.IgnoreCase) ||
				x.Language2.Contains(text, TextHelper.IgnoreCase) ||
				x.Transcription.Contains(text, TextHelper.IgnoreCase)
			);
		}
	}
}