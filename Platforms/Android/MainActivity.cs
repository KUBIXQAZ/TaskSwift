using Android.App;
using Android.Content.PM;
using Android.OS;
using Microsoft.Maui.Controls.Compatibility.Platform.Android;

namespace TaskSwift;

[Activity(Theme = "@style/Maui.SplashTheme", ScreenOrientation = ScreenOrientation.Portrait, MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
    protected override void OnCreate(Bundle savedInstanceState)
    {
        Window.SetStatusBarColor(Color.FromHex("#121212").ToAndroid());
        Window.SetNavigationBarColor(Color.FromHex("#121212").ToAndroid());

        base.OnCreate(savedInstanceState);
    }
}
