using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAvsIA : MonoBehaviour {

	public int movementNum;

	// primero crear game y después states con los parámetros de game
	// (mapa, team1 y team2)
	private States states;
	private QLearningGame game;
    public IAActions IAactions;

    public GameObject iaActionsObj;
    QMatrix qmatrix;


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
    public int politicaA;
    public int politicaB;

    public Functions funciones;

	QLearningGame qGame;
    int numeroPartidas = 0;


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
        IAactions = iaActionsObj.GetComponent<IAActions>();

		TeamA = qGame.GetTeam_A();
		TeamB = qGame.GetTeam_B();

		states = new States(qGame.GetMap(), TeamA, TeamB);
		qmatrix = gameObject.AddComponent<QMatrix> ();
		funciones = new Functions(qGame, states, IAactions, movementNum, qmatrix);

		//cargo matrices
        // cambiar;
		funciones.QTA = qmatrix.ChargeQMatrix(funciones.QTA, qmatrix.Route_QMatrix_Begginer_A_Tank, 18, 5);
		funciones.QTB = qmatrix.ChargeQMatrix (funciones.QTB, qmatrix.Route_QMatrix_Begginer_B_Tank, 18, 5);
		funciones.QHA = qmatrix.ChargeQMatrix (funciones.QHA, qmatrix.Route_QMatrix_Begginer_A_Healer, 18, 5);
		funciones.QHB = qmatrix.ChargeQMatrix (funciones.QHB, qmatrix.Route_QMatrix_Begginer_B_Healer, 18, 5);
		funciones.QMA = qmatrix.ChargeQMatrix (funciones.QMA, qmatrix.Route_QMatrix_Begginer_A_Mele, 18, 5);
		funciones.QMB = qmatrix.ChargeQMatrix (funciones.QMB, qmatrix.Route_QMatrix_Begginer_B_Mele, 18, 5);
		funciones.QDA = qmatrix.ChargeQMatrix (funciones.QDA, qmatrix.Route_QMatrix_Begginer_A_Distance, 18, 5);
		funciones.QDB = qmatrix.ChargeQMatrix (funciones.QDB, qmatrix.Route_QMatrix_Begginer_B_Distance, 18, 5);


        for (int i = 0; i < nPartidas; i++)
         {
			funciones.entrenamiento(QTanqueA, QTanqueB, QHealerA, QHealerB, QMeleA, QMeleB, QDistanceA, QDistanceB, learning_rate, discount_factor, politicaA, politicaB, TeamA, TeamB);
            numeroPartidas++;
            Debug.Log("Partidas" + numeroPartidas);
        }

        qmatrix.SaveQMatrix(funciones.QDA, qmatrix.Route_QMatrix_Begginer_A_Distance, 18, 5);
        qmatrix.SaveQMatrix(funciones.QDB, qmatrix.Route_QMatrix_Begginer_B_Distance, 18, 5);
        qmatrix.SaveQMatrix(funciones.QHA, qmatrix.Route_QMatrix_Begginer_A_Healer, 18, 5);
        qmatrix.SaveQMatrix(funciones.QHB, qmatrix.Route_QMatrix_Begginer_B_Healer, 18, 5);
        qmatrix.SaveQMatrix(funciones.QMA, qmatrix.Route_QMatrix_Begginer_A_Mele, 18, 5);
        qmatrix.SaveQMatrix(funciones.QMB, qmatrix.Route_QMatrix_Begginer_B_Mele, 18, 5);
        qmatrix.SaveQMatrix(funciones.QTA, qmatrix.Route_QMatrix_Begginer_A_Tank, 18, 5);
        qmatrix.SaveQMatrix(funciones.QTB, qmatrix.Route_QMatrix_Begginer_B_Tank, 18, 5);


    }



    private void Update()
    {

       


        //bug.Log(QTanqueA[0,0]);
      

    }

    public void FillQ(float[,] Q)
    {
        

        for (int i = 0; i < 18; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                Q[i, j] = 0f;
            }
        }
    }

    void initializeQs()
    {
        QTanqueA = new float[18, 5];
        QTanqueB = new float[18, 5];
        QMeleA = new float[18, 5];
        QMeleB = new float[18, 5];
        QHealerA = new float[18, 5];
        QHealerB = new float[18, 5];
        QDistanceA= new float[18, 5];
        QDistanceB = new float[18, 5];
    }


}
