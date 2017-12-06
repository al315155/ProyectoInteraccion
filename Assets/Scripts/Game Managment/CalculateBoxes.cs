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

public class CalculateBoxes : MonoBehaviour {

	// Obtiene las casillas finales del suelo donde el personaje podrá desplazarse
	// Por ejemplo si ya existe otro personaje sobre una de esas celdas, esa celda
	// será eliminada de su rango de movimiento.
	public List<Vector2> AllowMovement(List<Vector2> positions, List<Unit> team1, List<Unit> team2){

		List<Unit> units = new List<Unit> ();
		foreach (Unit unit in team1) {
			units.Add (unit);
		}
		foreach (Unit unit in team2) {
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

	// Función específica del rol Mele que obtiene las celdas para su habilidad Area
	public List<Vector2> GetMeleHabilityBoxes(GameObject[,] map, Unit unit, int range){
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
		return positions;

	}

	// Obtiene las celdas dentro de un rango determinado
	public List<Vector2> GetBoxesInsideRange(GameObject[,] map, Unit unit, int range){
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
		return positions;
	}

	// Función que determina los bordes del área que encierra las posibles posiciones
	// iniciales de los personajes
	public Vector2[] GetAreaReferences(int dimension, string team){
		int sideMargin, topMargin, botMargin;
		calculateMargins (dimension, out sideMargin, out topMargin, out botMargin);

		Vector2 topLeft;
		Vector2 botRight;

		if (team.Equals ("Team_1")) {
			topLeft = new Vector2 (sideMargin, topMargin);
			botRight = new Vector2 ((int) Math.Sqrt(dimension) - sideMargin, (int) Math.Sqrt(dimension) / 2 - botMargin);
		} else {
			topLeft = new Vector2 (sideMargin, (int) Math.Sqrt(dimension) / 2 + botMargin);
			botRight = new Vector2 ((int) Math.Sqrt(dimension) - sideMargin, (int) Math.Sqrt(dimension) - topMargin);
		}
		return new Vector2[]{ topLeft, botRight };
	}

	private void calculateMargins(int dimension, out int sideMargin, out int topMargin, out int botMargin){
		int normalMargin = (int) Math.Sqrt(dimension) * 20 / 100;
		sideMargin = normalMargin;
		topMargin = normalMargin / 2;
		botMargin = normalMargin;
	}
}
