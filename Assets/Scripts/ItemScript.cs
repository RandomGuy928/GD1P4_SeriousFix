using UnityEngine;
using System.Collections;
using System;

public class ItemScript : MonoBehaviour{
	public string itemName;
	public int daysRemaining;
	public float basePrice;
	public Texture2D image;
	public string[] keywords;
	public float bakeTime;

	ItemPositionScript pos;

	void Start(){
		//Debug.Log (GetPriceString());
	}

	public string GetPriceString(){
		return GetPriceString (basePrice);
	}

	static public string GetPriceString(float value){
		string output = Math.Truncate ((double)value).ToString () + "zl ";
		float gr = value - Mathf.Round ((float)Math.Truncate ((double)value));
		if(gr < .1f){
			output = output + "0" + Mathf.Round (gr*100f).ToString () + "gr";
		}
		else{
			output = output + Mathf.Round (gr*100f).ToString () + "gr";
		}
		return output;	}

	public void RemovePosition(){
		pos = null;
		transform.position = new Vector3(999f, 999f, 999f);
	}

	public void RemoveFromPosition(){
		if(pos != null){
			pos.RemoveItem ();
		}
	}

	public void SetPosition(ItemPositionScript p){
		pos = p;
		transform.position = p.transform.position;
	}

	public ItemPositionScript GetPosition(){
		return pos;
	}

	public string GetPlaced(){
		if(pos != null)
			return "d";
		return "";
	}

	public void HighlightPosition(){
		if(pos!=null){
			pos.Highlight ();
		}
	}

	public void DehighlightPosition(){
		if(pos!=null){
			pos.Dehighlight ();
		}
	}

	public bool HasKeyword(string key){
		for(int i = 0; i < keywords.Length; i++){
			if(key.Equals (keywords[i].ToLower ()))
				return true;
		}
		return false;
	}
}
