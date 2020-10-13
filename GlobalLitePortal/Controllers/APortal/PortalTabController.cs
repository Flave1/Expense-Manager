using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using PluglexHelper.PortalManager;
using PluglexHelper.PortalObjs;
using GlobalLitePortal.PortalCore;
using GlobalLitePortal.PortalCore.Model;
using XPLUG.WEBTOOLKIT;

namespace GlobalLitePortal.Controllers.APortal
{
    public class PortalTabController : Controller
    {
        [PortalAuthorize(Roles = "PortalAdmin")]
        public ActionResult Index()
        {

            try
            {
                var userData = MvcApplication.GetUserData(User.Identity.Name);
                if (userData == null || userData.UserId < 1)
                {
                    ViewBag.Error = "Session Has Expired! Please Re-Login";
                    return View(new List<TabObj>());
                }

                // var bearerAuth = MvcApplication.GetSessionBearerData(userData.Username);
                
                var searchObj = new RoleSearchObj
                {
                    AdminUserId = userData.UserId,
                    RoleId = 0,
                    Status =  -2
                };
                var roleVal = new PortalRoleManager().LoadRoles(searchObj, userData.Username);
                if (roleVal?.Status == null)
                {
                    ViewBag.Error = "Role Item list is empty! Roles must be defined before setting up tabs";
                    return View(new List<TabObj>());
                }
                

                if (!roleVal.Status.IsSuccessful)
                {
                    ViewBag.Error = string.IsNullOrEmpty(roleVal.Status.Message.FriendlyMessage)
                        ? "Role Item list is empty! Roles must be defined before setting up tabs"
                        : roleVal.Status.Message.FriendlyMessage;
                    return View(new List<TabObj>());
                }

                if (roleVal.Roles == null)
                {
                    ViewBag.Error = "Role Item list is empty! Roles must be defined before setting up tabs";
                    return View(new List<TabObj>());
                }

                var roles = roleVal.Roles.Where(m => m.Status == 1).ToList();
                if (!roles.Any())
                {
                    ViewBag.Error = "Role Item list is empty! Roles must be defined before setting up tab";
                    return View(new List<TabObj>());
                }

                var allRoles = new List<NameValueObject>();

                foreach (var item in roles)
                {
                    allRoles.Add(new NameValueObject
                    {
                        Id = item.RoleId,
                        Name = item.Name
                    });
                }

                Session["_portal_tab_allroles"] = allRoles;

                var searchObj2 = new TabSearchObj
                {
                    AdminUserId = userData.UserId,
                    Status = -2,
                };

                var retVal = new PortalTabManager().LoadTabs(searchObj2, userData.Username);
                if (retVal?.Status == null)
                {
                    ViewBag.Error = "Tab Item list is empty!";
                    return View(new List<TabObj>());
                }

                if (!retVal.Status.IsSuccessful)
                {
                    ViewBag.Error = string.IsNullOrEmpty(retVal.Status.Message.FriendlyMessage)
                        ? "Tab Item list is empty!"
                        : retVal.Status.Message.FriendlyMessage;
                    return View(new List<TabObj>());
                }
                if (!retVal.Tabs.Any())
                {
                    ViewBag.Error = "Tab Item list is empty!";
                    return View(new List<TabObj>());
                }

                var allTabs = retVal.Tabs.OrderBy(m => m.TabId).ToList();
                Session["_portalTabs"] = allTabs;
                return View(allTabs);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                return View(new List<TabObj>());
            }
        }


        #region Add Tab
        public ActionResult _AddTab()
        {
            try
            {
                ViewBag.Error = "";
                var tabModel = new PortalRoleViewModel { AllRoles = new List<NameValueObject>(), MyRoleIds = new[] { 0 } };

                var userData = MvcApplication.GetUserData(User.Identity.Name);
                if (userData == null || userData.UserId < 1)
                {
                    ViewBag.Error = "Session Has Expired! Please Re-Login";
                    return View(tabModel);
                }

                if (!(Session["_portal_tab_allroles"] is List<NameValueObject> allRoles) || !allRoles.Any())
                {
                    ViewBag.Error = "Session Has Expired! Please Re-Login";
                    return View(tabModel);
                }

                tabModel.AllRoles = allRoles;


                return View(tabModel);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error Occurred! Please try again later";
                UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                return View(new PortalRoleViewModel());
            }
        }

        public JsonResult ProcessAddTabRequest(PortalRoleViewModel tabItem)
        {
            try
            {

                var userData = MvcApplication.GetUserData(User.Identity.Name) ?? new UserData();

                if (userData.UserId < 1)
                {
                    return Json(new { IsSuccessful = false, Error = "Your session has expired", IsAuthenticated = false });
                }

                if (tabItem == null)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Invalid Tab Item" });
                }

                if (string.IsNullOrEmpty(tabItem.Title) || tabItem.Title.Length < 2)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Tab Title is required" });
                }


                if (tabItem.TabOrder < 1)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Tab Order is required" });
                }

                if (tabItem.TabType < 1 || tabItem.TabType > 3)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Invalid Tab Type" });
                }

                if (tabItem.MyRoleIds == null || !tabItem.MyRoleIds.Any())
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Tab Roles are required" });
                }

                if (!(Session["_portal_tab_allroles"] is List<NameValueObject> allRoles) || !allRoles.Any())
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Your session has expired" });
                }

                var selRoles = allRoles.Where(m => tabItem.MyRoleIds.Contains(m.Id)).Select(m => m.Name).ToList();

                var passObj = new RegTabObj
                {
                    ContentUrl = string.IsNullOrEmpty(tabItem.ContentUrl) ? "NONE" : tabItem.ContentUrl,
                    TabParentId = tabItem.TabParentId,
                    LeftPanelUrl = string.IsNullOrEmpty(tabItem.LeftPanelUrl) ? "" : tabItem.LeftPanelUrl,
                    Roles = string.Join(";", selRoles),
                    RightPanelUrl = string.IsNullOrEmpty(tabItem.RightPanelUrl) ? "" : tabItem.RightPanelUrl,
                    Status = tabItem.StatusVal ? 1 : 0,
                    Title = tabItem.Title,
                    TabOrder = tabItem.TabOrder,
                    TabType = tabItem.TabType,
                    AdminUserId = userData.UserId,
                };


                var response = new PortalTabManager().AddTab(passObj, userData.Username);
                if (response == null)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Error Occurred! Please try again later" });
                }
                
                if (!response.Status.IsSuccessful)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = string.IsNullOrEmpty(response.Status.Message.FriendlyMessage) ? "Process Failed! Unable to add role" : response.Status.Message.FriendlyMessage });
                }

                return Json(new { IsAuthenticated = true, IsSuccessful = true, IsReload = false, Error = "" });
            }
            catch (Exception ex)
            {
                UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Process Error Occurred! Please try again later" });
            }
        }
        #endregion

        #region Edit Tab
        public ActionResult _EditTab(int tabId)
        {
            try
            {
                var tabModel = new PortalRoleViewModel { AllRoles = new List<NameValueObject>(), MyRoleIds = new[] { 0 } };
                ViewBag.Error = "";
                if (tabId < 1)
                {
                    ViewBag.Error = "Invalid selection";
                    return View(tabModel);
                }

                if (!(Session["_portalTabs"] is List<TabObj> tabList) || tabList.Count < 1)
                {
                    ViewBag.Error = "Error Occurred! Unable to prepare this portal tab information for modification";
                    return View(tabModel);
                }

                var thisTab = tabList.Find(m => m.TabId == tabId);
                if (thisTab == null || thisTab.TabId < 1)
                {
                    ViewBag.Error = "Error Occurred! Unable to prepare this portal tab information for modification";
                    return View(tabModel);
                }

                var userData = MvcApplication.GetUserData(User.Identity.Name);
                if (userData == null || userData.UserId < 1)
                {
                    ViewBag.Error = "Session Has Expired! Please Re-Login";
                    return View(tabModel);
                }

                if (!(Session["_portal_tab_allroles"] is List<NameValueObject> allRoles) || !allRoles.Any())
                {
                    ViewBag.Error = "Session Has Expired! Please Re-Login";
                    return View(tabModel);
                }

                tabModel.AllRoles = allRoles;

                var rolesId = new List<int>();
                var splitedRoles = thisTab.Roles.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var item in allRoles)
                {
                    if (splitedRoles.Contains(item.Name))
                    {
                        rolesId.Add(item.Id);
                    }
                }

                tabModel.TabId = thisTab.TabId;
                tabModel.Title = thisTab.Title;
                tabModel.LeftPanelUrl = thisTab.LeftPanelUrl;
                tabModel.RightPanelUrl = thisTab.RightPanelUrl;
                tabModel.Roles = thisTab.Roles;
                tabModel.Status = thisTab.Status;
                tabModel.StatusVal = thisTab.Status == 1;
                tabModel.TabOrder = thisTab.TabOrder;
                tabModel.TabParentId = thisTab.TabParentId;
                tabModel.TabType = thisTab.TabType;
                tabModel.ContentUrl = thisTab.ContentUrl;
                tabModel.MyRoleIds = rolesId.ToArray();

                return View(tabModel);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error Occurred! Please try again later";
                UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                return View(new PortalRoleViewModel());
            }
        }

        public JsonResult ProcessEditTabRequest(PortalRoleViewModel tabItem)
        {
            try
            {

                var userData = MvcApplication.GetUserData(User.Identity.Name) ?? new UserData();

                if (userData.UserId < 1)
                {
                    return Json(new { IsSuccessful = false, Error = "Your session has expired", IsAuthenticated = false });
                }

                //var bearerAuth = MvcApplication.GetSessionBearerData(userData.Username);
                //var authToken = MvcApplication.GetSessionAuthData(userData.Username);

                //if (string.IsNullOrEmpty(bearerAuth) || bearerAuth.Length < 5)
                //{
                //    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Invalid User Session! Please Re-Login" });
                //}

                //if (string.IsNullOrEmpty(authToken) || authToken.Length < 5)
                //{
                //    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Invalid User Session! Please Re-Login" });
                //}

                if (tabItem == null)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Invalid Tab Item" });
                }

                if (string.IsNullOrEmpty(tabItem.Title) || tabItem.Title.Length < 2)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Tab Title is required" });
                }


                if (tabItem.TabOrder < 1)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Tab Order is required" });
                }

                if (tabItem.TabType < 1 || tabItem.TabType > 3)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Invalid Tab Type" });
                }

                if (tabItem.MyRoleIds == null || !tabItem.MyRoleIds.Any())
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Tab Roles are required" });
                }

                if (!(Session["_portal_tab_allroles"] is List<NameValueObject> allRoles) || !allRoles.Any())
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Your session has expired" });
                }

                var selRoles = allRoles.Where(m => tabItem.MyRoleIds.Contains(m.Id)).Select(m => m.Name).ToList();

                var passObj = new EditTabObj
                {
                    TabId = tabItem.TabId,
                    ContentUrl = string.IsNullOrEmpty(tabItem.ContentUrl) ? "Dashboard|Index|" : tabItem.ContentUrl,
                    TabParentId = tabItem.TabParentId,
                    LeftPanelUrl = string.IsNullOrEmpty(tabItem.LeftPanelUrl) ? "" : tabItem.LeftPanelUrl,
                    Roles = string.Join(";", selRoles),
                    RightPanelUrl = string.IsNullOrEmpty(tabItem.RightPanelUrl) ? "" : tabItem.RightPanelUrl,
                    Status = tabItem.StatusVal ? 1 : 0,
                    Title = tabItem.Title,
                    TabOrder = tabItem.TabOrder,
                    TabType = tabItem.TabType,
                    AdminUserId = userData.UserId,
                };


                var response = new PortalTabManager().ModifyTab(passObj, userData.Username);
                if (response == null)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Error Occurred! Please try again later" });
                }

                if (!response.Status.IsSuccessful)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = string.IsNullOrEmpty(response.Status.Message.FriendlyMessage) ? "Process Failed! Unable to add role" : response.Status.Message.FriendlyMessage });
                }

                return Json(new { IsAuthenticated = true, IsSuccessful = true, IsReload = false, Error = "" });
            }
            catch (Exception ex)
            {
                UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Process Error Occurred! Please try again later" });
            }
        }

        public JsonResult ProcessDeleteTabRequest(int tabId)
        {
            try
            {
                var userData = MvcApplication.GetUserData(User.Identity.Name) ?? new UserData();

                if (userData.UserId < 1)
                {
                    return Json(new { IsSuccessful = false, Error = "Your session has expired", IsAuthenticated = false });
                }

                //var bearerAuth = MvcApplication.GetSessionBearerData(userData.Username);
                //var authToken = MvcApplication.GetSessionAuthData(userData.Username);

                //if (string.IsNullOrEmpty(bearerAuth) || bearerAuth.Length < 5)
                //{
                //    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Invalid User Session! Please Re-Login" });
                //}

                //if (string.IsNullOrEmpty(authToken) || authToken.Length < 5)
                //{
                //    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Invalid User Session! Please Re-Login" });
                //}

                if (tabId < 1)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Invalid Tab Selection" });
                }



                var passObj = new DeleteTabObj
                {
                    TabId = tabId,
                    AdminUserId = userData.UserId,
                };


                var response = new PortalTabManager().DeleteTab(passObj, userData.Username);
                if (response == null)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Error Occurred! Please try again later" });
                }

               

                if (!response.Status.IsSuccessful)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = string.IsNullOrEmpty(response.Status.Message.FriendlyMessage) ? "Process Failed! Unable to delete tab" : response.Status.Message.FriendlyMessage });
                }

                return Json(new { IsAuthenticated = true, IsSuccessful = true, IsReload = false, Error = "" });
            }
            catch (Exception ex)
            {
                UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Process Error Occurred! Please try again later" });
            }
        }
        #endregion
    }
}