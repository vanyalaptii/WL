using System;
using System.Collections.Generic;
using System.Linq;
using WL.Context;
using WL.Model;

namespace WL.Operations
{
    public class DeckOperations
    {
        public DeckOperations() {}

        public void AddDeck(Deck _deck)
        {
            using (var Context = new WLContext())
            {
                Context.Decks.Add(_deck);
                Context.SaveChanges();
            }
        }

        public void RemoveDeck(Deck _deck)
        {
            using (var Context = new WLContext())
            {
                Context.Decks.Remove(_deck);
                Context.SaveChanges();
            }
        }

        public List<Deck> LoadAllDeck()
        {
            using (var Context = new WLContext())
            {
                return Context.Decks.ToList();
            }
        }

        //public void AddCardToDeck(Deck _deck, Card _card)
        //{
        //    using (var Context = new WLContext())
        //    {
        //        var card = _card;
        //        card.Decks.Add(_deck);
        //        Context.SaveChanges();
        //        //TODO: Test that
        //    }
        //}

        //public void RemoveCardFromDeck(Deck _deck, Card _card)
        //{
        //    using (var Context = new WLContext())
        //    {
        //        var card = _card;
        //        card.Decks.Remove(_deck);
        //        Context.SaveChanges();
        //        //TODO: Test that
        //    }
        //}

        //public List<Card> LoadAllCardsFromDeck(Deck _deck)
        //{
        //    using (var Context = new WLContext())
        //    {
        //        var deck = Context.Decks.FirstOrDefault(d => d == _deck);
        //        return deck.Cards.ToList();
        //        //TODO: Test that
        //    }
        //}
    }
}
