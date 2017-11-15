using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeOverManager : MonoBehaviour {

    MeshRenderer m_Render;
    Material original;
    public Material selected;

    private void Start()
    {
        m_Render = GetComponent<MeshRenderer>();
        original = gameObject.GetComponent<Renderer>().material;
    } 
    // Use this for initialization
    private void OnMouseOver()
    {
        Debug.Log("Hola");
        gameObject.GetComponent<Renderer>().material = selected;
    }
    private void OnMouseExit()
    {

        m_Render.material = original;
    }

}
