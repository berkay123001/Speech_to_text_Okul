using System;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Threading.Tasks;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SpeechToTextApp.Services;

namespace SpeechToTextApp.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        [ObservableProperty]
        private string _recognizedText = "Initializing...";

        [ObservableProperty]
        private bool _isListening;

        private AudioService? _audioService;
        private SpeechService? _speechService;

        private const string ModelPath = "model";
        private const string ModelUrl = "https://alphacephei.com/vosk/models/vosk-model-small-en-us-0.15.zip";

        public MainWindowViewModel()
        {
            // Fire-and-forget async initialization.
            _ = InitializeAsync();
        }

        private async Task InitializeAsync()
        {
            try
            {
                if (!Directory.Exists(ModelPath))
                {
                    await DownloadAndExtractModelAsync();
                }

                _audioService = new AudioService();
                _speechService = new SpeechService(ModelPath);

                _audioService.DataAvailable += OnAudioDataAvailable;
                _speechService.RecognitionResult += OnRecognitionResult;

                RecognizedText = "Ready to listen.";
            }
            catch (Exception e)
            {
                RecognizedText = $"Error initializing services: {e.Message}";
            }
        }

        private async Task DownloadAndExtractModelAsync()
        {
            var zipPath = Path.Combine(Path.GetTempPath(), "vosk-model.zip");
            RecognizedText = "Downloading model, please wait...";

            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(ModelUrl);
                response.EnsureSuccessStatusCode();
                using (var fs = new FileStream(zipPath, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    await response.Content.CopyToAsync(fs);
                }
            }

            RecognizedText = "Extracting model...";

            string tempExtractPath = Path.Combine(Path.GetTempPath(), "vosk-model-extracted");
            if (Directory.Exists(tempExtractPath))
            {
                Directory.Delete(tempExtractPath, true);
            }
            ZipFile.ExtractToDirectory(zipPath, tempExtractPath);

            var extractedDirs = Directory.GetDirectories(tempExtractPath);
            if (extractedDirs.Length == 1)
            {
                Directory.Move(extractedDirs[0], ModelPath);
            }
            else
            {
                throw new Exception("Downloaded model zip file has an unexpected structure.");
            }

            File.Delete(zipPath);
            Directory.Delete(tempExtractPath, true);
        }

        [RelayCommand]
        private void StartListening()
        {
            if (_audioService == null) return;
            _audioService.Start();
            IsListening = true;
        }

        [RelayCommand]
        private void StopListening()
        {
            if (_audioService == null) return;
            _audioService.Stop();
            IsListening = false;
        }

        private void OnAudioDataAvailable(byte[] data)
        {
            if (_speechService == null) return;
            _speechService.ProcessAudio(data);
        }

        private void OnRecognitionResult(string result)
        {
            Dispatcher.UIThread.Post(() =>
            {
                RecognizedText = result;
            });
        }
    }
}
