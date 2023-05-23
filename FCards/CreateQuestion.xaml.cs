namespace FCards;

public partial class CreateQuestion : ContentPage
{
	SQLCom sql = new SQLCom();
	public CreateQuestion()
	{
		InitializeComponent();
	}

	public void StudyPressed(Object sender, EventArgs e)
	{
        App.Current.MainPage = new NavigationPage(new MainPage());
    }

	public void SavedPressed(Object sender, EventArgs e) {
		sql.createCard(eQuestion.Text, eAnswer.Text);
	}
}