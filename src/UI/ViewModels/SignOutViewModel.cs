using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GraphVisitor.Common.DTOs;
using GraphVisitor.UI.Services;

namespace GraphVisitor.UI.ViewModels;

public partial class SignOutViewModel : ObservableObject
{
    private readonly IVisitService _visitService;

    public INavigation Navigation { get; set; }

    public SignOutViewModel(IVisitService visitService)
    {
        _visitService = visitService;
    }

    [ObservableProperty]
    private string visitorEmail;

    [RelayCommand]
    public async Task SignOut()
    {
        var dto = new SignOutDto
        {
            VisitorEmail = this.VisitorEmail
        };

        var signedOut = await _visitService.SignOut(dto);

        switch (signedOut)
        {
            case SignoutStatus.Success:
                await App.Current.MainPage.DisplayAlert("Signed Out", "Thanks for visiting!", "OK");
                await Navigation.PopAsync();
                break;
            case SignoutStatus.Error:
                await App.Current.MainPage.DisplayAlert("Error", "There was an error signing you out.", "OK");
                await Navigation.PopAsync();
                break;
            case SignoutStatus.NotFound:
                await App.Current.MainPage.DisplayAlert("No sign in found", "Looks like you forgot to sign in when you arrived.", "OK");
                await Navigation.PopAsync();
                break;
        }
    }
}
