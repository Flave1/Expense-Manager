using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using GlobalLitePortal.PortalCore;
using GlobalLitePortal.PortalCore.Model;
using GlobalLitePortalHelper.APIObjs;
using GlobalLitePortalHelper.APIServices;
using PluglexHelper.CoreService;
using XPLUG.WEBTOOLKIT;

namespace GlobalLitePortal.Controllers.APortal
{
    public class ClientManagerController : Controller
    {
        [PortalAuthorize(Roles = "PortalAdmin")]
        public ActionResult Index()
        {
            try
            {
                var userData = MvcApplication.GetUserData(User.Identity.Name);
                if (userData == null || userData.UserId < 1)
                {
                    ViewBag.Error = "Session Has Expired! Please Re-Login";
                    return View(new List<ClientObj>());
                }
               
                var searchObj = new ClientSearchObj
                {
                    AdminUserId = userData.UserId,
                    ClientId = 0,
                    Status = -2
                };

                var retVal = ClientService.LoadClients(searchObj, userData.Username);
                if (retVal?.Status == null)
                {
                    ViewBag.Error = "Client list is empty!";
                    return View(new List<ClientObj>());
                }

                if (!retVal.Status.IsSuccessful)
                {
                    ViewBag.Error = string.IsNullOrEmpty(retVal.Status.Message.FriendlyMessage)
                        ? " Client list is empty!"
                        : retVal.Status.Message.FriendlyMessage;
                    return View(new List<ClientObj>());
                }
                if (retVal.Clients == null || !retVal.Clients.Any())
                {
                    ViewBag.Error = "Client list is empty!";
                    return View(new List<ClientObj>());
                }

                //Order By Client Name 
                var clients = retVal.Clients.OrderBy(m => m.ClientName).ToList();
                Session["_ClientList_"] = clients;
                return View(clients);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                return View(new List<ClientObj>());
            }
        }
        

        #region Client Detail
            public ActionResult _ClientDetail(int clientId)
            {
                try
                {
                    ViewBag.Error = "";
                    ViewBag.SessionError = "";
                    var userData = MvcApplication.GetUserData(User.Identity.Name) ?? new UserData();
                    if (userData.UserId < 1)
                    {
                        ViewBag.SessionError = "Your session has expired! Please re-login";
                        return View(new ClientObj());
                    }

                    if (clientId < 1)
                    {
                        ViewBag.Error = "Invalid selection";
                        return View(new ClientObj());
                    }

                    if (!(Session["_ClientList_"] is List<ClientObj> Clients) || Clients.Count < 1)
                    {
                        ViewBag.Error = "Error Occurred! Unable to process selected item";
                        return View(new ClientObj());
                    }

                    var thisClient = Clients.Find(m => m.ClientId == clientId);
                    if (thisClient == null || thisClient.ClientId < 1)
                    {
                        ViewBag.Error = "Error Occurred! Unable to process selected item";
                        return View(new ClientObj());
                    }

                    return View(thisClient);
                }
                catch (Exception ex)
                {
                    ViewBag.Error = "Error Occurred! Please try again later";
                    UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                    return View(new ClientObj());
                }
            }
        #endregion
        #region Add Client
            public ActionResult _AddClient()
            {
                try
                {
                    ViewBag.Error = "";
                    ViewBag.SessionError = "";
                    var userData = MvcApplication.GetUserData(User.Identity.Name) ?? new UserData();
                    if (userData.UserId < 1)
                    {
                        ViewBag.SessionError = "Your session has expired! Please re-login";
                        return View(new RegClientObj());
                    }

                    return View(new RegClientObj());
                }
                catch (Exception ex)
                {
                    ViewBag.Error = "Error Occurred! Please try again later";
                    UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                    return View(new RegClientObj());
                }
            }
            public JsonResult ProcessAddClientRequest(RegClientObj model)
            {
                try
                {
                    var userData = MvcApplication.GetUserData(User.Identity.Name) ?? new UserData();

                    if (userData.UserId < 1)
                    {
                        return Json(new { IsSuccessful = false, Error = "Your session has expired", IsAuthenticated = false });
                    }

                    if (model == null)
                    {
                        return Json(new { IsSuccessful = false, Error = "Your session has expired", IsAuthenticated = false });
                    }

                    if (string.IsNullOrEmpty(model.ClientName) || model.ClientName.Length < 2)
                    {
                        return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Invalid Client Name" });
                    }
                    if (string.IsNullOrEmpty(model.Address) || model.Address.Length < 2)
                    {
                        return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Invalid Address " });
                    }
                    if (string.IsNullOrEmpty(model.MobileNumber) || model.MobileNumber.Length < 7 || model.MobileNumber.Length > 15)
                    {
                        return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Invalid Mobile Number " });
                    }

                    model.AdminUserId = userData.UserId;

                    var previousClientList =
                        (List<ClientObj>)Session["_ClientList_"];
                    if (previousClientList != null)
                    {
                        if (previousClientList.Count(x =>
                                x.ClientName.ToLower().Trim().ToStandardHash() == model.ClientName.ToLower().Trim().ToStandardHash()) > 0)
                            return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Client  Already Exist!" });
                    }


                    var response = ClientService.AddClient(model, userData.Username);
                    if (response?.Status == null)
                    {
                        return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Error Occurred! Please try again later" });
                    }

                    if (!response.Status.IsSuccessful)
                    {
                        return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = string.IsNullOrEmpty(response.Status.Message.TechnicalMessage) ? "Process Failed! Unable to add nomination Source" : response.Status.Message.TechnicalMessage });
                    }

                    var searchObj = new ClientSearchObj
                    {
                        AdminUserId = userData.UserId,
                        ClientId = 0,
                        Status = -2
                    };

                    var retVal = ClientService.LoadClients(searchObj, userData.Username);
                    if (retVal?.Status != null && retVal.Clients != null)
                    {
                        var clients = retVal.Clients.OrderBy(m => m.ClientId).ToList();
                        Session["_ClientList_"] = clients;
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
        #region Client User
            public ActionResult _ManageClientUsers(int clientId, string clientName)
            {
                try
                {
                    ViewBag.Error = "";
                    ViewBag.SessionError = "";
                    var userData = MvcApplication.GetUserData(User.Identity.Name) ?? new UserData();
                    if (userData.UserId < 1)
                    {
                        ViewBag.SessionError = "Your session has expired! Please re-login";
                        return View(new ClientItemObj());
                    }
                    if (clientId < 1 || string.IsNullOrEmpty(clientName))
                    {
                        ViewBag.Error = "Invalid Client Selection";
                        return View(new ClientItemObj());
                    }
                    return View(new ClientItemObj
                    {
                        ClientName = clientName,
                        ClientId = clientId
                    });
                }
                catch (Exception ex)
                {
                    ViewBag.Error = "Error Occurred! Please try again later";
                    UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                    return View(new ClientItemObj());
                }
            }
            public ActionResult _ClientUserView(int clientId)
            {
                try
                {
                    ViewBag.Error = "";
                    Session["_selUserIds_"] = new List<int>();
                    var userData = MvcApplication.GetUserData(User.Identity.Name);
                    if (userData == null || userData.UserId < 1)
                    {
                        ViewBag.Error = "Session Has Expired! Please Re-Login";
                        return View(new List<ClientUserObj>());
                    }
                    var searchObj = new ClientUserSearchObj
                    {
                        AdminUserId = userData.UserId,
                        ClientId = clientId,
                     
                    };

                    var retVal = ClientService.LoadClientUsers(searchObj, userData.Username);
                    if (retVal?.Status == null)
                    {
                        ViewBag.Error = "Users' List is empty!";
                        return View(new List<ClientUserObj>());
                    }

                    if (!retVal.Status.IsSuccessful)
                    {
                        ViewBag.Error = string.IsNullOrEmpty(retVal.Status.Message.FriendlyMessage)
                            ? "Users' list is empty!"
                            : retVal.Status.Message.FriendlyMessage;
                        return View(new List<ClientUserObj>());
                    }
                    if (retVal.ClientUsers == null || !retVal.ClientUsers.Any())
                    {
                        ViewBag.Error = " Users' list is empty!";
                        return View(new List<ClientUserObj>());
                    }

                    Session["_selUserIds_"] = retVal.ClientUsers.Select(m => m.UserId).ToList();

                    return View(retVal.ClientUsers);

                }
                catch (Exception ex)
                {
                    ViewBag.Error = "Unable to load Users' list! Please try again later";
                    UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                    return View(new List<ClientUserObj>());
                }
            }
            public JsonResult ProcessDeleteUserRequest(int clientId, int userId)
            {
                try
                {

                    var userData = MvcApplication.GetUserData(User.Identity.Name) ?? new UserData();

                    if (userData.UserId < 1)
                    {
                        return Json(new { IsSuccessful = false, Error = "Your session has expired", IsAuthenticated = false });
                    }

                    if (clientId < 1 || userId < 1)
                    {
                        return Json(new { IsAuthenticated = true, IsSuccessful = false, Error = "Invalid / Empty Selection" });
                    }
                   
                    var passObj = new DeleteClientUserObj
                    {
                        AdminUserId = userData.UserId,
                        ClientId = clientId,
                        UserId = userId,
                    };

                    var response = ClientService.DeleteClientUser(passObj, User.Identity.Name);
                    if (!response.Status.IsSuccessful)
                    {
                        return Json(new { IsAuthenticated = true, IsSuccessful = false, Error = response.Status.Message.FriendlyMessage });
                    }
                    return Json(new { IsAuthenticated = true, IsSuccessful = true, Error = "" });

                }
                catch (Exception ex)
                {
                    UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Process Error Occurred! Please try again later" });
                }
            }
            public JsonResult ProcessAddClientUserRequest(RegClientUserObj model)
            {
                try
                {
                    var userData = MvcApplication.GetUserData(User.Identity.Name) ?? new UserData();

                    if (userData.UserId < 1)
                    {
                        return Json(new { IsSuccessful = false, Error = "Your session has expired", IsAuthenticated = false });
                    }

                   
                    if (model.ClientId < 1 || model.UserId < 1)
                    {
                        return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Invalid User Selection" });
                    }

                    model.AdminUserId = userData.UserId;

                    if (!GenericVal.Validate(model, out var msg))
                    {
                        return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = $"Validation Error Occurred! Detail: {msg}" });
                    }


                    var response = ClientService.AddClientUser(model, userData.Username);
                    if (response?.Status == null)
                    {
                        return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Error Occurred! Please try again later" });
                    }

                    if (!response.Status.IsSuccessful)
                    {
                        return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = string.IsNullOrEmpty(response.Status.Message.TechnicalMessage) ? "Process Failed! Unable to add course of study" : response.Status.Message.TechnicalMessage });
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

        #region Edit Client

            public ActionResult _EditClient(int clientId)
            {
                try
                {
                    ViewBag.Error = "";
                    ViewBag.SessionError = "";
                    var userData = MvcApplication.GetUserData(User.Identity.Name) ?? new UserData();
                    if (userData.UserId < 1)
                    {
                        ViewBag.SessionError = "Your session has expired! Please re-login";
                        return View(new ClientObj());
                    }
                    if (clientId < 1)
                    {
                        ViewBag.Error = "Invalid selection";
                        return View(new ClientObj());
                    }

                    if (!(Session["_ClientList_"] is List<ClientObj> clientList) || clientList.Count < 1)
                    {
                        ViewBag.Error = "Error Occurred! Unable to process selected item";
                        return View(new ClientObj());
                    }

                    var client = clientList.Find(m => m.ClientId == clientId);
                    if (client == null || client.ClientId < 1)
                    {
                        ViewBag.Error = "Error Occurred! Unable to process selected item";
                        return View(new ClientObj());
                    }

                    Session["_CurrentSelClient_"] = client;

                    client.StatusVal = client.Status == 1;
                    return View(client);
                }
                catch (Exception ex)
                {
                    ViewBag.Error = "Error Occurred! Please try again later";
                    UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                    return View(new ClientObj());
                }
            }
            public JsonResult ProcessEditClientRequest(ClientObj model)
            {
                try
                {
                    var userData = MvcApplication.GetUserData(User.Identity.Name) ?? new UserData();

                    if (userData.UserId < 1)
                    {
                        return Json(new { IsSuccessful = false, Error = "Your session has expired", IsAuthenticated = false });
                    }

                    var selClient = Session["_CurrentSelClient_"] as ClientObj;
                    if (selClient == null || selClient.ClientId < 1)
                    {
                        return Json(new { IsSuccessful = false, Error = "Your session has expired", IsAuthenticated = false });
                    }

                    if (model.ClientId < 1)
                    {
                        return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Invalid Selection" });
                    }

                    if (string.IsNullOrEmpty(model.ClientName) || model.ClientName.Length < 2)
                    {
                        return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Invalid Client Name" });
                    }
                    if (string.IsNullOrEmpty(model.Address) || model.Address.Length < 2)
                    {
                        return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Invalid Address " });
                    }
                    if (string.IsNullOrEmpty(model.MobileNumber) || model.MobileNumber.Length < 7 || model.MobileNumber.Length > 15)
                    {
                        return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Invalid Mobile Number " });
                    }


                    var passObj = new RegResetObj
                    {
                        AdminUserId = userData.UserId,
                        ClientName = model.ClientName,
                        Address = model.Address,
                        ClientId = selClient.ClientId,
                        MobileNumber = model.MobileNumber,
                        Email = model.Email,
                        Status = model.StatusVal ? 1 : 0,

                    };

                    if (!GenericVal.Validate(model, out var msg))
                    {
                        return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = $"Validation Error Occurred! Detail: {msg}" });
                    }


                    var response = ClientService.UpdateClient(passObj, userData.Username);
                    if (response?.Status == null)
                    {
                        return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Error Occurred! Please try again later" });
                    }

                    if (!response.Status.IsSuccessful)
                    {
                        return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = string.IsNullOrEmpty(response.Status.Message.TechnicalMessage) ? "Process Failed! Unable to add course of study" : response.Status.Message.TechnicalMessage });
                    }

                    Session["_CurrentSelClient_"] = null;

                    var searchObj = new ClientSearchObj
                    {
                        AdminUserId = userData.UserId,
                        ClientId = 0,
                        Status = -2
                    };

                    var retVal = ClientService.LoadClients(searchObj, userData.Username);
                    if (retVal?.Status != null && retVal.Clients != null)
                    {
                        var clients = retVal.Clients.OrderBy(m => m.ClientId).ToList();
                        Session["_ClientList_"] = clients;
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

        #region Delete Item
            public JsonResult ProcessDeleteRequest(int ClientId)
            {
                try
                {

                    var userData = MvcApplication.GetUserData(User.Identity.Name) ?? new UserData();

                    if (userData.UserId < 1)
                    {
                        return Json(new { IsSuccessful = false, Error = "Your session has expired", IsAuthenticated = false });
                    }

                    if (ClientId < 1)
                    {
                        return Json(new { IsAuthenticated = true, IsSuccessful = false, Error = "Invalid / Empty Selection" });
                    }
                    var previousClientList = Session["_ClientList_"] as List<ClientObj>;
                    if (previousClientList == null)
                    {
                        return Json(new { IsSuccessful = false, Error = "Your session has expired", IsAuthenticated = true });
                    }

                    var thisItem = previousClientList.Find(m => m.ClientId == ClientId);
                    if (thisItem == null || thisItem.ClientId < 1)
                    {
                        return Json(new { IsAuthenticated = true, IsSuccessful = false, Error = "Selected item does not exist in the collection" });
                    }
                    
                    var passObj = new DeleteClientObj
                    {
                        AdminUserId = userData.UserId,
                        ClientId = ClientId
                    };

                    var response = ClientService.DeleteClient(passObj, User.Identity.Name);
                    if (!response.Status.IsSuccessful)
                    {
                        return Json(new { IsAuthenticated = true, IsSuccessful = false, Error = response.Status.Message.FriendlyMessage });
                    }
                    return Json(new { IsAuthenticated = true, IsSuccessful = true, Error = "" });

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