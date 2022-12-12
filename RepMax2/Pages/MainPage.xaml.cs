using RepMax2.ViewModels;

namespace RepMax2.Pages;

public partial class MainPage : ContentPage
{
	int count = 0;
	private readonly MainPageViewModel _viewModel;
	public MainPage(MainPageViewModel viewModel)
	{
		InitializeComponent();

		_viewModel = viewModel;
		BindingContext = _viewModel;
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();
		//await _viewModel.LoadData();		
	}
}

