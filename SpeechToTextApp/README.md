# SpeechFlow - Cinematic Speech-to-Text Avalonia App for Linux

This is a premium, Apple-style Speech-to-Text application built with C# and AvaloniaUI, designed to be compatible with Linux. It uses the Vosk speech recognition toolkit for offline speech-to-text conversion and features a cinematic, animated interface.

## 🪄 Features

-   **Animated Welcome Screen:** A smooth, blurred fade-in welcome screen with text animation.
-   **Cinematic Main Interface:** A central circular microphone button with a glowing pulse animation when recording, and an animated "Listening..." indicator.
-   **Dynamic Waveform Visualization:** A waveform visualization that reacts to microphone input.
-   **Continuous Transcript Display:** Recognized text is continuously appended, with each new word appearing with a typewriter animation and a subtle fade-in and upward motion.
-   **Automatic Light/Dark Mode:** The application automatically adapts to your system's theme.
-   **Responsive Design:** The UI is fully responsive for screens from 720p to 1440p.

## UI Preview

![SpeechFlow UI Preview](https://i.imgur.com/YOUR_GIF_URL.gif)

*Note: Replace the URL above with a GIF or screenshot of the new UI.*

## Prerequisites

-   [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
-   A working microphone.

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
    On the first run, the application will automatically download the required Vosk English model (approx. 40MB). Please wait for the download and extraction to complete.

4.  **Start Listening:**
    Once the model is ready, click the microphone button and start speaking. The recognized text will appear on the screen with cinematic animations.
