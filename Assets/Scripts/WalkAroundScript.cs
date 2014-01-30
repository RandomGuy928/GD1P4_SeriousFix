using UnityEngine;
using System.Collections;

public class WalkAroundScript : MonoBehaviour {

	public float speed = 5f;

	bool wasStationary;
	Animator animator;

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		rigidbody2D.velocity = new Vector3(Input.GetAxis ("Horizontal"),Input.GetAxis ("Vertical"),0)*speed;
	}

	void Update(){
		if(wasStationary && (Input.GetAxis ("Horizontal") != 0f || Input.GetAxis ("Vertical") != 0f)){
			transform.up = -new Vector3(Input.GetAxis ("Horizontal"),Input.GetAxis ("Vertical"),0).normalized;
		}

		wasStationary = true;
		if(rigidbody2D.velocity.normalized != Vector2.zero){
			animator.Play ("walking");
			//transform.up = -(rigidbody2D.velocity.normalized);
			wasStationary = false;
			transform.up = Vector3.Lerp (transform.up, -rigidbody2D.velocity.normalized, .7f);
		}
		else{
			animator.Play ("standing");
		}
	}
}
