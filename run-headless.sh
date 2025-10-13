#!/bin/bash
export DISPLAY=:99
Xvfb :99 -screen 0 1024x768x16 &
pid=$!
cd SpeechToTextApp
dotnet run
kill $pid
