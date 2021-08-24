using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace WL.Model
{
    public class BaseDBObject
    {
        public int Id { get; set; }

        public BaseDBObject() {}
    }
}
