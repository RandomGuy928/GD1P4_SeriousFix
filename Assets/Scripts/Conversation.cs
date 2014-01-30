using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Conversation {
	public string name;
	public string currentText;
	public string currentInteractionType;
	public string[] options;
	public Texture2D left;
	public Texture2D right;
	public string currentSpeaker;

	public Dictionary<string, ConversationNode> convNodes = new Dictionary<string,ConversationNode>();
	public Dictionary<string, Texture2D> speakers = new Dictionary<string, Texture2D>();
	public ConversationNode currentNode;

	public void AddNode(ConversationNode n){
		convNodes.Add(n.name, n);
	}

	public void Initialize(){
		currentNode = convNodes["START"];
		SetUpNewNode ();
		speakers["null"] = null;
	}

	public void Advance(int i){
		if(currentNode.type == "listen"){
			string temp = currentNode.NextString();
			if(temp == ""){
				SwapToNewNode(0);
			}
			else{
				currentText = temp;
				currentSpeaker = currentNode.CurrentSpeaker();
			}
		}
		else if(currentNode.type == "respond"){
			SwapToNewNode (i);
		}
		else if(currentNode.type == "barter"
		        || currentNode.type == "need_item"
		        || currentNode.type == "find_item"){
			SwapToNewNode (i);
		}
		else{
			SwapToNewNode (0);
		}
	}

	public void SwapToNewNode(int i){
		currentNode.ResetNode ();
		currentNode = convNodes[currentNode.targets[i]];
		SetUpNewNode ();
	}

	void SetUpNewNode(){
		currentInteractionType = currentNode.type;
		if(currentNode.left != ""){
			left = (Texture2D)speakers[currentNode.left];
		}
		if(currentNode.right != ""){
			right = (Texture2D)speakers[currentNode.right];
		}
		if(currentInteractionType == "listen"){
			currentText = currentNode.NextString ();
		}
		else if(currentInteractionType == "respond"){
			options = currentNode.GetAllText ();
		}
		else if(currentInteractionType == "listen_random"){
			currentText = currentNode.GetRandomString();
		}
		if(currentNode.speakers != null && currentNode.speakers.Length > 0 && currentNode.speakers[0] != null && currentNode.speakers[0] != ""){
			currentSpeaker = currentNode.speakers[0];
		}
	}

	public string GetCurrentNodeName(){
		return currentNode.name;
	}
}


public class ConversationNode{
	public string type;
	public string name;
	public string[] text;
	public string[] targets;
	public string left;
	public string right;
	public string[] speakers;

	int currIndex;

	public ConversationNode(){
		type = "listen";
		currIndex = -1;
		left = "";
		right = "";
	}

	public string CurrentSpeaker(){
		return speakers[currIndex];
	}

	public void ResetNode(){
		currIndex = -1;
	}

	public string NextString(){
		currIndex += 1;
		if(currIndex >= text.Length){
			return "";
		}
		return text[currIndex];
	}

	public string[] GetAllText(){
		return text;
	}

	public bool ContinueNode(){
		if(currIndex < text.Length-1){
			return true;
		}
		return false;
	}

	public string GetNextNode(){
		return targets[0];
	}

	public string GetNextNode(int i){
		return targets[i];
	}

	public string GetRandomString(){
		return text[Random.Range((int)0, (int)text.Length)];
	}

	public void InitializeTextCount(int count){
		text = new string[count];
		targets = new string[count];
		speakers = new string[count];
	}

	public void AddText(string t){
		if(text == null){
			text = new string[1];
		}
		int i = 0;
		while(text[i] != null){
			i+=1;
		}
		text[i] = t;
	}

	public void AddTarget(string t){
		if(targets == null){
			targets = new string[1];
		}
		int i = 0;
		while(targets[i] != null){
			i+=1;
		}
		targets[i] = t;
	}

	public void AddSpeaker(string t){
		if(speakers == null){
			speakers = new string[1];
		}
		int i = 0;
		while(speakers[i] != null){
			i+=1;
		}
		speakers[i] = t;
	}
}