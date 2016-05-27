using System.Collections.Generic;

namespace HearthstoneDraft.Models.History
{
    public class History
    {
        public string Playername { get; set; }
        public List<DraftHistory> Drafts { get; set; }
    }
}