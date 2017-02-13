using UnityEngine;
using System.Collections;

public class Intro : MonoBehaviour {

	private float timerLimit = 2;

	float timer = 1f;
	Color32 nextColor = MenuColors.menuColor;

	void Update () {
		timer += Time.deltaTime;
		if (timer > timerLimit) {
			timer = 0;
			nextColor = new Color(Random.Range (0.5f, 1.0f), Random.Range (0.5f, 1.0f), Random.Range (0.5f, 1.0f), 1);
		}
		Camera.main.backgroundColor = Color32.Lerp(Camera.main.backgroundColor, nextColor, Time.deltaTime);	
	}
}
