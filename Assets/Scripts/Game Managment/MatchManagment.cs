﻿using System.Collections;
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




	States qStates;
	IAActionsInGame qIA;
	QMatrix qMatrix;





	// GAME MANAGER --------------------------------------------------
	private GameManager gameManager;

	// SCENE MANAGER -------------------------------------------------
	public GameObject focused;	// El icono de Focused
	public GameObject agro;		// El icono de Agro
	public GameObject target;	// El icono que indica al jugador actual
	public Drawer drawer;		// Script que dibuja celdas del suelo
	public CalculateBoxes calculator; // Script que devuelve celdas determinadas
	public UnitFunctions functions;   // Script con las funciones de las unidades

	// Para saber si hay alguien con Agro usado en el equipo
	public Unit team_1_Agro;
	public Unit team_2_Agro;

	// Para llevar la cuenta de turnos que dura el Agro
	public int team_1_AgroCount;
	public int team_2_AgroCount;

	// Para almacenar la última acción 
	public AssemblyCSharp.Action currentAction;

	// Para almacenar las celdas viables
	public List<Vector2> allowedBoxes;

	// Para manejar el sistema de turnos del juego
	public List<Unit> round;
	public int unitTurn;

	// ATRIBUTOS DEL MAPA --------------------------------------------
	public GameObject[,] map;
	public GameObject grid_unit; //El prefab de la unidad del suelo
	public int dimension; // Por seguridad, debe ser del tipo (a * a)

	// ATRIBUTOS DE LOS PERSONAJES DEL JUEGO -------------------------

	// Las listas tipo Unit de ambos equipos
	public List<Unit> team_1_unitList;
	public List<Unit> team_2_unitList;

	// Las listas tipo GameObject de ambos equipos
	private List<GameObject> team_2_gameObjectList;
	private List<GameObject> team_1_gameObjectList;

	// Los objetos que representan a los personajes
	public GameObject Team_1_Prefab;
	public GameObject Team_2_Prefab;

	// Los canvas de ambos equipos
	public GameObject team_1_Canvas;
	public GameObject team_2_Canvas;

	// ATRIBUTOS FINAL DE PARTIDA -----------------------------------
	private List<Unit> winner;
	private List<Unit> looser;
	public GameObject finalCanvas;

	//
	private bool playerTurn;

	void Awake(){
		map = new GameObject[(int) Mathf.Sqrt(dimension), (int) Mathf.Sqrt(dimension)];
		buildMap ();
		qMatrix = gameObject.AddComponent<QMatrix> ();


	}

	void Start(){
		gameManager = GameObject.Find ("Game Manager").GetComponent<GameManager> ();
		drawer = GetComponent<Drawer> ();
		calculator = GetComponent<CalculateBoxes> ();
		functions = GetComponent<UnitFunctions> ();
		buildMatch ();
		qStates = new States (map, team_1_unitList, team_2_unitList);
		qIA = GameObject.Find ("IA").GetComponent<IAActionsInGame> ();

		NextTurn ();
	}

	void Update(){
		if (playerTurn) {

			if (currentAction.Equals (AssemblyCSharp.Action.Move) && Input.GetMouseButton (0)) {
				// meterle el equipo al cual pertenece y eso
				functions.Move (round [unitTurn], allowedBoxes);
			}

			if (currentAction.Equals (AssemblyCSharp.Action.Attack) && Input.GetMouseButton (0)) {
				functions.Attack (round [unitTurn], allowedBoxes);
			}

			if (currentAction.Equals (AssemblyCSharp.Action.Hability) && Input.GetMouseButton (0)) {
				switch (round [unitTurn].UnitRol) {
				case Rol.Healer:
					functions.Heal (round [unitTurn], allowedBoxes);
					break;

				case Rol.Distance:
					functions.Focus (round [unitTurn], allowedBoxes);
					break;

				case Rol.Mele:
					functions.Area (round [unitTurn], allowedBoxes);
					break;
				}
			}
		} 
	}

	void enemyTurn ()
	{
		bool[] unitState = null;
		Rol currentRol = Rol.Boss;
		switch (round [unitTurn].UnitRol) {
		case Rol.Healer:
			currentRol = Rol.Healer;
			unitState = qStates.GetHealerState (round [unitTurn]);
			break;
			
		case Rol.Distance:
			currentRol = Rol.Distance;
			unitState = qStates.GetDistanceConditions (round [unitTurn]);
			break;

		case Rol.Tank:
			currentRol = Rol.Tank;
			unitState = qStates.GetTankState (round [unitTurn]);
			break;

		case Rol.Mele:
			currentRol = Rol.Mele;
			unitState = qStates.GetMeleConditions (round [unitTurn]);
			break;
		}

		float[] marks = new float[5];
		int row = GetQRow (unitState);
		marks [0] = round [unitTurn].Matrix [row, 0];
		marks [1] = round [unitTurn].Matrix [row, 1];
		marks [2] = round [unitTurn].Matrix [row, 2];
		marks [3] = round [unitTurn].Matrix [row, 3];
		marks [4] = round [unitTurn].Matrix [row, 4];

		int bestMark = 0;
		for (int i = 0; i < marks.Length; i++) {
			if (marks [i] > marks [bestMark]) {
				bestMark = i;
			}
		}
		Unit objectiveUnit;
		GameObject unitInScene;

		switch (bestMark) {
		case 0:
			qIA.Attack (round [unitTurn], round [unitTurn].AttackRange);
			break;

		case 1:

			if (currentRol.Equals (Rol.Healer)) {
				qIA.Heal (round [unitTurn], round [unitTurn].HabilityRange);

			} else if (currentRol.Equals (Rol.Tank)) {
				qIA.Agro (round [unitTurn]);

				//					matchManagment.agro.SetActive (true);
				//					GameObject agroIcon = Instantiate (matchManagment.agro);
				//					matchManagment.agro.SetActive (false);
				//
				//					agroIcon.transform.position = matchManagment.GameObjectFromUnit(currentUnit).transform.position + Vector3.up * 2;
				//					agroIcon.transform.SetParent (matchManagment.GameObjectFromUnit(currentUnit).transform);
				//					matchManagment.team_2_Agro = currentUnit;
				//					matchManagment.team_2_AgroCount = 5;


			} else if (currentRol.Equals (Rol.Mele)) {
				qIA.Area (round [unitTurn], round [unitTurn].HabilityRange);

			} else if (currentRol.Equals (Rol.Distance)) {
				qIA.Focus (round [unitTurn], round [unitTurn].HabilityRange);

				//					focusedUnit.Focused = true;
				//					focusedUnit.FocusedCount = 6;
				//
				//					matchManagment.focused.SetActive (true);
				//					GameObject focusedIcon = Instantiate (matchManagment.focused);
				//					matchManagment.focused.SetActive (false);
				//
				//					focusedIcon.transform.position = hit.collider.transform.position + Vector3.up * 2;
				//					focusedIcon.transform.SetParent (hit.collider.transform);
			}


			break;

		case 2:
			objectiveUnit = team_1_unitList [UnityEngine.Random.Range (0, team_1_unitList.Count)];
			qIA.GoNearer (round [unitTurn], objectiveUnit);

			unitInScene = GameObjectFromUnit (round [unitTurn]);
			unitInScene.transform.position = map [(int)round [unitTurn].Position.x, (int)round [unitTurn].Position.y].transform.position + Vector3.up * 1.5f;
			break;

		case 3:
			
			objectiveUnit = team_1_unitList [UnityEngine.Random.Range (0, team_1_unitList.Count)];
			qIA.GoFarther (round [unitTurn], objectiveUnit);

			unitInScene = GameObjectFromUnit (round [unitTurn]);
			unitInScene.transform.position = map [(int)round [unitTurn].Position.x, (int)round [unitTurn].Position.y].transform.position + Vector3.up * 1.5f;
			break;
			
		case 4:
			break;

		}


		NextTurn ();

	}



	private void buildMap(){
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

	private void buildMatch(){
		playerTurn = false;

		team_1_AgroCount = 0;
		team_2_AgroCount = 0;

		currentAction = AssemblyCSharp.Action.None;
		allowedBoxes = new List<Vector2> ();

		team_1_gameObjectList = new List<GameObject> ();
		team_2_gameObjectList = new List<GameObject> ();

		// El equipo elegido por el jugador en el menú de selección
		team_1_unitList = Unit.GenerateTeam(gameManager.GetTeam());
		Vector2[] team_1_Area = calculator.GetAreaReferences(dimension, "Team_1");

		// Un equipo prefijado
		team_2_unitList = Unit.GenerateTeam (gameManager.GetGameMode ().GetBasicTeam ());
		Vector2[] team_2_Area = calculator.GetAreaReferences(dimension, "Team_2");

		foreach (Unit unit in team_2_unitList) {
			if (unit.UnitRol.Equals (Rol.Healer)) {
				unit.Matrix = qMatrix.ChargeQMatrix(qMatrix.QMatrix_Begginer_B_Healer, qMatrix.Route_QMatrix_Begginer_B_Healer, 18, 5);
//				unit.Name = "Jordi";
			}
			if (unit.UnitRol.Equals (Rol.Distance)) {
				unit.Matrix = qMatrix.ChargeQMatrix(qMatrix.QMatrix_Begginer_B_Distance, qMatrix.Route_QMatrix_Begginer_B_Distance, 18, 5);

			}
			if (unit.UnitRol.Equals (Rol.Mele)) {
				unit.Matrix = qMatrix.ChargeQMatrix(qMatrix.QMatrix_Begginer_B_Mele, qMatrix.Route_QMatrix_Begginer_B_Mele, 18, 5);
			}
			if (unit.UnitRol.Equals (Rol.Tank)) {
				unit.Matrix = qMatrix.ChargeQMatrix(qMatrix.QMatrix_Begginer_B_Tank, qMatrix.Route_QMatrix_Begginer_B_Tank, 18, 5);
			}

		}

		placeCharacters (team_1_unitList, team_1_Area);
		placeCharacters (team_2_unitList, team_2_Area);

		charactersInScene (team_1_unitList, team_1_gameObjectList);
		charactersInScene (team_2_unitList, team_2_gameObjectList);

		round = GetTurns ();
		unitTurn = -1;
	}

	private bool isGameOver(){
		if (team_1_unitList.Count == 0) {
			winner = team_2_unitList;
			looser = team_1_unitList;
			return true;
		}

		if (team_2_unitList.Count == 0) {
			winner = team_1_unitList;
			looser = team_2_unitList;
			return true;
		}

		return false;
	}

	public void NextTurn(){

		if (isGameOver ()) {
			finalCanvas.SetActive (true);
			if (winner.Equals (team_1_unitList)) {
				finalCanvas.GetComponentInChildren<Text> ().text = "Congratulations TEAM 1, you win this time!";
			} else {
				finalCanvas.GetComponentInChildren<Text> ().text = "Congratulations TEAM 2, you win this time!";
			}

			SceneManager.LoadScene ("Menu");
		} else {
						 
			unitTurn++;

			if (unitTurn >= round.Count) {
				unitTurn = 0;
			}	 

			if (team_1_unitList.Contains(round[unitTurn])) {
				playerTurn = true;
			} else {
				playerTurn = false;
			}                          
				
			if (team_1_Agro != null) {
				team_1_AgroCount -= 1;
				if (team_1_AgroCount <= 0) {
					Destroy (GameObjectFromUnit (team_1_Agro).transform.GetChild (0).gameObject);
					team_1_Agro = null;
				}
			}

			if (team_2_Agro != null) {
				team_2_AgroCount -= 1;
				if (team_2_AgroCount <= 0) {
					Destroy (GameObjectFromUnit (team_2_Agro).transform.GetChild (0).gameObject);
					team_2_Agro = null;
				}
			}


			foreach (Unit unit in team_2_unitList) {
				if (unit.Focused) {
					unit.FocusedCount = unit.FocusedCount - 1;

					if (unit.FocusedCount <= 0) {
						unit.Focused = false;

						Destroy (team_2_gameObjectList [team_2_unitList.IndexOf (unit)].transform.GetChild (0).gameObject);
					}
				}
			}

			foreach (Unit unit in team_1_unitList) {
				if (unit.Focused) {
					unit.FocusedCount = unit.FocusedCount - 1;

					if (unit.FocusedCount <= 0) {
						unit.Focused = false;

						Destroy (team_1_gameObjectList [team_1_unitList.IndexOf (unit)].transform.GetChild (0).gameObject);
					}
				}
			}

			drawer.UnDrawBoxes (map, allowedBoxes);
			currentAction = AssemblyCSharp.Action.None;

			if (GetUnitTeam (round [unitTurn]).Equals (team_1_unitList)) {
				actualizeCanvas (team_1_Canvas, team_2_Canvas);
			} else {
				actualizeCanvas (team_2_Canvas, team_1_Canvas);
			}
		}

		if (!playerTurn) {
			enemyTurn ();
		}
	}

	private void actualizeCanvas(GameObject canvas1, GameObject canvas2){
		canvas1.SetActive (true);
		canvas2.SetActive (false);

		GameObject enemyInScene = GameObjectFromUnit (round [unitTurn]);

		canvas1.transform.GetChild(0).transform.Find ("Unit Icon").GetComponent<Image> ().color = enemyInScene.GetComponent<Renderer>().material.color;
		canvas1.transform.GetChild(0).transform.Find ("Unit Rol Name").GetComponent<Text> ().text = round[unitTurn].UnitRol.ToString();

		float value = (float) round [unitTurn].CurrentLife / (float) round [unitTurn].Life;
		canvas1.transform.GetChild (0).transform.Find ("Unit Scrollbar Life").GetComponent<Scrollbar> ().size = value;
		canvas1.transform.GetChild (0).transform.Find ("Life Label").GetComponent<Text> ().text = round [unitTurn].CurrentLife + "/" + round [unitTurn].Life;

		target.transform.position = new Vector3 (enemyInScene.transform.position.x, enemyInScene.transform.position.y + 5f, enemyInScene.transform.position.z);
	}

	private void placeCharacters(List<Unit> team, Vector2[] area){
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

	private void charactersInScene(List<Unit> team, List<GameObject> teamInScene){

		GameObject obj;

		if (team.Equals (team_2_unitList)) {
			obj = Team_1_Prefab;
		} else {
			obj = Team_2_Prefab;
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

		//Creo lista vacía
		List<Unit> turns = new List<Unit> ();

		//La lleno con las unidades del equipo enemigo
		for (int i = 0; i < team_2_unitList.Count; i++) {
			turns.Add (team_2_unitList [i]);
		}

		//Y con las del equipo del jugador
		for (int i = 0; i < team_1_unitList.Count; i++) {
			turns.Add (team_1_unitList [i]);
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
		

	public void ActivateMovement(){
		drawer.UnDrawBoxes (map, allowedBoxes);

		allowedBoxes = calculator.AllowMovement(calculator.GetBoxesInsideRange(map, round[unitTurn], round[unitTurn].Movement), team_1_unitList, team_2_unitList);
		drawer.DrawBoxes (map, allowedBoxes, new Vector4(1.0f, 0.2f, 0.7f, 1.0f));
		currentAction = AssemblyCSharp.Action.Move;
	}
		

	public void ActivateAttack(){

		drawer.UnDrawBoxes (map, allowedBoxes);

		allowedBoxes = calculator.GetBoxesInsideRange(map, round[unitTurn], round[unitTurn].AttackRange);
		drawer.DrawBoxes (map, allowedBoxes, new Vector4(0.5f, 0.5f, 0.8f, 1f));
		currentAction = AssemblyCSharp.Action.Attack;
	}
		

	public void ActivateHability(){		

		drawer.UnDrawBoxes (map, allowedBoxes);

		switch (round [unitTurn].UnitRol) {
		case Rol.Healer:
			allowedBoxes = calculator.GetBoxesInsideRange (map, round [unitTurn], round [unitTurn].HabilityRange);
			drawer.DrawBoxes (map, allowedBoxes, new Vector4(0.7f, 0.3f, 0.8f, 1f));
			currentAction = AssemblyCSharp.Action.Hability;
			break;

		case Rol.Distance:
			allowedBoxes = calculator.GetBoxesInsideRange (map, round [unitTurn], round [unitTurn].HabilityRange);
			drawer.DrawBoxes (map, allowedBoxes, new Vector4(0.2f, 0.3f, 0.2f, 1f));
			currentAction = AssemblyCSharp.Action.Hability;
			break;

		case Rol.Mele:
			allowedBoxes = calculator.GetMeleHabilityBoxes (map, round [unitTurn], round [unitTurn].HabilityRange);
			drawer.DrawBoxes (map, allowedBoxes, new Vector4 (0.1f, 0.1f, 0.5f, 1f));
			currentAction = AssemblyCSharp.Action.Hability;
			break;

		case Rol.Tank:
			functions.Agro (round[unitTurn]);
			break;
		}
	}

	public void EndTurn(){
		NextTurn ();
	}

	// Función que devuelve el equipo al que pertenece un Unit
	public List<Unit> GetUnitTeam(Unit unit){
		if (team_1_unitList.Contains (unit))
			return team_1_unitList;
		
		return team_2_unitList;
	}

	// Función que devuelve el equipo al que pertence un GameObject
	public List<GameObject> GetObjectTeam(GameObject obj){
		if (team_1_gameObjectList.Contains (obj))
			return team_1_gameObjectList;

		return team_2_gameObjectList;
	}

	// Función que devuelve el GameObject correspondiente a un Unit
	public GameObject GameObjectFromUnit(Unit unit){
		List<Unit> team = GetUnitTeam (unit);

		if (team.Equals (team_1_unitList)) {
			return team_1_gameObjectList [team.IndexOf (unit)];
		}
		return team_2_gameObjectList [team.IndexOf (unit)];
	}

	// Función que devuelve el Unit correspondiente a un GameObject
	public Unit UnitFromGameObject(GameObject obj){
		List<GameObject> team = GetObjectTeam (obj);

		if (team.Equals (team_1_gameObjectList)) {
			return team_1_unitList [team.IndexOf (obj)];
		}
		return team_2_unitList [team.IndexOf (obj)];
	}

	public void RemoveUnit(Unit unit){
		unitTurn--;
		round.Remove (unit);

		if (GetUnitTeam (unit).Equals (team_1_unitList)) {
			GameObject unitInScene = GameObjectFromUnit (unit);
			team_1_gameObjectList.Remove (unitInScene);
			team_1_unitList.Remove (unit);

			Destroy (unitInScene.gameObject);
		} else {
			GameObject unitInScene = GameObjectFromUnit (unit);
			team_2_gameObjectList.Remove (unitInScene);
			team_2_unitList.Remove (unit);

			Destroy (unitInScene.gameObject);
		}
	}





	int GetQRow(bool[] estado){

		if (estado [0] == true && estado [1] == true && estado [2] == true && estado [3] == true) {
			return 0;
		}
		else if(estado [0] == false && estado [1] == true && estado [2] == true && estado [3] == true) {
			return 1;
		}
		else if(estado [0] == false && estado [1] == false && estado [2] == true && estado [3] == true) {
			return 2;
		}
		else if(estado [0] == false && estado [1] == false && estado [2] == false && estado [3] == true) {
			return 3;
		}
		else if(estado [0] == false && estado [1] == false && estado [2] == false && estado [3] == false) {
			return 4;
		}
		else if(estado [0] == true && estado [1] == true && estado [2] == true && estado [3] == false) {
			return 5;
		}
		else if(estado [0] == true && estado [1] == true && estado [2] == false && estado [3] == false) {
			return 6;
		}
		else if(estado [0] == true && estado [1] == false && estado [2] == false && estado [3] == false) {
			return 7;
		}
		else if(estado [0] == true && estado [1] == false && estado [2] == false && estado [3] == true) {
			return 8;
		}
		else if(estado [0] == false && estado [1] == true && estado [2] == true && estado [3] == false) {
			return 9;
		}
		else if(estado [0] == true && estado [1] == false && estado [2] == true && estado [3] == false) {
			return 10;
		}
		else if(estado [0] == false && estado [1] == true && estado [2] == false && estado [3] == true) {
			return 11;
		}
		else if(estado [0] == true && estado [1] == false && estado [2] == true && estado [3] == true) {
			return 12;
		}
		else if(estado [0] == false && estado [1] == true && estado [2] == false && estado [3] == false) {
			return 13;
		}
		else if(estado [0] == true && estado [1] == true && estado [2] == false && estado [3] == true) {
			return 14;
		}
		else /*(estado [0] == false && estado [0] == false && estado [0] == true && estado [0] == false) */{
			return 15;
		}

	}
}