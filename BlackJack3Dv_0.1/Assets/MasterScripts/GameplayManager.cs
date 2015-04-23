using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameplayManager : MonoBehaviour {
	private BlackJackGame game;

	//Non-existant UI
	public Sprite[] sprites;
	public Image PlayerOneCard;
	public Image PlayerTwoCard;
	public Image DealerCard;
	public Sprite flippedCard;
	private int Dealerhits = 1;
	private List<Image> playerOneCardsArray;
	private List<Image> playerTwoCardsArray;

	//Just Initial Stuff, Feel Free To Add More
	public enum State{ GAME_START, PLAYER_ONE_CLOSE_EYES, PLAYER_TWO_CLOSE_EYES, REVEAL_ONE, REVEAL_TWO, DECISION_ONE, DECISION_TWO, WIN_BOTH, WIN_ONE, WIN_TWO, LOSE_BOTH, PRINT_CARDS_ONE, PRINT_CARDS_TWO};
	public State currState = State.GAME_START;
	
	//TODO; REMOVE THIS
	private bool printText = true;
	
	// Use this for initialization
	void Start () {
		//Dealer is PlayerId 1
		PlayerOneCard.enabled = false;
		PlayerTwoCard.enabled = false;
		DealerCard.enabled = false;

		game = new BlackJackGame (2, sprites);
		PrintDescriptionText("GAME START");
		game.Hit (0);
		game.Hit (1);
		
/*		foreach (Card c in game.Deck) {
			Debug.Log (c.Name);
		}
*/		/**
		 * Example
		for (int i = 0; i<5; i++) {
			game.Hit (0);
		}*/

/*		foreach (Card c in game.Hands[0]) {
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
		case State.GAME_START:

			List<Card> dealerCards = game.GetDealerCards();
			string str11 = "";
			foreach(Card c in dealerCards){
				str11+= (c.Name)+" - ";
				DealerCard.GetComponent<Image> ().sprite = flippedCard;
				DealerCard.enabled = true;
				
				//Debug.Log (str11);
			}

			if (Input.GetKeyDown(KeyCode.Space)){
				ChangeState(State.PLAYER_TWO_CLOSE_EYES);
			}
			break;
		case State.PLAYER_ONE_CLOSE_EYES:
			PrintDescriptionText ("Player One Close Eyes");
			if (Input.GetKeyDown(KeyCode.Space)){
				ChangeState(State.REVEAL_TWO);
			}
			break;
		case State.PLAYER_TWO_CLOSE_EYES:
			//set turn to be player 1
			//tell player 2 to close eyes
			PrintDescriptionText ("Player Two Close Eyes");
			if(Input.GetKeyDown(KeyCode.Space)){
				ChangeState(State.REVEAL_ONE);
			}
			break;
			
			//REVEAL
		case State.REVEAL_ONE:
			//player 2 eyes are closed
			//press something to continue
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
			
			//print all cards in their hand
			//make sure to tell from which turn
			//press something to continue
			List<Card> playerOneCards = game.GetPlayerOneCards();
			showPlayerCards( playerOneCards, 0, 1 );
			/*string str1 = "";
			foreach(Card c in playerOneCards){
				str1+= (c.Name)+" - ";
				PlayerOneCard.GetComponent<Image> ().sprite = c.Image;
				PlayerOneCard.enabled = true;
			}*/

			//PrintDescriptionText(str1);
			
			if (Input.GetKeyDown(KeyCode.Space)){
				//PlayerOneCard.GetComponent<Image>().sprite = flippedCard;
				ChangeState(State.DECISION_ONE);
			}
			break;
		case State.PRINT_CARDS_TWO:
			
			List<Card> playerTwoCards = game.GetPlayerTwoCards();
			showPlayerCards( playerTwoCards, 1, 1 );
			/*string str2 = "";
			foreach(Card c in playerTwoCards){
				str2+= (c.Name)+" - ";
				PlayerTwoCard.GetComponent<Image> ().sprite = c.Image;
				PlayerTwoCard.enabled = true;
			}*/
			
			//PrintDescriptionText(str2);
			
			if(Input.GetKeyDown(KeyCode.Space)){
				//PlayerTwoCard.GetComponent<Image>().sprite = flippedCard;
				ChangeState(State.DECISION_TWO);
			}
			break;
			
			//DECISION
		case State.DECISION_ONE:
		//still player 1's turn
		//player 2 can now look
		//player 1's decision to make	
			PrintDescriptionText ("One: A to Hit. S to Stay.");
			
			if (Input.GetKeyDown(KeyCode.A)){
				Card c = addPlayerTwoCard();
				//need to know dealer's score
				//For now just have dealer hit when player does
				addDealerCard();
				
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
				Card c = addPlayerOneCard();
				//For now just have dealer hit when player does
				addDealerCard();
				
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

	private void showPlayerCards( List<Card> playerCards, int whoseturn, int show )
	{
		if (show == 1) {
			if (whoseturn == 0) {
				//playerones turn
				string strP1 = "";
				foreach (Card c in playerCards) {
					strP1 = "";
					foreach (Image i in playerOneCardsArray) {
						//Debug.Log (playerOneCardsArray [i]);
						Debug.Log (c.Image);
					}
				}
			}
		}	
						/*playerOneCardsArray [i].GetComponent<Image> ().sprite = c.Image;
						playerOneCardsArray [i].enabled = true;
					}
				}
			} else {
				string strP2 = "";
				foreach (Card c in playerCards) {
					strP2 = "";
					foreach (Image i in playerTwoCardsArray) {
						playerTwoCardsArray [i].GetComponent<Image> ().sprite = c.Image;
						playerTwoCardsArray [i].enabled = true;
					}
				}
			}
		} else {
			if (whoseturn == 0) {
				//playerones turn
				string strP1 = "";
				foreach (Card c in playerCards) {
					strP1 = "";
					foreach (Image i in playerOneCardsArray) {
						playerOneCardsArray [i].GetComponent<Image> ().sprite = flippedCard;
						playerOneCardsArray [i].enabled = true;
					}
				}
			} else {
				string strP2 = "";
				foreach (Card c in playerCards) {
					strP2 = "";
					foreach (Image i in playerTwoCardsArray) {
						playerTwoCardsArray [i].GetComponent<Image> ().sprite = flippedCard;
						playerTwoCardsArray [i].enabled = true;
					}
				}
			}
		}*/

	}


	private void addDealerCard()
	{
		game.Hit (1);
		Image clone;
		Vector3 NewPos = DealerCard.transform.position;
		NewPos.x = DealerCard.transform.position.x - (10 * Dealerhits);;
		clone = Instantiate (DealerCard, NewPos, transform.rotation) as Image;
		clone.transform.SetParent( DealerCard.transform, true );
		clone.transform.localScale = new Vector3 (1, 1, 1);
		clone.GetComponent<Image> ().sprite = flippedCard;
		Dealerhits++;
	}
	private Card addPlayerOneCard()
	{
		Card c = game.Hit (0);
		Image cloneOne;
		Vector3 NewPosOne = PlayerOneCard.transform.position;
		NewPosOne.x = PlayerOneCard.transform.position.x - 10;
		cloneOne = Instantiate (PlayerOneCard, NewPosOne, PlayerOneCard.transform.rotation) as Image;
		cloneOne.transform.SetParent( PlayerOneCard.transform, true );
		cloneOne.transform.localScale = new Vector3 (1, 1, 1);
		cloneOne.GetComponent<Image> ().sprite = flippedCard;
		playerOneCardsArray.Add (cloneOne);
		return c;
	}
	private Card addPlayerTwoCard()
	{
		Card c = game.Hit (0);
		Image cloneTwo;
		Vector3 NewPosTwo = PlayerTwoCard.transform.position;
		NewPosTwo.x = PlayerTwoCard.transform.position.x - 10;
		cloneTwo = Instantiate (PlayerTwoCard, NewPosTwo, PlayerTwoCard.transform.rotation) as Image;
		cloneTwo.transform.SetParent( PlayerTwoCard.transform, true );
		cloneTwo.transform.localScale = new Vector3 (1, 1, 1);
		cloneTwo.GetComponent<Image> ().sprite = flippedCard;
		playerTwoCardsArray.Add (cloneTwo);
		return c;
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