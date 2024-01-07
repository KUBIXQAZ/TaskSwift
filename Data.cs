namespace TaskSwift
{
    public class Stats
    {
        public int tasksDone { set; get; }
        public int tasksDoneOverdue { set; get; }
        public int tasksPending { set; get; }
    }

    internal class Data
    {
        public static List<Views.Task> tasks = new List<Views.Task>();
        
        public static Stats stats = new Stats();
    }
}
