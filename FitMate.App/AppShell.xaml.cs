namespace FitMate;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
    }

    protected override void OnNavigating(ShellNavigatingEventArgs args)
    {
        base.OnNavigating(args);

        if (args.Target != null)
        {
            // System.Diagnostics.Debug.WriteLine(args.Target.Location);
        }
    }
}