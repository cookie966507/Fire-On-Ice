﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class UI_Manager : MonoBehaviour {

	public GameManager gm;

	private List<GameObject> playerList;

	public List<GameObject> playerUIs = new List<GameObject>();

	public List<GameObject> abilityUIs = new List<GameObject>();

	public GameObject WinUI;

	public GameObject readyText;
	public GameObject goText;
	public float readyTextDuration;
	public float goTextDuration;

	private GameObject[] cooldownUIs;
	
	// Use this for initialization
	public void Start () {
		playerList = gm.playerList;
		cooldownUIs = GameObject.FindGameObjectsWithTag ("Cooldown");
		SetupAbilityPanels ();
		SetupAbilityIcons ();
	}

	public void ReadyTextEnable(){
		if(readyText){
			readyText.SetActive (true);
		}
		Destroy (readyText, readyTextDuration);
		Invoke ("GoTextEnable", readyTextDuration);
	}

	public void GoTextEnable(){
		if(goText){
			goText.SetActive (true);
		}
		Destroy (goText, goTextDuration);
		Invoke ("UIReady", goTextDuration);
	}

	public void UIReady(){
		gm.StartGame ();
	}

	void SetupAbilityPanels(){
		//set ability UIs
		for(int j = 0; j < playerList.Count; j++){
			playerUIs[j].SetActive(true);
		}

	}

	void SetupAbilityIcons(){
		//Debug.Log ("SetupAbilityIcons");
		for (int i = 0; i < playerList.Count; i++){
			//Get sprites from playerAttack

			List<Sprite> sprites = new List<Sprite>();
			for (int j = 0; j < 3; j++){
				sprites.Add(playerList[i].GetComponent<PlayerAttack>().abilities[j].icon);
			}
			Debug.Log(sprites.Count);
			//Assign sprites
			//int playerNum = playerList[i].GetComponent<PlayerAttack>().joystickNum-1;

			for (int k = 0; k < 3; k++){

				Image image = abilityUIs[i].GetComponent<RectTransform>().Find("Ability_" + (k+1))
					.GetComponent<Image>();
				image.sprite = sprites[k];
			}

		}
	}


	void OnGUI(){
		//update cooldown UI
		//i is player number; j is ability number.
		if(gm.GameInProgress){
			for (int i = 0; i < playerList.Count; i++) {
				for (int j = 0; j < 3; j++){
					int playerNum = playerList[i].GetComponent<PlayerAttack>().playerNum;
					
					bool abilityReady = playerList[i].GetComponent<PlayerAttack>().abilities[j].abilityReady;
					
					GameObject cooldownUI = abilityUIs[playerNum-1].GetComponent<RectTransform>().Find("Ability_" + (j+1) 
					                                                                                   + "/Cooldown").gameObject;
					cooldownUI.SetActive(!abilityReady);
					if(!abilityReady){
						Text text = cooldownUI.GetComponent<RectTransform>().Find("Text").GetComponent<Text>();
						text.text = playerList[i].GetComponent<PlayerAttack>().abilities[j].timeUntilReset.ToString();
					}
				}
			}
		}
	}

	public void ShowWinScreen(int winPlayerNum, int mostDamagePlayerNum){
		Text resultText = WinUI.transform.Find ("GameResult").GetComponent<Text> ();

		resultText.text = "Player " + winPlayerNum + " Wins!\n\n";
		if(mostDamagePlayerNum == 0){
			resultText.text += "WTH! You all did 0 damage!";
		}
		else{
			resultText.text += "Player "+  mostDamagePlayerNum + " Dealt the most damage!";
		}
		WinUI.SetActive (true);

	}

	public void Rematch(){
		Application.LoadLevel ("SelectionScreen");
	}

	public void BackToMainMenu(){
		Application.LoadLevel ("Main_Menu");
	}

}
