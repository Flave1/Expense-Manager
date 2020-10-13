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
    public class ProductManagerController : Controller
    {
        [PortalAuthorize(Roles = "PortalAdmin,ClientAdmin")]
        public ActionResult Index(int? clientId)
        {
            try
            {
                ViewBag.ClientId = (clientId ?? 0).ToString();
                ViewBag.Error = "";
                var userData = MvcApplication.GetUserData(User.Identity.Name);
                if (userData == null || userData.UserId < 1)
                {


                    ViewBag.Error = "Session Has Expired! Please Re-Login";
                    return View();
                }
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                return View();
            }
        }
        #region Detail
            public ActionResult ProductDetail(int clientId, int prodId, string clientName)
            {
                try
                {
                    ViewBag.Error = "";
                    ViewBag.SessionError = "";
                    var userData = MvcApplication.GetUserData(User.Identity.Name) ?? new UserData();
                    if (userData.UserId < 1)
                    {
                        ViewBag.SessionError = "Your session has expired! Please re-login";
                        return View(new RegProductObj());
                    }

                    if (clientId < 1 || clientName.IsNullOrEmpty() || clientName.Length < 2)
                    {
                        ViewBag.SessionError = "Invalid Client Selection";
                        return View(new RegProductObj());
                    }

                    if (prodId < 1)
                    {
                        ViewBag.SessionError = "Invalid Product Selection";
                        return View(new RegProductObj());
                    }

                    var prodList = Session[$"_myProductList_{clientId}"] as List<ProductObj>;
                    if (prodList == null || !prodList.Any())
                    {
                        ViewBag.SessionError = "Invalid Product List";
                        return View(new RegProductObj());
                    }

                    var thisProd = prodList.Find(m => m.ProductId == prodId && m.ClientId == clientId);
                    if (thisProd == null || thisProd.ProductId < 1)
                    {
                        ViewBag.SessionError = "Invalid Product Information";
                        return View(new RegProductObj());
                    }

                    Session["_curSelectedProductInfo_"] = thisProd;

                    var retVal = new RegProductObj
                    {
                        ClientId = clientId,
                        ProductId = prodId,
                        ClientName = clientName,
                        Name = thisProd.Name,
                       Description = thisProd.Description,
                       Status = thisProd.Status,
                    };


                    return View(retVal);
                }
                catch (Exception ex)
                {
                    ViewBag.SessionError = "Error Occurred! Please try again later";
                    UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                    return View(new RegProductObj());
                }
            }
        #endregion
        #region Add Product
            public ActionResult AddProduct(int clientId, string clientName)
            {
                try
                {
                    ViewBag.Error = "";
                    ViewBag.SessionError = "";
                    var userData = MvcApplication.GetUserData(User.Identity.Name) ?? new UserData();
                    if (userData.UserId < 1)
                    {
                        ViewBag.SessionError = "Your session has expired! Please re-login";
                        return View(new RegProductObj());
                    }

                    if (clientId < 1 || clientName.IsNullOrEmpty() || clientName.Length < 2)
                    {
                        ViewBag.SessionError = "Invalid Client Selection";
                        return View(new RegProductObj());
                    }

                    return View(new RegProductObj{ClientId = clientId , ClientName = clientName });
                }
                catch (Exception ex)
                {
                    ViewBag.SessionError = "Error Occurred! Please try again later";
                    UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                    return View(new RegProductObj());
                }
            }
            public JsonResult ProcessAddProductRequest(RegProductObj model)
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

                    if (string.IsNullOrEmpty(model.Name) || model.Name.Length < 2)
                    {
                        return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Invalid Product Name" });
                    } 

                    if (model.ClientId < 1)
                    {
                        return Json(new { IsSuccessful = false, Error = "Invalid Client Information", IsAuthenticated = true, IsReload = false, });
                    }
                    

                    if (string.IsNullOrEmpty(model.Description) || model.Description.Length < 2)
                    {
                        return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Invalid Product Description" });
                    }

                    var addObj = new RegProductObj
                    {
                        AdminUserId = userData.UserId,
                        ClientId = model.ClientId,
                        Description = model.Description,
                        Name = model.Name,
                        Status = 1,
                    };
                
                    var response = ProductService.AddProduct(addObj, userData.Username);
                    if (response?.Status == null)
                    {
                        return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Error Occurred! Please try again later" });
                    }

                    if (!response.Status.IsSuccessful)
                    {
                        return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = string.IsNullOrEmpty(response.Status.Message.TechnicalMessage) ? "Process Failed! Unable to add nomination Source" : response.Status.Message.TechnicalMessage });
                    }

                    var searchObj = new ProductSearchObj
                    {
                        AdminUserId = userData.UserId,
                        ClientId = model.ClientId,
                        Status = -2,
                    };

                    var retVal = ProductService.LoadProducts(searchObj, userData.Username);
                    if (retVal?.Status != null && retVal.Status.IsSuccessful)
                    {
                        if (retVal.Products != null && retVal.Products.Any())
                        {
                            Session[$"_myProductList_{model.ClientId}"] = retVal.Products;
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
        #region Edit Product
            public ActionResult EditProduct(int clientId, int productId, string clientName)
            {
                try
                {
                    ViewBag.Error = "";
                    ViewBag.SessionError = "";
                    var userData = MvcApplication.GetUserData(User.Identity.Name) ?? new UserData();
                    if (userData.UserId < 1)
                    {
                        ViewBag.SessionError = "Your session has expired! Please re-login";
                        return View(new RegProductObj());
                    }

                    if (clientId < 1 || clientName.IsNullOrEmpty() || clientName.Length < 2)
                    {
                        ViewBag.SessionError = "Invalid Client Selection";
                        return View(new RegProductObj());
                    }

                    if (productId < 1 )
                    {
                        ViewBag.SessionError = "Invalid Product Selection";
                        return View(new RegProductObj());
                    }

                    var prodList = Session[$"_myProductList_{clientId}"] as List<ProductObj>;
                    if (  prodList == null || !prodList.Any())
                    {
                        ViewBag.SessionError = "Invalid Product List";
                        return View(new RegProductObj());
                    }

                    var thisProd = prodList.Find(m => m.ProductId == productId && m.ClientId == clientId);
                    if (thisProd == null || thisProd.ProductId < 1)
                    {
                        ViewBag.SessionError = "Invalid Product Information";
                        return View(new RegProductObj());
                    }

                    Session["_curSelectedProductInfo_"] = thisProd;

                    var retVal = new RegProductObj
                    {
                        ClientId = clientId,
                        ProductId = productId,
                        ClientName = clientName,
                        Description = thisProd.Description,
                        Name = thisProd.Name,
                        Status = thisProd.Status,
                        StatusVal = thisProd.Status == 1,
                    };


                    return View(retVal);
                }
                catch (Exception ex)
                {
                    ViewBag.SessionError = "Error Occurred! Please try again later";
                    UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                    return View(new RegProductObj());
                }
            }
            public JsonResult ProcessEditProductRequest(RegProductObj model)
            {
                try
                {
                    var userData = MvcApplication.GetUserData(User.Identity.Name) ?? new UserData();

                    if (userData.UserId < 1)
                    {
                        return Json(new { IsSuccessful = false, Error = "Your session has expired", IsAuthenticated = false });
                    }

                    var selProd = Session["_curSelectedProductInfo_"] as ProductObj;
                    if (selProd == null || selProd.ProductId < 1)
                    {
                        return Json(new { IsSuccessful = false, Error = "Your session has expired", IsAuthenticated = false });
                    }

                    if (model == null)
                    {
                        return Json(new { IsSuccessful = false, Error = "Your session has expired", IsAuthenticated = false });
                    }

                    if (string.IsNullOrEmpty(model.Name) || model.Name.Length < 2)
                    {
                        return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Invalid Product Name" });
                    }

                   

                    if (model.ClientId < 1)
                    {
                        return Json(new { IsSuccessful = false, Error = "Invalid Client Information", IsAuthenticated = true, IsReload = false, });
                    }
                    

                    if (string.IsNullOrEmpty(model.Description) || model.Description.Length < 2)
                    {
                        return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Invalid Product Description" });
                    }

                    var addObj = new EditProductObj
                    {
                        AdminUserId = userData.UserId,
                        ClientId = model.ClientId,
                        Description = model.Description,
                        Name = model.Name,
                        Status = model.StatusVal? 1 : 0,
                        ProductId = model.ProductId,
                    };

                    var response = ProductService.UpdateProduct(addObj, userData.Username);
                    if (response?.Status == null)
                    {
                        return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Error Occurred! Please try again later" });
                    }

                    if (!response.Status.IsSuccessful)
                    {
                        return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = string.IsNullOrEmpty(response.Status.Message.TechnicalMessage) ? "Process Failed! Unable to add nomination Source" : response.Status.Message.TechnicalMessage });
                    }


                    var searchObj = new ProductSearchObj
                    {
                        AdminUserId = userData.UserId,
                        ClientId = model.ClientId,
                        Status = -2,
                    };

                    var retVal = ProductService.LoadProducts(searchObj, userData.Username);
                    if (retVal?.Status != null && retVal.Status.IsSuccessful)
                    {
                        if (retVal.Products != null  && retVal.Products.Any())
                        {
                            Session[$"_myProductList_{model.ClientId}"] = retVal.Products;
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
        #region Display
            public ActionResult _ProductListView(int clientId)
            {
                try
                {
                    ViewBag.Error = "";
                    var userData = MvcApplication.GetUserData(User.Identity.Name);
                    if (userData == null || userData.UserId < 1)
                    {
                        ViewBag.Error = "Session Has Expired! Please Re-Login";
                        return View(new List<ProductObj>());
                    }

                    if (clientId < 1)
                    {
                        ViewBag.Error = "Invalid Client Information";
                        return View(new List<ProductObj>());
                    }

                    if (Session[$"_myProductList_{clientId}"] is List<ProductObj> prodList && prodList.Any())
                    {
                        return View(prodList);
                    }

                    var searchObj = new ProductSearchObj
                    {
                        AdminUserId = userData.UserId,
                        ClientId = clientId,
                        Status = -2,
                    };

                    var retVal = ProductService.LoadProducts(searchObj, userData.Username);
                    if (retVal?.Status == null)
                    {
                        ViewBag.Error = "Product List is empty!";
                        return View(new List<ProductObj>());
                    }

                    if (!retVal.Status.IsSuccessful)
                    {
                        ViewBag.Error = string.IsNullOrEmpty(retVal.Status.Message.FriendlyMessage)
                            ? "Product list is empty!"
                            : retVal.Status.Message.FriendlyMessage;
                        return View(new List<ProductObj>());
                    }
                    if (retVal.Products == null || !retVal.Products.Any())
                    {
                        ViewBag.Error = "Product list is empty!";
                        return View(new List<ProductObj>());
                    }

                    Session[$"_myProductList_{clientId}"] = retVal.Products;

                    return View(retVal.Products);

                }
                catch (Exception ex)
                {
                    ViewBag.Error = "Unable to load Product list! Please try again later";
                    UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                    return View(new List<ProductObj>());
                }
            }
        #endregion
        #region Product Users
            public ActionResult _ManageProductUsers(int clientId, int productId, string productName)
            {
                try
                {
                    ViewBag.Error = "";
                    ViewBag.SessionError = "";
                    var userData = MvcApplication.GetUserData(User.Identity.Name) ?? new UserData();
                    if (userData.UserId < 1)
                    {
                        ViewBag.SessionError = "Your session has expired! Please re-login";
                        return View(new ProductUserItemObj());
                    }
                    if (clientId < 1 || string.IsNullOrEmpty(productName))
                    {
                        ViewBag.Error = "Invalid Client Selection";
                        return View(new ProductUserItemObj());
                    }
                    return View(new ProductUserItemObj
                    {
                        ProductName = productName,
                        ProductId = productId,
                        ClientId = clientId
                    });
                }
                catch (Exception ex)
                {
                    ViewBag.Error = "Error Occurred! Please try again later";
                    UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                    return View(new ProductUserItemObj());
                }
            }
        public ActionResult _ProductUserView(int clientId, int productId)
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
                    ProductId = productId
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
        public JsonResult ProcessDeleteUserRequest(int clientId, int userId, int productId)
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
                    ProductId = productId
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
        #region Delete Item
        public JsonResult ProcessDeleteRequest(int clientId, int prodId)
            {
                try
                {

                    var userData = MvcApplication.GetUserData(User.Identity.Name) ?? new UserData();

                    if (userData.UserId < 1)
                    {
                        return Json(new { IsSuccessful = false, Error = "Your session has expired", IsAuthenticated = false });
                    }

                    if (clientId < 1 || prodId < 1)
                    {
                        return Json(new { IsAuthenticated = true, IsSuccessful = false, Error = "Invalid / Empty Selection" });
                    }
                   
                    var passObj = new DeleteProductObj
                    {
                        AdminUserId = userData.UserId,
                        ProductId = prodId,
                    };

                    var response = ProductService.DeleteProduct(passObj, User.Identity.Name);
                    if (!response.Status.IsSuccessful)
                    {
                        return Json(new { IsAuthenticated = true, IsSuccessful = false, Error = response.Status.Message.FriendlyMessage });
                    }

                    var searchObj = new ProductSearchObj
                    {
                        AdminUserId = userData.UserId,
                        ClientId = clientId,
                        ProductId = prodId,
                        Status = -2,
                    };

                    var retVal = ProductService.LoadProducts(searchObj, userData.Username);
                    if (retVal?.Status != null && retVal.Status.IsSuccessful)
                    {
                        if (retVal.Products != null && retVal.Products.Any())
                        {
                            Session[$"_myProductList_{clientId}"] = retVal.Products;
                        }
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