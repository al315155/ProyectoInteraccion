using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour {

	public float maxHeight;
	public float minHeight;
	public float velocity;
	public int direction;

	void Start(){
		direction = -1;
	}

	void Update () {
		transform.position += Vector3.up * (velocity * Time.deltaTime * direction);

		if (transform.position.y >= maxHeight)
			direction = -1;
		if (transform.position.y <= minHeight)
			direction = 1;

	}

	private void OnMouseOver(){
		GetComponent<Collider> ().enabled = false;
	}

	private void OnMouseExit(){
		GetComponent<Collider> ().enabled = true;
	}
}
