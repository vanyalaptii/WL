using System;
using System.Collections.Generic;
using System.Linq;
using WL.Context;
using WL.Model;

namespace WL.Operations
{
    public class CardOperations
    {
        public CardOperations(){}

        public void AddCard(Card _card)
        {
            using (var Context = new WLContext())
            {
                Context.Cards.Add(_card);
                Context.SaveChanges();
            }
        }

        public void AddCardToDeck(Card _card, Deck deck)
        {
            using (var Context = new WLContext())
            {
                var deckInContext = Context.Decks.FirstOrDefault(d => d == deck);
                //deckInContext.Cards.Add(new CardDeck
                //{
                    
                //    Card = _card,
                //    CardId = _card.Id,
                //    Deck = deck,
                //    DeckId = deck.Id,
                //});

                deckInContext.Cards = new List<CardDeck>
                {
                    new CardDeck
                    {
                        Card = _card,
                        CardId = _card.Id,
                        Deck = deck,
                        DeckId = deck.Id,
                    }
                };

                Context.Decks.Update(deckInContext);
                Context.SaveChanges();
            }
        }

        public void RemoveCard(Card _card)
        {
            using (var Context = new WLContext())
            {
                Context.Cards.FirstOrDefault(x => x == _card);
                Context.SaveChanges();
            }
        }

        public List<Card> LoadAllCards()
        {
            using (var Context = new WLContext())
            {
                return Context.Cards.ToList();
            }
        }

        public List<Card> LoadCardsByCategory(Category _category)
        {
            using (var Context = new WLContext())
            {
                return Context.Cards.Where(x => x.Category == _category).ToList();
            }
        }

        public void MarkAsMemorised(Card _card)
        {
            using (var Context = new WLContext())
            {
                var card = Context.Cards.FirstOrDefault(c => c == _card);
                card.IsMemorised = true;
                Context.Update(card);
            }
        }

        public void UnMarkAsMemorised(Card _card)
        {
            using (var Context = new WLContext())
            {
                var card = Context.Cards.FirstOrDefault(c => c == _card);
                card.IsMemorised = false;
                Context.Update(card);
            }
        }
    }
}
