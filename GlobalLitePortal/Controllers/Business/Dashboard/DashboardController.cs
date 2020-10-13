using GlobalLitePortal.PortalCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using GlobalLitePortalHelper.APIObjs;
using XPLUG.WEBTOOLKIT;
using PlugLitePortalHelper.APIServices;
using GlobalLitePortalHelper.APICore;
using GlobalLitePortalHelper.APIObjs.Business;
using GlobalLitePortalHelper.APIServices;

namespace GlobalLitePortal.Controllers.Business
{
    public class DashboardController : Controller
    {
        [PortalAuthorize] 
        public ActionResult Index(int? ClientId, int? ProductId, int? ProductItemId)
        {
            try
            {
                #region Curent user session check

                var userData = MvcApplication.GetUserData(User.Identity.Name);
                if (userData == null || userData.UserId < 1)
                {
                    return RedirectToActionPermanent("Login", "Portal");
                }

                #endregion

                #region check if productitemid is 0, initialize  productItemId from ProductItemService based on client and product 

                if (ProductItemId == 0)
                {
                    var searchObjForItem = new ProductItemSearchObj
                    {
                        AdminUserId = userData.UserId,
                        ClientId = ClientId ?? 0,
                        ProductId = ProductId ?? 0,
                        Status = -2,
                    };
                    var retValForItem = ProductItemService.LoadProductItems(searchObjForItem, userData.Username);
                    var items = retValForItem.ProductItems.Where(m => m.ProductId == ProductId && m.ClientId == ClientId).ToList();

                    var itemCount = items.Count(m => m.ClientId == ClientId && m.ProductId == ProductId);

                    ViewBag.productItemCount = itemCount;
                    if (itemCount == 1)
                    {
                        var itemId = Convert.ToInt32(items.FirstOrDefault(m => m.ProductId == ProductId && m.ClientId == ClientId).ProductItemId);
                        ViewBag.productItemId = itemId;
                        ProductItemId = itemId;
                    }

                }


                #endregion

                #region client, product and item session initialization
                AppSession newSession = null;
                var userClientSession = (AppSession)Session["_UserClientSession_"];

                newSession = new AppSession
                {
                    ClientId = ClientId ?? 0,
                    ProductId = ProductId ?? 0,
                    ProductItemId = ProductItemId ?? 0,
                };
                Session["_UserClientSession_"] = newSession;

                var clientId = ClientId ?? newSession.ClientId;
                var productId = ProductId ?? newSession.ProductId;
                var productItemId = ProductItemId ?? newSession.ProductItemId;

                #endregion
                 

                var obj = new ClientProductInfoObj{CurrentClientId = clientId, CurrentProductId = productId, CurrentProductItemId = productItemId, ProductInfo = new ProductItemObj()};

                if (clientId > 0 && productId > 0)
                {
                    var prodInfo = getProductItemObj(clientId, productId, productItemId);
                    if (prodInfo.ProductItemId > 0)
                    {
                        obj.ProductInfo = prodInfo;
                    }
                }

                var searchObj = new AppStatSearchObj
                {
                    AdminUserId = userData.UserId,
                    StartDate = "",
                    StopDate = "",
                    ProductItemId = productItemId,
                    ProductId = productId,
                    ClientId   = clientId,
                    UserId = userData.UserId,
                };

                var retVal = DashboardServices.LoadDashboard(searchObj, userData.Username);
                if (retVal?.Status == null)
                {
                    ViewBag.Error = "No record found!";
                    return View(new ClientProductInfoObj());
                }

                if (!retVal.Status.IsSuccessful)
                {
                    ViewBag.Error = "No record found!";
                    return View(new ClientProductInfoObj());
                }
                 
                obj.Appstats = retVal;
  
                if (obj == null)
                {
                    return View(new ClientProductInfoObj());
                }
                return View(obj);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                return View(new ClientProductInfoObj());
            }
        }

        private ProductObj getProductObj(int clientId, int prodId)
        {

            try
            {
                var userData = MvcApplication.GetUserData(User.Identity.Name);
                if (userData == null || userData.UserId < 1)
                {
                    return new ProductObj();
                }

                if (userData.ClientProductList == null || !userData.ClientProductList.Any())
                {
                    return new ProductObj();
                }

                var clients = new List<ProductObj>();
                foreach (var item in userData.ClientProductList)
                {
                    if (item.ClientInfo.ClientId == clientId)
                    {
                        clients.AddRange(item.Products.Select(m => m.ProductInfo).ToList());
                        break;
                    }

                }

                if (!clients.Any())
                {
                    return new ProductObj();
                }
                
                var product = clients.Find(c => c.ProductId == prodId && c.Status == 1);
                if (product == null || product.ProductId < 1)
                {
                    return new ProductObj();
                }

                return product;
            }
            catch (Exception ex)
            {
                UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                return new ProductObj();
            }

        }

        private ProductItemObj getProductItemObj(int clientId, int prodId, int prodItem)
        {

            try
            {
                var userData = MvcApplication.GetUserData(User.Identity.Name);
                if (userData == null || userData.UserId < 1)
                {
                    return new ProductItemObj();
                }

                if (userData.ClientProductList == null || !userData.ClientProductList.Any())
                {
                    return new ProductItemObj();
                }

                var clients = new List<ProductItemObj>();
                foreach (var item in userData.ClientProductList)
                {
                    if (item.ClientInfo.ClientId == clientId)
                    {
                        var prodItemList = item.Products.Find(m => m.ProductInfo.ProductId == prodId);
                        if (prodItemList!= null && prodItemList.ProductInfo.ProductId > 0)
                        {
                            clients.AddRange(prodItemList.ProductItems);
                            break;
                        }
                        
                    }

                }

                if (!clients.Any())
                {
                    return new ProductItemObj();
                }

                var product = clients.Find(c => c.ProductItemId == prodItem && c.Status == 1);
                if (product == null || product.ProductItemId < 1)
                {
                    return new ProductItemObj();
                }

                return product;
            }
            catch (Exception ex)
            {
                UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                return new ProductItemObj();
            }

        }
    }
}