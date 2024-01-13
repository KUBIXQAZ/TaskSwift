using Newtonsoft.Json;
using TaskSwift.Models;
using TaskSwift.Views;

namespace TaskSwift;

public partial class App : Application
{
    public static List<Views.Task> tasks = new List<Views.Task>();
    public static List<Views.Task> completedTasks = new List<Views.Task>();

    public static List<FlagModel> flags = new List<FlagModel>();

    public static StatsModel stats = new StatsModel();

    string tasksFileName = "Tasks.json";
    public static string tasksFilePath;
    string statsFileName = "Stats.json";
    public static string statsFilePath;
    string completedTasksFileName = "CompletedTasks.json";
    public static string completedTasksPath;
    string flagsFileName = "Flags.json";
    public static string flagsFilePath;

    public App()
	{
		InitializeComponent();

        tasksFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), tasksFileName);
        statsFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), statsFileName);
        completedTasksPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), completedTasksFileName);
        flagsFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), flagsFileName);

        //ResetData();

        LoadStats();
        LoadJson();
        FlagModel.LoadFlags();

        MainPage = new AppShell();
    }

    private void ResetData()
    {
        //File.Delete(tasksFilePath);
        //File.Delete(statsFilePath);
        File.Delete(completedTasksPath);
        //File.Delete(flagsFilePath);
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
        if (File.Exists(completedTasksPath))
        {
            string json = File.ReadAllText(completedTasksPath);
            completedTasks = JsonConvert.DeserializeObject<List<Views.Task>>(json);
        }
    }
}