using UnityEngine;
using System.Collections;

public class CrumbledBlock : MonoBehaviour {

	private float colorDivide = 2;

	int numberOfSteps = 2;
	float r, g, b;

	void Start () {
		Renderer renderer = GetComponent<Renderer> ();
		Material mat = renderer.material;
		Color bg = Camera.main.backgroundColor;
		r = mat.color.r;
		g = mat.color.g;
		b = mat.color.b;
		mat.color = new Color ((r + bg.r) / colorDivide, (g + bg.g) / colorDivide, (b + bg.b) / colorDivide, 1);
	}

	public void decreaseSteps () {
		GetComponent<MeshFilter> ().mesh = (Mesh)Resources.Load ("StandardBlock", typeof(Mesh));
		Renderer renderer = GetComponent<Renderer> ();
		Material mat = renderer.material;
		if (numberOfSteps > 1) {
			mat.color = new Color (r, g, b, 1);
		}
		numberOfSteps--;
	}

	public int getSteps () {
		return numberOfSteps;
	}
}
