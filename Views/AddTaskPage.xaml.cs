using Microsoft.Maui;
using Microsoft.Maui.Layouts;
using Microsoft.Maui.Platform;
using Newtonsoft.Json;

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

    public Frame GenerateTask(string title, DateTime date, bool noDeadline, bool display, Task task)
    {
        string daysLeft = Date.GetDaysLeft(date);
        Color color = Colors.White;
        Color bgColor = Color.FromRgb(77, 77, 77);
        Color titleColor = Colors.White;

        if (!display) task = Data.createTask(title, date, noDeadline);

        int heightRequest;
        if (!noDeadline)
        {
            heightRequest = 78;
            if (daysLeft == "Overdue.")
            {
                color = Color.FromRgb(163, 15, 15);
                titleColor = Color.FromRgb(179, 179, 179);
                bgColor = Color.FromRgb(64, 54, 54);
            }
        }
        else heightRequest = 50;

        Frame frame = new Frame
        {
            BackgroundColor = bgColor,
            CornerRadius = 15,
            BorderColor = Colors.Transparent,
            Margin = new Thickness(8, 3, 8, 0),
            HeightRequest = heightRequest,
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

        if(title.Length > 32) title = title.Substring(0,32) + "...";

        Label titleLabel = new Label
        {
            Margin = new Thickness(40, 10, 0, 0),
            Text = title,
            TextColor = titleColor,
            FontSize = 20
        };

        Label dueLabel = new Label();
        if (!noDeadline)
        {
            dueLabel = new Label
            {
                Margin = new Thickness(40, 40, 0, 0),
                Text = daysLeft,
                TextColor = color,
                FontSize = 20
            };
        }

        frame.Content = absoluteLayout;

        absoluteLayout.Children.Add(titleLabel);
        if (!noDeadline) absoluteLayout.Children.Add(dueLabel);
        absoluteLayout.Children.Add(button);
        absoluteLayout.Children.Add(radioButton);

        if (!display)
        {
            Data.tasks.Add(task);
            SaveTask();
        }
        return frame;
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

        if(currentShellItem is ProfilePage profilePage) {
            profilePage.displayCurrent();
        }

        if (Date.GetOverdue(date)) Data.stats.tasksDoneOverdue++;
        else Data.stats.tasksDone++;

        Data.stats.tasksPending = Data.tasks.Count;
        SaveStats();

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

        bool noDeadline = !DeadlineCheckbox.IsChecked;

        GenerateTask(title, combinedDateTime, noDeadline, false, null);

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