using UnityEngine;
using System.Collections;

public class FinancesScript : MonoBehaviour {

	public GUISkin skin;

	Transaction[] transactions;
	Bill[] bills;
	PersonScript[] family;
	PersonScript[] jews;
	float money;
	double rev = 0; //counts up revenue
	double expen = 0; //counts up billcosts
	//double prof = 0; //calculates your profit for the day
	public int howmanymandatory = 0; //how many mandatory bills you have left to pay
	double totalprofit = 40; //all money you have saved up
	public int howmanybills;	//used to navigate. How many bills you have
	public int currentbill = 0; //what bill youre on
	bool donegui = false;
	public Font financeFont;
	bool gameover = false;

	FamilyManagerScript fms;
	EventManagerScript eventManager;
	InputManagerScript input;
	AudioManagerScript audioManager;


	public bool financing = false;

	// Use this for initialization
	void Start () {
		fms = GameObject.FindGameObjectWithTag("FamilyManager").GetComponent<FamilyManagerScript>();
		eventManager = GetComponent<EventManagerScript>();
		input = GameObject.FindGameObjectWithTag("EventManager").GetComponent<InputManagerScript>();
		audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManagerScript>();
	}
	
	// Update is called once per frame
	void Update () {
		if(financing){
			if(Input.GetButtonDown ("Select")){

				if( currentbill == howmanybills + 1) {//on done financing
					if(howmanymandatory ==0){ //if were done
						audioManager.PlayAudioNext();
						expen = 0;
						rev = 0;
						howmanybills = 0;
						currentbill = 0;
						EndFinances();
					}
					else {
						Debug.Log("Still have mandatory bills");
						audioManager.PlayUnsuccessful();
					}
				}




				else if (currentbill >= 0 && currentbill <= howmanybills){

					float billprice = bills[currentbill].GetAmount();
					paybill(billprice, bills[currentbill], bills[currentbill].IsMandatory());

				}
				if(gameover ==true) 
					Application.LoadLevel("Menu");
			}

			if(input.tickUp){
				currentbill--;
				audioManager.PlayAudioScroll ();
				if(currentbill < 0 && donegui == false)
					donegui = true;

				else if(currentbill < 0 && donegui == true){
					currentbill = howmanybills;
					donegui = false;

				}
				else if((currentbill == howmanybills || currentbill ==0)&& donegui ==true){
					donegui = false;
				}
				
				
			}
			if(input.tickDown) {
				currentbill++;
				audioManager.PlayAudioScroll();
				if(currentbill > howmanybills && donegui == false)
					donegui = true;

				else if(currentbill > howmanybills && donegui == true){
					currentbill = 0;
					donegui = false;
				}
				else if ((currentbill < 0  || currentbill == howmanybills) && donegui == true)
					donegui = false;

			}


		}
		if(currentbill >= 0 && currentbill <= howmanybills)
			donegui = false;
	}

	public void StartFinances(Transaction[] t, Bill[] b, float m){
		float mandatorycosts = 0;
		howmanymandatory = 0;
		transactions = t;
		bills = b;
		family = fms.GetFamily ();
		jews = fms.GetJews ();
		financing = true;
		money = m;
		howmanybills = (bills.Length - 1); //one extra to account for Done Financing screen

		//calculate rev and expenses
		for(int i = 0; i < transactions.Length; i++)
		{
			if(transactions[i] == null)
				continue;
			float saleprice = transactions[i].GetSalePrice();
			rev += saleprice;


			
		}
		totalprofit += rev; //adds your rev to your overall money
		for( int i = 0; i < bills.Length ; i++)
		{


			if( bills[i].IsMandatory()){
				Debug.Log("incrementing");
				howmanymandatory++;
				mandatorycosts += bills[i].GetAmount();
			}
		}
		if(totalprofit < mandatorycosts){
			Debug.Log("No sufficent funds to persist");
			gameover = true;
		}

	}

	public void EndFinances(){
		financing = false;
		eventManager.ReceiveFinances (bills);
	}

	public void paybill(float price, Bill thebill, bool mandatory) {
		if(totalprofit >= price) { //you have enough money

			int BillstoPay = thebill.GetCount();//local variable for how manby bills you have to pay

			if(thebill.howManyPaid < BillstoPay){ //if you haven't reached your cap on how many bills to pay
				//pay bill
				totalprofit -= price; //subtract from your money
				thebill.paid = true; //bill has been paid
				thebill.howManyPaid++; //increments how many bills youve paid
				expen += price; //adds to how much youre spending
				
				if(mandatory)		//is it mandatory?
					howmanymandatory--;
		}
			else Debug.Log("Already paid bill");
		}
		else Debug.Log("not enough money");
	}

	void OnGUI(){

		if(!financing)
			return;

		float pictureleft = Screen.width *.02f;
		float leftside = Screen.width *.11f;
		float secondside = Screen.width * .66f;//.55f;
		float rightside = Screen.width * .70f; //.85f;
		float helpside = secondside * 1.2f;//.740f;
		float paidside = secondside * .89f;//.875f;
		float bottom = Screen.height * .78f;



		float picturewidth = Screen.width * .05f;
		float pictureheight = Screen.height *.05f;
		float sizeofbubble = 380;
		float smallbubble = 250;
		float smallestbubble = 130;
		float bubbleheight = 40;
		float spacing = 2.0f;
		GUI.skin = skin;
		GUI.skin.label.font = financeFont;
		//int fontSize = GUI.skin.label.fontSize;
		//GUI.skin.label.fontSize = 30;
		//GUI.Label (new Rect(xGUI, yGUI, widthGUI, heightGUI), "IMMA DOIN FINANCES");
		//GUI.skin.label.fontSize = fontSize;

		GUI.Box(new Rect(0, 0, Screen.width,Screen.height), "");
		GUI.Label(new Rect (leftside,0,smallestbubble,bubbleheight), "Sales");
		GUI.Label(new Rect(secondside, 0, smallestbubble, bubbleheight), "Bills");
		GUI.Label(new Rect(rightside,bottom,smallbubble,bubbleheight), ("Revenue: " + rev.ToString("F2")));
		GUI.Label(new Rect(rightside,bottom * 1.1f,smallbubble,bubbleheight), ("Expenses: " + expen.ToString("F2")));
		GUI.Label(new Rect(rightside *.975f,bottom * 1.2f,smallbubble,30), ("Total Money: " + totalprofit.ToString("F2")));



		for(int i = 0; i < transactions.Length; i++)
		{	if(transactions[i] == null)
				continue;
			string itemname = transactions[i].GetItemName();
			float saleprice = transactions[i].GetSalePrice();

			float yposition = ((30 * (spacing* i)))+ 31.2f;

			GUI.skin.label.normal.background = transactions[i].GetIcon();
			GUI.Label (new Rect(pictureleft, yposition, picturewidth, pictureheight), "");	//item picture
			GUI.skin.label.normal.background = null;
			GUI.Label((new Rect(leftside, yposition, sizeofbubble,bubbleheight)), (itemname + ", Sold for: " + saleprice));	//what you sold
			
		}
		
		for( int i = 0; i < bills.Length ; i++)
		{
			string billname = bills[i].GetName();
			float billprice = bills[i].GetAmount();


			GUIStyle myStyle = new GUIStyle();
			myStyle.fontSize = 25;
			myStyle.font = financeFont;
			GUIStyle doneStyle = new GUIStyle();
			doneStyle.fontSize = 25;
			doneStyle.font = financeFont;
			GUIStyle costStyle = new GUIStyle();
			costStyle.fontSize = 17;
			costStyle.font = financeFont;
			GUIStyle mandatoryStyle = new GUIStyle();
			mandatoryStyle.fontSize = 22;
			mandatoryStyle.font = financeFont;
			GUIStyle paidStyle = new GUIStyle();
			paidStyle.fontSize = 22;
			paidStyle.font = financeFont;


			mandatoryStyle.normal.textColor = Color.red;
			paidStyle.normal.textColor = Color.green;


			if( bills[i].IsMandatory()) //Display mandatory 
				GUI.Label((new Rect (helpside,(30 * (spacing *i)) + 31.2f, smallestbubble, bubbleheight)), ("Mandatory"), mandatoryStyle);

			if( i == currentbill){
				myStyle.normal.textColor = Color.cyan;
				costStyle.normal.textColor = Color.cyan;
			}
			else{
				myStyle.normal.textColor = Color.gray;
				costStyle.normal.textColor = Color.gray;
			}


			float verticalposition = (30 * (spacing *i ) + 31.2f); //position for bills
			float costposition = verticalposition + (Screen.height * .04f); //position for costs

			//Checks count on bill to add to name if theres more than one bill
			int howmanylefttopay = (bills[i].GetCount() - bills[i].howManyPaid); //sets variable for how many bills you have left to paid

			if(howmanylefttopay > 1)
				GUI.Label((new Rect (secondside, verticalposition, sizeofbubble, bubbleheight)), (billname) + " (" + howmanylefttopay + ")", myStyle); //GUI for multiple bills
			else
				GUI.Label((new Rect (secondside, verticalposition, sizeofbubble, bubbleheight)), (billname), myStyle);//GUI for bill name


			/*if(billprice < 0)
				GUI.Label((new Rect (secondside, costposition, sizeofbubble, bubbleheight)), ("Cost: loof of bread"), costStyle);
			else*/
				GUI.Label((new Rect (secondside, costposition, sizeofbubble, bubbleheight)), ("Cost: " + billprice), costStyle); //GUI for cost
			//Done Financing button
			if(donegui)
				doneStyle.normal.textColor = Color.cyan;
			else
				doneStyle.normal.textColor = Color.gray;
			GUI.Label((new Rect (secondside,bottom * .96f, sizeofbubble, bubbleheight)),( "Done Financing"), doneStyle);	

			
			if(bills[i].paid)
				GUI.Label((new Rect (paidside,(30 * (spacing *i)) + 31.2f, smallestbubble, bubbleheight)), ("Paid"), paidStyle);
			
		//	if(bills[i].paid && bills[i].IsMandatory() == false)
		//		GUI.Label((new Rect (helpside,(30 * (spacing *i)) + 31.2f, smallestbubble, bubbleheight)), ("Paid"), paidStyle);
		}

		if( gameover ==true) {
			GUIStyle gameoverstyle = new GUIStyle();
			gameoverstyle.fontSize = 50;
			GUI.Label((new Rect (0,0, Screen.width, Screen.height)), "GAME OVER", gameoverstyle);
		}
	}
}
