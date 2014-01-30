using UnityEngine;
using System.Collections;

public class Bill {

	public bool paid;

	string billName;
	float amount;
	bool mandatory;
	int count;
	public int howManyPaid;

	public Bill(string n, float a, bool m){
		billName = n;
		amount = a;
		mandatory = m;
		count = 1;
	}
	public void SetCount(int c) {
		count = c;

	}
	public int GetCount() {
		return count;
	}
	public string GetName(){
		return billName;
	}

	public float GetAmount(){
		return amount;
	}

	public bool IsMandatory(){
		return mandatory;
	}
}
