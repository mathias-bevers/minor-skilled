using FitMate.Views;
using Microsoft.Extensions.Configuration;

namespace FitMate;

public partial class App : Application
{
    public static readonly int USER_ID = GetUserId();
    internal static Settings SETTINGS { get; set; } = new();

    public App(IConfiguration configuration)
    {
        InitializeComponent();

        SetSettings(configuration);

        Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(SETTINGS.SyncfusionAPI);
        
        Routing.RegisterRoute("AllWorkouts/Workout", typeof(WorkoutPage));
        Routing.RegisterRoute("AllWorkouts/Workout/AllExercises", typeof(AllExercisesPage));
        Routing.RegisterRoute("AllWorkouts/Workout/Exercise", typeof(ExercisePage));
        Routing.RegisterRoute("AllWorkouts/Workout/Exercise/History", typeof(ExerciseHistoryPage));
        Routing.RegisterRoute("AllWorkouts/Workout/Exercise/LeaderBoard", typeof(ExerciseLeaderboardPage));
        Routing.RegisterRoute("AllWorkouts/Workout/AllExercises/ExerciseType", typeof(ExerciseTypePage));
        Routing.RegisterRoute("Friends/Profile", typeof(ProfilePage));
    }

    private static int GetUserId()
    {
        string filePath = Path.Combine(FileSystem.AppDataDirectory, "tasks.json");

        if (!File.Exists(filePath))
        {
            //TODO: Remove and login 
            File.WriteAllText(filePath, "1");
        }
        
        return int.Parse(File.ReadAllText(filePath));
    }

    protected override Window CreateWindow(IActivationState? activationState) => new(new AppShell());


    private static void SetSettings(IConfiguration configuration) =>
        SETTINGS = configuration.GetSection("Settings").Get<Settings>();
}