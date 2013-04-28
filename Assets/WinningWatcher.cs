using UnityEngine;
using System.Collections;

public class WinningWatcher : MonoBehaviour {
	
	private const float MAX_PCT_DIFFERENCE_SQUARED = 0.0002f;
	private const float LEVEL_TRANSITION_TIME = 0.8f;
	
	private static WinningWatcher singleton;
	
	private float targetWidth;
	private float targetPct = 0.5f;
	private bool locked;
	
	void Start () {
		singleton = this;
	}
	
	public static bool Locked {
		get {
			return singleton.locked;
		}
	}
	
	public static void setPct (float colorDiff) {
		if (colorDiff < MAX_PCT_DIFFERENCE_SQUARED) {
			singleton.targetWidth = MouseFollower.cameraWidth * 2f;
		}
		else {
			singleton.targetWidth = Mathf.Lerp(MouseFollower.cameraWidth * 2f, 10f, colorDiff / (MAX_PCT_DIFFERENCE_SQUARED * 60f));
		}
	}
	
	void Update () {
		Vector3 scale = transform.localScale;
		if (scale.x < targetWidth) {
			scale.x += 6;
			if (Environment.squareTime()) {
				scale.y += 6;
			}
			if (scale.x > MouseFollower.cameraWidth && !locked) {
				StartCoroutine(Winning());
			}
		}
		else if (scale.x > targetWidth) {
			scale.x --;
			if (Environment.squareTime()) {
				scale.y --;
			}
		}
		transform.localScale = scale;
		SoundManager.setVolume((scale.x - 10f) / MouseFollower.cameraWidth * 2f);
	}
	
	private IEnumerator Winning() {
		float secsLeft = LEVEL_TRANSITION_TIME;
		locked = true;
		Color beginningBackground = Camera.main.backgroundColor;
		Color nextColor = Color.Lerp(Environment.TargetColor, Color.red, 0.5f);
		while (secsLeft > 0f) {
			yield return 0;
			secsLeft -= Time.deltaTime;
			Camera.main.backgroundColor = Color.Lerp(beginningBackground, Environment.TargetColor, (LEVEL_TRANSITION_TIME - secsLeft) / LEVEL_TRANSITION_TIME);
			SoundManager.setVolume(secsLeft / LEVEL_TRANSITION_TIME);
		}
		locked = false;
		Environment.nextLevel();
		transform.localScale = new Vector3(10f, Environment.squareTime() ? 10f : 200f, 1f);
	}
}