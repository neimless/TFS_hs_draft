using HearthstoneDraft.Logic;
using HearthstoneDraft.Models.CardBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HearthstoneDraft.Controllers
{
    public class CardBaseController : Controller
    {
        // GET: CardBase
        public ActionResult Index()
        {
            var model = FileDatabase.RetrieveCardbase();
            return View(model);
        }

        public ActionResult Save(Card model)
        {
            FileDatabase.AddCardToCardbase(model);
            return this.RedirectToAction("Index");
        }

        public ActionResult Remove(string id)
        {
            FileDatabase.RemoveCardFromCardbase(id);
            return this.RedirectToAction("Index");
        }
    }
}