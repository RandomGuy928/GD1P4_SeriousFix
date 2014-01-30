using UnityEngine;
using System.Collections;

public class CircleAnimTest : MonoBehaviour {

	AnimationEvent animEvent;

	// Use this for initialization
	void Start () {
		animEvent = new AnimationEvent();
		animEvent.functionName = "testFunc";
	}

	public void testFunc(AnimationEvent e){
		e.animationState.wrapMode = WrapMode.Once;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown("e")){
			GetComponent<Animator>().Play ("circle3");
		}
	}

}
