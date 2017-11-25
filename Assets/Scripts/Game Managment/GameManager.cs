using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using AssemblyCSharp;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class GameManager : MonoBehaviour {

	private GameMode gameMode;
	private String archiveName; 

	public List<string> TeamMembers;
	private String teamrute;

	public static GameManager gameManager;

	const string 	TeamVSTeam_mode_title = "TEAM VS TEAM";
	const string 	TeamVSTeam_mode_explanation = "You must defeat all the enemy team members.";
	const int 		TeamVSTeam_mode_members = 6;
	const string 	OneKill_mode_title = "ONE KILL";
	const string 	OneKill_mode_explanation = "You must destroy an object the enemy team is guarding.";
	const int 		OneKill_mode_members = 6;
	const string 	BossFight_mode_title = "BOSS FIGHT";
	const string 	BossFight_mode_explanation = "You must defeat a powerful enemy.";
	const int 		BossFight_mode_members = 3;

//	public int currentGameMode = -1;

	private GameMode TeamVsTeam;
	private GameMode OneKill;
	private GameMode BossFight;


	void Awake(){
		archiveName = Application.persistentDataPath + "/data.txt";
		teamrute = Application.persistentDataPath + "/team.txt";

		if (gameManager == null) {
			gameManager = this;
			DontDestroyOnLoad (this.gameObject);
		} else if (gameManager != this) {
			Destroy (gameObject);
		}
	}

	void Start(){
		TeamVsTeam 	= new GameMode (TeamVSTeam_mode_title, 	TeamVSTeam_mode_explanation, 	GameType.Team_vs_Team, 	TeamVSTeam_mode_members);
		OneKill 	= new GameMode (OneKill_mode_title, 	OneKill_mode_explanation, 		GameType.One_Kill, 		OneKill_mode_members);
		BossFight 	= new GameMode (BossFight_mode_title,	BossFight_mode_explanation, 	GameType.Boss_Fight,	BossFight_mode_members);
	//	ChargeGameMode ();
		ChargeTeamConfiguration ();
	}

	void Update(){
	}

	public GameMode GetTeamVsTeamMode(){
		return TeamVsTeam;
	}
	public GameMode GetOneKillMode(){
		return OneKill;
	}
	public GameMode GetBossFightMode(){
		return BossFight;
	}
//
	public void SetGameMode(GameMode gameMode){
		this.gameMode = gameMode;
	}

	public GameMode GetGameMode(){
		return gameMode;
	}

	public void SetTeamConfiguration(List<string> team){
		TeamMembers = team;
		SaveTeamConfiguration ();
	}

	public List<String> GetTeam(){
		return TeamMembers;
	}

	public void SaveTeamConfiguration(){
		Debug.Log ("entro");
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Create (teamrute);

		KeepData fgfdgwedf = new KeepData (this.TeamMembers);

		bf.Serialize (file, fgfdgwedf);

		file.Close ();
	}

	public void ChargeTeamConfiguration(){
		if (File.Exists (teamrute)) {
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Open (teamrute, FileMode.Open);
			Debug.Log (bf.Deserialize (file));

			KeepData aaaadadads = (KeepData)bf.Deserialize (file);
			TeamMembers = aaaadadads.members;

			file.Close ();
		} else {
			TeamMembers = null;
		}
	}

//	public void SaveGameMode(){
//		BinaryFormatter bf = new BinaryFormatter ();
//		FileStream file = File.Create (archiveName);
//
//		KeepData fgfdgwedf = new KeepData (this.currentGameMode);
//
//		bf.Serialize (file, fgfdgwedf);
//
//		file.Close ();
//	}

//	public void ChargeGameMode(){
//		if (File.Exists (archiveName)) {
//			BinaryFormatter bf = new BinaryFormatter ();
//			FileStream file = File.Open (archiveName, FileMode.Open);
//			Debug.Log (bf.Deserialize (file));
//
//			KeepData aaaadadads = (KeepData)bf.Deserialize (file);
//
//			currentGameMode = aaaadadads.gameMode;
//
//			file.Close ();
//		} else {
//			currentGameMode = -1;
//		}
//	}
}


[Serializable]
public class KeepData{
	//public int gameMode;
	public List<string> members;

//	public KeepData(int gameMode){
//		this.gameMode = gameMode;
//	}

	public KeepData( List<string> members){
		this.members = members;
	}
}
