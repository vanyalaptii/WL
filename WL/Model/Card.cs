using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace WL.Model
{
    public class Card : BaseDBObject
    {
        public Card() {}

        public string FrontSide { get; set; }

        public string BackSide { get; set; }

        public bool IsMemorised { get; set; }

        [ForeignKey("Category")]
        public int CategoryId { get; set; }

        public Category Category { get; set; }

        public List<CardDeck> Decks { get; set; }
    }
}
