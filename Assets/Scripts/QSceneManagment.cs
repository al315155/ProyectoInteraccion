using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;
using System;

public class QSceneManagment : MonoBehaviour {

//	public GameObject EnemyPrefab;
//	public GameObject PlayerPrefab;
//
//	private List<Unit> playerTeam;
//	private List<Unit> enemyTeam;
//
//	public GameObject grid_unit;
//	public int dimension;
//	private GameObject[,] map;
//
//	private List<GameObject> enemies;
//	private List<GameObject> players;
//
//
//	public bool IsSomeoneNear(Unit unit, List<Unit> team, int range){
//
//		List<Vector2> movementArea = new List<Vector2> ();
//
//		int minX = (int)unit.Position.x - range;
//		if (minX < 0)
//			minX = 0;
//
//		int maxX = (int)unit.Position.x + range;
//		if (maxX > map.GetLength (0))
//			maxX = map.GetLength (0);
//
//		int minY = (int)unit.Position.y - range;
//		if (minY < 0)
//			minY = 0;
//
//		int maxY = (int)unit.Position.y + range;
//		if (maxX > map.GetLength (1))
//			maxX = map.GetLength (1);
//
//
//		for (int i = minX; i < maxX; i++) {
//			for (int j = minY; j < maxY; j++) {
//
//				if (i % 2.0f != 0) {
//					if ((i + j) % 2.0f == 0) {
//
//						movementArea.Add (new Vector2 (i, j));
//					}
//				}
//
//				if (i % 2.0f == 0) {
//					if ((i + j) % 2.0f != 0) {
//
//						movementArea.Add (new Vector2 (i, j));
//					}
//				}
//			}
//		}
//
//		foreach (Vector2 position in movementArea) {
//
//			foreach (Unit u in team) {
//
//				if (u.Position.Equals (position)) {
//
//					return true;
//				}
//			}
//		}
//
//		return false;
//	}
//
//	public bool IsSomeoneFocused(Unit unit){
//
//		if (enemyTeam.Contains (unit)) {
//			foreach (Unit partner in enemyTeam) {
//				if (!unit.Equals (partner)) {
//					if (/*partner.focused*/){
//						return true;
//					}
//				}
//			}
//		} else {
//			foreach (Unit partner in playerTeam) {
//				if (!unit.Equals (partner)) {
//					if (/*partner.focused*/){
//						return true;
//					}
//				}
//			}
//		}
//
//		return false;
//	}
//
//	public bool SomeoneInMeleDamageArea(Unit unit, List<Unit> team, int range){
//
//		List<Vector2> attackArea = new List<Vector2> ();
//
//		int minX = (int)unit.Position.x - range;
//		if (minX < 0)
//			minX = 0;
//
//		int maxX = (int)unit.Position.x + range;
//		if (maxX > map.GetLength (0))
//			maxX = map.GetLength (0);
//
//		int minY = (int)unit.Position.y - range;
//		if (minY < 0)
//			minY = 0;
//
//		int maxY = (int)unit.Position.y + range;
//		if (maxX > map.GetLength (1))
//			maxX = map.GetLength (1);
//
//		// el area es circular a su alrededor (bueno, cuadrado)
//		for (int i = minX; i < maxX; i++){
//			for (int j = minY; j < maxY; j++) {
//				attackArea.Add (new Vector2 (i, j));
//			}
//		}
//
//		foreach (Vector2 position in attackArea) {
//
//			foreach (Unit u in team) {
//
//				if (u.Position.Equals (position)) {
//
//					return true;
//				}
//			}
//		}
//
//		return false;
//
//	}
//
//	public int ALotOfRivalsInMeleDamageArea(Unit unit, List<Unit> team, int range){
//		
//		List<Vector2> attackArea = new List<Vector2> ();
//
//		int minX = (int)unit.Position.x - range;
//		if (minX < 0)
//			minX = 0;
//
//		int maxX = (int)unit.Position.x + range;
//		if (maxX > map.GetLength (0))
//			maxX = map.GetLength (0);
//
//		int minY = (int)unit.Position.y - range;
//		if (minY < 0)
//			minY = 0;
//
//		int maxY = (int)unit.Position.y + range;
//		if (maxX > map.GetLength (1))
//			maxX = map.GetLength (1);
//
//		// el area es circular a su alrededor (bueno, cuadrado)
//		for (int i = minX; i < maxX; i++){
//			for (int j = minY; j < maxY; j++) {
//				attackArea.Add (new Vector2 (i, j));
//			}
//		}
//
//		int rivals = 0;
//
//		foreach (Vector2 position in attackArea) {
//
//			foreach (Unit u in team) {
//
//				if (u.Position.Equals (position)) {
//
//					rivals += 1;
//				}
//			}
//		}
//
//		if (rivals > 1)
//			return true;
//		return false;
//	}
//
//	public List<Unit> GetTeam(Unit unit, string LookInTeam){
//
//		//Si LookInTeam equivale a "Rival" devolverá
//		//el equipo contrario al que la unidad pertenece.
//		if (enemyTeam.Contains (unit)) {
//			if (LookInTeam.Equals ("Rival")) {
//				return playerTeam;
//			} else {
//				return enemyTeam;
//			}
//		} else {
//			if (LookInTeam.Equals ("Rival")) {
//				return enemyTeam;
//			} else {
//				return playerTeam;
//			}
//		}
//	}
//
//	private void PlaceCharacters(List<Unit> team, Vector2[] area){
//		List<Vector2> ocupedPositions = new List<Vector2>();
//		int cont = 0;
//
//		while (ocupedPositions.Count < team.Count) {
//			int x = UnityEngine.Random.Range ((int)area [0].x, (int)area [1].x);
//			int y = UnityEngine.Random.Range ((int)area [0].y, (int)area [1].y);
//
//			Vector2 newPosition = new Vector2 (x, y);
//
//			if (!ocupedPositions.Contains (newPosition)){
//				team [cont].Position = newPosition;
//				ocupedPositions.Add (newPosition);
//				cont++;
//			}
//		}
//	}
//
//	private void CharactersInScene(List<Unit> team, List<GameObject> teamInScene){
//
//		if (team.Equals (enemyTeam)) {
//			EnemyPrefab.gameObject.SetActive (true);
//			foreach (Unit unit in team) {
//				GameObject obj = Instantiate (EnemyPrefab);
//				teamInScene.Add (obj);
//
//				Vector3 positionInScene = map [(int) unit.Position.x, (int) unit.Position.y].transform.position;
//				positionInScene += Vector3.up * 1.5f;
//				obj.transform.position = positionInScene;
//
//				//añadair color por rol
//			}
//			EnemyPrefab.gameObject.SetActive (false);
//		} 
//		else {
//			PlayerPrefab.gameObject.SetActive (true);
//			foreach (Unit unit in team) {
//				GameObject obj = Instantiate (PlayerPrefab);
//				teamInScene.Add (obj);
//
//				Vector3 positionInScene = map [(int) unit.Position.x, (int) unit.Position.y].transform.position;
//				positionInScene += Vector3.up * 1.5f;
//				obj.transform.position = positionInScene;
//
//				//añadair color por rol
//			}
//			PlayerPrefab.gameObject.SetActive (false);
//		}
//	}
//
//	private void DrawArea(Vector2[] area){
//		for (int i = (int) area [0].x; i < (int)area [1].x; i++) {
//
//			for (int j = (int) area [0].y; j < (int) area [1].y; j++) {
//				map [i, j].GetComponent<Renderer>().material.color = Color.blue;
//			}
//		}
//	}
//
//	public Vector2[] GetReferences(int dimension, string team){
//		int sideMargin, topMargin, botMargin;
//		CalculateMargins (dimension, out sideMargin, out topMargin, out botMargin);
//
//		Vector2 topLeft;
//		Vector2 botRight;
//
//		if (team.Equals ("Player")) {
//			topLeft = new Vector2 (sideMargin, topMargin);
//			botRight = new Vector2 ((int) Math.Sqrt(dimension) - sideMargin, (int) Math.Sqrt(dimension) / 2 - botMargin);
//		} else {
//			topLeft = new Vector2 (sideMargin, (int) Math.Sqrt(dimension) / 2 + botMargin);
//			botRight = new Vector2 ((int) Math.Sqrt(dimension) - sideMargin, (int) Math.Sqrt(dimension) - topMargin);
//		}
//		return new Vector2[]{ topLeft, botRight };
//	}
//
//	private void CalculateMargins(int dimension, out int sideMargin, out int topMargin, out int botMargin){
//
//		int normalMargin = (int) Math.Sqrt(dimension) * 20 / 100;
//		sideMargin = normalMargin;
//		topMargin = normalMargin / 2;
//		botMargin = normalMargin;
//	}
//
//	private void GenerateMap(){
//		map = new GameObject[(int) Mathf.Sqrt(dimension), (int) Mathf.Sqrt(dimension)];
//
//		for (int i = 0; i < (int)Mathf.Sqrt (dimension); i++) {
//			for (int j = 0; j < (int)Mathf.Sqrt (dimension); j++) {
//				GameObject newCube = Instantiate (grid_unit);
//				newCube.transform.position = 
//					new Vector3 (-(int)Mathf.Sqrt (dimension) + i * newCube.transform.localScale.x, 
//					0, -(int)Mathf.Sqrt (dimension) + j * newCube.transform.localScale.z);
//				map [i, j] = newCube;
//			}
//		}
//	}
//
//	// Use this for initialization
//	void Start () {
//		GenerateMap ();
//
//		enemyTeam = new List<Unit> ();
//		enemyTeam.Add (new Healer());
//		enemyTeam.Add (new Tank ());
//		enemyTeam.Add (new DistDamage ());
//		enemyTeam.Add (new MeleDamage ());
//
//		playerTeam = new List<Unit> ();
//		playerTeam.Add (new Healer());
//		playerTeam.Add (new Tank ());
//		playerTeam.Add (new DistDamage ());
//		playerTeam.Add (new MeleDamage ());
//
//		enemies = new List<GameObject> ();
//		players = new List<GameObject> ();
//
//		Vector2[] playerArea = GetReferences(dimension, "Player");
//		Vector2[] enemyArea = GetReferences(dimension, "Enemy");
//
//		DrawArea (playerArea);
//		DrawArea (enemyArea);
//
//		PlaceCharacters (playerTeam, playerArea);
//		PlaceCharacters (enemyTeam, enemyArea);
//
//		CharactersInScene (playerTeam, players);
//		CharactersInScene (enemyTeam, enemies);
//		
//	}
//	
//	// Update is called once per frame
//	void Update () {
//		
//	}
}
