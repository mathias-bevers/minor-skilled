﻿using FitMate.Views;

namespace FitMate;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

		MainPage = new AppShell();
		Routing.RegisterRoute("AllWorkouts/Workout", typeof(WorkoutPage));
		Routing.RegisterRoute("AllWorkouts/Workout/AllExercises", typeof(AllExercisesPage));
		Routing.RegisterRoute("AllWorkouts/Workout/Exercise", typeof(ExercisePage));
	}
}
