namespace FitMate.Views;

public partial class ExerciseTypePage : ContentPage
{
    private readonly ViewModels.ExerciseTypeViewModel viewModel;

    public ExerciseTypePage()
    {
        InitializeComponent();
        Title = "New Exercise Preset";

        viewModel = new ViewModels.ExerciseTypeViewModel();
        BindingContext = viewModel;
    }

    private void OnSaveClicked(object sender, EventArgs eventArgs)
    {
        if (string.IsNullOrEmpty(viewModel.ExerciseName) || viewModel.SelectedMuscleType < 0 ||
            viewModel.SelectedMeasurementType < 0)
        {
            DisplayAlert("Invalid Input", "Make sure all fields are filled.", "OK");
            return;
        }
        
        string output = $"Successfully created \'{viewModel.ExerciseName}\'";
        output += $" of muscle group \'{((ViewModels.Mockups.MuscleGroup)viewModel.SelectedMuscleType).ToString()}\'";
        output += $" of measurement type \'{(viewModel.SelectedMeasurementType == 0 ?
            "kg per repetition" : "meter per second")}\'.";
        
        DisplayAlert("Success", output, "OK");
    }
}