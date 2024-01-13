using CommunityToolkit.Maui.Views;
using TaskSwift.Models;

namespace TaskSwift.Views;

public partial class CreateFlagPopup : Popup
{
	List<Frame> frames = new List<Frame>();
	Color color = Colors.White;

	public CreateFlagPopup()
	{
		InitializeComponent();

        EventHandler<TappedEventArgs> eventHandler = (s, e) => {
			Frame frame = (Frame)s;
			color = frame.BackgroundColor;
        };

        foreach (Frame frame in colorsHorizontalStackLayout.Children)
		{
			frames.Add(frame);
			TapGestureRecognizer tapGestureRecognizer = new TapGestureRecognizer();
			tapGestureRecognizer.Tapped += eventHandler;
		
			frame.GestureRecognizers.Add(tapGestureRecognizer);
        }
	}

    private void CancelButton_Clicked(object sender, EventArgs e)
    {
		Close();
    }

    private void SubmitButton_Clicked(object sender, EventArgs e)
    {
		FlagModel flag = new FlagModel
		{
			Name = flagName.Text,
			Color = color,
		};
		App.flags.Add(flag);
		FlagModel.SaveFlags();
		if(Shell.Current.CurrentPage is MainPage mainPage) mainPage.DisplayFlags();

		Close();
    }

    private void flagName_TextChanged(object sender, TextChangedEventArgs e)
    {
        int i = 0;
        string title = flagName.Text;
        i = title.Replace(" ", "").Length;

        if (i <= 0) submitB.IsEnabled = false;
        else submitB.IsEnabled = true;
    }
}