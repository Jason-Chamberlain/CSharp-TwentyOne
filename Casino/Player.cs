using System;
using System.Collections.Generic;

namespace Casino
{
    // Define a class to represent a player in the Twenty-One game
    public class Player
    {        
        // Constructor to initialize a player with name and beginning balance
        public Player(string name) : this(name, 100)
        {
        }
        public Player(string name, int beginningBalance)
        {
            Hand = new List<Card>();
            Balance = beginningBalance;
            Name = name;
        }

        private List<Card> _hand = new List<Card>();        // List to store player's hand
        public List<Card> Hand { get { return _hand; } set { _hand = value; } } // Property to access player's hand
        public int Balance { get; set; }                    // Player's balance (amount of money)
        public string Name { get; set; }                    // Player's name
        public bool isActivelyPlaying { get; set; }         // Whether the player is actively participating in the game
        public Guid Id { get; set; }                        // Player's Guid 
        public bool Stay { get; set; }                      // Whether the player chose to stay in their turn
        

        // Method to place a bet
        public bool Bet(int amount)
        {
            if (Balance - amount < 0)
            {
                Console.WriteLine("You do not have enough to place a bet of that size.");
                return false;
            }
            else
            {
                Balance -= amount;
                return true;
            }
        }

        // Overload the + operator to add a player to a game
        public static Game operator +(Game game, Player player)
        {
            game.Players.Add(player);
            return game;
        }

        // Overload the - operator to remove a player from a game
        public static Game operator -(Game game, Player player)
        {
            game.Players.Remove(player);
            return game;
        }
    }
}