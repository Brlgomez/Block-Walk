using UnityEngine;
using System.Collections;

public class CrumbledBlock : MonoBehaviour {

	int numberOfSteps = 2;

	void Start () {
		Renderer renderer = GetComponent<Renderer> ();
		Material mat = renderer.material;
		mat.color = new Color (mat.color.r, mat.color.g, mat.color.b, 0.5f);
	}

	public void decreaseSteps () {
		GetComponent<MeshFilter>().mesh = (Mesh)Resources.Load("Cube", typeof(Mesh));
		Renderer renderer = GetComponent<Renderer> ();
		Material mat = renderer.material;
		if (numberOfSteps > 1) {
			mat.color = new Color (mat.color.r, mat.color.g, mat.color.b, 1);
		}
		numberOfSteps--;
	}

	public int getSteps() {
		return numberOfSteps;
	}
}
