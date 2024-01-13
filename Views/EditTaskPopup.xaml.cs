using CommunityToolkit.Maui.Views;
using TaskSwift.Models;

namespace TaskSwift.Views;

public partial class EditTaskPopup : Popup
{
    Task taskToEdit;

	public EditTaskPopup(Task task)
	{
		InitializeComponent();

        taskToEdit = task;

        Title.Text = task.title;

        if(task.withDeadline)
        {
            DeadlineCheckbox.IsChecked = true;
            TimeCheckbox.IsChecked = true;

            TaskDate.Date = task.date;
            TaskTime.Time = task.date.TimeOfDay;
        }

        DisplayFlags();
    }

    private void CancelButton_Clicked(object sender, EventArgs e)
    {
		Close();
    }

    Frame selectedFlagFrame = null;
    FlagModel selectedFlag = null;
    private void DisplayFlags()
    {
        FlagsHorizontalStackLayout.Clear();

        foreach (FlagModel flag in App.flags)
        {
            EventHandler<TappedEventArgs> eventHandler = (sender, e) =>
            {
                if (selectedFlagFrame != null || (Frame)sender == selectedFlagFrame)
                {
                    selectedFlagFrame.Background = Colors.Transparent;
                }
                if ((Frame)sender != selectedFlagFrame)
                {
                    selectedFlagFrame = (Frame)sender;
                    selectedFlag = flag;
                    selectedFlagFrame.Background = flag.Color;
                }
                else
                {
                    selectedFlagFrame = null;
                    selectedFlag = null;
                }
            };

            FlagsHorizontalStackLayout.Add(FlagModel.FlagUI(flag.Color, flag.Name, eventHandler));
        }
    }

    public void AddTaskButton(object sender, EventArgs e)
    {
        string title = Title.Text;

        DateTime selectedDate = TaskDate.Date;
        TimeSpan selectedTime;

        if (TimeCheckbox.IsChecked == true) selectedTime = TaskTime.Time;
        else selectedTime = new TimeSpan(24, 0, 0);

        DateTime combinedDateTime = selectedDate.Add(selectedTime);

        bool withDeadline = DeadlineCheckbox.IsChecked;

        App.tasks[App.tasks.IndexOf(taskToEdit)] = Task.createTask(title, withDeadline ? combinedDateTime : DateTime.MinValue, withDeadline, selectedFlag);

        Title.Text = string.Empty;

        App.stats.tasksPending = App.tasks.Count;

        Task.SaveStats();
        Task.SaveTask();

        Close();
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