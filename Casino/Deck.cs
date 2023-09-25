using System;
using System.Collections.Generic;

namespace Casino
{
    // Define a class to represent a deck of playing cards
    public class Deck
    {
        public Deck()
        {
            Cards = new List<Card>();

            // Populate the deck with cards of different suits and face values
            for (int i = 0; i < 13; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    Card card = new Card();
                    card.Face = (Face)i;            // Assigning the face value using enumeration
                    card.Suit = (Suit)j;            // Assigning the suit using enumeration
                    Cards.Add(card);
                }
            }
        }

        public List<Card> Cards { get; set; }       // List to hold the cards in the deck

        // Method to shuffle the deck
        public void Shuffle(int times = 1)
        {
            for (int i = 0; i < times; i++)
            {
                List<Card> tempList = new List<Card>();
                Random random = new Random();

                // Randomly reorder the cards by selecting and removing cards from the original list
                while (Cards.Count > 0)
                {
                    int randomIndex = random.Next(0, Cards.Count);
                    tempList.Add(Cards[randomIndex]);
                    Cards.RemoveAt(randomIndex);
                }
                Cards = tempList;                   // Update the deck with the shuffled cards
            }
        }
    }
}