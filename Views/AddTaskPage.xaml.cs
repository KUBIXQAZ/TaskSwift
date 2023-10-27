using Microsoft.Maui.Layouts;
using Newtonsoft.Json;

namespace TaskSwift.Views;

public partial class AddTaskPage : ContentPage
{
	public AddTaskPage()
	{
		InitializeComponent();

        TaskDate.MinimumDate = DateTime.Now;
        TaskTime.Time = DateTime.Now.TimeOfDay;
    }

    public Frame GenerateTask(string title, DateTime date, bool noDeadline, bool display, Task taskJson)
    {
        if (display == true)
        {
            Frame frame = new Frame
            {
                BackgroundColor = Color.FromHex("#4D4D4D"),
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
                destroy(this, frame, taskJson);
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

            string daysLeft = Date.GetDaysLeft(date);

            string labelText;
            if (noDeadline == true) labelText = title;
            else labelText = title + " " + daysLeft;
            
            Label label = new Label
            {
                Margin = new Thickness(40, 10, 0, 0),
                Text = labelText,
                TextColor = Colors.White,
                FontSize = 20
            };
            
            frame.Content = absoluteLayout;

            absoluteLayout.Children.Add(label);
            absoluteLayout.Children.Add(button);
            absoluteLayout.Children.Add(radioButton);
            
            return frame;
        }
        else
        {
            Task task = Data.createTask(title, date, noDeadline);

            Frame frame = new Frame
            {
                BackgroundColor = Color.FromHex("#4D4D4D"),
                CornerRadius = 15,
                Margin = new Thickness(8, 3, 8, 0),
                HeightRequest = 30,
                Padding = 12
            };

            AbsoluteLayout absoluteLayout = new AbsoluteLayout();

            RadioButton radioButton = new RadioButton();
            radioButton.CheckedChanged += (sender, e) =>
            {
                destroy(this, frame, task);
            };

            Button button = new Button
            {
                BackgroundColor = Colors.Transparent,
                Margin = new Thickness(35, -12, -12, -12)
            };
            //AbsoluteLayout.SetLayoutBounds(button, new Rect(0, 0, 1, 1));
            //AbsoluteLayout.SetLayoutFlags(button, AbsoluteLayoutFlags.All);
            button.Clicked += (sender, e) =>
            {
                ViewTask();
            };

            string daysLeft = Date.GetDaysLeft(date);

            Label label = new Label
            {
                Margin = new Thickness(35, 0, 0, 0),
                Text = title + " " + daysLeft,
                TextColor = Colors.White,
                FontSize = 20
            };

            frame.Content = absoluteLayout;

            absoluteLayout.Children.Add(label);
            absoluteLayout.Children.Add(button);
            absoluteLayout.Children.Add(radioButton);

            Data.tasks.Add(task);

            SaveTask();
            return frame;
        }
    }

    public void SaveTask()
    {
        string json = JsonConvert.SerializeObject(Data.tasks);

        string fileName = "Reminder.json";
        string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), fileName);

        File.WriteAllText(filePath, json);
    }
    
    public void destroy(object sender, Frame frame, Task task)
    {
        Data.tasks.Remove(task);
        MainPage.tasksContainer.Children.Remove(frame);
        SaveTask();
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

        await Shell.Current.GoToAsync("//MainPage");
    }

    private void TimeCheckbox_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (TimeCheckbox.IsChecked == true) TaskTime.IsEnabled = true;
        else TaskTime.IsEnabled = false;
    }

    private void TaskTime_Unfocused(object sender, FocusEventArgs e)
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
            TaskTime.IsEnabled = true;
            TaskDate.IsEnabled = true;
            TimeCheckbox.IsEnabled = true;
        }
        else
        {
            TaskTime.IsEnabled = false;
            TaskDate.IsEnabled = false;
            TimeCheckbox.IsEnabled = false;
            TimeCheckbox.IsChecked = false;
        }
    }
}