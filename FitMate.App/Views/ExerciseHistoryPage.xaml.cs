using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitMate.Views;

public partial class ExerciseHistoryPage : ContentPage
{
    ViewModels.ExerciseHistoryViewModel viewModel;
    
    public ExerciseHistoryPage()
    {
        InitializeComponent();
        Title = "History";
        
        viewModel = new ViewModels.ExerciseHistoryViewModel();
        BindingContext = viewModel;
    }
}