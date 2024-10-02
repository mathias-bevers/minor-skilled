using FitMate.DataBase;
using FitMate.Views;

namespace FitMate;

public partial class App : Application
{
    public static readonly ServerSettings SERVER_SETTINGS = new();
    public static readonly int USER_ID = GetUserId();

    public App()
    {
        InitializeComponent();

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
}

public class ServerSettings : IServerSettings
{
    public string ConnectionString => $"Server={Server};Database=FitMate;User Id={UserName};" +
                                      $"Password={Password};MultipleActiveResultSets=true;Encrypt=false;";
    public string Server => "192.168.1.135";
    public string UserName => "application";
    public string Password => "P@$$w0rd";
}