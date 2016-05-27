using System.Collections.Generic;

namespace HearthstoneDraft.Models.CardBase
{
    public class CardBase
    {
        public CardBase()
        {
            this.Commons = new List<Card>();
            this.Classcards = new List<Card>();
            this.Rares = new List<Card>();
            this.Epics = new List<Card>();
            this.Legendaries = new List<Card>();
        }

        public List<Card> Commons { get; set; }

        public List<Card> Classcards { get; set; }

        public List<Card> Rares { get; set; }

        public List<Card> Epics { get; set; }

        public List<Card> Legendaries { get; set; }
    }
}