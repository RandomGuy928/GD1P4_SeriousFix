using UnityEngine;
using System.Collections;

public class AudioDoorScript : MonoBehaviour {

	public AudioClip audioDoor;
	public AudioClip audioDoorNazi;

	// Use this for initialization
	void Start () {
		audio.volume = .4f;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void PlayAudioDoor () {
		if ( !audio.isPlaying ) {
			audio.clip = audioDoor;
			audio.Play();
			//audio.volume = 1f;
		}
	}

	public void PlayAudioDoorNazi () {
		if ( !audio.isPlaying ) {
			audio.clip = audioDoorNazi;
			audio.Play();
		}
	}
}
