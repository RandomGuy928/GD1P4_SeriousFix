using UnityEngine;
using System.Collections;

public class AudioManagerScript : MonoBehaviour {

	public AudioSource door;
	
	[HideInInspector]
	public AudioSource music;
	public AudioClip bell;

	AudioConversationScript acScript;
	AudioDoorScript adScript;
	AudioMusicScript amScript;
	AudioTickScript atScript;

	// Use this for initialization
	void Start () {
		acScript = GetComponentInChildren<AudioConversationScript>();
		adScript = GetComponentInChildren<AudioDoorScript>();
		amScript = GetComponentInChildren<AudioMusicScript>();
		atScript = GetComponentInChildren<AudioTickScript>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void PlayAudioSuccesfulPurchase () {
		atScript.PlayAudioSuccesfulPurchase();
	}

	public void PlayUnsuccessful() {
		atScript.PlayUnsuccessful();
	}
	
	public void PlayAudioStart () {
		atScript.PlayAudioStart ();
	}
	
	public void PlayAudioNext() {
		atScript.PlayAudioNext();
	}
	
	public void PlayAudioScroll () {
		atScript.PlayAudioScroll ();
	}
	
	public void PlayManHappy () {
		acScript.PlayManHappy ();
	}
	
	public void PlayManSad () {
		acScript.PlayManSad();
	}
	
	public void PlayManVoice() {
		acScript.PlayManVoice ();
	}

	public void PlayWomanHappy () {
		acScript.PlayWomanHappy ();
	}
	
	public void PlayWomanSad () {
		acScript.PlayWomanSad();
	}
	
	public void PlayWomanVoice() {
		acScript.PlayWomanVoice ();
	}

	public void PlayAudioDoor () {
		adScript.PlayAudioDoor ();
	}
	
	public void PlayAudioDoorNazi () {
		adScript.PlayAudioDoorNazi ();
	}

	public void PlayBackgroundMusic () {
		amScript.PlayBackgroundMusic();
	}

	public void PlayNaziMusic() {
		amScript.PlayNaziMusic();
	}

	public void StopMusic() {
		amScript.StopMusic();
	}

	public void FadeOut() {
		amScript.FadeOut();
	}
}
