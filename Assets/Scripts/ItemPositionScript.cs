using UnityEngine;
using System.Collections;

public class ItemPositionScript : MonoBehaviour {

	public GameObject up;
	public GameObject down;
	public GameObject left;
	public GameObject right;
	public GameObject item;

	SpriteRenderer sprite;
	bool selected;

	void Start(){
		sprite = GetComponent<SpriteRenderer>();
		sprite.enabled = false;
	}

	public void Select(){
		selected = true;
		sprite.enabled = true;
	}

	public void Deselect(){
		selected = false;
		sprite.enabled = false;
	}

	public void Highlight(){
		sprite.enabled = true;
	}

	public void Dehighlight(){
		if(!selected){
			sprite.enabled = false;
		}
	}

	public void SetItem(GameObject newItem){
		if(newItem!=null){
			newItem.GetComponent<ItemScript>().RemoveFromPosition();
		}
		if(item != null){
			RemoveItem();
		}
		item = newItem;
		if(item!=null){
			item.GetComponent<ItemScript>().SetPosition(gameObject.GetComponent<ItemPositionScript>());
		}
	}

	public void RemoveItem(){
		if(item!=null){
			item.GetComponent<ItemScript>().RemovePosition();
		}
		item = null;
	}
}
