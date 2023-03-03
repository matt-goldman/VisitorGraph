namespace GraphVisitor.UI;

public partial class App : Application
{
	public App(MainPage mainPage)
	{
		InitializeComponent();

        MainPage = new NavigationPage(mainPage);
    }

    protected override Window CreateWindow(IActivationState activationState)
    {
        var window = base.CreateWindow(activationState);

#if WINDOWS || MACCATALYST
        window.Title = "VisitorGraph";
        window.Width = 1000;
        window.Height = 800;
#endif
        return window;
    }
}
