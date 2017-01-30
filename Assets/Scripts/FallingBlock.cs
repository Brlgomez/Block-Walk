using UnityEngine;
using System.Collections;

public class FallingBlock : MonoBehaviour {
	
	//float alpha = 0;
	//float r, g, b;

	void Start () {
		//r = GetComponent<Renderer> ().material.color.r;
		//g = GetComponent<Renderer> ().material.color.g;
		//b = GetComponent<Renderer> ().material.color.b;
		gameObject.GetComponent<BoxCollider> ().enabled = false;
		Camera.main.GetComponent<BlockManagement> ().removeBlock (gameObject);
	}

	void Update () {
		//alpha += Time.deltaTime;
		//GetComponent<Renderer> ().material.color = Color.Lerp (GetComponent<Renderer> ().material.color, Camera.main.backgroundColor, Time.deltaTime * 6);
		//GetComponent<Renderer> ().material.color = new Color (r, g, b, alpha);
		transform.localScale = Vector3.Lerp (transform.localScale, Vector3.zero, Time.deltaTime * 4);
		if (transform.localScale.x < 0.05f) {
			Destroy (gameObject);
		}
	}
}
