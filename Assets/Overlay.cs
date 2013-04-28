using UnityEngine;
using System.Collections;

public class Overlay : MonoBehaviour {
	
	private Texture man;
	private Texture frisbee;
	private Texture dog;
	private Texture puzzle;
	private Texture potato;

	void Start () {
		man = Resources.Load("ManAlone") as Texture;
		frisbee = Resources.Load("Frisbee") as Texture;
		dog = Resources.Load("Dog") as Texture;
		puzzle = Resources.Load("Choice") as Texture;
		potato = Resources.Load("Potato") as Texture;
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
		else if (Environment.Level == 16) {
			GUI.DrawTexture(new Rect(0f, 0f, Screen.width, Screen.height), puzzle);
		}
		else if (Environment.Level == 20) {
			GUI.DrawTexture(new Rect(0f, 0f, Screen.width, Screen.height), potato);
		}
		else if (Environment.Level == 42) {
			GUI.skin.label.alignment = TextAnchor.MiddleCenter;
			GUILayout.BeginArea(new Rect(0f, Screen.height * 0.4f, Screen.width, Screen.height * 0.6f));
			GUILayout.Label("Created by Isaac James");
			GUILayout.Label("For Ludum Dare 26");
			GUILayout.Label("Music by Ramiz Haddad");
			GUILayout.EndArea();
		}
	}
}
