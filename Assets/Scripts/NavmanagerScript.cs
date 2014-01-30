using UnityEngine;
using System.Collections;

public class NavmanagerScript : MonoBehaviour {

	public LayerMask navMask;
	public bool enableNavLines = false;

	GameObject[] navPoints;
	GameObject navGroupManager;

	// Use this for initialization
	void Awake () {
		ArrayList np = new ArrayList();
		for(int i = 0; i < transform.childCount; i++){
			if(transform.GetChild (i).GetComponent<NavpointScript>() != null){
				np.Add (transform.GetChild (i).gameObject);
			}
			else if(transform.GetChild (i).GetComponent<NavgroupScript>() != null){
				navGroupManager = transform.GetChild(i).gameObject;
			}
		}

		navPoints = new GameObject[np.Count];
		for(int i = 0; i < np.Count; i++){
			navPoints[i] = (GameObject)np[i];
		}
	}
	
	// Update is called once per frame
	void Update () {

	}

	public GameObject GetNavGroupManager(){
		return navGroupManager;
	}
	
	public GameObject GetRandomDestination(GameObject current){
		bool foundDest = false;
		GameObject go = null;
		while(!foundDest){
			int i = Random.Range (0, navPoints.Length);
			go = navPoints[i];
			foundDest = (go.GetComponent<NavpointScript>().isDestination && !current.Equals (go));
		}
		return go;
	}

	public GameObject GetNavPoint(string navName){
		for(int i = 0; i < navPoints.Length; i++){
			if(navPoints[i].name.Equals (navName)){
				return navPoints[i];
			}
		}
		return null;
	}

	public Stack ShortestPath(GameObject start, GameObject end){
		InitializeNodesForShortestPath();
		start.GetComponent<NavpointScript>().SetDist(0f);

		SortedList nodes = new SortedList();

		nodes.Add (0f, start);

		while(nodes.Count > 0){
			if(end.Equals ((GameObject)nodes.GetByIndex (0))){
				break;
			}
			NavpointScript u = ((GameObject)nodes.GetByIndex (0)).GetComponent<NavpointScript>();
			nodes.RemoveAt (0);
			u.SetVisited();

			GameObject[] adj = u.GetAdjacentNodes();
			for(int i = 0; i < adj.Length; i++){
				NavpointScript v = adj[i].GetComponent<NavpointScript>();
				float alt = u.GetDistByIndex (i) + u.GetDist ();
				if(alt < v.GetDist () && !v.Visited ()){
					v.SetDist(alt);
					v.SetPrev(u.gameObject);
					if(nodes.ContainsValue (v))
						nodes.RemoveAt (nodes.IndexOfValue (v));
					nodes.Add (alt, v.gameObject);
				}
			}
		}
		Stack s = new Stack();

		GameObject node = end;
		while(node != null){
			s.Push(node);
			node = node.GetComponent<NavpointScript>().GetPrev();
		}

		return s;
	}

	void InitializeNodesForShortestPath(){
		for(int i = 0; i < navPoints.Length; i++){
			navPoints[i].GetComponent<NavpointScript>().ResetForPath ();
		}
	}
}
