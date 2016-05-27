using HSDraft.Logic;
using HSDraft.Models.Maintenance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HSDraft.Controllers
{
    [Authorize(Users = "neimless")]
    public class MaintenanceController : Controller
    {
        public ActionResult Index()
        {
            var model = new Maintenance();
            var fullHistory = FileDatabase.GetHistoryDb();
            model.Users = FileDatabase.GetUserfile();
            model.FinishedDrafts = fullHistory;
            return this.View("Index", model);
        }

        public ActionResult ResetContainer()
        {
            SessionData.DraftContainer = new List<Models.Draft.Draft>();
            return this.RedirectToAction("Index");
        }

        public ActionResult ResetHistory()
        {
            FileDatabase.ResetDraftHistory();
            return this.RedirectToAction("Index");
        }

        public ActionResult ResetUserbase()
        {
            FileDatabase.ResetUserDb();
            return this.RedirectToAction("Index");
        }
    }
}