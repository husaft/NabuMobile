using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Nabu.ViewModels
{
    public class WordsViewModel : BaseViewModel
    {
        public WordsViewModel()
        {
            Title = "Words";
            OpenWebCommand = new Command(async () => await Browser.OpenAsync("https://aka.ms/xamarin-quickstart"));
        }

        public ICommand OpenWebCommand { get; }
    }
}