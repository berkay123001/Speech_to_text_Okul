# Speech-to-Text Avalonia App (Linux & Windows)

Basit, çevrimdışı çalışan bir Konuşmadan-Yazıya (Speech-to-Text) uygulaması. C# + AvaloniaUI ile yazıldı ve Vosk kullanır. Linux ve Windows'ta çalışır.

## Features

- Real-time speech-to-text transcription.
- Simple and modern UI using Avalonia.
- Automatic download of the Vosk speech model on first run.
- Cross-platform: Linux ve Windows desteklenir.
- Transcript paneli: Son (final) tanınan cümleler ekranda listelenir.
- Otomatik kayıt: Her dinleme seansı `transcripts/` klasöründe zaman damgalı `.txt` dosyasına kaydedilir.

> Varsayılan model: Türkçe (Vosk small tr). İsterseniz `ViewModels/MainWindowViewModel.cs` içindeki `ModelUrl` değerini değiştirerek farklı bir model kullanabilirsiniz (örn. İngilizce).

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Mikrofon

## How to Run (Linux/Windows)

1.  **Clone the repository:**
    ```bash
    git clone <repository-url>
    cd <repository-directory>/Speech_to_text_Okul/SpeechToTextApp
    ```

2.  **Run the application:**
    ```bash
    dotnet run
    ```

3.  **Model Download:**
    İlk çalıştırmada gerekli Vosk modeli otomatik indirilir ve çıkartılır. Ekranda ilerleme bilgisi görünür. (Varsayılan: küçük İngilizce model.)

4.  **Start Listening:**
    Model hazır olunca "🎙️ Start Listening" ile dinlemeyi başlatın. Anlık tanıma üstte görünür. Final cümleler alttaki Transcript kutusuna eklenir ve diske yazılır.

5.  **Transcript Files:**
    - Kayıt yeri: `transcripts/`
    - Dosya adı: `transcript_YYYYMMDD_HHMMSS.txt`
    - Her satır bir final tanıma sonucudur. Başlangıç/bitiş zamanları dosyada işaretlenir.

## Windows: Create a portable .exe

Self-contained ve tek dosya exe üretimi (x64):

```bash
dotnet publish -c Release -r win-x64 --self-contained true \
  /p:PublishSingleFile=true /p:IncludeNativeLibrariesForSelfExtract=true
```

Çıktı: `bin/Release/net8.0/win-x64/publish/` içinde `SpeechToTextApp.exe`.

### Çalıştırma (Windows)
- Dosyayı çift tıklayarak çalıştırın veya Komut İstemi'nde:
  ```cmd
  cd bin\Release\net8.0\win-x64\publish
  SpeechToTextApp.exe
  ```
- İlk açılışta model indirileceği için internet ve yazma izni gerekir.

Notlar:
- İlk çalıştırmada model indirileceği için exe bulunduğu klasöre yazma izni olmalı.
- Mikrofon erişimi için Windows'ta Sistem Ayarları > Gizlilik > Mikrofon bölümünden izin verdiğinizden emin olun.
- Eğer ses girişinde sorun yaşarsanız Visual C++ Runtime veya ses sürücülerini güncellemeniz gerekebilir.

## Troubleshooting

- Model indirme başarısız: Ağ bağlantısını ve firewall ayarlarını kontrol edin.
- Performans düşükse: Daha büyük/lokal dile uygun bir Vosk modeli kullanmayı düşünün (`model` klasörünü uygun modelle değiştirin) veya `ModelUrl`'i değiştirip uygulamayı yeniden başlatın.
- Windows Defender bazen tek dosya exe'lerde yanlış pozitif uyarı verebilir. Kaynağın size ait olduğundan emin olarak izin verin.
- Mikrofon çalışmıyor: OS mikrofon izinleri ve input device ayarlarını kontrol edin.
