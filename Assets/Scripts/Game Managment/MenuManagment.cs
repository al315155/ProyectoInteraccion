using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManagment : MonoBehaviour {

	private const string nextSceneName = "Selection";

	public void StartGame(){
		SceneManager.LoadScene (nextSceneName);
	}

	public void Quit(){
		Application.Quit ();
	}

}
