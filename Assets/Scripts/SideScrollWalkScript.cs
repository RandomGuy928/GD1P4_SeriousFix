using UnityEngine;
using System.Collections;

public class SideScrollWalkScript : MonoBehaviour {

	public float speed;
	public bool cameraTrack;
	public float cameraLevel;
	public bool isActive;
	public float minCam;
	public float maxCam;

	Animator animator;
	float xScale;
	Vector2 lastVel;
	Vector3 startPos;
	TransitionScript transition;

	// Use this for initialization
	void Start () {
		transition = GameObject.FindGameObjectWithTag("Transition").GetComponent<TransitionScript>();
		cameraTrack = false;
		isActive = false;
		animator = GetComponent<Animator>();
		xScale = transform.localScale.x;
		startPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		if(cameraTrack){
			Camera.main.transform.position = new Vector3(Mathf.Max (Mathf.Min (transform.position.x, maxCam), minCam), cameraLevel, Camera.main.transform.position.z);
		}
		if(isActive){
			if(Input.GetAxis("Horizontal") > 0f){
				transform.localScale = new Vector3(xScale, transform.localScale.y, transform.localScale.z);
			}
			else if(Input.GetAxis ("Horizontal") < 0f){
				transform.localScale = new Vector3(-xScale, transform.localScale.y, transform.localScale.z);
			}
			if(rigidbody2D.velocity.normalized != Vector2.zero || lastVel != Vector2.zero){
				animator.Play("otherwalking");
			}
			else{
				animator.Play ("otherstanding");
			}
			lastVel = rigidbody2D.velocity;
		}
	}

	void FixedUpdate(){
		if(isActive){
			rigidbody2D.velocity = new Vector3(Input.GetAxis ("Horizontal"),0f,0)*speed;
		}
	}

	public void StartWalking(){
		transition.QuickTransition (new Vector3(0f, 0f, -1000f));
		transform.position = startPos;
		isActive = true;
		cameraTrack = true;
	}

	public void StopWalking(){
		if(!isActive)
			return;
		isActive = false;
		cameraTrack = false;
		animator.Play ("otherstanding");
	}

	public void ResetPosition(){
		transform.position = startPos;
	}
}
