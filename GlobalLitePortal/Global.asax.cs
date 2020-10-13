using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using PluglexHelper.CoreService;
using PluglexHelper.PortalObjs;
using GlobalLitePortal.PortalCore.Model;
using XPLUG.WEBTOOLKIT;

namespace GlobalLitePortal
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        void Application_AuthenticateRequest()
        {
            var myCookie = FormsAuthentication.FormsCookieName;
            var myAuthCookie = Context.Request.Cookies[myCookie];

            if (null == myAuthCookie)
            {
                return;
            }

            FormsAuthenticationTicket myAuthTicket;
            try
            {
                myAuthTicket = FormsAuthentication.Decrypt(myAuthCookie.Value);
            }
            catch (Exception ex)
            {
                UtilTools.LogE(ex.StackTrace, ex.Source, ex.GetBaseException().Message);
                return;
            }

            if (null == myAuthTicket)
            {
                return;
            }

            var userDataSplit = myAuthTicket.UserData.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            if (!userDataSplit.Any() || userDataSplit.Length != 3)
            {
                return;
            }

            if (!userDataSplit[0].Trim().IsNumeric() || !userDataSplit[1].Trim().IsNumeric())
            {
                return;
            }

            var roles = userDataSplit[2].Split(new[] { ';', ',' }, StringSplitOptions.RemoveEmptyEntries);
            if (!roles.Any())
            {
                return;

            }

            var id = new FormsIdentity(myAuthTicket);
            IPrincipal principal = new PlugPortalPrincipal(id, roles);
            Context.User = principal;
        }

        void Application_PreRequestHandlerExecute(object sender, EventArgs e)
        {
            if (HttpContext.Current.Session != null)
            {
                //This extends cache timeout due to recent access of the cache data
                var sKey = Session["UserINFO"] as string;
                var dKey = Session["UserDATAINFO"] as string;

                if (!string.IsNullOrEmpty(sKey))
                {
                    var sUser = HttpContext.Current.Cache[sKey] as string;
                }
                if (!string.IsNullOrEmpty(dKey))
                {
                    var xcode = "PLUG_USER_DATA_" + dKey;
                    var bearerCode = "PLUG_SESSION_BEARER_DATA_" + dKey;
                    var tokenCode = "PLUG_SESSION_AUTH_DATA_" + dKey;

                    var sUserData = HttpContext.Current.Cache[xcode] as UserData;
                    var sAuthData = HttpContext.Current.Cache[tokenCode] as string;
                    var sBearerData = HttpContext.Current.Cache[bearerCode] as string;
                }

            }
            else
            {
                foreach (var nItem in HttpContext.Current.Cache.OfType<string>())
                {
                    HttpContext.Current.Cache.Remove(nItem);
                }
                foreach (var nItem in HttpContext.Current.Cache.OfType<UserData>())
                {
                    {
                        var xcode = "PLUG_USER_DATA_" + nItem.Username;
                        var bearerCode = "PLUG_SESSION_BEARER_DATA_" + nItem.Username;
                        var tokenCode = "PLUG_SESSION_AUTH_DATA_" + nItem.Username;

                        HttpContext.Current.Cache.Remove(xcode);
                        HttpContext.Current.Cache.Remove(bearerCode);
                        HttpContext.Current.Cache.Remove(tokenCode);
                    }
                }
            }
        }

        public static bool IsUserAlreadyLoggedIn(string code, out string msg)
        {
            var storedUser = Convert.ToString(HttpContext.Current.Cache[code]);
            if (string.IsNullOrEmpty(storedUser))
            {
                var timeout = new TimeSpan(0, 0, HttpContext.Current.Session.Timeout, 0, 0);
                HttpContext.Current.Cache.Insert(code, code, null, DateTime.MaxValue, timeout, CacheItemPriority.High, null);
                msg = "";
                return false;
            }

            msg = "Multiple Login Not Allowed!";
            return true;

        }

        public static void ResetLogin(string code)
        {
            try
            {
                HttpContext.Current.Cache[code] = null;

            }
            catch (Exception ex)
            {
                UtilTools.LogE(ex.StackTrace, ex.Source, ex.GetBaseException().Message);
            }
        }

        public static bool SetUserData(UserData userData)
        {
            try
            {
                if (userData == null)
                {
                    return false;
                }

                var usercode = "PLUG_USER_DATA_" + userData.Username;


                if (HttpContext.Current.Cache[usercode] != null)
                {
                    HttpContext.Current.Cache.Remove(usercode);
                }

                var timeout = new TimeSpan(0, 0, HttpContext.Current.Session.Timeout, 0, 0);
                HttpContext.Current.Cache.Insert(usercode, userData, null, DateTime.MaxValue, timeout, CacheItemPriority.High, null);
                return true;
            }
            catch (Exception ex)
            {
                UtilTools.LogE(ex.StackTrace, ex.Source, ex.GetBaseException().Message);
                return false;
            }

        }


        public static UserData GetUserData(string username)
        {
            try
            {
                if (string.IsNullOrEmpty(username))
                {
                    return null;
                }
                var usercode = "PLUG_USER_DATA_" + username;
                if (!(HttpContext.Current.Cache[usercode] is UserData userData) || userData.UserId < 1)
                {
                    return null;
                }
                return userData;

            }
            catch (Exception ex)
            {
                UtilTools.LogE(ex.StackTrace, ex.Source, ex.GetBaseException().Message);
                return null;
            }

        }

        public static bool SetPortalTabData(List<TabItemObj> tabs, string username)
        {
            try
            {
                if (tabs == null || !tabs.Any() || string.IsNullOrEmpty(username))
                {
                    return false;
                }

                var usercode = "PLUG_PORTAL_TAB_DATA_" + username;


                if (HttpContext.Current.Cache[usercode] != null)
                {
                    HttpContext.Current.Cache.Remove(usercode);
                }

                var timeout = new TimeSpan(0, 0, HttpContext.Current.Session.Timeout, 0, 0);
                HttpContext.Current.Cache.Insert(usercode, tabs, null, DateTime.MaxValue, timeout, CacheItemPriority.High, null);
                return true;
            }
            catch (Exception ex)
            {
                UtilTools.LogE(ex.StackTrace, ex.Source, ex.GetBaseException().Message);
                return false;
            }

        }

        public static List<TabItemObj> GetPortalTabData(string username)
        {
            try
            {
                if (string.IsNullOrEmpty(username))
                {
                    return null;
                }
                var usercode = "PLUG_PORTAL_TAB_DATA_" + username;
                var tabs = HttpContext.Current.Cache[usercode] as List<TabItemObj>;
                return tabs;

            }
            catch (Exception ex)
            {
                UtilTools.LogE(ex.StackTrace, ex.Source, ex.GetBaseException().Message);
                return null;
            }

        }


        public static void ResetUserData(string username)
        {
            try
            {
                HttpContext.Current.Cache["PLUG_USER_DATA_" + username] = null;
                HttpContext.Current.Cache["PLUG_SESSION_AUTH_DATA_" + username] = null;
                HttpContext.Current.Cache["PLUG_SESSION_BEARER_DATA_" + username] = null;

            }
            catch (Exception ex)
            {
                UtilTools.LogE(ex.StackTrace, ex.Source, ex.GetBaseException().Message);
            }
        }
    }
}
