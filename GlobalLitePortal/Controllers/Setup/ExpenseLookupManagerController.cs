using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GlobalLitePortal;
using GlobalLitePortal.PortalCore.Model;
using GlobalLitePortalHelper.APICore;
using GlobalLitePortalHelper.APIObjs.Business;
using PluglexHelper.CoreService; 
using PlugLitePortalHelper.APIObjs;
using PlugLitePortalHelper.APIServices;
using XPLUG.WEBTOOLKIT;


namespace PlugLitePortal.Controllers.Setup
{
    public class ExpenseLookupManagerController : Controller
    {
        public ActionResult Index(int? clientId, int? productId )
        {
            try
            { 
                ViewBag.Error = "";
                #region Client Product productItem Session Check

                var userClientSession = (AppSession)Session["_UserClientSession_"];
                if (userClientSession == null || userClientSession.ClientId < 1 || userClientSession.ProductId < 1 || userClientSession.ProductItemId < 1)
                {
                    return RedirectToAction("Index", "Dashboard");
                }
                var ClientId = clientId ?? userClientSession.ClientId;
                var ProductId = productId ?? userClientSession.ProductId;
                var ProductItemId = userClientSession.ProductItemId;

                #endregion

                #region Current User Session Check

                var userData = MvcApplication.GetUserData(User.Identity.Name);
                if (userData == null || userData.UserId < 1)
                {
                    ViewBag.Error = "Session Has Expired! Please Re-Login";
                    return View(new List<ExpenseLookupObj>());
                }

                #endregion


                #region Check if session list is empty else return to view 

                if (Session["_ExpenseLookupList_"] is List<ExpenseLookupObj> expenseLookup && expenseLookup.Any())
                {
                    var clientProd = expenseLookup.Where(m => m.ClientId == ClientId && m.ProductId == ProductId ).ToList();
                    return View(clientProd);
                }

                #endregion


                #region Request And Response Validations

                var searchObj = new ExpenseLookupSearchObj
                {
                    AdminUserId = userData.UserId,
                    ExpenseLookupId = 0,
                    Status = 1
                };
                var retVal = ExpenseLookUpServices.LoadExpenseLookups(searchObj, userData.Username);

                if (retVal?.Status == null)
                {
                    ViewBag.Error = " ExpenseLookup list is empty!";
                    return View(new List<ExpenseLookupObj>());
                }

                if (!retVal.Status.IsSuccessful)
                {
                    ViewBag.Error = string.IsNullOrEmpty(retVal.Status.Message.FriendlyMessage)
                        ? "  ExpenseLookup list is empty!"
                        : retVal.Status.Message.FriendlyMessage;
                    return View(new List<ExpenseLookupObj>());
                }
                if (retVal.ExpenseLookups == null || !retVal.ExpenseLookups.Any())
                {
                    ViewBag.Error = " ExpenseLookup list is empty!";
                    return View(new List<ExpenseLookupObj>());
                }

                #endregion

                #region List Initialization into Session
                var ExpenseLookups = retVal.ExpenseLookups.OrderBy(m => m.ExpenseLookupId).ToList();

                var ClientProdList = ExpenseLookups.Where(m => m.ClientId == ClientId && m.ProductId == ProductId ).ToList();

                Session["_ExpenseLookupList_"] = ClientProdList;

                #endregion

                var clientProdExpLkps = ExpenseLookups.Where(m => m.ClientId == ClientId && m.ProductId == ProductId ).ToList();

                return View(clientProdExpLkps);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                return View(new List< ExpenseLookupObj>());
            }
        }

        
        #region Add ExpenseLookup
        public ActionResult AddExpenseLookup(int clientId, int productId)
        {
            try
            {
                ViewBag.Error = "";
                ViewBag.SessionError = "";
                #region Client Product productItem Session Check

                var userClientSession = (AppSession)Session["_UserClientSession_"];
                if (userClientSession == null || userClientSession.ClientId < 1 || userClientSession.ProductId < 1 || userClientSession.ProductItemId < 1)
                {
                    return RedirectToAction("Index", "Dashboard");
                } 

                #endregion
                #region Check if current user session is empty

                var userData = MvcApplication.GetUserData(User.Identity.Name) ?? new UserData();
                if (userData.UserId < 1)
                {
                    ViewBag.SessionError = "Your session has expired! Please re-login";
                    return View(new RegExpenseLookupObj());
                }

                #endregion
                  
                return View(new RegExpenseLookupObj { ProductId = productId, ClientId = clientId, ProductItemId = userClientSession.ProductItemId });
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error Occurred! Please try again later";
                UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                return View(new RegExpenseLookupObj());
            }
        }
        public JsonResult ProcessAddExpenseLookupRequest(ExpenseLookupObj model)
        {
            try
            {
                #region Current User Session Check

                var userData = MvcApplication.GetUserData(User.Identity.Name) ?? new UserData();
                if (userData.UserId < 1)
                {
                    return Json(new { IsSuccessful = false, Error = "Your session has expired", IsAuthenticated = false });
                }

                #endregion

                #region Model validations

                if (model == null)
                {
                    return Json(new { IsSuccessful = false, Error = "Your session has expired", IsAuthenticated = false });
                }

                if (model.ClientId < 1)
                {
                    return Json(new { isauthenticated = true, issuccessful = false, isreload = false, error = "client required " });
                }

                if (model.ProductItemId < 1)
                {
                    return Json(new { isauthenticated = true, issuccessful = false, isreload = false, error = "Product Item required " });
                }

                if (model.ProductId < 1)
                {
                    return Json(new { isauthenticated = true, issuccessful = false, isreload = false, error = "Product required " });
                }
                if (string.IsNullOrEmpty(model.InclusionList) || model.InclusionList.Length < 0)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "No Item Selected" });
                }

                if (string.IsNullOrEmpty(model.LookupName) || model.LookupName.Length < 2)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Lookup Name is required" });
                }

                #endregion


                #region  Expense   Lookup from   service  
                var searchObjLKP = new ExpenseLookupSearchObj
                {
                    AdminUserId = userData.UserId,
                    ExpenseLookupId = 0,
                    Status = -2
                };

                var retValLKP = ExpenseLookUpServices.LoadExpenseLookups(searchObjLKP, userData.Username);
                if (retValLKP?.Status == null && retValLKP.ExpenseLookups == null)
                {

                }

                var LKPList = retValLKP.ExpenseLookups
                            .Where(m => m.LookupName.Trim().ToLower().ToStandardHash()
                            == model.LookupName.ToString().Trim().ToLower().ToStandardHash() 
                            && m.ClientId == model.ClientId && m.ProductId == model.ProductId && m.ProductItemId == model.ProductItemId ).ToList();

                if (LKPList.Count() != 0 )
                {
                    return Json(new { IsSuccessful = false, IsAuthenticated = true });
                }

                
                #endregion
                 

                #region Build Request Object

                ExpenseLookupRegRespObj response = null;
                var previousExpenseLookupList = (List<ExpenseLookupObj>)Session["_ExpenseLookupList_"];

                var requestObjAdd = new RegExpenseLookupObj
                {   
                    ClientId = model.ClientId,
                    ProductId = model.ProductId,
                    AdminUserId = userData.UserId,
                    ChannelCode = "Channel",
                    ExclusionList = " 10001,10001,10001,10001,10001,10001,10001,10001,10001,10001,10001,10001,10001,10001,10001,10001,10001,10001,10001,10001,10001,10001,10001,10001,10001,10001,10001,10001,10001,10001,10001,10001,10001,10001,10001,10001,10001,10001,10001,10001",
                    InclusionList = model.InclusionList,
                    IsEnabled = model.IsEnabled,
                    LookupItem = model.LookupItem,
                    LookupName = model.LookupName,
                    RegisteredBy = userData.UserId,
                    Status = 1,
                    TimeStampRegistered = DateTime.Now.ToString("ddd-mmm-yyyy"),
                    ProductItemId = model.ProductItemId,

                };
                response = ExpenseLookUpServices.AddExpenseLookup(requestObjAdd, userData.Username);


                #endregion

                #region Request Responses And Validations


                if (response?.Status == null)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Error Occurred! Please try again later" });
                }

                if (!response.Status.IsSuccessful)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = string.IsNullOrEmpty(response.Status.Message.TechnicalMessage) ? "Process Failed! Unable to add nomination Source" : response.Status.Message.TechnicalMessage });
                }


                var searchObj = new ExpenseLookupSearchObj
                {
                    AdminUserId = userData.UserId,
                    ExpenseLookupId = 0,
                    Status = -2
                };

                var retVal = ExpenseLookUpServices.LoadExpenseLookups(searchObj, userData.Username);
                if (retVal?.Status != null && retVal.ExpenseLookups != null)
                {
                    var ExpenseLookups = retVal.ExpenseLookups.OrderBy(m => m.ExpenseLookupId).ToList();
                    Session["_ExpenseLookupList_"] = ExpenseLookups.Where(m => m.ProductId == model.ProductId && m.ClientId == model.ClientId && m.ProductItemId == model.ProductItemId).ToList();
                }

                #endregion 

                return Json(new { IsAuthenticated = true, IsSuccessful = true, IsReload = false, Error = "" });
            }
            catch (Exception ex)
            {
                UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Process Error Occurred! Please try again later" });
            }
        }
        #endregion
         
        #region ExpenseLookup Detail
        public ActionResult _ExpenseLookupDetail(int ExpenseLookupId)
        {

            try
            {
                ViewBag.Error = "";
                ViewBag.SessionError = "";
                var userData = MvcApplication.GetUserData(User.Identity.Name) ?? new UserData();
                if (userData.UserId < 1)
                {
                    ViewBag.SessionError = "Your session has expired! Please re-login";
                    return View(new ExpenseLookupObj());
                }
                if (!(Session["_ExpenseLookupList_"] is List<ExpenseLookupObj> ExpenseLookupList) || ExpenseLookupList.Count < 1)
                {
                    ViewBag.Error = "Error Occurred! Unable to process selected item";
                    return View(new ExpenseLookupObj());
                }

                var ExpenseLookup = ExpenseLookupList.Find(m => m.ExpenseLookupId == ExpenseLookupId);
                if (ExpenseLookup == null || ExpenseLookup.ExpenseLookupId < 1)
                {
                    ViewBag.Error = "Error Occurred! Unable to process selected item";
                    return View(new ExpenseLookupObj());
                }

                Session["_CurrentSelExpenseLookup_"] = ExpenseLookup;

                ExpenseLookup.StatusVal = ExpenseLookup.Status == 1;
                return View(ExpenseLookup);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error Occurred! Please try again later";
                UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                return View(new ExpenseLookupObj());
            }
        }
        #endregion

        #region Edit ExpenseLookup

        public ActionResult _EditExpenseLookup(int ExpenseLookupId)
        {
            try
            {
                ViewBag.Error = "";
                ViewBag.SessionError = "";
                var userData = MvcApplication.GetUserData(User.Identity.Name) ?? new UserData();
                if (userData.UserId < 1)
                {
                    ViewBag.SessionError = "Your session has expired! Please re-login";
                    return View(new ExpenseLookupObj());
                }
                if (ExpenseLookupId < 1)
                {
                    ViewBag.Error = "Invalid selection";
                    return View(new ExpenseLookupObj());
                }

                if (!(Session["_ExpenseLookupList_"] is List<ExpenseLookupObj> ExpenseLookupList) || ExpenseLookupList.Count < 1)
                {
                    ViewBag.Error = "Error Occurred! Unable to process selected item";
                    return View(new ExpenseLookupObj());
                }

                var ExpenseLookup = ExpenseLookupList.Find(m => m.ExpenseLookupId == ExpenseLookupId);
                if (ExpenseLookup == null || ExpenseLookup.ExpenseLookupId < 1)
                {
                    ViewBag.Error = "Error Occurred! Unable to process selected item";
                    return View(new ExpenseLookupObj());
                }

                Session["_CurrentSelExpenseLookup_"] = ExpenseLookup;

                ExpenseLookup.StatusVal = ExpenseLookup.Status == 1;
                return View(ExpenseLookup);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error Occurred! Please try again later";
                UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                return View(new ExpenseLookupObj());
            }
        }
       
        public JsonResult ProcessEditExpenseLookupRequest(ExpenseLookupObj model)
        {
            try
            {
                #region Current User Session Check

                var userData = MvcApplication.GetUserData(User.Identity.Name) ?? new UserData();

                if (userData.UserId < 1)
                {
                    return Json(new { IsSuccessful = false, Error = "Your session has expired", IsAuthenticated = false });
                }

                #endregion

                #region Validation Checks
                var selExpenseLookup = Session["_CurrentSelExpenseLookup_"] as ExpenseLookupObj;
                if (selExpenseLookup == null || selExpenseLookup.ExpenseLookupId < 1)
                {
                    return Json(new { IsSuccessful = false, Error = "Your session has expired", IsAuthenticated = false });
                }

                if (model.ClientId < 1)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Client required " });
                }
                if (model.ExpenseLookupId < 1)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "ExpenseLookupId required" });
                }

                if (string.IsNullOrEmpty(model.LookupName) || model.LookupName.Length < 2)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Lookup Name is required" });
                }
                if (!GenericVal.Validate(model, out var msg))
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = $"Validation Error Occurred! Detail: {msg}" });
                }
                #endregion

                #region Build Requesr Object


                var passObj = new EditExpenseLookupObj()
                {

                    ClientId = model.ClientId,
                    ProductId = model.ProductId,
                    AdminUserId = userData.UserId,
                    ChannelCode = "Channel",
                    ExclusionList = model.ExclusionList,
                    InclusionList = model.InclusionList,
                    IsEnabled = model.IsEnabled,
                    LookupItem = model.LookupItem,
                    LookupName = model.LookupName,
                    RegisteredBy = 1,
                    Status = 1,
                    TimeStampRegistered = DateTime.Now.ToString("ddd-mmm-yyyy"),
                    ExpenseLookupId = model.ExpenseLookupId,
                    ProductItemId = model.ProductItemId

                };


                #endregion

                #region Request and responses Validation checks

                var response = ExpenseLookUpServices.UpdateExpenseLookup(passObj, userData.Username);
                if (response?.Status == null)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Error Occurred! Please try again later" });
                }

                if (!response.Status.IsSuccessful)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = string.IsNullOrEmpty(response.Status.Message.TechnicalMessage) ? "Process Failed! Unable to add course of study" : response.Status.Message.TechnicalMessage });
                }

                Session["_CurrentSelExpenseLookup_"] = null;

                var searchObj = new ExpenseLookupSearchObj
                {
                    AdminUserId = userData.UserId,
                    ExpenseLookupId = 0,
                    Status = -2
                };

                var retVal = ExpenseLookUpServices.LoadExpenseLookups(searchObj, userData.Username);
                if (retVal?.Status != null && retVal.ExpenseLookups != null)
                {
                    var ExpenseLookups = retVal.ExpenseLookups.OrderBy(m => m.ExpenseLookupId).ToList();
                    Session["_ExpenseLookupList_"] = ExpenseLookups.Where(m => m.ProductId == model.ProductId && m.ClientId == model.ClientId && m.ProductItemId == model.ProductItemId).ToList();
                }

                #endregion

                return Json(new { IsAuthenticated = true, IsSuccessful = true, IsReload = false, Error = "" });
            }
            catch (Exception ex)
            {
                UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Process Error Occurred! Please try again later" });
            }
        }
        #endregion
          
         
        #region Add Classification ExpenseLookups

        #region Categories
        public PartialViewResult _Categories()
        {
            var userData = MvcApplication.GetUserData(User.Identity.Name);
            if (userData == null || userData.UserId < 1)
            {
                return PartialView(new List<ExpenseCategoryObj>());
            }

            var searchObj = new ExpenseCategorySearchObj
            {
                AdminUserId = userData.UserId,
                Status = -2,
                ExpenseCategoryId = 0

            };

            var retVal = ExpenseLookUpServices.LoadExpenseCategories(searchObj, userData.Username);
            if (retVal?.Status == null)
            {
                return PartialView(new List<ExpenseCategoryObj>());
            }

            if (!retVal.Status.IsSuccessful)
            {
                return PartialView(new List<ExpenseCategoryObj>());
            }

            if (!retVal.ExpenseCategories.Any())
            {
                return PartialView(new List<ExpenseCategoryObj>());
            }

            var categories = retVal.ExpenseCategories.Where(c => c.Status == 1).OrderBy(c => c.ExpenseCategoryId);

            return PartialView(categories);
        }

        #endregion

        public ActionResult AddClassificationLookup(int? categoryId, int? clientId, int? productId )
        {
            try
            {
                ViewBag.Error = "";
                ViewBag.SessionError = "";
                #region Current User Session Check 

                var userData = MvcApplication.GetUserData(User.Identity.Name) ?? new UserData();
                if (userData.UserId < 1)
                {
                    ViewBag.SessionError = "Your session has expired! Please re-login";
                    return View(new ExpenseLookupObj());
                }

                #endregion

                #region Client Product productItem Session Check

                var userClientSession = (AppSession)Session["_UserClientSession_"];
                if (userClientSession == null || userClientSession.ClientId < 1 || userClientSession.ProductId < 1 || userClientSession.ProductItemId < 1)
                {
                    return RedirectToAction("Index", "Dashboard");
                }
                var ClientId = clientId ?? userClientSession.ClientId;
                var ProductId = productId ?? userClientSession.ProductId;
                var ProductItemId = userClientSession.ProductItemId;
                ViewBag.ClientId = ClientId;
                ViewBag.ProductId = ProductId;
                ViewBag.ProductItemId = ProductItemId;
                #endregion

                #region Fetch Client and Product Added Category    

                var searchObjFiltered = new FilteredLookupSearchObj
                {
                    AdminUserId = userData.UserId,
                    ProductId = productId??0,
                    ClientId = clientId??0,

                };
                var retValForCat = ExpenseLookUpServices.LoadFilteredExpenseCategories(searchObjFiltered, userData.Username);
                var catList = retValForCat.ExpenseCategories.OrderBy(m => m.ExpenseCategoryId);

                ViewBag.CategoryList = catList;

                #endregion

                #region Check if List Session is null else return to view

                //if (!(Session["_ExpenseLookupList_"] is List<ExpenseLookupObj> ExpenseLookupList) || ExpenseLookupList.Count < 1)
                //{
                //    return View(new ExpenseLookupObj());
                //}

                #endregion
                
                #region  Expense Category Lookup from Category service or Session is not null

                ExpenseLookupRespObj retVal = null;

                //if (Session["_clientProdCategoryLKP_"] is ExpenseLookupObj ExpenseLookup)
                //{
                //    return View(ExpenseLookup);
                //}
                //else
                //{
                    var searchObj = new ExpenseLookupSearchObj
                    {
                        AdminUserId = userData.UserId,
                        ExpenseLookupId = 0,
                        Status = -2
                    };

                    retVal = ExpenseLookUpServices.LoadExpenseLookups(searchObj, userData.Username);
                    if (retVal?.Status == null && retVal.ExpenseLookups == null)
                    {
                        return View(new ExpenseLookupObj());
                    }
                //}
             

                var classificationLKP = retVal.ExpenseLookups
                            .Where(m => m.LookupName.Trim().ToLower().ToStandardHash()
                            == ExpenseLookupItems.ExpenseClassification.ToString().Trim().ToLower().ToStandardHash())
                                                 .OrderBy(m => m.ExpenseLookupId).ToList();

                if (classificationLKP == null || !classificationLKP.Any())
                {
                    return View(new ExpenseLookupObj());
                }

                var clientProdClassificationLKP = classificationLKP.SingleOrDefault(m => m.ClientId == clientId && m.ProductId == productId);

                if (clientProdClassificationLKP == null || clientProdClassificationLKP.ExpenseLookupId < 1)
                {
                    return View(new ExpenseLookupObj());
                }

                Session["_clientProdClassificationLKP_"] = clientProdClassificationLKP;
                string[] classificationIds;
                classificationIds = clientProdClassificationLKP.InclusionList.Split(',');


                

                return View(clientProdClassificationLKP);
                #endregion


            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error Occurred! Please try again later";
                UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                return View(new RegExpenseLookupObj());
            }
        }
        public JsonResult ProcessAddDepenDentLKPRequest(RegExpenseLookupObj model)
        {
            try
            {
                #region Current user Session check

                var userData = MvcApplication.GetUserData(User.Identity.Name) ?? new UserData();
                if (userData.UserId < 1)
                {
                    return Json(new { IsSuccessful = false, Error = "Your session has expired", IsAuthenticated = false });
                }


                #endregion

                #region model validations

                if (model == null)
                {
                    return Json(new { IsSuccessful = false, Error = "Your session has expired", IsAuthenticated = false });
                }

                if (model.ClientId < 1)
                {
                    return Json(new { isauthenticated = true, issuccessful = false, isreload = false, error = "client required " });
                }

                if (model.ProductItemId < 1)
                {
                    return Json(new { isauthenticated = true, issuccessful = false, isreload = false, error = "Product Item required " });
                }

                if (model.ProductId < 1)
                {
                    return Json(new { isauthenticated = true, issuccessful = false, isreload = false, error = "Product required " });
                }
                if (string.IsNullOrEmpty(model.InclusionList) || model.InclusionList.Length < 0)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "No Item Selected" });
                }

                if (string.IsNullOrEmpty(model.LookupName) || model.LookupName.Length < 2)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Lookup Name is required" });
                }

                #endregion

                #region  Expense Lookup from service  

                var searchObjLKP = new ExpenseLookupSearchObj
                {
                    AdminUserId = userData.UserId,
                    ExpenseLookupId = 0,
                    Status = -2
                };

                var retValLKP = ExpenseLookUpServices.LoadExpenseLookups(searchObjLKP, userData.Username);
                if (retValLKP?.Status == null && retValLKP.ExpenseLookups == null)
                {

                }

                var LKPList = retValLKP.ExpenseLookups
                            .Where(m => m.LookupName.Trim().ToLower().ToStandardHash()
                            == model.LookupName.ToString().Trim().ToLower().ToStandardHash())
                                                 .OrderBy(m => m.ExpenseLookupId).ToList();

                if (LKPList == null || !LKPList.Any())
                {
                    return Json(new { IsSuccessful = false, IsAuthenticated = true });
                }

                var thisClientProdLKP = LKPList.SingleOrDefault(m => m.ClientId == model.ClientId && m.ProductId == model.ProductId);

                if (thisClientProdLKP == null || thisClientProdLKP.ExpenseLookupId < 1)
                {
                    return Json(new { IsSuccessful = false, IsAuthenticated = true, Error = "Something went wrong" });
                }

                thisClientProdLKP.InclusionList = thisClientProdLKP.InclusionList.TrimStart(',').TrimEnd(',');

                string[] NewLKPIdsArray;

                string[] addedLkpIds = thisClientProdLKP.InclusionList.Trim().Split(',');

                string[] modelLKPIds = model.InclusionList.Trim().Split(',');

                NewLKPIdsArray = addedLkpIds.Concat(modelLKPIds).ToArray();

                HashSet<string> newList = new HashSet<string>(NewLKPIdsArray);

                newList.UnionWith(addedLkpIds);
                string lkpIDtoAdd = string.Join(",", newList); 



                #endregion

                #region Build Request Object

                var requestObj = new EditExpenseLookupObj
                {
                    ClientId = model.ClientId,
                    ProductId = model.ProductId,
                    AdminUserId = userData.UserId,
                    ChannelCode = thisClientProdLKP.ChannelCode,
                    ExclusionList = " 10001,10001,10001,10001,10001,10001,10001,10001,10001,10001,10001,10001,10001,10001,10001,10001,10001,10001,10001,10001,10001,10001,10001,10001,10001,10001,10001,10001,10001,10001,10001,10001,10001,10001,10001,10001,10001,10001,10001,10001",
                    InclusionList = lkpIDtoAdd,
                    IsEnabled = thisClientProdLKP.IsEnabled,
                    LookupItem = model.LookupItem,
                    LookupName = model.LookupName,
                    RegisteredBy = userData.UserId,
                    Status = 1,
                    TimeStampRegistered = DateTime.Now.ToString("ddd-mmm-yyyy"),
                    ProductItemId = thisClientProdLKP.ProductItemId,
                    ExpenseLookupId = thisClientProdLKP.ExpenseLookupId,

                };

                #endregion

                #region Request and response validations

                var response = ExpenseLookUpServices.UpdateExpenseLookup(requestObj, userData.Username);
                if (response?.Status == null)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Error Occurred! Please try again later" });
                }

                if (!response.Status.IsSuccessful)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = string.IsNullOrEmpty(response.Status.Message.TechnicalMessage) ? "Process Failed! Unable to add nomination Source" : response.Status.Message.TechnicalMessage });
                }


                var searchObj = new ExpenseLookupSearchObj
                {
                    AdminUserId = userData.UserId,
                    ExpenseLookupId = 0,
                    Status = -2
                };

                var retVal = ExpenseLookUpServices.LoadExpenseLookups(searchObj, userData.Username);
                if (retVal?.Status != null && retVal.ExpenseLookups != null)
                {
                    var ExpenseLookups = retVal.ExpenseLookups.OrderBy(m => m.ExpenseLookupId).ToList();
                    Session["_ExpenseLookupList_"] = ExpenseLookups;
                }


                #endregion

                ViewBag.ClientId = model.ClientId ;
                ViewBag.ProductId = model.ProductId;
                ViewBag.ProductItemId = model.ProductItemId;

                return Json(new { IsAuthenticated = true, IsSuccessful = true, IsReload = false, Error = "" });
            }
            catch (Exception ex)
            {
                UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Process Error Occurred! Please try again later" });
            }
        }
        #endregion

        #region Add Epense Item Lookpus

        public ActionResult AddItemLookup(int? categoryId, int? clientId, int? productId)
        {
            try
            {
                ViewBag.Error = "";
                ViewBag.SessionError = "";
                #region Curret User Session Check 

                var userData = MvcApplication.GetUserData(User.Identity.Name) ?? new UserData();
                if (userData.UserId < 1)
                {
                    ViewBag.SessionError = "Your session has expired! Please re-login";
                    return View(new ExpenseLookupObj());
                }

                #endregion

                #region Client Product productItem Session Check

                var userClientSession = (AppSession)Session["_UserClientSession_"];
                if (userClientSession == null || userClientSession.ClientId < 1 || userClientSession.ProductId < 1 || userClientSession.ProductItemId < 1)
                {
                    return RedirectToAction("Index", "Dashboard");
                }
                var ClientId = clientId ?? userClientSession.ClientId;
                var ProductId = productId ?? userClientSession.ProductId;
                var ProductItemId =  userClientSession.ProductItemId;

                ViewBag.ClientId = ClientId;
                ViewBag.ProductId = ProductId;
                ViewBag.ProductItemId = ProductItemId;
                #endregion

                #region  Expense Item Lookup from  Expense items service 

                ExpenseLookupRespObj retVal = null;

               
                    var searchObj = new ExpenseLookupSearchObj
                    {
                        AdminUserId = userData.UserId,
                        ExpenseLookupId = 0,
                        Status = -2
                    };

                    retVal = ExpenseLookUpServices.LoadExpenseLookups(searchObj, userData.Username);
                    if (retVal?.Status == null && retVal.ExpenseLookups == null)
                    {
                        return View(new ExpenseLookupObj());
                    }
              

                var itemsLKP = retVal.ExpenseLookups
                            .Where(m => m.LookupName.Trim().ToLower().ToStandardHash()
                            == ExpenseLookupItems.ExpenseItem.ToString().Trim().ToLower().ToStandardHash())
                                                 .OrderBy(m => m.ExpenseLookupId).ToList();

                if (itemsLKP == null || !itemsLKP.Any())
                {
                    return View(new ExpenseLookupObj());
                }

                var clientProdItemsLKP = itemsLKP.SingleOrDefault(m => m.ClientId == clientId && m.ProductId == productId);

                if (clientProdItemsLKP == null || clientProdItemsLKP.ExpenseLookupId < 1)
                {
                    return View(new ExpenseLookupObj());
                }

                Session["_clientProdCategoryLKP_"] = clientProdItemsLKP;
                string[] itemIds;
                itemIds = clientProdItemsLKP.InclusionList.Split(',');

               

                return View(clientProdItemsLKP);
                #endregion


            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error Occurred! Please try again later";
                UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                return View(new RegExpenseLookupObj());
            }
        }

        #endregion
    }
}