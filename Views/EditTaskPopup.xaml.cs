using CommunityToolkit.Maui.Views;

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

            TaskDate.Date = task.date.Value;
            TaskTime.Time = task.date.Value.TimeOfDay;
        }
    }

    private void CancelButton_Clicked(object sender, EventArgs e)
    {
		Close();
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


        if (withDeadline)
        {
            App.tasks[App.tasks.IndexOf(taskToEdit)] = Task.createTask(title, combinedDateTime, withDeadline);
        }
        else
        {
            App.tasks[App.tasks.IndexOf(taskToEdit)] = Task.createTask(title, null, withDeadline);
        }

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