using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAvsIA : MonoBehaviour {

	// primero crear game y después states con los parámetros de game
	// (mapa, team1 y team2)
	private States states;
	private QLearningGame game;


	//---//---//---//---//

    float[,] QTanqueA;
    float[,] QTanqueB;
    float[,] QMeleA;
    float[,] QMeleB;
    float[,] QHealerA;
    float[,] QHealerB;
    float[,] QDistanceA;
    float[,] QDistanceB;


    public List<Unit> TeamA;
    public List<Unit> TeamB;

    public int nPartidas;
    public float learning_rate;
    public float discount_factor;
    public float politicaA;
    public float politicaB;

    public Functions funciones;

	QLearningGame qGame;


    void Start()
    {

        initializeQs();
        FillQ(QTanqueA);
        FillQ(QTanqueB);
        FillQ(QHealerA);
        FillQ(QHealerB);
        FillQ(QMeleA);
        FillQ(QMeleB);
        FillQ(QDistanceA);
        FillQ(QDistanceB);

		qGame = gameObject.AddComponent<QLearningGame>();
//		qGame.StartGame();

		TeamA = qGame.GetTeam_A();
		TeamB = qGame.GetTeam_B();

		states = new States(qGame.GetMap(), TeamA, TeamB);

		funciones = new Functions(qGame, states);


        for (int i = 0; i < nPartidas; i++)
        {
			funciones.entrenamiento(QTanqueA, QTanqueB, QHealerA, QHealerB, QMeleA, QMeleB, QDistanceA, QDistanceB, learning_rate, discount_factor, politicaA, politicaB, TeamA, TeamB);
        }


    }

    private void Update()
    {

       


        //bug.Log(QTanqueA[0,0]);
      

    }

    public void FillQ(float[,] Q)
    {
        

        for (int i = 0; i < 18; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                Q[i, j] = 0f;
            }
        }
    }

    void initializeQs()
    {
        QTanqueA = new float[18, 4];
        QTanqueB = new float[18, 4];
        QMeleA = new float[18, 4];
        QMeleB = new float[18, 4];
        QHealerA = new float[18, 4];
        QHealerB = new float[18, 4];
        QDistanceA= new float[18, 4];
        QDistanceB = new float[18, 4];
    }


}
