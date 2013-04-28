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
	// 16, Choose: target to yellow
	// 17 - 19, Find: box random edges
	// 20, Potatoe: doesn't change target or bg, just mouse over
	// 21 - 40, Wait: play volume at full, just win the level immidiately, set target to replay previous 20
	// 41 same as 21 - 40, but set target to black instead of replaying
	// 42, Shake: Show credits, set target to white.  OnWin return to level 1
	
	private const float RESTRICTED_WIN_AREA = 0.7f;
	private const int FIRST_LEVEL_GROUP = 5;
	private const float COLOR_DIFFERENCE_THRESHOLD = 0.6f;
	
	private static Color BURGANDY = new Color(1f, 43f / 255f, 60f / 255f, 1f);
	private static Color YELLOW = new Color(1f, 221f / 255f, 2f / 255f, 1f);
	
	private static Environment singleton;
	
	private int level = 1;
	private Color targetColor;
	private Vector3 oldCenter;
	private Color centerColor;
	private Color leftColor = Color.white;
	private Color rightColor = Color.black;
	private Color topColor = Color.white;
	private Color bottomColor = Color.black;
	
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
			case 20:
				return new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0f);
			}
			return Vector3.zero;
		}
	}
	
	public static int Level {
		get {
			return singleton.level;
		}
	}
	
	public static bool inSecondRandomPhase {
		get {
			return (Level >= 12 && Level <= 15) || (Level >= 17 && Level <= 19);
		}
	}
	
	public static void nextLevel() {
		++singleton.level;
		Debug.Log("Level up! Now: " + singleton.level);
		if (singleton.level <= FIRST_LEVEL_GROUP || inSecondRandomPhase) {
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
			float newTarget = Random.Range(0f, 1f - RESTRICTED_WIN_AREA);
			newTarget *= Screen.width;
			newTarget += Input.mousePosition.x + RESTRICTED_WIN_AREA * Screen.width / 2f;
			newTarget %= Screen.width;
			if (newTarget > Input.mousePosition.x) {
				singleton.targetColor = Color.Lerp(singleton.centerColor, singleton.rightColor, newTarget / (Screen.width - Input.mousePosition.x));
			}
			else {
				singleton.targetColor = Color.Lerp(singleton.leftColor, singleton.centerColor, newTarget / Input.mousePosition.x);
			}
			if (Level > 13) {
				do {
					singleton.topColor = randomColor();
					singleton.bottomColor = randomColor();
				} while (
					(singleton.topColor.ToVector() - singleton.bottomColor.ToVector()).sqrMagnitude < COLOR_DIFFERENCE_THRESHOLD ||
					(singleton.topColor.ToVector() - singleton.centerColor.ToVector()).sqrMagnitude < COLOR_DIFFERENCE_THRESHOLD ||
					(singleton.centerColor.ToVector() - singleton.bottomColor.ToVector()).sqrMagnitude < COLOR_DIFFERENCE_THRESHOLD
				);
			}
			if (inSecondRandomPhase) {
				if (Random.Range(0f, 1f) * Screen.height > Input.mousePosition.y) {
					singleton.targetColor = Color.Lerp(singleton.targetColor, singleton.bottomColor, Random.Range(0f, 1f));
				}
				else {
					singleton.targetColor = Color.Lerp(singleton.targetColor, singleton.topColor, Random.Range(0f, 1f));
				}
			}
			if (Level == 15) {
				singleton.targetColor = YELLOW;
				singleton.leftColor = YELLOW;
				singleton.rightColor = YELLOW;
			}
		}
		else if (Level == 6 || Level == 8 || Level == 10) {
			singleton.targetColor = BURGANDY;
		}
		else if (Level == 7 || Level == 9 || Level == 11 || Level == 20) {
			singleton.targetColor = randomColor();
			MouseFollower.FadeIn();
		}
		else if (Level == 16) {
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
		else if (singleton.level <= FIRST_LEVEL_GROUP || inSecondRandomPhase) {
			if (Input.mousePosition.x > singleton.oldCenter.x) {
				lastColor = Color.Lerp(singleton.centerColor, singleton.rightColor, (Input.mousePosition.x - singleton.oldCenter.x) / (Screen.width - singleton.oldCenter.x));
			}
			else {
				lastColor = Color.Lerp(singleton.leftColor, singleton.centerColor, Input.mousePosition.x / singleton.oldCenter.x);
			}
			if (inSecondRandomPhase) {
				if (Input.mousePosition.y > singleton.oldCenter.y) {
					lastColor = Color.Lerp(lastColor, singleton.bottomColor, (Input.mousePosition.y - singleton.oldCenter.y) / (Screen.height - singleton.oldCenter.y));
				}
				else {
					lastColor = Color.Lerp(singleton.topColor, lastColor, Input.mousePosition.y / singleton.oldCenter.y);
				}
			}
			WinningWatcher.setPct((lastColor.ToVector() - TargetColor.ToVector()).sqrMagnitude);
		}
		else if (singleton.level == 6 || singleton.level == 8 || singleton.level == 10) {
			lastColor = BURGANDY;
			WinningWatcher.setPct(MouseFollower.TimeHeld);
		}
		else if (singleton.level == 7 || singleton.level == 9 || singleton.level == 11 || Level == 20) {
			lastColor = TargetColor;
			WinningWatcher.setPct(Mathf.Pow((Input.mousePosition - hotSpot).sqrMagnitude / 10000f, 4f));
		}
		else if (Level == 16) {
			lastColor = TargetColor;
		}
		return lastColor;
	}
}

public static class ColorToVect {
	public static Vector3 ToVector(this Color x) {
		return new Vector3(x.r, x.g, x.b);
	}
}