using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Functions {

    //Declarar QSceneMagager;
    int action;
    public States QLear;
  
        
    //Tendremos un estado para cada rol.
    bool[] estadoTanqueA;
    String winner;

	public void entrenamiento(float[,] QTA, float[,] QTB, float[,] QHA, float[,] QHB, float[,] QMA, float[,] QMB, float[,] QDA, float[,] QDB, float learningRate, float discountFactor,float politicaA, float politicaB, List<Unit> TeamA, List<Unit> TeamB)
    {
        //Iniciar partida
        //Inicializar QSceneMager
       // estadoTanqueA = QLear.GetTankState(TeamA[0]);
        
        //TeamA = QSM.GetTeam()
        //TeamB = QSM.GetTeam()

        //Bucle que recorra los equipos. Un rol por turno.

        //La llamada cambia según el jugador y el rol
        for (int i = 0; i < 4; i++)
        {

            if (TeamA[i] == TeamA[0])
            {
                Debug.Log("Tank");
                action = getAction('A',politicaA, "Tanque", QTA, estadoTanqueA);
                bool[] estadoTanqueAT1 = DoAction('A', estadoTanqueA, action, "Tanque");
                winner = CheckWinner(estadoTanqueAT1);
                ActualizarQ(QTA);
                Debug.Log(action);
            }

            //si TeamA(0) 
            //si TeamA(1).... Dependiendo del rol se las variables a pasar a cada metodo seran distintas
            // action = getAction('A', politicaA, "Healer",QHA,estadoHealer);....

            //Si no hay ganador e i == 3 se reinicia el bucle
            

        }
        Debug.Log("Hola");
        //Esto es general, hay que cambiarlo
        /*/action = getAction('A', politicaA, "Tanque", QTA, estado);
        List<bool> estadoT1 = DoAction('A', estado, action, "Tanque");
        winner = CheckWinner(estadoT1);
        ActualizarQ(QTA);
        estado = estadoT1;*/



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
