using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace HSDraft.Hubs
{
    public class DraftHub : Hub
    {
        public void UpdateDraft()
        {
            Clients.All.refreshDraft();
        }
    }
}