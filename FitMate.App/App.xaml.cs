using FitMate.Views;
using Microsoft.Extensions.Configuration;

namespace FitMate;

public partial class App : Application
{
    public static int UserID { get; private set; }
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

    public static void DeleteUserID()
    {
        string filePath = Path.Combine(FileSystem.AppDataDirectory, "tasks.json");
        if (!File.Exists(filePath))
        {
            return;
        }

        File.Delete(filePath);
    }

    public static void SetUserID(int id = -1)
    {
        string filePath = Path.Combine(FileSystem.AppDataDirectory, "tasks.json");

        if (!File.Exists(filePath))
        {
            File.WriteAllText(filePath, string.Empty);
        }

        if (id >= 0)
        {
            File.WriteAllText(filePath, id.ToString());
            UserID = id;
        }
        else
        {
            string fileContent = File.ReadAllText(filePath);

            if (string.IsNullOrEmpty(fileContent))
            {
                UserID = -1;
                return;
            }

            UserID = int.Parse(fileContent);
        }
    }

    protected override Window CreateWindow(IActivationState? activationState) => new(new AppShell());


    private static void SetSettings(IConfiguration configuration) =>
        SETTINGS = configuration.GetSection("Settings").Get<Settings>();
}