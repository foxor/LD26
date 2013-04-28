using UnityEngine;
using System.Collections;

public class MouseFollower : MonoBehaviour {
	
	private const float FADE_TIME = 1.5f;
	
	private static MouseFollower singleton;
	
	private const float HOLD_TIME_MODIFIER = 0.3f;
	
	public float holdTime;
	
	public static float screenPct {
		get {
			return Input.mousePosition.x / ((float)Screen.width);
		}
	}
	
	public static float cameraHeight {
		get {
			return Camera.main.orthographicSize * 2;
		}
	}
	
	public static float cameraWidth {
		get {
			return cameraHeight * Camera.main.aspect;
		}
	}
	
	public static float TimeHeld {
		get {
			return 0.5f - (singleton.holdTime / HOLD_TIME_MODIFIER);
		}
	}
	
	public static void FadeIn() {
		singleton.StartCoroutine(singleton.fadeIn());
	}
	
	private IEnumerator fadeIn() {
		Color startColor = new Color(Environment.TargetColor.r, Environment.TargetColor.g, Environment.TargetColor.b, 0f);
		renderer.material.color = startColor;
		float timeLeft = FADE_TIME;
		while (timeLeft > 0f) {
			yield return 0;
			timeLeft -= Time.deltaTime;
			renderer.material.color = Color.Lerp(Environment.TargetColor, startColor, timeLeft / FADE_TIME);
		}
	}
	
	void Start () {
		singleton = this;
	}
	
	void Update () {
		if (Input.GetMouseButton(0)) {
			holdTime += Time.deltaTime;
		}
		else {
			holdTime = 0f;
		}
		if (!WinningWatcher.Locked) {
			renderer.material.color = Environment.getColor();
		}
		Vector3 pos = transform.position;
		pos.x = Mathf.Lerp(cameraWidth / -2f, cameraWidth / 2f, screenPct);
		if (Environment.squareTime()) {
			pos.y = Mathf.Lerp(cameraHeight / -2f, cameraHeight / 2f, Input.mousePosition.y / ((float)Screen.height));
		}
		else {
			pos.y = 0f;
		}
		transform.position = pos;
	}
}
