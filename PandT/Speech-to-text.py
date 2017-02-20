# NOTE: PyPI, setup.py (distutils) and PyAudio | It works awesomely! :) | from https://github.com/Uberi/speech_recognition/blob/master/examples/microphone_recognition.py

import speech_recognition as sr
from os import path
AUDIO_FILE = path.join(path.dirname(path.realpath(__file__)), "")


def transcribeAudio():
    # use the audio file as the audio source
    r = sr.Recognizer()
    with sr.AudioFile(AUDIO_FILE) as source:
        audio = r.record(source)  # read the entire audio file

    # recognize speech using Google Speech Recognition
    try:
        # for testing purposes, we're just using the default API key
        # to use another API key, use `r.recognize_google(audio, key="GOOGLE_SPEECH_RECOGNITION_API_KEY")`
        # instead of `r.recognize_google(audio)`
        return r.recognize_google(audio)
    except sr.UnknownValueError:
        return "Google Speech Recognition could not understand audio"
    except sr.RequestError as e:
        return "Could not request results from Google Speech Recognition service; {0}".format(e)


print(transcribeAudio())

