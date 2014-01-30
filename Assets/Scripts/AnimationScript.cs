using UnityEngine;
using System.Collections;

public class AnimationScript : MonoBehaviour {
	
	void Awake(){
		GetComponent<MySimpleSprite>().PrePlay ();
		StartCoroutine (GetComponent<MySimpleSprite>().PlayAnimation (0));
	}
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
