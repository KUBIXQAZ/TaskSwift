namespace TaskSwift
{
    class jsonSettings
    {
        public static string getTasksStorageFilePath()
        {
            string tasksStorageFileName = "TaskSwift.json";
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), tasksStorageFileName);
        }

        public static string getStatsFileNamePath()
        {
            string statsFileName = "Stats.json";
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), statsFileName);
        }
    }
}