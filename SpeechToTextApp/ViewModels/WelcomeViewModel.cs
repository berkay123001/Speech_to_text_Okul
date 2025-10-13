using CommunityToolkit.Mvvm.ComponentModel;

namespace SpeechToTextApp.ViewModels
{
    public partial class WelcomeViewModel : ViewModelBase
    {
        [ObservableProperty]
        private string _welcomeText = "Merhaba, uygulamaya hoş geldiniz.";
    }
}
