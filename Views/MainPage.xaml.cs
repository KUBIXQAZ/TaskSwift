using Newtonsoft.Json;

namespace TaskSwift.Views;

public partial class MainPage : ContentPage
{
    public static StackLayout tasksContainer;

    public MainPage()
	{
		InitializeComponent();

        tasksContainer = TasksContainer;

        LoadJson();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

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
            var stack = new StackLayout
            {
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand
            };

            var gradient = new LinearGradientBrush
            {
                GradientStops =
                {
                    new GradientStop(Color.FromHex("#66B4FF"), 0),
                    new GradientStop(Color.FromHex("#428bff"), 0.5f),
                    new GradientStop(Color.FromHex("#66B4FF"), 1),
                }
            };

            var label = new Label
            {
                Text = "You have nothing planned yet.",
                FontSize = 16,
                TextColor = Color.FromHex("#66B4FF")
            };

            var button = new Button
            {
                Text = "Add task",
                Background = gradient,
                TextColor = Color.FromHex("#121212"),
                FontSize = 16,
                FontAttributes = FontAttributes.Bold,
                WidthRequest = 120,
                HeightRequest = 40,
                Padding = 0,
                Margin = 4
            };

            button.Clicked += async (sender, e) =>
            {
                await Shell.Current.GoToAsync("//AddTaskPage");
            };

            stack.Children.Add(label);
            stack.Children.Add(button);

            tasksContainer.Children.Add(stack);
        }
    }

    public void displayTasks()
    {
        tasksContainer.Children.Clear();

        int tasksCount = Data.tasks.Count;

        for (int i = 0; i < tasksCount; i++)
        {
            Task task = Data.tasks[i]; 

            AddTaskPage at = new AddTaskPage();
            tasksContainer.Children.Add(at.DisplayTasks(task));
        }
    }
}