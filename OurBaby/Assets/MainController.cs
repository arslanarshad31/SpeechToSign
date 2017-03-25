using System;
using UnityEngine;
using System.IO;
using System.Collections;
using UnityEngine.UI;
using System.Threading;

public class MainController : MonoBehaviour {
	AudioClip recordedClip;
	static string translatedString = "HI MOM DAD EAT MORE";
	static Animator anim;
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
		//translatedString = req.text;
	}

	public void getText() {
		InputField inputText = GameObject.Find("/Canvas/TranslateText").GetComponent<InputField>();
		anim = this.transform.Find("Aj").gameObject.GetComponent<Animator>();
		string[] tokens = translatedString.ToLower().Split(' ');
		StartCoroutine(wait(tokens));
	}

	IEnumerator wait(string[] tokens) {
		for (var i=0; i<tokens.Length;++i){
			Debug.Log("Setting Trig: " + tokens[i]);
			anim.SetTrigger(tokens[i]);
			yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
		 }
	}

}	