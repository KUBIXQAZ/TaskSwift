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
        DisplayWhenNoTasks();
    }

    private void LoadJson()
    {
        if (File.Exists(jsonSettings.getTasksStorageFilePath()))
        {
            string json = File.ReadAllText(jsonSettings.getTasksStorageFilePath());
            Data.tasks = JsonConvert.DeserializeObject<List<Task>>(json);
        }
    }

    public static void DisplayWhenNoTasks()
    {
        if (Data.tasks.Count == 0)
        {
            var noTasksLabel = new Label
            {
                Text = "You have nothing planned yet.",
                FontSize = 16,
                TextColor = Color.FromHex("#C0C0C0"),
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand
            };

            tasksContainer.Children.Add(noTasksLabel);
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