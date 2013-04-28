using UnityEngine;
using System.Collections;

public class GoalText : MonoBehaviour {
	
	private readonly string[] INSTRUCTIONS = {
		"FIND",
		"FIND",
		"FIND",
		"FIND",
		"FIND",
		"FIND",
		"HOLD",
		"MAN",
		"HOLD",
		"DISK",
		"HOLD",
		"DOG",
		"FIND",
		"FIND",
		"FIND",
		"FIND",
		"SOLVE",
		"FIND",
		"FIND",
		"FIND",
		"POTATO",
		"WAIT",
		"WAIT",
		"WAIT",
		"WAIT",
		"WAIT",
		"WAIT",
		"WAIT",
		"WAIT",
		"WAIT",
		"WAIT",
		"WAIT",
		"WAIT",
		"WAIT",
		"WAIT",
		"WAIT",
		"WAIT",
		"WAIT",
		"WAIT",
		"WAIT",
		"WAIT",
		"WAIT",
		"SHAKE" 
	};
	
	private const float FONT_LERP_TIMER = 3f;
	
	private static GoalText singleton;
	
	private Color CurColor;
	private bool locked;
	
	void Start () {
		singleton = this;
	}
	
	public static void LerpTextColor() {
		if (!singleton.locked) {
			singleton.StartCoroutine(singleton.lerpTextcolor());
		}
	}
	
	private IEnumerator lerpTextcolor() {
		locked = true;
		Color startColor = CurColor;
		Color endColor = Environment.TargetColor;
		float lerpTime = FONT_LERP_TIMER;
		while (lerpTime > 0f) {
			yield return 0;
			lerpTime -= Time.deltaTime;
			CurColor = Color.Lerp(endColor, startColor, lerpTime / FONT_LERP_TIMER);
			if (endColor != Environment.TargetColor) {
				startColor = CurColor;
				endColor = Environment.TargetColor;
				lerpTime = FONT_LERP_TIMER;
			}
		}
		locked = false;
	}
	
	void OnGUI () {
		GUI.skin.label.fontSize = 40;
		GUI.skin.label.alignment = TextAnchor.MiddleCenter;
		if (!locked) {
			CurColor = Environment.TargetColor;
		}
		GUI.skin.label.normal.textColor = CurColor;
		GUILayout.BeginArea(new Rect(0f, 0f, Screen.width, 50f));
		GUILayout.Label(INSTRUCTIONS[Environment.Level]);
		GUILayout.EndArea();
	}
}
