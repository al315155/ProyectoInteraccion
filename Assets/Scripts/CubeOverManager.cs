using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeOverManager : MonoBehaviour {

	Camera camera;

    MeshRenderer m_Render;
    Material original;
    public Material selected;

	public GameObject Canvas;

    private void Start()
    {
		camera = Camera.main;

        m_Render = GetComponent<MeshRenderer>();
		original = gameObject.GetComponent<Renderer>().material;
    } 
    // Use this for initialization
    private void OnMouseOver()
    {
        //Debug.Log("Hola");
        gameObject.GetComponent<Renderer>().material = selected;


    }
    private void OnMouseExit()
    {
		Debug.Log ("entro");
        m_Render.material = original;
    }

	private void OnMouseDown()
	{
		if (gameObject.tag.Equals("Player") || gameObject.tag.Equals("Enemy")){
			Canvas.SetActive(true);

		}
	}

}
