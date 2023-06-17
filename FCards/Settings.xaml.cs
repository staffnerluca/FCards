namespace FCards;

public partial class Settings : ContentPage
{
	public Settings()
	{
		InitializeComponent();
	}

    private void btnSavaChanges_Clicked(object sender, EventArgs e)
    {
		if(int.TryParse(eHard.Text, out int hard))
			SQLCom.hard = hard;
		if(int.TryParse(eIntermediate.Text, out int intermediate))
			SQLCom.intermediate = intermediate;
		if(int.TryParse(eEasy.Text, out int easy))
			SQLCom.easy = easy;
    }

	private void btnStudyPressed(object sender, EventArgs e)
	{
        App.Current.MainPage = new NavigationPage(new MainPage());
    }
}