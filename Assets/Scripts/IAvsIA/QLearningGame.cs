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

	public CalculateBoxes calculator; // Script que devuelve celdas determinadas
	public UnitFunctions functions;   // Script con las funciones de las unidades

	private object[,] map;
	private int dimension = 15;

	public List<Vector2> allowedBoxes;


	// Para saber si hay alguien con Agro usado en el equipo
	public Unit team_1_Agro;
	public Unit team_2_Agro;

	// Para llevar la cuenta de turnos que dura el Agro
	public int team_1_AgroCount;
	public int team_2_AgroCount;

	AssemblyCSharp.Action currentAction;


	void Awake(){
		QSceneManagment.CreateTeams (team_1, team_2);

		// Generación del mapa
		map = new object[(int)Mathf.Sqrt (dimension), (int)Mathf.Sqrt (dimension)];
		for (int i = 0; i < (int)Mathf.Sqrt (dimension); i++) {
			for (int j = 0; j < (int)Mathf.Sqrt (dimension); j++) {
				map [i, j] = new object ();
			}
		}

		calculator = GetComponent<CalculateBoxes> ();
		functions = GetComponent<UnitFunctions> ();
		buildMatch ();
	}

	void Start(){

		// Genero los turnos
		round = getTurns();
		unitTurn = -1;

		// Comienzo con los turnos
		NextTurn();
	}

	// Para el script de STATES //
	public object[,] GetMap(){return map;}
	public List<Unit> GetTeam_A(){return team_1;}
	public List<Unit> GetTeam_B(){return team_2;}


	private void buildMatch(){
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

	private void NextTurn(){

		// AQUI TIENE QUE PONERSE LA FORMA DE TOMAR ACCIONES (A VOLEO; TENIENDO EN CUENTA Q; ETC)
		// TAL UNIDAD QUE TIENE ESTE TURNO, REALIZAR ESTA ACCION: TIENE ESTADO ANTERIOR? ESTADO ACTUAL = ESTADO ANTERIOR
		// CALCULO DE ALLOWEDBOXES SEGUN ACCION, ETC.

		if (isGameOver ()) {
			
			// llamar a la actualizacion de las matrices dependiendo del ganador??
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
}