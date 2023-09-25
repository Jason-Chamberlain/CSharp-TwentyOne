using System;
using System.Collections.Generic;

namespace Casino
{
    // Define an abstract class representing a generic game
    public abstract class Game
    {
        private List<Player> _players = new List<Player>();                                     // List to store players in the game
        private Dictionary<Player, int> _bets = new Dictionary<Player, int>();                  // Dictionary to store player bets

        public List<Player> Players { get { return _players; } set { _players = value; } }      // Property to access players in the game
        public string Name { get; set; }                                                        // Name of the game
        public Dictionary<Player, int> Bets { get { return _bets; } set { _bets = value; } }    // Property to access player bets

        public abstract void Play();                                                            // Abstract method representing the game's play logic

        // Virtual method to list players in the game (can be overridden by derived classes)
        public virtual void ListPlayers()
        {
            foreach (Player player in Players)
            {
                Console.WriteLine(player.Name);
            }
        }
    }
}