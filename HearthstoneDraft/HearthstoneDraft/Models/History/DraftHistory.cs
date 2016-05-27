using HearthstoneDraft.Models.CardBase;
using System.Collections.Generic;

namespace HearthstoneDraft.Models.History
{
    public class DraftHistory
    {
        public string DraftId { get; set; }
        public List<Card> Picks { get; set; }
    }
}