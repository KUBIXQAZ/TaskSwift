using Newtonsoft.Json;

namespace TaskSwift;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

        LoadStats();

        MainPage = new AppShell();
    }

    public void LoadStats()
    {
        if (File.Exists(jsonSettings.getStatsFileNamePath()))
        {
            string json = File.ReadAllText(jsonSettings.getStatsFileNamePath());
            Data.stats = JsonConvert.DeserializeObject<Stats>(json);
        }
    }
}