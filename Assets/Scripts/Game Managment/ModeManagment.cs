using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ModeManagment : MonoBehaviour {

	private const string nextSceneName = "Team";

	public 	Button controlButton;
	private Button currentButton;
	private Button lastButton;
	public 	Button AVAButton;
	public 	Button OKButton;
	public 	Button BFButton;

	private const string AVA 		= "ALL\nVERSUS\nALL";
	private const string AVAExpl 	= "All vs All:\nDefeat all the members of the enemy team in order to win.";
	private const string OK 		= "ONE\nKILL";
	private const string OKExpl 	= "One Kill:\nYou must destroy an object the enemy team is guarding in order to win.";
	private const string BF 		= "BOSS\nFIGHT";
	private const string BFExpl 	= "Boss Fight:\nYou must defeat an only enemy with great power in order to win.";

	void Awake(){
		DontDestroyOnLoad (transform.gameObject);
	}

	void Start(){
		currentButton 	= controlButton;
		lastButton 		= null;
	}

	void Update(){
		if (!currentButton.Equals(controlButton) && Input.GetKeyDown (KeyCode.Return)) {
			SceneManager.LoadScene (nextSceneName);
		}
	}

	public Button GetButtonMode(){
		return currentButton;
	}

	public void ShowDetails(Button b){
		ActualizeButton (b);
	}

	private void ActualizeButton(Button button){
		if (lastButton != currentButton) {
			lastButton 		= currentButton;
			currentButton 	= button;
			ActualizeText ();
		}
	}

	private void ActualizeText(){

		if (currentButton.Equals (AVAButton)) {
			currentButton.GetComponentInChildren<Text> ().text = AVAExpl;
			if (lastButton.Equals (OKButton)) {
				lastButton.GetComponentInChildren<Text> ().text = OK;
			} else if (lastButton.Equals (BFButton)){
				lastButton.GetComponentInChildren<Text> ().text = BF;
			}
		} else if (currentButton.Equals (OKButton)) {
			currentButton.GetComponentInChildren<Text> ().text = OKExpl;

			if (lastButton.Equals (AVAButton)) {
				lastButton.GetComponentInChildren<Text> ().text = AVA;
			} else if (lastButton.Equals (BFButton)){
				lastButton.GetComponentInChildren<Text> ().text = BF;
			}
		} else {
			currentButton.GetComponentInChildren<Text> ().text = BFExpl;
			if (lastButton.Equals (AVAButton)) {
				lastButton.GetComponentInChildren<Text> ().text = AVA;
			} else if (lastButton.Equals (OKButton)){
				lastButton.GetComponentInChildren<Text> ().text = OK;
			}
		}
		currentButton.GetComponentInChildren<Text> ().fontSize 	= 18;
		lastButton.GetComponentInChildren<Text> ().fontSize 	= 30;
	}
}
