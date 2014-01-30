using UnityEngine;
using System.Collections;

public class AudioConversationScript : MonoBehaviour {

	public AudioClip successfulPurchase;
	public AudioClip manHappy;
	public AudioClip manSad;
	public AudioClip womanHappy;
	public AudioClip womanSad;

	public AudioClip[] manVoice;
	public AudioClip[] womanVoice;

	// Use this for initialization
	void Start () {
		audio.volume = .5f;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void PlayManHappy () {
		if ( !audio.isPlaying ) {
			audio.clip = manHappy;
			audio.Play();
		}
	}

	public void PlayManSad () {
		if ( !audio.isPlaying ) {
			audio.clip = manSad;
			audio.Play();
		}
	}

	public void PlayManVoice() {
		if ( !audio.isPlaying ) {
			audio.clip = manVoice[Random.Range (0, manVoice.Length)];
			audio.Play();
		}

		//audio.volume = .2f;
	}

	public void PlayWomanHappy () {
		if ( !audio.isPlaying ) {
			audio.clip = womanHappy;
			audio.Play();
		}
	}
	
	public void PlayWomanSad () {
		if ( !audio.isPlaying ) {
			audio.clip = womanSad;
			audio.Play();
		}
	}
	
	public void PlayWomanVoice() {
		if ( !audio.isPlaying ) {
			audio.clip = womanVoice[Random.Range (0, womanVoice.Length)];
			audio.Play();
		}
	}
}
