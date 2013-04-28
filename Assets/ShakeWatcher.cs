using UnityEngine;
using System.Collections;

public class ShakeWatcher : MonoBehaviour {
	
	private float[] history = new float[5];
	private int histPtr;
	private Vector3 lastPos;

	void Update () {
		if (Environment.Level == 42) {
			history[histPtr] = (Input.mousePosition - lastPos).sqrMagnitude;
			lastPos = Input.mousePosition;
			histPtr++;
			histPtr %= 5;
			float total = history[0] + history[1] + history[2] + history[3] + history[4];
			WinningWatcher.setPct(Mathf.Max(0f, 0.02f - total / 2000f));
		}
	}
}