using UnityEngine;
using System.Collections;

public class Puzzle : MonoBehaviour {
	
	private const float SQUARED_THRESHOLD_DISTANCE = 2000f;
	
	private static Vector3 One = new Vector3(0.28f, 0.5f, 0f);
	private static Vector3 Two = new Vector3(0.5f, 0.5f, 0f);
	private static Vector3 Three = new Vector3(0.64f, 0.5f, 0f);
	
	private readonly int[] sequence = {
		1,
		3,
		2
	};
	
	private readonly float[] pcts = {
		0.02f,
		0.011f,
		0.01f,
		0f
	};
	
	private int progress;
	
	private static Vector3 mouseMapped(Vector3 x) {
		return new Vector3(x.x * Screen.width, x.y * Screen.height, 0f);
	}
	
	private static bool checkHit(Vector3 test) {
		return (Input.mousePosition - mouseMapped(test)).sqrMagnitude < SQUARED_THRESHOLD_DISTANCE;
	}
	
	void Update () {
		if (Environment.Level == 16) {
			int startProgress = progress;
			if (checkHit(One)) {
				progress = (progress == 0 || progress == 1) ? 1 : 0;
			}
			if (checkHit(Two)) {
				progress = (progress == 2 || progress == 3) ? 3 : 0;
			}
			if (checkHit(Three)) {
				progress = (progress == 1 || progress == 2) ? 2 : 0;
			}
			if (startProgress != progress) {
				Debug.Log("Puzzle on stage " + progress);
			}
			WinningWatcher.setPct(pcts[progress]);
		}
	}
}