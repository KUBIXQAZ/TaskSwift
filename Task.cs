using CommunityToolkit.Maui.Views;
using Newtonsoft.Json;
using TaskSwift.Models;

namespace TaskSwift.Views
{
    public class Task
    {
        public string title { set; get; }
        public DateTime? date { set; get; }
        public bool withDeadline { set; get; }
        public FlagModel flag { set; get; }

        public static Frame GenerateElementWithoutDeadline(Task task, string title)
        {
            Color bgColor = Color.FromRgb(77, 77, 77);
            Color titleColor = Colors.White;

            Frame frame = new Frame
            {
                BackgroundColor = bgColor,
                CornerRadius = 15,
                BorderColor = task.flag.Color == null ? Colors.Transparent : task.flag.Color,
                Margin = new Thickness(8, 3, 8, 0),
                HeightRequest = 50,
                Padding = 0
            };

            TapGestureRecognizer tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += (s, e) => {
                ViewTask(s, e, task);
            };
            frame.GestureRecognizers.Add(tapGestureRecognizer);

            Grid grid = new Grid
            {
                Padding = new Thickness(5, 0, 5, 0)
            };
            grid.AddColumnDefinition(new ColumnDefinition { Width = 50 });
            grid.AddColumnDefinition(new ColumnDefinition { Width = GridLength.Star });

            RadioButton radioButton = new RadioButton();
            radioButton.CheckedChanged += (sender, e) =>
            {
                destroy(task, null);
            };

            if (title.Length > 29) title = title.Substring(0, 29) + "...";

            Label titleLabel = new Label
            {
                VerticalOptions = LayoutOptions.Center,
                Text = title,
                TextColor = titleColor,
                FontSize = 20
            };

            grid.Add(radioButton, 0);
            grid.Add(titleLabel, 1);

            frame.Content = grid;

            return frame;
        }

        public static Frame GenerateElementWithDeadline(Task task, DateTime date, string title)
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
                BorderColor = task.flag.Color == null ? Colors.Transparent : task.flag.Color,
                Margin = new Thickness(8, 3, 8, 0),
                HeightRequest = 78,
                Padding = 0
            };

            TapGestureRecognizer tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += (s,e) => {
                ViewTask(s,e,task);
            };
            frame.GestureRecognizers.Add(tapGestureRecognizer);

            RadioButton radioButton = new RadioButton();
            radioButton.CheckedChanged += (sender, e) =>
            {
                destroy(task, date);
            };

            if (title.Length > 29) title = title.Substring(0, 29) + "...";

            Label titleLabel = new Label
            {
                VerticalOptions = LayoutOptions.Center,
                Text = title,
                TextColor = titleColor,
                FontSize = 20
            };

            Label dueLabel = new Label();
            dueLabel = new Label
            {
                Text = daysLeft,
                TextColor = color,
                FontSize = 20
            };

            Grid grid = new Grid
            {
                Padding = new Thickness(5, 0, 5, 0)
            };
            grid.AddColumnDefinition(new ColumnDefinition { Width = 50 });
            grid.AddColumnDefinition(new ColumnDefinition { Width = GridLength.Star });
            grid.AddRowDefinition(new RowDefinition { Height = GridLength.Star });
            grid.AddRowDefinition(new RowDefinition { Height = GridLength.Star });

            grid.Add(radioButton, 0, 0);
            grid.SetRowSpan(radioButton, 2);

            grid.Add(titleLabel, 1, 0);
            grid.Add(dueLabel, 1, 1);

            frame.Content = grid;

            return frame;
        }

        public static Frame GenerateCompletedTask(Task task)
        {
            Color bgColor = Color.FromRgb(59, 59, 59);
            Color titleColor = Colors.Gray;

            Frame frame = new Frame
            {
                BackgroundColor = bgColor,
                CornerRadius = 15,
                BorderColor = task.flag.Color == null ? Colors.Transparent : task.flag.Color.WithAlpha(20).WithSaturation(20),
                Margin = new Thickness(8, 3, 8, 0),
                HeightRequest = 50,
                Padding = 0
            };

            Grid grid = new Grid
            {
                Padding = new Thickness(5, 0, 5, 0)
            };
            grid.AddColumnDefinition(new ColumnDefinition { Width = 50 });
            grid.AddColumnDefinition(new ColumnDefinition { Width = GridLength.Star });

            RadioButton radioButton = new RadioButton
            {
                IsEnabled = false,
            };

            if (task.title.Length > 29) task.title = task.title.Substring(0, 29) + "...";

            Label titleLabel = new Label
            {
                VerticalOptions = LayoutOptions.Center,
                Text = task.title,
                TextColor = titleColor,
                FontSize = 20
            };

            grid.Add(radioButton, 0);
            grid.Add(titleLabel, 1);

            frame.Content = grid;

            return frame;
        }

        public static void SaveCompletedTasks()
        {
            string json = JsonConvert.SerializeObject(App.completedTasks);

            File.WriteAllText(App.completedTasksPath, json);
        }

        public static Frame DisplayTasks(Task task)
        {
            if (task.withDeadline) return GenerateElementWithDeadline(task, task.date.Value, task.title);
            else return GenerateElementWithoutDeadline(task, task.title);
        }

        public static Task createTask(string title, DateTime? date, bool withDeadline, FlagModel? flag)
        {
            Task task = new Task();
            task.date = date;
            task.title = title;
            task.withDeadline = withDeadline;
            task.flag = flag;
            return task;
        }

        public static void SaveTask()
        {
            string json = JsonConvert.SerializeObject(App.tasks);

            File.WriteAllText(App.tasksFilePath, json);
        }

        public static void SaveStats()
        {
            string json = JsonConvert.SerializeObject(App.stats);

            File.WriteAllText(App.statsFilePath, json);
        }

        public static void destroy(Task task, DateTime? date)
        {
            App.completedTasks.Add(task);
            SaveCompletedTasks();

            App.tasks.Remove(task);

            SaveTask();

            var currentShellItem = Shell.Current.CurrentPage;

            if (date != null) if (Date.GetOverdue(date.Value)) App.stats.tasksDoneOverdue++;
                else App.stats.tasksDone++;
            App.stats.tasksPending = App.tasks.Count;

            SaveStats();

            if (currentShellItem is ProfilePage profilePage)
            {
                profilePage.DisplayStats();
                profilePage.displayCurrent();
                profilePage.DisplayDoneTasks();
            }
            else if (currentShellItem is MainPage mainPage)
            {
                mainPage.displayTasks();
            }

            MainPage.DisplayWhenNoTasks();
        }

        public static async void ViewTask(object sender, EventArgs e, Task task)
        {
            EditTaskPopup editTaskPopup = new EditTaskPopup(task);
            if (Shell.Current.CurrentPage is MainPage mainPage)
            {
                await mainPage.ShowPopupAsync(editTaskPopup);
                mainPage.displayTasks();
            } else if (Shell.Current.CurrentPage is ProfilePage profilePage)
            {
                await profilePage.ShowPopupAsync(editTaskPopup);
                profilePage.displayCurrent();
                profilePage.DisplayDoneTasks();
            }
        }
    }
}