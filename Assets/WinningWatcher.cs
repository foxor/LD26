using UnityEngine;
using System.Collections;

public class WinningWatcher : MonoBehaviour {
	
	private const float MAX_PCT_DIFFERENCE_SQUARED = 0.0002f;
	private const float LEVEL_TRANSITION_TIME = 0.3f;
	private const float EXPAND_ACCELERATION = 0.02f;
	
	private static WinningWatcher singleton;
	
	private float targetWidth;
	private float targetPct = 0.5f;
	private bool locked;
	private float expandSpeed;
	
	void Start () {
		singleton = this;
	}
	
	public static bool Locked {
		get {
			return singleton.locked;
		}
	}
	
	public static void setPct (float colorDiff) {
		if (singleton.locked) {
			return;
		}
		if (colorDiff < MAX_PCT_DIFFERENCE_SQUARED) {
			singleton.targetWidth = MouseFollower.cameraWidth * 3f;
		}
		else {
			singleton.targetWidth = Mathf.Lerp(MouseFollower.cameraWidth * 3f, 10f, colorDiff / (MAX_PCT_DIFFERENCE_SQUARED * 60f));
		}
	}
	
	void Update () {
		Vector3 scale = transform.localScale;
		if (scale.x < targetWidth) {
			expandSpeed += EXPAND_ACCELERATION;
			scale.x = Mathf.Min(targetWidth, scale.x + expandSpeed);
			if (Environment.squareTime()) {
				scale.y = Mathf.Min(targetWidth, scale.y + expandSpeed);
			}
			else {
				scale.y = 200f;
			}
			if (scale.x > ((Input.mousePosition.x / Screen.width) * MouseFollower.cameraWidth) &&
				scale.x > (((Screen.width - Input.mousePosition.x) / Screen.width) * MouseFollower.cameraWidth) &&
				!locked
			) {
				StartCoroutine(Winning());
			}
		}
		else if (scale.x > targetWidth) {
			expandSpeed = 0f;
			scale.x = Mathf.Max(targetWidth, scale.x - 4);
			if (Environment.squareTime()) {
				scale.y = Mathf.Max(targetWidth, scale.y - 4);
			}
		}
		transform.localScale = scale;
		SoundManager.setVolume((scale.x - 10f) / MouseFollower.cameraWidth * 2f);
	}
	
	private IEnumerator Winning() {
		locked = true;
		if (Environment.Level <= 20 || Environment.Level >= 41) {
			expandSpeed = 0f;
		}
		float secsLeft = LEVEL_TRANSITION_TIME;
		Vector3 startScale = transform.localScale;
		Vector3 endScale = new Vector3(MouseFollower.cameraWidth * 2f, 400f, 1f); 
		Color beginningColor = renderer.material.color;
		while (secsLeft > 0f) {
			yield return 0;
			secsLeft -= Time.deltaTime;
			renderer.material.color = Color.Lerp(Environment.TargetColor, beginningColor, secsLeft / LEVEL_TRANSITION_TIME);
			SoundManager.setVolume(secsLeft / LEVEL_TRANSITION_TIME);
			transform.localScale = Vector3.Lerp(endScale, startScale, secsLeft / LEVEL_TRANSITION_TIME);
		}
		Camera.main.backgroundColor = Environment.TargetColor;
		Environment.nextLevel();
		transform.localScale = new Vector3(10f, Environment.squareTime() ? 10f : 200f, 1f);
		locked = false;
	}
}