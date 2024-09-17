using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace minor_skilled.Views;


public partial class WorkoutsPage : ContentPage
{
    public WorkoutsPage()
    {
        InitializeComponent();
    }
    private void OnCreateNewWorkout(object? sender, EventArgs e)
    {
        Shell.Current.GoToAsync("/Workout");
    }
}