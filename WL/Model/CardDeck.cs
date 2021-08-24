using System;
namespace WL.Model
{
    public class CardDeck
    {
        public int CardId { get; set; }

        public Card Card { get; set; }

        public int DeckId { get; set; }

        public Deck Deck { get; set; }

        public CardDeck() {}
    }
}
