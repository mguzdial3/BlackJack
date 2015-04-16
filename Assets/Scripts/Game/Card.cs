using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public enum Suit {SPADES, HEARTS, DIAMONDS, CLUBS};
//Just an information holder for cards
public class Card  {
	public string Name {get{ return GetNumberString() +" of "+GetSuitString();}}
	//Card number 2-10 are what you'd expect. 1 is Ace. 11 is Jack. 12 is Queen. 13 is King.
	private int number;
	public int Number{get{return number;}}

	//Suit Info
	private Suit suit;
	public Suit MySuit{get{return suit;}}



	//Texture Info
	private Texture image;
	public Texture Image {get {return image;}}

	//Constructor 1 - Use This With no Images
	public Card(int _number, Suit _suit): this(_number, _suit, null){}

	//Constructor 2 - Use This When You Have Images
	public Card(int _number, Suit _suit, Texture _image){
		number = _number;
		suit = _suit;
		image = _image;
	}

	//Getter for Number Sring
	public string GetNumberString(){
		string[] numberStrs = new string[]{"Ace", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Jack", "Queen", "King"};

		return numberStrs[number-1];
	}

	//Getter for Suit String
	public string GetSuitString(){
		string toReturn = "";

		switch (suit) {
		case Suit.SPADES:
			toReturn = "Spades";
			break;
		case Suit.HEARTS:
			toReturn = "Hearts";
			break;
		case Suit.DIAMONDS:
			toReturn = "Diamonds";
			break;
		case Suit.CLUBS:
			toReturn = "Clubs";
			break;
		}

		return toReturn;
	}

	//Returns Score for this Score as a list of numbers (cause of ace)
	public List<int> Score(){
		if (number > 1 && number <= 10) {
			return new List<int>{number};
		}
		else if (number==1){
			return new List<int>{1, 11};
		}
		else{
			return new List<int>{10};
		}
	}





}


