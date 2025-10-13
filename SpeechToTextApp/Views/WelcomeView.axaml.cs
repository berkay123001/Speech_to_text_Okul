using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System.Threading.Tasks;

namespace SpeechToTextApp.Views
{
    public partial class WelcomeView : Window
    {
        public WelcomeView()
        {
            InitializeComponent();
            _ = AnimateAndClose();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private async Task AnimateAndClose()
        {
            await Task.Delay(100); // Allow the window to render

            var welcomeTextBlock = this.FindControl<TextBlock>("WelcomeTextBlock");
            if (welcomeTextBlock != null)
            {
                // Trigger the animation
                welcomeTextBlock.Opacity = 1;
            }

            await Task.Delay(2500); // Wait for the animation to complete

            var mainWindow = new MainWindow
            {
                DataContext = new ViewModels.MainWindowViewModel()
            };
            mainWindow.Show();
            this.Close();
        }
    }
}
