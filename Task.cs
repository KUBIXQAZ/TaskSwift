using Microsoft.Maui.Layouts;
using Newtonsoft.Json;

namespace TaskSwift.Views
{
    public class Task
    {
        public string title { set; get; }
        public DateTime? date { set; get; }
        public bool withDeadline { set; get; }

        public Frame GenerateElementWithoutDeadline(Task task, string title)
        {
            Color bgColor = Color.FromRgb(77, 77, 77);
            Color titleColor = Colors.White;

            Frame frame = new Frame
            {
                BackgroundColor = bgColor,
                CornerRadius = 15,
                BorderColor = Colors.Transparent,
                Margin = new Thickness(8, 3, 8, 0),
                HeightRequest = 50,
                Padding = 0
            };

            AbsoluteLayout absoluteLayout = new AbsoluteLayout();

            RadioButton radioButton = new RadioButton
            {
                Margin = new Thickness(6, 5, 0, 0)
            };
            radioButton.CheckedChanged += (sender, e) =>
            {
                destroy(frame, task, null);
            };

            Button button = new Button
            {
                BackgroundColor = Colors.Transparent,
            };
            AbsoluteLayout.SetLayoutBounds(button, new Rect(0, 0, 1, 1));
            AbsoluteLayout.SetLayoutFlags(button, AbsoluteLayoutFlags.All);
            button.Clicked += (sender, e) =>
            {
                ViewTask();
            };

            if (title.Length > 29) title = title.Substring(0, 29) + "...";

            Label titleLabel = new Label
            {
                Margin = new Thickness(40, 10, 0, 0),
                Text = title,
                TextColor = titleColor,
                FontSize = 20
            };

            frame.Content = absoluteLayout;

            absoluteLayout.Children.Add(titleLabel);
            absoluteLayout.Children.Add(button);
            absoluteLayout.Children.Add(radioButton);

            return frame;
        }

        public Frame GenerateElementWithDeadline(Task task, DateTime date, string title)
        {
            string daysLeft = Date.GetDaysLeft(date);
            Color color = Colors.White;
            Color bgColor = Color.FromRgb(77, 77, 77);
            Color titleColor = Colors.White;

            if (daysLeft == "Overdue.")
            {
                color = Color.FromRgb(163, 15, 15);
                titleColor = Color.FromRgb(179, 179, 179);
                bgColor = Color.FromRgb(64, 54, 54);
            }

            Frame frame = new Frame
            {
                BackgroundColor = bgColor,
                CornerRadius = 15,
                BorderColor = Colors.Transparent,
                Margin = new Thickness(8, 3, 8, 0),
                HeightRequest = 78,
                Padding = 0
            };

            AbsoluteLayout absoluteLayout = new AbsoluteLayout();

            RadioButton radioButton = new RadioButton
            {
                Margin = new Thickness(6, 5, 0, 0)
            };
            radioButton.CheckedChanged += (sender, e) =>
            {
                destroy(frame, task, date);
            };

            Button button = new Button
            {
                BackgroundColor = Colors.Transparent,
            };
            AbsoluteLayout.SetLayoutBounds(button, new Rect(0, 0, 1, 1));
            AbsoluteLayout.SetLayoutFlags(button, AbsoluteLayoutFlags.All);
            button.Clicked += (sender, e) =>
            {
                ViewTask();
            };

            if (title.Length > 29) title = title.Substring(0, 29) + "...";

            Label titleLabel = new Label
            {
                Margin = new Thickness(40, 10, 0, 0),
                Text = title,
                TextColor = titleColor,
                FontSize = 20
            };

            Label dueLabel = new Label();
            dueLabel = new Label
            {
                Margin = new Thickness(40, 40, 0, 0),
                Text = daysLeft,
                TextColor = color,
                FontSize = 20
            };

            frame.Content = absoluteLayout;

            absoluteLayout.Children.Add(titleLabel);
            absoluteLayout.Children.Add(dueLabel);
            absoluteLayout.Children.Add(button);
            absoluteLayout.Children.Add(radioButton);

            return frame;
        }

        public static Frame DisplayTasks(Task task)
        {
            Task x = new Task();
            if (task.withDeadline) return x.GenerateElementWithDeadline(task, task.date.Value, task.title);
            else return x.GenerateElementWithoutDeadline(task, task.title);
        }

        public static Task createTask(string title, DateTime? date, bool withDeadline)
        {
            Task task = new Task();
            task.date = date;
            task.title = title;
            task.withDeadline = withDeadline;
            return task;
        }

        public static void SaveTask()
        {
            string json = JsonConvert.SerializeObject(Data.tasks);

            File.WriteAllText(jsonSettings.getTasksStorageFilePath(), json);
        }

        public static void SaveStats()
        {
            string json = JsonConvert.SerializeObject(Data.stats);

            File.WriteAllText(jsonSettings.getStatsFileNamePath(), json);
        }

        public void destroy(Frame frame, Task task, DateTime? date)
        {
            Data.tasks.Remove(task);
            MainPage.tasksContainer.Children.Remove(frame);

            SaveTask();

            var currentShellItem = Shell.Current.CurrentPage;

            if(date != null) if (Date.GetOverdue(date.Value)) Data.stats.tasksDoneOverdue++;
            else Data.stats.tasksDone++;
            Data.stats.tasksPending = Data.tasks.Count;

            SaveStats();

            if (currentShellItem is ProfilePage profilePage)
            {
                profilePage.DisplayStats();
                profilePage.displayCurrent();
            }

            MainPage.DisplayWhenNoTasks();
        }

        public void ViewTask()
        {

        }
    }
}
