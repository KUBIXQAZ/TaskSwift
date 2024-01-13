using Newtonsoft.Json;

namespace TaskSwift.Models
{
    public class FlagModel
    {
        public Color Color { get; set; }
        public string Name { get; set; }

        public static Frame FlagUI(Color color, string Name, EventHandler<TappedEventArgs> eventHandler)
        {
            Frame frame = new Frame
            {
                BackgroundColor = Colors.Transparent,
                BorderColor = color,
                HorizontalOptions = LayoutOptions.Start,
                Padding = new Thickness(20,12,20,12),
                VerticalOptions = LayoutOptions.Start,
                Margin = new Thickness(0,0,8,0)
            };

            TapGestureRecognizer tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += eventHandler;
            frame.GestureRecognizers.Add(tapGestureRecognizer);

            Label label = new Label
            {
                Text = Name,
                VerticalOptions = LayoutOptions.Center,
            };

            frame.Content = label;

            return frame;
        }

        public static void LoadFlags()
        {
            if (!File.Exists(App.flagsFilePath))
            {
                FlagModel flag1 = new FlagModel
                {
                    Color = Colors.Red,
                    Name = "Important",
                };
                FlagModel flag2 = new FlagModel
                {
                    Color = Colors.Blue,
                    Name = "Homework",
                };
                FlagModel flag3 = new FlagModel
                {
                    Color = Colors.Green,
                    Name = "Test",
                };

                App.flags.Add(flag1);
                App.flags.Add(flag2);
                App.flags.Add(flag3);

                SaveFlags();
            }
            string json = File.ReadAllText(App.flagsFilePath);
            App.flags = JsonConvert.DeserializeObject<List<FlagModel>>(json);
        }

        public static void SaveFlags()
        {
            string json = JsonConvert.SerializeObject(App.flags);

            File.WriteAllText(App.flagsFilePath, json);
        }
    }
}
