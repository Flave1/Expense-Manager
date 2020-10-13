using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using GlobalLitePortalHelper.APIObjs;
using GlobalLitePortalHelper.APIServices;
using XPLUG.WEBTOOLKIT;

namespace GlobalLitePortal.Controllers.Business.Dashboard
{
    public class QuickDashboardController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult _StatsDisplayView()
        {
            try
            {
                ViewBag.Error = "";
                var userData = MvcApplication.GetUserData(User.Identity.Name);
                if (userData == null || userData.UserId < 1)
                {
                    ViewBag.Error = "Session Has Expired! Please Re-Login";
                    return View(new List<LookupStatObj>());
                }
              

                var retVal = LookupVersionService.LoadLookupStats(userData.Username);
                if (retVal?.Status == null)
                {
                    ViewBag.Error = "List is empty!";
                    return View(new List<LookupStatObj>());
                }

                if (!retVal.Status.IsSuccessful)
                {
                    ViewBag.Error = string.IsNullOrEmpty(retVal.Status.Message.FriendlyMessage)
                        ? "list is empty!"
                        : retVal.Status.Message.FriendlyMessage;
                    return View(new List<LookupStatObj>());
                }
                if (retVal.LookupStats == null || !retVal.LookupStats.Any())
                {
                    ViewBag.Error = "list is empty!";
                    return View(new List<LookupStatObj>());
                }
                
                return View(retVal.LookupStats);

            }
            catch (Exception ex)
            {
                ViewBag.Error = "Unable to load statistics! Please try again later";
                UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                return View(new List<LookupStatObj>());
            }
        }
    }
}