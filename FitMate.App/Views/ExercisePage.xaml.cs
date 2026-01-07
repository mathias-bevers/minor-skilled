using System.Diagnostics;
using FitMate.Utils;
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

        try
        {
            Task.Run(() => ViewModel.InsertExerciseAsync(kgsOrMtr, repsOrSecs)).Wait();
        }
        catch (AggregateException ae)
        {
            for (int i = 0; i < ae.InnerExceptions.Count; ++i)
            {
                if (ae.InnerExceptions[i] is not PopupException pe)
                {
                    continue;
                }

                DisplayAlert(pe.Title, pe.Message, "OK");
                return;
            }

            throw;
        }

        // try
        // {
        //     string? errorMessage = Task.Run(() => ViewModel.InsertExerciseAsync(kgsOrMtr, repsOrSecs)).Result;
        // }
        // catch (AggregateException e)
        // {
        //     Console.WriteLine(e);
        //     throw;
        // }
        //
        // if (string.IsNullOrEmpty(errorMessage))
        // {
        //     ViewModel.KgsOrMtr = ViewModel.Repetitions = string.Empty;
        //     ViewModel.Seconds = TimeSpan.Zero;
        //     ViewModel.TimeButton = "Set Time";
        // }
        // else
        // {
        //     DisplayAlert("Database error", errorMessage, "OK");
        // }
    }

    private void OnHistoryClicked(object sender, EventArgs args)
    {
        ShellNavigationQueryParameters navigationQueryParameters = new()
        {
            { "exercise_name", ViewModel.ExerciseTypeID }
        };

        Shell.Current.GoToAsync("/History", navigationQueryParameters);
    }

    private void OnLeaderboardClicked(object sender, EventArgs args)
    {
        ShellNavigationQueryParameters navigationQueryParameters = new()
        {
            { "exercise_name", ViewModel.ExerciseTypeID }
        };

        Shell.Current.GoToAsync("/LeaderBoard", navigationQueryParameters);
    }

    private void OnSelectTimeClicked(object sender, EventArgs args) => ViewModel.IsTimePickerOpened = true;

    private void OnTimePickerOk(object sender, EventArgs args)
    {
        //NOTE: the sync-fusion library does not seem to work with observable properties.
        SfTimePicker timePicker = (SfTimePicker)sender;
        Debug.Assert(timePicker.SelectedTime != null);
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