using Nabu.Services;
using Xamarin.Forms;

namespace Nabu
{
	public partial class App : Application
	{

		public App()
		{
			InitializeComponent();

			DependencyService.Register<JsonDataStore>();
			MainPage = new AppShell();
		}

		protected override void OnStart()
		{
		}

		protected override void OnSleep()
		{
		}

		protected override void OnResume()
		{
		}
	}
}
