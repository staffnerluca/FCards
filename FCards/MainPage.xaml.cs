namespace FCards;

public partial class MainPage : ContentPage
{
	public SQLCom sql = new SQLCom();
	public MainPage()
	{
		InitializeComponent();
		sql.createDatabaseIfNotExists();
		sql.connectToDB();
		sql.createExamples();
	}

	public void createNewPressed(Object sender, EventArgs e)
	{
		App.Current.MainPage = new NavigationPage(new CreateQuestion());
	}

	public void studyPressed(Object sender, EventArgs e)
	{
		List<String> list = sql.getAllCards();
		int numberOfCards=list.Count/3;
		Random r = new Random();
		int id = r.Next(numberOfCards)*3;
		if(!(list.Count == 0))
		{
            lblQuestionID.Text = list[id-3];
            eQuestion.Text = list[id-2];
            eAnswer.Text = list[id-1];
        }
		else
		{
			eQuestion.Text = "There are no cards";
		}
	}
	public void btnDiffPressed(Object sender, EventArgs e)
	{
		Button b = sender as Button;
		sql.updateDueDate(b.Text, int.Parse(lblQuestionID.Text));
		studyPressed(sender, e);
	}
}

