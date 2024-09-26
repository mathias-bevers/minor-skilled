using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitMate.Views;

public partial class ExerciseLeaderboardPage : ContentPage
{
    private ViewModels.ExerciseLeaderboardViewModel viewModel;
    
    public ExerciseLeaderboardPage()
    {
        InitializeComponent();
        Title = "Leaderboard";
        
        viewModel = new ViewModels.ExerciseLeaderboardViewModel();
        BindingContext = viewModel;
    }
}