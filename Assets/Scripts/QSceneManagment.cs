using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;
using System;

public static class QSceneManagment{

	// Unit rols
	static Unit tank = new Tank();
	static Unit healer = new Healer();
	static Unit distance = new DistDamage();
	static Unit mele = new MeleDamage();

	// Stats
	static int Tank_minLife = (30 * tank.Life) / 100;
	static int Mele_minLife = (40 * mele.Life) / 100;
	static int Distance_minLife = (40 * healer.Life) / 100;
	static int Healer_minLife = (50 * distance.Life) / 100;

	// Método que inicializa los equipos de forma predeterminada
	public static void CreateTeams(List<Unit> team1, List<Unit> team2){
		team1 = new List<Unit> (4) { tank, healer, distance, mele};
		team2 = new List<Unit> (4) { tank, healer, distance, mele};

	}

    public static List<Unit> CreateTeam()
    {
        List<Unit> team1 = new List<Unit>(4) { tank, healer, distance, mele };
        return team1;
    }

	// Método que usa el tanque para saber si alguien de su equipo está focuseado
	public static bool IsSomeoneFocused(Unit unit, List<Unit> team1, List<Unit> team2){
		List<Unit> team = GetUnitTeam (unit, team1, team2);
		foreach (Unit ally in team) {
			if (ally.Focused) {
				return true;
			}
		}
		return false;
	}

	// Método que permite saber si una unidad está "herida"
	public static bool IsHurted(Unit unit){
		int minLife = 0;

		switch (unit.UnitRol) {
		case Rol.Tank:
			minLife = Tank_minLife;
			break;
		case Rol.Distance:
			minLife = Distance_minLife;
			break;
		case Rol.Mele:
			minLife = Mele_minLife;
			break;
		case Rol.Healer:
			minLife = Healer_minLife;
			break;
		}

		if (unit.CurrentLife < minLife){
			return true;
		}
		return false;
	}

	// Función que devuelve el equipo al que pertenece un Unit
	public static List<Unit> GetUnitTeam(Unit unit, List<Unit> team1, List<Unit> team2){
		if (team1.Contains (unit))
			return team1;
		return team2;
	}

	// Función que devuelve el equipo ENEMIGO al que pertenece un Unit
	public static List<Unit> GetEnemyTeam(Unit unit, List<Unit> team1, List<Unit> team2){
		if (team1.Contains (unit))
			return team2;
		return team1;
	}

	// Función que devuelve true si una unidad del equipo Team está en el rango Range de Unit
	// Rango en forma de estrella gorda
	public static bool SomeoneInRange(object[,] map, Unit unit, List<Unit> team, int range){
		List<Vector2> positions = new List<Vector2> ();
		Vector2 pos = unit.Position;

		int minX = (int)unit.Position.x - range;
		int maxX = (int)unit.Position.x + range;
		int minY = (int)unit.Position.y - range;
		int maxY = (int)unit.Position.y + range;

		if (minX < 0)
			minX = 0;
		if (maxX >= map.GetLength (0))
			maxX = map.GetLength (0) - 1;

		if (minY < 0)
			minY = 0;
		if (maxY >= map.GetLength (1))
			maxY = map.GetLength (1) - 1;

		int aux = 0;
		for (int i = minX; i <= maxX; i++) {
			for (int j = 0; j <= aux; j++) {
				Vector2 newPosition = Vector2.one;

				if ((pos.y + j) <= maxY) {
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
			
		// Con todas las posiciones que existen dentro del rango, reviso si en una
		// de ellas existe una unidad del equipo Team. Si es así devuelvo true.
		foreach (Vector2 position in positions) {
			foreach (Unit u in team) {
				if (u.Position.Equals (position)) {
					return true;
				}
			}
		}
		return false;
	}

	public static bool SomeoneInMeleRange(object[,] map, Unit unit, List<Unit> enemyTeam, int range){
		List<Vector2> positions = new List<Vector2> ();
		Vector2 pos = unit.Position;

		int minX = (int)unit.Position.x - range;
		int maxX = (int)unit.Position.x + range;
		int minY = (int)unit.Position.y - range;
		int maxY = (int)unit.Position.y + range;

		if (minX < 0)
			minX = 0;
		if (maxX >= map.GetLength (0))
			maxX = map.GetLength (0) - 1;

		if (minY < 0)
			minY = 0;
		if (maxY >= map.GetLength (1))
			maxY = map.GetLength (1) - 1;

		for (int i = minX; i <= maxX; i++) {
			for (int j = minY; j <= maxY; j++) {
				Vector2 newPosition = Vector2.one;

				if (i == (int)pos.x) {
					newPosition = new Vector2 (i, j);
					positions.Add (newPosition);
				}

				if (j == (int)pos.y) {
					newPosition = new Vector2 (i, j);
					positions.Add (newPosition);
				}
			}
		}

		// Con todas las posiciones que existen dentro del rango, reviso si en una
		// de ellas existe una unidad del equipo Team. Si es así devuelvo true.
		foreach (Vector2 position in positions) {
			foreach (Unit u in enemyTeam) {
				if (u.Position.Equals (position)) {
					return true;
				}
			}
		}
		return false;
	}

	public static bool CrowdInsideMeleRange(object[,] map, Unit unit, List<Unit> enemyTeam, int range){
		List<Vector2> positions = new List<Vector2> ();
		Vector2 pos = unit.Position;

		int minX = (int)unit.Position.x - range;
		int maxX = (int)unit.Position.x + range;
		int minY = (int)unit.Position.y - range;
		int maxY = (int)unit.Position.y + range;

		if (minX < 0)
			minX = 0;
		if (maxX >= map.GetLength (0))
			maxX = map.GetLength (0) - 1;

		if (minY < 0)
			minY = 0;
		if (maxY >= map.GetLength (1))
			maxY = map.GetLength (1) - 1;

		for (int i = minX; i <= maxX; i++) {
			for (int j = minY; j <= maxY; j++) {
				Vector2 newPosition = Vector2.one;

				if (i == (int)pos.x) {
					newPosition = new Vector2 (i, j);
					positions.Add (newPosition);
				}

				if (j == (int)pos.y) {
					newPosition = new Vector2 (i, j);
					positions.Add (newPosition);
				}
			}
		}

		// Contabilizo el total de enemigos dentro del área. Si es mayor que
		// uno devuelvo true.
		int enemies = 0;
		foreach (Vector2 position in positions) {
			foreach (Unit u in enemyTeam) {
				if (u.Position.Equals (position)) {
					enemies += 1;
				}
			}
		}
		if (enemies > 1)
			return true;
		return false;
	}

	// Colocar las unidades en el mapa
	public static void PlaceCharacters(List<Unit> team, Vector2[] area){
		List<Vector2> ocupedPositions = new List<Vector2>();
		int cont = 0;

		while (ocupedPositions.Count < team.Count) {
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

	// Obtener el área inicial de las unidades
	public static Vector2[] GetReferences(int dimension, string team){
		int sideMargin, topMargin, botMargin;
		CalculateMargins (dimension, out sideMargin, out topMargin, out botMargin);

		Vector2 topLeft;
		Vector2 botRight;

		if (team.Equals ("Player")) {
			topLeft = new Vector2 (sideMargin, topMargin);
			botRight = new Vector2 ((int) Math.Sqrt(dimension) - sideMargin, (int) Math.Sqrt(dimension) / 2 - botMargin);
		} else {
			topLeft = new Vector2 (sideMargin, (int) Math.Sqrt(dimension) / 2 + botMargin);
			botRight = new Vector2 ((int) Math.Sqrt(dimension) - sideMargin, (int) Math.Sqrt(dimension) - topMargin);
		}
		return new Vector2[]{ topLeft, botRight };
	}

	// Márgenes para el área de colocación de las unidades
	private static void CalculateMargins(int dimension, out int sideMargin, out int topMargin, out int botMargin){
		int normalMargin = (int) Math.Sqrt(dimension) * 20 / 100;
		sideMargin = normalMargin;
		topMargin = normalMargin / 2;
		botMargin = normalMargin;
	}
}
