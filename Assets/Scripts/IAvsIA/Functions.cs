using  System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Functions {

//	 QLearningGame game;
//	 public States states;
//     IAActions actionsIA;
//
//    int action;
//	bool isGameOver = false;
//        
//    //Tendremos un estado para cada rol.
//    bool[] estadoTanqueA; // actual
//    bool[] estadoTanqueB;
//    bool[] estadoHealerA;
//    bool[] estadoHealerB;
//    bool[] estadoMeleA;
//    bool[] estadoMeleB;
//    bool[] estadoDistA;
//    bool[] estadoDistB;
//
//    List<Unit> teamA_this;
//    List<Unit> teamB_this;
//
//    float[,] thisQTA;
//    float[,] thisQTB;
//    float[,] thisQHA;
//    float[,] thisQHB;
//    float[,] thisQMA;
//    float[,] thisQMB;
//    float[,] thisQDA;
//    float[,] thisQDB;
//
//	int partidas = 0;
//	int partidasTotales = 50;
//
//	int movements;
//	QMatrix qmatrix;
//
//    float[,] QMatrix;
//
//	public Functions(QLearningGame game, States states, IAActions actions, int movements, QMatrix qmatrix)
//    {
//        this.actionsIA = actions;
//        this.game = game;
//        this.states = states;
//
//		this.movements = movements;
//		this.qmatrix = qmatrix;
//    }
//    public void BegginerTraining(float learningRate, float discountFactor, int politicaA, int politicaB, List<Unit> TeamA, List<Unit> TeamB)
//    {
//		partidas = 0;
//
//		while (partidas < partidasTotales) {
//			//Iniciar partida
//			int cont = 0;
//			game.StartGame (); // comienza los turnos, etc.
//			teamA_this = TeamA;
//			teamB_this = TeamB;
//
//			game.NextTurn ();
//
//			while (cont < movements) {
//
//				// pillo la unidad a la cual le toca.
//				Unit currentUnit = game.GetCurrentPlayer ();
//
//				// mirar a quién le toca de qué equipo.
//				if (QSceneManagment.GetUnitTeam (currentUnit, TeamA, TeamB).Equals (TeamA)) {
//					switch (currentUnit.UnitRol) {
//					case Rol.Tank:
//						estadoTanqueA = states.GetTankState (currentUnit);
//						QLearning (qmatrix.QMatrix_Begginer_A_Tank, estadoTanqueA, politicaA, TeamA, currentUnit, discountFactor, learningRate);
//						break;
//
//					case Rol.Healer:
//						estadoHealerA = states.GetHealerState (currentUnit);
//						QLearning (qmatrix.QMatrix_Begginer_A_Healer, estadoHealerA, politicaA, TeamA, currentUnit, discountFactor, learningRate);
//						break;
//					case Rol.Mele:
//						estadoMeleA = states.GetMeleConditions (currentUnit);
//						QLearning (qmatrix.QMatrix_Begginer_A_Mele, estadoMeleA, politicaA, TeamA, currentUnit, discountFactor, learningRate);
//						break;
//					case Rol.Distance:
//						estadoDistA = states.GetDistanceConditions (currentUnit);
//						QLearning (qmatrix.QMatrix_Begginer_A_Distance, estadoDistA, politicaA, TeamA, currentUnit, discountFactor, learningRate);
//						break;
//					}
//
//				} else {
//					switch (currentUnit.UnitRol) {
//					case Rol.Tank:
//						estadoTanqueB = states.GetTankState (currentUnit);
//						QLearning (qmatrix.QMatrix_Begginer_B_Tank, estadoTanqueB, politicaB, TeamB, currentUnit, discountFactor, learningRate);
//						break;
//					case Rol.Healer:
//						estadoHealerB = states.GetHealerState (currentUnit);
//						QLearning (qmatrix.QMatrix_Begginer_B_Healer, estadoHealerB, politicaB, TeamB, currentUnit, discountFactor, learningRate);
//						break;
//					case Rol.Mele:
//						estadoMeleB = states.GetMeleConditions (currentUnit);
//						QLearning (qmatrix.QMatrix_Begginer_B_Mele, estadoMeleB, politicaB, TeamB, currentUnit, discountFactor, learningRate);
//						break;
//					case Rol.Distance:
//						estadoDistB = states.GetDistanceConditions (currentUnit);
//						QLearning (qmatrix.QMatrix_Begginer_B_Distance, estadoDistB, politicaB, TeamB, currentUnit, discountFactor, learningRate);
//						break;
//					}
//				}
//
//				// tengo la matriz del personaje en el turno actual --> QMatrix
//				UpdateQs (teamA_this, teamB_this);
//				isGameOver = game.NextTurn ();
//				cont++;
//			}
//
//			partidas++;
//
//		}
//		for (int i = 0; i < 18; i++) {
//			for (int j = 0; j < 5; j++) {
//				Debug.Log (qmatrix.QMatrix_Begginer_A_Tank [i, j]);
//			}
//		}
//	}
//
//    private void UpdateQs(List<Unit> teamA_this, List<Unit> teamB_this)
//    {
//        Unit currentUnit;
//        Unit currentUnitB;
//		for (int i = 0; i < teamA_this.Count; i++) {
//			currentUnit = teamA_this [i];
//            
//			switch (currentUnit.UnitRol) {
//			case Rol.Tank:
//				estadoTanqueA = states.GetTankState (currentUnit);
//                       
//				break;
//			case Rol.Healer:
//				estadoHealerA = states.GetHealerState (currentUnit);
//                        
//				break;
//			case Rol.Mele:
//				estadoMeleA = states.GetMeleConditions (currentUnit);
//                        
//				break;
//			case Rol.Distance:
//				estadoDistA = states.GetDistanceConditions (currentUnit);
//                        
//				break;
//			}
//
//		}
//        for (int i = 0; i < teamB_this.Count; i++)
//        {
//            currentUnitB = teamB_this[i];
//
//			switch (currentUnitB.UnitRol) {
//			case Rol.Tank:
//				estadoTanqueB = states.GetTankState (currentUnitB);
//				break;
//
//			case Rol.Healer:
//				estadoHealerB = states.GetHealerState (currentUnitB);
//				break;
//
//			case Rol.Mele:
//				estadoMeleB = states.GetMeleConditions (currentUnitB);
//				break;
//
//			case Rol.Distance:
//				estadoDistB = states.GetDistanceConditions (currentUnitB);
//				break;
//			}
//        }
//    }
//
//    private void QLearning(float[,] Q, bool[] estado, int politica, List<Unit> team, Unit currentUnit,float discount, float learningRate)
//	{
//		action = selectActionAuto (Q, estado, politica);
//		bool[] estadoT1 = DoAction (estado, action, currentUnit);
//
//		ActualizarQ (Q, estado, estadoT1, team, currentUnit, discount, learningRate);
//
//		qmatrix.SaveQMatrix (Q, getRoute (currentUnit, team), 18, 5);
//		qmatrix.ChargeQMatrix (Q, getRoute (currentUnit, team), 18, 5);
//
//	}
//
//	private void ActualizarQ(float[,] q, bool[] estado, bool[] estadoT1, List<Unit> team, Unit currentUnit,float discount, float learningRate)
//	{
//		float max_value = GetMaxValue (q, estadoT1);
//		int reward = GetReward (estadoT1, currentUnit);
//		q [GetQRow (estado), action] += learningRate * (reward + discount * max_value - q [GetQRow (estado), action]);
//
//	}
//
//	private String getRoute(Unit unit, List<Unit> team){
//		if (team.Equals (teamA_this)) {
//			switch (unit.UnitRol) {
//			case Rol.Tank:
//				return qmatrix.Route_QMatrix_Begginer_A_Tank;
//			case Rol.Healer:
//				return qmatrix.Route_QMatrix_Begginer_A_Healer;
//			case Rol.Mele:
//				return qmatrix.Route_QMatrix_Begginer_A_Mele;
//			case Rol.Distance:
//				return qmatrix.Route_QMatrix_Begginer_A_Distance;
//			}
//
//		} else {
//			switch (unit.UnitRol) {
//			case Rol.Tank:
//				return qmatrix.Route_QMatrix_Begginer_B_Tank;
//			case Rol.Healer:
//				return qmatrix.Route_QMatrix_Begginer_B_Healer;
//			case Rol.Mele:
//				return qmatrix.Route_QMatrix_Begginer_B_Mele;
//			case Rol.Distance:
//				return qmatrix.Route_QMatrix_Begginer_B_Distance;
//			}
//		}
//
//		return "";
//	}
//
//	public int selectActionAuto(float[,] Q, bool[] estado, int politica)
//	{
//		if (politica == 0) {
//			return getRandomAction ();
//		} 
//		else {
//			System.Random rand = new System.Random ();
//			float x = rand.Next (1, 100);
//			if (x > politica) {
//				return getRandomAction ();
//			} else {
//				float maxValue = 0;
//				int iMax = 0;
//
//				//cambiar a 5 cuando se ñada la otra funcion de mover
//				for (int i = 0; i < 5; i++) {
//					if (Q [GetQRow (estado), i] > maxValue) {
//						maxValue = Q [GetQRow (estado), i];
//						iMax = i;
//					}
//				}
//				return iMax;
//			}
//		}
//	}
//
//	public int getRandomAction()
//	{
//		System.Random rd = new System.Random();
//		//cambiar cuando se añada función de mover 
//		int rand = rd.Next (0, 5);
//		return rand;
//	}
//
//	float GetMaxValue (float[,] q, bool[] estadoT1)
//	{
//        float maxValue = 0;
//        //cambiar a 5 cuando se ñada la otra funcion de mover
//        for (int i = 0; i < 4; i++)
//        {
//            if(q[GetQRow(estadoT1),i] > maxValue)
//            {
//                maxValue = q[GetQRow(estadoT1), i];
//            }
//        }
//		return maxValue;
//	}
//
//	private bool[] DoAction(bool[] estado, int action, Unit unit)
//	{
//		bool[] estadoT1 = estado;
//		switch (unit.UnitRol)
//		{
//		case Rol.Tank:
//			switch (action)
//			{   
//			//Atacar
//			case 0:
//				if(estado[2]== true)
//				{
//					actionsIA.IA_Attack(game.GetMap(),unit,QSceneManagment.GetEnemyTeam(unit,teamA_this,teamB_this),unit.AttackRange);
//				}
//				break;
//
//			//Agro
//			case 1:
//				if (estado[3] == true)
//				{
//					actionsIA.IA_Agro(unit, teamA_this, teamB_this);
//				}
//				break;
//				//Moverse hacia enemigo
//			case 2:
//				//Moverse
//				//como se quien es el traget?
//
//
//				actionsIA.GoNearer(game.GetMap(), unit,GetEnemy(teamA_this,teamB_this,unit));
//				break;
//				//No hacer nada
//			
//				//moverse lejos
//			case 4:
//				actionsIA.GoFarther(game.GetMap(), GetEnemy(teamA_this, teamB_this, unit), unit);
//				break;
//			case 3:
//				//nada
//				break;
//			}
//			//actualizar estadoT1;
//			estadoT1 = states.GetTankState(unit);
//
//			break;
//		case Rol.Healer:
//			switch (action)
//			{
//			//Atacar
//			case 0:
//				if(estado[2]== true)
//				{
//					//atacar
//					actionsIA.IA_Attack(game.GetMap(),unit,QSceneManagment.GetEnemyTeam(unit,teamA_this,teamB_this),unit.AttackRange);
//				}
//				break;
//				//Sanar
//			case 1:
//				if (estado[3] == true)
//				{
//					actionsIA.IA_Heal (game.GetMap(), unit, QSceneManagment.GetUnitTeam (unit, teamA_this, teamB_this),unit.HabilityRange);
//				}
//				break;
//				//Moverse cerca
//			case 2:
//				actionsIA.GoNearer(game.GetMap(), unit, GetEnemy(teamA_this, teamB_this, unit));
//				break;
//			case 3:
//				actionsIA.GoFarther(game.GetMap(), GetEnemy(teamA_this, teamB_this, unit), unit);
//				break;
//
//			case 4:
//				break;
//			}
//			//actualizar estadoT1:
//			estadoT1 = states.GetHealerState(unit);
//			break;
//		case Rol.Mele:
//			switch (action)
//			{   //atacar
//			case 0:
//				if (estado[1] == true)
//				{
//					actionsIA.IA_Attack(game.GetMap(),unit,QSceneManagment.GetEnemyTeam(unit,teamA_this,teamB_this),unit.AttackRange);
//				}
//				break;
//				//habilidad Area
//			case 1:
//				if(estado[2] == true || estado[3] == true)
//				{
//					//habilidad area
//					actionsIA.IA_Area(game.GetMap(), unit, teamA_this, teamB_this, unit.HabilityRange);
//				}
//				break;
//				//moverse cerca
//			case 2:
//				actionsIA.GoNearer(game.GetMap(), unit, GetEnemy(teamA_this, teamB_this, unit));
//				break;
//				//no hacer nada
//			case 3:
//				actionsIA.GoFarther(game.GetMap(), GetEnemy(teamA_this, teamB_this, unit), unit);
//				break;
//				//moverse lejos
//			case 4:
//				break;
//			}
//			estadoT1 = states.GetMeleConditions(unit);
//			break;
//		case Rol.Distance:
//			switch (action)
//			{   //Atacar
//			case 0:
//				if (estado[1] == true)
//				{
//					actionsIA.IA_Attack(game.GetMap(),unit,QSceneManagment.GetEnemyTeam(unit,teamA_this,teamB_this),unit.AttackRange);
//				}
//				break;
//				//Habilidad marcar
//			case 1:
//				if (estado[3] == true)
//				{
//					//marcar
//					actionsIA.IA_Focus(game.GetMap(),unit,QSceneManagment.GetEnemyTeam(unit,teamA_this,teamB_this),unit.HabilityRange);
//				}
//				break;
//				//moverse lejos
//			case 2:
//				actionsIA.GoNearer(game.GetMap(), unit, GetEnemy(teamA_this, teamB_this, unit));
//				break;
//				//no hacer nada
//			case 3:
//				actionsIA.GoFarther(game.GetMap(), GetEnemy(teamA_this, teamB_this, unit), unit);
//				break;
//				//moverse cerca
//			case 4:
//				break;
//			}
//			estadoT1 = states.GetDistanceConditions(unit);
//			break;
//		}
//
//		return estadoT1;
//	}
//
//    private int GetReward(bool[] estadoT1, Unit currentUnit)
//    {
//        int reward = 0;
//        int bestReward = 1000;
//        int worstReward = -1000;
//        int killEnemieReward = 100;
//        int goodReward = 10;
//        int goodLessReward = 1;
//        int badLessReward = -1;
//        int badReward = -10;
//
//
//		switch (currentUnit.UnitRol) {
//		case Rol.Tank:
//			switch (action) {
//
//			// SOY TANQUE Y HE ATACADO---------------------------------------------------------------------------
//			case 0:
//				if (estadoT1 [2] == true) {
//					if (estadoT1 [0] == true) {
//
//						if (actionsIA.isEnemyDead == true) {
//							if (teamA_this.Count == 0 || teamB_this.Count == 0) {
//								reward = bestReward;
//							} else if (teamA_this.Count != 0 && teamB_this.Count != 0) {
//								reward = killEnemieReward;
//							}
//							actionsIA.isEnemyDead = false;
//						} else {
//							reward = goodReward;
//						}
//					} else {
//						reward = goodLessReward; 
//					}
//				} else {
//					reward = badReward;
//				}
//				break;
//
//			// SOY TANQUE Y HE USADO HABILIDAD---------------------------------------------------------------------
//			case 1:
//				if (estadoT1 [0] == true) {
//                           
//					if (estadoT1 [1] == false || estadoT1 [3] == true) {
//						reward = goodReward;
//
//					} else {
//						reward = badReward;
//					}
//				} else {
//					reward = badReward;
//				}
//				break;
//			
//			// SOY TANQUE Y ME MUEVO HACIA EL OBJETIVO-------------------------------------------------------------
//			case 2:
//				if (estadoT1 [2] == true) {
//					reward = goodReward;
//				} else {
//				}
//				break;
//			
//			// SOY TANQUE Y ME ALEJO DEL EL OBJETIVO-------------------------------------------------------------
//			case 3:
//				if (estadoT1 [2] == true) {
//					reward = badReward;
//				} else {
//				}
//				break;
//
//			// SOY TANQUE Y NO HAGO NADA---------------------------------------------------------------------------
//			case 4: 
//				reward = badReward;
//				break;
//			}
//			break;
//
//
//		case Rol.Healer:
//			switch (action) {
//			// SOY HEALER Y ATACO---------------------------------------------------------------------------------
//			case 0:
//				if (estadoT1 [2] == true) {
//					if (estadoT1 [0] == true && estadoT1 [1] == true) {
//
//						if (actionsIA.isEnemyDead == true) {
//							if (teamA_this.Count == 0 || teamB_this.Count == 0) {
//								reward = bestReward;
//							} else if (teamA_this.Count != 0 && teamB_this.Count != 0) {
//								reward = killEnemieReward;
//							}
//							actionsIA.isEnemyDead = false;
//
//						} else {
//							reward = goodReward;
//						}
//
//					} else if (estadoT1 [0] == false || estadoT1 [1] == false) {
//						reward = badReward;
//					}
//				} else {
//					reward = badReward;
//				}
//
//				break;
//			
//			// SOY HEALER Y USO HABILIDAD---------------------------------------------------------------------------------
//			case 1:
//				if (estadoT1 [0] == false) {
//					reward = goodReward;
//				} else {
//					reward = badReward;
//				}
//				if (estadoT1 [3] == true) {
//					if (estadoT1 [1] == false) {
//						reward = goodReward;
//					} else {
//						reward = badReward;
//					}
//				} else {
//					reward = badReward;
//				}
//				break;
//			
//			// SOY HEALER Y ME ACERCO A UN ENEMIGO---------------------------------------------------------------------------------
//			case 2:
//				if (estadoT1 [2] == true || estadoT1 [3] == false) {
//					reward = badReward;
//				}
//				if (estadoT1 [2] == true || estadoT1 [3] == true) {
//					reward = goodLessReward;
//				}
//				if (estadoT1 [2] == false || estadoT1 [3] == true) {
//					reward = goodReward;
//				}
//				if (estadoT1 [2] == false || estadoT1 [3] == false) {
//					reward = badLessReward;
//				}
//				break;
//
//			// SOY HEALER Y ME ALEJO DE UN ENEMIGO---------------------------------------------------------------------------------
//			case 3:
//				if (estadoT1 [0] == false) {
//					reward = goodReward;
//				}
//				if (estadoT1 [0] == true || estadoT1 [3] == false) {
//					reward = badReward;
//				}	
//				break;
//			
//			// SOY HEALER Y NO HAGO NADA-----------------------------------------------------------------------------------------
//			case 4:
//				reward = badReward;
//				break;
//
//			}
//			break;
//
//
//		case Rol.Distance:
//			switch (action) {
//			// SOY DISTANCE Y ATACO-----------------------------------------------------------------------------------------
//			case 0:
//				if (estadoT1 [1] == true) {
//					if (actionsIA.isEnemyDead == true) {
//						if (teamA_this.Count == 0 || teamB_this.Count == 0) {
//							reward = bestReward;
//						} else if (teamA_this.Count != 0 && teamB_this.Count != 0) {
//							reward = killEnemieReward;
//						}
//						actionsIA.isEnemyDead = false;
//					} else {
//						reward = goodReward;
//					}
//				} else {
//					reward = badReward;
//				}
//				break;
//
//			// SOY DISTANCE Y USO HABILIDAD-----------------------------------------------------------------------------------------
//			case 1:
//				if (estadoT1 [1] == false || estadoT1 [3] == true) {
//					reward = goodReward;
//				} else if (estadoT1 [1] == true && estadoT1 [3] == true) {
//					reward = goodLessReward;
//				}
//				break;
//
//			// SOY DISTANCE Y ME MUEVO AL ENEMIGO------------------------------------------------------------------------------------
//			case 2:
//				if (estadoT1 [0] == false) {
//					reward = badReward;
//				} else {
//					reward = goodReward;
//				}
//				break;
//
//			// SOY DISTANCE Y ME ALEJO DEL ENEMIGO------------------------------------------------------------------------------------
//			case 3:
//				if (estadoT1 [0] == true) {
//					reward = badReward;
//				} else {
//					reward = goodReward;
//				}
//				break;
//
//			// SOY DISTANCE Y NO HAGO NADA-----------------------------------------------------------------------------------------
//			case 4:
//				reward = badReward;
//				break;
//			}
//			break;
//
//
//		case Rol.Mele:
//			switch (action) {
//
//			// SOY MELE Y ATACO--------------------------------------------------------------------------------------------
//			case 0:
//				if (estadoT1 [0] == true) {
//					if (estadoT1 [1] == true) {
//						// si mata a enemigo: KillEnemieReward
//						if (actionsIA.isEnemyDead == true) {
//							if (teamA_this.Count == 0 || teamB_this.Count == 0) {
//								reward = bestReward;
//							} else if (teamA_this.Count != 0 && teamB_this.Count != 0) {
//								reward = killEnemieReward;
//							}
//							actionsIA.isEnemyDead = false;
//
//						} else {
//							//si no:
//							reward = goodReward;
//						}
//					} else {
//						reward = badReward;
//					}
//				} else {
//					reward = badReward;
//				}
//				break;
//
//			// SOY MELE Y USO HABILIDAD--------------------------------------------------------------------------------------------
//			case 1:
//				if (estadoT1 [2] == true) {
//					if (estadoT1 [3] == true) {
//						reward = goodReward;
//					} else {
//						reward = goodLessReward;
//					}
//				} else {
//					reward = badReward;
//				}
//				break;
//
//			// SOY MELE Y ME ACERCO AL ENEMIGO--------------------------------------------------------------------------------------------
//			case 2:
//				if (estadoT1 [0] == true) {
//					reward = goodReward;
//				} else {
//					reward = badReward;
//				}
//				break;
//
//			// SOY MELE Y ME ALEJO DEL ENEMIGO--------------------------------------------------------------------------------------------
//			case 3:
//				if (estadoT1 [0] == false) {
//					reward = goodReward;
//				} else {
//					reward = badReward;
//				}
//				break;
//			
//			// SOY MELE Y NO HAGO NADA--------------------------------------------------------------------------------------------
//			case 4:
//				reward = badReward;
//				break;
//			}
//			break;
//		}
//		return reward;
//    }
//
//    private string CheckWinner(bool[] estadoT1)
//    {
//        return null;
//    }
//
//	Unit GetEnemy (List<Unit> teamA_this, List<Unit> teamB_this, Unit unit)
//	{
//		List <Unit> enemies = QSceneManagment.GetEnemyTeam(unit,teamA_this,teamB_this);
//		System.Random rd = new System.Random ();
//
//		int rand = rd.Next (0, enemies.Count);
//		return enemies [rand];
//	}
//		
//
//	int GetQRow(bool[] estado){
//		
//		if (estado [0] == true && estado [1] == true && estado [2] == true && estado [3] == true) {
//			return 0;
//		}
//		else if(estado [0] == false && estado [1] == true && estado [2] == true && estado [3] == true) {
//			return 1;
//		}
//		else if(estado [0] == false && estado [1] == false && estado [2] == true && estado [3] == true) {
//			return 2;
//		}
//		else if(estado [0] == false && estado [1] == false && estado [2] == false && estado [3] == true) {
//			return 3;
//		}
//		else if(estado [0] == false && estado [1] == false && estado [2] == false && estado [3] == false) {
//			return 4;
//		}
//		else if(estado [0] == true && estado [1] == true && estado [2] == true && estado [3] == false) {
//			return 5;
//		}
//		else if(estado [0] == true && estado [1] == true && estado [2] == false && estado [3] == false) {
//			return 6;
//		}
//		else if(estado [0] == true && estado [1] == false && estado [2] == false && estado [3] == false) {
//			return 7;
//		}
//		else if(estado [0] == true && estado [1] == false && estado [2] == false && estado [3] == true) {
//			return 8;
//		}
//		else if(estado [0] == false && estado [1] == true && estado [2] == true && estado [3] == false) {
//			return 9;
//		}
//		else if(estado [0] == true && estado [1] == false && estado [2] == true && estado [3] == false) {
//			return 10;
//		}
//		else if(estado [0] == false && estado [1] == true && estado [2] == false && estado [3] == true) {
//			return 11;
//		}
//		else if(estado [0] == true && estado [1] == false && estado [2] == true && estado [3] == true) {
//			return 12;
//		}
//		else if(estado [0] == false && estado [1] == true && estado [2] == false && estado [3] == false) {
//			return 13;
//		}
//		else if(estado [0] == true && estado [1] == true && estado [2] == false && estado [3] == true) {
//			return 14;
//		}
//		else /*(estado [0] == false && estado [0] == false && estado [0] == true && estado [0] == false) */{
//			return 15;
//		}
//       
//	}
//
//
//
//    public float[,] QTA{
//		set { thisQTA = value; }
//		get{ return thisQTA; }
//    }
//
//    public float[,] QTB
//    {
//		set{ thisQTB = value; }
//		get{
//			return thisQTB;
//		}
//    }
//
//	public float[,] QHA
//    {
//		set{ thisQHA = value; }
//		get{return thisQHA;}
//    }
//
//    public float[,] QHB
//    {
//		set{ thisQHB = value; }
//		get{return thisQHB;}
//    }
//
//    public float[,] QMA
//    {
//		set{ thisQMA = value; }
//		get{return thisQMA;}
//    }
//
//    public float[,] QMB
//    {
//		set{ thisQMB = value; }
//		get{
//			return thisQMB;
//		}
//    }
//
//    public float[,] QDA
//    {
//		set{ thisQDA = value; }
//		get{return thisQDA;}
//    }
//
//    public float[,] QDB
//    {
//		set{ thisQDB = value; }
//		get{return thisQDB;}
//    }
//
//
//

}
//Falta comprobar si es game over para poder cerrar el bucle. Decidir las recompensas restantes... y Ya? Estados ganar perder partida. Hablar sobre la accion atacar.



