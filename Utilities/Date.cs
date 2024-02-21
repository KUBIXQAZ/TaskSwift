namespace TaskSwift.Utilities
{
    internal class Date
    {
        public static string GetDaysLeft(DateTime taskDate)
        {
            TimeSpan timeLeft = GetTimeLeftNow(taskDate);

            if (isOverdue(taskDate)) return "Overdue.";
            else if (timeLeft.Days == 0)
            {
                if (timeLeft.Hours >= 1) return timeLeft.Hours + " H left.";
                else if (timeLeft.Minutes >= 1) return timeLeft.Minutes + " Min left.";
                else return timeLeft.Seconds + " Sec left.";
            }
            else return timeLeft.Days + " Days left.";
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

        public static bool isOverdue(DateTime taskDate)
        {
            if (GetTimeLeftNow(taskDate).TotalSeconds < 0) return true;
            else return false;
        }
    }
}
