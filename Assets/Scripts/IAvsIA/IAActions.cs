using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAActions : MonoBehaviour
{

	QLearningGame qGame;

	void Start(){
		qGame = GetComponent<QLearningGame> ();
	}

	// HEALER
	public void IA_Heal(object[,] map, Unit healer, List<Unit> myTeam, int range){
		List<Unit> hurtedAllies = QSceneManagment.HurtedAllies (map, healer, myTeam,range);

		// ¿Curo a quién peor esté?
//		Unit victim;
//
//		float probability = UnityEngine.Random.Range (0, 100);
//		if (probability < healer.HabilityCritic) {
//			Debug.Log ("cura critica");
//			victim.CurrentLife += healer.GetHeal (victim) * 2;
//
//		} else {
//			Debug.Log ("cura normal");
//			victim.CurrentLife += healer.GetHeal (victim);
//		}
//
//		if (victim.CurrentLife > victim.Life) {
//			victim.CurrentLife = victim.Life;
//		}
	}

	// DISTANCE
	public void IA_Focus(object[,] map, Unit distance, List<Unit> enemyTeam, int range){
		List<Unit> enemiesAtRange = QSceneManagment.EnemiesInside_BasicRange (map, distance, enemyTeam, range);

		// ¿A quién priorizo, a quien tiene menos vida?
//		Unit victim;
//
//		float probability = UnityEngine.Random.Range (0, 100);
//		if (probability > (100 - victim.Agility)) {
//			Debug.Log ("fallo focus");
//			// Debería pasar turno 
//				
//		} else {
//			Debug.Log ("acierto focus");
//			victim.Focused = true;
//			victim.FocusedCount = 6;
//		}
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


}


