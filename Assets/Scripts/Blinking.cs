using UnityEngine;
using System.Collections;

public class Blinking : MonoBehaviour {

	void Start () {
		InvokeRepeating("Blink", 0.0f, 0.4f);
	}

	IEnumerator Blink() {
		gameObject.GetComponent<Renderer>().enabled = false;
		yield return new WaitForSeconds(1.0f);
		gameObject.GetComponent<Renderer>().enabled = true;
	}
}
