﻿using  System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Functions {

	 QLearningGame game;
	 public States states;
     IAActions actionsIA;

    int action;
	bool isGameOver = false;
        
    //Tendremos un estado para cada rol.
    bool[] estadoTanqueA; // actual
    bool[] estadoTanqueB;
    bool[] estadoHealerA;
    bool[] estadoHealerB;
    bool[] estadoMeleA;
    bool[] estadoMeleB;
    bool[] estadoDistA;
    bool[] estadoDistB;

    List<Unit> teamA_this;
    List<Unit> teamB_this;

    float[,] thisQTA;
    float[,] thisQTB;
    float[,] thisQHA;
    float[,] thisQHB;
    float[,] thisQMA;
    float[,] thisQMB;
    float[,] thisQDA;
    float[,] thisQDB;


	int movements;
	QMatrix qmatrix;

    float[,] QMatrix;

	public Functions(QLearningGame game, States states, IAActions actions, int movements, QMatrix qmatrix)
    {
        this.actionsIA = actions;
        this.game = game;
        this.states = states;

		this.movements = movements;
		this.qmatrix = qmatrix;
    }
    public void entrenamiento(float[,] QTA, float[,] QTB, float[,] QHA, float[,] QHB, float[,] QMA, float[,] QMB, float[,] QDA, float[,] QDB, float learningRate, float discountFactor, int politicaA, int politicaB, List<Unit> TeamA, List<Unit> TeamB)
    {
        //Iniciar partida
		int cont = 0;
        game.StartGame(); // comienza los turnos, etc.
        teamA_this = TeamA;
        teamB_this = TeamB;

		game.NextTurn();


        thisQDA = QDA;
        thisQDB = QDB;
        thisQHA = QHA;
        thisQHB = QHB;
        thisQMA = QMA;
        thisQMB = QMB;
        thisQTA = QTA;
        thisQTB = QTB;
		while (cont<movements) {

			// pillo la unidad a la cual le toca.
			Unit currentUnit = game.GetCurrentPlayer ();

			// mirar a quién le toca de qué equipo.
			if (QSceneManagment.GetUnitTeam (currentUnit, TeamA, TeamB).Equals (TeamA)) {
				switch (currentUnit.UnitRol) {
				case Rol.Tank:
					estadoTanqueA = states.GetTankState (currentUnit);

					QLearning (QTA, estadoTanqueA, politicaA, TeamA, currentUnit, discountFactor, learningRate);
					break;
				case Rol.Healer:
					estadoHealerA = states.GetHealerState (currentUnit);
					QLearning (QHA, estadoHealerA, politicaA, TeamA, currentUnit, discountFactor, learningRate);
					break;
				case Rol.Mele:
					estadoMeleA = states.GetMeleConditions (currentUnit);
					QLearning (QMA, estadoMeleA, politicaA, TeamA, currentUnit, discountFactor, learningRate);
					break;
				case Rol.Distance:
					estadoDistA = states.GetDistanceConditions (currentUnit);
					QLearning (QDA, estadoDistA, politicaA, TeamA, currentUnit, discountFactor, learningRate);
					break;
				}

			} else {
				switch (currentUnit.UnitRol) {
				case Rol.Tank:
					estadoTanqueB = states.GetTankState (currentUnit);
					QLearning (QTB, estadoTanqueB, politicaB, TeamB, currentUnit, discountFactor, learningRate);
					break;
				case Rol.Healer:
					estadoHealerB = states.GetHealerState (currentUnit);
					QLearning (QHB, estadoHealerB, politicaB, TeamB, currentUnit, discountFactor, learningRate);
					break;
				case Rol.Mele:
					estadoMeleB = states.GetMeleConditions (currentUnit);
					QLearning (QMB, estadoMeleB, politicaB, TeamB, currentUnit, discountFactor, learningRate);
					break;
				case Rol.Distance:
					estadoDistB = states.GetMeleConditions (currentUnit);
					QLearning (QDB, estadoDistB, politicaB, TeamB, currentUnit, discountFactor, learningRate);
					break;
				}
			}

			// tengo la matriz del personaje en el turno actual --> QMatrix
			UpdateQs (teamA_this, teamB_this);
			isGameOver = game.NextTurn ();
			cont++;
		}

	

		Debug.Log ("entro y guardo");
		/*qmatrix.SaveQMatrix(QDA, qmatrix.Route_QMatrix_Begginer_A_Distance, 18, 5);
		qmatrix.SaveQMatrix(QDB, qmatrix.Route_QMatrix_Begginer_B_Distance, 18,5);
		qmatrix.SaveQMatrix(QHA, qmatrix.Route_QMatrix_Begginer_A_Healer, 18, 5);
		qmatrix.SaveQMatrix(QHB, qmatrix.Route_QMatrix_Begginer_B_Healer, 18, 5);
		qmatrix.SaveQMatrix(QMA, qmatrix.Route_QMatrix_Begginer_A_Mele, 18, 5);
		qmatrix.SaveQMatrix(QMB, qmatrix.Route_QMatrix_Begginer_B_Mele, 18, 5);
		qmatrix.SaveQMatrix(QTA, qmatrix.Route_QMatrix_Begginer_A_Tank, 18, 5);
		qmatrix.SaveQMatrix(QTB, qmatrix.Route_QMatrix_Begginer_B_Tank, 18, 5);*/
		
	}

    private void UpdateQs(List<Unit> teamA_this, List<Unit> teamB_this)
    {
        Unit currentUnit;
        Unit currentUnitB;
        for (int i = 0; i <teamA_this.Count; i++)
        {
            currentUnit = teamA_this[i];
            
                switch (currentUnit.UnitRol)
                {
                    case Rol.Tank:
                        estadoTanqueA = states.GetTankState(currentUnit);
                       
                        break;
                    case Rol.Healer:
                        estadoHealerA = states.GetHealerState(currentUnit);
                        
                        break;
                    case Rol.Mele:
                        estadoMeleA = states.GetMeleConditions(currentUnit);
                        
                        break;
                    case Rol.Distance:
                        estadoDistA = states.GetDistanceConditions(currentUnit);
                        
                        break;
                }

            }
        for (int i = 0; i < teamB_this.Count; i++)
        {
            currentUnitB = teamB_this[i];

            switch (currentUnitB.UnitRol)
            {
                case Rol.Tank:
                    estadoTanqueB = states.GetTankState(currentUnitB);

                    break;
                case Rol.Healer:
                    estadoHealerB = states.GetHealerState(currentUnitB);

                    break;
                case Rol.Mele:
                    estadoMeleB = states.GetMeleConditions(currentUnitB);

                    break;
                case Rol.Distance:
                    estadoDistB = states.GetMeleConditions(currentUnitB);

                    break;
            }
        }

    }
    

    private void QLearning(float[,] Q, bool[] estado, int politica, List<Unit> team, Unit currentUnit,float discount, float learningRate)
    {
       
            Debug.Log("Tank");
		Debug.Log ("Estado"+estadoTanqueA);
            action = selectActionAuto(Q,estado,politica);

            // nuevo estado (posterior) para actualizar la Q
		bool[] estadoT1 = DoAction('A', estado, action, currentUnit);
            
          
            ActualizarQ(Q,estado,estadoT1,team,currentUnit,discount,learningRate);
            Debug.Log(action);
        }

	private void ActualizarQ(float[,] q, bool[] estado, bool[] estadoT1, List<Unit> team, Unit currentUnit,float discount, float learningRate)
    {
		float max_value = GetMaxValue (q, estadoT1);
        int reward = GetReward(estadoT1, currentUnit);
		q [GetQRow (estado), action] += learningRate * (reward + discount * max_value - q [GetQRow (estado), action]);

    }

	float GetMaxValue (float[,] q, bool[] estadoT1)
	{
        float maxValue = 0;
        //cambiar a 5 cuando se ñada la otra funcion de mover
        for (int i = 0; i < 4; i++)
        {
            if(q[GetQRow(estadoT1),i] > maxValue)
            {
                maxValue = q[GetQRow(estadoT1), i];
            }
        }
		return maxValue;
	}

    private int GetReward(bool[] estadoT1, Unit currentUnit)
    {
        int reward = 0;
        int bestReward = 1000;
        int worstReward = -1000;
        int killEnemieReward = 100;
        int goodReward = 10;
        int goodLessReward = 1;
        int badLessReward = -1;
        int badReward = -10;
        switch (currentUnit.UnitRol)
        {
            case Rol.Tank:
                switch (action)
                {
                    //ataque
                    case 0:
                        if(estadoT1[2]== true)//habria que tener en cuenta tambien si hay un personaje aliado herido
                        {
                            //provisional
                            //si la vida es superior a 40%
                            if (estadoT1[0] == true)
                            {
                                // si mata a enemigo: KillEnemieReward
                                if (actionsIA.isEnemyDead == true)
                                {
                                    if (teamA_this.Count == 0 || teamB_this.Count == 0)
                                    {
                                        reward = bestReward;
                                    }
                                    else if (teamA_this.Count != 0 && teamB_this.Count != 0)
                                    {
                                        reward = killEnemieReward;
                                    }
                                    actionsIA.isEnemyDead = false;
                                }
                                else
                                {
                                    //si no:
                                    reward = goodReward;
                                }
                            }
                            else
                            {
                                //lo mismo si mata al enemigo
                                //si no:
                                reward = goodLessReward; //O badLessReward?
                            }
                        }
                  
                        else
                        {
                            reward = badReward;
                        }

                        break;
                    //agro
			case 1:
                        //Consultar
				if (estadoT1 [0] == true) {
                           
					if (estadoT1 [1] == false || estadoT1 [3] == true) {
						reward = goodReward;

					} else {
						reward = badReward;
					}
				}
				else
                {
					reward = badReward;
                }
                        //falta tener en cuenta la propia salud del tanque para dar rencompesa negativa.
                        break;
                    //moverse
                    case 2:
                        /*
                         moverse hacia el enemigo
                         salud > 40:
                            recompensa positiva
                         
                         */
                        break;
                    case 3:
                        reward = badReward;
                        break;



                }
                break;
            case Rol.Healer:

			switch (action)
			{
			//ataque
			case 0:
				/*
				 * Si enemigo dentro de rango:
                 * 
				 * 		Si saludHealer ok y compañeros ok:
				 * 			es bueno
				 * 		Si compañeros no ok o yo no ok:
				 * 			malo
				 * 	Si fuera de rango:
				 * 		malo
				 */
                 if(estadoT1[2] == true)
                        {
                            if(estadoT1[0] == true && estadoT1[1] == true)
                            {
                                // si mata a enemigo: KillEnemieReward
                                if (actionsIA.isEnemyDead == true)
                                {
                                    if (teamA_this.Count == 0 || teamB_this.Count == 0)
                                    {
                                        reward = bestReward;
                                    }
                                    else if (teamA_this.Count != 0 && teamB_this.Count != 0)
                                    {
                                        reward = killEnemieReward;
                                    }
                                    actionsIA.isEnemyDead = false;

                                }
                                else
                                {
                                    //si no:
                                    reward = goodReward;
                                }

                            }
                            else if(estadoT1[0]== false || estadoT1[1]== false)
                            {
                                reward = badReward;
                            }
                        }
                 else
                        {
                            reward = badReward;
                        }

				break;

			case 1:

                        /*
                         *Si Healer poca vida:
                         *	Bueno
                         *Si no si mucha vida healer:
                         *	malo
                         *Si aliado dentro de rango:
                         *	Si poca vida:
                         *		bueno
                         *	Si no:
                         *		malo
                         *si fuera rango:
                         *		malo
                        
                         *  */
                        if (estadoT1[0] == false)
                        {
                            reward = goodReward;
                        }
                        else
                        {
                            reward = badReward;
                        }
                        if (estadoT1[3] == true)
                        {
                            if (estadoT1[1] == false)
                            {
                                reward = goodReward;
                            }
                            else
                            {
                                reward = badReward;
                            }
                        }
                        else
                        {
                            reward = badReward;
                        }
                    break;
			case 2:
				/*
                       
                         
                         */
				break;
			case 3:
				/*
				 */
				reward = badReward;
				break;



			}
			
                break;
            case Rol.Distance:
                switch (action)
                {
			case 0:
				if (estadoT1 [1] == true) {
                            // si mata a enemigo: KillEnemieReward
                            if (actionsIA.isEnemyDead == true)
                            {
                                if (teamA_this.Count == 0 || teamB_this.Count == 0)
                                {
                                    reward = bestReward;
                                }
                                else if (teamA_this.Count != 0 && teamB_this.Count != 0)
                                {
                                    reward = killEnemieReward;
                                }
                                actionsIA.isEnemyDead = false;
                            }
                            else
                            {
                                //si no:
                                reward = goodReward;
                            }
                        } else {
					reward = badReward;
				}

                        break;
				//Marcar
			case 1:
				if (estadoT1 [1] = false && estadoT1 [3] == true) {
					reward = goodReward;
				} else if (estadoT1 [1] == true && estadoT1 [3] == true) {
					reward = goodLessReward;
				}
                        break;
                    case 2:
				
                        break;
			case 3:
				reward = badReward;
                        break;
                }
                break;
            case Rol.Mele:
                switch (action)
                {
					//ataque
			case 0:
				if (estadoT1 [0] == true) {
					if (estadoT1 [1] == true) {
                                // si mata a enemigo: KillEnemieReward
                                if (actionsIA.isEnemyDead == true)
                                {
                                    if (teamA_this.Count == 0 || teamB_this.Count == 0)
                                    {
                                        reward = bestReward;
                                    }
                                    else if (teamA_this.Count != 0 && teamB_this.Count != 0)
                                    {
                                        reward = killEnemieReward;
                                    }
                                    actionsIA.isEnemyDead = false;

                                }
                                else
                                {
                                    //si no:
                                    reward = goodReward;
                                }
                            } else {
						reward = badReward;
					}
				} else {
					reward = badReward;
				}
				    break;
				//Area
			case 1:
				if (estadoT1 [2] == true) {
					if (estadoT1 [3] == true) {
						reward = goodReward;
					} else {
						reward = goodLessReward;
					}


				}else{
					reward = badReward;
				}
                        break;
                    case 2:
                        break;
			case 3:
				reward = badReward;
                        break;
                }
                break;
        }

            return reward;

    }

    private string CheckWinner(bool[] estadoT1)
    {
        return null;
    }

	Unit GetEnemy (List<Unit> teamA_this, List<Unit> teamB_this, Unit unit)
	{
		List <Unit> enemies = QSceneManagment.GetEnemyTeam(unit,teamA_this,teamB_this);
		System.Random rd = new System.Random ();

		int rand = rd.Next (0, enemies.Count);
		return enemies [rand];
	}

    private bool[] DoAction(char player, bool[] estado, int action, Unit unit)
    {
        bool[] estadoT1 = estado;
        switch (unit.UnitRol)
        {
            case Rol.Tank:
                switch (action)
                {   //Atacar
                    case 0:
                        if(estado[2]== true)
                        {
					actionsIA.IA_Attack(game.GetMap(),unit,QSceneManagment.GetEnemyTeam(unit,teamA_this,teamB_this),unit.AttackRange);
                        }
                        break;
                    //Agro
                    case 1:
                        if (estado[3] == true)
                        {
                            actionsIA.IA_Agro(unit, teamA_this, teamB_this);
                        }
                        break;
                    //Moverse hacia enemigo
			case 2:
                        //Moverse
				        //como se quien es el traget?

						
				actionsIA.GoNearer(game.GetMap(), unit,GetEnemy(teamA_this,teamB_this,unit));
                        break;
                    //No hacer nada
                    case 3:
                        //nada
                        break;
                    //moverse lejos
					case 4:
				actionsIA.GoFarther(game.GetMap(), GetEnemy(teamA_this, teamB_this, unit), unit);
						break;
                }
                //actualizar estadoT1;
                estadoT1 = states.GetTankState(unit);
                
                break;
            case Rol.Healer:
                switch (action)
                {
                    //Atacar
                    case 0:
                        if(estado[2]== true)
                        {
                            //atacar
					actionsIA.IA_Attack(game.GetMap(),unit,QSceneManagment.GetEnemyTeam(unit,teamA_this,teamB_this),unit.AttackRange);
                        }
                        break;
                    //Sanar
                    case 1:
                        if (estado[3] == true)
                        {
					actionsIA.IA_Heal (game.GetMap(), unit, QSceneManagment.GetUnitTeam (unit, teamA_this, teamB_this),unit.HabilityRange);
                        }
                        break;
                    //Moverse cerca
                    case 2:
				actionsIA.GoNearer(game.GetMap(), unit, GetEnemy(teamA_this, teamB_this, unit));
                        break;
                    //No hacer nada
                    case 3:
                        break;
                    //moverse lejos
                    case 4:
				actionsIA.GoFarther(game.GetMap(), GetEnemy(teamA_this, teamB_this, unit), unit);
                        break;
                }
                //actualizar estadoT1:
                estadoT1 = states.GetHealerState(unit);
                break;
            case Rol.Mele:
                switch (action)
                {   //atacar
                    case 0:
                        if (estado[1] == true)
                        {
					actionsIA.IA_Attack(game.GetMap(),unit,QSceneManagment.GetEnemyTeam(unit,teamA_this,teamB_this),unit.AttackRange);
                        }
                        break;
                    //habilidad Area
                    case 1:
                        if(estado[2] == true || estado[3] == true)
                        {
                            //habilidad area
					actionsIA.IA_Area(game.GetMap(), unit, teamA_this, teamB_this, unit.HabilityRange);
                        }
                        break;
                    //moverse cerca
                    case 2:
				actionsIA.GoNearer(game.GetMap(), unit, GetEnemy(teamA_this, teamB_this, unit));
                        break;
                    //no hacer nada
                    case 3:
                        break;
                    //moverse lejos
                    case 4:
				actionsIA.GoFarther(game.GetMap(), GetEnemy(teamA_this, teamB_this, unit), unit);
                        break;
                }
                estadoT1 = states.GetMeleConditions(unit);
                break;
            case Rol.Distance:
                switch (action)
                {   //Atacar
                    case 0:
                        if (estado[1] == true)
                        {
					actionsIA.IA_Attack(game.GetMap(),unit,QSceneManagment.GetEnemyTeam(unit,teamA_this,teamB_this),unit.AttackRange);
                        }
                        break;
                    //Habilidad marcar
                    case 1:
                        if (estado[3] == true)
                        {
                            //marcar
					actionsIA.IA_Focus(game.GetMap(),unit,QSceneManagment.GetEnemyTeam(unit,teamA_this,teamB_this),unit.HabilityRange);
                        }
                        break;
                    //moverse lejos
                    case 2:
				actionsIA.GoNearer(game.GetMap(), unit, GetEnemy(teamA_this, teamB_this, unit));
                        break;
                    //no hacer nada
                    case 3:
                        break;
                    //moverse cerca
                    case 4:
				actionsIA.GoFarther(game.GetMap(), GetEnemy(teamA_this, teamB_this, unit), unit);
                        break;
                }
                estadoT1 = states.GetDistanceConditions(unit);
                break;
        }
        
        return estadoT1;
    }



	public int getAction()
	{
		System.Random rd = new System.Random();
        //cambiar cuando se añada función de mover 
		int rand = rd.Next (0, 5);
		return rand;
	}

	int GetQRow(bool[] estado){
		
		if (estado [0] == true && estado [1] == true && estado [2] == true && estado [3] == true) {
			return 0;
		}
		else if(estado [0] == false && estado [1] == true && estado [2] == true && estado [3] == true) {
			return 1;
		}
		else if(estado [0] == false && estado [1] == false && estado [2] == true && estado [3] == true) {
			return 2;
		}
		else if(estado [0] == false && estado [1] == false && estado [2] == false && estado [3] == true) {
			return 3;
		}
		else if(estado [0] == false && estado [1] == false && estado [2] == false && estado [3] == false) {
			return 4;
		}
		else if(estado [0] == true && estado [1] == true && estado [2] == true && estado [3] == false) {
			return 5;
		}
		else if(estado [0] == true && estado [1] == true && estado [2] == false && estado [3] == false) {
			return 6;
		}
		else if(estado [0] == true && estado [1] == false && estado [2] == false && estado [3] == false) {
			return 7;
		}
		else if(estado [0] == true && estado [1] == false && estado [2] == false && estado [3] == true) {
			return 8;
		}
		else if(estado [0] == false && estado [1] == true && estado [2] == true && estado [3] == false) {
			return 9;
		}
		else if(estado [0] == true && estado [1] == false && estado [2] == true && estado [3] == false) {
			return 10;
		}
		else if(estado [0] == false && estado [1] == true && estado [2] == false && estado [3] == true) {
			return 11;
		}
		else if(estado [0] == true && estado [1] == false && estado [2] == true && estado [3] == true) {
			return 12;
		}
		else if(estado [0] == false && estado [1] == true && estado [2] == false && estado [3] == false) {
			return 13;
		}
		else if(estado [0] == true && estado [1] == true && estado [2] == false && estado [3] == true) {
			return 14;
		}
		else /*(estado [0] == false && estado [0] == false && estado [0] == true && estado [0] == false) */{
			return 15;
		}
       
	}



    public float[,] QTA{
		set { thisQTA = value; }
		get{ return thisQTA; }
    }

    public float[,] QTB
    {
		set{ thisQTB = value; }
		get{
			return thisQTB;
		}
    }

	public float[,] QHA
    {
		set{ thisQHA = value; }
		get{return thisQHA;}
    }

    public float[,] QHB
    {
		set{ thisQHB = value; }
		get{return thisQHB;}
    }

    public float[,] QMA
    {
		set{ thisQMA = value; }
		get{return thisQMA;}
    }

    public float[,] QMB
    {
		set{ thisQMB = value; }
		get{
			return thisQMB;
		}
    }

    public float[,] QDA
    {
		set{ thisQDA = value; }
		get{return thisQDA;}
    }

    public float[,] QDB
    {
		set{ thisQDB = value; }
		get{return thisQDB;}
    }

    public int selectActionAuto(float[,] Q, bool[] estado, int politica)
    {
        if (politica == 0)
        {
            return getAction();
        }
        else
        {
            System.Random rand = new System.Random();
            float x = rand.Next(1,100);
            if (x > politica)
            {
                return getAction();
            }
            else
            {
                float maxValue = 0;
                int iMax = 0;
                //cambiar a 5 cuando se ñada la otra funcion de mover
                for (int i = 0; i < 4; i++)
                {
                    if (Q[GetQRow(estado), i] > maxValue)
                    {
                        maxValue = Q[GetQRow(estado), i];
                        iMax = i;
                    }
                }
                return iMax;
            }

        }
    }


}
//Falta comprobar si es game over para poder cerrar el bucle. Decidir las recompensas restantes... y Ya? Estados ganar perder partida. Hablar sobre la accion atacar.



