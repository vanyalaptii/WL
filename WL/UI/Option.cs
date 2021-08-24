using System;

namespace WL.UI
{
    public class Option
    {
        public string Name { get; }
        public Action Selected { get; }
        public int Id { get; }

        public Option(string name, Action selected)
        {
            Name = name;
            Selected = selected;
        }
    }
}
