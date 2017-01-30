using UnityEngine;
using System.Collections;

public class CrumbledBlock : MonoBehaviour {

	int numberOfSteps = 2;
	float r, g, b;

	void Start () {
		Renderer renderer = GetComponent<Renderer> ();
		Material mat = renderer.material;
		Color bg = Camera.main.backgroundColor;
		r = mat.color.r;
		g = mat.color.g;
		b = mat.color.b;
		mat.color = new Color ((r + bg.r)/2, (g + bg.g)/2, (b + bg.b)/2, 1);
	}

	public void decreaseSteps () {
		GetComponent<MeshFilter>().mesh = (Mesh)Resources.Load("StandardBlock", typeof(Mesh));
		Renderer renderer = GetComponent<Renderer> ();
		Material mat = renderer.material;
		if (numberOfSteps > 1) {
			mat.color = new Color (r, g, b, 1);
		}
		numberOfSteps--;
	}

	public int getSteps() {
		return numberOfSteps;
	}
}
