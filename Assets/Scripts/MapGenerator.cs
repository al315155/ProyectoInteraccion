using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour {

	public GameObject cube;

//	private Tile[,] tileMap = new Tile[,]{
//		new Tile(1, 1, true), new Tile(

	// Use this for initialization
	// Update is called once per frame

	public Tile[] tileMap = new Tile[9];

	public int[] map;


	void Update () {
		
	}

	void Start(){

		map = new int[]{
			1, 1, 1,	1, 1, 1,	1, 1, 1,	
			1, 1, 1, 	1, 1, 1, 	1, 1, 1, 
			1, 1, 1, 	1, 1, 1, 	1, 1, 1
		};

		for (int i = 0, j = 0; j < tileMap.Length; i+=3, j++) {
			Debug.Log (map [i+1]);
			tileMap[j] = new Tile (map [i], map [i+1], map [i+2]);
		}

		int cont = 0;
		for (int i = 0; i < 3; i++) {
			for (int j = 0; j < 3; j++) {
				GameObject newCube = Instantiate (cube);
				newCube.transform.localScale = new Vector3 (
					tileMap [cont].GetSize () * newCube.transform.localScale.x,
					tileMap [cont].GetHeight () * newCube.transform.localScale.y,
					tileMap [cont].GetSize () * newCube.transform.localScale.z);

				newCube.transform.position = new Vector3 (i * 1, 0 + tileMap[cont].GetHeight()/2, j * 1);
				cont++;
			
			}
		}
	}



}
