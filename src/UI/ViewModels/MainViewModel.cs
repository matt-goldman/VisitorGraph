using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GraphVisitor.Common.DTOs;
using GraphVisitor.UI.Services;
using System.Collections.ObjectModel;

namespace GraphVisitor.UI.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly IStaffService _staffService;
    private readonly IVisitService _visitService;

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

    public MainViewModel(IStaffService staffService, IVisitService visitService)
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

        try
        {
            var dto = new SignInDto
            {
                StaffId = SelectedStaff!.StaffId,
                VisitorName = VisitorName,
                VisitorEmail = VisitorEmail
            };

            var success = await _visitService.SignIn(dto);

            // TODO: replace these with MCT popups
            if (success)
            {
                await App.Current.MainPage.DisplayAlert("Signed in successfully", $"{SelectedStaff.DisplayName} has been informed and knows you are waiting.", "OK");
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Error", "Failed to sign in", "OK");
            }
        }
        catch (Exception)
        {
            await App.Current.MainPage.DisplayAlert("Error", "Failed to sign in", "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }
}
