using GraphVisitor.UI.ViewModels;

namespace GraphVisitor.UI;

public partial class MainPage : ContentPage
{
    public MainViewModel ViewModel { get; set; }
	
    public MainPage(MainViewModel viewModel)
	{
		InitializeComponent();
		ViewModel = viewModel;
        ViewModel.Navigation = Navigation;
        BindingContext = ViewModel;
    }
}

