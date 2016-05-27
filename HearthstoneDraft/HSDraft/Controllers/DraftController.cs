using HSDraft.Hubs;
using HSDraft.Logic;
using HSDraft.Models.CardBase;
using HSDraft.Models.Draft;
using HSDraft.Models.History;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HSDraft.Controllers
{
    [System.Web.Mvc.Authorize]
    public class DraftController : Controller
    {
        public ActionResult Index()
        {
            var model = new DraftIndex();
            model.Draft = SessionData.DraftContainer;
            return this.View("Index", model);
        }

        public ActionResult Start(int players)
        {
            if (players > 8)
            {
                return null;
            }

            var draft = new Draft(players);
            draft.AddPlayer(User.Identity.Name);
            SessionData.DraftContainer.Add(draft);

            var context = GlobalHost.ConnectionManager.GetHubContext<DraftHub>();
            context.Clients.All.refreshLobby();

            var url = new UrlHelper(Request.RequestContext).Action("Draft", draft);
            return this.Json(new { Url = url });
        }

        public ActionResult Join(string id)
        {
            if (this.IsNewPlayer(User.Identity.Name, id))
            {
                SessionData.DraftContainer.First(x => x.Id == id).AddPlayer(User.Identity.Name);
            }
            
            var context = GlobalHost.ConnectionManager.GetHubContext<DraftHub>();
            context.Clients.All.refreshDraft();
            context.Clients.All.refreshStatus();
            context.Clients.All.refreshPicks();
            context.Clients.All.refreshLobby();
            var url = new UrlHelper(Request.RequestContext).Action("Draft", SessionData.DraftContainer.First(x => x.Id == id));
            return this.Json(new { Url = url });
        }

        public ActionResult Draft(string id)
        {
            return this.View("Draft", SessionData.DraftContainer.First(x => x.Id == id));
        }

        public PartialViewResult RefreshStatus(string id)
        {
            var draftcontainer = SessionData.DraftContainer.First(x => x.Id == id);
            this.CheckIfDraftFinished(id);
            return this.PartialView("_Status", draftcontainer);
        }

        public PartialViewResult RefreshDraft(string id)
        {
            var draft = SessionData.DraftContainer.FirstOrDefault(x => x.Id == id);
            if (draft == null || draft.State != Enums.Draftstate.Started)
            {
                return this.PartialView("_Draft", new Pack());
            }
            var player = draft.Players.FirstOrDefault(x => x.Name == User.Identity.Name);
            var packnumber = (draft.Picknumber + player.Position - 1) % draft.Maxplayers;
            if (packnumber == 0) packnumber = draft.Maxplayers;
            var pack = draft.Packs.ElementAt(packnumber - 1);
            return this.PartialView("_Draft", pack);
        }

        public PartialViewResult RefreshPicks(string id)
        {
            var player = SessionData.DraftContainer.First(x => x.Id == id).Players.FirstOrDefault(x => x.Name == User.Identity.Name);
            return this.PartialView("_Picks", player);
        }

        public PartialViewResult RefreshManacurve(string id)
        {
            var player = SessionData.DraftContainer.First(x => x.Id == id).Players.FirstOrDefault(x => x.Name == User.Identity.Name);
            return this.PartialView("_Manacurve", player);
        }

        public void Pick(string id, int packnro, string cardId)
        {
            var draft = SessionData.DraftContainer.First(x => x.Id == id);
            var context = GlobalHost.ConnectionManager.GetHubContext<DraftHub>();
            var pack = draft.Packs.FirstOrDefault(x => x.Packnumber == packnro);
            var player = draft.Players.FirstOrDefault(x => x.Name == User.Identity.Name);
            var pick = pack.Cards.FirstOrDefault(x => x.Id == cardId);
            player.Picks.Add(pick);
            pack.Cards.Remove(pick);
            pack.EnableForPick = false;
            if (pack.Cards.Count == 0)
            {
                draft.NextPack();
            }
            if (draft.Players.All(x => x.Picks.Count == draft.Picknumber))
            {
                draft.NextPick();     
                context.Clients.All.refreshDraft();
                if (draft.State == Enums.Draftstate.Finished)
                {
                    context.Clients.All.refreshStatus();
                }                
            }
        }

        public PartialViewResult ShowCardInfo(string id, string cardId)
        {
            var card = SessionData.DraftContainer.First(x => x.Id == id).AllCards.FirstOrDefault(x => x.Id == cardId);
            return this.PartialView("_Cardinfo", card);
        }

        public ActionResult DraftHistory()
        {
            var player = User.Identity.Name;
            var model = FileDatabase.GetHistoryDbForPlayer(player);
            model.AllCards = FileDatabase.GetAllCards();
            return this.View("Drafthistory", model);
        }

        private bool IsNewPlayer(string name, string id)
        {
            return SessionData.DraftContainer.First(x => x.Id == id).Players.FirstOrDefault(x => x.Name == name) == null;
        }

        private void CheckIfDraftFinished(string id)
        {
            var draft = SessionData.DraftContainer.First(x => x.Id == id);
            if (draft.State == Enums.Draftstate.Finished)
            {
                var player = draft.Players.FirstOrDefault(x => x.Name == User.Identity.Name);
                player.Finished = true;
                if (draft.Players.All(x => x.Finished))
                {
                    var draftHistory = new History();
                    draftHistory.AddFullDraftToHistory(draft);
                    draftHistory.DraftIds.Add(draft.Id);
                    FileDatabase.UpdateDraftHistory(draftHistory);
                    SessionData.DraftContainer.Remove(draft);
                }
            }            
        }
    }
}