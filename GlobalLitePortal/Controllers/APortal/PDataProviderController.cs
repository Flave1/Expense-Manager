using PluglexHelper.PortalObjs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using GlobalLitePortalHelper.APIObjs;
using GlobalLitePortalHelper.APIServices;
using PluglexHelper.PortalManager;
using XPLUG.WEBTOOLKIT;

namespace GlobalLitePortal.Controllers.APortal
{
    public class PDataProviderController : Controller
    {
        public ActionResult LoadTabParents()
        {
            var add = new NameValueObject { Id = 0, Name = "-- Empty Tab Parent List --" };
            try
            {
                var userData = MvcApplication.GetUserData(User.Identity.Name);
                if (userData == null || userData.UserId < 1)
                {
                    return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
                }
                
                var searchObj = new TabSearchObj
                {
                    AdminUserId = userData.UserId,
                   Status = -2,
                   StopDate = "",
                   StartDate = "",
                   TabId = 0
                };

                var retVal = new PortalTabManager().LoadTabs(searchObj, userData.Username);
                if (retVal?.Status == null)
                {
                    return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
                }

                if (!retVal.Status.IsSuccessful)
                {
                    return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
                }

                if (!retVal.Tabs.Any())
                {
                    return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
                }

                var parentTabs = retVal.Tabs.FindAll(m => m.TabType < 2).OrderBy(g => g.Title);
                add = new NameValueObject { Id = 0, Name = "-- Select Item --" };

                var jsonitem = parentTabs.Select(o => new NameValueObject { Id = o.TabId, Name = o.Title }).ToList();
                jsonitem.Insert(0, add);
                return Json(jsonitem, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult LoadUsers()
        {
            var add = new NameValueObject { Id = 0, Name = "-- Empty User List --" };
            try
            {
                var userData = MvcApplication.GetUserData(User.Identity.Name);
                if (userData == null || userData.UserId < 1)
                {
                    return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
                }

                var searchObj = new UserSearchObj
                {
                    AdminUserId = userData.UserId,
                    Status = -2,
                    StopDate = "",
                    StartDate = "",
                    UserId = 0
                };

                var retVal = new PortalUserManager().LoadUsers(searchObj, userData.Username);
                if (retVal?.Status == null)
                {
                    return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
                }

                if (!retVal.Status.IsSuccessful)
                {
                    return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
                }

                if (!retVal.Users.Any())
                {
                    return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
                }

                var sleIds = Session["_selUserIds_"] as List<int> ?? new List<int>();
                
                var parentTabs = retVal.Users.FindAll(m => !sleIds.Contains(m.UserId)).OrderBy(g => g.Email);
                add = new NameValueObject { Id = 0, Name = "-- Select Item --" };

                var jsonitem = parentTabs.Select(o => new NameValueObject { Id = o.UserId, Name = o.Email }).ToList();
                jsonitem.Insert(0, add);
                return Json(jsonitem, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult LoadClients()
        {
            var add = new NameValueObject { Id = 0, Name = "-- Empty Client List --" };
            try
            {
                var userData = MvcApplication.GetUserData(User.Identity.Name);
                if (userData == null || userData.UserId < 1)
                {
                    return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
                }

                var searchObj = new ClientSearchObj
                {
                    AdminUserId = userData.UserId,
                    Status = -2,
                    ClientId = 0
                };

                var retVal = ClientService.LoadClients(searchObj, userData.Username);
                if (retVal?.Status == null)
                {
                    return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
                }

                if (!retVal.Status.IsSuccessful)
                {
                    return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
                }

                if (!retVal.Clients.Any())
                {
                    return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
                }
                
                var parentTabs = retVal.Clients.OrderBy(g => g.ClientName);
                add = new NameValueObject { Id = 0, Name = "-- Select Client --" };

                var jsonitem = parentTabs.Select(o => new NameValueObject { Id = o.ClientId, Name = o.ClientName }).ToList();
                jsonitem.Insert(0, add);
                return Json(jsonitem, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
            }

        }
    }
}