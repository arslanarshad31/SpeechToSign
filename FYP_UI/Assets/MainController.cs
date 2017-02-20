using System;
using UnityEngine;
using System.IO;
using System.Collections;
using UnityEngine.UI;

public class MainController : MonoBehaviour {
	AudioClip recordedClip;
	public void RecordAudio() {
		Debug.Log ("Recording Audio...");
		recordedClip = Microphone.Start(null, false, 60, 44100);
	}

	public void SaveAudio() {
		var position = Microphone.GetPosition(null);
		if (Microphone.IsRecording(null)) Microphone.End(null);
 		var soundData = new float[recordedClip.samples * recordedClip.channels];
 		recordedClip.GetData (soundData, 0);
		var newData = new float[position * recordedClip.channels];
		for (int i = 0; i < newData.Length; i++) { newData[i] = soundData[i]; }
		var newClip = AudioClip.Create (recordedClip.name, position, recordedClip.channels, recordedClip.frequency, false, false);
		AudioClip.Destroy(recordedClip);
 		recordedClip = newClip;   
		string filename =  "audio_" + DateTime.Now.ToString("yyyyMMddHHmmssffff");
		SavWav.Save(filename, recordedClip);
		
	}

	public void UploadAudio(AudioClip clip) {
		// float[] data = new float[clip.samples * clip.channels];
		
		// WWWForm form = new WWWForm();
		// form.AddBinaryData("fileUpload", bytes, "audio.wav", "audio/wav");

	}
	public void TranslateController(){
		InputField inputText = GameObject.Find("/Canvas/InputText").GetComponent<InputField>();
		Debug.Log("Input text: " + inputText.text);
		// HTTP Request
		StartCoroutine(getTranslate(inputText.text.Replace(' ', '%')));
	}

	private IEnumerator getTranslate(String text){
    	WWW req = new WWW("http://localhost:8081/translate/"+ text);
    	yield return req;
		InputField translateText = GameObject.Find("/Canvas/TranslateText").GetComponent<InputField>();
    	translateText.text = req.text;
	}

}	