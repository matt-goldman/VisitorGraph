using GraphVisitor.UI.ViewModels;

namespace GraphVisitor.UI.Pages;

public partial class SigninPage : ContentPage
{
    public SignInViewModel ViewModel { get; set; }
	
    public SigninPage(SignInViewModel viewModel)
	{
		InitializeComponent();
        ViewModel = viewModel;
        ViewModel.Navigation = Navigation;
        BindingContext = ViewModel;
    }
}