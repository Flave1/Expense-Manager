using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using PluglexHelper.CoreService;
using PluglexHelper.PortalManager;
using PluglexHelper.PortalObjs;
using GlobalLitePortal.PortalCore.Model;
using GlobalLitePortalHelper.APIObjs;
using GlobalLitePortalHelper.APIServices;
using XPLUG.WEBTOOLKIT;

namespace GlobalLitePortal.PortalCore
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public sealed class PortalChangeAccessAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            filterContext.Controller.ViewBag.IsSuccessful = false;
            filterContext.Controller.ViewBag.Error = "";
            
            var modelList = filterContext.ActionParameters.Where(ap => ap.Key == "model").ToList();
            if (modelList.IsNullOrEmpty())
            {
               filterContext.Controller.ViewBag.Error = "Invalid Password Information";
                return;
            }
            if (!modelList.Any() || modelList.Count != 1)
            {
                filterContext.Controller.ViewBag.Error = "Invalid Password Information";
                return;
            }

            if (!(modelList[0].Value is ChangePasswordContract model))
            {
                filterContext.Controller.ViewBag.Error = "Invalid Password Information";
                return;
            }

            if (!GenericVal.Validate(model, out var msg))
            {
                filterContext.Controller.ViewBag.Error = msg;
                return;
            }

            if (
              string.Compare(model.OldPassword.Trim(), model.NewPassword.Trim(),
                  StringComparison.InvariantCultureIgnoreCase) == 0)
            {
                model.ConfirmPassword = "";
                model.NewPassword = "";
                model.OldPassword = "";
                filterContext.Controller.ViewBag.Error = "Current Password and New Password cannot be same";
                return;
            }

            if (
                string.Compare(model.ConfirmPassword.Trim(), model.NewPassword.Trim(),
                    StringComparison.InvariantCultureIgnoreCase) != 0)
            {
                model.ConfirmPassword = "";
                model.NewPassword = "";
                model.OldPassword = "";
                filterContext.Controller.ViewBag.Error = "New Password and Confirm New Password must match";
                return;
            }

            var passObj = new ChangePasswordObj
            {
                NewPassword = model.NewPassword,
                OldPassword = model.OldPassword,
                UserId = model.UserId,
                ChangeType = (int) PasswordChangeType.Regular
            };

            var changePassword = new PortalUserManager().ChangePassword(passObj, model.Username);
            if (changePassword == null)
            {
                filterContext.Controller.ViewBag.Error = "Process Failed! Unable to change password";
                return;
            }
            if (!changePassword.Status.IsSuccessful)
            {
               filterContext.Controller.ViewBag.Error = string.IsNullOrEmpty(changePassword.Status.Message.FriendlyMessage) ? "Process Failed! Unable to change your password" : changePassword.Status.Message.FriendlyMessage;
                return;
            }


            filterContext.Controller.ViewBag.IsSuccessful = true;
            base.OnActionExecuting(filterContext);
        }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public sealed class PortalChangeFirstAccessAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            filterContext.Controller.ViewBag.IsSuccessful = false;
            filterContext.Controller.ViewBag.Error = "";

            var modelList = filterContext.ActionParameters.Where(ap => ap.Key == "model").ToList();
            if (modelList.IsNullOrEmpty())
            {
                filterContext.Controller.ViewBag.Error = "Invalid update information";
                return;
            }
            if (!modelList.Any() || modelList.Count != 1)
            {
                filterContext.Controller.ViewBag.Error = "Invalid update information";
                return;
            }

            if (!(modelList[0].Value is ChangePasswordContract model))
            {
                filterContext.Controller.ViewBag.Error = "Invalid update information";
                return;
            }

            if (!GenericVal.Validate(model, out var msg))
            {
                filterContext.Controller.ViewBag.Error = msg;
                return;
            }


            if (string.Compare(model.OldPassword.Trim(), model.NewPassword.Trim(), StringComparison.InvariantCultureIgnoreCase) == 0)
            {
                model.ConfirmPassword = "";
                model.NewPassword = "";
                model.OldPassword = "";
                filterContext.Controller.ViewBag.Error = "Old Password and New Password and must be different";
                return;
            }

            if (string.Compare(model.ConfirmPassword.Trim(), model.NewPassword.Trim(), StringComparison.InvariantCultureIgnoreCase) != 0)
            {
                model.ConfirmPassword = "";
                model.NewPassword = "";
                model.OldPassword = "";
                filterContext.Controller.ViewBag.Error = "New Password and Confirm New Password must match";
                return;
            }
            
            var passObj = new ChangePasswordObj
            {
                NewPassword = model.NewPassword,
                OldPassword = model.OldPassword,
                UserId = model.UserId,
                ChangeType = (int)PasswordChangeType.First_Time
            };

            var changePassword = new PortalUserManager().ChangeFirstTimePassword(passObj, model.Username);
            if (changePassword == null)
            {
                filterContext.Controller.ViewBag.Error = "Process Failed! Unable to change password";
                return;
            }
            if (!changePassword.Status.IsSuccessful)
            {
                filterContext.Controller.ViewBag.Error = string.IsNullOrEmpty(changePassword.Status.Message.FriendlyMessage) ? "Process Failed! Unable to update password" : changePassword.Status.Message.FriendlyMessage;
                return;
            }


            filterContext.Controller.ViewBag.IsSuccessful = true;
            base.OnActionExecuting(filterContext);
        }

    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public sealed class PortalResetAccessAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            filterContext.Controller.ViewBag.IsSuccessful = false;
            filterContext.Controller.ViewBag.Error = "";
           
            var modelList = filterContext.ActionParameters.Where(ap => ap.Key == "model").ToList();
            if (modelList.IsNullOrEmpty())
            {
                filterContext.Controller.ViewBag.Error = "Invalid update information";
                return;
            }
            if (!modelList.Any() || modelList.Count != 1)
            {
                filterContext.Controller.ViewBag.Error = "Invalid update information";
                return;
            }

            if (!(modelList[0].Value is ResetPasswordContract model))
            {
                filterContext.Controller.ViewBag.Error = "Invalid update information";
                return;
            }
            if (!GenericVal.Validate(model, out var msg))
            {
                filterContext.Controller.ViewBag.Error = msg;
                return;
            }

            var contract = new ResetPasswordObj
            {
                AdminUserId = 1,
                Email = model.Username,
            };

            var passReset = new PortalUserManager().ResetPassword(contract, model.Username);
            if (passReset == null)
            {
                filterContext.Controller.ViewBag.Error = "Process Failed! Unable to reset password";
                return;
            }
            if (!passReset.Status.IsSuccessful)
            {
                filterContext.Controller.ViewBag.Error = string.IsNullOrEmpty(passReset.Status.Message.FriendlyMessage) ? "Process Failed! Unable to update password" : passReset.Status.Message.FriendlyMessage;
                return;
            }

            filterContext.Controller.ViewBag.IsSuccessful = true;
            filterContext.Controller.ViewBag.ThisNewPassword = passReset.NewPassword;
            base.OnActionExecuting(filterContext);
        }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public sealed class PortalUnlockAccessAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            filterContext.Controller.ViewBag.IsSuccessful = false;
          
            var modelList = filterContext.ActionParameters.Where(ap => ap.Key == "model").ToList();
            if (modelList.IsNullOrEmpty())
            {
                filterContext.Controller.ViewBag.Error = "Invalid update information";
                return;
            }
            if (!modelList.Any() || modelList.Count != 1)
            {
                filterContext.Controller.ViewBag.Error = "Invalid update information";
                return;
            }

            if (!(modelList[0].Value is ResetPasswordContract model))
            {
                filterContext.Controller.ViewBag.Error = "Invalid update information";
                return;
            }

            if (!GenericVal.Validate(model, out var msg))
            {
                filterContext.Controller.ViewBag.Error = msg;
                return;
            }

            var contract = new ControlUserObj
            {
                AdminUserId = 1,
                Email = model.Username,
                ControlType = (int)UserControlType.Activate
            };

            var unlock = new PortalUserManager().UnLockUser(contract, model.Username);
            if (unlock == null)
            {
                filterContext.Controller.ViewBag.Error = "Process Failed! Unable to unlock user";
                return;
            }
            if (!unlock.Status.IsSuccessful)
            {
                filterContext.Controller.ViewBag.Error = string.IsNullOrEmpty(unlock.Status.Message.FriendlyMessage) ? "Process Failed! Unable to update user's account" : unlock.Status.Message.FriendlyMessage;
                return;
            }

            filterContext.Controller.ViewBag.IsSuccessful = true;
            base.OnActionExecuting(filterContext);
        }
    }


    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public sealed class PortalLockAccessAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            filterContext.Controller.ViewBag.IsSuccessful = false;
           
            var modelList = filterContext.ActionParameters.Where(ap => ap.Key == "model").ToList();
            if (modelList.IsNullOrEmpty())
            {
                filterContext.Controller.ViewBag.Error = "Invalid update information";
                return;
            }
            if (!modelList.Any() || modelList.Count != 1)
            {
               filterContext.Controller.ViewBag.Error = "Invalid update information";
                return;
            }

            if (!(modelList[0].Value is ResetPasswordContract model))
            {
                filterContext.Controller.ViewBag.Error = "Invalid update information";
                return;
            }

            if (!GenericVal.Validate(model, out var msg))
            {
                filterContext.Controller.ViewBag.Error = msg;
                return;
            }

            var contract = new ControlUserObj
            {
                AdminUserId = 1,
                Email = model.Username,
                ControlType = (int)UserControlType.Locked_Out
            };

            var lockUser = new PortalUserManager().LockUser(contract, model.Username);
            if (lockUser == null)
            {
                filterContext.Controller.ViewBag.Error = "Process Failed! Unable to unlock account";
                return;
            }
            if (!lockUser.Status.IsSuccessful)
            {
                filterContext.Controller.ViewBag.Error = string.IsNullOrEmpty(lockUser.Status.Message.FriendlyMessage) ? "Process Failed! Unable to update user's account" : lockUser.Status.Message.FriendlyMessage;
                return;
            }

            filterContext.Controller.ViewBag.IsSuccessful = true;
            base.OnActionExecuting(filterContext);
        }
    }


    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public sealed class SecurityAccessAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                filterContext.Controller.ViewBag.UserAuthInfo = null;
                base.OnActionExecuting(filterContext);
                return;
            }

            var frmId = (FormsIdentity)filterContext.HttpContext.User.Identity;
            var usData = frmId.Ticket.UserData;
            if (string.IsNullOrEmpty(usData))
            {
                filterContext.Controller.ViewBag.UserAuthInfo = null;
                base.OnActionExecuting(filterContext);
                return;
            }

            var userDataSplit = usData.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            if (!userDataSplit.Any() || userDataSplit.Length != 3)
            {
                filterContext.Controller.ViewBag.UserAuthInfo = null;
                base.OnActionExecuting(filterContext);
                return;
            }

            if (!userDataSplit[0].IsNumeric() || !userDataSplit[1].IsNumeric())
            {
                filterContext.Controller.ViewBag.UserAuthInfo = null;
                base.OnActionExecuting(filterContext);
                return;
            }

            var roles = userDataSplit[2].Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

            var identity = new FormsIdentity(frmId.Ticket);
            var principal = new PlugPortalPrincipal(identity, roles);

            var userData = new UserData
            {
                UserId = int.Parse(userDataSplit[0].Trim()),
                Username = frmId.Name,
                Email = frmId.Name,
                Roles = roles,
                UserType = (UserType)int.Parse(userDataSplit[1].Trim()),
            };

            if (!MvcApplication.SetUserData(userData))
            {
                filterContext.Controller.ViewBag.UserAuthInfo = null;
                base.OnActionExecuting(filterContext);
                return;
            }


            filterContext.Controller.ViewBag.UserAuthInfo = userData;
            filterContext.HttpContext.User = principal;
            base.OnActionExecuting(filterContext);
        }

    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public sealed class PortalAuthenticateAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            filterContext.Controller.ViewBag.UserINFOCode = null;
            filterContext.Controller.ViewBag.FirstLogin = null;
            filterContext.Controller.ViewBag.EmailConfirmed = null;
            filterContext.Controller.ViewBag.Error = "";
           

            var modelList = filterContext.ActionParameters.Where(ap => ap.Key == "model").ToList();
            if (modelList.IsNullOrEmpty() || !modelList.Any() || modelList.Count != 1)
            {
                filterContext.Controller.ViewBag.Error = "Invalid Login Information";
                return;
            }

            if (!(modelList[0].Value is UserLoginContract model))
            {
                filterContext.Controller.ViewBag.Error = "Invalid Login Information";
                return;
            }

            if (!GenericVal.Validate(model, out var msg))
            {
                filterContext.Controller.ViewBag.Error = msg;
                return;
            }

            if (string.IsNullOrEmpty(model.Username) || string.IsNullOrEmpty(model.Password) || model.Password.Length < 2)
            {
                model.Password = "";
                filterContext.ActionParameters["model"] = model;
                filterContext.Controller.ViewBag.Error = "Empty / Invalid username or password or password length";
                return;
            }

           
            UserLoginRespObj user;
            try
            {
                var loginObj = new UserLoginObj
                {
                    Password = model.Password,
                    Email = model.Username,
                    LoginChannel = (int) LoginChannel.Web,
                    SourceAddress = "192.168.17.25",
                };

                user = new PortalUserManager().Login(loginObj);
                if (user == null)
                {
                    model.Password = "";
                    filterContext.ActionParameters["model"] = model;
                    filterContext.Controller.ViewBag.Error = "Login Failed! Please try again later";
                    return;
                }

                if (!user.Status.IsSuccessful)
                {
                    model.Password = "";
                    filterContext.ActionParameters["model"] = model;
                    filterContext.Controller.ViewBag.Error = string.IsNullOrEmpty(user.Status.Message.FriendlyMessage)
                        ? "Login Failed!"
                        : user.Status.Message.FriendlyMessage;
                    if (!string.IsNullOrEmpty(user.Status.Message.MessageId) &&
                        user.Status.Message.MessageId.Length > 3)
                    {
                        if (user.Status.Message.MessageId == "1001")
                        {
                            filterContext.Controller.ViewBag.EmailConfirmed = false;
                        }
                    }
                    return;
                }

                if (user.UserItem == null || user.UserItem.UserId < 1)
                {
                    model.Password = "";
                    filterContext.ActionParameters["model"] = model;
                    filterContext.Controller.ViewBag.Error = string.IsNullOrEmpty(user.Status.Message.FriendlyMessage)
                        ? "Login Failed!"
                        : user.Status.Message.FriendlyMessage;
                    return;
                }
            }
            catch (Exception ex)
            {
                UtilTools.LogE(ex.StackTrace, ex.Source, ex.GetBaseException().Message);
                model.Password = "";
                filterContext.ActionParameters["model"] = model;
                filterContext.Controller.ViewBag.Error = "Error Occurred! Unable to complete your request";
                return;
            }

            //Check Multiple Login
            //Log user Out of previous login
            //Create new login
            var code = model.Username.Trim() + model.Password.Trim();

            if (MvcApplication.IsUserAlreadyLoggedIn(code, out _))
            {
                MvcApplication.ResetLogin(code);
                MvcApplication.ResetUserData(model.Username.Trim());
                filterContext.HttpContext.Session["UserINFO"] = null;
                new FormsAuthenticationService().SignOut();
            }

            var userId = user.UserItem.UserId;

            var clientProdList =
                ClientService.LoadClientProductList(new ClientProdSearchObj { AdminUserId = userId, UserId = userId },
                    model.Username.Trim()) ?? new ClientProdsRespObj();

            var userData = new UserData
            {
                UserId = userId,
                Username = user.UserItem.Email,
                Email = user.UserItem.Email,
                Roles = user.UserItem.RoleNames.ToArray(),
                UserType = (UserType)user.UserItem.UserType,
                ClientProductList = clientProdList.ClientProductList ?? new List<ClientProductInfo>() //
            };

            if (!MvcApplication.SetPortalTabData(user.UserItem.TabItems, user.UserItem.Email))
            {
                model.Password = "";
                filterContext.ActionParameters["model"] = model;
                filterContext.Controller.ViewBag.Error = "Invalid authentication!";
                return;
            }

            if (!MvcApplication.SetUserData(userData))
            {
                model.Password = "";
                filterContext.ActionParameters["model"] = model;
                filterContext.Controller.ViewBag.Error = "Invalid authentication!";
                return;
            }

            var ticketData = userId + "|" + user.UserItem.UserType + "|" + string.Join(";", user.UserItem.RoleNames.ToArray());
            var encTicket = new FormsAuthenticationService().SignIn(model.Username, false, ticketData);
            if (string.IsNullOrEmpty(encTicket))
            {
                model.Password = "";
                filterContext.ActionParameters["model"] = model;
                filterContext.Controller.ViewBag.Error = "Invalid authentication!";
                return;
            }

            filterContext.HttpContext.Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encTicket));
            filterContext.Controller.ViewBag.UserINFOCode = code.GetHashCode().ToString(CultureInfo.InvariantCulture);
            filterContext.Controller.ViewBag.FirstLogin = user.UserItem.IsFirstTimeLogin;
            filterContext.Controller.ViewBag.EmailConfirmed = user.UserItem.IsEmailConfirmed;
            filterContext.Controller.ViewBag.LoginDataItem = userData;
            base.OnActionExecuting(filterContext);
        }

    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public sealed class PortalRegisterAccessAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!filterContext.HttpContext.Request.IsAjaxRequest())
            { return; }
            filterContext.HttpContext.Response.StatusCode = 600;
            filterContext.Controller.ViewBag.ValidAuthourized = "0";
            
            var modelList = filterContext.ActionParameters.Where(ap => ap.Key == "model").ToList();
            if (modelList.IsNullOrEmpty())
            {
                filterContext.HttpContext.Response.AppendHeader("message", "Invalid Registration Information");
                return;
            }
            if (!modelList.Any() || modelList.Count != 1)
            {
                filterContext.HttpContext.Response.AppendHeader("message", "Invalid Registration Information");
                return;
            }

            if (!(modelList[0].Value is PortalUserContract model))
            {
                filterContext.HttpContext.Response.AppendHeader("message", "Invalid Registration Information");
                return;
            }

            if (!GenericVal.Validate(model, out var msg))
            {
                filterContext.Controller.ViewBag.Error = msg;
                return;
            }

            if (string.IsNullOrEmpty(model.UserName) || string.IsNullOrEmpty(model.Password) || string.IsNullOrEmpty(model.FirstName) || string.IsNullOrEmpty(model.LastName))
            {
                filterContext.HttpContext.Response.AppendHeader("message", "Invalid Registration Information");
                return;
            }


            //string msg;
            //var retVal = ProfileService.RegisterNewUser(model, out msg);
            //if (!retVal)
            //{
            //    filterContext.HttpContext.Response.AppendHeader("message", msg.Length > 0 ? msg : "Invalid Registration Information");
            //    return;
            //}
            filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.OK;
            filterContext.HttpContext.Response.AppendHeader("", "");
            filterContext.Controller.ViewBag.ValidAuthourized = "1";
            base.OnActionExecuting(filterContext);
        }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public sealed class PortalAuthorizeAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            try
            {
                if (httpContext == null)
                {
                    throw new ArgumentNullException(nameof(httpContext));
                }

                var user = httpContext.User;
                if (!user.Identity.IsAuthenticated)
                {
                    return false;
                }

                var usersSplit = Users.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                var rolesSplit = Roles.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);

                if (usersSplit.Length > 0 && !usersSplit.Contains(user.Identity.Name, StringComparer.OrdinalIgnoreCase))
                {
                    return false;
                }

                if (rolesSplit.Length > 0 && !rolesSplit.Any(user.IsInRole))
                {
                    return false;
                }

                if (MvcApplication.GetUserData(user.Identity.Name) == null)
                {
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                return false;
            }

        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);

            if (filterContext.HttpContext.User.Identity.IsAuthenticated && filterContext.Result is HttpUnauthorizedResult)
            {
                var values = new RouteValueDictionary(new
                {
                    action = "Index",
                    controller = "SignOut"
                });
                filterContext.Result = new RedirectToRouteResult(values);
            }
        }

    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public sealed class PlugPortalParamActionAttribute : ActionNameSelectorAttribute
    {
        public override bool IsValidName(ControllerContext controllerContext, string actionName, MethodInfo methodInfo)
        {
            if (actionName.Equals(methodInfo.Name, StringComparison.InvariantCultureIgnoreCase))
                return true;

            var request = controllerContext.RequestContext.HttpContext.Request;
            var myItem = request[methodInfo.Name];
            return myItem != null;
        }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public sealed class NoCacheAttribute : ActionFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            filterContext.HttpContext.Response.Cache.SetExpires(DateTime.UtcNow.AddDays(-1));
            filterContext.HttpContext.Response.Cache.SetValidUntilExpires(false);
            filterContext.HttpContext.Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
            filterContext.HttpContext.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            filterContext.HttpContext.Response.Cache.SetNoStore();

            base.OnResultExecuting(filterContext);
        }
    }

    
}