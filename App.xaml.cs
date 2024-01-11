using Newtonsoft.Json;
using System.IO;
using TaskSwift.Models;

namespace TaskSwift;

public partial class App : Application
{
    public static List<Views.Task> tasks = new List<Views.Task>();

    public static StatsModel stats = new StatsModel();

    string tasksFileName = "TaskSwift.json";
    public static string tasksFilePath;
    string statsFileName = "Stats.json";
    public static string statsFilePath;

    public App()
	{
		InitializeComponent();

        tasksFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), tasksFileName);
        statsFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), statsFileName);

        LoadStats();
        LoadJson();

        MainPage = new AppShell();
    }

    public void LoadStats()
    {
        if (File.Exists(statsFilePath))
        {
            string json = File.ReadAllText(statsFilePath);
            stats = JsonConvert.DeserializeObject<StatsModel>(json);
        }
    }

    private void LoadJson()
    {
        if (File.Exists(tasksFilePath))
        {
            string json = File.ReadAllText(tasksFilePath);
            tasks = JsonConvert.DeserializeObject<List<Views.Task>>(json);
        }
    }
}