using System;
using System.Runtime.InteropServices;
using PortAudioSharp;

namespace SpeechToTextApp.Services
{
    /// <summary>
    /// Manages audio input from the microphone using PortAudioSharp.
    /// </summary>
    public class AudioService : IDisposable
    {
        private Stream? _stream;
        public event Action<byte[]>? DataAvailable;

        private const int SampleRate = 16000;
        private const int Channels = 1;
        private const uint FramesPerBuffer = (uint)PaStreamCallback.PaFramesPerBufferUnspecified;

        public AudioService()
        {
            try
            {
                PortAudio.Initialize();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error initializing PortAudio: {e.Message}");
                throw;
            }
        }

        /// <summary>
        /// Starts capturing audio from the microphone.
        /// </summary>
        public void Start()
        {
            if (_stream != null && _stream.IsActive) return;

            var inputParameters = new StreamParameters
            {
                device = PortAudio.DefaultInputDevice,
                channelCount = Channels,
                sampleFormat = SampleFormat.Int16,
                suggestedLatency = PortAudio.GetDeviceInfo(PortAudio.DefaultInputDevice).defaultLowInputLatency
            };

            _stream = new Stream(inputParameters, null, SampleRate, FramesPerBuffer, StreamFlags.ClipOff, AudioCallback, IntPtr.Zero);
            _stream.Start();
        }

        /// <summary>
        /// Stops capturing audio.
        /// </summary>
        public void Stop()
        {
            if (_stream == null || !_stream.IsActive) return;

            _stream.Stop();
            _stream.Close();
            _stream.Dispose();
            _stream = null;
        }

        private StreamCallbackResult AudioCallback(IntPtr input, IntPtr output, uint frameCount, ref StreamCallbackTimeInfo timeInfo, StreamCallbackFlags statusFlags, IntPtr userData)
        {
            if (input == IntPtr.Zero)
            {
                return StreamCallbackResult.Continue;
            }

            int byteCount = (int)(frameCount * Channels * sizeof(short)); // sizeof(short) for Int16
            byte[] buffer = new byte[byteCount];
            Marshal.Copy(input, buffer, 0, byteCount);

            DataAvailable?.Invoke(buffer);

            return StreamCallbackResult.Continue;
        }

        /// <summary>
        /// Disposes of the PortAudio stream and terminates the library.
        /// </summary>
        public void Dispose()
        {
            Stop();
            try
            {
                if (PortAudio.IsInitialized)
                {
                    PortAudio.Terminate();
                }
            }
            catch (Exception e)
            {
                 Console.WriteLine($"Error terminating PortAudio: {e.Message}");
            }
        }
    }
}
