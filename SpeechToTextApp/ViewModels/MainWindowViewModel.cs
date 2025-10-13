using System;
using System.Collections.ObjectModel;
using System.IO;
using System.IO.Compression;
using System.Linq;
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
        private bool _isListening;

        public ObservableCollection<string> TranscriptWords { get; } = new();

        private bool _initialMessageCleared = false;
        private AudioService? _audioService;
        private SpeechService? _speechService;

        private const string ModelPath = "model";
        private const string ModelUrl = "https://alphacephei.com/vosk/models/vosk-model-small-en-us-0.15.zip";

        public MainWindowViewModel()
        {
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
            }
            catch (Exception e)
            {
                TranscriptWords.Add($"Error initializing services: {e.Message}");
            }
        }

        private async Task DownloadAndExtractModelAsync()
        {
            var zipPath = Path.Combine(Path.GetTempPath(), "vosk-model.zip");
            TranscriptWords.Add("Downloading model, please wait...");

            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(ModelUrl);
                response.EnsureSuccessStatusCode();
                using (var fs = new FileStream(zipPath, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    await response.Content.CopyToAsync(fs);
                }
            }

            TranscriptWords.Clear();
            TranscriptWords.Add("Extracting model...");

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
            TranscriptWords.Clear();
            TranscriptWords.Add("Ready to listen.");
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
            Dispatcher.UIThread.Post(async () =>
            {
                if (!_initialMessageCleared && TranscriptWords.Count > 0)
                {
                    TranscriptWords.Clear();
                    _initialMessageCleared = true;
                }

                var words = result.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                foreach (var word in words)
                {
                    if (string.IsNullOrWhiteSpace(word)) continue;

                    TranscriptWords.Add("");
                    var index = TranscriptWords.Count - 1;

                    foreach (var character in word)
                    {
                        TranscriptWords[index] += character;
                        await Task.Delay(50); // Adjust delay for typing speed
                    }
                    TranscriptWords[index] += " "; // Add space after word
                }
            });
        }

        [RelayCommand]
        private void ClearTranscript()
        {
            TranscriptWords.Clear();
            _initialMessageCleared = true; // Consider a clear as user interaction
        }
    }
}
