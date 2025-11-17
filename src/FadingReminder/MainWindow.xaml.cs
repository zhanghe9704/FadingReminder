using System.Windows;

namespace FadingReminder;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// This window is hidden - the application runs from the system tray
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        // Hide the window immediately
        Hide();
    }
}
