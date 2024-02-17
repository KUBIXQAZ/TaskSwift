using TaskSwift.Views;

namespace TaskSwift;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

        Routing.RegisterRoute("MainPage", typeof(MainPage));
        Routing.RegisterRoute("AddTaskPage", typeof(AddTaskPage));
    }
}