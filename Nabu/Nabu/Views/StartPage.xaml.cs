using Nabu.ViewModels;
using Xamarin.Forms;

namespace Nabu.Views
{
	public partial class StartPage : ContentPage
	{
		private StartViewModel _viewModel;

		public StartPage()
		{
			InitializeComponent();

			BindingContext = _viewModel = new StartViewModel();
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();
			_viewModel.OnAppearing();
		}
	}
}