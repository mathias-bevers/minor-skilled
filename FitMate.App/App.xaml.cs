using FitMate.Views;

namespace FitMate;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

		MainPage = new AppShell();
		Routing.RegisterRoute("Workouts/Workout", typeof(WorkoutPage));
	}
}
