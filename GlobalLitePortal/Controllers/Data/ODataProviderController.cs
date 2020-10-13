using System;
using System.Linq;
using System.Web.Mvc;
using GlobalLitePortal;
using XPLUG.WEBTOOLKIT;
using System.Collections.Generic;
using GlobalLitePortalHelper.APICore;
using GlobalLitePortalHelper.APIObjs;
using GlobalLitePortalHelper.APIServices;

namespace PluglexPortal.Controllers.Data
{
    public class ODataProviderController : Controller
    {
        public ActionResult LoadClients()
        {
            var add = new NameValueObject { Id = 0, Name = "-- Empty Client List --" };
            try
            {
                var userData = MvcApplication.GetUserData(User.Identity.Name);
                if (userData == null || userData.UserId < 1)
                {
                    return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
                }

                var searchObj = new ClientSearchObj
                {
                    AdminUserId = userData.UserId,
                    Status = -2,
                    ClientId = 0

                };

                var retVal = ClientService.LoadClients(searchObj, userData.Username);
                if (retVal?.Status == null)
                {
                    return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
                }

                if (!retVal.Status.IsSuccessful)
                {
                    return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
                }

                if (!retVal.Clients.Any())
                {
                    return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
                }

                var parentTabs = retVal.Clients.Where(c => c.Status == 1).OrderBy(c => c.ClientId);
                add = new NameValueObject { Id = 0, Name = "-- Select Client  --" };

                var jsonitem = parentTabs.Select(o => new NameValueObject { Id = o.ClientId, Name = o.ClientName }).ToList();
                jsonitem.Insert(0, add);
                return Json(jsonitem, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult LoadProducts()
        {
            var add = new NameValueObject { Id = 0, Name = "-- Empty Product List --" };
            try
            {
                var userData = MvcApplication.GetUserData(User.Identity.Name);
                if (userData == null || userData.UserId < 1)
                {
                    return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
                }

                var searchObj = new ProductSearchObj
                {
                    AdminUserId = userData.UserId,
                    Status = -2,
                    ClientId = 0,
                    ProductId = 0

                };

                var retVal = ProductService.LoadProducts(searchObj, userData.Username);
                if (retVal?.Status == null)
                {
                    return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
                }

                if (!retVal.Status.IsSuccessful)
                {
                    return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
                }

                if (!retVal.Products.Any())
                {
                    return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
                }

                var parentTabs = retVal.Products.Where(c => c.Status == 1).OrderBy(c => c.ClientId);
                add = new NameValueObject { Id = 0, Name = "-- Select Product --" };

                var jsonitem = parentTabs.Select(o => new NameValueObject { Id = o.ProductId, Name = o.Name }).ToList();
                jsonitem.Insert(0, add);
                return Json(jsonitem, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult LoadProductsByClientId(int clientId)
        {
            var add = new NameValueObject { Id = 0, Name = "-- Empty Product List --" };
            try
            {
                var userData = MvcApplication.GetUserData(User.Identity.Name);
                if (userData == null || userData.UserId < 1)
                {
                    return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
                }

                var searchObj = new ProductSearchObj
                {
                    AdminUserId = userData.UserId,
                    Status = -2,
                    ClientId = 0,
                    ProductId = 0

                };

                var retVal = ProductService.LoadProducts(searchObj, userData.Username);
                if (retVal?.Status == null)
                {
                    return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
                }

                if (!retVal.Status.IsSuccessful)
                {
                    return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
                }

                if (!retVal.Products.Any())
                {
                    return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
                }

                var parentTabs = retVal.Products.Where(c => c.Status == 1 && c.ClientId==clientId).OrderBy(c => c.ClientId);
                add = new NameValueObject { Id = 0, Name = "-- Select Product --" };

                var jsonitem = parentTabs.Select(o => new NameValueObject { Id = o.ProductId, Name = o.Name }).ToList();
                jsonitem.Insert(0, add);
                return Json(jsonitem, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
            } 
        }
        public ActionResult LoadCurrentImplementation()
        {
            var add = new NameValueObject { Id = 0, Name = "-- Empty List --" };
            try
            {
                var instantTestType = typeof(CurrentImplementation).ToNameValueList();
                if (instantTestType == null || !instantTestType.Any())
                {
                    return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
                }
                var parentTabs = instantTestType.OrderBy(c => c.Id);
                add = new NameValueObject { Id = 0, Name = "-- Select Item --" };

                var jsonitem = parentTabs.Select(o => new NameValueObject { Id = o.Id, Name = o.Name }).ToList();
                jsonitem.Insert(0, add);
                return Json(jsonitem, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult LoadUserClients()
        {
            var add = new NameValueObject { Id = 0, Name = "-- Empty Client List --" };
            try
            {
                var userData = MvcApplication.GetUserData(User.Identity.Name);
                if (userData == null || userData.UserId < 1)
                {
                    return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
                }

                if (userData.ClientProductList == null || !userData.ClientProductList.Any())
                {
                    return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
                }

                var clients = new List<ClientObj>();
                foreach (var item in userData.ClientProductList)
                {
                    clients.Add(item.ClientInfo);
                }
               
                if (!clients.Any())
                {
                    return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
                }

                var parentTabs = clients.Where(c => c.Status == 1).OrderBy(c => c.ClientId);
                add = new NameValueObject { Id = 0, Name = "-- Select Client  --" };

                var jsonitem = parentTabs.Select(o => new NameValueObject { Id = o.ClientId, Name = o.ClientName }).ToList();
                jsonitem.Insert(0, add);
                return Json(jsonitem, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult LoadUserClientProducts(int clientId)
        {
            var add = new NameValueObject { Id = 0, Name = "-- Empty Client List --" };
            try
            {
                var userData = MvcApplication.GetUserData(User.Identity.Name);
                if (userData == null || userData.UserId < 1)
                {
                    return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
                }

                if (userData.ClientProductList == null || !userData.ClientProductList.Any())
                {
                    return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
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
                    return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
                }

                var parentTabs = clients.Where(c => c.Status == 1).OrderBy(c => c.ProductId);
                add = new NameValueObject { Id = 0, Name = "-- Select Product  --" };

                var jsonitem = parentTabs.Select(o => new NameValueObject { Id = o.ProductId, Name = o.Name }).ToList();
                jsonitem.Insert(0, add);
                return Json(jsonitem, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult LoadUserClientProductItems(int clientId, int productId)
        {
            //var add = new NameValueObject { Id = 0, Name = "-- Empty Client List --" };
            var add = new NameValueObject { Id = 0, Name = "-- Empty Product Item List --" };
            try
            {
                var userData = MvcApplication.GetUserData(User.Identity.Name);
                if (userData == null || userData.UserId < 1)
                {
                    return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
                }

                if (userData.ClientProductList == null || !userData.ClientProductList.Any())
                {
                    return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
                }

                var clients = new List<ProductItemObj>();
                foreach (var item in userData.ClientProductList)
                {
                    if (item.ClientInfo.ClientId == clientId)
                    {
                        var prodItemList = item.Products.Find(m => m.ProductInfo.ProductId == productId );
                        if (prodItemList != null && prodItemList.ProductInfo.ProductId > 0)
                        {
                            clients.AddRange(prodItemList.ProductItems);
                            break;
                        }

                    }
                }

                if (!clients.Any())
                {
                    return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
                }

                var parentTabs = clients.Where(c => c.Status == 1).OrderBy(c => c.ProductId);
                add = new NameValueObject { Id = 0, Name = "-- Select Product  --" };

                var jsonitem = parentTabs.Select(o => new NameValueObject { Id = o.ProductItemId, Name = o.Name }).ToList();
                jsonitem.Insert(0, add);
                return Json(jsonitem, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult LoadNewAlertProviders()
        {
            var add = new NameValueObject { Id = 0, Name = "-- Empty Provider List --" };
            try
            {
                var userData = MvcApplication.GetUserData(User.Identity.Name);
                if (userData == null || userData.UserId < 1)
                {
                    return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
                }

                var searchObj = new AlertProviderSearchObj
                {
                    AdminUserId = userData.UserId,
                };

                var retVal = AlertProviderService.LoadNewAlertProviders(searchObj, userData.Username);
                if (retVal?.Status == null)
                {
                    return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
                }

                if (!retVal.Status.IsSuccessful)
                {
                    return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
                }

                if (!retVal.AlertProviders.Any())
                {
                    return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
                }

                var parentTabs = retVal.AlertProviders.Where(c => c.Status == 1).OrderBy(c => c.AlertProviderId);
                add = new NameValueObject { Id = 0, Name = "-- Select Provider  --" };

                var jsonitem = parentTabs.Select(o => new NameValueObject { Id = o.AlertProviderId, Name = o.Name }).ToList();
                jsonitem.Insert(0, add);
                return Json(jsonitem, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
            }

        }
    }

}
