using System;
using System.IO;
using System.Text.Json;
using Vosk;

namespace SpeechToTextApp.Services
{
    /// <summary>
    /// Performs speech-to-text conversion using the Vosk library.
    /// </summary>
    public class SpeechService : IDisposable
    {
        private readonly VoskRecognizer _recognizer;
        private readonly Model _model;

        public event Action<string>? RecognitionResult;

        /// <summary>
        /// Initializes the speech service by loading the Vosk model.
        /// Throws an exception if the model path is not valid.
        /// </summary>
        /// <param name="modelPath">The path to the Vosk model directory.</param>
        public SpeechService(string modelPath)
        {
            if (!Directory.Exists(modelPath))
            {
                throw new DirectoryNotFoundException($"Vosk model not found at path: {modelPath}");
            }

            _model = new Model(modelPath);
            _recognizer = new VoskRecognizer(_model, 16000.0f);
            _recognizer.SetMaxAlternatives(0);
            _recognizer.SetWords(false);
        }

        /// <summary>
        /// Processes a chunk of audio data and raises the RecognitionResult event
        /// with partial or final results.
        /// </summary>
        /// <param name="buffer">The audio data to process.</param>
        public void ProcessAudio(byte[] buffer)
        {
            if (_recognizer.AcceptWaveform(buffer, buffer.Length))
            {
                var result = _recognizer.Result();
                OnRecognitionResult(result);
            }
            else
            {
                var partialResult = _recognizer.PartialResult();
                if (!string.IsNullOrEmpty(partialResult))
                {
                    OnRecognitionResult(partialResult);
                }
            }
        }

        private void OnRecognitionResult(string jsonResult)
        {
            try
            {
                using var doc = JsonDocument.Parse(jsonResult);
                if (doc.RootElement.TryGetProperty("text", out var textElement))
                {
                    var text = textElement.GetString();
                    if (!string.IsNullOrEmpty(text))
                    {
                        RecognitionResult?.Invoke(text);
                    }
                }
                else if (doc.RootElement.TryGetProperty("partial", out var partialElement))
                {
                    var partialText = partialElement.GetString();
                    if (!string.IsNullOrEmpty(partialText))
                    {
                        RecognitionResult?.Invoke(partialText);
                    }
                }
            }
            catch (JsonException)
            {
                // Ignore invalid JSON
            }
        }

        /// <summary>
        /// Disposes the Vosk recognizer and model.
        /// </summary>
        public void Dispose()
        {
            _recognizer.Dispose();
            _model.Dispose();
        }
    }
}
