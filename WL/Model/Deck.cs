using System;
using System.Collections.Generic;

namespace WL.Model
{
    public class Deck : BaseDBObject
    {
        public Deck() {}

        public string Name { get; set; }

        public List<CardDeck> Cards { get; set; }
    }
}
