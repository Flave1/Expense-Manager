using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GlobalLitePortal;
using GlobalLitePortal.PortalCore.Model;
using GlobalLitePortalHelper.APIObjs;
using GlobalLitePortalHelper.APIObjs.Business;
using PluglexHelper.CoreService; 
using PlugLitePortalHelper.APIObjs;
using PlugLitePortalHelper.APIServices;
using XPLUG.WEBTOOLKIT;


namespace PlugLitePortal.Controllers.Setup
{
    public class BeneficiaryManagerController : Controller
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
                    return View(new List<BeneficiaryObj>());
                }

                #endregion

                #region Check if Benficiary List is  null else return to view 

                if (Session["_BeneficiaryList_"] is List<BeneficiaryObj> Beneficiary && Beneficiary.Any())
                {
                    var clientProdbeneficiaryList = Beneficiary.Where(m => m.ClientId == ClientId && m.ProductId == ProductId && m.ProductItemId == ProductItemId).ToList();
                    return View(clientProdbeneficiaryList);
                }

                #endregion

                #region Request Response and validation Checks

                var searchObj = new BeneficiarySearchObj
                {
                    AdminUserId = userData.UserId,
                    BeneficiaryId = 0,
                    Status = -2
                };
                var retVal = BeneficiaryServices.LoadBeneficiaries(searchObj, userData.Username);
                if (retVal?.Status == null)
                {
                    ViewBag.Error = " Beneficiary list is empty!";
                    return View(new List<BeneficiaryObj>());
                }

                if (!retVal.Status.IsSuccessful)
                {
                    ViewBag.Error = string.IsNullOrEmpty(retVal.Status.Message.FriendlyMessage)
                        ? "  Beneficiary list is empty!"
                        : retVal.Status.Message.FriendlyMessage;
                    return View(new List<BeneficiaryObj>());
                }
                if (retVal.Beneficiaries == null || !retVal.Beneficiaries.Any())
                {
                    ViewBag.Error = " Beneficiary list is empty!";
                    return View(new List<BeneficiaryObj>());
                }

                #endregion

                #region Send Client Product ProductItem To View
                var clientBeneficiaries = retVal.Beneficiaries.OrderBy(m => m.BeneficiaryId)
                   .Where(m => m.ClientId == ClientId && m.ProductId == ProductId )
                   .ToList();

                Session["_BeneficiaryList_"] = clientBeneficiaries;

                var clientBeneList = clientBeneficiaries.Where(m => m.ClientId == ClientId && m.ProductId == ProductId ).ToList();

                #endregion

                return View(clientBeneList);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                return View(new List< BeneficiaryObj>());
            }
        }

        #region Add Beneficiary
        public ActionResult AddBeneficiary(int clientId, int productId)
        {
            try
            {
                #region Client Product productItem Session Check

                var userClientSession = (AppSession)Session["_UserClientSession_"];
                if (userClientSession == null || userClientSession.ClientId < 1 || userClientSession.ProductId < 1 || userClientSession.ProductItemId < 1)
                {
                    return RedirectToAction("Index", "Dashboard");
                }

                #endregion

             


                ViewBag.Error = "";
                ViewBag.SessionError = "";
                var userData = MvcApplication.GetUserData(User.Identity.Name) ?? new UserData(); 
                if (userData.UserId < 1)
                {
                    ViewBag.SessionError = "Your session has expired! Please re-login";
                    return View(new RegBeneficiaryObj());
                }

                return View(new RegBeneficiaryObj { ProductId = productId, ClientId = clientId, ProductItemId = userClientSession.ProductItemId });
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error Occurred! Please try again later";
                UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                return View(new RegBeneficiaryObj());
            }
        }
        public JsonResult ProcessAddBeneficiaryRequest(RegBeneficiaryObj model)
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

                #region Model Validations
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
                if (string.IsNullOrEmpty(model.FirstName) || model.FirstName.Length < 0 || string.IsNullOrEmpty(model.FirstName) || model.FirstName.Length < 0)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = " First Name and Last Name required" });
                }

                if (string.IsNullOrEmpty(model.Email) || model.Email.Length < 2)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Email is required" });
                }
                #endregion
                
                #region Check If Item Exists from Session

                var previousBeneficiaryList = (List<BeneficiaryObj>)Session["_BeneficiaryList_"];
                if (previousBeneficiaryList != null)
                {
                    if (previousBeneficiaryList.Count(x => x.CompanyName.ToLower().Trim().ToStandardHash() == model.CompanyName.ToLower().Trim().ToStandardHash()
                                                              && x.ProductId == model.ProductId
                                                              && x.FirstName == model.FirstName
                                                              && x.LastName == model.LastName
                                                              && x.ClientId == model.ClientId
                                                               && x.ProductItemId == model.ProductItemId) > 0)
                        return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Beneficiary Already Exist!" });
                }

                #endregion

                #region Build Object Request

                var requestObj = new RegBeneficiaryObj
                {
                    ClientId = model.ClientId,
                    ProductId = model.ProductId,
                    AdminUserId = userData.UserId,
                    Status = 1,
                    ProductItemId = model.ProductItemId,
                    MiddleName = model.MiddleName,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    CompanyName = model.CompanyName,
                    BeneficiaryCode = "23flave23",
                    BeneficiaryType = model.BeneficiaryType,
                    DepartmentId = model.DepartmentId,
                    Email = model.Email,
                    MobileNumber = model.MobileNumber,


                };
                #endregion

                #region Request Response and Validations

                var response = BeneficiaryServices.AddBeneficiary(requestObj, userData.Username);
                if (response?.Status == null)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Error Occurred! Please try again later" });
                }

                if (!response.Status.IsSuccessful)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = string.IsNullOrEmpty(response.Status.Message.TechnicalMessage) ? "Process Failed! Unable to add nomination Source" : response.Status.Message.TechnicalMessage });
                }


                var searchObj = new BeneficiarySearchObj
                {
                    AdminUserId = userData.UserId,
                    BeneficiaryId = 0,
                    Status = -2
                };

                var retVal = BeneficiaryServices.LoadBeneficiaries(searchObj, userData.Username);
                if (retVal?.Status != null && retVal.Beneficiaries != null)
                {
                    var Beneficiarys = retVal.Beneficiaries.OrderBy(m => m.BeneficiaryId).ToList();
                    Session["_BeneficiaryList_"] = Beneficiarys;
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
        
        #region Beneficiary Detail
        public ActionResult _BeneficiaryDetail(int BeneficiaryId)
        {
            try
            {
                ViewBag.Error = "";
                ViewBag.SessionError = "";
                var userData = MvcApplication.GetUserData(User.Identity.Name) ?? new UserData();
                if (userData.UserId < 1)
                {
                    ViewBag.SessionError = "Your session has expired! Please re-login";
                    return View(new BeneficiaryObj());
                }

                if (BeneficiaryId < 1)
                {
                    ViewBag.Error = "Invalid selection";
                    return View(new BeneficiaryObj());
                }

                if (!(Session["_BeneficiaryList_"] is List<BeneficiaryObj> Beneficiarys) || Beneficiarys.Count < 1)
                {
                    ViewBag.Error = "Error Occurred! Unable to process selected item";
                    return View(new BeneficiaryObj());
                }

                var thisBeneficiary = Beneficiarys.Find(m => m.BeneficiaryId == BeneficiaryId);
                if (thisBeneficiary == null || thisBeneficiary.BeneficiaryId < 1)
                {
                    ViewBag.Error = "Error Occurred! Unable to process selected item";
                    return View(new BeneficiaryObj());
                }

                return View(thisBeneficiary);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error Occurred! Please try again later";
                UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                return View(new BeneficiaryObj());
            }
        }
        #endregion

        #region Edit Beneficiary

        public ActionResult _EditBeneficiary(int BeneficiaryId)
        {
            try
            {
                ViewBag.Error = "";
                ViewBag.SessionError = "";
                var userData = MvcApplication.GetUserData(User.Identity.Name) ?? new UserData();
                if (userData.UserId < 1)
                {
                    ViewBag.SessionError = "Your session has expired! Please re-login";
                    return View(new BeneficiaryObj());
                }
                if (BeneficiaryId < 1)
                {
                    ViewBag.Error = "Invalid selection";
                    return View(new BeneficiaryObj());
                }

                if (!(Session["_BeneficiaryList_"] is List<BeneficiaryObj> BeneficiaryList) || BeneficiaryList.Count < 1)
                {
                    ViewBag.Error = "Error Occurred! Unable to process selected item";
                    return View(new BeneficiaryObj());
                }

                var Beneficiary = BeneficiaryList.Find(m => m.BeneficiaryId == BeneficiaryId);
                if (Beneficiary == null || Beneficiary.BeneficiaryId < 1)
                {
                    ViewBag.Error = "Error Occurred! Unable to process selected item";
                    return View(new BeneficiaryObj());
                }

                Session["_CurrentSelBeneficiary_"] = Beneficiary;

                Beneficiary.StatusVal = Beneficiary.Status == 1;
                return View(Beneficiary);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error Occurred! Please try again later";
                UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                return View(new BeneficiaryObj());
            }
        }
        public JsonResult ProcessEditBeneficiaryRequest(BeneficiaryObj model)
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

                #region Model Validations

                var selBeneficiary = Session["_CurrentSelBeneficiary_"] as BeneficiaryObj;
                if (selBeneficiary == null || selBeneficiary.BeneficiaryId < 1)
                {
                    return Json(new { IsSuccessful = false, Error = "Your session has expired", IsAuthenticated = false });
                }

                if (model.ClientId < 1)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Client required " });
                }
                if (model.BeneficiaryId < 1)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "BeneficiaryId required" });
                }

                var previousBeneficiaryList = (List<BeneficiaryObj>)Session["_BeneficiaryList_"];
                if (previousBeneficiaryList != null)
                {
                    if (previousBeneficiaryList.Count(x => x.CompanyName.ToLower().Trim().ToStandardHash() == model.CompanyName.ToLower().Trim().ToStandardHash()
                                                           && x.ProductId == model.ProductId
                                                           && x.FirstName == model.FirstName
                                                           && x.LastName == model.LastName
                                                           && x.ClientId == model.ClientId
                                                           && x.ProductItemId == model.ProductItemId
                                                           && x.BeneficiaryId != model.BeneficiaryId) > 0)
                        return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Beneficiary Already Exist!" });
                }

                #endregion

                #region Build Object 
                var passObj = new EditBeneficiaryObj()
                {

                    ClientId = model.ClientId,
                    ProductId = model.ProductId,
                    AdminUserId = userData.UserId,
                    Status = model.StatusVal ? 1 : 0,
                    ProductItemId = model.ProductItemId,
                    MiddleName = model.MiddleName,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    CompanyName = model.CompanyName,
                    BeneficiaryCode = "23flave23",
                    BeneficiaryType = model.BeneficiaryType,
                    DepartmentId = model.DepartmentId,
                    Email = model.Email,
                    MobileNumber = model.MobileNumber,
                    BeneficiaryId = model.BeneficiaryId,

                };
                #endregion

                #region Response Validations

                if (!GenericVal.Validate(model, out var msg))
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = $"Validation Error Occurred! Detail: {msg}" });
                }


                var response = BeneficiaryServices.UpdateBeneficiary(passObj, userData.Username);
                if (response?.Status == null)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Error Occurred! Please try again later" });
                }

                if (!response.Status.IsSuccessful)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = string.IsNullOrEmpty(response.Status.Message.TechnicalMessage) ? "Process Failed! Unable to add course of study" : response.Status.Message.TechnicalMessage });
                }


                #endregion
 
                Session["_CurrentSelBeneficiary_"] = null;

                #region Request and Response Validation

                var searchObj = new BeneficiarySearchObj
                {
                    AdminUserId = userData.UserId,
                    BeneficiaryId = 0,
                    Status = -2
                };

                ViewBag.ClientId = model.ClientId;
                ViewBag.ProductId = model.ProductId;
                ViewBag.ProductItemId = model.ProductItemId;

                var retVal = BeneficiaryServices.LoadBeneficiaries(searchObj, userData.Username);
                if (retVal?.Status != null && retVal.Beneficiaries != null)
                {
                    var Beneficiarys = retVal.Beneficiaries.OrderBy(m => m.BeneficiaryId).ToList();
                    Session["_BeneficiaryList_"] = Beneficiarys;
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


    }
}