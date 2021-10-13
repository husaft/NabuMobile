using Nabu.Models;

namespace Nabu.ViewModels
{
	public class SessionViewModel : BaseViewModel
	{
		public static Unit Unit { get; set; }

		public Vocabulary Vocabulary => Unit?.Vocabulary ?? new Vocabulary();
		public Mode Mode => Unit?.Mode ?? new Mode();
		public string Lection => Unit?.Lection;

		public double Correct { get; set; } = 0.3;
		public double Wrong { get; set; } = 0.56;
		public bool PlaySound { get; set; } = true;

		public SessionViewModel()
		{
			Title = "Session";
		}

		public void OnAppearing()
		{
			IsBusy = true;
		}
	}
}