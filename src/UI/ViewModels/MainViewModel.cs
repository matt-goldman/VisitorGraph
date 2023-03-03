using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GraphVisitor.UI.Pages;
using Maui.Plugins.PageResolver;

namespace GraphVisitor.UI.ViewModels;

public partial class MainViewModel : ObservableObject
{
    public INavigation Navigation { get; set; }
    
    public MainViewModel()
    {
    }

    [RelayCommand]
    public async Task SignIn()
    {
        await Navigation.PushAsync<SigninPage>();
    }

    [RelayCommand]
    public async Task SignOut()
    {
        await Navigation.PushAsync<SignoutPage>();
    }
}
