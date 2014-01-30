using UnityEngine;
using System.Collections;

public class AudioTickScript : MonoBehaviour {

	public AudioClip textStart;
	public AudioClip textScroll;
	public AudioClip textNext;
	public AudioClip successfulPurchase;
	public AudioClip unsuccessful;

	// Use this for initialization
	void Start () {
		audio.volume = .1f;
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void PlayUnsuccessful() {
		if ( !audio.isPlaying ) {
			audio.clip = unsuccessful;
			audio.Play();
		}
	}

	public void PlayAudioSuccesfulPurchase () {
		if ( !audio.isPlaying ) {
			audio.clip = successfulPurchase;
			audio.Play();
		}
	}


	public void PlayAudioStart () {
		if ( !audio.isPlaying ) {
			audio.clip = textStart;
			audio.Play();
		}
	}
	
	public void PlayAudioNext() {
		if ( !audio.isPlaying ) {
			audio.clip = textNext;
			audio.Play ();
		}
	}
	
	public void PlayAudioScroll () {
		if ( !audio.isPlaying ) {
			audio.clip = textScroll;
			audio.Play();
		}
	}
}
