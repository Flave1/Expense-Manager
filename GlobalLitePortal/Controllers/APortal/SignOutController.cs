using System.Web.Mvc;
using PluglexHelper.CoreService;

namespace GlobalLitePortal.Controllers.APortal
{
    public class SignOutController : Controller
    {
        public ActionResult Index(string retUrl)
        {
            //empty all persistent variables & sessions
            Session["UserINFO"] = null;
            Session["UserDATAINFO"] = null;
            Session["_UserClientSession_"] = null;
            new FormsAuthenticationService().SignOut(); 
            return RedirectToAction("Login", "Portal", new { returnUrl = retUrl });

        }
    }
}