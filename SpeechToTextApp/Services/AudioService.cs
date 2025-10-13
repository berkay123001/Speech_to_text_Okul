using System;
using NAudio.Wave;

namespace SpeechToTextApp.Services
{
    /// <summary>
    /// Manages audio input from the microphone using NAudio.
    /// </summary>
    public class AudioService
    {
        private WaveInEvent? _waveIn;
        public event Action<byte[]>? DataAvailable;

        /// <summary>
        /// Starts capturing audio from the microphone.
        /// </summary>
        public void Start()
        {
            if (_waveIn != null) return;

            _waveIn = new WaveInEvent
            {
                WaveFormat = new WaveFormat(16000, 1) // 16kHz, Mono
            };

            _waveIn.DataAvailable += OnDataAvailable;
            _waveIn.StartRecording();
        }

        /// <summary>
        /// Stops capturing audio.
        /// </summary>
        public void Stop()
        {
            if (_waveIn == null) return;

            _waveIn.StopRecording();
            _waveIn.Dispose();
            _waveIn = null;
        }

        private void OnDataAvailable(object? sender, WaveInEventArgs e)
        {
            DataAvailable?.Invoke(e.Buffer);
        }
    }
}
