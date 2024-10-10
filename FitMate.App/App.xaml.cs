using FitMate.DataBase;
using FitMate.Views;
using Microsoft.Extensions.Configuration;

namespace FitMate;

public partial class App : Application
{
    public static readonly int USER_ID = GetUserId();
    internal static ServerSettings SERVER_SETTINGS { get; private set; } = new();
    private IConfiguration configuration;

    public App(IConfiguration configuration)
    {
        InitializeComponent();

        this.configuration = configuration;
        SetServerSettings(configuration);

        MainPage = new AppShell();
        Routing.RegisterRoute("AllWorkouts/Workout", typeof(WorkoutPage));
        Routing.RegisterRoute("AllWorkouts/Workout/AllExercises", typeof(AllExercisesPage));
        Routing.RegisterRoute("AllWorkouts/Workout/Exercise", typeof(ExercisePage));
        Routing.RegisterRoute("AllWorkouts/Workout/Exercise/History", typeof(ExerciseHistoryPage));
        Routing.RegisterRoute("AllWorkouts/Workout/Exercise/LeaderBoard", typeof(ExerciseLeaderboardPage));
        Routing.RegisterRoute("AllWorkouts/Workout/AllExercises/ExerciseType", typeof(ExerciseTypePage));
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

    private static void SetServerSettings(IConfiguration configuration)
    { 
        SERVER_SETTINGS = configuration.GetSection("Settings:Server").Get<ServerSettings>();
    }
}