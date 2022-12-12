using CommunityToolkit.Maui;
using RepMax2.Data;
using RepMax2.Pages;
using RepMax2.ViewModels;

namespace RepMax2;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.UseMauiCommunityToolkit()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

		var services = builder.Services;
		services.AddSingleton<IRepository, Repository>();

		services.AddSingleton<MainPage>();
		services.AddTransient<MainPageViewModel>();
		services.AddTransient<AddExercisePage>();
		services.AddTransient<AddExerciseViewModel>();

		return builder.Build();
	}
}
