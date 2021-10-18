using System.Linq;
using Nabu.Helpers;
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

		private void InputView_OnTextChanged(object sender, TextChangedEventArgs e)
		{
			var text = e.NewTextValue;
			if (string.IsNullOrWhiteSpace(text))
			{
				dictView.ItemsSource = _viewModel.AllWords.Take(_viewModel.MaxNumber);
				return;
			}
			dictView.ItemsSource = _viewModel.AllWords.Where(x =>
					 x.Language1.Contains(text, TextHelper.InvIgnoreCase) ||
					 x.Language2.Contains(text, TextHelper.InvIgnoreCase) ||
					 x.Transcription.Contains(text, TextHelper.InvIgnoreCase) ||
					 x.Transcription.ToASCII().Contains(text, TextHelper.InvIgnoreCase)
			).Take(_viewModel.MaxNumber);
		}
	}
}