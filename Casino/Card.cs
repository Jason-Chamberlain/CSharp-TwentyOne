namespace Casino

{
    // Define a structure for a playing card
    public struct Card
    {
        public Suit Suit { get; set; }              // Represents the suit of the card
        public Face Face { get; set; }              // Represents the face value of the card

        // Override the ToString() method to provide a formatted string representation of the card
        public override string ToString()
        {
            return string.Format("{0} of {1}", Face, Suit);
        }
    }

    // Enumeration for different card suits
    public enum Suit
    {
        Clubs,
        Diamonds,
        Hearts,
        Spades
    }

    // Enumeration for different card face values
    public enum Face
    {
        Two,
        Three,
        Four,
        Five,
        Six,
        Seven,
        Eight,
        Nine,
        Ten,
        Jack,
        Queen,
        King,
        Ace
    }
}