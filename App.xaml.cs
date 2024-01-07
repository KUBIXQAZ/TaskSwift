using Newtonsoft.Json;

namespace TaskSwift;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

        LoadStats();
        LoadJson();

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

    private void LoadJson()
    {
        if (File.Exists(jsonSettings.getTasksStorageFilePath()))
        {
            string json = File.ReadAllText(jsonSettings.getTasksStorageFilePath());
            Data.tasks = JsonConvert.DeserializeObject<List<Views.Task>>(json);
        }
        Console.WriteLine("asdas");
    }
}