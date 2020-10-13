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
    public class PortalRoleController : Controller
    {
        #region Role Admin

        [PortalAuthorize(Roles = "PortalAdmin")]
        public ActionResult Index()
        {
            try
            {
                var userData = MvcApplication.GetUserData(User.Identity.Name);
                if (userData == null || userData.UserId < 1)
                {
                    ViewBag.Error = "Session Has Expired! Please Re-Login";
                    return View(new List<RoleObj>());
                }

                var searchObj = new RoleSearchObj
                {
                    AdminUserId = userData.UserId,
                    RoleId = 0,
                    Status = -200,
                   
                };
                var retVal = new PortalRoleManager().LoadRoles(searchObj, userData.Username);
                if (retVal?.Status == null)
                {
                    ViewBag.Error = "Role Item list is empty!";
                    return View(new List<RoleObj>());
                }

                if (!retVal.Status.IsSuccessful)
                {
                    ViewBag.Error = string.IsNullOrEmpty(retVal.Status.Message.FriendlyMessage)
                        ? "Role Item list is empty!"
                        : retVal.Status.Message.FriendlyMessage;
                    return View(new List<RoleObj>());
                }
                if (!retVal.Roles.Any())
                {
                    ViewBag.Error = "Role Item list is empty!";
                    return View(new List<RoleObj>());
                }

                var allRoles = retVal.Roles.OrderBy(m => m.RoleId).ToList();
                Session["_portalRoles"] = allRoles;
                return View(allRoles);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                return View(new List<RoleObj>());
            }
        }

        #region Add Role
        public ActionResult _AddRole()
        {
            try
            {
                ViewBag.Error = "";
                return View(new RoleObj());
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error Occurred! Please try again later";
                UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                return View(new RoleObj());
            }
        }

        public JsonResult ProcessAddRoleRequest(string myRoleName, bool myRoleStatus)
        {
            try
            {

                var userData = MvcApplication.GetUserData(User.Identity.Name) ?? new UserData();

                if (userData.UserId < 1)
                {
                    return Json(new { IsSuccessful = false, Error = "Your session has expired", IsAuthenticated = false });
                }
                
                if (string.IsNullOrEmpty(myRoleName) || myRoleName.Length < 2)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Invalid Role Name" });
                }


                var passObj = new RegRoleObj
                {
                    Name = myRoleName,
                    Status = myRoleStatus ? 1 : 0,
                    AdminUserId = userData.UserId,
                };


                var response = new PortalRoleManager().AddRole(passObj, userData.Username);
                if (response?.Status == null)
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

        #region Edit Role
        public ActionResult _EditRole(int roleId)
        {
            try
            {
                ViewBag.Error = "";
                if (roleId < 1)
                {
                    ViewBag.Error = "Invalid selection";
                    return View(new RoleObj());
                }

                if (!(Session["_portalRoles"] is List<RoleObj> roleList) || roleList.Count < 1)
                {
                    ViewBag.Error = "Error Occurred! Unable to prepare this portal role information for modification";
                    return View(new RoleObj());
                }

                var thisRole = roleList.Find(m => m.RoleId == roleId);
                if (thisRole == null || thisRole.RoleId < 1)
                {
                    ViewBag.Error = "Error Occurred! Unable to prepare this portal role information for modification";
                    return View(new RoleObj());
                }

                thisRole.StatusVal = thisRole.Status == 1;

                return View(thisRole);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error Occurred! Please try again later";
                UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                return View(new RoleObj());
            }
        }

        public JsonResult ProcessRoleUpdateRequest(int myRoleId, string myRoleName, bool myRoleStatus)
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

                if (myRoleId < 1)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Invalid Role Information" });
                }

                if (string.IsNullOrEmpty(myRoleName) || myRoleName.Length < 2)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Invalid Role Name" });
                }


                var passObj = new EditRoleObj
                {
                    RoleId = myRoleId,
                    Name = myRoleName,
                    Status = myRoleStatus ? 1 : 0,
                    AdminUserId = userData.UserId,
                };


                var response = new PortalRoleManager().ModifyRole(passObj, userData.Username);
                if (response?.Status == null)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Error Occurred! Please try again later" });
                }
                

                if (!response.Status.IsSuccessful)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = string.IsNullOrEmpty(response.Status.Message.FriendlyMessage) ? "Process Failed! Unable to modify role" : response.Status.Message.FriendlyMessage });
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

        public JsonResult ProcessDeleteRequest(int myRoleId)
        {
            try
            {
                var userData = MvcApplication.GetUserData(User.Identity.Name) ?? new UserData();

                if (userData.UserId < 1)
                {
                    return Json(new { IsSuccessful = false, Error = "Your session has expired", IsAuthenticated = false });
                }
                
                if (myRoleId < 1)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Invalid Role Information" });
                }
                
                var passObj = new DeleteRoleObj
                {
                    RoleId = myRoleId,
                    AdminUserId = userData.UserId,
                }; 

                var response = new PortalRoleManager().DeleteRole(passObj, userData.Username);
                if (response?.Status == null)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Error Occurred! Please try again later" });
                }

                if (!response.Status.IsSuccessful)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = string.IsNullOrEmpty(response.Status.Message.FriendlyMessage) ? "Process Failed! Unable to modify role" : response.Status.Message.FriendlyMessage });
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