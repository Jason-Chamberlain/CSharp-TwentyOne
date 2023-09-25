using System;
using System.Collections.Generic;
using System.Linq;
using Casino.Interfaces;

namespace Casino.TwentyOne

{
    // Define a class to represent the specific game of Twenty-One
    public class TwentyOneGame : Game, IWalkAway
    {
        public TwentyOneDealer Dealer { get; set; } // Instance of the dealer for the game

        // Override the Play method with the logic for playing Twenty-One
        public override void Play()
        {
            Dealer = new TwentyOneDealer();         // Create a new dealer for the game
            foreach (Player player in Players)
            {
                player.Hand = new List<Card>();     // Initialize player hands and stay status
                player.Stay = false;
            }
            Dealer.Hand = new List<Card>();         // Initialize dealer's hand and stay status
            Dealer.Stay = false;
            Dealer.Deck = new Deck();               // Create a new deck and shuffle it multiple times
            Dealer.Deck.Shuffle(3);
            

            // Ask players to place their bets and store them in the Bets dictionary
            foreach (Player player in Players)
            {
                bool validAnswer = false;
                int bet = 0;
                while (!validAnswer)
                {
                    Console.WriteLine("Place your bet!");
                    validAnswer = int.TryParse(Console.ReadLine(), out bet);
                    if (!validAnswer) Console.WriteLine("Please enter digits only, no decimal places.");
                }
                if (bet < 0) 
                {
                    throw new FraudException("Negative Bet Attempted. Security, kick this person out!");
                }
                bool successfullyBet = player.Bet(bet);
                if (!successfullyBet)
                {
                    return;
                }
                Bets[player] = bet;
            }

            // Deal initial cards to players and dealer
            for (int i = 0; i < 2; i++)
            {
                Console.WriteLine("Dealing...");
                foreach (Player player in Players)
                {
                    Console.Write("{0}: ", player.Name);
                    Dealer.Deal(player.Hand);
                    if (i == 1)
                    {
                        bool blackJack = TwentyOneRules.CheckForBlackJack(player.Hand);
                        if (blackJack)
                        {
                            // Handle player blackjack
                            Console.WriteLine("Blackjack! {0} wins {1}", player.Name, Bets[player]);
                            player.Balance += Convert.ToInt32((Bets[player] * 1.5) + Bets[player]);
                            return;
                        }
                    }
                }
                Console.Write("Dealer: ");
                Dealer.Deal(Dealer.Hand);
                if (i == 1)
                {
                    bool blackJack = TwentyOneRules.CheckForBlackJack(Dealer.Hand);
                    if (blackJack)
                    {
                        // Handle dealer blackjack
                        Console.WriteLine("Dealer has Blackjack! Everyone loses!");
                        foreach (KeyValuePair<Player, int> entry in Bets)
                        {
                            Dealer.Balance += entry.Value;
                        }
                        return;
                    }
                }
            }

            // Player turns
            foreach (Player player in Players)
            {
                while (!player.Stay)
                {
                    Console.WriteLine("Your cards are: ");
                    foreach (Card card in player.Hand)
                    {
                        Console.Write("{0}\n", card.ToString());
                    }
                    Console.WriteLine("\n\nHit or stay?");
                    string answer = Console.ReadLine().ToLower();
                    if (answer == "stay")
                    {
                        player.Stay = true;
                        break;
                    }
                    else if (answer == "hit")
                    {
                        Dealer.Deal(player.Hand);
                    }
                    bool busted = TwentyOneRules.isBusted(player.Hand);
                    if (busted)
                    {
                        // Handle player bust
                        Dealer.Balance += Bets[player];
                        Console.WriteLine("{0} busted! You lose your bet of {1}. Your balance is now {2}", player.Name, Bets[player], player.Balance);
                        Console.WriteLine("Do you want to play again?");
                        answer = Console.ReadLine().ToLower();
                        if (answer == "yes" || answer == "y" || answer == "yeah" || answer == "ya")
                        {
                            player.isActivelyPlaying = true;
                            return;
                        }
                        else
                        {
                            player.isActivelyPlaying = false;
                            return;
                        }
                    }
                }
            }

            // Dealer's turn
            Dealer.isBusted = TwentyOneRules.isBusted(Dealer.Hand);
            Dealer.Stay = TwentyOneRules.ShouldDealerStay(Dealer.Hand);
            while (!Dealer.Stay && !Dealer.isBusted)
            {
                Console.WriteLine("Dealer is hitting...");
                Dealer.Deal(Dealer.Hand);
                Dealer.isBusted = TwentyOneRules.isBusted(Dealer.Hand);
                Dealer.Stay = TwentyOneRules.ShouldDealerStay(Dealer.Hand);
            }
            if (Dealer.Stay)
            {
                Console.WriteLine("Dealer is staying.");
            }
            if (Dealer.isBusted)
            {
                // Handle dealer bust
                Console.WriteLine("Dealer busted!");

                // Update player and dealer balances after the dealer busts
                foreach (KeyValuePair<Player, int> entry in Bets)
                {
                    Console.WriteLine("{0} won {1}!", entry.Key.Name, entry.Value);
                    Players.Where(player => player.Name == entry.Key.Name).First().Balance += (entry.Value * 2);
                    Dealer.Balance -= entry.Value;
                }
                return;
            }

            // Compare hands and determine winners
            foreach (Player player in Players)
            {
                bool? playerWon = TwentyOneRules.CompareHands(player.Hand, Dealer.Hand);
                if (playerWon == null)
                {
                    // Handle push (tie)
                    Console.WriteLine("Push! No one wins.");
                    player.Balance += Bets[player];
                    Bets.Remove(player);
                }
                else if (playerWon == true)
                {
                    // Handle player win
                    Console.WriteLine("{0} won {1}!", player.Name, Bets[player]);
                    player.Balance += Bets[player] * 2;
                    Dealer.Balance -= Bets[player];
                }
                else
                {
                    // Handle dealer win
                    Console.WriteLine("Dealer wins {0}!", Bets[player]);
                    Dealer.Balance += Bets[player];
                }
                Console.WriteLine("Play again?");
                string answer = Console.ReadLine().ToLower();
                if (answer == "yes" || answer == "y" || answer == "yeah" || answer == "ya")
                {
                    player.isActivelyPlaying = true;
                }
                else
                {
                    player.isActivelyPlaying = false;
                }
            }
        }

        // Override the ListPlayers method to list players in the game
        public override void ListPlayers()
        {
            Console.WriteLine("21 Players:");
            base.ListPlayers();
        }

        // Implementation of the WalkAway method from the IWalkAway interface
        public void WalkAway(Player player)
        {
            throw new NotImplementedException();
        }
    }
}