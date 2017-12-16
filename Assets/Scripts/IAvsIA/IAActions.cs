using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAActions : MonoBehaviour
{

	QLearningGame qGame;
    public bool isEnemyDead;
  

	void Start(){
		qGame = GetComponent<QLearningGame> ();
        isEnemyDead = false;

	}

	public void IA_Attack(object[,] map, Unit attacker, List<Unit> enemyTeam, int range){
		//List<Unit> hurtedEnemies = QSceneManagment.HurtedAllies (map, attacker, enemyTeam, range);
		List<Unit> enemiesAtRange = QSceneManagment.EnemiesInside_BasicRange (map, attacker, enemyTeam, range);
        
		Unit victim = enemiesAtRange[Random.Range(0, enemiesAtRange.Count)];

		float probability = UnityEngine.Random.Range (0, 100);
		if (probability > (100 - victim.Agility)) {
			Debug.Log ("fallo ataque");
			// Debería pasar turno 

		} else {
			Debug.Log ("acierto ataque");
			//acierto el área en esta unidad
			//critico??

			probability = UnityEngine.Random.Range (0, 100);
			if (probability < attacker.Critic / 2) {
				//critico
				victim.CurrentLife -= attacker.Damage * 2;
			} else {
				//no critico
				victim.CurrentLife -= attacker.Damage;
			}

			if (victim.CurrentLife <= 0) {
				qGame.RemoveUnit (victim, enemyTeam);
                isEnemyDead = true;
                
			}
		}

	}
 

	// HEALER
	public void IA_Heal(object[,] map, Unit healer, List<Unit> myTeam, int range){
		List<Unit> hurtedAllies = QSceneManagment.HurtedAllies (map, healer, myTeam, range);

		// ¿Curo a quién peor esté?
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
	public void IA_Focus(object[,] map, Unit distance, List<Unit> enemyTeam, int range){
		List<Unit> enemiesAtRange = QSceneManagment.EnemiesInside_BasicRange (map, distance, enemyTeam, range);

		// ¿A quién priorizo, a quien tiene menos vida?
		Unit victim = enemiesAtRange[Random.Range(0, enemiesAtRange.Count)];

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
	public void IA_Agro(Unit tank, List<Unit> team1, List<Unit> team2){
		if (QSceneManagment.GetUnitTeam (tank, team1, team2).Equals (team1)) {
			qGame.SetAgro (team1, tank);
		} else {
			qGame.SetAgro (team2, tank);
		}
	}

	// MELE
	public void IA_Area(object[,] map, Unit mele, List<Unit> unitTeam, List<Unit> enemyTeam, int range){
		List<Unit> enemiesInRange = QSceneManagment.EnemiesInside_MeleRange (map, mele, enemyTeam, range);
		List<Unit> deadUnits = new List<Unit> ();

		foreach (Unit unit in enemyTeam) {

			float probability = UnityEngine.Random.Range (0, 100);
			if (probability > (100 - unit.Agility)) {
				//acierto el área en esta unidad
				int value = UnityEngine.Random.Range ((int)mele.GetArea (unit).x, (int)mele.GetArea (unit).y);
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
			}
		}

		foreach (Unit unit in deadUnits) {
				if (unit.CurrentLife <= 0) {
				qGame.RemoveUnit (unit, QSceneManagment.GetUnitTeam (unit, unitTeam, enemyTeam));
			}
		}
	}

	public void GoNearer(List<Vector2> allowedBoxes,Unit me, Unit target)
	{
		float[] values = GetPositionValues (allowedBoxes, target);
		Vector2[] positions = ArrayFromList (allowedBoxes);
        Debug.Log("Tamaño" + values.Length);
		positions = Nearest2Furthest (positions, values);
		me.Position = positions [0];
	}

	public void GoFarther (List<Vector2> allowedBoxes, Unit unit, Unit me){

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


