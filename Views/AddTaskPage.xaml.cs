using Microsoft.Maui.Layouts;
using Newtonsoft.Json;
using Microsoft.Maui.Graphics;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Threading.Tasks;

namespace TaskSwift.Views;

public partial class AddTaskPage : ContentPage
{
	public AddTaskPage()
	{
		InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        TaskDate.MinimumDate = DateTime.Now;
        TaskTime.Time = DateTime.Now.TimeOfDay;
        TaskDate.Date = DateTime.Now;

        DeadlineCheckbox.IsChecked = false; 
        TimeCheckbox.IsChecked = false;

        DeadlineCheckbox_CheckedChanged(null,null);
        TimeCheckbox_CheckedChanged(null, null);

        Title.Text = string.Empty;

        Device.StartTimer(TimeSpan.FromMilliseconds(1), () =>
        {
            DateTime taskDate = TaskDate.Date;
            DateTime currDate = DateTime.Now.Date;
            if (taskDate == currDate)
            {
                TimeSpan taskTime = TaskTime.Time;
                TimeSpan currTime = DateTime.Now.TimeOfDay;
                if (taskTime < currTime)
                {
                    TaskTime.Time = DateTime.Now.TimeOfDay;
                }
            }
            return true;
        });
    }

    public Frame GenerateElementWithoutDeadline(Task task, string title)
    {
        Color bgColor = Color.FromRgb(77, 77, 77);
        Color titleColor = Colors.White;

        Frame frame = new Frame
        {
            BackgroundColor = bgColor,
            CornerRadius = 15,
            BorderColor = Colors.Transparent,
            Margin = new Thickness(8, 3, 8, 0),
            HeightRequest = 50,
            Padding = 0
        };

        AbsoluteLayout absoluteLayout = new AbsoluteLayout();

        RadioButton radioButton = new RadioButton
        {
            Margin = new Thickness(6, 5, 0, 0)
        };
        radioButton.CheckedChanged += (sender, e) =>
        {
            Data.tasks.Remove(task);
            MainPage.tasksContainer.Children.Remove(frame);

            SaveTask();

            var currentShellItem = Shell.Current.CurrentPage;

            Data.stats.tasksDone++;
            Data.stats.tasksPending = Data.tasks.Count;

            SaveStats();

            if (currentShellItem is ProfilePage profilePage)
            {
                profilePage.displayCurrent();
                profilePage.DisplayStats();
            }

            MainPage.DisplayWhenNoTasks();
        };

        Button button = new Button
        {
            BackgroundColor = Colors.Transparent,
        };
        AbsoluteLayout.SetLayoutBounds(button, new Rect(0, 0, 1, 1));
        AbsoluteLayout.SetLayoutFlags(button, AbsoluteLayoutFlags.All);
        button.Clicked += (sender, e) =>
        {
            ViewTask();
        };

        if (title.Length > 29) title = title.Substring(0, 29) + "...";

        Label titleLabel = new Label
        {
            Margin = new Thickness(40, 10, 0, 0),
            Text = title,
            TextColor = titleColor,
            FontSize = 20
        };

        frame.Content = absoluteLayout;

        absoluteLayout.Children.Add(titleLabel);
        absoluteLayout.Children.Add(button);
        absoluteLayout.Children.Add(radioButton);

        return frame;
    }

    public Frame GenerateElementWithDeadline(Task task, DateTime date, string title)
    {
        string daysLeft = Date.GetDaysLeft(date);
        Color color = Colors.White;
        Color bgColor = Color.FromRgb(77, 77, 77);
        Color titleColor = Colors.White;

        if (daysLeft == "Overdue.")
        {
            color = Color.FromRgb(163, 15, 15);
            titleColor = Color.FromRgb(179, 179, 179);
            bgColor = Color.FromRgb(64, 54, 54);
        }

        Frame frame = new Frame
        {
            BackgroundColor = bgColor,
            CornerRadius = 15,
            BorderColor = Colors.Transparent,
            Margin = new Thickness(8, 3, 8, 0),
            HeightRequest = 78,
            Padding = 0
        };

        AbsoluteLayout absoluteLayout = new AbsoluteLayout();

        RadioButton radioButton = new RadioButton
        {
            Margin = new Thickness(6, 5, 0, 0)
        };
        radioButton.CheckedChanged += (sender, e) =>
        {
            destroy(this, frame, task, date);
        };

        Button button = new Button
        {
            BackgroundColor = Colors.Transparent,
        };
        AbsoluteLayout.SetLayoutBounds(button, new Rect(0, 0, 1, 1));
        AbsoluteLayout.SetLayoutFlags(button, AbsoluteLayoutFlags.All);
        button.Clicked += (sender, e) =>
        {
            ViewTask();
        };

        if (title.Length > 29) title = title.Substring(0, 29) + "...";

        Label titleLabel = new Label
        {
            Margin = new Thickness(40, 10, 0, 0),
            Text = title,
            TextColor = titleColor,
            FontSize = 20
        };

        Label dueLabel = new Label();
        dueLabel = new Label
        {
            Margin = new Thickness(40, 40, 0, 0),
            Text = daysLeft,
            TextColor = color,
            FontSize = 20
        };

        frame.Content = absoluteLayout;

        absoluteLayout.Children.Add(titleLabel);
        absoluteLayout.Children.Add(dueLabel);
        absoluteLayout.Children.Add(button);
        absoluteLayout.Children.Add(radioButton);

        return frame;
    }

    public Frame DisplayTasks(Task task)
    {
        if (task.withDeadline) return GenerateElementWithDeadline(task, task.date.Value, task.title);
        else return GenerateElementWithoutDeadline(task, task.title);
    }

    public void GenerateTask(bool withDeadline, string title, DateTime? date)
    {
        Task task;

        if (withDeadline)
        {
            task = Data.createTask(title, date, withDeadline);
        }
        else
        {
            task = Data.createTask(title, null, withDeadline);
        }

        Data.tasks.Add(task);
        SaveTask();
    }


    public void SaveTask()
    {
        string json = JsonConvert.SerializeObject(Data.tasks);

        File.WriteAllText(jsonSettings.getTasksStorageFilePath(), json);
    }
    
    public void destroy(object sender, Frame frame, Task task, DateTime date)
    {
        Data.tasks.Remove(task);
        MainPage.tasksContainer.Children.Remove(frame);

        SaveTask();

        var currentShellItem = Shell.Current.CurrentPage;

        if (Date.GetOverdue(date)) Data.stats.tasksDoneOverdue++;
        else Data.stats.tasksDone++;

        Data.stats.tasksPending = Data.tasks.Count;
        SaveStats();

        if (currentShellItem is ProfilePage profilePage)
        {
            Console.WriteLine("bongbong " + Data.stats.tasksDone.ToString() + " " + Data.stats.tasksDoneOverdue.ToString() + " " + Data.stats.tasksPending.ToString());
            profilePage.DisplayStats();
            profilePage.displayCurrent();
        }

        MainPage.DisplayWhenNoTasks();
    }

    public void SaveStats()
    {
        string json = JsonConvert.SerializeObject(Data.stats);

        File.WriteAllText(jsonSettings.getStatsFileNamePath(), json);
    }

    public void ViewTask()
    {
        
    }

    public async void AddTaskButton(object sender, EventArgs e)
    {
        string title = Title.Text;

        DateTime selectedDate = TaskDate.Date;
        TimeSpan selectedTime;

        if (TimeCheckbox.IsChecked == true) selectedTime = TaskTime.Time;
        else selectedTime = new TimeSpan(24, 0, 0);

        DateTime combinedDateTime = selectedDate.Add(selectedTime);

        bool withDeadline = DeadlineCheckbox.IsChecked;

        GenerateTask(withDeadline, title, combinedDateTime);

        Title.Text = string.Empty;

        Data.stats.tasksPending = Data.tasks.Count;
        SaveStats();

        await Shell.Current.GoToAsync("//MainPage");
    }

    private void TimeCheckbox_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (TimeCheckbox.IsChecked == true) TaskTime.IsEnabled = true;
        else TaskTime.IsEnabled = false;
    }

    public void CheckEntry(object sender, EventArgs e)
    {
        int i = 0;
        string title = Title.Text;
        Button submit = Submit;
        i = title.Replace(" ", "").Length;

        if (i <= 0) submit.IsEnabled = false;
        else submit.IsEnabled = true;
    }

    private void DeadlineCheckbox_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (DeadlineCheckbox.IsChecked == true)
        {
            TaskDate.IsEnabled = true;
            TaskTime.IsEnabled = false;
            TimeCheckbox.IsEnabled = true;
            TimeCheckbox.IsChecked = false;
        }
        else
        {
            TaskDate.IsEnabled = false;
            TaskTime.IsEnabled = false;
            TimeCheckbox.IsEnabled = false;
            TimeCheckbox.IsChecked = false;
        }
    }
}