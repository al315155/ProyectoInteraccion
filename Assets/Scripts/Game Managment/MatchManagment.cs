using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using AssemblyCSharp;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class MatchManagment : MonoBehaviour
{
	private List<String> teamString;
	private List<Unit> playerTeam;
	private List<Unit> enemyTeam;

	public GameObject groud_unit;
	public GameObject grid_unit;
	public int dimension;


	void Awake(){
	//	groud_unit.transform.localScale = new Vector3 (dimension / 2, 1, dimension / 2);

		for (int i = 0; i < dimension / 2; i+=2) {
			for (int j = 0; j < dimension / 2; j+=2) {
				GameObject newCube = Instantiate (grid_unit);
				newCube.transform.position = new Vector3 (-dimension/4 + i, 0, -dimension/4 + j);
			}
		} 
	}


	void Start(){
		playerTeam = new List<Unit> ();
		teamString = GameObject.Find ("Game Manager").GetComponent<GameManager> ().GetTeam ();



//		foreach (string m in teamString) {
//
//			switch (m) {
//			case "Healer":
//				teamUnit.Add (new Healer ());
//				break;
//			case "Tank":
//				teamUnit.Add (new Tank ());
//				break;
//			case "Distance Damage":
//				teamUnit.Add (new DistDamage ());
//
//				break;
//			case "Mele Damage":
//				teamUnit.Add (new MeleDamage ());
//				break;
//			}
//		}


	}
}


