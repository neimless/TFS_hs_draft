using HSDraft.Models.CardBase;
using HSDraft.Models.Draft;
using System.Collections.Generic;

namespace HSDraft.Models.History
{
    public class DraftHistory
    {
        public DraftHistory()
        {
            this.Players = new List<Player>();
        }

        public List<Player> Players { get; set; }
        public string DraftId { get; set; }        
    }
}