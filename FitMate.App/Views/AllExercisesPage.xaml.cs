using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitMate.Views;

public partial class AllExercisesPage : ContentPage
{
    private ViewModels.AllExercisesViewModel viewModel;
    
    public AllExercisesPage()
    {
        InitializeComponent();
        Title = "All Exercises";
        
        viewModel = new ViewModels.AllExercisesViewModel();
        BindingContext = viewModel;
    }

    private void OnExerciseSelected(object? sender, SelectionChangedEventArgs e)
    {
        Shell.Current.GoToAsync("../Exercise");
    }
}