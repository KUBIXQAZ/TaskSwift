using Newtonsoft.Json;

namespace TaskSwift.Views;

public partial class MainPage : ContentPage
{
    public static StackLayout tasksContainer;

    public MainPage()
	{
		InitializeComponent();

        tasksContainer = TasksContainer;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        LoadJson();
        displayTasks();
    }

    private void LoadJson()
    {
        if (File.Exists(jsonSettings.getTasksStorageFilePath()))
        {
            string json = File.ReadAllText(jsonSettings.getTasksStorageFilePath());
            Data.tasks = JsonConvert.DeserializeObject<List<Task>>(json);
        }
    }

    public void displayTasks()
    {
        tasksContainer.Children.Clear();

        int tasksCount = Data.tasks.Count;
        
        for (int i = 0; i < tasksCount; i++)
        {
            string taskTitle = Data.tasks[i].Title();
            DateTime date = Data.tasks[i].date;
            bool noDeadline = Data.tasks[i].noDeadline;
            
            AddTaskPage at = new AddTaskPage();
            tasksContainer.Children.Add(at.GenerateTask(taskTitle, date, noDeadline, true, Data.tasks[i]));
        }
    }
}