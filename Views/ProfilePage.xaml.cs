using Newtonsoft.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TaskSwift.Views;

public partial class ProfilePage : ContentPage
{
	public ProfilePage()
	{
		InitializeComponent();

        LoadStats();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        doneText.Text = Data.stats.tasksDone.ToString();
        doneOverdueText.Text = Data.stats.tasksDoneOverdue.ToString();
        pendingText.Text = Data.stats.tasksPending.ToString();

        displayCurrent();
    }

    public void LoadStats()
    {
        if(File.Exists(jsonSettings.getStatsFileNamePath()))
        {
            string json = File.ReadAllText(jsonSettings.getStatsFileNamePath());
            Data.stats = JsonConvert.DeserializeObject<Stats>(json);
        }
    }

    public void displayCurrent()
    {
        StackLayoutCurrTask.Children.Clear();

        List<Task> tasksWithDeadline = new List<Task>();
        for (int i = 0; i < Data.tasks.Count; i++)
        {
            if (!Data.tasks[i].noDeadline)
            {
                tasksWithDeadline.Add(Data.tasks[i]);
            }
        }

        List<Task> currTasks = new List<Task>();
        Task currTask = null;
        DateTime now = Date.GetDate();

        foreach (Task task in tasksWithDeadline)
        {
            TimeSpan timeLeft = Date.GetTimeLeft(task.date, now);
            TimeSpan currTaskLeft = new TimeSpan();

            try { currTaskLeft = Date.GetTimeLeft(currTask.date, now); }
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

        foreach (Task task in currTasks)
        {
            AddTaskPage at = new AddTaskPage();
            StackLayoutCurrTask.Children.Add(at.GenerateTask(task.title, task.date, task.noDeadline, true, task));
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