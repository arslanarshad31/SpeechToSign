using System;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Collections;
using UnityEngine.UI;
using System.Threading;

public class MainController : MonoBehaviour {
	static Animator anim;
	void Start () {}
	
	public void TranslateController(){
		InputField inputText = GameObject.Find("/Canvas/InputText").GetComponent<InputField>();
		Debug.Log("Input text: " + inputText.text);
		// Sending HTTP Request
		StartCoroutine(getTranslate(inputText.text.Replace(' ', '@')));
	}

	private IEnumerator getTranslate(String text){
		string request = "http://localhost:8080/translate/"+ text;
		Debug.Log(request);
    	WWW req = new WWW(request);
    	yield return req;
		// HTTP response recieved
		InputField translateText = GameObject.Find("/Canvas/TranslateText").GetComponent<InputField>();
		translateText.text = req.text;
		anim = this.transform.Find("Aj").gameObject.GetComponent<Animator>();
		//anim = GetComponent<Animator>();
		string[] tokens = req.text.ToLower().Split(' ');
		StartCoroutine(waitedAnimate(tokens));
		//anim.SetTrigger("idle");
	}
	
	IEnumerator waitedAnimate(string[] tokens) {
		
		Text displayWord = GameObject.Find("/Canvas/DisplayWord").GetComponent<Text>();
		
//		string[] corpus = {"hkust", "arslan", "onur", "akanksha", "sheetal", "aj", "tony"};
		
		for (var i=0; i<tokens.Length;++i){
			if (tokens[i].Equals(".") || tokens[i].Equals("question") || tokens[i].Equals("neutral") || tokens[i].Equals("q") || tokens[i].Equals("exclamation")) continue;
			
			displayWord.text = tokens[i];
			
			anim.SetTrigger(tokens[i]);
			//Debug.Log(tokens[i] + ": " + anim.GetCurrentAnimatorStateInfo(0).length);
			yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
		 }
	}

}	