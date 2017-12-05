using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using AssemblyCSharp;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.EventSystems;

public class Drawer : MonoBehaviour
{
	public Material ground;
	public GameObject currentOver;

	public Material original;
	public Material selected;

	void Start(){
		currentOver = null;
	}

	void Update(){

		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hitInfo;

		if (Physics.Raycast (ray, out hitInfo)){
			if (!hitInfo.collider.tag.Equals ("Target")) {
				if (currentOver != hitInfo.collider.gameObject) {
					if (currentOver == null) {
						original = hitInfo.collider.gameObject.GetComponent<Renderer> ().material;
					} else {
						currentOver.GetComponent<Renderer> ().material = original;
						original = hitInfo.collider.gameObject.GetComponent<Renderer> ().material;
					}

					currentOver = hitInfo.collider.gameObject;
					currentOver.GetComponent<Renderer> ().material = selected;
				}
			}
		}
	}

	public void DrawArea(GameObject[,] map, Vector2[] area){
		for (int i = (int) area [0].x; i < (int)area [1].x; i++) {
			for (int j = (int) area [0].y; j < (int) area [1].y; j++) {
				map [i, j].GetComponent<Renderer>().material.color = Color.cyan;
			}
		}
	}

	public void DrawBoxes(GameObject[,] map, List<Vector2> positions, Color color){
		foreach (Vector2 position in positions) {
			map[(int) position.x, (int) position.y].GetComponent<Renderer> ().material.color = color;
		}
	}

	public void UnDrawBoxes(GameObject[,] map, List<Vector2> positions){
		foreach (Vector2 position in positions) {
			map [(int)position.x, (int)position.y].GetComponent<Renderer> ().material.color = ground.color;
		}
	}
}

