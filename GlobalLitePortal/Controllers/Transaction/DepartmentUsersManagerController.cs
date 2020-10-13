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
using PluglexHelper.PortalManager;
using PluglexHelper.PortalObjs;
using PlugLitePortalHelper.APIObjs;
using PlugLitePortalHelper.APIServices;
using XPLUG.WEBTOOLKIT;


namespace PlugLitePortal.Controllers.Setup
{
    public class DepartmentUserManagerController : Controller
    {
        public ActionResult Index(int? clientId, int? productId)
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
                    return View(new List<DepartmentUserObj>());
                }

                #endregion

                #region Check if Department User Session is null esle return to list

                if (Session["_DepartmentUserList_"] is List<DepartmentUserObj> DepartmentUser && DepartmentUser.Any())
                {
                    if(Session["_DepartmentList_"] is List<DepartmentObj> DepartmentList)
                    {
                        Session["_DepartmentList_"] = DepartmentList;
                    }
                    if(Session["_UsersList_"] is List<UserItemObj> UserList)
                    {
                        Session["_UsersList_"] = UserList;
                    }
                    var depList = DepartmentUser.Where(m => m.ClientId == ClientId && m.ProductId == ProductId).ToList();
                    return View(depList);
                }

                #endregion
                
                #region Request Response and Validation of responses

                var searchObj2 = new UserSearchObj
                {
                    AdminUserId = userData.UserId,
                    Status = -1000,
                    StopDate = "",
                    StartDate = "",
                    UserId = 0,

                };
                var retValForUsers = new PortalUserManager().LoadUsers(searchObj2, userData.Username);

                if (retValForUsers?.Status == null)
                {
                    ViewBag.Error = "Error Occurred!";
                    return View(new List<DepartmentUserObj>());
                }

                if (retValForUsers.Users == null || !retValForUsers.Users.Any())
                {
                    ViewBag.Error = "Error Occurred!";
                    return View(new List<DepartmentUserObj>());
                }

                var searchObj3 = new DepartmentSearchObj
                {
                    AdminUserId = userData.UserId,
                    DepartmentId = 0,
                    Status = -1000,
                    StopDate = "",
                    StartDate = ""
                };
                var retValForDepartment = ExpenseLookUpServices.LoadDepartments(searchObj3, userData.Username);
                if (retValForDepartment?.Status == null)
                {
                    ViewBag.Error = "Error Occurred!";
                    return View(new List<DepartmentUserObj>());
                }

                if (retValForDepartment.Departments == null || !retValForDepartment.Departments.Any())
                {
                    ViewBag.Error = "Error Occurred!";
                    return View(new List<DepartmentUserObj>());
                }



                var searchObj = new DepartmentUserSearchObj
                {
                    AdminUserId = userData.UserId,
                    DepartmentUserId = 0,
                };
                var retVal = DepartmentUserServices.LoadDepartmentUsers(searchObj, userData.Username);

                if (retVal?.Status == null)
                {
                    ViewBag.Error = " DepartmentUser list is empty!";
                    return View(new List<DepartmentUserObj>());
                }

                if (!retVal.Status.IsSuccessful)
                {
                    ViewBag.Error = string.IsNullOrEmpty(retVal.Status.Message.FriendlyMessage)
                        ? "  DepartmentUser list is empty!"
                        : retVal.Status.Message.FriendlyMessage;
                    return View(new List<DepartmentUserObj>());
                }
                if (retVal.DepartmentUsers == null || !retVal.DepartmentUsers.Any())
                {
                    ViewBag.Error = " DepartmentUser list is empty!";
                    return View(new List<DepartmentUserObj>());
                }


                #endregion

                #region Initialization of Responses into Sessions

                var Users = retValForUsers.Users.OrderBy(m => m.UserId).ToList();
                var Depts = retValForDepartment.Departments.OrderBy(m => m.DepartmentId).ToList();
                var DepartmentUsers = retVal.DepartmentUsers.OrderBy(m => m.DepartmentUserId).Where(m => m.ClientId == ClientId
                                                   && m.ProductId == ProductId ).ToList();



                Session["_DepartmentUserList_"] = DepartmentUsers;
                Session["_DepartmentList_"] = Depts.ToList();

                Session["_UsersList_"] = Users.ToList();

                #endregion
                 
                return View(DepartmentUsers);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                return View(new List< DepartmentUserObj>());
            }
        }

        #region Add DepartmentUser
        public ActionResult AddDepartmentUser(int clientId, int productId)
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

                var userData = MvcApplication.GetUserData(User.Identity.Name) ?? new UserData(); 
                
                if (userData.UserId < 1)
                {
                    ViewBag.SessionError = "Your session has expired! Please re-login";
                    return View(new RegDepartmentUserObj());
                }
                
                if (productId < 1 || clientId < 1 )
                {
                    RedirectToAction("Index", "Dashboard");
                }
                
                return View(new RegDepartmentUserObj { ProductId = productId, ClientId = clientId,  ProductItemId = userClientSession.ProductItemId });
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error Occurred! Please try again later";
                UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                return View(new RegDepartmentUserObj());
            }
        }
        public JsonResult ProcessAddDepartmentUserRequest(RegDepartmentUserObj model)
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

                if (model.DepartmentId < 1)
                {
                    return Json(new { isauthenticated = true, issuccessful = false, isreload = false, error = "Department  required " });
                }

                if (model.UserId < 1)
                {
                    return Json(new { isauthenticated = true, issuccessful = false, isreload = false, error = "User required " });
                }

                #endregion

                #region Check if User Exist in Department Session

                if(!(Session["_DepartmentUserList_"] is List<DepartmentUserObj> DepartmentUserList && !DepartmentUserList.Any()))
                {

                }
                var previousDepartmentList = (List<DepartmentUserObj>)Session["_DepartmentUserList_"];

                if (previousDepartmentList != null)
                {
                    if (previousDepartmentList.Count(x => x.DepartmentId == model.DepartmentId
                                                               && x.ClientId == model.ClientId
                                                               && x.ProductId == model.ProductId
                                                               && x.ProductItemId == model.ProductItemId
                                                               && x.UserId == model.UserId) > 0)
                        return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "User Already Added to Department" });
                }

                #endregion

                #region Request from Users Service

                var searchObj2 = new UserSearchObj
                {
                    AdminUserId = userData.UserId,
                    Status = -2,
                    StopDate = "",
                    StartDate = "",
                    UserId = 0,

                };

                var userRetVal = new PortalUserManager().LoadUsers(searchObj2, userData.Username);

                if (userRetVal?.Status == null)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Error Occurred! Please try again later" });
                }

                var userList = userRetVal.Users.OrderBy(x => x.UserId).ToList();

                #endregion

                

                #region Build Request Object

                var requestObj = new RegDepartmentUserObj
                {
                    ClientId = model.ClientId,
                    ProductId = model.ProductId,
                    AdminUserId = userData.UserId,
                    Status = 1,
                    ProductItemId = model.ProductItemId,
                    DepartmentId = model.DepartmentId,
                    UserId = model.UserId,
                    UserRoles = string.Join(",", userList.FirstOrDefault(x => x.UserId == model.UserId).RoleNames),
                    Email = userList.FirstOrDefault(x => x.UserId == model.UserId).Email,
                    FullName = userList.FirstOrDefault(x => x.UserId == model.UserId).FirstName + " " + userList.FirstOrDefault(x => x.UserId == model.UserId).LastName,

                };

                #endregion

                #region Response and validation
                var response = DepartmentUserServices.AddDepartmentUser(requestObj, userData.Username);

                if (response?.Status == null)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Error Occurred! Please try again later" });
                }

                if (!response.Status.IsSuccessful)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = string.IsNullOrEmpty(response.Status.Message.TechnicalMessage) ? "Process Failed! Unable to add nomination Source" : response.Status.Message.TechnicalMessage });
                } 
                var searchObj = new DepartmentUserSearchObj
                {
                    AdminUserId = userData.UserId,
                    DepartmentUserId = 0,
                };

                var retVal = DepartmentUserServices.LoadDepartmentUsers(searchObj, userData.Username);

                if (retVal?.Status != null && retVal.DepartmentUsers != null)
                {
                    var DepartmentUsers = retVal.DepartmentUsers.OrderBy(m => m.DepartmentUserId)
                        .Where(m => m.ClientId == model.ClientId && m.ProductId == model.ProductItemId && m.ProductItemId == model.ProductItemId)
                        .ToList();
                    Session["_DepartmentUserList_"] = DepartmentUsers;
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


        #region DepartmentUser Detail
        public ActionResult _DepartmentUserDetail(int DepartmentUserId)
        {
            try
            {
                ViewBag.Error = "";
                ViewBag.SessionError = "";
                var userData = MvcApplication.GetUserData(User.Identity.Name) ?? new UserData();
                if (userData.UserId < 1)
                {
                    ViewBag.SessionError = "Your session has expired! Please re-login";
                    return View(new DepartmentUserObj());
                }

                if (DepartmentUserId < 1)
                {
                    ViewBag.Error = "Invalid selection";
                    return View(new DepartmentUserObj());
                }

                if (!(Session["_DepartmentUserList_"] is List<DepartmentUserObj> DepartmentUsers) || DepartmentUsers.Count < 1)
                {
                    ViewBag.Error = "Error Occurred! Unable to process selected item";
                    return View(new DepartmentUserObj());
                }

                var thisDepartmentUser = DepartmentUsers.Find(m => m.DepartmentUserId == DepartmentUserId);
                if (thisDepartmentUser == null || thisDepartmentUser.DepartmentUserId < 1)
                {
                    ViewBag.Error = "Error Occurred! Unable to process selected item";
                    return View(new DepartmentUserObj());
                }

                return View(thisDepartmentUser);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error Occurred! Please try again later";
                UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                return View(new DepartmentUserObj());
            }
        }
        #endregion

        #region Edit DepartmentUser

        public ActionResult _EditDepartmentUser(int DepartmentUserId)
        {
            try
            {
                ViewBag.Error = "";
                ViewBag.SessionError = "";
                var userData = MvcApplication.GetUserData(User.Identity.Name) ?? new UserData();
                if (userData.UserId < 1)
                {
                    ViewBag.SessionError = "Your session has expired! Please re-login";
                    return View(new List<DepartmentUserObj>());
                }
                if (DepartmentUserId < 1)
                {
                    ViewBag.Error = "Invalid selection";
                    return View(new DepartmentUserObj());
                }

                if (!(Session["_DepartmentUserList_"] is List<DepartmentUserObj> DepartmentUserList) || DepartmentUserList.Count < 1)
                {
                    ViewBag.Error = "Error Occurred! Unable to process selected item";
                    return View(new DepartmentUserObj());
                }

                var DepartmentUser = DepartmentUserList.Find(m => m.DepartmentUserId == DepartmentUserId);
                if (DepartmentUser == null || DepartmentUser.DepartmentUserId < 1)
                {
                    ViewBag.Error = "Error Occurred! Unable to process selected item";
                    return View(new DepartmentUserObj());
                }

                Session["_CurrentSelDepartmentUser_"] = DepartmentUser;
                 
                return View(DepartmentUser);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error Occurred! Please try again later";
                UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                return View(new DepartmentUserObj());
            }
        }
        public JsonResult ProcessEditDepartmentUserRequest(DepartmentUserObj model)
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

                #region Model Validation

                if (model == null)
                {
                    return Json(new { IsSuccessful = false, Error = "Your session has expired", IsAuthenticated = false });
                }

                if (model.ClientId < 1)
                {
                    return Json(new { isauthenticated = true, issuccessful = false, isreload = false, error = "client required " });
                }
                if (model.DepartmentUserId < 1)
                {
                    return Json(new { isauthenticated = true, issuccessful = false, isreload = false, error = "Department User Id required " });
                }

                if (model.ProductItemId < 1)
                {
                    return Json(new { isauthenticated = true, issuccessful = false, isreload = false, error = "Product Item required " });
                }

                if (model.ProductId < 1)
                {
                    return Json(new { isauthenticated = true, issuccessful = false, isreload = false, error = "Product required " });
                }
                if (model.DepartmentId < 1)
                {
                    return Json(new { isauthenticated = true, issuccessful = false, isreload = false, error = "Department  required " });
                }
                if (model.UserId < 1)
                {
                    return Json(new { isauthenticated = true, issuccessful = false, isreload = false, error = "User required " });
                }
                if (!GenericVal.Validate(model, out var msg))
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = $"Validation Error Occurred! Detail: {msg}" });
                }
                #endregion

                #region Check if Item Already Exist 

                var previousDepartmentList = (List<DepartmentUserObj>)Session["_DepartmentUserList_"];
                if (previousDepartmentList != null)
                {
                    if (previousDepartmentList.Count(x => x.DepartmentId == model.DepartmentId
                                                               && x.ClientId == model.ClientId
                                                               && x.ProductId == model.ProductId
                                                               && x.ProductItemId == model.ProductItemId
                                                               && x.UserId == model.UserId
                                                               && x.DepartmentUserId != x.DepartmentUserId) > 0)
                        return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "User Already Added to Department" });
                }

                #endregion

                #region Request from Users Service

                var searchObj2 = new UserSearchObj
                {
                    AdminUserId = userData.UserId,
                    Status = -2,
                    StopDate = "",
                    StartDate = "",
                    UserId = 0,

                };

                var userRetVal = new PortalUserManager().LoadUsers(searchObj2, userData.Username);

                if (userRetVal?.Status == null)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Error Occurred! Please try again later" });
                }

                var userList = userRetVal.Users.OrderBy(x => x.UserId).ToList();

                #endregion


                var previousDepartmentUserList = (List<DepartmentUserObj>)Session["_DepartmentUserList_"];

                #region Build Request


                var passObj = new EditDepartmentUserObj()
                {

                    ClientId = model.ClientId,
                    ProductId = model.ProductId,
                    AdminUserId = userData.UserId,
                    Status = 1,
                    ProductItemId = model.ProductItemId,
                    DepartmentId = model.DepartmentId,
                    UserId = model.UserId,
                    UserRoles = string.Join(",", userList.FirstOrDefault(x => x.UserId == model.UserId).RoleNames),
                    Email = userList.FirstOrDefault(x => x.UserId == model.UserId).Email,
                    FullName = userList.FirstOrDefault(x => x.UserId == model.UserId).FirstName + " " + userList.FirstOrDefault(x => x.UserId == model.UserId).LastName,
                    DepartmentUserId = model.DepartmentUserId

                };


                #endregion

                #region Request and Response Validations

                var response = DepartmentUserServices.UpdateDepartmentUser(passObj, userData.Username);
                if (response?.Status == null)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Error Occurred! Please try again later" });
                }

                if (!response.Status.IsSuccessful)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = string.IsNullOrEmpty(response.Status.Message.TechnicalMessage) ? "Process Failed! Unable to add course of study" : response.Status.Message.TechnicalMessage });
                }

                Session["_CurrentSelDepartmentUser_"] = null;

                var searchObj = new DepartmentUserSearchObj
                {
                    AdminUserId = userData.UserId,
                    DepartmentUserId = 0,
                    Status = -2
                };

                var retVal = DepartmentUserServices.LoadDepartmentUsers(searchObj, userData.Username);
                if (retVal?.Status != null && retVal.DepartmentUsers != null)
                {
                    var DepartmentUsers = retVal.DepartmentUsers.OrderBy(m => m.DepartmentUserId).ToList();
                    Session["_DepartmentUserList_"] = DepartmentUsers.Where(m => m.ClientId == model.ClientId && m.ProductId == model.ProductItemId && m.ProductItemId == model.ProductItemId).ToList();
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