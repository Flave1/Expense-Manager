using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GlobalLitePortal;
using GlobalLitePortal.PortalCore.Model;
using GlobalLitePortalHelper.APIObjs;
using PluglexHelper.CoreService;
using PluglexHelper.PortalManager;
using PluglexHelper.PortalObjs;
using PlugLitePortalHelper.APIObjs;
using PlugLitePortalHelper.APIServices;
using XPLUG.WEBTOOLKIT;


namespace PlugLitePortal.Controllers.Setup
{
    public class WorkflowItemManagerController : Controller
    {
        public ActionResult Index(int? ExpenseRequisitionId)
        {
            try
            { 
                ViewBag.Error = "";
                #region current user session check

                var userData = MvcApplication.GetUserData(User.Identity.Name);
                if (userData == null || userData.UserId < 1)
                {
                    ViewBag.Error = "Session Has Expired! Please Re-Login";
                    return View(new List<WorkflowItemObj>());
                }

                #endregion

                var searchObjForUser = new UserSearchObj
                {
                    AdminUserId = userData.UserId,
                    Status = -2,
                    StopDate = "",
                    StartDate = "",
                    UserId = 0,

                };
                var retValForUsers = new PortalUserManager().LoadUsers(searchObjForUser, userData.Username);
                var usersList = retValForUsers.Users.Where(m => m.Status == 1).ToList();
                Session["_usersList_"] = usersList;
                var searchObj = new  WorkflowItemSearchObj
                {
                    AdminUserId = userData.UserId,
                    WorkflowItemId = 0,
                    Status = -2,
                    StartDate = "",
                    StopDate ="",
                };  
                var retVal = WorkflowItemServices.LoadWorkflowItems(searchObj, userData.Username);
                if (retVal?.Status == null)
                {
                    ViewBag.Error = " WorkflowItem list is empty!";
                    return View(new List< WorkflowItemObj>());
                }
                 
                if (retVal. WorkflowItems == null || !retVal. WorkflowItems.Any())
                { 
                    return View(new List< WorkflowItemObj>());
                } 
                var  WorkflowItems = retVal.WorkflowItems.OrderBy(m => m.WorkflowItemId).Where(x=>x.ExpenseRequisitionId == ExpenseRequisitionId).ToList(); 
                return View(WorkflowItems);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                return View(new List< WorkflowItemObj>());
            }
        }
 
    }
}