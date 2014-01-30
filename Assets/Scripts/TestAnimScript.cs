using UnityEngine;
using System.Collections;

public class TestAnimScript : MonoBehaviour {

	Animator animator;

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator>();
		animator.Play ("car 0");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
