using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TeamManagment : MonoBehaviour
{

	private const string nextSceneName = "Game";

	private const string AVAModeText = "You must defeat all the enemy team members. Select your team members now.";

	void Awake(){
		GameObject previousManager = GameObject.Find ("Scene Manager") as GameObject;
		if (previousManager.GetComponent<ModeManagment> ().GetButtonMode ().Equals (
			previousManager.GetComponent<ModeManagment> ().AVAButton)) {
			numberOfMembers = AVASelection.members;
			explanation = AVASelection.explanation;

		} else if (previousManager.GetComponent<ModeManagment> ().GetButtonMode ().Equals (
			previousManager.GetComponent<ModeManagment> ().OKButton)) {
			numberOfMembers = OKSelection.members;
			explanation = OKSelection.explanation;

		} else {
			numberOfMembers = BFSelection.members;
			explanation = BFSelection.explanation;
		}
	}

	private string explanation;
	private int numberOfMembers;
	private List<Unit> teamList;

	public Button HealerButton;
	public Button TankButton;
	public Button DAButton;
	public Button BAButton;

	void Start(){
		teamList = new List<Unit> (numberOfMembers);
	}

	public void AddUnit(Button b){
	}
}

public static class AVASelection {
	public static string explanation = "You must defeat all the enemy team members. Select your team members now.";
	public static int members = 10;
}

public static class OKSelection{
	public static string explanation = "You must destroy an object the enemy team is guarding. Select your team members now";
	public static int members = 10;
}

public static class BFSelection{
	public static string explanation = "You must defeat a powerful enemy. Select your team members now";
	public static int members = 5;
}


