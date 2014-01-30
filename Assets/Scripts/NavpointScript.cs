using UnityEngine;
using System.Collections;

public class NavpointScript : MonoBehaviour {

	public bool isDestination;

	LayerMask mask;
	GameObject[] adjNodes;
	float[] dists;
	bool visited = false;
	float dist = 999f;
	GameObject prev;
	NavmanagerScript navManager;

	// Use this for initialization
	void Start () {
		navManager = GameObject.FindGameObjectWithTag ("NavManager").GetComponent<NavmanagerScript>();
		mask = navManager.navMask;
		SortedList adjacentNav = new SortedList();
		GameObject[] navs = navManager.GetNavGroupManager().GetComponent<NavgroupScript>().GetPointsInSharedGroups(gameObject);
		for(int i = 0; i < navs.Length; i++){
			if(!navs[i].Equals (gameObject) && !Physics2D.Linecast(transform.position, navs[i].transform.position, mask)){
				adjacentNav.Add(Vector3.Distance (transform.position, navs[i].transform.position), navs[i]);
			}
		}
		adjNodes = new GameObject[adjacentNav.Count];
		dists = new float[adjacentNav.Count];
		for(int i = 0; i < adjacentNav.Count; i++){
			adjNodes[i] = (GameObject)(adjacentNav.GetByIndex (i));
			dists[i] = Vector3.Distance (transform.position, adjNodes[i].transform.position);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(navManager.enableNavLines){
			for(int i = 0; i < adjNodes.Length; i++){
				Debug.DrawLine (transform.position, adjNodes[i].transform.position);
			}
		}
	}

	public GameObject[] GetAdjacentNodes(){
		return adjNodes;
	}

	public float GetDistByIndex(int i){
		return dists[i];
	}

	public float GetDist(){
		return dist;
	}

	public void ResetForPath(){
		dist = 999f;
		visited = false;
		prev = null;
	}

	public void SetDist(float d){
		dist = d;
	}

	public void SetVisited(){
		visited = true;
	}

	public bool Visited(){
		return visited;
	}

	public void SetPrev(GameObject go){
		prev = go;
	}

	public GameObject GetPrev(){
		return prev;
	}
	
}
