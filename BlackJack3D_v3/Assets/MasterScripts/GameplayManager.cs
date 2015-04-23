using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameplayManager : MonoBehaviour {
	private BlackJackGame game;

	//Non-existant UI
	public AudioSource sound;

	public Sprite[] sprites;
	public Image PlayerOneCard;
	public Image PlayerTwoCard;
	public Image DealerCard;
	public Sprite flippedCard;
	private int Dealerhits = 1;
	private int firstDeal = 0;
	private List<Image> playerOneCardsArray;
	private List<Image> playerTwoCardsArray;
	private List<Image> dealerCardsArray;

	public Text Player1CloseEyes;
	public Text Player2CloseEyes;
	public Text Player1Remember;
	public Text Player2Remember;
	public Text BothWon;
	public Text OneWon;
	public Text TwoWon;
	public Text BothLost;

	public Button Player1Hit;
	public Button Player2Hit;
	public Button Player1Stay;
	public Button Player2Stay;

	public Image Player1Panel;
	public Image Player2Panel;

	public bool isPlaying;
	public bool shouldPlay;

	//Just Initial Stuff, Feel Free To Add More
	public enum State{ GAME_START, PLAYER_ONE_CLOSE_EYES, PLAYER_TWO_CLOSE_EYES, REVEAL_ONE, REVEAL_TWO, DECISION_ONE, DECISION_TWO, WIN_BOTH, WIN_ONE, WIN_TWO, LOSE_BOTH, PRINT_CARDS_ONE, PRINT_CARDS_TWO};
	public State currState = State.GAME_START;
	
	//TODO; REMOVE THIS
	private bool printText = true;
	
	// Use this for initialization
	void Start () {
		//Dealer is PlayerId 1

		sound = sound.GetComponent<AudioSource>();
		playerOneCardsArray = new List<Image> ();
		playerTwoCardsArray = new List<Image> ();
		dealerCardsArray    = new List<Image> ();

		Player1Hit = Player1Hit.GetComponent<Button> ();
		Player2Hit = Player2Hit.GetComponent<Button> ();
		Player1Stay = Player1Stay.GetComponent<Button> ();
		Player2Stay = Player2Stay.GetComponent<Button> ();

		Player1Panel = Player1Panel.GetComponent<Image> ();
		Player2Panel = Player2Panel.GetComponent<Image> ();

		Player1CloseEyes = Player1CloseEyes.GetComponent<Text> ();
		Player2CloseEyes = Player2CloseEyes.GetComponent<Text> ();
		Player1Remember  = Player1Remember.GetComponent<Text> ();
		Player2Remember  = Player2Remember.GetComponent<Text> ();
		BothWon          = BothWon.GetComponent<Text> ();
		OneWon           = OneWon.GetComponent<Text> ();
		TwoWon           = TwoWon.GetComponent<Text> ();
		BothLost         = BothLost.GetComponent<Text> ();

		Player1CloseEyes.enabled = false;
		Player2CloseEyes.enabled = false;
		Player1Remember.enabled  = false;
		Player2Remember.enabled  = false;
		BothWon.enabled          = false;
		OneWon.enabled           = false;
		TwoWon.enabled           = false;
		BothLost.enabled         = false;

		Player1Panel.enabled = false;
		Player2Panel.enabled = false;

		Player1Hit.enabled = false;
		Player2Hit.enabled = false;
		Player1Stay.enabled = false;
		Player2Stay.enabled = false;

		PlayerOneCard.enabled = false;
		PlayerTwoCard.enabled = false;
		DealerCard.enabled = false;

		game = new BlackJackGame (2, sprites);
		PrintDescriptionText("GAME START");
		Card c = game.Hit(0);
		Card d = game.Hit (1);
		addDealerCard ( d );
		addPlayerOneCard ( c );
		/*game.Hit (0);
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
	public void newGame()
	{
		Dealerhits = 1;
		firstDeal = 0;
		//updated players scores
		sound = sound.GetComponent<AudioSource>();
		playerOneCardsArray = new List<Image> ();
		playerTwoCardsArray = new List<Image> ();
		dealerCardsArray    = new List<Image> ();

		Player1Hit = Player1Hit.GetComponent<Button> ();
		Player2Hit = Player2Hit.GetComponent<Button> ();
		Player1Stay = Player1Stay.GetComponent<Button> ();
		Player2Stay = Player2Stay.GetComponent<Button> ();

		Player1Panel = Player1Panel.GetComponent<Image> ();
		Player2Panel = Player2Panel.GetComponent<Image> ();

		Player1CloseEyes = Player1CloseEyes.GetComponent<Text> ();
		Player2CloseEyes = Player2CloseEyes.GetComponent<Text> ();
		Player1Remember  = Player1Remember.GetComponent<Text> ();
		Player2Remember  = Player2Remember.GetComponent<Text> ();
		BothWon          = BothWon.GetComponent<Text> ();
		OneWon           = OneWon.GetComponent<Text> ();
		TwoWon           = TwoWon.GetComponent<Text> ();
		BothLost         = BothLost.GetComponent<Text> ();

		Player1CloseEyes.enabled = false;
		Player2CloseEyes.enabled = false;
		Player1Remember.enabled  = false;
		Player2Remember.enabled  = false;
		BothWon.enabled          = false;
		OneWon.enabled           = false;
		TwoWon.enabled           = false;
		BothLost.enabled         = false;

		Player1Panel.enabled = false;
		Player2Panel.enabled = false;

		Player1Hit.enabled = false;
		Player2Hit.enabled = false;
		Player1Stay.enabled = false;
		Player2Stay.enabled = false;

		PlayerOneCard.enabled = false;
		PlayerTwoCard.enabled = false;
		DealerCard.enabled = false;
		game = new BlackJackGame (2, sprites);
		PrintDescriptionText("GAME START");
		Card c = game.Hit(0);
		Card d = game.Hit (1);
		addDealerCard ( d );
		addPlayerOneCard ( c );

	}
	
	// Update is called once per frame
	void Update () {
		UpdateState ();
		playSound();
	}

	public void playSound()
	{
		if( !isPlaying && shouldPlay )
		{
			sound.Play();
			isPlaying = true;
		}
		else if( isPlaying && !shouldPlay )
		{
			sound.Stop();
			isPlaying = false;
		}
		else
		{
			sound.Stop();
			isPlaying = false;
		}
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
			Player1CloseEyes.enabled = true;
			Player2Panel.enabled = false;
			Player1Panel.enabled = false;
			if (Input.GetKeyDown(KeyCode.Space)){
				Player1CloseEyes.enabled = false;
				ChangeState(State.REVEAL_TWO);
			}
			break;
		case State.PLAYER_TWO_CLOSE_EYES:
			//set turn to be player 1
			//tell player 2 to close eyes
			Player2CloseEyes.enabled = true;
			PrintDescriptionText ("Player Two Close Eyes");
			Player2Panel.enabled = false;
			Player1Panel.enabled = false;
			if(Input.GetKeyDown(KeyCode.Space)){
				Player2CloseEyes.enabled = false;
				ChangeState(State.REVEAL_ONE);
			}
			break;
			
			//REVEAL
		case State.REVEAL_ONE:
			//player 2 eyes are closed
			//press something to continue
			PrintDescriptionText ("Here are your cards one, remember them!");
			Player1Remember.enabled = true;
			Player2Panel.enabled = false;
			Player1Panel.enabled = true;
			
			
			if (Input.GetKeyDown(KeyCode.Space)){
				Player1Remember.enabled = false;
				ChangeState(State.PRINT_CARDS_ONE);
			}
			break;
		case State.REVEAL_TWO:
			Player2Remember.enabled = true;
			PrintDescriptionText ("Here are your cards two, remember them!");
			Player1Panel.enabled = false;
			Player2Panel.enabled = true;
			
			
			if(Input.GetKeyDown(KeyCode.Space)){
				Player2Remember.enabled = false;
				ChangeState(State.PRINT_CARDS_TWO);
			}
			break;

						//Print Cards
		case State.PRINT_CARDS_ONE:
			shouldPlay = true;
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
				shouldPlay = false;
				//PlayerOneCard.GetComponent<Image>().sprite = flippedCard;
				List<Card> playerOneCardsHit = game.GetPlayerOneCards();
				showPlayerCards( playerOneCardsHit, 0, 0 );
				ChangeState(State.DECISION_ONE);
			}
			break;
		case State.PRINT_CARDS_TWO:
			shouldPlay = true;
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
				shouldPlay=false;
				//PlayerTwoCard.GetComponent<Image>().sprite = flippedCard;
				List<Card> playerTwoCardsHit = game.GetPlayerTwoCards();
				showPlayerCards( playerTwoCardsHit, 1, 0 );
				ChangeState(State.DECISION_TWO);
			}
			break;
			
			//DECISION
		case State.DECISION_ONE:
			PrintDescriptionText ("One: A to Hit. S to Stay.");
			Player1Hit.enabled  = true;
			Player1Stay.enabled = true;
			break;
		case State.DECISION_TWO:
			PrintDescriptionText ("Two: A to Hit. S to Stay.sa");
			Player2Hit.enabled = true;
			Player2Stay.enabled = true;	
			break;
			
			//END STATES
		case State.WIN_ONE:
			showAllCards( 2 );
			OneWon.enabled = true;
			PrintDescriptionText ("Player One Won!");
			if(Input.GetKeyDown(KeyCode.Space)){
				newGame();
				ChangeState(State.GAME_START);
			}
			break;
		case State.WIN_TWO:
			showAllCards( 1 );
			TwoWon.enabled = true;
			PrintDescriptionText ("Player Two Won!");
			if(Input.GetKeyDown(KeyCode.Space)){
				newGame();
				ChangeState(State.GAME_START);
			}
			break;
		case State.WIN_BOTH:
			showAllCards( 0 );
			BothWon.enabled = true;
			PrintDescriptionText ("Both Won!");
			if(Input.GetKeyDown(KeyCode.Space)){
				newGame();
				ChangeState(State.GAME_START);
			}
			break;
		case State.LOSE_BOTH:
			showAllCards( 0 );
			BothLost.enabled = true;
			PrintDescriptionText("BOTH LOST");
			if(Input.GetKeyDown(KeyCode.Space)){
				newGame();
				ChangeState(State.GAME_START);
			}
			break;
		}
	}

	private void showAllCards( int winner )
	{
		List<Card> FinalplayerTwoCards = game.GetPlayerTwoCards();
		List<Card> FinalplayerOneCards = game.GetPlayerOneCards();
		List<Card> FinaldealerCards    = game.GetDealerCards();
/*			Debug.Log(dealerCardsArray.Count);
			foreach( Image i in dealerCardsArray )
			{
			Debug.Log(i);
			}
			foreach( Card c in FinaldealerCards )
			{
			Debug.Log(c.Name);
			}
			
*/		if( winner == 1 )
		{
			int j = 0;
			foreach( Card c in FinalplayerTwoCards )
			{
				if( j < FinalplayerTwoCards.Count - 1 )
				{
					foreach (Image i in playerTwoCardsArray) {
						i.GetComponent<Image>().sprite = c.Image;
						i.enabled = true;
					}
				}
				j++;
			}
			foreach (Card c in FinalplayerOneCards) {
				foreach (Image i in playerOneCardsArray) {
					i.GetComponent<Image>().sprite = c.Image;
					i.enabled = true;
				}
			}
			foreach (Card c in FinaldealerCards) {
				foreach (Image i in dealerCardsArray ) {
					i.GetComponent<Image>().sprite = c.Image;
					i.enabled = true;
				}
			}
		}
		else if ( winner == 2 )
		{
			int j = 0;
			foreach( Card c in FinalplayerOneCards )
			{
				if( j < FinalplayerOneCards.Count - 1 )
				{
					foreach (Image i in playerOneCardsArray) {
						i.GetComponent<Image>().sprite = c.Image;
						i.enabled = true;
					}
				}
				j++;
			}
			foreach (Card c in FinalplayerTwoCards) {
				foreach (Image i in playerTwoCardsArray) {
					i.GetComponent<Image>().sprite = c.Image;
					i.enabled = true;
				}
			}
			foreach (Card c in FinaldealerCards) {
				foreach (Image i in dealerCardsArray ) {
					i.GetComponent<Image>().sprite = c.Image;
					i.enabled = true;
				}
			}

		}
		else
		{

			foreach (Card c in FinalplayerOneCards) {
				foreach (Image i in playerOneCardsArray) {
					i.GetComponent<Image>().sprite = c.Image;
					i.enabled = true;
				}
			}
			foreach (Card c in FinalplayerTwoCards) {
				foreach (Image i in playerTwoCardsArray) {
					i.GetComponent<Image>().sprite = c.Image;
					i.enabled = true;
				}
			}
			foreach (Card c in FinaldealerCards) {
				foreach (Image i in dealerCardsArray ) {
					i.GetComponent<Image>().sprite = c.Image;
					i.enabled = true;
				}
			}
		}

	}

	private void showPlayerCards( List<Card> playerCards, int whoseturn, int show )
	{
		if (show == 1) {
			if (whoseturn == 0) {
				int k, l;
				k = 0;
				foreach( Image i in playerOneCardsArray ) {
					l = 0;
					foreach (Card c in playerCards)
					{
						if( k == l )
						{
							i.GetComponent<Image>().sprite = c.Image;
							i.enabled = true;
						}
						l++;
					}
					k++;
				}
			} else {
				foreach (Card c in playerCards) {
					foreach (Image i in playerTwoCardsArray) {
						i.GetComponent<Image>().sprite = c.Image;
						i.enabled = true;
					}
				}
			}
		} else {
			if (whoseturn == 0) {
				foreach (Card c in playerCards) {
					foreach (Image i in playerOneCardsArray) {
						i.GetComponent<Image>().sprite = flippedCard;
						i.enabled = true;
					}
				}
			} else {
				foreach (Card c in playerCards) {
					foreach (Image i in playerTwoCardsArray) {
						i.GetComponent<Image>().sprite = flippedCard;
						i.enabled = true;
					}
				}
			}
		}
	}

	private void addDealerCard( Card c )
	{
		if (firstDeal == 0) {
			DealerCard.GetComponent<Image> ().sprite = flippedCard;
			DealerCard.enabled                       = true;
			dealerCardsArray.Add (DealerCard);
			firstDeal = 1;
		} 
		else 
		{
			Image clone;
			int last                            = dealerCardsArray.Count;
			DealerCard                          = dealerCardsArray[last-1];
			Vector3 NewPos                      = DealerCard.transform.position;
			NewPos.x                            = DealerCard.transform.position.x + 10;
			clone                               = Instantiate (DealerCard, NewPos, transform.rotation) as Image;
			clone.transform.SetParent( DealerCard.transform, true );
			clone.transform.localScale          = new Vector3 (1, 1, 1);
			clone.GetComponent<Image> ().sprite = flippedCard;
			clone.enabled                       = true;
			dealerCardsArray.Add(clone);
			Dealerhits++;
		}
	}
	private void addPlayerOneCard( Card c )
	{
		if (firstDeal == 1) {
			PlayerOneCard.GetComponent<Image> ().sprite = flippedCard;
			PlayerOneCard.enabled                       = false;
			playerOneCardsArray.Add (PlayerOneCard);
			firstDeal = 2;
		} 
		else 
		{
			Image cloneOne;
			int last                               = playerOneCardsArray.Count;
			PlayerOneCard                          = playerOneCardsArray[last-1];
			Vector3 NewPosOne                      = PlayerOneCard.transform.position;
			NewPosOne.y                            = PlayerOneCard.transform.position.y - 15;
			cloneOne                               = Instantiate (PlayerOneCard, NewPosOne, PlayerOneCard.transform.rotation) as Image;
			cloneOne.transform.SetParent (PlayerOneCard.transform, true);
			cloneOne.transform.localScale          = new Vector3 (1, 1, 1);
			cloneOne.GetComponent<Image> ().sprite = flippedCard;
			cloneOne.enabled                       = true;
			playerOneCardsArray.Add (cloneOne);
		}
	}
	private void addPlayerTwoCard( Card c )
	{
		if( firstDeal == 2 )
		{
			PlayerTwoCard.GetComponent<Image> ().sprite = flippedCard;
			PlayerTwoCard.enabled                       = true;
			playerTwoCardsArray.Add (PlayerTwoCard);
			firstDeal                                   = 3;
		}
		else 
		{
			Image cloneTwo;
			int last                               = playerTwoCardsArray.Count;
			PlayerTwoCard                          = playerTwoCardsArray[last-1];
			Vector3 NewPosTwo                      = PlayerTwoCard.transform.position;
			NewPosTwo.y                            = PlayerTwoCard.transform.position.y+15;
			cloneTwo                               = Instantiate (PlayerTwoCard, NewPosTwo, PlayerTwoCard.transform.rotation) as Image;
			cloneTwo.transform.SetParent( PlayerTwoCard.transform, true );
			cloneTwo.transform.localScale          = new Vector3 (1, 1, 1);
			cloneTwo.GetComponent<Image> ().sprite = flippedCard;
			cloneTwo.enabled                       = true;
			playerTwoCardsArray.Add (cloneTwo);
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
		Player1Hit.enabled  = false;
		Player1Stay.enabled = false;
		Player2Hit.enabled  = false;
		Player2Stay.enabled = false;
		printText = true;
		currState = newState;
	}

	public void player1Hit()
	{
		Card c = game.Hit (0);
		Card d = game.Hit (1);
		List<int> scores = game.Score(0);
		bool above = true;
		foreach (int s in scores){
			if(s<=21){
				above = false;
				break;
			}
		}
		
		if(above){
			Debug.Log(c.Name);
			ChangeState(State.WIN_TWO);
		}
		else{
			addPlayerTwoCard( c );
			//need to know dealer's score
			//For now just have dealer hit when player does
			addDealerCard(d);
			
			ChangeState(State.PLAYER_ONE_CLOSE_EYES);
		}
	}

	public void player1Stay()
	{
		//Did Both Win or Did Both Lose?
		int winner = game.GetWinnerId();
		
		if(winner==0){
			ChangeState(State.WIN_BOTH);
		}
		else{
			ChangeState(State.LOSE_BOTH);
		}
	}
	public void player2Hit()
	{
		//Check and see if you went over
		Card c = game.Hit (0);
		Card d = game.Hit (1);
		List<int> scores = game.Score(0);
		bool above = true;
		foreach (int s in scores){
			if(s<=21){
				above = false;
				break;
			}
		}
		
		if(above){
			Debug.Log(c.Name);
			ChangeState(State.WIN_ONE);
		}
		else{
			addPlayerOneCard( c );
			//For now just have dealer hit when player does
			addDealerCard(d);
			
			ChangeState(State.PLAYER_TWO_CLOSE_EYES);
		}
	}

	public void player2Stay()
	{
		//Did Both Win or Did Both Lose?
		int winner = game.GetWinnerId();
		
		if(winner==0){
			ChangeState(State.WIN_BOTH);
		}
		else{
			ChangeState(State.LOSE_BOTH);
		}
	}
}