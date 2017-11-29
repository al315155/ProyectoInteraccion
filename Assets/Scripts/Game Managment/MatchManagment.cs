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
	private List<Unit> round;
	private int turn;

	public GameObject enemyCanvas;
	public GameObject playerCanvas;

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

		round = GetTurns ();
		turn = 0;
		NextTurn ();
	}

	private void NextTurn(){

		if (enemyTeam.Contains (round [turn])) {

			Debug.Log ("soy enemigo");
			enemyCanvas.gameObject.SetActive (true);
			playerCanvas.gameObject.SetActive (false);

			enemyCanvas.transform.GetChild(0).transform.Find ("Unit Icon").GetComponent<Image> ().color = enemies[enemyTeam.IndexOf(round[turn])].GetComponent<Renderer>().material.color;
		
			enemyCanvas.transform.GetChild(0).transform.Find ("Unit Rol Name").GetComponent<Text> ().text = round [turn].UnitRol.ToString();

			//cambio la vida (scrollbar)
			//cambio el número de vida
		} 
		else {
			Debug.Log ("soy amigo");

			enemyCanvas.gameObject.SetActive (false);
			playerCanvas.gameObject.SetActive (true);

			playerCanvas.transform.GetChild(0).transform.Find ("Unit Icon").GetComponent<Image> ().color = players[playerTeam.IndexOf(round[turn])].GetComponent<Renderer>().material.color;

			playerCanvas.transform.GetChild(0).transform.Find ("Unit Rol Name").GetComponent<Text> ().text = round [turn].UnitRol.ToString();

			//cambio la vida (scrollbar)
			//cambio el número de vida
		}

		turn++;

		if (turn >= round.Count) {
			turn = 0;
		}
	}

	private void DrawArea(Vector2[] area){
		for (int i = (int) area [0].x; i < (int)area [1].x; i++) {

			for (int j = (int) area [0].y; j < (int) area [1].y; j++) {
				map [i, j].GetComponent<Renderer>().material.color = Color.cyan;
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

	private List<GameObject> AllowMovement(Unit unit, int maxR){

		List<GameObject> cubes = new List<GameObject> ();

		Vector2 pos = unit.Position;

		int minX = (int) unit.Position.x - maxR;
		if (minX < 0) {
			minX = 0;
		}

		int maxX = (int) unit.Position.x + maxR;
		if (maxX >= map.GetLength (0)) {
			maxX = map.GetLength (0);
		}

		int minY = (int) unit.Position.y - maxR;
		if (minY < 0) {
			minY = 0;
		}

		int maxY = (int) unit.Position.y + maxR;
		if (maxY >= map.GetLength (0)) {
			maxY = map.GetLength (0);
		}

		for (int i = minX; i < maxX; i++) {
			for (int j = minY; j < maxY; j++) {

				if (i < (int) unit.Position.x) {
					if (j + i == (int) unit.Position.x) {
						cubes.Add (map [i, j]);
					}
				}

				if (i > (int) unit.Position.x) {
					if (j - i == (int) unit.Position.x) {
						cubes.Add (map [i, j]);
					}
				}
			}
		}

		return cubes;
	}

	private void DrawMovement(List<GameObject> cubes){

		foreach (GameObject cube in cubes) {
			cube.GetComponent<Renderer> ().material.color = new Vector4 (0.5f, 0.3f, 0.75f, 1f);
		}

	}

	private void CharactersInScene(List<Unit> team, List<GameObject> teamInScene){

		GameObject obj;

		if (team.Equals (enemyTeam)) {
			obj = EnemyPrefab;
		} else {
			obj = PlayerPrefab;
		}

		foreach (Unit unit in team) {

			GameObject copy = Instantiate (obj);
			teamInScene.Add (copy);
			
			Vector3 positionInScene = map [(int)unit.Position.x, (int)unit.Position.y].transform.position;
			positionInScene += Vector3.up * 1.5f;
			copy.transform.position = positionInScene;

			switch (unit.UnitRol) {
			case Rol.Boss:
				copy.GetComponent<Renderer> ().material.color = Color.gray;
			
				break;
			case Rol.Distance:
				copy.GetComponent<Renderer> ().material.color = Color.yellow;
				break;
			
			case Rol.Healer:
				copy.GetComponent<Renderer> ().material.color = Color.green;
				break;
			
			case Rol.Mele:
				copy.GetComponent<Renderer> ().material.color = Color.red;
				break;
			
			case Rol.Tank:					
				copy.GetComponent<Renderer> ().material.color = Color.blue;
				break;
			}
		}
	}

	public List<Unit> GetTurns(){

		List<Unit> turns = new List<Unit> ();

		for (int i = 0; i < enemyTeam.Count; i++) {
			turns.Add (enemyTeam [i]);
		}

		for (int i = 0; i < playerTeam.Count; i++) {
			turns.Add (playerTeam [i]);
		}

		for (int i = 0; i < turns.Count; i++) {
			int max = i;

			for (int j = i; j < turns.Count; j++) {

				if (turns [j].Velocity > turns [max].Velocity) {
					max = j;
				}
			}

			Unit aux = turns [i];
			turns [i] = turns [max];
			turns [max] = aux;
		}

		return turns;
	}

	private void DrawRange(int max){


	}

	public void Move(){

		List<GameObject> lista = AllowMovement (round [turn], round [turn].Movement);
		DrawMovement (lista);
	}

	public void Attack(){
		NextTurn ();
	}

	public void Hability(){		
		NextTurn ();	
	}

	public void EndTurn(){
		NextTurn ();
	}
}


