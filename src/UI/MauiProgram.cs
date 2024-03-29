﻿using CommunityToolkit.Maui;
using GraphVisitor.UI.Pages;
using GraphVisitor.UI.Services;
using GraphVisitor.UI.ViewModels;
using Maui.Plugins.PageResolver;
using Microsoft.Extensions.Logging;

namespace GraphVisitor.UI;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			})
			.UseMauiCommunityToolkit()
			.UsePageResolver();

#if DEBUG
		builder.Logging.AddDebug();
#endif

		builder.Services.AddSingleton<MainPage>();
		builder.Services.AddSingleton<MainViewModel>();

		builder.Services.AddTransient<SigninPage>();
        builder.Services.AddTransient<SignInViewModel>();

		builder.Services.AddTransient<SignoutPage>();
        builder.Services.AddTransient<SignOutViewModel>();

        builder.Services.AddSingleton<IStaffService, StaffService>();
		builder.Services.AddSingleton<IVisitService, VisitService>();

        builder.Services.AddHttpClient(Constants.ApiClientName, client => client.BaseAddress = new Uri(Constants.BaseUrl));

        return builder.Build();
	}
}
