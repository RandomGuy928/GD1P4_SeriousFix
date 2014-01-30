using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CustomerSpawner {

	public string firstName = "Polish Man";
	public bool isMale = true;
	public float money = 10f;
	public float willingMarkup = 1.1f;
	public float markupLeniency = 0f;
	public string AIType = "customer";
	public string goal = "Delicacy";
	public string convID = "test_conversation";

	static string[] KeywordArray = {"bread", "pastry", "cake", "breakfast", "entree", "dessert", "sweet"};

	static CustomerSpawner MakeRandomCustomer(ArrayList usedWords){
		CustomerSpawner output = new CustomerSpawner();

		int r = (int)Random.Range (0, KeywordArray.Length);
		while(usedWords.Contains (KeywordArray[r])){
			r = (int)Random.Range (0, KeywordArray.Length);
		}
		output.goal = KeywordArray[r];
		usedWords.Add (KeywordArray[r]);
	
		r = (int)Random.Range (0, 5);
		switch(r){
		case 0:
			output.convID = "rand1";
			output.isMale = true;
			output.money = (Random.Range (9f,14f));
			output.willingMarkup = 1.4f;
			output.markupLeniency = 0f;
			break;

		case 1:
			output.convID = "rand2";
			output.isMale = true;
			output.money = (Random.Range (3f,6f));
			output.willingMarkup = 1.2f;
			output.markupLeniency = 0f;
			break;

		case 2:
			output.convID = "rand3";
			output.isMale = false;
			output.money = (Random.Range (2f,5f));
			output.willingMarkup = 1.2f;
			output.markupLeniency = 0f;
			break;

		case 3:
			output.convID = "rand4";
			output.isMale = false;
			output.money = (Random.Range (5f,9f));
			output.willingMarkup = 1.15f;
			output.markupLeniency = 0f;
			break;

		case 4:
			output.convID = "rand5";
			output.isMale = false;
			output.money = (Random.Range (9f,12f));
			output.willingMarkup = 1f;
			output.markupLeniency = 0f;
			break;
		}
		return output;
	}

	public static CustomerSpawner SpawnCustomer(int day, int transactionNumber, Dictionary<string, int> memory, ArrayList usedWords){
		CustomerSpawner output = new CustomerSpawner();
		switch(day){
		case 1:
			switch(transactionNumber){
			case 1:
				output = MakeRandomCustomer (usedWords);
				break;

			case 2:
				output = MakeRandomCustomer (usedWords);
				break;

			case 3:
				output.convID = "homeless1";
				output.isMale = true;
				output.goal = "any";
				break;

			case 4:
				output = MakeRandomCustomer (usedWords);
				break;

			case 5:
				output = MakeRandomCustomer (usedWords);
				break;

			case 6:
				output.convID = "lawyer2";
				output.isMale = true;
				break;

			case 7:
				output = MakeRandomCustomer (usedWords);
				break;
			}
			break;

		case 2:
			switch(transactionNumber){
			case 1:
				output = MakeRandomCustomer (usedWords);
				break;
				
			case 2:
				output.convID = "gossip3";
				output.isMale = false;
				break;
				
			case 3:
				output = MakeRandomCustomer (usedWords);
				break;
				
			case 4:
				output = MakeRandomCustomer (usedWords);
				break;
				
			case 5:
				output = MakeRandomCustomer (usedWords);
				break;
				
			case 6:
				output.convID = "homeless4";
				output.isMale = true;
				output.goal = "any";
				break;
				
			case 7:
				output = MakeRandomCustomer (usedWords);
				break;
			}
			break;

		case 3:
			switch(transactionNumber){
			case 1:
				output.convID = "barkeep5";
				output.isMale = true;
				break;
				
			case 2:
				output = MakeRandomCustomer (usedWords);
				break;
				
			case 3:
				output = MakeRandomCustomer (usedWords);
				break;
				
			case 4:
				output = MakeRandomCustomer (usedWords);
				break;
				
			case 5:
				output.convID = "lawyer6";
				output.isMale = true;
				break;
				
			case 6:
				output = MakeRandomCustomer (usedWords);
				break;
				
			case 7:
				output = MakeRandomCustomer (usedWords);
				break;
			}
			break;

		case 4:
			switch(transactionNumber){
			case 1:
				output = MakeRandomCustomer (usedWords);
				break;
				
			case 2:
				output = MakeRandomCustomer (usedWords);
				break;
				
			case 3:
				output.convID = "mother7";
				output.money = 35f;
				output.willingMarkup = 35f;
				output.isMale = false;
				output.goal = "entree";
				break;
				
			case 4:
				output = MakeRandomCustomer (usedWords);
				break;
				
			case 5:
				output = MakeRandomCustomer (usedWords);
				break;
				
			case 6:
				output = MakeRandomCustomer (usedWords);
				break;
				
			case 7:
				output.convID = "priest8";
				output.isMale = true;
				break;
			}
			break;

		case 5:
			switch(transactionNumber){
			case 1:
				output = MakeRandomCustomer (usedWords);
				break;
				
			case 2:
				output = MakeRandomCustomer (usedWords);
				break;
				
			case 3:
				output = MakeRandomCustomer (usedWords);
				break;
				
			case 4:
				output = MakeRandomCustomer (usedWords);
				break;
				
			case 5:
				output = MakeRandomCustomer (usedWords);
				break;
				
			case 6:
				output = MakeRandomCustomer (usedWords);
				break;
				
			case 7:
				output.convID = "gossip9";
				output.isMale = false;
				break;
			}
			break;

		case 6:
			switch(transactionNumber){
			case 1:
				output = MakeRandomCustomer (usedWords);
				break;
				
			case 2:
				output.convID = "butcher10";
				output.isMale = true;
				break;
				
			case 3:
				output = MakeRandomCustomer (usedWords);
				break;
				
			case 4:
				output = MakeRandomCustomer (usedWords);
				break;
				
			case 5:
				output = MakeRandomCustomer (usedWords);
				break;
				
			case 6:
				output.convID = "lawyer11";
				output.isMale = true;
				break;
				
			case 7:
				output = MakeRandomCustomer (usedWords);
				break;
			}
			break;

		case 7:
			switch(transactionNumber){
			case 1:
				output = MakeRandomCustomer (usedWords);
				break;
				
			case 2:
				output = MakeRandomCustomer (usedWords);
				break;
				
			case 3:
				if(memory.ContainsKey("buyhomeless2") && memory["buyhomeless2"] == 0){
					output.convID = "homeless12";
					output.isMale = true;
					output.goal = "any";
				}
				else{
					output.convID = "barkeep12";
					output.isMale = true;
				}
				break;
				
			case 4:
				output = MakeRandomCustomer (usedWords);
				break;
				
			case 5:
				output = MakeRandomCustomer (usedWords);
				break;
				
			case 6:
				output = MakeRandomCustomer (usedWords);
				break;
				
			case 7:
				output.convID = "gossip13";
				output.isMale = false;
				break;
			}
			break;

		case 8:
			switch(transactionNumber){
			case 1:
				output.convID = "soldier14";
				output.isMale = true;
				break;
				
			case 2:
				output = MakeRandomCustomer (usedWords);
				break;
				
			case 3:
				output = MakeRandomCustomer (usedWords);
				break;
				
			case 4:
				output = MakeRandomCustomer (usedWords);
				break;
				
			case 5:
				if((memory.ContainsKey("buyhomeless2") && memory["buyhomeless2"] == 0)
				   && !(memory.ContainsKey ("buyhomeless3") && memory["buyhomeless3"] == 0)){
					output.convID = "priestalt15";
					output.isMale = true;
				}
				else{
					output.convID = "priest15";
					output.isMale = true;
				}
				break;
				
			case 6:
				output = MakeRandomCustomer (usedWords);
				break;
				
			case 7:
				output = MakeRandomCustomer (usedWords);
				break;
			}
			break;

		case 9:
			switch(transactionNumber){
			case 1:
				output = MakeRandomCustomer (usedWords);
				break;
				
			case 2:
				output.convID = "butcher16";
				output.isMale = true;
				break;
				
			case 3:
				output = MakeRandomCustomer (usedWords);
				break;
				
			case 4:
				output = MakeRandomCustomer (usedWords);
				break;
				
			case 5:
				output = MakeRandomCustomer (usedWords);
				break;
				
			case 6:
				output = MakeRandomCustomer (usedWords);
				break;
				
			case 7:
				output.convID = "gossip17";
				output.isMale = false;
				break;
			}
			break;

		case 10:
			switch(transactionNumber){
			case 1:
				output = MakeRandomCustomer (usedWords);
				break;
				
			case 2:
				output = MakeRandomCustomer (usedWords);
				break;
				
			case 3:
				output = MakeRandomCustomer (usedWords);
				break;
				
			case 4:
				output = MakeRandomCustomer (usedWords);
				break;
				
			case 5:
				output = MakeRandomCustomer (usedWords);
				break;
				
			case 6:
				output.convID = "lawyer18";
				output.isMale = true;
				break;
				
			case 7:
				output.convID = "gossip19";
				output.isMale = false;
				break;
			}
			break;
		}
		return output;
	}
}
