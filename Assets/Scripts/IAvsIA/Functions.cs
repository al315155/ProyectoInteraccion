using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Functions {

    //Declarar QSceneMagager;
    int action;
    //Tendremos un estado para cada rol.
    List<bool> estado;
    String winner;

	public void entrenamiento(float[,] QTA, float[,] QTB, float[,] QHA, float[,] QHB, float[,] QMA, float[,] QMB, float[,] QDA, float[,] QDB, float learningRate, float discountFactor,float politicaA, float politicaB)
    {
        //Iniciar partida
        //Inicializar QSceneMager
        //TeamA = QSM.GetTeam()
        //TeamB = QSM.GetTeam()

        //Bucle que recorra los equipos. Un rol por turno.

        //La llamada cambia según el jugador y el rol
        for (int i = 0; i < 4; i++)
        {
            //si TeamA(0) 
            //si TeamA(1).... Dependiendo del rol se las variables a pasar a cada metodo seran distintas
            // action = getAction('A', politicaA, "Healer",QHA,estadoHealer);....

            //Si no hay ganador e i == 3 se reinicia el bucle

        }
        //Esto es general, hay que cambiarlo
        action = getAction('A', politicaA, "Tanque", QTA, estado);
        List<bool> estadoT1 = DoAction('A', estado, action, "Tanque");
        winner = CheckWinner(estadoT1);
        ActualizarQ(QTA);
        estado = estadoT1;



    }

    private void ActualizarQ(float[,] Q)
    {
        
    }

    private string CheckWinner(List<bool> estadoT1)
    {
        return null;
    }

    private List<bool> DoAction(char player, List<bool> estado, int action, string rol)
    {
        return null;
    }

    private int getAction(char player, float politica, String rol, float[,] Q, List<bool> estado)
    {
        return 0;
    }
}
