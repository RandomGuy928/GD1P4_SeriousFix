using UnityEngine;
using System.Collections;

public class Transaction{

	Texture2D icon;
	string itemName;
	string customerName;
	float basePrice;
	float salePrice;
	float profit;
	bool succeeded;

	public Transaction(){
		itemName = "no sale";
		succeeded = false;
	}

	public Transaction(GameObject item, GameObject customer, float price){
		ItemScript it = item.GetComponent<ItemScript>();
		icon = it.image;
		itemName = it.itemName;
		basePrice = it.basePrice;
		salePrice = price;
		profit = salePrice - basePrice;
		customerName = customer.GetComponent<CustomerScript>().firstName;
		succeeded = true;
	}

	public bool GetSucceeded(){
		return succeeded;
	}

	public Texture2D GetIcon(){
		return icon;
	}

	public string GetItemName(){
		return itemName;
	}

	public string GetCustomerName(){
		return customerName;
	}

	public float GetBasePrice(){
		return basePrice;
	}

	public string GetBasePriceString(){
		return ItemScript.GetPriceString (basePrice);
	}

	public float GetSalePrice(){
		return salePrice;
	}

	public string GetSalePriceString(){
		return ItemScript.GetPriceString (basePrice);
	}

	public float GetProfit(){
		return profit;
	}

	public string GetProfitString(){
		return ItemScript.GetPriceString (profit);
	}
}
