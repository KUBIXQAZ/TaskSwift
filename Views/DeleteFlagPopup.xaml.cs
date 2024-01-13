using CommunityToolkit.Maui.Views;
using TaskSwift.Models;

namespace TaskSwift.Views;

public partial class DeleteFlagPopup : Popup
{
	public DeleteFlagPopup()
	{
		InitializeComponent();

        foreach(FlagModel flag in App.flags)
        {
            Grid grid = new Grid
            {
                HorizontalOptions = LayoutOptions.Center,
                Margin = new Thickness(0,5,0,0)
            };
            grid.AddColumnDefinition(new ColumnDefinition { Width = GridLength.Auto });
            grid.AddColumnDefinition(new ColumnDefinition { Width = GridLength.Star });

            Frame frame = new Frame
            {
                BackgroundColor = Colors.Transparent,
                BorderColor = Colors.White,
                Padding = new Thickness(12, 12, 12, 12),
                Margin = new Thickness(0, 0, 8, 0)
            };
            TapGestureRecognizer tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += (s, e) =>
            {
                foreach (Task task in App.tasks)
                {
                    if(task.flag != null)
                    {
                        if (task.flag.Name == flag.Name && task.flag.Color == flag.Color)
                        {
                            task.flag = null;
                            Task.SaveTask();
                            if (Shell.Current.CurrentPage is MainPage mainPage) mainPage.displayTasks();
                        }
                    }
                }
                App.flags.Remove(flag);
                flagsVerticalStackLayout.Remove(grid);
                FlagModel.SaveFlags();
            };
            frame.GestureRecognizers.Add(tapGestureRecognizer);

            Image image = new Image
            {
                Source = ImageSource.FromFile("minus.svg")
            };

            frame.Content = image;

            grid.Add(frame,0);

            Frame frame2 = FlagModel.FlagUI(flag.Color, flag.Name, null);
            frame2.MinimumWidthRequest = 150;

            Label label = (Label)frame2.Content;
            label.HorizontalOptions = LayoutOptions.Center;

            grid.Add(frame2,1);

            flagsVerticalStackLayout.Add(grid);
        }
	}

    private void CloseButton_Clicked(object sender, EventArgs e)
    {
        if (Shell.Current.CurrentPage is MainPage mainPage) mainPage.DisplayFlags();
        Close();
    }
}