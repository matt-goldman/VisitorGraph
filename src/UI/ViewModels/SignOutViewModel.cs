using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GraphVisitor.Common.DTOs;
using GraphVisitor.UI.Popups;
using GraphVisitor.UI.Services;

namespace GraphVisitor.UI.ViewModels;

public partial class SignOutViewModel : ObservableObject
{
    private readonly IVisitService _visitService;

    public INavigation Navigation { get; set; }

    public SignOutViewModel(IVisitService visitService)
    {
        _visitService = visitService;
        IsBusy = false;
    }

    [ObservableProperty]
    private string visitorEmail;

    [ObservableProperty]
    private bool isBusy;

    [RelayCommand]
    public async Task SignOut()
    {
        IsBusy = true;
        
        var dto = new SignOutDto
        {
            VisitorEmail = this.VisitorEmail
        };

        var signedOut = await _visitService.SignOut(dto);

        string title;
        string body;

        switch (signedOut)
        {
            case SignoutStatus.Success:
                title = "Signed Out";
                body = "Thanks for visiting!";
                break;
            case SignoutStatus.Error:
                title = "Error";
                body = "There was an error signing you out.";
                break;
            case SignoutStatus.NotFound:
                title = "No sign in found";
                body = "Looks like you forgot to sign in when you arrived.";
                break;
            default:
                title = "Error";
                body = "An unknown error has occured.";
                break;
        }

        IsBusy = false;
        
        var popup = new Dialog(title, body);
        var closed = await App.Current.MainPage.ShowPopupAsync(popup);
        await Navigation.PopAsync();        
    }
}
