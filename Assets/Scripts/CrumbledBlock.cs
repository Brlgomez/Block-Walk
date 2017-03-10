using UnityEngine;
using System.Collections;

public class CrumbledBlock : MonoBehaviour {

	static float colorDivide = 2;
	static float alpha = 0.75f;

	int numberOfSteps = 2;
	float r, g, b;

	void Start () {
		Renderer renderer = GetComponent<Renderer> ();
		Material mat = renderer.material;
		Color bg = Camera.main.backgroundColor;
		r = mat.color.r;
		g = mat.color.g;
		b = mat.color.b;
		mat.color = new Color ((r + bg.r) / colorDivide, (g + bg.g) / colorDivide, (b + bg.b) / colorDivide, alpha);
	}

	public void decreaseSteps () {
		Renderer renderer = GetComponent<Renderer> ();
		if (numberOfSteps > 1) {
			GetComponent<Renderer>().material = Camera.main.GetComponent<DeleteCubes>().getStandard();
			Material mat = renderer.material;
			mat.color = new Color (r, g, b, 1);
		}
		numberOfSteps--;
	}

	public int getSteps () {
		return numberOfSteps;
	}
}
