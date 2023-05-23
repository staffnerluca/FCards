namespace FCards;

public partial class MainPage : ContentPage
{
	public SQLCom sql = new SQLCom();
	public MainPage()
	{
		InitializeComponent();
		sql.createDatabaseIfNotExists();
		sql.createExamples();
	}

	public void createNewPressed(Object sender, EventArgs e)
	{
		App.Current.MainPage = new NavigationPage(new CreateQuestion());
	}

	public void studyPressed(Object sender, EventArgs e)
	{
		List<String> list = sql.getRandomCard();
		lblQuestionID.Text = list[0];
		eQuestion.Text = list[1];
		eAnswer.Text = list[2];
	}

}

