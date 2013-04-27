using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {
	
	private static SoundManager singleton;
	
	private AudioSource music;
	
	void Start () {
		singleton = this;
		music = GetComponent<AudioSource>();
	}
	
	public static void setVolume(float pct) {
		singleton.music.volume = pct;
	}
}