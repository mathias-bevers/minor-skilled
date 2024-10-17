using System.Diagnostics;
using FitMate.ViewModels;
using Syncfusion.Maui.Picker;

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
        int kgsOrMtr;
        int repsOrSecs;
        
        if (ViewModel.IsInKgs)
        {
            if (string.IsNullOrEmpty(ViewModel.KgsOrMtr) || string.IsNullOrEmpty(ViewModel.Repetitions))
            {
                DisplayAlert("Invalid Input", "Make sure both fields are not empty!", "OK");
                return;
            }
            
            repsOrSecs = Convert.ToInt32(ViewModel.Repetitions);
        }
        else
        {
            if (string.IsNullOrEmpty(ViewModel.KgsOrMtr) || ViewModel.Seconds == TimeSpan.Zero)
            {
                DisplayAlert("Invalid Input", "Make sure both fields are not empty!", "OK");
                return;
            }
            repsOrSecs = Convert.ToInt32(Math.Round(ViewModel.Seconds.TotalSeconds));
        }
        
        kgsOrMtr = Convert.ToInt32(ViewModel.KgsOrMtr);
        string? errorMessage = Task.Run(() => ViewModel.AddExerciseAsync(kgsOrMtr, repsOrSecs)).Result;

        if (string.IsNullOrEmpty(errorMessage)) { ViewModel.KgsOrMtr = ViewModel.Repetitions = string.Empty; }
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

    private void OnSelectTimeClicked(object sender, EventArgs args) => ViewModel.IsTimePickerOpened = true;

    private void OnTimePickerOk(object sender, EventArgs args)
    {
        //NOTE: the sync-fusion library does not seem to work with observable properties.
        SfTimePicker timePicker = (SfTimePicker)sender;
        Debug.Assert(timePicker.SelectedTime != null, "timePicker.SelectedTime != null");
        ViewModel.Seconds = timePicker.SelectedTime.Value;
        ViewModel.TimeButton = ViewModel.Seconds.ToString(@"hh\:mm\:ss");
        ViewModel.IsTimePickerOpened = false;
    }

    private void OnTimePickerCancel(object sender, EventArgs args)
    {
        ViewModel.Seconds = TimeSpan.Zero;
        ViewModel.IsTimePickerOpened = false;
    }
}