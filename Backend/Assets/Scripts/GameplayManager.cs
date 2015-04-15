using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameplayManager : MonoBehaviour {
	private BlackJackGame game;
	
	//Non-existant UI
	
	//Just Initial Stuff, Feel Free To Add More
	public enum State{PLAYER_ONE_CLOSE_EYES, PLAYER_TWO_CLOSE_EYES, REVEAL_ONE, REVEAL_TWO, DECISION_ONE, DECISION_TWO, WIN_BOTH, WIN_ONE, WIN_TWO, LOSE_BOTH, PRINT_CARDS_ONE, PRINT_CARDS_TWO};
	public State currState = State.PLAYER_TWO_CLOSE_EYES;
	
	//TODO; REMOVE THIS
	private bool printText = true;
	
	// Use this for initialization
	void Start () {
		//Dealer is PlayerId 1
		game = new BlackJackGame (2);
		game.Hit (0);
		game.Hit (1);

		/**
		 * Example
		for (int i = 0; i<5; i++) {
			game.Hit (0);
		}

		foreach (Card c in game.Hands[0]) {
			Debug.Log (c.Name);
		}
		Debug.Log ("Scores");
		foreach (int s in game.Score(0)) {
			Debug.Log (s);
		}
		*/
		
	}
	
	// Update is called once per frame
	void Update () {
		UpdateState ();
	}
	
	//Switch Statements Cause Why Not
	public void UpdateState(){
		//TODO; This should call individual methods to do this stuff, not everything should be here
		switch (currState) {
		case State.PLAYER_ONE_CLOSE_EYES:
			PrintDescriptionText ("Player One Close Eyes");
			if (Input.GetKeyDown(KeyCode.Space)){
				ChangeState(State.REVEAL_TWO);
			}
			break;
		case State.PLAYER_TWO_CLOSE_EYES:
			PrintDescriptionText ("Player Two Close Eyes");
			if(Input.GetKeyDown(KeyCode.Space)){
				ChangeState(State.REVEAL_ONE);
			}
			break;
			
			//REVEAL
		case State.REVEAL_ONE:
			PrintDescriptionText ("Here are your cards one, remember them!");
			
			
			if (Input.GetKeyDown(KeyCode.Space)){
				ChangeState(State.PRINT_CARDS_ONE);
			}
			break;
		case State.REVEAL_TWO:
			PrintDescriptionText ("Here are your cards two, remember them!");
			
			
			if(Input.GetKeyDown(KeyCode.Space)){
				ChangeState(State.PRINT_CARDS_TWO);
			}
			break;
			
			//Print Cards
		case State.PRINT_CARDS_ONE:
			
			List<Card> playerOneCards = game.GetPlayerOneCards();
			string str1 = "";
			foreach(Card c in playerOneCards){
				str1+= (c.Name)+" - ";
			}
			
			PrintDescriptionText(str1);
			
			if (Input.GetKeyDown(KeyCode.Space)){
				ChangeState(State.DECISION_ONE);
			}
			break;
		case State.PRINT_CARDS_TWO:
			
			List<Card> playerTwoCards = game.GetPlayerTwoCards();
			string str2 = "";
			foreach(Card c in playerTwoCards){
				str2+= (c.Name)+" - ";
			}
			
			PrintDescriptionText(str2);
			
			if(Input.GetKeyDown(KeyCode.Space)){
				ChangeState(State.DECISION_TWO);
			}
			break;
			
			//DECISION
		case State.DECISION_ONE:
			PrintDescriptionText ("One: A to Hit. S to Stay.");
			
			if (Input.GetKeyDown(KeyCode.A)){
				Card c = game.Hit(0);
				//For now just have dealer hit when player does
				game.Hit (1);
				
				Debug.Log (c.Name);
				
				//Check and see if you went over
				List<int> scores = game.Score(0);
				bool above = true;
				foreach (int s in scores){
					if(s<=21){
						above = false;
						break;
					}
				}
				
				if(above){
					ChangeState(State.WIN_TWO);
				}
				else{
					ChangeState(State.PLAYER_ONE_CLOSE_EYES);
					
				}
				
			}
			else if(Input.GetKeyDown(KeyCode.S)){
				//Did Both Win or Did Both Lose?
				int winner = game.GetWinnerId();
				
				if(winner==0){
					ChangeState(State.WIN_BOTH);
				}
				else{
					ChangeState(State.LOSE_BOTH);
				}
			}
			
			break;
		case State.DECISION_TWO:
			PrintDescriptionText ("Two: A to Hit. S to Stay.sa");
			
			
			if (Input.GetKeyDown(KeyCode.A)){
				Card c = game.Hit(0);
				//For now just have dealer hit when player does
				game.Hit (1);
				
				Debug.Log (c.Name);
				
				//Check and see if you went over
				List<int> scores = game.Score(0);
				bool above = true;
				foreach (int s in scores){
					if(s<=21){
						above = false;
						break;
					}
				}
				
				if(above){
					ChangeState(State.WIN_ONE);
					
				}
				else{
					ChangeState(State.PLAYER_TWO_CLOSE_EYES);
				}
				
			}
			else if(Input.GetKeyDown(KeyCode.S)){
				//Did Both Win or Did Both Lose?
				int winner = game.GetWinnerId();
				
				if(winner==0){
					ChangeState(State.WIN_BOTH);
				}
				else{
					ChangeState(State.LOSE_BOTH);
				}
			}
			break;
			
			//END STATES
		case State.WIN_ONE:
			PrintDescriptionText ("Player One Won!");
			break;
		case State.WIN_TWO:
			PrintDescriptionText ("Player Two Won!");
			break;
		case State.WIN_BOTH:
			PrintDescriptionText ("Both Won!");
			break;
		case State.LOSE_BOTH:
			PrintDescriptionText("BOTH LOST");
			break;
			
		}
		
		
	}
	
	//TODO; REMOVE THIS
	private void PrintDescriptionText(string str){
		if (printText) {
			Debug.Log (str);
			printText = false;
		}
	}
	
	private void ChangeState(State newState){
		printText = true;
		currState = newState;
	}
}
