using HearthstoneDraft.Models.CardBase;
using System.Collections.Generic;

namespace HearthstoneDraft.Models.Draft
{
    public class Player
    {
        public Player()
        {
            this.Picks = new List<Card>();
        }

        public string Name { get; set; }

        public int Position { get; set; }

        public List<Card> Picks { get; set; }
    }
}