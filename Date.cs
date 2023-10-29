using System.Data;

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

        public static TimeSpan GetTimeLeftNow(DateTime taskDate)
        {
            TimeSpan daysLeft = taskDate - DateTime.Now;
            return daysLeft;
        }

        public static TimeSpan GetTimeLeft(DateTime taskDate, DateTime now)
        {
            TimeSpan daysLeft = taskDate - now;
            return daysLeft;
        }

        public static bool GetOverdue(DateTime taskDate)
        {
            TimeSpan daysLeft = taskDate - DateTime.Now;

            if (daysLeft.TotalMicroseconds < 0) return true;
            else return false;
        }
    }
}
