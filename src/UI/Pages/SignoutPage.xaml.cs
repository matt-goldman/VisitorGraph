using GraphVisitor.UI.ViewModels;

namespace GraphVisitor.UI.Pages;

public partial class SignoutPage : ContentPage
{
	public SignOutViewModel ViewModel  { get; set; }
	
	public SignoutPage(SignOutViewModel viewModel)
	{
		InitializeComponent();
		ViewModel = viewModel;
		ViewModel.Navigation = Navigation;
		BindingContext = ViewModel;
	}
}