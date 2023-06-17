namespace FCards;

public partial class MainPage : ContentPage
{
	int currentId = 0;
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
		string noCardsText = "There are no cards";
        if (eQuestion.Text.Equals("") || eQuestion.Text.Equals(noCardsText))
		{
            List<String> list = sql.getAllCards();
            int numberOfCards = list.Count / 3;
            Random r = new Random();
            currentId = r.Next(numberOfCards) * 3;
            if (!(list.Count == 0))
            {
				try
				{
                    lblQuestionID.Text = list[currentId - 3];
                    eQuestion.Text = list[currentId - 2];
					//eAnswer.Text = list[id-1]; //only for tests;
				}
				catch(Exception ex) {}

            }
            else
            {
                eQuestion.Text = noCardsText;
            }
        }
	}

	public void btnDifPressed(Object sender, EventArgs e)
	{
		Button b = sender as Button;
		sql.updateDueDate(b.Text.ToLower(), int.Parse(lblQuestionID.Text));
		eQuestion.Text = "";
		eAnswer.Text = "";
		lblQuestionID.Text = "";
		studyPressed(sender, e);

	}

    private void btnShowAnswerClicked(object sender, EventArgs e)
    {
        List<String> list = sql.getAllCards();
		if(list.Count > 0)
		{
			eAnswer.Text = list[currentId-1];
		}
    }
}

