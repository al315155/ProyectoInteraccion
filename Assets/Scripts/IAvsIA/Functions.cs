using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Functions {

	 QLearningGame game;
	 public States states;
	 public Functions(QLearningGame game, States states){
	 this.game = game;
	 this.states = states;
	}

    //Declarar QSceneMagager;
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
   

	float[,] QMatrix;

	public void entrenamiento(float[,] QTA, float[,] QTB, float[,] QHA, float[,] QHB, float[,] QMA, float[,] QMB, float[,] QDA, float[,] QDB, float learningRate, float discountFactor,float politicaA, float politicaB, List<Unit> TeamA, List<Unit> TeamB)
	{
		//Iniciar partida

		game.StartGame (); // comienza los turnos, etc.

//		while (!isGameOver) {

			// pillo la unidad a la cual le toca.
			Unit currentUnit = game.GetCurrentPlayer ();

			// mirar a quién le toca de qué equipo.
			if (QSceneManagment.GetUnitTeam (currentUnit, TeamA, TeamB).Equals (TeamA)) {
				switch (currentUnit.UnitRol) {
				case Rol.Tank:
					
                    QLearning(QTA, estadoTanqueA, politicaA, TeamA, currentUnit);
					break;
				case Rol.Healer:
                        QLearning(QHA, estadoHealerA, politicaA, TeamA, currentUnit);
                        break;
				case Rol.Mele:
                        QLearning(QMA, estadoMeleA, politicaA, TeamA, currentUnit);
                        break;
				case Rol.Distance:
                        QLearning(QDA, estadoDistA, politicaA, TeamA, currentUnit);
                        break;
				}

			} else {
				switch (currentUnit.UnitRol) {
				case Rol.Tank:
                        QLearning(QTB, estadoTanqueB, politicaB, TeamB, currentUnit);
                        break;
				case Rol.Healer:
                        QLearning(QHB, estadoHealerB, politicaB, TeamB, currentUnit);
                        break;
				case Rol.Mele:
                        QLearning(QMB, estadoMeleB, politicaB, TeamB, currentUnit);
                        break;
				case Rol.Distance:
                        QLearning(QDB, estadoDistB, politicaB, TeamB, currentUnit);
                        break;
				}
			}

			// tengo la matriz del personaje en el turno actual --> QMatrix
	




        



			isGameOver = game.NextTurn ();

		}
	
    

    private void QLearning(float[,] Q, bool[] estado, float politica, List<Unit> team, Unit currentUnit)
    {
       
            Debug.Log("Tank");
            action = getAction('A', politica, "Tanque", Q, estado);

            // nuevo estado (posterior) para actualizar la Q
            bool[] estadoT1 = DoAction('A', estado, action, currentUnit);
            
          
            ActualizarQ(Q,estado,estadoT1,team,currentUnit);
            Debug.Log(action);
        }

    private void ActualizarQ(float[,] q, bool[] estado, bool[] estadoT1, List<Unit> team, Unit currentUnit)
    {
        float reawrd = GetReward(estadoT1, currentUnit);
    }

    private float GetReward(bool[] estadoT1, Unit currentUnit)
    {
        switch (currentUnit.UnitRol)
        {
            case Rol.Tank:
                switch (action)
                {
                    //ataque
                    case 0:
                        if(estadoT1[2]== true)//habria que tener en cuenta tambien si hay un personaje aliado herido
                        {
                            //dar recompensa positiva
                        }
                        else
                        {
                            //no dar recompensa o dar recompesa negativa?
                        }

                        break;
                    //agro
                    case 1:
                        if(estadoT1[2] == true || estadoT1[3] == false)
                        {
                            //no dar recompensa o negativa?
                        }else
                        {
                            //dar recompensa
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
                        break;



                }
                break;
            case Rol.Healer:
                break;
            case Rol.Distance:
                break;
            case Rol.Mele:
                break;
        }

            return 0f;

    }

    private string CheckWinner(bool[] estadoT1)
    {
        return null;
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
                            //atacar enemigo en rango
                            //Necesito saber cual es el enemigo que está en rango
                        }
                        break;
                    //Agro
                    case 1:
                        if (estado[3] == true)
                        {
                            //Usar habilidad sobre aliado
                            //Necesito saber que aliado está focus
                        }
                        break;
                    //Moverse
                    case 2:
                        //Moverse
                        break;
                    //No hacer nada
                    case 3:
                        //nada
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
                        }
                        break;
                    //Sanar
                    case 1:
                        if (estado[3] == true)
                        {
                            //sanar
                        }
                        break;
                    //Moverse
                    case 2:

                        break;
                    //No hacer nada
                    case 3:
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
                            //atacar
                        }
                        break;
                    //habilidad Area
                    case 1:
                        if(estado[2] == true || estado[3] == true)
                        {
                            //habilidad area
                        }
                        break;
                    //moverse
                    case 2:
                        break;
                    //no hacer nada
                    case 3:
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
                            //atacar
                        }
                        break;
                    //Habilidad marcar
                    case 1:
                        if (estado[3] == true)
                        {
                            //marcar
                        }
                        break;
                    //moverse
                    case 2:
                        break;
                    //no hacer nada
                    case 3:
                        break;
                }
                estadoT1 = states.GetDistanceConditions(unit);
                break;
        }
        
        return estadoT1;
    }

    private int getAction(char player, float politica, String rol, float[,] Q, bool[] estado)
    {
        System.Random rd = new System.Random();
        int rand = rd.Next(1, 4);
        return rand;
    }

}

