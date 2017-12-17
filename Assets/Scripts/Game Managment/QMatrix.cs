using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using AssemblyCSharp;
using System.IO;
using System;
using System.Runtime.Serialization.Formatters.Binary;

public class QMatrix : MonoBehaviour
{
	public float[,] QMatrix_Begginer_A_Tank;
	public float[,] QMatrix_Begginer_A_Healer;
	public float[,] QMatrix_Begginer_A_Distance;
	public float[,] QMatrix_Begginer_A_Mele;
	public float[,] QMatrix_Begginer_B_Tank;
	public float[,] QMatrix_Begginer_B_Healer;
	public float[,] QMatrix_Begginer_B_Distance;
	public float[,] QMatrix_Begginer_B_Mele;
	public float[,] QMatrix_Intermediate_A_Tank;
	public float[,] QMatrix_Intermediate_A_Healer;
	public float[,] QMatrix_Intermediate_A_Distance;
	public float[,] QMatrix_Intermediate_A_Mele;
	public float[,] QMatrix_Intermediate_B_Tank;
	public float[,] QMatrix_Intermediate_B_Healer;
	public float[,] QMatrix_Intermediate_B_Distance;
	public float[,] QMatrix_Intermediate_B_Mele;
	public float[,] QMatrix_Difficult_A_Tank;
	public float[,] QMatrix_Difficult_A_Healer;
	public float[,] QMatrix_Difficult_A_Distance;
	public float[,] QMatrix_Difficult_A_Mele;
	public float[,] QMatrix_Difficult_B_Tank;
	public float[,] QMatrix_Difficult_B_Healer;
	public float[,] QMatrix_Difficult_B_Distance;
	public float[,] QMatrix_Difficult_B_Mele;

	public String Route_QMatrix_Begginer_A_Tank;
	public String Route_QMatrix_Begginer_A_Healer;
	public String Route_QMatrix_Begginer_A_Distance;
	public String Route_QMatrix_Begginer_A_Mele;
	public String Route_QMatrix_Begginer_B_Tank;
	public String Route_QMatrix_Begginer_B_Healer;
	public String Route_QMatrix_Begginer_B_Distance;
	public String Route_QMatrix_Begginer_B_Mele;
	public String Route_QMatrix_Intermediate_A_Tank;
	public String Route_QMatrix_Intermediate_A_Healer;
	public String Route_QMatrix_Intermediate_A_Distance;
	public String Route_QMatrix_Intermediate_A_Mele;
	public String Route_QMatrix_Intermediate_B_Tank;
	public String Route_QMatrix_Intermediate_B_Healer;
	public String Route_QMatrix_Intermediate_B_Distance;
	public String Route_QMatrix_Intermediate_B_Mele;
	public String Route_QMatrix_Difficult_A_Tank;
	public String Route_QMatrix_Difficult_A_Healer;
	public String Route_QMatrix_Difficult_A_Distance;
	public String Route_QMatrix_Difficult_A_Mele;
	public String Route_QMatrix_Difficult_B_Tank;
	public String Route_QMatrix_Difficult_B_Healer;
	public String Route_QMatrix_Difficult_B_Distance;
	public String Route_QMatrix_Difficult_B_Mele;

	void Start ()
	{
		Route_QMatrix_Begginer_A_Tank 			= Application.persistentDataPath + "/Begginer-A-Tank.text.bytes";
		Route_QMatrix_Begginer_A_Healer 		= Application.persistentDataPath + "/Begginer-A-Healer.text.bytes";
		Route_QMatrix_Begginer_A_Distance 		= Application.persistentDataPath + "/Begginer-A-Distance.text.bytes";
		Route_QMatrix_Begginer_A_Mele 			= Application.persistentDataPath + "/Begginer-A-Mele.text.bytes";
		Route_QMatrix_Begginer_B_Tank 			= Application.persistentDataPath + "/Begginer-B-Tank.text.bytes";
		Route_QMatrix_Begginer_B_Healer 		= Application.persistentDataPath + "/Begginer-B-Healer.text.bytes";
		Route_QMatrix_Begginer_B_Distance 		= Application.persistentDataPath + "/Begginer-B-Distance.text.bytes";
		Route_QMatrix_Begginer_B_Mele 			= Application.persistentDataPath + "/Begginer-B-Mele.text.bytes";
		Route_QMatrix_Intermediate_A_Tank 		= Application.persistentDataPath + "/Intermediate-A-Tank.text.bytes";
		Route_QMatrix_Intermediate_A_Healer 	= Application.persistentDataPath + "/Intermediate-A-Healer.text.bytes";
		Route_QMatrix_Intermediate_A_Distance 	= Application.persistentDataPath + "/Intermediate-A-Distance.text.bytes";
		Route_QMatrix_Intermediate_A_Mele 		= Application.persistentDataPath + "/Intermediate-A-Mele.text.bytes";
		Route_QMatrix_Intermediate_B_Tank 		= Application.persistentDataPath + "/Intermediate-B-Tank.text.bytes";
		Route_QMatrix_Intermediate_B_Healer 	= Application.persistentDataPath + "/Intermediate-B-Healer.text.bytes";
		Route_QMatrix_Intermediate_B_Distance 	= Application.persistentDataPath + "/Intermediate-B-Distance.text.bytes";
		Route_QMatrix_Intermediate_B_Mele 		= Application.persistentDataPath + "/Intermediate-B-Mele.text.bytes";
		Route_QMatrix_Difficult_A_Tank 			= Application.persistentDataPath + "/Difficult-A-Tank.text.bytes";
		Route_QMatrix_Difficult_A_Healer 		= Application.persistentDataPath + "/Difficult-A-Healer.text.bytes";
		Route_QMatrix_Difficult_A_Distance 		= Application.persistentDataPath + "/Difficult-A-Distance.text.bytes";
		Route_QMatrix_Difficult_A_Mele 			= Application.persistentDataPath + "/Difficult-A-Mele.text.bytes";
		Route_QMatrix_Difficult_B_Tank 			= Application.persistentDataPath + "/Difficult-B-Tank.text.bytes";
		Route_QMatrix_Difficult_B_Healer 		= Application.persistentDataPath + "/Difficult-B-Healer.text.bytes";
		Route_QMatrix_Difficult_B_Distance 		= Application.persistentDataPath + "/Difficult-B-Distance.text.bytes";
		Route_QMatrix_Difficult_B_Mele 			= Application.persistentDataPath + "/Difficult-B-Mele.text.bytes";
	}

	public void SaveQMatrix(float[,] Q, String route, int f, int c){
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Create (route);
		KeepMatrixData data = new KeepMatrixData (FromMatrix2Array (Q, f, c));
		bf.Serialize (file, data);
		file.Close();
	}

	public float[,] ChargeQMatrix(float[,] Q, String route, int f, int c){
		if (File.Exists (route)) {
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Open (route, FileMode.Open);
			KeepMatrixData data = (KeepMatrixData)bf.Deserialize (file);
			Q = FromArray2Matrix (data.Data, f, c);
			file.Close ();
		} else {
			Q = new float[f, c];
			for (int i = 0; i < f; i++) {
				for (int j = 0; j < c; j++) {
					Q [i, j] = 0f;
				}
			}
		}

		return Q;
	}
		
	public float[,] FromArray2Matrix(float[] array, int f, int c){
		float[,] matrix = new float[f, c];

		int cont = 0;
		for (int i = 0; i < f; i++) {
			for (int j = 0; j < c; j++) {
				matrix [i, j] = array [cont];
				cont++;
			}
		}
		return matrix;
	}

	public float[] FromMatrix2Array(float[,] matrix, int f, int c){
		float[] array = new float[f * c];

		int cont = 0;
		for (int i = 0; i < f; i++) {
			for (int j = 0; j < c; j++) {
				array [cont] = matrix [i, j];
				cont++;
			}
		}
		return array;
	}

}

[Serializable]
public class KeepMatrixData{
	
	float[] data;

	public KeepMatrixData(float[] data){
		this.data = data;
	}

	public float[] Data{
		set{ data = value;}
		get{ return data; }
	}
}
