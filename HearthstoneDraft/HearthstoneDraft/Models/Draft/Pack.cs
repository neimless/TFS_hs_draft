using HearthstoneDraft.Models.CardBase;
using System.Collections.Generic;

namespace HearthstoneDraft.Models.Draft
{
    public class Pack
    {
        public Pack()
        {
            this.Cards = new List<Card>();
            this.EnableForPick = true;
        }

        public int Packnumber { get; set; }

        public bool EnableForPick { get; set; }

        public List<Card> Cards { get; set; }
    }
}