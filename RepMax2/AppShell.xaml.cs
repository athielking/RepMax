using RepMax2.Pages;
using RepMax2.ViewModels;

namespace RepMax2;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

		Routing.RegisterRoute("AddExercise", typeof(AddExercisePage));
	}
}
