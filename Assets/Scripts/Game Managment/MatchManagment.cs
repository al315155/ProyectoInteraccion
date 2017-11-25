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
	public GameObject EnemyPrefab;
	public GameObject PlayerPrefab;

	public GameObject sphere;

	private GameManager gameManager;

	private List<String> teamString;
	private List<Unit> playerTeam;
	private List<Unit> enemyTeam;

	public GameObject groud_unit;
	public GameObject grid_unit;
	public int dimension;
	private GameObject[,] map;

	private List<GameObject> enemies;
	private List<GameObject> players;

	void Awake(){
		
		map = new GameObject[(int) Mathf.Sqrt(dimension), (int) Mathf.Sqrt(dimension)];

		for (int i = 0; i < (int) Mathf.Sqrt(dimension); i++) {
			for (int j = 0; j < (int) Mathf.Sqrt(dimension); j++) {
				GameObject newCube = Instantiate (grid_unit);
				newCube.transform.position = 
					new Vector3 (-(int) Mathf.Sqrt(dimension) + i * newCube.transform.localScale.x, 
						0, -(int) Mathf.Sqrt(dimension) + j * newCube.transform.localScale.z);
				map [i, j] = newCube;
			}
		} 
	}


	void Start(){
		enemies = new List<GameObject> ();
		players = new List<GameObject> ();

		gameManager = GameObject.Find ("Game Manager").GetComponent<GameManager> ();

		playerTeam = Unit.GenerateTeam(gameManager.GetTeam());
		Vector2[] playerArea = gameManager.GetGameMode().GetReferences(dimension, "Player");

		enemyTeam = Unit.GenerateTeam (gameManager.GetGameMode ().GetEnemyTeam ());
		Vector2[] enemyArea = gameManager.GetGameMode().GetReferences(dimension, "Enemy");

		DrawArea (playerArea);
		DrawArea (enemyArea);

		PlaceCharacters (playerTeam, playerArea);
		PlaceCharacters (enemyTeam, enemyArea);

		CharactersInScene (playerTeam, players);
		CharactersInScene (enemyTeam, enemies);
	}

	private void DrawArea(Vector2[] area){
		for (int i = (int) area [0].x; i < (int)area [1].x; i++) {

			for (int j = (int) area [0].y; j < (int) area [1].y; j++) {
				map [i, j].GetComponent<Renderer>().material.color = Color.blue;
			}
		}
	}

	private void PlaceCharacters(List<Unit> team, Vector2[] area){
		List<Vector2> ocupedPositions = new List<Vector2>();
		int cont = 0;

		while (ocupedPositions.Count < gameManager.GetGameMode ().Members) {
			int x = UnityEngine.Random.Range ((int)area [0].x, (int)area [1].x);
			int y = UnityEngine.Random.Range ((int)area [0].y, (int)area [1].y);

			Vector2 newPosition = new Vector2 (x, y);

			if (!ocupedPositions.Contains (newPosition)){
				team [cont].Position = newPosition;
				ocupedPositions.Add (newPosition);
				cont++;
			}
		}
	}

	private void CharactersInScene(List<Unit> team, List<GameObject> teamInScene){

		if (team.Equals (enemyTeam)) {
			EnemyPrefab.gameObject.SetActive (true);
			foreach (Unit unit in team) {
				GameObject obj = Instantiate (EnemyPrefab);
				teamInScene.Add (obj);

				Vector3 positionInScene = map [(int) unit.Position.x, (int) unit.Position.y].transform.position;
				positionInScene += Vector3.up * 1.5f;
				obj.transform.position = positionInScene;
			}
			EnemyPrefab.gameObject.SetActive (false);
		} 
		else {
			PlayerPrefab.gameObject.SetActive (true);
			foreach (Unit unit in team) {
				GameObject obj = Instantiate (PlayerPrefab);
				teamInScene.Add (obj);

				Vector3 positionInScene = map [(int) unit.Position.x, (int) unit.Position.y].transform.position;
				positionInScene += Vector3.up * 1.5f;
				obj.transform.position = positionInScene;
			}
			PlayerPrefab.gameObject.SetActive (false);
		}
	}
}


