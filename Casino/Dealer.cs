using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace Casino
{
    // Define a class to represent a dealer in the Twenty-One game
    public class Dealer
    {
        public string Name { get; set; }        // Name of the dealer
        public Deck Deck { get; set; }          // The deck of cards used by the dealer
        public int Balance { get; set; }        // Dealer's balance (not typically used in game logic)

        // Method to deal a card to a player's hand
        public void Deal(List<Card> Hand)
        {
            Hand.Add(Deck.Cards.First());       // Add the first card from the deck to the player's hand
            string card = string.Format(Deck.Cards.First().ToString() ); // Format the card as a string
            Console.WriteLine(card);            // Display the dealt card

            // Append the delt card to a log file
            using (StreamWriter file = new StreamWriter(@"..\Logs\log.txt", true))
            {
                file.Write(DateTime.Now + ": ");
                file.WriteLine(card);
            }
            Deck.Cards.RemoveAt(0);             // Remove the dealt card from the deck
        }
    }
}