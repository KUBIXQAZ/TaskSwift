namespace TaskSwift
{
    public class Task
    {
        public string title { set; get; }
        public DateTime date { set; get; }
        public bool noDeadline { set; get; }

        public string Title()
        {
            return title;
        }
    }

    public class Stats
    {
        public int tasksDone { set; get; }
        public int tasksDoneOverdue { set; get; }
        public int tasksPending { set; get; }
    }

    internal class Data
    {
        public static List<Task> tasks = new List<Task>();
        
        public static Stats stats = new Stats();
        
        public static Task createTask(string title, DateTime date, bool noDeadline)
        {
            Task task = new Task();
            task.date = date;
            task.title = title;
            task.noDeadline = noDeadline;
            return task;
        }
    }
}
