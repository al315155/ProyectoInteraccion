using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IaVsIa  {

	float [,] QTanqueA;
	float [,] QTanqueB;
	float [,] QMeleA;
	float [,] QMeleB;
	float[,] QHealerA;
	float [,]QHealerB;
	float[,] QDistanceA;
	float[,] QDistanceB;

	void Start() {

		InitializeQ (QTanqueA);
	}

	public void InitializeQ(float[,] Q)
	{
		Q = new float[18, 4];
	}


}
