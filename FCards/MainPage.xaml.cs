namespace FCards;

public partial class MainPage : ContentPage
{
	public SQLCom sql = new SQLCom();
	public MainPage()
	{
		InitializeComponent();
		sql.createDatabaseIfNotExists();
		sql.connectToDB();
		//sql.createExamples();
		showAllCards();
	}

	public void createNewPressed(Object sender, EventArgs e)
	{
		App.Current.MainPage = new NavigationPage(new CreateQuestion());
	}

	public void studyPressed(Object sender, EventArgs e)
	{
		List<String> list = sql.getRandomCard();
		if(!(list.Count == 0))
		{
            lblQuestionID.Text = list[0];
            eQuestion.Text = list[1];
            eAnswer.Text = list[2];
        }
		else
		{
			eQuestion.Text = "There are no cards";
		}
	}
	public async void showAllCards()
	{
		List<string> cards = sql.getAllCards();
		if(cards.Count == 0)
		{
			await DisplayAlert("Message", "Where are the cars?", "Ok");
		}
		foreach(string card in cards)
		{
			await DisplayAlert("Message", card, "Ok");
		}
	}

}

