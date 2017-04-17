using System;
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Collections;
using UnityEngine.UI;
using System.Threading;
using System.Linq;

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
		string[] vocabulary = {"aj", "hkust","hk","ust", "onur"};

		var j = tokens.Length;
		for (var i=0; i<j;++i){
			if (vocabulary.Contains(tokens[i])){
				string[] letter = tokens[i].ToCharArray().Select( c => c.ToString()).ToArray();
				List<string> myList = tokens.ToList();
				myList.RemoveAt(i);
				myList.InsertRange(i, letter);
				tokens = myList.ToArray();
				j+= letter.Length-1;
				i+= letter.Length-1;
			}
		}	
		StartCoroutine(waitedAnimate(tokens));
	}
	
	IEnumerator waitedAnimate(string[] tokens) {

		string[] vocabulary = {"aj", "hkust","hk","ust", "onur"};
		
		Text displayWord = GameObject.Find("/Canvas/DisplayWord").GetComponent<Text>();
				
		for (var i=0; i<tokens.Length;++i){
			if (tokens[i].Equals("neutral") || tokens[i].Equals("question")|| tokens[i].Equals("exclamation")) continue;
			else if (tokens[i].Equals(".") ||  tokens[i].Equals("q")) {
				displayWord.text = "<pause>";
				anim.SetTrigger("idle");
				yield return new WaitForSeconds(2f);
			}
			else {
				displayWord.text = tokens[i];
				anim.SetTrigger(tokens[i]);
				yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
				}
			}
		displayWord.text = " ";
		anim.SetTrigger("idle");
		yield return new WaitForSeconds(2f);

	}
}	