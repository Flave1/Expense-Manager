using System;
using System.Linq;
using System.Web.Mvc;
using PluglexHelper.PortalManager;
using PluglexHelper.PortalObjs;
using GlobalLitePortal.PortalCore;
using GlobalLitePortal.PortalCore.Model;
using XPLUG.WEBTOOLKIT;
using GlobalLitePortalHelper.APIObjs.Business;

namespace GlobalLitePortal.Controllers.Core.Portal
{
    public class PortalController : Controller
    {
      
      
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.Error = "";
            Session["_UserClientSession_"] = null;
            ////AppSession appSession = new AppSession 
            ////{ 
            ////    ProductItemId = 0,
            ////    ProductId = 0,
            ////    ClientId = 0,
            ////};
            ////Session["_UserClientSession_"] = appSession; 
            Session.Abandon();

            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [PortalAuthenticate]
        public ActionResult Login(string returnUrl, UserLoginContract model)
        {
            try
            {
                var userCode = ViewBag.UserINFOCode as string;
                var firstLogin = ViewBag.FirstLogin;
                var emailConfirmed = ViewBag.EmailConfirmed;
                var userData = ViewBag.LoginDataItem as UserData;
                
                if (!emailConfirmed)
                {
                    Session["thisUserEmail"] = model.Username.Trim();
                    return RedirectToAction("EmailConfirmMessage");
                }

                if (string.IsNullOrEmpty(userCode))
                {
                    return View(model);
                }
                if (userData == null || userData.UserId < 1)
                {
                    return View(model);
                }

                Session["UserINFO"] = userCode;
                Session["UserDATAINFO"] = userData.Username;
                
                if (firstLogin)
                {
                    ViewBag.MyUserName = model.Username.Trim();
                    return RedirectToAction("ChangeFirstTimePassword");
                }
                return RedirectToLocal(userData.Roles, returnUrl);

            }
            catch (Exception ex)
            {
                UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                ModelState.AddModelError("", ex.Message);
                return View(model);
            } 
        }

        private ActionResult RedirectToLocal(string[] roles, string returnUrl)
        {
            if (roles == null || !roles.Any())
            {
                return RedirectToAction("Login", "Portal");
            }
            if (Url.IsLocalUrl(returnUrl))
            {
                return RedirectPermanent(returnUrl);
            }
            return RedirectToActionPermanent("Index", "Dashboard");
        }

        #region Password
            [PortalAuthorize]
            public ActionResult ChangeFirstTimePassword()
            {
                ViewBag.Error = "";
                var userData = MvcApplication.GetUserData(User.Identity.Name) ?? new UserData();
                var model = new ChangePasswordContract { Username = User.Identity.Name, UserId = userData .UserId};
                return View(model);
            }

            [HttpPost]
            [PortalChangeFirstAccess]
            public ActionResult ChangeFirstTimePassword(ChangePasswordContract model)
            {
                var retVal = ViewBag.IsSuccessful;
                if (!retVal)
                {
                    return View(model);
                }

                return RedirectToActionPermanent("Index", "Dashboard");

            }

            [PortalAuthorize]
            public ActionResult ChangeUserPassword()
            {
                try
                {
                    ViewBag.Error = "";
                    var userData = MvcApplication.GetUserData(User.Identity.Name);
                    if (userData == null || userData.UserId < 1)
                    {
                        return RedirectToAction("Login", new { returnUrl = "" });
                    }
                    var model = new ChangePasswordContract { Username = User.Identity.Name, UserId = userData.UserId };
                    return View(model);
               
                }
                catch (Exception ex)
                {
                    ViewBag.Error = "Error Occurred! Please try again later";
                    UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                    return View(new ChangePasswordContract());
                }
            }
        
            [PortalAuthorize(Roles = "PortalAdmin,SiteAdmin")]
            public ActionResult ResetPassword()
            {
                return View(new ResetPasswordContract());
            }
        #endregion

        #region Account Profile
           
            [PortalAuthorize]
            public ActionResult MyProfile()
            {
                try
                {
                    ViewBag.Error = "";
                    var userData = MvcApplication.GetUserData(User.Identity.Name);
                    if (userData == null || userData.UserId < 1)
                    {
                        ModelState.AddModelError("", "Invalid Profile Session");
                        return View();
                    }

                    //var bearerAuth = MvcApplication.GetSessionBearerData(userData.Username);
                   
                    var searchObj = new UserSearchObj
                    {
                       AdminUserId = userData.UserId,
                       UserId = userData.UserId,
                       StartDate = "",
                       Status = -5,
                       StopDate = ""
                    };

                    var matchingProfiles = new PortalUserManager().GetPortalUser(searchObj, userData.Username);
                    if (matchingProfiles?.Status == null || !matchingProfiles.Status.IsSuccessful || matchingProfiles.Users.Count != 1) return View(new UserItemObj());
                    var myProfile = matchingProfiles.Users[0];

                    return View(myProfile);
                }
                catch (Exception ex)
                {
                    UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                    return View(new UserItemObj());
                }

            }
            public JsonResult ProcessProfileUpdate(EditUserObj model)
            {
                try
                {
                    var userData = MvcApplication.GetUserData(User.Identity.Name) ?? new UserData();

                    if (userData.UserId < 1)
                    {
                        return Json(new { IsSuccessful = false, Error = "Your session has expired", IsAuthenticated = false });
                    }
                    
                    if (model.UserId < 1)
                    {
                        return Json(new { IsAuthenticated = true, IsSuccessful = false,  Error = "Invalid Email Address" });
                    }

                    if (string.IsNullOrEmpty(model.FirstName) || model.FirstName.Length < 2)
                    {
                        return Json(new { IsAuthenticated = true, IsSuccessful = false, Error = "First Name must be at least 2 characters" });
                    }

                    if (string.IsNullOrEmpty(model.LastName) || model.LastName.Length < 2)
                    {
                        return Json(new { IsAuthenticated = true, IsSuccessful = false,  Error = "Last Name must be at least 2 characters" });
                    }

                    if (string.IsNullOrEmpty(model.MobileNumber) || model.MobileNumber.Length != 11)
                    {
                        return Json(new { IsAuthenticated = true, IsSuccessful = false, Error = "Invalid Mobile Number" });
                    }

                    model.AdminUserId = userData.UserId;
                    var retVal = new PortalUserManager().ModifyUser(model, userData.Email);
                    if (retVal == null)
                    {
                        return Json(new { IsAuthenticated = true, IsSuccessful = false, Error = "Unable to update your profile at this time" });
                    }

                    if (!retVal.Status.IsSuccessful)
                    {
                      return Json(new { IsAuthenticated = true, IsSuccessful = false, Error = string.IsNullOrEmpty(retVal.Status.Message.FriendlyMessage) ? "Unable to update your profile at this time!" : retVal.Status.Message.FriendlyMessage });
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

        #region Reset Password
        public JsonResult ProcessPasswordResetRequest(string myUsername)
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

                    if (string.IsNullOrEmpty(myUsername) || myUsername.Length < 2)
                    {
                        return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Invalid Username" });
                    }


                    var passObj = new ResetPasswordObj
                    {
                        Email = myUsername,
                        AdminUserId = userData.UserId,
                    };


                    var changePassword = new PortalUserManager().ResetPassword(passObj, userData.Username);
                    if (changePassword?.Status == null)
                    {
                        return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Error Occurred! Please try again later" });
                    }

                    //MvcApplication.SetSessionBearerData(changePassword.Status.CustomSetting, userData.Username);
                    //MvcApplication.SetSessionAuthData(changePassword.Status.CustomToken, userData.Username);

                    if (!changePassword.Status.IsSuccessful)
                    {
                        return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = string.IsNullOrEmpty(changePassword.Status.Message.FriendlyMessage) ? "Process Failed! Unable to change your password" : changePassword.Status.Message.FriendlyMessage });
                    }

                    return Json(new { IsAuthenticated = true, IsSuccessful = true, IsReload = false, Error = changePassword.NewPassword });
                }
                catch (Exception ex)
                {
                    UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Process Error Occurred! Please try again later" });
                }
            }
        #endregion
        #region Change Password
           public JsonResult ProcessPasswordChangeRequest(string myOldPassword, string myNewPassword, string myConfirmPassword)
            {
                try
                {

                    var userData = MvcApplication.GetUserData(User.Identity.Name) ?? new UserData();

                    if (userData.UserId < 1)
                    {
                        return Json(new { IsSuccessful = false, Error = "Your session has expired", IsAuthenticated = false });
                    }
                    if (string.IsNullOrEmpty(myOldPassword) || myOldPassword.Length < 3)
                    {
                        return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Old Password must be at least 3 characters" });
                    }

                    if (string.IsNullOrEmpty(myNewPassword) || myNewPassword.Length < 3)
                    {
                        return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "New Password must be at least 3 characters" });
                    }

                    if (string.IsNullOrEmpty(myConfirmPassword) || myConfirmPassword.Length < 3)
                    {
                        return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Confirm Password must be at least 3 characters" });
                    }

                    if (string.Compare(myOldPassword, myNewPassword, StringComparison.CurrentCultureIgnoreCase) == 0)
                    {
                        return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "New Password must be different from the Old Password" });
                    }
                    if (string.Compare(myConfirmPassword, myNewPassword, StringComparison.CurrentCultureIgnoreCase) != 0)
                    {
                        return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "New Password and Confirm Password must match" });
                    }

                    var passObj = new ChangePasswordObj
                    {
                        NewPassword = myNewPassword,
                        OldPassword = myOldPassword,
                        ChangeType = (int)PasswordChangeType.Regular,
                        UserId = userData.UserId,
                    };


                    var changePassword = new PortalUserManager().ChangePassword(passObj, userData.Username);
                    if (changePassword == null || changePassword.Status == null)
                    {
                        return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Error Occurred! Please try again later" });
                    }

                    if (!changePassword.Status.IsSuccessful)
                    {
                        return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = string.IsNullOrEmpty(changePassword.Status.Message.FriendlyMessage) ? "Process Failed! Unable to change your password" : changePassword.Status.Message.FriendlyMessage });
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
        #region Forgot Password
            [AllowAnonymous]
            public ActionResult ForgotPassword()
            {
                return View();
            }

            [HttpPost]
            public ActionResult ForgotPassword(PasswordRetrievalContract model)
            {
                    ViewBag.Error = "";
                    ViewBag.Success = "";
                    try
                    {
                        if (string.IsNullOrEmpty(model.Email) || model.Email.Length < 5)
                        {
                            ViewBag.Error = "Invalid / Empty Email Address";
                            return View(model);
                        }

                        var passObj = new EmailResendObj
                        {
                            Email = model.Email
                        };

                        var response = new PortalUserManager().SendPasswordResetEmail(passObj, "");
                        if (response?.Status == null)
                        {
                            ViewBag.Error = "Error Occurred! Please try again late";
                            return View(model);
                        }

                        if (!response.Status.IsSuccessful)
                        {
                            ViewBag.Error = string.IsNullOrEmpty(response.Status.Message.FriendlyMessage)
                                ? "Process Failed! Unable to process your request. Please try again later"
                                : response.Status.Message.FriendlyMessage;
                            return View(model);
                        }

                        model.Email = "";
                        ViewBag.Success = "Password Retrieval Email Was Sent";
                        return View(model);
                    }
                    catch (Exception ex)
                    {
                        UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                        return View(model);
                    }

            }
      
            public JsonResult ProcessForgotPasswordRequest(string email)
            {
                try
                {
                    if (string.IsNullOrEmpty(email) || email.Length < 5)
                    {
                        return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Invalid Email Address" });
                    }
                    
                    var passObj = new EmailResendObj
                    {
                       Email = email
                    };
                    
                    var response = new PortalUserManager().SendPasswordResetEmail(passObj, "");
                    if (response?.Status == null)
                    {
                        return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Error Occurred! Please try again later" });
                    }
                    
                    if (!response.Status.IsSuccessful)
                    {
                        return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = string.IsNullOrEmpty(response.Status.Message.FriendlyMessage) ? "Process Failed! Unable to process your request. Please try again later" : response.Status.Message.FriendlyMessage });
                    }

                    return Json(new { IsAuthenticated = true, IsSuccessful = true, IsReload = false, Error = "" });
                }
                catch (Exception ex)
                {
                    UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Process Error Occurred! Please try again later" });
                }
            }
            public ActionResult ResetUserAccess(string userId, string userCode)
            {
                try
                {
                    ViewBag.Error = "";
                    Session["_resetSystem_data"] = null;
                    if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(userCode))
                    {
                        ViewBag.Error = "Invalid Password Reset Link";
                        return View(new ResetUserAccessInfoObj());
                    }

                    userId = userId.CleanEncryptedString();
                    userCode = userCode.CleanEncryptedString();

                    var username = userId.DecryptUrlItem();
                    var code = userCode.DecryptUrlItem();

                    if (string.IsNullOrEmpty(username) || username.Length < 5)
                    {
                        ViewBag.Error = "Invalid Password Reset Link";
                        return View(new ResetUserAccessInfoObj());
                    }

                    if (string.IsNullOrEmpty(code) || code.Length < 5)
                    {
                        ViewBag.Error = "Invalid Password Reset Link";
                        return View(new ResetUserAccessInfoObj());
                    }

                    var confirm = new ResetUserAccessInfoObj
                    {
                        ConfirmationCode = code.Trim(),
                        Email = username,
                        NewPassword = "",
                        ConfirmPassword = ""
                    };
                    Session["_resetSystem_data"] = confirm;
                    return View(confirm);
                }
                catch (Exception ex)
                {
                    ViewBag.Error = "Error Occurred! Please try again later";
                    UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                    return View(new ResetUserAccessInfoObj());
                }
            }
            public JsonResult ProcessPasswordResetRequest(ResetUserAccessInfoObj resetObj)
            {
                try
                {
                    if (resetObj == null || string.IsNullOrEmpty(resetObj.Email) ||
                        string.IsNullOrEmpty(resetObj.ConfirmationCode) || resetObj.Email.Length < 5 ||
                        resetObj.ConfirmationCode.Length < 5)
                    {
                        return Json(new {  IsSuccessful = false,  Error = "Invalid Password Reset Information" });
                    }

                    if (string.IsNullOrEmpty(resetObj.NewPassword) || resetObj.NewPassword.Length < 3)
                    {
                        return Json(new { IsSuccessful = false, Error = "New Password must be at least 3 characters" });
                    }

                    if (string.IsNullOrEmpty(resetObj.ConfirmPassword) || resetObj.ConfirmPassword.Length < 3)
                    {
                        return Json(new {  IsSuccessful = false,  Error = "Confirm Password must be at least 3 characters" });
                    }
                    
                    if (string.Compare(resetObj.NewPassword, resetObj.ConfirmPassword, StringComparison.CurrentCultureIgnoreCase) != 0)
                    {
                        return Json(new {  IsSuccessful = false,  Error = "New Password and Confirm Password must match" });
                    }

                    var passObj = new PasswordResetConfirmObj
                    {
                        Password = resetObj.NewPassword,
                        Email = resetObj.Email,
                        ConfirmationCode = resetObj.ConfirmationCode,
                    };
                
                    var changePassword = new PortalUserManager().ProcessPasswordReset(passObj, "");
                    if (changePassword?.Status == null)
                    {
                        return Json(new {  IsSuccessful = false, Error = "Error Occurred! Please try again later" });
                    }
                    
                    if (!changePassword.Status.IsSuccessful)
                    {
                        return Json(new { IsSuccessful = false,  Error = string.IsNullOrEmpty(changePassword.Status.Message.FriendlyMessage) ? "Process Failed! Unable to reset your password" : changePassword.Status.Message.FriendlyMessage });
                    }

                    return Json(new {  IsSuccessful = true,  Error = "" });
                }
                catch (Exception ex)
                {
                    UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Process Error Occurred! Please try again later" });
                }
            }
        #endregion
        #region Account Confirmation
            public ActionResult EmailConfirmMessage()
            {
                ViewBag.MyUserName = Session["thisUserEmail"] as string;
                ViewBag.Error = "";
                return View();
            }
            [HttpPost]
            public ActionResult EmailConfirmMessage(string thisUsername)
            {
                try
                {
                    ViewBag.RegUsername = "";
                    ViewBag.Error = "";
                    if (string.IsNullOrEmpty(thisUsername) || thisUsername.Length < 5)
                    {
                        return View();
                    }
                    var modelObj = new EmailResendObj
                    {
                        Email = thisUsername
                    };
                    
                    var response = new PortalUserManager().ResendActivationEmail(modelObj, ""); 
                    if (response?.Status == null)
                    {
                        ViewBag.Error =  "Unable to resend email at this time. Please try again later";
                        return View();
                    }
                    if (!response.Status.IsSuccessful)
                    {
                        ViewBag.Error = string.IsNullOrEmpty(response.Status.Message.FriendlyMessage) ? "Unable to resend email at this time. Please try again later" : response.Status.Message.FriendlyMessage;
                        return View();
                    }

                    return View();
                }
                catch (Exception ex)
                {
                    ViewBag.Error = "Unable to resend email at this time. Please try again later";
                    UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                    return View();
                }

            }
            public ActionResult EmailChecker(string userId, string userCode)
            {
                try
                {
                    ViewBag.Error = "";
                    if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(userCode))
                    {
                        ViewBag.Error = "Invalid Activation Link";
                        return View();
                    }

                    userId = userId.CleanEncryptedString();
                    userCode = userCode.CleanEncryptedString();
                
                    var username = userId.DecryptUrlItem();
                    var code = userCode.DecryptUrlItem();

                    if (string.IsNullOrEmpty(username) || username.Length < 5)
                    {
                        ViewBag.Error = "Invalid Activation Link";
                        return View();
                    }

                    if (string.IsNullOrEmpty(code) || code.Length < 5)
                    {
                        ViewBag.Error = "Invalid Activation Link";
                        return View();
                    }

                    var confirm = new EmailConfirmObj
                    {
                        ConfirmationCode = code.Trim(),
                        Email = username
                    };

                    var response = new PortalUserManager().ConfirmUserEmail(confirm, "");
                    if (response?.Status == null)
                    {
                        ViewBag.Error =  "Unable to confirm your email. Please try again later";
                        return View();
                    }
                    if (!response.Status.IsSuccessful)
                    {
                        ViewBag.Error = string.IsNullOrEmpty(response.Status.Message.FriendlyMessage) ? "Unable to confirm your email. Please try again later" : response.Status.Message.FriendlyMessage;
                        return View();
                    }

                    return RedirectToAction("Login");
                }
                catch (Exception ex)
                {
                    ViewBag.Error = "Error Occurred! Please try again later";
                    UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                    return View();
                }
            }
        #endregion



    }
}