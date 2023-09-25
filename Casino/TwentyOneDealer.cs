using System.Collections.Generic;

namespace Casino.TwentyOne
{
    // Define a class to represent the dealer in the Twenty-One game
    public class TwentyOneDealer : Dealer
    {
        private List<Card> _hand = new List<Card>();        // List to store dealer's hand
        public List<Card> Hand { get { return _hand; } set { _hand = value; } } // Property to access dealer's hand
        public bool Stay { get; set; }                      // Whether the dealer has chosen to stay
        public bool isBusted { get; set; }                  // Whether the dealer has busted (hand value > 21)
    }
}