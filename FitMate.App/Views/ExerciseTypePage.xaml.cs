using System.Text.RegularExpressions;
using FitMate.Utils;

namespace FitMate.Views;

public partial class ExerciseTypePage : ContentPage
{
    private readonly ViewModels.ExerciseTypeViewModel viewModel = new();
    private readonly Regex regex = new(@"^[A-Za-z\s]+$");

    public ExerciseTypePage()
    {
        InitializeComponent();
        Title = "New Exercise Preset";

        BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        try
        {
            viewModel.SelectFromDB();
        }
        catch (PopupException e)
        {
            System.Diagnostics.Debug.WriteLine(e.Message);
            DisplayAlert(e.Title, e.Message, "OK");
        }
    }

    private void OnSaveClicked(object sender, EventArgs eventArgs)
    {
        if (string.IsNullOrEmpty(viewModel.ExerciseName) || viewModel.SelectedMuscleType < 0 ||
            viewModel.SelectedMeasurementType < 0)
        {
            DisplayAlert("Invalid Input", "Make sure all fields are filled.", "OK");
            return;
        }

        if (!regex.IsMatch(viewModel.ExerciseName))
        {
            DisplayAlert("Invalid Input", "The exercise name can only contain letters and spaces", "OK");
            return;
        }

        try
        {
            string result = Task.Run(viewModel.InsertExerciseType).Result;
            DisplayAlert("Success", result, "OK");
        }
        catch (PopupException exception)
        {
            DisplayAlert(exception.Title, exception.Message, "OK");
        }
    }
}