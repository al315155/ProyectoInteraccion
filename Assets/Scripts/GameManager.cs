using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	public void Quit(){
		Application.Quit ();
	}

	public void ChooseGameMode(){
		SceneManager.LoadScene ("Game Mode Selection");
	}

	public void ChooseTeam(){
		if (choosed != null && Input.anyKeyDown) {
			switch (choosed.tag) {
			case AVAtag:
				SceneManager.LoadScene ("Ava Mode");
				break;
			case OKtag:
				SceneManager.LoadScene ("OK Mode");
				break;
			case FBtag:
				SceneManager.LoadScene ("Boss Fight Mode");
				break;
			}
		}
	}
		
	public void ShowDetails(Button b){
		switch (b.tag) {
		case AVAtag:
			AVAButton.GetComponentInChildren<Text> ().text = AVAExpl;
			AVAButton.GetComponentInChildren<Text> ().fontSize = 16;

			OKButton.GetComponentInChildren<Text> ().text = OK;
			OKButton.GetComponentInChildren<Text> ().fontSize = 30;

			FBButton.GetComponentInChildren<Text> ().text = FB;
			FBButton.GetComponentInChildren<Text> ().fontSize = 30;

			break;

		case OKtag:
			OKButton.GetComponentInChildren<Text> ().text = OKExpl;
			OKButton.GetComponentInChildren<Text> ().fontSize = 16;

			AVAButton.GetComponentInChildren<Text> ().text = AVA;
			AVAButton.GetComponentInChildren<Text> ().fontSize = 30;

			FBButton.GetComponentInChildren<Text> ().text = FB;
			FBButton.GetComponentInChildren<Text> ().fontSize = 30;

			break;

		case FBtag:
			FBButton.GetComponentInChildren<Text> ().text = FBExpl;
			FBButton.GetComponentInChildren<Text> ().fontSize = 16;

			AVAButton.GetComponentInChildren<Text> ().text = AVA;
			AVAButton.GetComponentInChildren<Text> ().fontSize = 30;

			OKButton.GetComponentInChildren<Text> ().text = OK;
			OKButton.GetComponentInChildren<Text> ().fontSize = 30;

			break;
		}

		choosed = b;
	}

	void Start(){
		choosed = null;
	}

	void Update(){
		ChooseTeam ();
	}

	private Button choosed;
	public Button AVAButton;
	public Button OKButton;
	public Button FBButton;

	public const string 
	AVAtag = "AVA", 
	OKtag = "OK", 
	FBtag = "FB";

	private const string AVA = "ALL\nVERSUS\nALL";
	private const string AVAExpl = "All vs All:\nDefeat all the members of the enemy team in order to win.";
	private const string OK = "ONE\nKILL";
	private const string OKExpl = "One Kill:\nYou must destroy an object the enemy team is guarding in order to win.";
	private const string FB = "BOSS\nFIGHT";
	private const string FBExpl = "Boss Fight:\nYou must defeat an only enemy with great power in order to win.";
}
