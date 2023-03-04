using CommunityToolkit.Maui.Views;

namespace GraphVisitor.UI.Popups;

public partial class Dialog : Popup
{
	public Dialog(string title, string body)
	{
		InitializeComponent();
		TitleLabel.Text = title;
        BodyLabel.Text = body;
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        Close();
    }
}