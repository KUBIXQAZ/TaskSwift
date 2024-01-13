using CommunityToolkit.Maui.Views;
using Microsoft.Maui.Controls;
using System;
using TaskSwift.Models;

namespace TaskSwift.Views;

public partial class MainPage : ContentPage
{
    public static StackLayout tasksContainer;

    public MainPage()
    {
        InitializeComponent();

        tasksContainer = TasksContainer;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        displayTasks();
        DisplayWhenNoTasks();
        DisplayFlags();
    }

    Frame selectedFlagFrame = null;
    FlagModel selectedFlag = null;
    public void DisplayFlags()
    {
        FlagsHorizontalStackLayout.Clear();

        foreach (FlagModel flag in App.flags)
        {
            EventHandler<TappedEventArgs> eventHandler = (sender, e) =>
            {
                if (selectedFlagFrame != null || (Frame)sender == selectedFlagFrame)
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
                displayTasks();
            };

            FlagsHorizontalStackLayout.Add(FlagModel.FlagUI(flag.Color, flag.Name, eventHandler));
        }

        Frame frame = new Frame
        {
            BackgroundColor = Colors.Transparent,
            BorderColor = Colors.White,
            HorizontalOptions = LayoutOptions.Start,
            Padding = new Thickness(12, 12, 12, 12),
            VerticalOptions = LayoutOptions.Start,
            Margin = new Thickness(0, 0, 8, 0)
        };

        TapGestureRecognizer tapGestureRecognizer = new TapGestureRecognizer();
        tapGestureRecognizer.Tapped += async (s,e)=>
        {
            CreateFlagPopup createFlagPopup = new CreateFlagPopup();
            await this.ShowPopupAsync(createFlagPopup);
        };
        frame.GestureRecognizers.Add(tapGestureRecognizer);

        Image image = new Image
        {
            Source = ImageSource.FromFile("plus.svg")
        };

        frame.Content = image;

        FlagsHorizontalStackLayout.Add(frame);
    }

    public static void DisplayWhenNoTasks()
    {
        if (App.tasks.Count == 0)
        {
            var stack = new StackLayout
            {
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand
            };

            var gradient = new LinearGradientBrush
            {
                GradientStops =
                {
                    new GradientStop(Color.FromHex("#66B4FF"), 0),
                    new GradientStop(Color.FromHex("#428bff"), 0.5f),
                    new GradientStop(Color.FromHex("#66B4FF"), 1),
                }
            };

            var label = new Label
            {
                Text = "You have nothing planned yet.",
                FontSize = 16,
                TextColor = Color.FromHex("#66B4FF")
            };

            var button = new Button
            {
                Text = "Add task",
                Background = gradient,
                TextColor = Color.FromHex("#121212"),
                FontSize = 16,
                FontAttributes = FontAttributes.Bold,
                WidthRequest = 120,
                HeightRequest = 40,
                Padding = 0,
                Margin = 4
            };

            button.Clicked += (sender, e) =>
            {
                Shell.Current.GoToAsync("//AddTaskPage");
            };

            stack.Children.Add(label);
            stack.Children.Add(button);

            tasksContainer.Children.Add(stack);
        }
    }

    public void displayTasks()
    {
        tasksContainer.Children.Clear();

        StackLayout stackLayout = new StackLayout();

        List<Task> tasks = null;
        if (selectedFlag != null)
        {
            tasks = new List<Task>();
            foreach (Task task in App.tasks)
            {
                if(task.flag != null) if (Equals(task.flag.Name, selectedFlag.Name) && Equals(task.flag.Color, selectedFlag.Color)) tasks.Add(task);
            }
        }
        else tasks = App.tasks;

        var orderTasks = tasks.OrderByDescending(task => task.date).Reverse();
        var groupedTasks = orderTasks.GroupBy(task => task.date.Date);

        foreach (var taskGroup in groupedTasks)
        {
            DateTime groupDate = taskGroup.Key;

            if (groupDate != DateTime.MinValue)
            {
                string title = null;
                var daysLeft = Date.GetDayLeft(groupDate);
                if (daysLeft < 0) title = "Overdue.";
                else if (daysLeft == 0) title = "Today.";
                else title = $"{daysLeft} Days left.";

                Label sectionTitle = new Label
                {
                    Text = title,
                    TextColor = Color.FromHex("#C0C0C0"),
                    FontSize = 20,
                    Margin = new Thickness(5, 10, 0, 5)
                };
                stackLayout.Add(sectionTitle);
            } else
            {
                Label sectionTitle = new Label
                {
                    Text = "No deadline.",
                    TextColor = Color.FromHex("#C0C0C0"),
                    FontSize = 20,
                    Margin = new Thickness(5, 10, 0, 5)
                };
                stackLayout.Add(sectionTitle);
            }

            foreach (var task in taskGroup)
            {
                stackLayout.Add(Task.DisplayTasks(task));
            }
        }

        tasksContainer.Children.Add(stackLayout);
    }
}