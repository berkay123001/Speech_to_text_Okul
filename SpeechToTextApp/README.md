# Speech-to-Text Avalonia App for Linux

This is a simple Speech-to-Text application built with C# and AvaloniaUI, designed to be compatible with Linux. It uses the Vosk speech recognition toolkit for offline speech-to-text conversion.

## Features

- Real-time speech-to-text transcription.
- Simple and modern UI using Avalonia.
- Automatic download of the Vosk speech model on first run.
- Cross-platform (should also work on Windows and macOS).

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- A working microphone.

## How to Run

1.  **Clone the repository:**
    ```bash
    git clone <repository-url>
    cd <repository-directory>/SpeechToTextApp
    ```

2.  **Run the application:**
    ```bash
    dotnet run
    ```

3.  **Model Download:**
    On the first run, the application will automatically download the required Vosk English model (approx. 40MB). Please wait for the download and extraction to complete. The UI will show the progress.

4.  **Start Listening:**
    Once the model is ready, click the "🎙️ Start Listening" button and start speaking. The recognized text will appear in the text box.
