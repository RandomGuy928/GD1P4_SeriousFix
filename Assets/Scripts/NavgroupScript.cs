using UnityEngine;
using System.Collections;

public class NavgroupScript : MonoBehaviour {

	GameObject[] navGroups;

	// Use this for initialization
	void Awake () {
		ArrayList ng = new ArrayList();
		for(int i = 0; i < transform.childCount; i++){
			if(transform.GetChild (i).collider2D != null){
				ng.Add (transform.GetChild (i).gameObject);
			}
		}
		
		navGroups = new GameObject[ng.Count];
		for(int i = 0; i < ng.Count; i++){
			navGroups[i] = (GameObject)ng[i];
		}

		GameObject[] navPoints = GameObject.FindGameObjectsWithTag("NavPoint");
		for(int i = 0; i < navPoints.Length; i++){
			for(int j = 0; j < navGroups.Length; j++){
				if(navGroups[j].collider2D.OverlapPoint (navPoints[i].transform.position)){
					navGroups[j].GetComponent<NavspaceScript>().navPoints.Add (navPoints[i]);
				}
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public GameObject[] GetPointsInSharedGroups(GameObject go){
		ArrayList np = new ArrayList();
		for(int j = 0; j < navGroups.Length; j++){
			NavspaceScript ns = navGroups[j].GetComponent<NavspaceScript>();
			if(ns.navPoints.Contains (go)){
				for(int i = 0; i < ns.navPoints.Count; i++){
					if(!ns.navPoints[i].Equals (go) && !np.Contains (ns.navPoints[i])){
						np.Add (ns.navPoints[i]);
					}
				}
			}
		}
		
		GameObject[] output = new GameObject[np.Count];
		for(int i = 0; i < np.Count; i++){
			output[i] = (GameObject)np[i];
		}

		return output;
	}
}
