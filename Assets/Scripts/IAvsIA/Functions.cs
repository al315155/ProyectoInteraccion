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
					
                    QLearning(QTA, estadoTanqueA, politicaA, TeamA, currentUnit.UnitRol);
					break;
				case Rol.Healer:
                        QLearning(QHA, estadoHealerA, politicaA, TeamA, currentUnit.UnitRol);
                        break;
				case Rol.Mele:
                        QLearning(QMA, estadoMeleA, politicaA, TeamA, currentUnit.UnitRol);
                        break;
				case Rol.Distance:
                        QLearning(QDA, estadoDistA, politicaA, TeamA, currentUnit.UnitRol);
                        break;
				}

			} else {
				switch (currentUnit.UnitRol) {
				case Rol.Tank:
                        QLearning(QTB, estadoTanqueB, politicaB, TeamB, currentUnit.UnitRol);
                        break;
				case Rol.Healer:
                        QLearning(QHB, estadoHealerB, politicaB, TeamB, currentUnit.UnitRol);
                        break;
				case Rol.Mele:
                        QLearning(QMB, estadoMeleB, politicaB, TeamB, currentUnit.UnitRol);
                        break;
				case Rol.Distance:
                        QLearning(QDB, estadoDistB, politicaB, TeamB, currentUnit.UnitRol);
                        break;
				}
			}

			// tengo la matriz del personaje en el turno actual --> QMatrix
	





			// estadoTanqueA = QLear.GetTankState(TeamA[0]);
        
			//TeamA = QSM.GetTeam()
			//TeamB = QSM.GetTeam()

			//Bucle que recorra los equipos. Un rol por turno.

			//La llamada cambia según el jugador y el rol
		

				//si TeamA(0) 
				//si TeamA(1).... Dependiendo del rol se las variables a pasar a cada metodo seran distintas
				// action = getAction('A', politicaA, "Healer",QHA,estadoHealer);....

				//Si no hay ganador e i == 3 se reinicia el bucle
            



			isGameOver = game.NextTurn ();

		}
			//Esto es general, hay que cambiarlo
			/*/action = getAction('A', politicaA, "Tanque", QTA, estado);
        List<bool> estadoT1 = DoAction('A', estado, action, "Tanque");
        winner = CheckWinner(estadoT1);
        ActualizarQ(QTA);
        estado = estadoT1;*/

//		}
    

    private void QLearning(float[,] Q, bool[] estado, float politica, List<Unit> team, Rol unitRol)
    {
       
            Debug.Log("Tank");
            action = getAction('A', politica, "Tanque", Q, estado);

            // nuevo estado (posterior) para actualizar la Q
            bool[] estadoTanqueAT1 = DoAction('A', estado, action, "Tanque");
          
            ActualizarQ(Q);
            Debug.Log(action);
        }

    private void ActualizarQ(float[,] Q)
    {
        
    }

    private string CheckWinner(bool[] estadoT1)
    {
        return null;
    }

    private bool[] DoAction(char player, bool[] estado, int action, string rol)
    {
        return null;
    }

    private int getAction(char player, float politica, String rol, float[,] Q, bool[] estado)
    {
        System.Random rd = new System.Random();
        int rand = rd.Next(1, 4);
        return rand;
    }

}

