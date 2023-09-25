using System.Collections.Generic;
using System.Linq;

namespace Casino.TwentyOne
{
    // Define a class to provide game rules and logic for the Twenty-One game
    public class TwentyOneRules
    {
        // Dictionary to store card values
        private static Dictionary<Face, int> _cardValues = new Dictionary<Face, int>()
        {
            // Assign values to different card faces
            [Face.Two] = 2,
            [Face.Three] = 3,
            [Face.Four] = 4,
            [Face.Five] = 5,
            [Face.Six] = 6,
            [Face.Seven] = 7,
            [Face.Eight] = 8,
            [Face.Nine] = 9,
            [Face.Ten] = 10,
            [Face.Jack] = 10,
            [Face.Queen] = 10,
            [Face.King] = 10,
            [Face.Ace] = 1
        };

        // Calculate all possible hand values considering Aces as 1 or 11
        private static int[] GetAllPossibleHandValues(List<Card> Hand)
        {
            int aceCount = Hand.Count(card => card.Face == Face.Ace);
            int[] result = new int[aceCount + 1];
            int value = Hand.Sum(card => _cardValues[card.Face]);
            result[0] = value;
            if (result.Length == 1) return result;
            for (int i = 1; i < result.Length; i++)
            {
                value += (1 * 10);
                result[1] = value;
            }
            return result;
        }

        // Check if a hand is a blackjack (sum of cards is 21)
        public static bool CheckForBlackJack(List<Card> Hand)
        {
            int[] possibleValues = GetAllPossibleHandValues(Hand);
            int value = possibleValues.Max();
            if (value == 21) return true;
            else return false;
        }

        // Check if a hand is busted (sum of cards is over 21)
        public static bool isBusted(List<Card> Hand)
        {
            int value = GetAllPossibleHandValues(Hand).Min();
            if (value > 21) return true;
            else return false;
        }

        // Determine if the dealer should stay (stop drawing cards)
        public static bool ShouldDealerStay(List<Card> Hand)
        {
            int[] possibleHandValues = GetAllPossibleHandValues(Hand);
            foreach (int value in possibleHandValues)
            {
                if (value > 16 && value < 22)
                {
                    return true;
                }
            }
            return false;
        }

        // Compare hands to determine the winner or a push
        public static bool? CompareHands(List<Card> PlayerHand, List<Card> DealerHand)
        {
            int[] playerResults = GetAllPossibleHandValues(PlayerHand);
            int[] dealerResults = GetAllPossibleHandValues(DealerHand);

            int playerScore = playerResults.Where(x => x < 22).Max();
            int dealerScore = dealerResults.Where(x => x < 22).Max();

            if (playerScore > dealerScore) return true;
            if (playerScore < dealerScore) return false;
            else return null; // Indicates a push (tie)
        }
    }
}