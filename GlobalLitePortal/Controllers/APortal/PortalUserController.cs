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
    public class PortalUserController : Controller
    {
        [PortalAuthorize(Roles = "PortalAdmin,SiteAdmin")]
        public ActionResult Index()
        {

            try
            {
                var userData = MvcApplication.GetUserData(User.Identity.Name);
                if (userData == null || userData.UserId < 1)
                {
                    ViewBag.Error = "Session Has Expired! Please Re-Login";
                    return View(new List<UserItemObj>());
                }

                //var bearerAuth = MvcApplication.GetSessionBearerData(userData.Username);
                //var authToken = MvcApplication.GetSessionAuthData(userData.Username);

                var searchObj = new RoleSearchObj
                {
                    AdminUserId = userData.UserId,
                    RoleId = 0,
                    Status = -2,
                };

                var roleVal = new PortalRoleManager().LoadRoles(searchObj, userData.Username);
                if (roleVal?.Status == null)
                {
                    ViewBag.Error = "Role Item list is empty! Roles must be defined before setting up users";
                    return View(new List<UserItemObj>());
                }

                

                if (!roleVal.Status.IsSuccessful)
                {
                    ViewBag.Error = string.IsNullOrEmpty(roleVal.Status.Message.FriendlyMessage)
                        ? "Role Item list is empty! Roles must be defined before setting up users"
                        : roleVal.Status.Message.FriendlyMessage;
                    return View(new List<UserItemObj>());
                }

                if (roleVal.Roles == null)
                {
                    ViewBag.Error = "Role Item list is empty! Roles must be defined before setting up users";
                    return View(new List<UserItemObj>());
                }

                var roles = roleVal.Roles.Where(m => m.Status == 1).ToList();
                if (!roles.Any())
                {
                    ViewBag.Error = "Role Item list is empty! Roles must be defined before setting up users";
                    return View(new List<UserItemObj>());
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

                Session["_portal_user_allroles"] = allRoles;

                var searchObj2 = new UserSearchObj
                {
                    AdminUserId = userData.UserId,
                    Status = -2,
                    StopDate = "",
                    StartDate = "",
                    UserId = 0,
                    
                };

                var retVal = new PortalUserManager().LoadUsers(searchObj2, userData.Username);
                if (retVal?.Status == null)
                {
                    ViewBag.Error = "Portal User list is empty!";
                    return View(new List<UserItemObj>());
                }

                if (!retVal.Status.IsSuccessful)
                {
                    ViewBag.Error = string.IsNullOrEmpty(retVal.Status.Message.FriendlyMessage)
                        ? "Portal User list is empty!"
                        : retVal.Status.Message.FriendlyMessage;
                    return View(new List<UserItemObj>());
                }
                if (!retVal.Users.Any())
                {
                    ViewBag.Error = "Portal User list is empty!";
                    return View(new List<UserItemObj>());
                }

                var allUsers = retVal.Users.OrderBy(m => m.UserId).ToList();
                Session["_portalUsers"] = allUsers;
                return View(allUsers);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                return View(new List<UserItemObj>());
            }
        }


        #region Add User
        public ActionResult _AddUser()
        {
            try
            {
                ViewBag.Error = "";
                var userModel = new PortalUserViewModel { AllRoles = new List<NameValueObject>(), MyRoleIds = new[] { 0 } };

                var userData = MvcApplication.GetUserData(User.Identity.Name);
                if (userData == null || userData.UserId < 1)
                {
                    ViewBag.Error = "Session Has Expired! Please Re-Login";
                    return View(userModel);
                }

                if (!(Session["_portal_user_allroles"] is List<NameValueObject> allRoles) || !allRoles.Any())
                {
                    ViewBag.Error = "Session Has Expired! Please Re-Login";
                    return View(userModel);
                }

                userModel.AllRoles = allRoles;
                //Use this to check it its admin user
                userModel.UserId = userData.UserId == 1 ? 1 : 0;
                return View(userModel);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error Occurred! Please try again later";
                UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                return View(new PortalUserViewModel());
            }
        }

        public JsonResult ProcessAddUserRequest(PortalUserViewModel portalUser)
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

                if (portalUser == null)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Invalid Tab Item" });
                }

                if (string.IsNullOrEmpty(portalUser.FirstName))
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "First Name is required" });
                }

                if (string.IsNullOrEmpty(portalUser.Email))
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Email is required" });
                }

                if (string.IsNullOrEmpty(portalUser.Password) || portalUser.Password.Length < 3)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Password is required" });
                }

                if (string.IsNullOrEmpty(portalUser.ConfirmPassword) || portalUser.ConfirmPassword.Length < 3)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Password Confirmation is required" });
                }

                if (string.Compare(portalUser.ConfirmPassword, portalUser.Password, StringComparison.CurrentCultureIgnoreCase) != 0)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Password and Password Confirmation must be equal" });
                }

                if (portalUser.MyRoleIds == null || !portalUser.MyRoleIds.Any())
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Tab Roles are required" });
                }

                if (!(Session["_portal_user_allroles"] is List<NameValueObject> allRoles) || !allRoles.Any())
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Your session has expired" });
                }

                // var selRoles = allRoles.Where(m => portalUser.MyRoleIds.Contains(m.Id)).Select(m => m.Name).ToList();

                var passObj = new RegUserObj
                {
                    FirstName = portalUser.FirstName,
                    LastName = portalUser.LastName,
                    Password = portalUser.Password,
                    Email = portalUser.Email,
                    MobileNumber = portalUser.MobileNumber,
                    AdminUserId = userData.UserId,
                    UserType = 1,
                 };


                var response = new PortalUserManager().AddUser(passObj, userData.Username);
                if (response == null)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Error Occurred! Please try again later" });
                }

           

                if (!response.Status.IsSuccessful)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = string.IsNullOrEmpty(response.Status.Message.FriendlyMessage) ? "Process Failed! Unable to add user" : response.Status.Message.FriendlyMessage });
                }
                

                if (response.UserId > 0)
                {
                    var userRole = new RegUserRoleObj
                    {
                        UserId = response.UserId,
                        RoleIds = string.Join(",", portalUser.MyRoleIds),
                        AdminUserId = userData.UserId,
                    };

                   var  response2 = new PortalRoleManager().AdUserToRole(userRole, userData.Username);
                   

                    if (!response2.Status.IsSuccessful)
                    {
                        return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = string.IsNullOrEmpty(response.Status.Message.FriendlyMessage) ? "User information was added but user roles not added" : "User information was added but role Error: " + response.Status.Message.FriendlyMessage + " occured!" });
                    }
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

        #region Edit User
        public ActionResult _EditUser(int userId)
        {
            try
            {
                var userModel = new PortalUserViewModel { AllRoles = new List<NameValueObject>(), MyRoleIds = new[] { 0 } };
                ViewBag.Error = "";
                if (userId < 1)
                {
                    ViewBag.Error = "Invalid selection";
                    return View(userModel);
                }

                if (!(Session["_portalUsers"] is List<UserItemObj> userList) || userList.Count < 1)
                {
                    ViewBag.Error = "Error Occurred! Unable to prepare this portal tab information for modification";
                    return View(userModel);
                }

                var thisUser = userList.Find(m => m.UserId == userId);
                if (thisUser == null || thisUser.UserId < 1)
                {
                    ViewBag.Error = "Error Occurred! Unable to prepare this portal user information for modification";
                    return View(userModel);
                }

                var userData = MvcApplication.GetUserData(User.Identity.Name);
                if (userData == null || userData.UserId < 1)
                {
                    ViewBag.Error = "Session Has Expired! Please Re-Login";
                    return View(userModel);
                }

                if (!(Session["_portal_user_allroles"] is List<NameValueObject> allRoles) || !allRoles.Any())
                {
                    ViewBag.Error = "Session Has Expired! Please Re-Login";
                    return View(userModel);
                }

                userModel.AllRoles = allRoles;
                var rolesId = new List<int>();
                var splitedRoles = thisUser.RoleNames; //.Select(m => m.Name).ToArray();
                foreach (var item in allRoles)
                {
                    if (splitedRoles.Contains(item.Name))
                    {
                        rolesId.Add(item.Id);
                    }
                }

                userModel.Username = thisUser.Email;
                userModel.UserId = thisUser.UserId;
                userModel.Email = thisUser.Email;
                userModel.FirstName = thisUser.FirstName;
                userModel.LastName = thisUser.LastName;
                userModel.MobileNumber = thisUser.MobileNumber;
                userModel.IsApproved = ((UserStatus) thisUser.Status) == UserStatus.Active;
                userModel.MyRoleIds = rolesId.ToArray();

                //Use this to check it its admin user
                userModel.UserId = userData.UserId == 1 ? 1 : 0;


                return View(userModel);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error Occurred! Please try again later";
                UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                return View(new PortalUserViewModel());
            }
        }

        public JsonResult ProcessEditUserRequest(PortalUserViewModel portalUser)
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

                if (portalUser == null)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Invalid User Item" });
                }

                if (string.IsNullOrEmpty(portalUser.FirstName) || portalUser.FirstName.Length < 2)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "First Name is required" });
                }

                if (string.IsNullOrEmpty(portalUser.LastName) || portalUser.LastName.Length < 2)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Last Name is required" });
                }

                if (string.IsNullOrEmpty(portalUser.Email) || portalUser.Email.Length < 5)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Email is required" });
                }


                var passObj = new EditUserObj
                {
                    UserId = portalUser.UserId,
                    FirstName = portalUser.FirstName,
                    LastName = portalUser.LastName,
                    AdminUserId = userData.UserId,
                    MobileNumber = portalUser.MobileNumber,
                };


                var response = new PortalUserManager().ModifyUser(passObj, userData.Username);
                if (response == null)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Error Occurred! Please try again later" });
                }

             
                if (!response.Status.IsSuccessful)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = string.IsNullOrEmpty(response.Status.Message.FriendlyMessage) ? "Process Failed! Unable to add role" : response.Status.Message.FriendlyMessage });
                }

                var updateRole = new EditUserRoleObj
                {
                    AdminUserId = userData.UserId,
                    UserId = portalUser.UserId,
                    RoleIds = string.Join(",", portalUser.MyRoleIds)
                };

               var response2 = new PortalRoleManager().UpdateUserRoles(updateRole, userData.Username);
                if (response2 == null)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Error Occurred! Please try again later" });
                }

         
                if (!response2.Status.IsSuccessful)
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
    }
}