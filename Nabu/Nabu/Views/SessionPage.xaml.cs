using Nabu.ViewModels;
using Xamarin.Forms;

namespace Nabu.Views
{
	public partial class SessionPage : ContentPage
	{
		private SessionViewModel _viewModel;

		public SessionPage()
		{
			InitializeComponent();

			BindingContext = _viewModel = new SessionViewModel();
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();
			_viewModel.OnAppearing();
		}
	}
}