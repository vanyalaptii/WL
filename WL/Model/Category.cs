using System;
using System.Collections.Generic;

namespace WL.Model
{
    public class Category : BaseDBObject
    {
        public Category() { }

        public string Name { get; set; }

        public List<Card> Cards { get; set; }
    }
}
