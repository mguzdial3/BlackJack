using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BlackJackGame {
	//Deck Stuff
	private List<Card> deck;
	public List<Card> Deck {get{return deck;}}
	//Player Hands playerId key to list of cards Value. 
	private Dictionary<int, List<Card>> hands;
	public Dictionary<int, List<Card>> Hands{ get { return hands; } }

	public const int PLAYER_ID = 0;


	public BlackJackGame (int numPlayers): this(numPlayers, new Sprite[14]){}

	//Constructor 2-When you have imaged
	public BlackJackGame(int numPlayers, Sprite[] textures){
		//Debug.Log (textures);
		deck = new List<Card> ();
		Suit[] suits = new Suit[]{Suit.HEARTS, Suit.CLUBS, Suit.DIAMONDS, Suit.SPADES};

		foreach (Suit s in suits) {
			for (int i = 0; i<14; i++){
				deck.Add(new Card(i, s, textures[i]));
			}
		}
		//Shuffle the deck
		deck = Shuffle(deck);

		//Create all of the players
		hands = new Dictionary<int, List<Card>> ();
		hands [PLAYER_ID] = new List<Card> ();
		for (int i = 1; i<numPlayers; i++) {
			hands[i] = new List<Card>();
		}
	}

	//Take top card off deck and give it to player (returns the added card for display purposes)
	public Card Hit(int playerId){
		Card cardToAdd = deck [0];
		deck.Remove(cardToAdd);
		hands[playerId].Add(cardToAdd);

		return cardToAdd;
	}

	//Scores Hand and Returns an array of potential scores
	public List<int> Score(int playerId){
		List<int> scores = new List<int> ();
		scores.Add (0);
		List<Card> hand = hands [playerId];
		foreach (Card c in hand) {
			List<int> cardScores = c.Score();


			if (cardScores.Count==1){//This card is not an ace
				for(int i = 0; i<scores.Count; i++){
					scores[i]+=cardScores[0];
				}
			}
			else{//This card was an ace, special case
				List<int> scoresA = new List<int>(scores);
				List<int> scoresB = new List<int>(scores);
				for(int i = 0; i<scores.Count; i++){
					scoresA[i]+=cardScores[0];
					scoresB[i]+=cardScores[1];
				}
				//Either nothing happens or you added 11
				scoresA.AddRange(scoresB);
				scores = scoresA;
			}
		}

		return scores;
	}

	//For Shuffling Cards
	public List<Card> Shuffle(List<Card> list)  
	{  
		int n = list.Count;  
		while (n > 1) {  
			n--;  
			int k = Random.Range(0, n + 1);  
			Card value = list[k];  
			list[k] = list[n];  
			list[n] = value;  
		}

		return list;
	}

	//Get Winner Id
	public int GetWinnerId(){
		int winner = -1;
		int maxBelow = 0;
		foreach (KeyValuePair<int, List<Card>> kvp in hands) {
			List<int> localScore = Score(kvp.Key);

			foreach(int s in localScore){
				if(s<=21 && s>maxBelow){
					maxBelow = s;
					winner = kvp.Key;
				}
			}
		}

		return winner;
	}

	//These Methods Give you Odd or Even cards for player 1 or 2. Purely for display

	//Get Even Cards
	public List<Card> GetDealerCards(){
		List<Card> cards = new List<Card> ();
		for (int i = 0; i<hands[1].Count; i++){
			cards.Add (hands[1][i]);

		}
		return cards;
	}
	//Get Even Cards
	public List<Card> GetPlayerOneCards(){
		List<Card> cards = new List<Card> ();
		for (int i = 0; i<hands[PLAYER_ID].Count; i++){
			if (i%2==0){
				cards.Add (hands[PLAYER_ID][i]);
			}
		}
		return cards;
	}

	//Get Odd Cards
	public List<Card> GetPlayerTwoCards(){
		List<Card> cards = new List<Card> ();
		for (int i = 0; i<hands[PLAYER_ID].Count; i++){
			if (i%2==1){
				cards.Add (hands[PLAYER_ID][i]);
			}
		}
		return cards;
	}
}
