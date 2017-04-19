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
		string[] nouns = {"aj", "hkust","hk","ust", "onur", "akanksha", "arslan", "sheetal", "mak", "nevin", "brian", "professor", "doctor", "zhang"};
		string vocabulary = "NEUTRAL, QUESTION, EXCLAMATION, YOU, MORE, EAT, MOM, DAD, THANK, HI, WITH, PLAY, MUSIC, MOVIE, LIKE, FOOTBALL, YEAR, OUR, SAY, STUDY, SEE, WHERE, THINK, NEXT, NAME, ME, MY, INDIA, IT, HAVE, COME, THIS, PAST, GO, GOOD, IN, RAIN, RAINY, US, THEY, ONE, TWO, THREE, FOUR, FIVE, SIX, SEVEN, EIGHT, NINE, TEN";
		string[] stringSeparators = new string[] {", "};
		string[] vocab = vocabulary.ToLower().Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries);
		
		var j = tokens.Length;

		for (var i=0; i<j;++i){
			Debug.Log(vocab.Contains(tokens[i]));
			if (nouns.Contains(tokens[i]) || vocab.Contains(tokens[i])==false){
				string[] letter = tokens[i].ToCharArray().Select( c => c.ToString()).ToArray();
				List<string> myList = tokens.ToList();
				myList.RemoveAt(i);
				myList.InsertRange(i, letter);
				tokens = myList.ToArray();
				j+= letter.Length-1;
				i+= letter.Length-1;
			}
		}	
		for (var i=0; i<j;++i){Debug.Log(tokens[i]);}
		StartCoroutine(waitedAnimate(tokens));
	}
	
	IEnumerator waitedAnimate(string[] tokens) {
		
		Text displayWord = GameObject.Find("/Canvas/DisplayWord").GetComponent<Text>();
		var i = 0;
		for (i=0; i<tokens.Length;++i){
			if (tokens[i].Equals("neutral") || tokens[i].Equals("question")|| tokens[i].Equals("exclamation")||tokens[i].Equals(".") ||  tokens[i].Equals("q")) {
				if (i!=0){
					displayWord.text = " ";
					yield return new WaitForSeconds(1f);
				}
			}
			else {
				displayWord.text = tokens[i];
				anim.SetTrigger(Animator.StringToHash(tokens[i]));
				yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
				}
			}
		if (i == tokens.Length){
			displayWord.text = " ";
			anim.SetTrigger(Animator.StringToHash("idle"));
			yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
		}
	}
}	