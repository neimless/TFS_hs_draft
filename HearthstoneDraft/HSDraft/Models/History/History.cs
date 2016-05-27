using HSDraft.Models.CardBase;
using System.Collections.Generic;

namespace HSDraft.Models.History
{
    public class History
    {
        public History()
        {
            this.Drafts = new List<DraftHistory>();
            this.DraftIds = new List<string>();
            this.AllCards = new List<Card>();
        }

        public List<string> DraftIds { get; set; }
        public List<DraftHistory> Drafts { get; set; }
        public List<Card> AllCards { get; set; }

        public void AddFullDraftToHistory(Models.Draft.Draft draft)
        {
            var hist = new DraftHistory();
            hist.DraftId = draft.Id;
            hist.Players = draft.Players;
            this.Drafts.Add(hist);
        }
    }
}