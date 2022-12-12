using RepMax2.ViewModels;

namespace RepMax2.Pages;

public partial class AddExercisePage : ContentPage
{
	private readonly AddExerciseViewModel _viewModel;
	public AddExercisePage(AddExerciseViewModel viewModel)
	{
		InitializeComponent();
		_viewModel = viewModel;

		BindingContext = viewModel;
	}
}