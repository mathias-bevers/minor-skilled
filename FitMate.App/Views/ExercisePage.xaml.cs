using System.Text.RegularExpressions;
using FitMate.ViewModels;

namespace FitMate.Views;

public partial class ExercisePage : ContentPage
{
    private ExerciseViewModel ViewModel { get; } = new();

    public ExercisePage()
    {
        Title = "Exercise";
        ViewModel.UpdateTitleEvent += OnUpdateTitle;
        BindingContext = ViewModel;

        InitializeComponent();
    }

    private void OnUpdateTitle(string newTitle) => Title = newTitle;

    private bool HasOnlyDigits(string source)
    {
        for (int i = 0; i < source.Length; ++i)
        {
            if (!char.IsDigit(source[i])) { return false; }
        }

        return true;
    }

    private void OnAddExercise(object sender, EventArgs args)
    {
        if (string.IsNullOrEmpty(ViewModel.KgsOrMtr) || string.IsNullOrEmpty(ViewModel.RepsOrSecs))
        {
            DisplayAlert("Invalid Input", "Make sure both fields are not empty.", "OK");
            return;
        }

        // Regex replaces all comma's with an empty string. 
        if (!HasOnlyDigits(ViewModel.KgsOrMtr) ||
            !HasOnlyDigits(Regex.Replace(ViewModel.RepsOrSecs, "[,]", string.Empty)))
        {
            DisplayAlert("Invalid Input", "Make sure both fields only contain numbers or a \',\'", "OK");
            return;
        }


        int kgsOrMtr = Convert.ToInt32(ViewModel.KgsOrMtr);
        int repsOrSecs = Convert.ToInt32(ViewModel.RepsOrSecs);
        string? errorMessage = Task.Run(() => ViewModel.AddExerciseAsync(kgsOrMtr, repsOrSecs)).Result;

        if (string.IsNullOrEmpty(errorMessage)) { ViewModel.KgsOrMtr = ViewModel.RepsOrSecs = string.Empty; }
        else { DisplayAlert("Database error", errorMessage, "OK"); }
    }

    private void OnHistoryClicked(object sender, EventArgs args)
    {
        ShellNavigationQueryParameters navigationQueryParameters = new()
        {
            { "exercise_name", ViewModel.ExerciseTypeName }
        };

        Shell.Current.GoToAsync("/History", navigationQueryParameters);
    }

    private void OnLeaderboardClicked(object sender, EventArgs args)
    {
        ShellNavigationQueryParameters navigationQueryParameters = new()
        {
            { "exercise_name", ViewModel.ExerciseTypeName }
        };

        Shell.Current.GoToAsync("/LeaderBoard", navigationQueryParameters);
    }
}