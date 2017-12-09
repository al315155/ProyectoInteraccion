using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitFunctions : MonoBehaviour {

	MatchManagment matchManagment;
	QLearningGame qLearning;

	GameObject[,] map;

	void Start(){
		matchManagment = GetComponent<MatchManagment> ();
		if (matchManagment == null) {
			qLearning = GetComponent<QLearningGame> ();
			map = qLearning.GetMap ();
		} else {
			map = matchManagment.map;
		}
	}

	private Vector2 worldToMap(GameObject obj){
		for (int i = 0; i < map.GetLength (0); i++) {
			for (int j = 0; j < map.GetLength (1); j++) {
				if (map [i, j].Equals (obj)) {
					return new Vector2 (i, j);
				}
			}
		}
		return new Vector2 (-1, -1);
	}
		
	public void Move(Unit currentUnit, List<Vector2> allowedBoxes){
		RaycastHit hit;
		Vector2 newPosition = new Vector2 (-1, -1);

		if (Physics.Raycast (Camera.main.ScreenPointToRay (Input.mousePosition), out hit)) {
			if (hit.collider.tag.Equals ("Ground")) {
				newPosition = worldToMap(hit.collider.gameObject);

				if (allowedBoxes.Contains (newPosition)) {
					currentUnit.Position = newPosition;
					GameObject unitInScene = matchManagment.GameObjectFromUnit (currentUnit);

					unitInScene.transform.position = map [(int)currentUnit.Position.x, (int)currentUnit.Position.y].transform.position + Vector3.up * 1.5f;
					matchManagment.NextTurn ();
				}
			}
		}
	}

	private void doAttack(Unit currentUnit, Unit victimUnit){
		float probability = UnityEngine.Random.Range (0, 100);

		if (probability > (100 - victimUnit.Agility)) {
			Debug.Log ("fallo ataque basico");
			matchManagment.NextTurn ();
				
		} else {
			probability = UnityEngine.Random.Range (0, 100);
			if (victimUnit.Focused) {
				if (probability < currentUnit.Critic / 2) {
					Debug.Log ("ataque basico critico");
					victimUnit.CurrentLife -= currentUnit.Damage * 2;
				}
				else {
					Debug.Log ("ataque basico normal");
					victimUnit.CurrentLife -= currentUnit.Damage;
				}
			} else {
				if (probability < currentUnit.Critic) {
					Debug.Log ("ataque basico critico");
					victimUnit.CurrentLife -= currentUnit.Damage * 2;

				}
				else {
					Debug.Log ("ataque basico normal");
					victimUnit.CurrentLife -= currentUnit.Damage;
				}
			}

			if (victimUnit.CurrentLife <= 0) {
				matchManagment.RemoveUnit (victimUnit);
			}

			matchManagment.NextTurn ();
		}
	}

	public void Attack(Unit currentUnit, List<Vector2> allowedBoxes){
		RaycastHit hit;

		if (Physics.Raycast (Camera.main.ScreenPointToRay (Input.mousePosition), out hit)) {
			if (hit.collider.tag.Equals ("Enemy") || hit.collider.tag.Equals ("Player")) { //Esto habría que cambiarlo por "unidad"
				Unit victimUnit = matchManagment.UnitFromGameObject(hit.collider.gameObject);

				if (allowedBoxes.Contains (victimUnit.Position) && allowedAttack (currentUnit, victimUnit)) {

					if (someoneIsAgro (victimUnit)){
						if (isUnitAgro (victimUnit)) {
							doAttack (currentUnit, victimUnit);
						}
					} else {
						doAttack (currentUnit, victimUnit);
					}
				}
			}
		}
	}

	public void Heal(Unit currentUnit, List<Vector2> allowedBoxes){
		RaycastHit hit;

		if (Physics.Raycast ( Camera.main.ScreenPointToRay (Input.mousePosition), out hit)) {
			if (hit.collider.tag.Equals ("Enemy") || hit.collider.tag.Equals ("Player")) {

				Unit healedUnit = matchManagment.UnitFromGameObject(hit.collider.gameObject);

				if (allowedBoxes.Contains(healedUnit.Position) && !allowedAttack(currentUnit, healedUnit)){

					float probability = UnityEngine.Random.Range (0, 100);
					if (probability < currentUnit.HabilityCritic) {
						Debug.Log ("cura critica");
						healedUnit.CurrentLife += currentUnit.GetHeal (healedUnit) * 2;

					} else {
						Debug.Log ("cura normal");
						healedUnit.CurrentLife += currentUnit.GetHeal (healedUnit);
					}

					if (healedUnit.CurrentLife > healedUnit.Life) {
						healedUnit.CurrentLife = healedUnit.Life;
					}

					matchManagment.NextTurn ();
				}
			}
		}
	}

	public void Focus(Unit currentUnit, List<Vector2> allowedBoxes){
		RaycastHit hit;

		if (Physics.Raycast (Camera.main.ScreenPointToRay (Input.mousePosition), out hit)) {
			if (hit.collider.tag.Equals ("Enemy") || hit.collider.tag.Equals ("Player")) {

				Unit focusedUnit = matchManagment.UnitFromGameObject(hit.collider.gameObject);
			
				if (allowedBoxes.Contains(focusedUnit.Position) && allowedAttack(currentUnit, focusedUnit)){

					if (!focusedUnit.Focused) {
						float probability = UnityEngine.Random.Range (0, 100);
						if (probability > (100 - focusedUnit.Agility)) {
							Debug.Log ("fallo focus");
							matchManagment.NextTurn ();

						} else {
							Debug.Log ("acierto focus");
							focusedUnit.Focused = true;
							focusedUnit.FocusedCount = 6;

							matchManagment.focused.SetActive (true);
							GameObject focusedIcon = Instantiate (matchManagment.focused);
							matchManagment.focused.SetActive (false);

							focusedIcon.transform.position = hit.collider.transform.position + Vector3.up * 2;
							focusedIcon.transform.SetParent (hit.collider.transform);
						}

						matchManagment.NextTurn ();
					}
				}
			}
		}
	}

	public void Agro(Unit currentUnit){
		if (matchManagment.GetUnitTeam (currentUnit).Equals (matchManagment.team_1_unitList)) {
			if (matchManagment.team_1_Agro == null) {
				matchManagment.agro.SetActive (true);
				GameObject agroIcon = Instantiate (matchManagment.agro);
				matchManagment.agro.SetActive (false);

				agroIcon.transform.position = matchManagment.GameObjectFromUnit(currentUnit).transform.position + Vector3.up * 2;
				agroIcon.transform.SetParent (matchManagment.GameObjectFromUnit(currentUnit).transform);

				matchManagment.team_1_Agro = currentUnit;
				matchManagment.team_1_AgroCount = 5;

				matchManagment.NextTurn ();
			}
		} 
		else {
			if (matchManagment.team_2_Agro == null) {

				matchManagment.agro.SetActive (true);
				GameObject agroIcon = Instantiate (matchManagment.agro);
				matchManagment.agro.SetActive (false);

				agroIcon.transform.position = matchManagment.GameObjectFromUnit(currentUnit).transform.position + Vector3.up * 2;
				agroIcon.transform.SetParent (matchManagment.GameObjectFromUnit(currentUnit).transform);
				matchManagment.team_2_Agro = currentUnit;
				matchManagment.team_2_AgroCount = 5;

				matchManagment.NextTurn ();
			}
		}
	}

	public void Area(Unit currentUnit, List<Vector2> allowedBoxes){
		RaycastHit hit;

		if (Physics.Raycast (Camera.main.ScreenPointToRay (Input.mousePosition), out hit)) {

			if (hit.collider.tag.Equals ("Ground")) {

				Vector2 position = worldToMap (hit.collider.gameObject);
				if (allowedBoxes.Contains (position)) {

					if (matchManagment.GetUnitTeam (currentUnit).Equals (matchManagment.team_1_unitList)) {

						List<Unit> deadUnits = new List<Unit> ();

						foreach (Unit unit in matchManagment.team_2_unitList) {
							if (allowedBoxes.Contains (unit.Position)) {
					
								float probability = UnityEngine.Random.Range (0, 100);
								if (probability > (100 - unit.Agility)) {
									//acierto el área en esta unidad
									int value = UnityEngine.Random.Range ((int)currentUnit.GetArea (unit).x, (int)currentUnit.GetArea (unit).y);
									//critico??

									probability = UnityEngine.Random.Range (0, 100);
									if (probability < currentUnit.Critic / 2) {
										//critico
										unit.CurrentLife -= value * 2;
									} else {
										//no critico
										unit.CurrentLife -= value;
									}

									if (unit.CurrentLife <= 0) {
										deadUnits.Add(unit);
									}
								}
							}
						}

						foreach (Unit unit in deadUnits) {
							if (unit.CurrentLife <= 0) {
								matchManagment.RemoveUnit (unit);
							}
						}

					} else {

						List<Unit> deadUnits = new List<Unit> ();

						foreach (Unit unit in matchManagment.team_1_unitList) {
							if (allowedBoxes.Contains (unit.Position)) {
								float probability = UnityEngine.Random.Range (0, 100);
								if (probability > (100 - unit.Agility)) {
									//acierto el área en esta unidad
									int value = UnityEngine.Random.Range ((int)currentUnit.GetArea (unit).x, (int)currentUnit.GetArea (unit).y);
									//critico??

									probability = UnityEngine.Random.Range (0, 100);
									if (probability < currentUnit.Critic / 2) {
										//critico
										unit.CurrentLife -= value * 2;
									} else {
										//no critico
										unit.CurrentLife -= value;
									}

									if (unit.CurrentLife <= 0) {
										deadUnits.Add(unit);
									}
								}
							}
						}

						foreach (Unit unit in deadUnits) {
							if (unit.CurrentLife <= 0) {
								matchManagment.RemoveUnit (unit);
							}
						}
					}
						

					matchManagment.NextTurn ();
				}
			}
		}
		
	}

	private bool allowedAttack(Unit unit1, Unit unit2){
		if (!matchManagment.GetUnitTeam (unit1).Equals (matchManagment.GetUnitTeam (unit2)))
			return true;
		return false;
	}

	private bool someoneIsAgro(Unit victimUnit){
		if (matchManagment.GetUnitTeam (victimUnit).Equals (matchManagment.team_1_unitList) && matchManagment.team_1_Agro != null) {
			return true;
		} else 
			if (matchManagment.GetUnitTeam (victimUnit).Equals (matchManagment.team_2_unitList) && matchManagment.team_2_Agro != null) {
			return true;
		}
		return false;
	}

	private bool isUnitAgro(Unit victimUnit){
		if (matchManagment.GetUnitTeam (victimUnit).Equals (matchManagment.team_1_unitList) && matchManagment.team_1_Agro.Equals (victimUnit)) {
			return true;
		}
		if (matchManagment.GetUnitTeam (victimUnit).Equals (matchManagment.team_2_unitList) && matchManagment.team_2_Agro.Equals (victimUnit)) {
			return true;
		}
		return false;
	}
}
