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
		Camera.main.GetComponent<BlockManagement> ().removeBlock (gameObject);
		gameObject.GetComponent<BoxCollider> ().enabled = false;
	}

	void Update () {
		alpha -= Time.deltaTime * 2;
		GetComponent<Renderer> ().material.color = new Color (r, g, b, alpha);
		if (alpha < 0) {
			Camera.main.GetComponent<CharacterMovement> ().deductNumberOfBlocks ();
			Destroy (gameObject);
		}
	}
}
