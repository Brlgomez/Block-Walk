using UnityEngine;
using System.Collections;

public class FallingBlock : MonoBehaviour {
	
	float alpha = 1;
	float r, g, b;

	void Start () {
		GetComponent<Collider> ().enabled = false;
		r = GetComponent<Renderer> ().material.color.r;
		g = GetComponent<Renderer> ().material.color.g;
		b = GetComponent<Renderer> ().material.color.b;
		gameObject.GetComponent<BoxCollider> ().enabled = false;
	}

	void Update () {
		alpha -= Time.deltaTime * 2;
		GetComponent<Renderer> ().material.color = new Color (r, g, b, alpha);
		if (alpha < 0) {
			Destroy (gameObject);
		}
	}
}
