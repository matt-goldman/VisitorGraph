using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GraphVisitor.Common.DTOs;
using GraphVisitor.UI.Services;

namespace GraphVisitor.UI.ViewModels;

public partial class SignOutViewModel : ObservableObject
{
    private readonly IVisitService _visitService;

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

        await _visitService.SignOut(dto);
    }
}
