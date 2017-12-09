using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;

public class States {

	object[,] map;
	List<Unit> team1;
	List<Unit> team2;

	public States(object[,] map, List<Unit> team1, List<Unit> team2){
		this.map = map;
		this.team1 = team1;
		this.team2 = team2;
	}

	public bool[] GetTankState(Unit tank){
		bool[] conditions = new bool[4];

		// Primera condición: ¿Estoy herido? ------------------------------------------------------------------
		if (!QSceneManagment.IsHurted (tank)) {
			conditions [0] = true;
		} else {
			conditions [0] = false;
		}

		// Segunda condición: ¿Alguién de mi equipo está herido? ----------------------------------------------
		bool isSomeoneHurted = false;
		foreach (Unit ally in QSceneManagment.GetUnitTeam(tank, team1, team2)) {
			if (!tank.Equals (ally)) {
				if (QSceneManagment.IsHurted (ally)) {
					isSomeoneHurted = true;
				}
			}
		}
		if (isSomeoneHurted)
			conditions [1] = false;
		else
			conditions [1] = true;	

		//Tercera condición: ¿Existe un enemigo dentro del rango de ataque? -----------------------------------
		if (QSceneManagment.SomeoneInRange (map, tank, QSceneManagment.GetEnemyTeam (tank, team1, team2), tank.AttackRange)) {
			conditions [2] = true;
		} else {
			conditions [2] = false;
		}
			
		//Cuarta condicion: ¿Algún aliado está siendo Focused? ------------------------------------------------
		if (QSceneManagment.IsSomeoneFocused (tank, team1, team2)) {
			conditions [3] = true;
		} else {
			conditions [3] = false;
		}

		return conditions;
	}

	public bool[] GetHealerState(Unit healer){
		bool[] conditions = new bool[4];

		// Primera condición: ¿Estoy herido? ------------------------------------------------------------------
		if (!QSceneManagment.IsHurted (healer)) {
			conditions [0] = true;
		} else {
			conditions [0] = false;
		}

		// Segunda condición: ¿Alguién de mi equipo está herido? ----------------------------------------------
		bool isAllyHurted = false;
		foreach (Unit ally in QSceneManagment.GetUnitTeam(healer, team1, team2)) {
			if (!healer.Equals (ally)) {
				if (QSceneManagment.IsHurted (ally)) {
					isAllyHurted = true;
				}
			}
		}
		if (isAllyHurted)
			conditions [1] = false;
		else
			conditions [1] = true;	

		//Tercera condición: ¿Existe un enemigo dentro del rango de ataque? ------------------------------------
		if (QSceneManagment.SomeoneInRange (map, healer, QSceneManagment.GetEnemyTeam (healer, team1, team2), healer.AttackRange)) {
			conditions [2] = true;
		} else {
			conditions [2] = false;
		}

		//Cuarta condicion: ¿Algún aliado está dentro de mi rango de habilidad? --------------------------------
		if (QSceneManagment.SomeoneInRange (map, healer, QSceneManagment.GetUnitTeam (healer, team1, team2), healer.HabilityRange)) {
			conditions [3] = true;
		} else {
			conditions [3] = false;
		}

		return conditions;
	}

	public bool[] GetDistanceConditions(Unit distance){
		bool[] conditions = new bool[4];

		// Primera condición: ¿Estoy herido? ------------------------------------------------------------------
		if (!QSceneManagment.IsHurted (distance)) {
			conditions [0] = true;
		} else {
			conditions [0] = false;
		}

		// Segunda condición: ¿Existe un enemigo dentro del rango de ataque? ------------------------------------
		if (QSceneManagment.SomeoneInRange (map, distance, QSceneManagment.GetEnemyTeam (distance, team1, team2), distance.AttackRange)) {
			conditions [1] = true;
		} else {
			conditions [1] = false;
		}

		// Tercera condición: ¿Existe algún enemigo herido? ----------------------------------------------------
		bool isEnemyHurted = false;
		foreach (Unit enemy in QSceneManagment.GetEnemyTeam(distance, team1, team2)) {
			if (QSceneManagment.IsHurted (enemy)) {
				isEnemyHurted = true;
			}
		}
		if (isEnemyHurted)
			conditions [2] = false;
		else
			conditions [2] = true;	

		// Cuarta condicion: ¿Algún enemigo está dentro de mi rango de habilidad? --------------------------------
		if (QSceneManagment.SomeoneInRange (map, distance, QSceneManagment.GetEnemyTeam (distance, team1, team2), distance.HabilityRange)) {
			conditions [3] = true;
		} else {
			conditions [3] = false;
		}

		return conditions;
	}

	public bool[] GetMeleConditions(Unit mele){
		bool[] conditions = new bool[4];

		// Primera condición: ¿Estoy herido? -------------------------------------------------------------------
		if (!QSceneManagment.IsHurted (mele)) {
			conditions [0] = true;
		} else {
			conditions [0] = false;
		}

		// Segunda condición: ¿Existe un enemigo dentro del rango de ataque? ------------------------------------
		if (QSceneManagment.SomeoneInRange (map, mele, QSceneManagment.GetEnemyTeam (mele, team1, team2), mele.AttackRange)) {
			conditions [1] = true;
		} else {
			conditions [1] = false;
		}

		// Tercera condicion: ¿Algún enemigo está dentro de mi rango de habilidad? --------------------------------
		if (QSceneManagment.SomeoneInMeleRange (map, mele, QSceneManagment.GetEnemyTeam (mele, team1, team2), mele.HabilityRange)) {
			conditions [2] = true;
		} else {
			conditions [2] = false;
		}

		// Cuarta condición: ¿Existe MÁS DE UN enemigo dentro de mi rango de habilidad? --------------------------
		if (QSceneManagment.CrowdInsideMeleRange(map, mele, QSceneManagment.GetEnemyTeam(mele, team1, team2), mele.HabilityRange)){
			conditions [3] = true;
		} else {
			conditions [3] = false;
		}

		return conditions;
	}
}