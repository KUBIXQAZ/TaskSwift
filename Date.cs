namespace TaskSwift
{
    internal class Date
    {
        public static DateTime GetDate()
        {
            DateTime currDate = DateTime.Now;
            return currDate;
        }

        public static string GetDaysLeft(DateTime taskDate)
        {
            TimeSpan daysLeft = taskDate - DateTime.Now;
            string result = "";

            if (daysLeft.Days == 0 && daysLeft.TotalMicroseconds > 0)
            {
                if (daysLeft.Hours >= 1) result = daysLeft.Hours.ToString() + " H left.";
                else if (daysLeft.Minutes >= 1) result = daysLeft.Minutes.ToString() + " Min left.";
                else if (daysLeft.Seconds >= 0) result = daysLeft.Seconds.ToString() + " Sec left.";
            }
            else if (daysLeft.Days > 0) result = daysLeft.Days.ToString() + " Days left.";
            else result = "Overdue.";

            return result;
        }
    }
}
