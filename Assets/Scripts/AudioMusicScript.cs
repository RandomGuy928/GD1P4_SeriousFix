using UnityEngine;
using System.Collections;

public class AudioMusicScript : MonoBehaviour {

	public AudioClip backgroundMusic;
	public AudioClip naziMusic;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void PlayBackgroundMusic() {
		if ( !audio.isPlaying ) {
			audio.clip = backgroundMusic;
			audio.Play ();
		}
	}

	public void PlayNaziMusic() {
		if ( !audio.isPlaying ) {
			audio.clip = naziMusic;
			audio.Play ();
		}
	}

	public void StopMusic() {
		if ( audio.isPlaying ) {
			audio.Stop ();
		}
	}

	public void FadeOut() {
		Debug.Log ("Playing Fadeout");
		while ( audio.isPlaying ) {
			if ( (audio.volume - 1 * Time.deltaTime) <= 0.1 ) {
				audio.Stop ();
			}
			else {
				audio.volume -= (.1f * Time.deltaTime);
				Debug.Log (audio.volume);
			}
		}
	}

	public void FadeIn() {
		while ( audio.isPlaying ) {
			if ( (audio.volume + 1 * Time.deltaTime) >= 1 ) {
				audio.volume = 1f;
			}
			else {
				audio.volume += (1 * Time.deltaTime);
			}
		}
	}
}
