using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Intro : MonoBehaviour {

	private float timerLimit = 2;

	float timer = 1f;
	Color32 nextColor = MenuColors.menuColor;
	GameObject text;

	void Start () {
		text = GameObject.Find("Title Text");
	}

	void Update () {
		timer += Time.deltaTime;
		Camera.main.backgroundColor = Color32.Lerp(Camera.main.backgroundColor, nextColor, Time.deltaTime);	
		if (timer > timerLimit) {
			timer = 0;
			nextColor = new Color(Random.Range(0.5f, 1.0f), Random.Range(0.5f, 1.0f), Random.Range(0.5f, 1.0f), 1);
		}
		text.transform.localPosition = new Vector3(
			text.transform.localPosition.x, 
			text.transform.localPosition.y + pingPong(Time.time * 1, -1.25f, 1.25f), 
			text.transform.localPosition.z
		);
		text.transform.rotation = Quaternion.Euler(
			0, 
			0, 
			text.transform.rotation.eulerAngles.z + pingPong(Time.time * 1.5f, -0.9f, 0.9f)
		);
	}

	float pingPong(float aValue, float aMin, float aMax) {
		return Mathf.PingPong(aValue, aMax-aMin) + aMin;
	}
}
