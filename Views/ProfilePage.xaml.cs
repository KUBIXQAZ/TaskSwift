using Newtonsoft.Json;

namespace TaskSwift.Views;

public partial class ProfilePage : ContentPage
{
	public ProfilePage()
	{
		InitializeComponent();

        LoadStats();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        doneText.Text = Data.stats.tasksDone.ToString();
        doneOverdueText.Text = Data.stats.tasksDoneOverdue.ToString();
        pendingText.Text = Data.stats.tasksPending.ToString();
    }

    public void LoadStats()
    {
        if(File.Exists(jsonSettings.getStatsFileNamePath()))
        {
            string json = File.ReadAllText(jsonSettings.getStatsFileNamePath());
            Data.stats = JsonConvert.DeserializeObject<Stats>(json);
        }
    }

    public void ResetStats()
    {
        File.Delete(jsonSettings.getStatsFileNamePath());

        Data.stats.tasksDone = 0;
        Data.stats.tasksDoneOverdue = 0;
        Data.stats.tasksPending = Data.tasks.Count;

        doneText.Text = Data.stats.tasksDone.ToString();
        doneOverdueText.Text = Data.stats.tasksDoneOverdue.ToString();
        pendingText.Text = Data.stats.tasksPending.ToString();
    }
}