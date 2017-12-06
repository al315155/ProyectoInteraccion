﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;

public class QLearning : MonoBehaviour {
//
//	QSceneManagment sceneManagment;
//
//	// Use this for initialization
//	void Start () {
//		sceneManagment = GameObject.Find ("Scene Managment").GetComponent<QSceneManagment> ();
//	}
//	
//	// Update is called once per frame
//	void Update () {
//		
//	}
//
//	private bool isHurted(Unit unit){
//		if (/*unit.currentLife < unit.minLife*/){
//			return true;
//		}
//		return false;
//	}
//
//	private void GetState(Unit unit){
//
//		//GetTeam(unit, "Rival") devuelve el equipo contrario a unit
//		//GetTeam(unit, "Ally") devuelve el equipo al que pertenece unit
//
//		//En idioma de Álvaro, ESTADOS
//		bool[] conditions = new bool[4];
//		bool isSomeoneHurted;
//
//		switch (/*unit.Rol*/){
//		case /*Rol.Tank*/:
//
//			//Primera condición: el estado de la vida de la propia unidad.
//			if (!isHurted(unit)){
//				conditions[0] = true;
//			} else {
//				conditions[0] = false;
//			}
//
//
//			//Segunda condicion: el estado de la vida de sus aliados
//			isSomeoneHurted = false;
//			foreach (Unit partner in sceneManagment.GetTeam(unit, "Ally")){
//				if (!unit.Equals(partner)){
//					if (isHurted(partner)){
//						isSomeoneHurted = true;
//					}
//				}
//			}
//			if (isSomeoneHurted) conditions[1] = false;
//			else conditions[1] = true;	
//
//			//Second condition
//			//Habrá que guardar quién está herido
//			//Si hay varios heridos, checkear quién está mas herido
//
//			//Quizá haya que separar el buscar al aliado más herido
//			//de ver si hay algun aliado herido (?)
////			bool IsSomeoneHurted = false;
////			int hurtedPartners = 0;
////			Unit hurtedPartner;
////			foreach(Unit partner in sceneManagment.GetTeam(unit)){
////				if (!unit.Equals(partner)){
////					if (isHurted(partner))
////					{
////						hurtedPartners += 1;
////						hurtedPartner = partner;
////					}
////				}
////			}
////
////			if (hurtedPartner == 0){
////				conditions[1] = false;
////			} else {
////				conditions[1] = true;
////
////				if (hurtedPartners > 1){
////
////				Unit weakest = hurtedPartner;
////
////				foreach (Unit partner in sceneManagment.GetTeam(unit)){
////
////					if (!unit.Equals(partner)){
////						if (isHurted(partner)){
////							if (/*partner.currentLife < weakest.currentLife*/){
////								weakest = partner;
////							}
////						}
////					}
////				}
////				hurtedPartner = weakest;
////				}
////			}
//
//			//Tercera condición: Existe un enemigo dentro del rango de ataque
//			if (sceneManagment.IsSomeoneNear(unit, sceneManager.GetTeam(unit, "Rival"), /*unit.attackRange*/))){
//				conditions[2] = true;
//			} else {conditions[2] = false;}
//			
//
//			//Cuarta condicion: Existe un aliado Focused
//			if (sceneManagment.IsSomeoneFocused(unit)){
//				conditions[3] = true;
//			} else {
//				conditions[3] = false;
//			}
//
//			break;
//
//
//
//
//		case /*Rol.Healer*/:
//
//			//Primera condición: el estado de la vida de la propia unidad.
//			if (!isHurted(unit)){
//				conditions[0] = true;
//			} else {
//				conditions[0] = false;
//			}
//
//			//Segunda condicion: tener dentro de su rango de movimiento a sus aliados
//			isSomeoneHurted = false;
//			foreach (Unit partner in sceneManagment.GetTeam(unit, "Ally")){
//				if (!unit.Equals(partner)){
//					if (isHurted(partner)){
//						isSomeoneHurted = true;
//					}
//				}
//			}
//			if (isSomeoneHurted) conditions[1] = false;
//			else conditions[1] = true;
//
//			//Tercera condición: tener un enemigo dentro de su rango de ataque
//			if (sceneManagment.IsSomeoneNear(unit, sceneManager.GetTeam(unit, "Rival"), /*unit.attackRange*/)){
//				conditions[2] = true;
//			} else {conditions[2] = false;}
//
//			//Cuarta condicion: tener un aliado dentro del rango de uso de habilidad
//			if (sceneManagment.IsSomeoneNear(unit, sceneManager.GetTeam(unit,"Ally"), /*unit.habilityRange*/)){
//				conditions[3] = true;
//			} else {
//				conditions[3] = false;
//			}
//
//			break;
//
//
//
//
//		case /*Rol.Mele Damage*/:
//
//			//Primera condición: el estado de la vida de la propia unidad.
//			if (!isHurted(unit)){
//				conditions[0] = true;
//			} else {
//				conditions[0] = false;
//			}
//
//			//Segunda condición: tener un enemigo dentro de su rango de ataque
//				if (sceneManagment.IsSomeoneNear(unit, sceneManager.GetTeam(unit, "Rival"), /*unit.attackRange*/)){
//				conditions[1] = true;
//			} else {conditions[1] = false;}
//
//			//Tercera condicion: tener 1 enemigo dentro del rango de habilidad
//			if (sceneManagment.IsSomeoneNear(unit, sceneManager.GetTeam(unit, "Rival"), /*unit.habilityRange*/)){
//				conditions[2] = true;
//			} else {
//				conditions[2] = false;
//			}
//		
//			//Cuarta condición: tener VARIOS enemigos dentro del rango de habilidad
//					if (sceneManagment.IsSomeoneNear(unit, sceneManagment.GetTeam(unit, "Rival"), /*unit.habilityRange*/)){
//				if(sceneManagment.ALotOfRivalsInMeleDamageArea(unit, sceneManagment.GetTeam(unit, "Rival"), /*unit.habilityRange*/){
//					conditions[3] = true;
//				}
//				else {conditions[3] = false;}
//			}
//			else {conditions[3] = false;}
//				
//
//			break;
//
//
//
//
//		case /*Rol.Distance Damage*/:
//
//			//Primera condición: el estado de la vida de la propia unidad.
//			if (!isHurted(unit)){
//				conditions[0] = true;
//			} else {
//				conditions[0] = false;
//			}
//
//			//Segunda condición: tener un enemigo dentro de su rango de ataque
//			if (sceneManagment.IsSomeoneNear(unit, sceneManager.GetTeam(unit, "Rival", /*unit.attackRange*/)){
//				conditions[1] = true;
//			} else {conditions[1] = false;}
//
//			//Tercera condición: contemplar el estado de vida de los enemigos (quién es más débil)
//			isSomeoneHurted = false;
//			foreach (Unit rival in sceneManagment.GetTeam(unit, "Rival")){
//				if (!unit.Equals(rival)){
//					if (isHurted(rival)){
//						isSomeoneHurted = true;
//					}
//				}
//			}
//			if (isSomeoneHurted) conditions[2] = true;
//			else conditions[2] = false;	
//
//			//Cuarta condición: enemigo dentro del rango de habilidad
//			if (sceneManagment.IsSomeoneNear(unit, sceneManager.GetTeam(unit,"Rival"), /*unit.habilityRange*/)){
//				conditions[3] = true;
//			} else {
//				conditions[3] = false;
//			}
//
//
//			break;
//		}
//
//		return conditions;
//	}
//
//
//

}