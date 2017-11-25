using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : MonoBehaviour {

    public Canvas canvas;
    MeshRenderer m_mesh;

    private void Start()
    {
        m_mesh = GetComponent<MeshRenderer>();
    }

    private void OnMouseDown()
    {
        canvas.enabled = true;    
    }
    private void OnMouseOver()
    {
        m_mesh.material.color = Color.red;
        
    }
    private void OnMouseExit()
    {
        m_mesh.material.color = Color.white;
    }
}
