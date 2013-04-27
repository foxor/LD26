using UnityEngine;
using System.Collections;

public class Environment : MonoBehaviour {
	
	//Level sequence:
	// 1, Find: tutorial, white to black
	// 2 - 3, Find: column random sides
	// 4 - 5, Find: box random sides
	// 6, Click: box to burgandy
	// 7, Throw: random color box, mouse over man to expand, on level end, fade man out and burgandy box in
	// 8, Click: box to burgandy
	// 9, Fly: rando color box, mouse over frisbee to expand
	// 10, Click: box to burgandy
	// 11, Catch: rando color box, mouse over dog to expand
	// 12 - 13, Find: box random sides, white top black bottom
	// 14 - 15, Find: box random edges
	
	private const float RESTRICTED_WIN_AREA = 0.7f;
	private const int FIRST_LEVEL_GROUP = 5;
	private const float COLOR_DIFFERENCE_THRESHOLD = 0.6f;
	
	private static Color BURGANDY = new Color(1f, 43f / 255f, 60f / 255f, 1f);
	
	private static Environment singleton;
	
	private int level = 1;
	private Color targetColor;
	private Vector3 oldCenter;
	private Color centerColor;
	private Color leftColor = Color.white;
	private Color rightColor = Color.black;
	
	public static Color TargetColor {
		get {
			return singleton.targetColor;
		}
	}
	
	void Start () {
		singleton = this;
		targetColor = Color.Lerp(Color.white, Color.black, Random.Range(0.3f, 0.7f));
	}
	
	public static Color randomColor() {
		return new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1f);
	}
	
	public static bool squareTime() {
		return singleton.level > 3;
	}
	
	private static Vector3 hotSpot {
		get {
			switch (singleton.level) {
			case 7:
				return new Vector3(Screen.width * 0.8f, Screen.height * 0.3f, 0f);
			case 9:
				return new Vector3(Screen.width * 0.43f, Screen.height * 0.3f, 0f);
			case 11:
				return new Vector3(Screen.width * 0.16f, Screen.height * 0.3f, 0f);
			}
			return Vector3.zero;
		}
	}
	
	public static int Level {
		get {
			return singleton.level;
		}
	}
	
	public static void nextLevel() {
		++singleton.level;
		Debug.Log("Level up! Now: " + singleton.level);
		if (singleton.level <= FIRST_LEVEL_GROUP) {
			singleton.oldCenter = Input.mousePosition;
			singleton.centerColor = singleton.targetColor;
			do {
				singleton.rightColor = randomColor();
				singleton.leftColor = randomColor();
			} while (
				(singleton.leftColor.ToVector() - singleton.rightColor.ToVector()).sqrMagnitude < COLOR_DIFFERENCE_THRESHOLD ||
				(singleton.leftColor.ToVector() - singleton.centerColor.ToVector()).sqrMagnitude < COLOR_DIFFERENCE_THRESHOLD ||
				(singleton.centerColor.ToVector() - singleton.rightColor.ToVector()).sqrMagnitude < COLOR_DIFFERENCE_THRESHOLD
			);
			float newTarget = Random.Range(0f, Screen.width * (1f - RESTRICTED_WIN_AREA));
			if (newTarget > Input.mousePosition.x - Screen.width * RESTRICTED_WIN_AREA / 2f) {
				newTarget -= Input.mousePosition.x;
				newTarget += Screen.width * (RESTRICTED_WIN_AREA / 2f);
				singleton.targetColor = Color.Lerp(singleton.centerColor, singleton.rightColor, newTarget / (Screen.width - Input.mousePosition.x));
			}
			else {
				singleton.targetColor = Color.Lerp(singleton.leftColor, singleton.centerColor, newTarget / Input.mousePosition.x);
			}
		}
		else if (Level == 6 || Level == 8 || Level == 10) {
			singleton.targetColor = BURGANDY;
		}
		else if (Level == 7 || Level == 9 || Level == 11) {
			singleton.targetColor = randomColor();
		}
		WinningWatcher.setPct(1000f);
		GoalText.LerpTextColor();
	}
	
	public static Color getColor() {
		Color lastColor = Color.white;
		if (singleton.level == 1) {
			lastColor = Color.Lerp(singleton.leftColor, singleton.rightColor, Input.mousePosition.x / Screen.width);
			WinningWatcher.setPct((lastColor.ToVector() - TargetColor.ToVector()).sqrMagnitude);
		}
		else if (singleton.level <= FIRST_LEVEL_GROUP) {
			if (Input.mousePosition.x > singleton.oldCenter.x) {
				lastColor = Color.Lerp(singleton.centerColor, singleton.rightColor, (Input.mousePosition.x - singleton.oldCenter.x) / (Screen.width - singleton.oldCenter.x));
			}
			else {
				lastColor = Color.Lerp(singleton.leftColor, singleton.centerColor, Input.mousePosition.x / singleton.oldCenter.x);
			}
			WinningWatcher.setPct((lastColor.ToVector() - TargetColor.ToVector()).sqrMagnitude);
		}
		else if (singleton.level == 6 || singleton.level == 8 || singleton.level == 10) {
			lastColor = BURGANDY;
			WinningWatcher.setPct(MouseFollower.TimeHeld);
		}
		else if (singleton.level == 7 || singleton.level == 9 || singleton.level == 11) {
			lastColor = TargetColor;
			WinningWatcher.setPct(Mathf.Pow((Input.mousePosition - hotSpot).sqrMagnitude / 10000f, 4f));
		}
		return lastColor;
	}
}

public static class ColorToVect {
	public static Vector3 ToVector(this Color x) {
		return new Vector3(x.r, x.g, x.b);
	}
}