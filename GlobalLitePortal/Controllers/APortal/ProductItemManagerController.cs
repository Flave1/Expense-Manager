using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using GlobalLitePortal.PortalCore;
using GlobalLitePortal.PortalCore.Model;
using GlobalLitePortalHelper.APIObjs;
using GlobalLitePortalHelper.APIServices;
using XPLUG.WEBTOOLKIT;

namespace GlobalLitePortal.Controllers.APortal
{
    public class ProductItemManagerController : Controller
    {
        [PortalAuthorize(Roles = "PortalAdmin,ClientAdmin")]
        public ActionResult Index(int? clientId, int? productId)
        {
            try
            {
                ViewBag.ClientId = (clientId ?? 0).ToString();
                ViewBag.ProductId = (productId ?? 0).ToString();
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
        public ActionResult ProductDetail(int clientId, int productId, int itemId, string clientName, string productName)
        {
            try
            {
                ViewBag.Error = "";
                ViewBag.SessionError = "";
                var userData = MvcApplication.GetUserData(User.Identity.Name) ?? new UserData();
                if (userData.UserId < 1)
                {
                    ViewBag.SessionError = "Your session has expired! Please re-login";
                    return View(new ProductItemObj());
                }

                if (clientId < 1 || clientName.IsNullOrEmpty() || clientName.Length < 2)
                {
                    ViewBag.SessionError = "Invalid Client Selection";
                    return View(new ProductItemObj());
                }

                if (productId < 1 || productName.IsNullOrEmpty() || productName.Length < 2)
                {
                    ViewBag.SessionError = "Invalid Product Selection";
                    return View(new ProductItemObj());
                }

                if (itemId < 1)
                {
                    ViewBag.SessionError = "Invalid Product Item Selection";
                    return View(new ProductItemObj());
                }

                var prodList = Session[$"_myProductItemList_{productId}"] as List<ProductItemObj>;
                if (prodList == null || !prodList.Any())
                {
                    ViewBag.SessionError = "Invalid Product List";
                    return View(new ProductItemObj());
                }

                var thisProd = prodList.Find(m => m.ProductId == productId && m.ClientId == clientId && m.ProductItemId == itemId);
                if (thisProd == null || thisProd.ProductId < 1)
                {
                    ViewBag.SessionError = "Invalid Product Information";
                    return View(new ProductItemObj());
                }

                var retVal = new ProductItemObj
                {
                    ClientId = thisProd.ClientId,
                    ProductId = thisProd.ProductId,
                    ClientName = clientName,
                    CurrentImplementation = thisProd.CurrentImplementation,
                    Description = thisProd.Description,
                    DisplayDemographicStat = thisProd.DisplayDemographicStat,
                    DisplayLocationStat = thisProd.DisplayLocationStat,
                    DisplaySummaryStat = thisProd.DisplaySummaryStat,
                    HasOtherStat = thisProd.HasOtherStat,
                    LiveAPIUrl = thisProd.LiveAPIUrl,
                    Name = thisProd.Name,
                    SandBoxAPIUrl = thisProd.SandBoxAPIUrl,
                    Title = thisProd.Title,
                    ProductName = productName,
                    Status = thisProd.Status,
                    ProductItemId = thisProd.ProductItemId,
                };


                return View(retVal);
            }
            catch (Exception ex)
            {
                ViewBag.SessionError = "Error Occurred! Please try again later";
                UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                return View(new ProductItemObj());
            }
        }
        #endregion
        #region Add Product
            public ActionResult AddProduct(int clientId, int productId, string clientName, string productName)
            {
                try
                {
                    ViewBag.Error = "";
                    ViewBag.SessionError = "";
                    var userData = MvcApplication.GetUserData(User.Identity.Name) ?? new UserData();
                    if (userData.UserId < 1)
                    {
                        ViewBag.SessionError = "Your session has expired! Please re-login";
                        return View(new RegProductItemObj());
                    }

                    if (clientId < 1 || clientName.IsNullOrEmpty() || clientName.Length < 2)
                    {
                        ViewBag.SessionError = "Invalid Client Selection";
                        return View(new RegProductItemObj());
                    }

                    if (productId < 1 || productName.IsNullOrEmpty() || productName.Length < 2)
                    {
                        ViewBag.SessionError = "Invalid Product Selection";
                        return View(new RegProductItemObj());
                    }

                    return View(new RegProductItemObj { ClientId = clientId, ProductId = productId,  ClientName = clientName, ProductName = productName });
                }
                catch (Exception ex)
                {
                    ViewBag.SessionError = "Error Occurred! Please try again later";
                    UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                    return View(new RegProductItemObj());
                }
            }
            public JsonResult ProcessAddProductRequest(RegProductItemObj model)
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

                    if (string.IsNullOrEmpty(model.Title) || model.Title.Length < 2)
                    {
                        return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Invalid Product Title" });
                    }


                    if (model.ClientId < 1)
                    {
                        return Json(new { IsSuccessful = false, Error = "Invalid Client Information", IsAuthenticated = true, IsReload = false, });
                    }

                    if (model.ProductId < 1)
                    {
                        return Json(new { IsSuccessful = false, Error = "Invalid Product Information", IsAuthenticated = true, IsReload = false, });
                    }

                    if (model.CurrentImplementation < 1 || model.CurrentImplementation > 2)
                    {
                        return Json(new { IsSuccessful = false, Error = "Invalid Current Implementation", IsAuthenticated = true, IsReload = false, });
                    }


                    if (string.IsNullOrEmpty(model.LiveAPIUrl) || model.LiveAPIUrl.Length < 2)
                    {
                        return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Invalid Live URL" });
                    }

                    if (string.IsNullOrEmpty(model.SandBoxAPIUrl) || model.SandBoxAPIUrl.Length < 2)
                    {
                        return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Invalid Sand-Box URL" });
                    }

                    if (string.IsNullOrEmpty(model.Description) || model.Description.Length < 2)
                    {
                        return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Invalid Product Description" });
                    }

                    var addObj = new RegProductItemObj
                    {
                        AdminUserId = userData.UserId,
                        ClientId = model.ClientId,
                        CurrentImplementation = model.CurrentImplementation,
                        Description = model.Description,
                        DisplayDemographicStat = model.DisplayDemographicStat,
                        DisplayLocationStat = model.DisplayLocationStat,
                        DisplaySummaryStat = model.DisplaySummaryStat,
                        HasOtherStat = model.HasOtherStat,
                        LiveAPIUrl = model.LiveAPIUrl,
                        Name = model.Name,
                        SandBoxAPIUrl = model.SandBoxAPIUrl,
                        Status = 1,
                        Title = model.Title,
                        ProductId = model.ProductId,
                        ClientName = "",
                    };

                    var response = ProductItemService.AddProductItem(addObj, userData.Username);
                    if (response?.Status == null)
                    {
                        return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Error Occurred! Please try again later" });
                    }

                    if (!response.Status.IsSuccessful)
                    {
                        return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = string.IsNullOrEmpty(response.Status.Message.TechnicalMessage) ? "Process Failed! Unable to add nomination Source" : response.Status.Message.TechnicalMessage });
                    }

                    var searchObj = new ProductItemSearchObj
                    {
                        AdminUserId = userData.UserId,
                        ClientId = model.ClientId,
                        ProductId = model.ProductId,
                        Status = -2,
                    };

                    var retVal = ProductItemService.LoadProductItems(searchObj, userData.Username);
                    if (retVal?.Status != null && retVal.Status.IsSuccessful)
                    {
                        if (retVal.ProductItems != null && retVal.ProductItems.Any())
                        {
                            Session[$"_myProductItemList_{model.ProductId}"] = retVal.ProductItems;
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
            public ActionResult EditProduct(int clientId, int productId, int itemId, string clientName, string productName)
            {
                try
                {
                    ViewBag.Error = "";
                    ViewBag.SessionError = "";
                    var userData = MvcApplication.GetUserData(User.Identity.Name) ?? new UserData();
                    if (userData.UserId < 1)
                    {
                        ViewBag.SessionError = "Your session has expired! Please re-login";
                        return View(new RegProductItemObj());
                    }

                    if (clientId < 1 || clientName.IsNullOrEmpty() || clientName.Length < 2)
                    {
                        ViewBag.SessionError = "Invalid Client Selection";
                        return View(new RegProductItemObj());
                    }

                    if (productId < 1 || productName.IsNullOrEmpty() || productName.Length < 2)
                    {
                        ViewBag.SessionError = "Invalid Product Selection";
                        return View(new RegProductItemObj());
                    }

                    if (itemId < 1)
                    {
                        ViewBag.SessionError = "Invalid Product Item Selection";
                        return View(new RegProductItemObj());
                    }

                    var prodList = Session[$"_myProductItemList_{productId}"] as List<ProductItemObj>;
                    if (prodList == null || !prodList.Any())
                    {
                        ViewBag.SessionError = "Invalid Product List";
                        return View(new RegProductItemObj());
                    }

                    var thisProd = prodList.Find(m => m.ProductId == productId && m.ClientId == clientId && m.ProductItemId == itemId);
                    if (thisProd == null || thisProd.ProductId < 1)
                    {
                        ViewBag.SessionError = "Invalid Product Information";
                        return View(new RegProductItemObj());
                    }

                    Session["_curSelectedProductItemInfo_"] = thisProd;

                    var retVal = new RegProductItemObj
                    {
                        ClientId = thisProd.ClientId,
                        ProductId = thisProd.ProductId,
                        ClientName = clientName,
                        CurrentImplementation = thisProd.CurrentImplementation,
                        Description = thisProd.Description,
                        DisplayDemographicStat = thisProd.DisplayDemographicStat,
                        DisplayLocationStat = thisProd.DisplayLocationStat,
                        DisplaySummaryStat = thisProd.DisplaySummaryStat,
                        HasOtherStat = thisProd.HasOtherStat,
                        LiveAPIUrl = thisProd.LiveAPIUrl,
                        Name = thisProd.Name,
                        SandBoxAPIUrl = thisProd.SandBoxAPIUrl,
                        Title = thisProd.Title,
                        ProductName = productName,
                        AdminUserId = 1,
                        Status = thisProd.Status,
                        ProductItemId = thisProd.ProductItemId,
                    };


                    return View(retVal);
                }
                catch (Exception ex)
                {
                    ViewBag.SessionError = "Error Occurred! Please try again later";
                    UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                    return View(new RegProductItemObj());
                }
            }
            public JsonResult ProcessEditProductRequest(RegProductItemObj model)
            {
                try
                {
                    var userData = MvcApplication.GetUserData(User.Identity.Name) ?? new UserData();

                    if (userData.UserId < 1)
                    {
                        return Json(new { IsSuccessful = false, Error = "Your session has expired", IsAuthenticated = false });
                    }

                    var selProd = Session["_curSelectedProductItemInfo_"] as ProductItemObj;
                    if (selProd == null || selProd.ProductId < 1 || selProd.ProductItemId < 1)
                    {
                        return Json(new { IsSuccessful = false, Error = "Your session has expired", IsAuthenticated = true });
                    }

                    if (model == null)
                    {
                        return Json(new { IsSuccessful = false, Error = "Your session has expired", IsAuthenticated = true });
                    }

                    if (string.IsNullOrEmpty(model.Name) || model.Name.Length < 2)
                    {
                        return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Invalid Product Item Name" });
                    }

                    if (string.IsNullOrEmpty(model.Title) || model.Title.Length < 2)
                    {
                        return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Invalid Product Item Title" });
                    }
                    
                    if (model.ClientId < 1)
                    {
                        return Json(new { IsSuccessful = false, Error = "Invalid Client Information", IsAuthenticated = true, IsReload = false, });
                    }

                    if (model.ProductId < 1)
                    {
                        return Json(new { IsSuccessful = false, Error = "Invalid Product Information", IsAuthenticated = true, IsReload = false, });
                    }

                    if (model.CurrentImplementation < 1 || model.CurrentImplementation > 2)
                    {
                        return Json(new { IsSuccessful = false, Error = "Invalid Current Implementation", IsAuthenticated = true, IsReload = false, });
                    }


                    if (string.IsNullOrEmpty(model.LiveAPIUrl) || model.LiveAPIUrl.Length < 2)
                    {
                        return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Invalid Live URL" });
                    }

                    if (string.IsNullOrEmpty(model.SandBoxAPIUrl) || model.SandBoxAPIUrl.Length < 2)
                    {
                        return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Invalid Sand-Box URL" });
                    }

                    if (string.IsNullOrEmpty(model.Description) || model.Description.Length < 2)
                    {
                        return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Invalid Product Item Description" });
                    }

                    var addObj = new EditProductItemObj
                    {
                        AdminUserId = userData.UserId,
                        ClientId = model.ClientId,
                        CurrentImplementation = model.CurrentImplementation,
                        Description = model.Description,
                        DisplayDemographicStat = model.DisplayDemographicStat,
                        DisplayLocationStat = model.DisplayLocationStat,
                        DisplaySummaryStat = model.DisplaySummaryStat,
                        HasOtherStat = model.HasOtherStat,
                        LiveAPIUrl = model.LiveAPIUrl,
                        Name = model.Name,
                        SandBoxAPIUrl = model.SandBoxAPIUrl,
                        Status = 1,
                        Title = model.Title,
                        ProductId = model.ProductId,
                        ProductItemId = model.ProductItemId,
                    };

                    var response = ProductItemService.UpdateProductItem(addObj, userData.Username);
                    if (response?.Status == null)
                    {
                        return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Error Occurred! Please try again later" });
                    }

                    if (!response.Status.IsSuccessful)
                    {
                        return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = string.IsNullOrEmpty(response.Status.Message.TechnicalMessage) ? "Process Failed! Unable to add nomination Source" : response.Status.Message.TechnicalMessage });
                    }


                    var searchObj = new ProductItemSearchObj
                    {
                        AdminUserId = userData.UserId,
                        ClientId = model.ClientId,
                        ProductId = model.ProductId,
                        Status = -2,
                    };

                    var retVal = ProductItemService.LoadProductItems(searchObj, userData.Username);
                    if (retVal?.Status != null && retVal.Status.IsSuccessful)
                    {
                        if (retVal.ProductItems != null && retVal.ProductItems.Any())
                        {
                            Session[$"_myProductItemList_{model.ProductId}"] = retVal.ProductItems;
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
            public ActionResult _ProductItemListView(int clientId, int prodId)
            {
                try
                {
                    ViewBag.Error = "";
                    var userData = MvcApplication.GetUserData(User.Identity.Name);
                    if (userData == null || userData.UserId < 1)
                    {
                        ViewBag.Error = "Session Has Expired! Please Re-Login";
                        return View(new List<ProductItemObj>());
                    }

                    if (clientId < 1)
                    {
                        ViewBag.Error = "Invalid Client Information";
                        return View(new List<ProductItemObj>());
                    }

                    if (prodId < 1)
                    {
                        ViewBag.Error = "Invalid Product Information";
                        return View(new List<ProductItemObj>());
                    }

                    if (Session[$"_myProductItemList_{prodId}"] is List<ProductItemObj> prodList && prodList.Any())
                    {
                        return View(prodList.Where(m => m.ProductId == prodId && m.ClientId == clientId));
                    }

                    var searchObj = new ProductItemSearchObj
                    {
                        AdminUserId = userData.UserId,
                        ClientId = clientId,
                        ProductId = prodId,
                        Status = -2,
                    };

                    var retVal = ProductItemService.LoadProductItems(searchObj, userData.Username);
                    if (retVal?.Status == null)
                    {
                        ViewBag.Error = "Product Item List is empty!";
                        return View(new List<ProductItemObj>());
                    }

                    if (!retVal.Status.IsSuccessful)
                    {
                        ViewBag.Error = string.IsNullOrEmpty(retVal.Status.Message.FriendlyMessage)
                            ? "Product list is empty!"
                            : retVal.Status.Message.FriendlyMessage;
                        return View(new List<ProductItemObj>());
                    }
                    if (retVal.ProductItems == null || !retVal.ProductItems.Any())
                    {
                        ViewBag.Error = "Product list is empty!";
                        return View(new List<ProductItemObj>());
                    }

                    Session[$"_myProductItemList_{prodId}"] = retVal.ProductItems;

                    return View(retVal.ProductItems.Where(m => m.ProductId == prodId && m.ClientId == clientId));

                }
                catch (Exception ex)
                {
                    ViewBag.Error = "Unable to load Product Item list! Please try again later";
                    UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                    return View(new List<ProductItemObj>());
                }
            }
        #endregion

        #region Delete Item
            public JsonResult ProcessDeleteRequest(int clientId, int prodId, int itemId)
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

                    var passObj = new DeleteProductItemObj
                    {
                        AdminUserId = userData.UserId,
                        ProductItemId = itemId,
                    };

                    var response = ProductItemService.DeleteProductItem(passObj, User.Identity.Name);
                    if (!response.Status.IsSuccessful)
                    {
                        return Json(new { IsAuthenticated = true, IsSuccessful = false, Error = response.Status.Message.FriendlyMessage });
                    }

                    var searchObj = new ProductItemSearchObj
                    {
                        AdminUserId = userData.UserId,
                        ClientId = clientId,
                        ProductId = prodId,
                        Status = -2,
                    };

                    var retVal = ProductItemService.LoadProductItems(searchObj, userData.Username);
                    if (retVal?.Status != null && retVal.Status.IsSuccessful)
                    {
                        if (retVal.ProductItems != null && retVal.ProductItems.Any())
                        {
                            Session[$"_myProductItemList_{prodId}"] = retVal.ProductItems;
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