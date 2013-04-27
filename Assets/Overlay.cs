using UnityEngine;
using System.Collections;

public class Overlay : MonoBehaviour {
	
	private Texture man;
	private Texture frisbee;
	private Texture dog;

	void Start () {
		man = Resources.Load("ManAlone") as Texture;
		frisbee = Resources.Load("Frisbee") as Texture;
		dog = Resources.Load("Dog") as Texture;
	}
	
	void Update () {
	}
	
	void OnGUI () {
		if (Environment.Level == 7) {
			GUI.DrawTexture(new Rect(0f, 0f, Screen.width, Screen.height), man);
		}
		else if (Environment.Level == 9) {
			GUI.DrawTexture(new Rect(0f, 0f, Screen.width, Screen.height), frisbee);
		}
		else if (Environment.Level == 11) {
			GUI.DrawTexture(new Rect(0f, 0f, Screen.width, Screen.height), dog);
		}
	}
}
