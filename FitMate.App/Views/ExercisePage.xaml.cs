using System.Diagnostics;
using System.Text.RegularExpressions;
using FitMate.ViewModels;

namespace FitMate.Views;

public partial class ExercisePage : ContentPage
{
    private ExerciseViewModel ViewModel { get; }= new ();

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
        
        DisplayAlert("Success", "Exercise has been added.", "OK");
        ViewModel.KgsOrMtr = ViewModel.RepsOrSecs = string.Empty;
    }

    private void OnHistoryClicked(object sender, EventArgs args)
    {
        Shell.Current.GoToAsync($"/History");
    }
    
    private void OnLeaderboardClicked(object sender, EventArgs args)
    {
        Shell.Current.GoToAsync($"/LeaderBoard");
    }
}