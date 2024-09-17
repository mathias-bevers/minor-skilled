using minor_skilled.Views;

namespace minor_skilled;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

		MainPage = new AppShell();
		Routing.RegisterRoute("Workouts/Workout", typeof(WorkoutPage));
	}
}
