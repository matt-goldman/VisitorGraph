using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GraphVisitor.Common.DTOs;
using GraphVisitor.UI.Popups;
using GraphVisitor.UI.Services;
using System.Collections.ObjectModel;

namespace GraphVisitor.UI.ViewModels;

public partial class SignInViewModel : ObservableObject
{
    private readonly IStaffService _staffService;
    private readonly IVisitService _visitService;
    public INavigation Navigation { get; set; }

    public ObservableCollection<StaffDto> SearchResults { get; set; } = new();

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(FormIsValid))]
    private StaffDto? selectedStaff;

    [ObservableProperty]
    private bool isBusy = false;

    [ObservableProperty]
    private string searchTerm;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(FormIsValid))]
    private string visitorName;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(FormIsValid))]
    private string visitorEmail;

    public bool FormIsValid => !string.IsNullOrWhiteSpace(VisitorName) && !string.IsNullOrWhiteSpace(VisitorEmail) && SelectedStaff != null;

    public SignInViewModel(IStaffService staffService, IVisitService visitService)
    {
        _staffService = staffService;
        _visitService = visitService;
    }

    [RelayCommand]
    public async Task SearchStaff()
    {
        IsBusy = true;

        SearchResults.Clear();

        var staff = await _staffService.Search(SearchTerm);

        foreach (var staffMember in staff)
        {
            SearchResults.Add(staffMember);
        }

        IsBusy = false;
    }

    [RelayCommand]
    public async Task SignIn()
    {
        IsBusy = true;

        string title = string.Empty;
        string body = string.Empty;

        try
        {
            var dto = new SignInDto
            {
                StaffId = SelectedStaff!.StaffId,
                VisitorName = VisitorName,
                VisitorEmail = VisitorEmail
            };

            var success = await _visitService.SignIn(dto);

            if (success)
            {
                title = "Signed in successfully";
                body = $"{SelectedStaff.DisplayName} has been informed and knows you are waiting.";
            }
            else
            {
                title = "Error";
                body = "Failed to sign in";
            }
        }
        catch (Exception)
        {
            title = "Error";
            body = "Failed to sign in";
        }
        finally
        {
            ResetForm();
            IsBusy = false;

            var popup = new Dialog(title, body);
            var closed = await App.Current.MainPage.ShowPopupAsync(popup);
            await Navigation.PopAsync();
        }
    }

    private void ResetForm()
    {
        SelectedStaff = null;
        SearchResults.Clear();
        VisitorName = string.Empty;
        VisitorEmail = string.Empty;
    }
}
