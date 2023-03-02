using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GraphVisitor.Common.DTOs;
using GraphVisitor.UI.Services;
using System.Collections.ObjectModel;

namespace GraphVisitor.UI.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly IStaffService _staffService;

    public ObservableCollection<StaffDto> SearchResults { get; set; } = new();

    [ObservableProperty]
    private StaffDto? selectedStaff;

    [ObservableProperty]
    private bool isBusy = false;

    [ObservableProperty]
    private string searchTerm;
    
    public MainViewModel(IStaffService staffService)
    {
        _staffService = staffService;
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
}
