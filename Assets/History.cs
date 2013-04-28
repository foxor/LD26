using UnityEngine;
using System.Collections.Generic;

public class History : MonoBehaviour {
	
	private static History singleton;
	
	private List<Color> sequence;
	
	void Start () {
		singleton = this;
		sequence = new List<Color>();
	}
	
	public static void SubmitNewLevelColor(Color x) {
		singleton.sequence.Add(x);
	}
	
	public static Color getLevelColor(int level) {
		return singleton.sequence[level];
	}
}