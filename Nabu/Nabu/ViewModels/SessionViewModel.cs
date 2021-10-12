using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Nabu.ViewModels
{
    public class SessionViewModel : BaseViewModel
    {
        public SessionViewModel()
        {
            Title = "Session";
            OpenWebCommand = new Command(async () => await Browser.OpenAsync("https://aka.ms/xamarin-quickstart"));
        }

        public ICommand OpenWebCommand { get; }
    }
}