using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAActionsInGame : MonoBehaviour
{
	MatchManagment game;
	GameObject[,] map;
	List<Unit> myTeam;
	List<Unit> enemyTeam;

	void Awake(){
		game = GameObject.Find ("Scene Manager").GetComponent<MatchManagment> ();
		map = game.map;
		myTeam = game.team_2_unitList;
		enemyTeam = game.team_1_unitList;
	}

	void Update(){
		map = game.map;
		myTeam = game.team_2_unitList;
		enemyTeam = game.team_1_unitList;
	}

	public void Attack(Unit attacker, int range){
		List<Unit> enemiesAtRange = QSceneManagment.EnemiesInside_BasicRange (map, attacker, enemyTeam, range);

		if (enemiesAtRange.Count > 0) {
			Unit victim = enemiesAtRange [Random.Range (0, enemiesAtRange.Count-1)];

			float probability = UnityEngine.Random.Range (0, 100);
			if (probability > (100 - victim.Agility)) {
				Debug.Log ("fallo ataque");

			} else {
				Debug.Log ("acierto ataque");
				probability = UnityEngine.Random.Range (0, 100);
				if (probability < attacker.Critic / 2) {
					victim.CurrentLife -= attacker.Damage * 2;
				} else {
					victim.CurrentLife -= attacker.Damage;
				}

				if (victim.CurrentLife <= 0) {
					game.RemoveUnit (victim);

				}
			}
		}
	}

	// HEALER
	public void Heal(Unit healer, int range){
		List<Unit> hurtedAllies = QSceneManagment.HurtedAllies (map, healer, myTeam, range);

		Unit victim = mostHurted(hurtedAllies);

		float probability = UnityEngine.Random.Range (0, 100);
		if (probability < healer.HabilityCritic) {
			Debug.Log ("cura critica");
			victim.CurrentLife += healer.GetHeal (victim) * 2;

		} else {
			Debug.Log ("cura normal");
			victim.CurrentLife += healer.GetHeal (victim);
		}

		if (victim.CurrentLife > victim.Life) {
			victim.CurrentLife = victim.Life;
		}
	}

	private Unit mostHurted(List<Unit> hurtedUnits){
		Unit u = hurtedUnits [0];
		foreach (Unit units in hurtedUnits) {
			if (units.CurrentLife < u.CurrentLife) {
				u = units;
			}
		}

		return u;
	}

	// DISTANCE
	public void Focus(Unit distance, int range){
		List<Unit> enemiesAtRange = QSceneManagment.EnemiesInside_BasicRange (map, distance, enemyTeam, range);

		// ¿A quién priorizo, a quien tiene menos vida?
		Unit victim = enemiesAtRange[Random.Range(0, enemiesAtRange.Count-1)];

		float probability = UnityEngine.Random.Range (0, 100);
		if (probability > (100 - victim.Agility)) {
			Debug.Log ("fallo focus");
			// Debería pasar turno 

		} else {
			Debug.Log ("acierto focus");
			victim.Focused = true;
			victim.FocusedCount = 6;
		}
	}

	// TANQUE
	public void Agro(Unit tank){

		if (game.team_2_Agro == null) {

			game.agro.SetActive (true);
			GameObject agroIcon = Instantiate (game.agro);
			game.agro.SetActive (false);

			agroIcon.transform.position = game.GameObjectFromUnit (tank).transform.position + Vector3.up * 2;
			agroIcon.transform.SetParent (game.GameObjectFromUnit (tank).transform);
			game.team_2_Agro = tank;
			game.team_2_AgroCount = 5;

		}

	}

	// MELE
	public void Area(Unit mele, int range){
		List<Unit> enemiesInRange = QSceneManagment.EnemiesInside_MeleRange (map, mele, enemyTeam, range);
		List<Unit> deadUnits = new List<Unit> ();

		foreach (Unit unit in enemyTeam) {

			float probability = UnityEngine.Random.Range (0, 100);
			if (probability < (100 - unit.Agility)) {
				//acierto el área en esta unidad
				Debug.Log ("acierto area");
				int value = UnityEngine.Random.Range ((int)mele.GetArea (unit).x, (int)mele.GetArea (unit).y-1);
				//critico??

				probability = UnityEngine.Random.Range (0, 100);
				if (probability < mele.Critic / 2) {
					//critico
					unit.CurrentLife -= value * 2;
				} else {
					//no critico
					unit.CurrentLife -= value;
				}

				if (unit.CurrentLife <= 0) {
					deadUnits.Add (unit);
				}
			} else {
				Debug.Log ("fallo area");
			}
		}

		if (deadUnits.Count > 0) {
			foreach (Unit unit in deadUnits) {
				if (unit.CurrentLife <= 0) {
					game.RemoveUnit (unit);

				}
			}
		}
	}

	public void GoNearer(Unit me, Unit target)
	{
		List<Vector2> allowedBoxes = new List<Vector2>();
		allowedBoxes = QSceneManagment.BasicRange(map, me, me.Movement);

		float[] values = GetPositionValues (allowedBoxes, target);
		Vector2[] positions = ArrayFromList (allowedBoxes);
		//        Debug.Log("Tamaño" + values.Length);
		positions = Nearest2Furthest (positions, values);
		me.Position = positions [0];
	}

	public void GoFarther (Unit unit, Unit me){
		List<Vector2> allowedBoxes = new List<Vector2>();
		allowedBoxes = QSceneManagment.BasicRange(map, me, me.Movement);

		float[] values = GetPositionValues (allowedBoxes, unit);
		Vector2[] positions = ArrayFromList (allowedBoxes);

		positions = Nearest2Furthest (positions, values);
		me.Position = positions [positions.Length - 1];

	}


	float[] GetPositionValues (List<Vector2> allowedBoxes, Unit unit)
	{
		float[] values = new float[allowedBoxes.Count];
		for (int i = 0; i < allowedBoxes.Count; i++) {
			float value = Mathf.Abs (unit.Position.x - allowedBoxes [i].x) + Mathf.Abs (unit.Position.y - allowedBoxes[i].y);
			values [i] = value;
		}
		return values;
	}

	Vector2[] ArrayFromList (List<Vector2> allowedBoxes)
	{
		Vector2[] arrayPositions = new Vector2[allowedBoxes.Count];
		for (int i = 0; i < allowedBoxes.Count; i++) {
			arrayPositions [i] = allowedBoxes [i];

		}
		return arrayPositions;
	}

	Vector2[] Nearest2Furthest (Vector2[] positions, float[] values)
	{
		for (int i = 0; i < positions.Length; i++) {
			int min = i;
			for (int j = i; j < positions.Length; j++) {
				if(values[j]<values[min]){
					min = j;
				}
			}
			float auxValue = values [i];
			Vector2 auxPos = positions [i];
			values [i] = values [min];
			positions [i] = positions [min];
			values [min] = auxValue;
			positions [min] = auxPos;
		}
		return positions;
	}

}


