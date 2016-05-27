using HearthstoneDraft.Hubs;
using HearthstoneDraft.Logic;
using HearthstoneDraft.Models.Draft;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HearthstoneDraft.Controllers
{
    public class DraftController : Controller
    {
        public ActionResult Index()
        {
            var model = new DraftIndex();
            if (SessionData.DraftContainer != null) model.Draft = SessionData.DraftContainer;
            return this.View("Index", model);
        }

        public ActionResult Start(string name, int players)
        {
            SessionData.DraftContainer = new Draft(players);
            SessionData.DraftContainer.AddPlayer(name);
            var url = new UrlHelper(Request.RequestContext).Action("Draft", new { playername = name });
            return this.Json(new { Url = url });
        }

        public ActionResult Join(string name)
        {
            if (this.IsNewPlayer(name))
            {
                SessionData.DraftContainer.AddPlayer(name);
            }
            
            var context = GlobalHost.ConnectionManager.GetHubContext<DraftHub>();
            context.Clients.All.refreshDraft();
            context.Clients.All.refreshStatus();
            context.Clients.All.refreshPicks();
            var url = new UrlHelper(Request.RequestContext).Action("Draft", new { playername = name });
            return this.Json(new { Url = url });
        }

        public ActionResult Draft(string playername)
        {
            var draftIndex = new DraftIndex();
            draftIndex.Playername = playername;
            draftIndex.Draft = SessionData.DraftContainer;
            return this.View("Draft", draftIndex);
        }

        public PartialViewResult RefreshStatus(string name)
        {
            var draftIndex = new DraftIndex();
            draftIndex.Playername = name;
            draftIndex.Draft = SessionData.DraftContainer;
            return this.PartialView("_Status", draftIndex);
        }

        public PartialViewResult RefreshDraft(string name)
        {
            if (SessionData.DraftContainer.State != Enums.Draftstate.Started)
            {
                return this.PartialView("_Draft", new Pack());
            }
            var player = SessionData.DraftContainer.Players.FirstOrDefault(x => x.Name == name);
            var packnumber = (SessionData.DraftContainer.Picknumber + player.Position - 1) % SessionData.DraftContainer.Maxplayers;
            if (packnumber == 0) packnumber = SessionData.DraftContainer.Maxplayers;
            var pack = SessionData.DraftContainer.Packs.ElementAt(packnumber - 1);
            return this.PartialView("_Draft", pack);
        }

        public PartialViewResult RefreshPicks(string name)
        {
            var player = SessionData.DraftContainer.Players.FirstOrDefault(x => x.Name == name);
            return this.PartialView("_Picks", player);
        }

        public PartialViewResult RefreshManacurve(string name)
        {
            var player = SessionData.DraftContainer.Players.FirstOrDefault(x => x.Name == name);
            return this.PartialView("_Manacurve", player);
        }

        public void Pick(string name, string id, int packnro)
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<DraftHub>();
            var pack = SessionData.DraftContainer.Packs.FirstOrDefault(x => x.Packnumber == packnro);
            var player = SessionData.DraftContainer.Players.FirstOrDefault(x => x.Name == name);
            var pick = pack.Cards.FirstOrDefault(x => x.Id == id);
            player.Picks.Add(pick);
            pack.Cards.Remove(pick);
            pack.EnableForPick = false;
            if (pack.Cards.Count == 0)
            {
                SessionData.DraftContainer.NextPack();
            }
            if (SessionData.DraftContainer.Players.All(x => x.Picks.Count == SessionData.DraftContainer.Picknumber))
            {
                SessionData.DraftContainer.NextPick();
                if (SessionData.DraftContainer.State == Enums.Draftstate.Finished)
                {
                    context.Clients.All.refreshStatus();
                    FileDatabase.SaveDraftFile(SessionData.DraftContainer);
                    context.Clients.All.refreshDraft();
                    SessionData.DraftContainer = null;
                    return;
                }

                context.Clients.All.refreshDraft();
            }
        }

        public PartialViewResult ShowCardInfo(string id)
        {
            var card = SessionData.DraftContainer.AllCards.FirstOrDefault(x => x.Id == id);
            return this.PartialView("_Cardinfo", card);
        }

        private bool IsNewPlayer(string name)
        {
            return SessionData.DraftContainer.Players.FirstOrDefault(x => x.Name == name) == null;
        }
    }
}