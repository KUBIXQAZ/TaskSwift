using Newtonsoft.Json;
using System.Runtime.CompilerServices;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TaskSwift.Views;

public partial class ProfilePage : ContentPage
{
	public ProfilePage()
	{
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        DisplayStats();
        displayCurrent();
    }

    public void DisplayStats()
    {
        doneText.Text = Data.stats.tasksDone.ToString();
        doneOverdueText.Text = Data.stats.tasksDoneOverdue.ToString();
        pendingText.Text = Data.stats.tasksPending.ToString();
    }

    LinearGradientBrush gradient = new LinearGradientBrush
    {
        GradientStops =
                {
                    new GradientStop(Color.FromHex("#66B4FF"), 0),
                    new GradientStop(Color.FromHex("#428bff"), 0.5f),
                    new GradientStop(Color.FromHex("#66B4FF"), 1),
                }
    };
    int max = 3;
    public void displayCurrent()
    {
        StackLayoutCurrTask.Children.Clear();
        currectTaskText.Children.Clear();

        List<Task> tasksWithDeadline = tasksWithDeadline = new List<Task>();
        for (int i = 0; i < Data.tasks.Count; i++)
        {
            if (Data.tasks[i].withDeadline)
            {
                tasksWithDeadline.Add(Data.tasks[i]);
            }
        }

        List<Task> currTasks = new List<Task>();
        Task currTask = null;
        DateTime now = Date.GetDate();

        if(Data.tasks.Count != 0)
        {
            Label sectionTitle = new Label
            {
                Text = "Current Task",
                TextColor = Color.FromHex("#C0C0C0"),
                FontSize = 20,
                Margin = new Thickness(0, 10, 0, 5)
            };

            currectTaskText.Children.Add(sectionTitle);
        }

        foreach (Task task in tasksWithDeadline)
        {
            TimeSpan timeLeft = Date.GetTimeLeft(task.date.Value, now);
            TimeSpan currTaskLeft = new TimeSpan();

            try { currTaskLeft = Date.GetTimeLeft(currTask.date.Value, now); }
            catch (Exception ex) { }

            if (currTask == null || timeLeft < currTaskLeft)
            {
                currTasks.Clear();
                currTask = task;
                currTasks.Add(task);
            }
            else if (timeLeft == currTaskLeft)
            {
                currTasks.Add(task);
            }
        }

        if(tasksWithDeadline.Count == 0)
        {
            if(Data.tasks.Count > 0)
            {
                currTasks = Data.tasks;
            }
        }

        int taskNum = 0;
        foreach (Task task in currTasks)
        {
            if (taskNum >= max) break;

            AddTaskPage at = new AddTaskPage();
            StackLayoutCurrTask.Children.Add(at.DisplayTasks(task));
            taskNum++;
        }

        if (Data.tasks.Count != 0)
        {
            var displayMoreButton = new Button
            {
                Text = "Show more.",
                Background = gradient,
                TextColor = Color.FromHex("#121212"),
                FontSize = 16,
                FontAttributes = FontAttributes.Bold,
                WidthRequest = 120,
                HeightRequest = 30,
                Padding = 0,
                Margin = 8
            };
            displayMoreButton.Clicked += (sender, e) =>
            {
                if (currTasks.Count > taskNum) max += 3;
                displayCurrent();
            };
            var displayLessButton = new Button
            {
                Text = "Show less.",
                Background = gradient,
                TextColor = Color.FromHex("#121212"),
                FontSize = 16,
                FontAttributes = FontAttributes.Bold,
                WidthRequest = 120,
                HeightRequest = 30,
                Padding = 0,
                Margin = 8
            };
            displayLessButton.Clicked += (sender, e) =>
            {
                max -= 3;
                if (max < 3) max = 3;
                displayCurrent();
            };

            StackLayout stackLayout = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.CenterAndExpand
            };

            if (taskNum <= 3) stackLayout.Children.Remove(displayLessButton);
            else if (taskNum > 3) stackLayout.Children.Add(displayLessButton);

            if (currTasks.Count > taskNum) stackLayout.Children.Add(displayMoreButton);
            else if (currTasks.Count == taskNum) stackLayout.Children.Remove(displayMoreButton);

            StackLayoutCurrTask.Children.Add(stackLayout);
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