using TaskSwift.Models;

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

        Dispatcher.StartTimer(TimeSpan.FromMilliseconds(1), () =>
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

        DisplayFlags();
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
                if(selectedFlagFrame != null || (Frame)sender == selectedFlagFrame)
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

    public void GenerateTask(bool withDeadline, string title, DateTime date, FlagModel flag)
    {
        Task task;

        task = Task.createTask(title, withDeadline ? date : DateTime.MinValue, withDeadline, flag);

        App.tasks.Add(task);
        Task.SaveTask();
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

        GenerateTask(withDeadline, title, withDeadline ? combinedDateTime : DateTime.MinValue, selectedFlag);

        Title.Text = string.Empty;

        App.stats.tasksPending = App.tasks.Count;

        Task.SaveStats();

        Shell.Current.GoToAsync("//MainPage");
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