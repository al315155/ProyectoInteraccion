using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using AssemblyCSharp;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;
using UnityEngine.EventSystems;


public class MatchManagment : MonoBehaviour
{
	public AssemblyCSharp.Action currentAction;
	public Drawer drawer;

	public GameObject target;

	public List<Vector2> allowedBoxes;

	public Material originalGround;

	public GameObject EnemyPrefab;
	public GameObject PlayerPrefab;

	private GameManager gameManager;

	private List<String> teamString;
	private List<Unit> playerTeam;
	private List<Unit> enemyTeam;

	public GameObject grid_unit;
	public int dimension;
	private GameObject[,] map;

	private List<GameObject> enemies;
	private List<GameObject> players;
	public List<Unit> round;
	public int turn;

	public GameObject enemyCanvas;
	public GameObject playerCanvas;

	public GameObject focusedIcon;

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
			
		Destroy (grid_unit);
	}


	void Start(){

		drawer = GetComponent<Drawer> ();

		currentAction = AssemblyCSharp.Action.None;
		allowedBoxes = new List<Vector2> ();

		enemies = new List<GameObject> ();
		players = new List<GameObject> ();

		gameManager = GameObject.Find ("Game Manager").GetComponent<GameManager> ();

		playerTeam = Unit.GenerateTeam(gameManager.GetTeam());
		Vector2[] playerArea = gameManager.GetGameMode().GetReferences(dimension, "Player");

		enemyTeam = Unit.GenerateTeam (gameManager.GetGameMode ().GetEnemyTeam ());
		Vector2[] enemyArea = gameManager.GetGameMode().GetReferences(dimension, "Enemy");

		PlaceCharacters (playerTeam, playerArea);
		PlaceCharacters (enemyTeam, enemyArea);

		CharactersInScene (playerTeam, players);
		CharactersInScene (enemyTeam, enemies);

		round = GetTurns ();
		turn = -1;


		NextTurn ();
	}

	private void NextTurn(){


		turn++;

		if (turn >= round.Count) {
			turn = 0;
		}	

		Debug.Log (round [turn].UnitRol);
		Debug.Log (round [turn].Movement);

		foreach (Unit unit in enemyTeam) {
			if (unit.Focused) {
				Debug.Log (unit.FocusedCount);
				unit.FocusedCount = unit.FocusedCount - 1;

				if (unit.FocusedCount <= 0) {
					unit.Focused = false;

					Destroy(enemies [enemyTeam.IndexOf (unit)].transform.GetChild (0).gameObject);
				}
			}
		}

		foreach (Unit unit in playerTeam) {
			if (unit.Focused) {
				unit.FocusedCount = unit.FocusedCount - 1;

				if (unit.FocusedCount <= 0) {
					unit.Focused = false;

					Destroy(players [playerTeam.IndexOf (unit)].transform.GetChild (0).gameObject);
				}
			}
		}

		drawer.UnDrawBoxes (map, allowedBoxes);
		currentAction = AssemblyCSharp.Action.None;

		if (enemyTeam.Contains (round [turn])) {

			Debug.Log ("Soy enemigo");

			enemyCanvas.SetActive (true);
			playerCanvas.gameObject.SetActive (false);


			Unit enemy = round [turn];
			GameObject enemyInScene = enemies[enemyTeam.IndexOf (enemy)];
		
			enemyCanvas.transform.GetChild(0).transform.Find ("Unit Icon").GetComponent<Image> ().color = enemyInScene.GetComponent<Renderer>().material.color;
			enemyCanvas.transform.GetChild(0).transform.Find ("Unit Rol Name").GetComponent<Text> ().text = enemy.UnitRol.ToString();

			float value = (float) round [turn].CurrentLife / (float) round [turn].Life;
			enemyCanvas.transform.GetChild (0).transform.Find ("Unit Scrollbar Life").GetComponent<Scrollbar> ().size = value;
			enemyCanvas.transform.GetChild (0).transform.Find ("Life Label").GetComponent<Text> ().text = round [turn].CurrentLife + "/" + round [turn].Life;

			target.transform.position = new Vector3 (enemyInScene.transform.position.x, enemyInScene.transform.position.y + 5f, enemyInScene.transform.position.z);
		} 
		else {
			Debug.Log ("soy amigo");

			enemyCanvas.gameObject.SetActive (false);
			playerCanvas.gameObject.SetActive (true);


			Unit player = round [turn];
			GameObject playerInScene = players[playerTeam.IndexOf (player)];


			playerCanvas.transform.GetChild(0).transform.Find ("Unit Icon").GetComponent<Image> ().color = playerInScene.GetComponent<Renderer>().material.color;

			playerCanvas.transform.GetChild(0).transform.Find ("Unit Rol Name").GetComponent<Text> ().text = player.UnitRol.ToString();


			float value = (float) round [turn].CurrentLife / (float) round [turn].Life;
			playerCanvas.transform.GetChild (0).transform.Find ("Unit Scrollbar Life").GetComponent<Scrollbar> ().size = value;
			playerCanvas.transform.GetChild (0).transform.Find ("Life Label").GetComponent<Text> ().text = round [turn].CurrentLife + "/" + round [turn].Life;

			target.transform.position = new Vector3 (playerInScene.transform.position.x, playerInScene.transform.position.y + 5f, playerInScene.transform.position.z);
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

	private List<Vector2> allowMovement(List<Vector2> positions){

		List<Unit> units = new List<Unit> ();
		foreach (Unit unit in playerTeam) {
			units.Add (unit);
		}
		foreach (Unit unit in enemyTeam) {
			units.Add (unit);
		}

		List<Vector2> finalPositions = new List<Vector2>();
		foreach (Vector2 position in positions) {
			finalPositions.Add (position);
		}

		foreach (Vector2 position in positions) {
			foreach (Unit unit in units) {
				if (unit.Position.Equals (position)) {
					finalPositions.Remove (position);
					break;
				}
			}
		}

		return finalPositions;
	}
		
	private List<Vector2> getBoxesInsideRange(Unit unit, int range){
		List<Vector2> positions = new List<Vector2> ();
		Vector2 pos = unit.Position;

		int minX = (int)unit.Position.x - range;
		int maxX = (int)unit.Position.x + range;
		int minY = (int)unit.Position.y - range;
		int maxY = (int)unit.Position.y + range;

		if (minX < 0)
			minX = 0;
		if (maxX >= map.GetLength (0))
			maxX = map.GetLength (0);

		if (minY < 0)
			minY = 0;
		if (maxY >= map.GetLength (1))
			maxY = map.GetLength (1);

		int aux = 0;
		for (int i = minX; i < maxX; i++) {
			for (int j = 0; j < aux; j++) {
				Vector2 newPosition = Vector2.one;

				if ((pos.y + j) < maxY) {
					newPosition = new Vector2 (i, (int)pos.y + j);
					positions.Add (newPosition);
				}
				if ((pos.y - j) >= minY) {
					newPosition = new Vector2 (i, (int)pos.y - j);
					positions.Add (newPosition);
				}
			}

			if (i < (int)pos.x) {
				aux++;
			} else {
				aux--;
			}
		}

		return positions;
	}

	private void CharactersInScene(List<Unit> team, List<GameObject> teamInScene){

		GameObject obj;

		if (team.Equals (enemyTeam)) {
			obj = EnemyPrefab;
		} else {
			obj = PlayerPrefab;
		}

		foreach (Unit unit in team) {

			Debug.Log (unit.UnitRol);
			Debug.Log (unit.Movement);

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

		//Creo lista vacía
		List<Unit> turns = new List<Unit> ();

		//La lleno con las unidades del equipo enemigo
		for (int i = 0; i < enemyTeam.Count; i++) {
			turns.Add (enemyTeam [i]);
		}

		//Y con las del equipo del jugador
		for (int i = 0; i < playerTeam.Count; i++) {
			turns.Add (playerTeam [i]);
		}

		//Ordeno la lista de mayor a menor con búsqueda binaria
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

	void Update(){


		if (currentAction.Equals(AssemblyCSharp.Action.Move) && Input.GetMouseButton(0)) {
			DoMove ();
		}

		if (currentAction.Equals (AssemblyCSharp.Action.Attack) && Input.GetMouseButton (0)) {
			DoAttack ();
		}

		if (currentAction.Equals (AssemblyCSharp.Action.Hability) && Input.GetMouseButton (0)) {

			switch (round [turn].UnitRol) {
			case Rol.Healer:
				Heal ();

				break;
			case Rol.Distance:
				Focus ();

				break;
			case Rol.Mele:
				Area ();

				break;
			case Rol.Tank:
				Agro ();

				break;
			}
		}
	}

	public void Move(){

		drawer.UnDrawBoxes (map, allowedBoxes);

		allowedBoxes = allowMovement(getBoxesInsideRange(round[turn], round[turn].Movement + 1));
		drawer.DrawBoxes (map, allowedBoxes, new Vector4(1.0f, 0.2f, 0.7f, 1.0f));
		currentAction = AssemblyCSharp.Action.Move;
	}

	public void DoMove(){
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hitInfo;
		Vector2 newPosition = new Vector2 (-1, -1);

		if (Physics.Raycast (ray, out hitInfo)) {
			if (hitInfo.collider.tag.Equals ("Ground")) {

				//Look for position inside map
				for (int i = 0; i < map.GetLength (0); i++) {
					for (int j = 0; j < map.GetLength (1); j++) {

						if (map [i, j].Equals (hitInfo.collider.gameObject)) {
							newPosition = new Vector2 (i, j);
						}
					}
				}
			}
		}

		if (newPosition != new Vector2 (-1, -1) && allowedBoxes.Contains(newPosition)) {
			round [turn].Position = newPosition;

			GameObject unitInScene;
			if (enemyTeam.Contains (round [turn])) {
				unitInScene = enemies [enemyTeam.IndexOf (round [turn])];	
			} else {
				unitInScene = players [playerTeam.IndexOf (round [turn])];
			}

			Vector3 positionInScene = map [(int)round [turn].Position.x, (int)round [turn].Position.y].transform.position;
			positionInScene += Vector3.up * 1.5f;
			unitInScene.transform.position = positionInScene;

			NextTurn ();
		}
	}

	public void Attack(){

		drawer.UnDrawBoxes (map, allowedBoxes);

		allowedBoxes = getBoxesInsideRange(round[turn], round[turn].AttackRange + 1);
		drawer.DrawBoxes (map, allowedBoxes, new Vector4(0.5f, 0.5f, 0.8f, 1f));
		currentAction = AssemblyCSharp.Action.Attack;
	}

	public void DoAttack(){
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hitInfo;

		if (Physics.Raycast (ray, out hitInfo)) {
			if (hitInfo.collider.tag.Equals ("Enemy") || hitInfo.collider.tag.Equals ("Player")) {

				GameObject unitInScene = hitInfo.collider.gameObject;
				Unit unit;
				if (unitInScene.tag.Equals ("Enemy")) {
					unit = enemyTeam [enemies.IndexOf (unitInScene)];
				} else {
					unit = playerTeam [players.IndexOf (unitInScene)];
				}

				if (allowedBoxes.Contains(unit.Position) && allowedAttack(round[turn], unit)){

					//Agilidad del atacado (si hay o no ataque)
					//La precisión por delante es de 20, por el lado de 70, y po detras de 90
					//Lo de la precisión segun por delante o detrás vamos a esperar xDDDDDD

					float probability = UnityEngine.Random.Range (0, 100);
					if (unit.Focused) {
						if (probability < 100 - unit.Agility) {
							Debug.Log ("fallo");
							NextTurn ();

						} else {
							Debug.Log ("ataco");
							//critico??
							probability = UnityEngine.Random.Range (0, 100);
							if (probability > round [turn].Critic / 2) {
								//critico
								Debug.Log ("critico");
								unit.CurrentLife -= round [turn].Damage * 2;
							} else {
								Debug.Log ("ataqueNormal");
								unit.CurrentLife -= round [turn].Damage;
							}
							NextTurn ();
						}
					} else {
						if (probability < 100 - unit.Agility) {
							Debug.Log ("fallo");
							NextTurn ();

						} else {
							Debug.Log ("ataco");
							//critico??
							probability = UnityEngine.Random.Range (0, 100);
							if (probability > round [turn].Critic) {
								//critico
								Debug.Log ("critico");
								unit.CurrentLife -= round [turn].Damage * 2;
							} else {
								Debug.Log ("ataqueNormal");
								unit.CurrentLife -= round [turn].Damage;
							}
							NextTurn ();
						}
					}
				
				}

			}
		}
	}

	private bool allowedAttack(Unit attacker, Unit victim){
		if (enemyTeam.Contains (attacker) && playerTeam.Contains (victim) ||
		    playerTeam.Contains (attacker) && enemyTeam.Contains (victim)) {
			return true;
		} else {
			return false;
		}
	}

	public void Hability(){		

		drawer.UnDrawBoxes (map, allowedBoxes);

		switch (round [turn].UnitRol) {
		case Rol.Healer:
			allowedBoxes = getBoxesInsideRange (round [turn], round [turn].HabilityRange + 1);
			drawer.DrawBoxes (map, allowedBoxes, new Vector4(0.7f, 0.3f, 0.8f, 1f));
			currentAction = AssemblyCSharp.Action.Hability;

			break;
		case Rol.Distance:
			allowedBoxes = getBoxesInsideRange (round [turn], round [turn].HabilityRange + 1);
			drawer.DrawBoxes (map, allowedBoxes, new Vector4(0.2f, 0.3f, 0.2f, 1f));
			currentAction = AssemblyCSharp.Action.Hability;

			break;
		case Rol.Mele:
			break;
		case Rol.Tank:
			break;
		}

	}

	public void EndTurn(){
		NextTurn ();
	}

	private void Heal(){
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hitInfo;

		if (Physics.Raycast (ray, out hitInfo)) {
			if (hitInfo.collider.tag.Equals ("Enemy") || hitInfo.collider.tag.Equals ("Player")) {

				GameObject unitInScene = hitInfo.collider.gameObject;
				Unit unit;
				if (unitInScene.tag.Equals ("Enemy")) {
					unit = enemyTeam [enemies.IndexOf (unitInScene)];
				} else {
					unit = playerTeam [players.IndexOf (unitInScene)];
				}

				if (allowedBoxes.Contains(unit.Position) && !allowedAttack(round[turn], unit)){

					float probability = UnityEngine.Random.Range (0, 100);
					if (probability < round [turn].HabilityCritic) {
						Debug.Log ("cura critica");
						unit.CurrentLife += round [turn].GetHeal (unit) * 2;

					} else {
						Debug.Log ("cura normal");
						unit.CurrentLife += round [turn].GetHeal (unit);
					}
					NextTurn ();
				}
			}
		}
	}

	private void Focus(){
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hitInfo;

		if (Physics.Raycast (ray, out hitInfo)) {
			if (hitInfo.collider.tag.Equals ("Enemy") || hitInfo.collider.tag.Equals ("Player")) {

				GameObject unitInScene = hitInfo.collider.gameObject;
				Unit unit;
				if (unitInScene.tag.Equals ("Enemy")) {
					unit = enemyTeam [enemies.IndexOf (unitInScene)];
				} else {
					unit = playerTeam [players.IndexOf (unitInScene)];
				}

				if (allowedBoxes.Contains(unit.Position) && allowedAttack(round[turn], unit)){

					if (!unit.Focused) {
						float probability = UnityEngine.Random.Range (0, 100);
						if (probability > 100 - unit.Agility) {
							Debug.Log ("fallo focus");
							NextTurn ();

						} else {
							Debug.Log ("acierto focus");
							unit.Focused = true;
							unit.FocusedCount = 3;

							focusedIcon.SetActive (true);
							GameObject focusedUnit = Instantiate (focusedIcon);
							focusedIcon.SetActive (false);

							focusedUnit.transform.position = unitInScene.transform.position + Vector3.up * 2;
							focusedUnit.transform.SetParent (unitInScene.transform);
						}
					
						NextTurn ();
					}
				}
			}
		}
	}

	private void Agro(){
	}

	private void Area(){
	}

}


