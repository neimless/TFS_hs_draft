using HSDraft.Models.CardBase;
using System.Collections.Generic;

namespace HSDraft.Models.Draft
{
    public class Player
    {
        public Player()
        {
            this.Picks = new List<Card>();
            this.Finished = false;
        }

        public string Name { get; set; }
        public int Position { get; set; }
        public List<Card> Picks { get; set; }
        public bool Finished { get; set; }
    }
}