using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;


public class QLearningGame : MonoBehaviour
{
	public List<Unit> team_1;
	public List<Unit> team_2;
	public List<Unit> winner, looser;


	public List<Unit> round;
	int unitTurn;


	private GameObject[,] map;
	private int dimension = 256;

	public List<Vector2> allowedBoxes;

	CalculateBoxes calculator;
	UnitFunctions functions;

	// Para saber si hay alguien con Agro usado en el equipo
	public Unit team_1_Agro;
	public Unit team_2_Agro;

	// Para llevar la cuenta de turnos que dura el Agro
	public int team_1_AgroCount;
	public int team_2_AgroCount;

	AssemblyCSharp.Action currentAction;


	void Awake(){
		// Generación del mapa
		map = new GameObject[(int)Mathf.Sqrt (dimension), (int)Mathf.Sqrt (dimension)];
//		for (int i = 0; i < (int)Mathf.Sqrt (dimension); i++) {
//			for (int j = 0; j < (int)Mathf.Sqrt (dimension); j++) {
//				map [i, j] = new GameObject ();
//			}
//		}

		calculator = gameObject.AddComponent<CalculateBoxes> ();
		functions = gameObject.AddComponent<UnitFunctions> ();
	}

	// Para el script de STATES //
	public GameObject[,] GetMap(){return map;}
	public List<Unit> GetTeam_A(){return team_1;}
	public List<Unit> GetTeam_B(){return team_2;}

	// Para QLearning
	public Unit GetCurrentPlayer(){return round[unitTurn];}

	public void StartGame(){

		team_1 = QSceneManagment.CreateTeam ();
		team_2 = QSceneManagment.CreateTeam ();

		team_1_AgroCount = 0;
		team_2_AgroCount = 0;

		currentAction = AssemblyCSharp.Action.None;
		allowedBoxes = new List<Vector2> ();

		Vector2[] team_1_Area = calculator.GetAreaReferences(dimension, "Team_1");
		Vector2[] team_2_Area = calculator.GetAreaReferences(dimension, "Team_2");

		placeCharacters (team_1, team_1_Area);
		placeCharacters (team_2, team_2_Area);

		round = getTurns ();
		unitTurn = -1;
	}

	private bool isGameOver(){
		if (team_1.Count == 0) {
			winner = team_2;
			looser = team_1;
			return true;
		}

		if (team_2.Count == 0) {
			winner = team_1;
			looser = team_2;
			return true;
		}

		return false;
	}

	private List<Unit> getTurns(){

		//Creo lista vacía
		List<Unit> turns = new List<Unit> ();

		//La lleno con las unidades del equipo enemigo
		for (int i = 0; i < team_2.Count; i++) {
			turns.Add (team_2 [i]);
		}

		//Y con las del equipo del jugador
		for (int i = 0; i < team_1.Count; i++) {
			turns.Add (team_1 [i]);
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

	public bool NextTurn(){

		if (isGameOver ()) {
			
			return true;
		} 
		else {

			unitTurn++;

			if (unitTurn >= round.Count) {
				unitTurn = 0;
			}	

			if (team_1_Agro != null) {
				team_1_AgroCount -= 1;
				if (team_1_AgroCount <= 0) {
					team_1_Agro = null;
				}
			}

			if (team_2_Agro != null) {
				team_2_AgroCount -= 1;
				if (team_2_AgroCount <= 0) {
					team_2_Agro = null;
				}
			}


			foreach (Unit unit in team_2) {
				if (unit.Focused) {
					unit.FocusedCount = unit.FocusedCount - 1;

					if (unit.FocusedCount <= 0) {
						unit.Focused = false;
					}
				}
			}

			foreach (Unit unit in team_1) {
				if (unit.Focused) {
					unit.FocusedCount = unit.FocusedCount - 1;

					if (unit.FocusedCount <= 0) {
						unit.Focused = false;
					}
				}
			}
				
			currentAction = AssemblyCSharp.Action.None;
		}

		return false;
	}

	private void placeCharacters(List<Unit> team, Vector2[] area){
		List<Vector2> ocupedPositions = new List<Vector2>();
		int cont = 0;

		// CUATRO PORQUE HAY 4 PJS POR EQUIPO!!
		while (ocupedPositions.Count < 4) {
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

	public void SetAgro(List<Unit> team, Unit tank){

		if (team.Equals (team_1)) {
			if (team_1_Agro == null) {
				team_1_Agro = tank;
				team_1_AgroCount = 5;
			}
		} else {
			if (team_2_Agro == null) {
				team_2_Agro = tank;
				team_2_AgroCount = 5;
			}
		}
	}


	public void RemoveUnit(Unit unit, List<Unit> unitTeam){
		unitTurn--;
		round.Remove (unit);
		unitTeam.Remove (unit);
	}
}