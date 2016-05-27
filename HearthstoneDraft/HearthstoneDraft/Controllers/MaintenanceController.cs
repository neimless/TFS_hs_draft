using HearthstoneDraft.Logic;
using HearthstoneDraft.Models.Maintenance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HearthstoneDraft.Controllers
{
    public class MaintenanceController : Controller
    {
        public ActionResult Index()
        {
            var model = new Maintenance();
            model.FinishedDrafts = FileDatabase.GetSavedFilenames();
            return this.View("Index", model);
        }

        public ActionResult ResetContainer()
        {
            SessionData.DraftContainer = null;
            return this.View("Index");
        }
    }
}