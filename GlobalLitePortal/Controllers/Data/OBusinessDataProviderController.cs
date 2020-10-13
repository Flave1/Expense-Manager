using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using GlobalLitePortalHelper.APICore;
using GlobalLitePortalHelper.APIObjs;
using GlobalLitePortalHelper.APIObjs.Business;
using GlobalLitePortalHelper.APIServices;
using PluglexHelper.PortalManager;
using PluglexHelper.PortalObjs;
using PlugLitePortalHelper.APIObjs;
using PlugLitePortalHelper.APIServices;
using XPLUG.WEBTOOLKIT;

namespace GlobalLitePortal.Controllers.Data
{
    public class OBusinessDataProviderController : Controller
    {
        public OBusinessDataProviderController()
        {
        }
         

        public ActionResult LoadAlertItems()
        {
            var add = new NameValueObject { Id = 0, Name = "-- Empty Item List --" };
            try
            {
                var userData = MvcApplication.GetUserData(User.Identity.Name);
                if (userData == null || userData.UserId < 1)
                {
                    return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
                }

                var searchObj = new AlertItemSearchObj
                {
                    AdminUserId = userData.UserId,
                    Status = 1,
                };

                var retVal = AlertItemService.LoadAlertItems(searchObj, userData.Username);
                if (retVal?.Status == null)
                {
                    return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
                }

                if (!retVal.Status.IsSuccessful)
                {
                    return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
                }

                if (!retVal.AlertItems.Any())
                {
                    return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
                }

                var parentTabs = retVal.AlertItems.Where(c => c.Status == 1).OrderBy(c => c.Name);
                add = new NameValueObject { Id = 0, Name = "-- Select Item  --" };

                var jsonitem = parentTabs.Select(o => new NameValueObject { Id = o.AlertItemId, Name = o.Name }).ToList();
                jsonitem.Insert(0, add);
                return Json(jsonitem, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
            }

        }
        
        public ActionResult LoadAlertProviders()
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
                    Status = 1,
                };

                var retVal = AlertProviderService.LoadAlertProviders(searchObj, userData.Username);
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

                var parentTabs = retVal.AlertProviders.Where(c => c.Status == 1).OrderBy(c => c.Name);
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
        
        public ActionResult LoadSMSProviders()
        {
            var add = new NameValueObject { Id = 0, Name = "-- Empty Provider List --" };
            try
            {
                var userData = MvcApplication.GetUserData(User.Identity.Name);
                if (userData == null || userData.UserId < 1)
                {
                    return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
                }

                var searchObj = new SMSProviderSearchObj
                {
                    AdminUserId = userData.UserId,
                    Status = 1,
                };

                var retVal = SMSProviderService.LoadSMSProviders(searchObj, userData.Username);
                if (retVal?.Status == null)
                {
                    return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
                }

                if (!retVal.Status.IsSuccessful)
                {
                    return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
                }

                if (!retVal.SMSProviders.Any())
                {
                    return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
                }

                var parentTabs = retVal.SMSProviders.Where(c => c.Status == 1).OrderBy(c => c.Name);
                add = new NameValueObject { Id = 0, Name = "-- Select Provider  --" };

                var jsonitem = parentTabs.Select(o => new NameValueObject { Id = o.SMSProviderId, Name = o.Name }).ToList();
                jsonitem.Insert(0, add);
                return Json(jsonitem, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
            }

        }
        
        public ActionResult LoadSMSRoutes()
        {
            var add = new NameValueObject { Id = 0, Name = "-- Empty Provider List --" };
            try
            {
                var userData = MvcApplication.GetUserData(User.Identity.Name);
                if (userData == null || userData.UserId < 1)
                {
                    return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
                }

                var searchObj = new SMSRouteSearchObj
                {
                    AdminUserId = userData.UserId,
                    Status = 1,
                };

                var retVal = SMSRouteService.LoadSMSRoutes(searchObj, userData.Username);
                if (retVal?.Status == null)
                {
                    return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
                }

                if (!retVal.Status.IsSuccessful)
                {
                    return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
                }

                if (!retVal.SMSRoutes.Any())
                {
                    return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
                }

                var parentTabs = retVal.SMSRoutes.Where(c => c.Status == 1).OrderBy(c => c.Name);
                add = new NameValueObject { Id = 0, Name = "-- Select Routes  --" };

                var jsonitem = parentTabs.Select(o => new NameValueObject { Id = o.SMSRouteId, Name = o.Name }).ToList();
                jsonitem.Insert(0, add);
                return Json(jsonitem, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
            }

        }
        
        public ActionResult LoadAlertNetworks()
        {
            var add = new NameValueObject { Id = 0, Name = "-- Empty Provider List --" };
            try
            {
                var userData = MvcApplication.GetUserData(User.Identity.Name);
                if (userData == null || userData.UserId < 1)
                {
                    return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
                }

                var searchObj = new AlertNetworkSearchObj
                {
                    AdminUserId = userData.UserId,
                    Status = 1,
                };

                var retVal = AlertNetworkService.LoadAlertNetworks(searchObj, userData.Username);
                if (retVal?.Status == null)
                {
                    return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
                }

                if (!retVal.Status.IsSuccessful)
                {
                    return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
                }

                if (!retVal.AlertNetworks.Any())
                {
                    return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
                }

                var parentTabs = retVal.AlertNetworks.Where(c => c.Status == 1).OrderBy(c => c.Name);
                add = new NameValueObject { Id = 0, Name = "-- Select Network  --" };

                var jsonitem = parentTabs.Select(o => new NameValueObject { Id = o.AlertNetworkId, Name = o.Name }).ToList();
                jsonitem.Insert(0, add);
                return Json(jsonitem, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
            }

        }
        

        public ActionResult LoadDepartments(int? placeValueNeeded)
        {

            var B = User.Identity.Name;
            var add = new NameValueObject { Id = 0, Name = "Empty List" };
            try
            {
                var userData = MvcApplication.GetUserData(User.Identity.Name);
                if (userData == null || userData.UserId < 1)
                {
                    return Json(JsonRequestBehavior.AllowGet);
                }

                var searchObj = new DepartmentSearchObj
                {
                    AdminUserId = userData.UserId,
                    DepartmentId = 0,
                    Status = -2,
                    StopDate = "",
                    StartDate = ""
                };


                var retVal = ExpenseLookUpServices.LoadDepartments(searchObj, userData.Username);
                if (retVal?.Status == null)
                {
                    return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
                }

                if (!retVal.Status.IsSuccessful)
                {
                    return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
                }

                if (!retVal.Departments.Any())
                {
                    return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
                }

                var parentTabs = retVal.Departments.Where(c => c.Status == 1).OrderBy(c => c.DepartmentId);
                //  add = new NameValueObject { Id = 0, Name = "-- Select Department  --" };

                var jsonitem = parentTabs.Select(o => new NameValueObject { Id = o.DepartmentId, Name = o.Name }).ToList();
                if (placeValueNeeded > 0)
                {
                    jsonitem.Insert(0, add);
                    return Json(jsonitem, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(jsonitem, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
            }

        } 
        public ActionResult LoadExpenseCategories(int? placeValueNeed)
        {
            var add = new NameValueObject { Id = 0, Name = " Empty List " };
            try
            {
                var userData = MvcApplication.GetUserData(User.Identity.Name);
                if (userData == null || userData.UserId < 1)
                {
                    return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
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
                    return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
                }

                if (!retVal.Status.IsSuccessful)
                {
                    return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
                }

                if (!retVal.ExpenseCategories.Any())
                {
                    return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
                }

                var parentTabs = retVal.ExpenseCategories.Where(c => c.Status == 1).OrderBy(c => c.ExpenseCategoryId);
                add = new NameValueObject { Id = 0, Name = "-- Select ExpenseCategory  --" };

                var jsonitem = parentTabs.Select(o => new NameValueObject { Id = o.ExpenseCategoryId, Name = o.Name }).ToList();
                if (placeValueNeed > 0)
                {
                    jsonitem.Insert(0, add);
                    return Json(jsonitem, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(jsonitem, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
            }

        }
        
        public ActionResult LoadExpenseClassifications(int? categoryId, int? clientId, int? productId)
        {
            var add = new NameValueObject { Id = 0, Name = "Empty List" };
            try
            {
                var userData = MvcApplication.GetUserData(User.Identity.Name);
                if (userData == null || userData.UserId < 1)
                {
                    return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
                }

                var searchObj = new ExpenseClassificationSearchObj
                {
                    AdminUserId = userData.UserId,
                    Status = 1,
                    ExpenseClassificationId = 0

                };

                var retValFrmClientDB = new FilteredLookupSearchObj
                {
                    AdminUserId = userData.UserId,
                    ProductId = productId??0,
                    ClientId = clientId??0,

                };

                var responseFrmClientDb = ExpenseLookUpServices.LoadFilteredClassifications(retValFrmClientDB, userData.Username);
                var retVal = ExpenseLookUpServices.LoadClassifications(searchObj, userData.Username);
                if (retVal?.Status == null)
                {
                    return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
                }

                if (!retVal.Status.IsSuccessful)
                {
                    return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
                }

                if (!retVal.ExpenseClassifications.Any())
                {
                    return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
                }
                add = new NameValueObject { Id = 0, Name = "-- Select Classification  --" };
                List<ExpenseClassificationObj> parentTabs = null;
                List<ExpenseClassificationObj> notAddedClassifications = null;
                List<NameValueObject> jsonitem = null;

                parentTabs = retVal.ExpenseClassifications.Where(c => c.Status == 1 && c.ExpenseCategoryId == categoryId).ToList();

                var listOfClassesification = responseFrmClientDb.ExpenseClassifications.Where(c => c.Status == 1).ToList();
                if (categoryId > 0)
                {
                    
                    //foreach (var addedClassification in listOfClassesification)
                    //{
                        
                    //      notAddedClassifications = parentTabs.Where(c => c.ExpenseClassificationId != addedClassification.ExpenseClassificationId).ToList();
                    //      //notAddedClassifications = parentTabs.Where(c => c.ExpenseClassificationId != addedClassification.ExpenseClassificationId).ToList();

                    //}
                     jsonitem = parentTabs.Select(o => new NameValueObject { Id = o.ExpenseClassificationId, Name = o.Name }).ToList();

                    return Json(jsonitem, JsonRequestBehavior.AllowGet);
                }
                else
                {
                     parentTabs = retVal.ExpenseClassifications.Where(c => c.Status == 1).OrderBy(c => c.ExpenseCategoryId).ToList();

                     jsonitem = parentTabs.Select(o => new NameValueObject { Id = o.ExpenseClassificationId, Name = o.Name }).ToList();
                    return Json(jsonitem, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
            }

        }
        
        public ActionResult LoadExpenseItems(int? categoryId, int? classificationId, int? reqPlaceHolder)
        {
            var add = new NameValueObject { Id = 0, Name = " Empty List " };
            try
            {
                var userData = MvcApplication.GetUserData(User.Identity.Name);
                if (userData == null || userData.UserId < 1)
                {
                    return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
                }

                var searchObj = new ExpenseItemSearchObj
                {
                    AdminUserId = userData.UserId,
                    Status = -2,
                    ExpenseItemId = 0
                };

                var retVal = ExpenseLookUpServices.LoadExpenseItems(searchObj, userData.Username);
                if (retVal?.Status == null)
                {
                    return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
                }

                if (!retVal.Status.IsSuccessful)
                {
                    return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
                }

                if (!retVal.ExpenseItems.Any())
                {
                    return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
                }

                add = new NameValueObject { Id = 0, Name = "-- Select ExpenseItem  --" };
                if (categoryId > 0 && classificationId > 0)
                {

                    var parentTabs = retVal.ExpenseItems.Where(c => c.Status == 1 && c.ExpenseCategoryId == categoryId && c.ExpenseClassificationId == classificationId).OrderBy(c => c.ExpenseItemId);
                    var jsonitem = parentTabs.Select(o => new NameValueObject { Id = o.ExpenseItemId, Name = o.Name }).ToList();
                    if(reqPlaceHolder > 0)
                    {
                        jsonitem.Insert(0, add);
                    }  
                    return Json(jsonitem, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var parentTabs = retVal.ExpenseItems.Where(c => c.Status == 1);
                    var jsonitem = parentTabs.Select(o => new NameValueObject { Id = o.ExpenseItemId, Name = o.Name }).ToList();
                    return Json(jsonitem, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
            }

        }
        
        public ActionResult LoadExpenseTypes(int? placeValueNeed)
        {
            var add = new NameValueObject { Id = 0, Name = "Empty List" };
            try
            {
                var userData = MvcApplication.GetUserData(User.Identity.Name);
                if (userData == null || userData.UserId < 1)
                {
                    return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
                }

                var searchObj = new ExpenseTypeSearchObj
                {
                    AdminUserId = userData.UserId,
                    Status = -2,
                    ExpenseTypeId = 0

                };

                var retVal = ExpenseLookUpServices.LoadExpenseTypes(searchObj, userData.Username);
                if (retVal?.Status == null)
                {
                    return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
                }

                if (!retVal.Status.IsSuccessful)
                {
                    return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
                }

                if (!retVal.ExpenseTypes.Any())
                {
                    return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
                }

                add = new NameValueObject { Id = 0, Name = "-- Select ExpenseType  --" }; 
                var parentTabs = retVal.ExpenseTypes.Where(c => c.Status == 1).OrderBy(c => c.ExpenseTypeId);
                var jsonitem = parentTabs.Select(o => new NameValueObject { Id = o.ExpenseTypeId, Name = o.Name }).ToList();

                if (placeValueNeed > 0)
                {
                    jsonitem.Insert(0, add);
                    return Json(jsonitem, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(jsonitem, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
            }
        }
        
        public ActionResult LoadWorkflowApprovalLevels()
        {
            var add = new NameValueObject { Id = 0, Name = "Empty List" };
            try
            {
                var userData = MvcApplication.GetUserData(User.Identity.Name);
                if (userData == null || userData.UserId < 1)
                {
                    return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
                }

                var searchObj = new WorkFlowApprovalLevelSearchObj
                {
                    AdminUserId = userData.UserId,
                    Status = -2,
                    WorkFlowApprovalLevelId = 0
                };

                var retVal = ExpenseLookUpServices.LoadWorkFlowApprovalLevels(searchObj, userData.Username);
                if (retVal?.Status == null)
                {
                    return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
                }

                if (!retVal.Status.IsSuccessful)
                {
                    return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
                }

                if (!retVal.WorkflowApprovalLevels.Any())
                {
                    return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
                }

                var parentTabs = retVal.WorkflowApprovalLevels.Where(c => c.Status == 1).OrderBy(c => c.WorkFlowApprovalLevelId);
                add = new NameValueObject { Id = 0, Name = "-- Select WorkFlowApprovalLevel  --" };

                var jsonitem = parentTabs.Select(o => new NameValueObject { Id = o.WorkFlowApprovalLevelId, Name = o.Name }).ToList();
                //jsonitem.Insert(0, add);
                return Json(jsonitem, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
            }

        }
        
        public ActionResult LoadWorkflowRoles( )
        {
            var add = new NameValueObject { Id = 0, Name = "Empty List" };
            try
            {
                var userData = MvcApplication.GetUserData(User.Identity.Name);
                if (userData == null || userData.UserId < 1)
                {
                    return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
                }

                var searchObj = new WorkFlowRoleSearchObj
                {
                    AdminUserId = userData.UserId,
                    Status = -2,
                    WorkFlowRoleId = 0

                };

                var retVal = ExpenseLookUpServices.LoadWorkFlowRoles(searchObj, userData.Username);
                if (retVal?.Status == null)
                {
                    return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
                }

                if (!retVal.Status.IsSuccessful)
                {
                    return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
                }

                if (!retVal.WorkflowRoles.Any())
                {
                    return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
                }

                var parentTabs = retVal.WorkflowRoles.Where(c => c.Status == 1).OrderBy(c => c.WorkFlowRoleId);
                //add = new NameValueObject { Id = 0, Name = "-- Select WorkFlowRole  --" };

                var jsonitem = parentTabs.Select(o => new NameValueObject { Id = o.WorkFlowRoleId, Name = o.Name }).ToList();
                //jsonitem.Insert(0, add);
                return Json(jsonitem, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
            }
        }
     
        public ActionResult LoadWorkFlowStages()
        {
            var add = new NameValueObject { Id = 0, Name = "Empty List" };
            try
            {
                var userData = MvcApplication.GetUserData(User.Identity.Name);
                if (userData == null || userData.UserId < 1)
                {
                    return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
                }

                var searchObj = new WorkFlowStageSearchObj
                {
                    AdminUserId = userData.UserId,
                    Status = -2,
                    WorkFlowStageId = 0

                };

                var retVal = ExpenseLookUpServices.LoadWorkFlowStages(searchObj, userData.Username);
                if (retVal?.Status == null)
                {
                    return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
                }

                if (!retVal.Status.IsSuccessful)
                {
                    return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
                }

                if (!retVal.WorkflowStages.Any())
                {
                    return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
                }

                var parentTabs = retVal.WorkflowStages.Where(c => c.Status == 1).OrderBy(c => c.WorkFlowStageId);
                //add = new NameValueObject { Id = 0, Name = "-- Select WorkFlowStage  --" }; 
                var jsonitem = parentTabs.Select(o => new NameValueObject { Id = o.WorkFlowStageId, Name = o.Name }).ToList();
                //jsonitem.Insert(0, add);
                return Json(jsonitem, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
            } 
        }
        
        public ActionResult LoadBeneficiaries(int beneficiaryId)
        {
            var add = new NameValueObject { Id = 0, Name = "Empty List" };
            try
            {
                var userData = MvcApplication.GetUserData(User.Identity.Name);
                if (userData == null || userData.UserId < 1)
                {
                    return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
                }

                AppSession userClientSession = (AppSession)Session["_UserClientSession_"];
                if (userClientSession == null || userClientSession.ClientId < 1 || userClientSession.ProductId < 1 || userClientSession.ProductItemId < 1)
                {
                    return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
                }
                var clientId = userClientSession.ClientId;
                var productId = userClientSession.ProductId; 
                var searchObj = new BeneficiarySearchObj
                {
                    AdminUserId = userData.UserId,
                    Status = -2,
                    BeneficiaryId = 0 
                };

                var retVal = BeneficiaryServices.LoadBeneficiaries(searchObj, userData.Username);
                if (retVal?.Status == null)
                {
                    return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
                }

                if (!retVal.Status.IsSuccessful)
                {
                    return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
                }

                if (!retVal.Beneficiaries.Any())
                {
                    return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
                }

                IEnumerable<BeneficiaryObj> parentTabs = retVal.Beneficiaries.OrderBy(c => c.BeneficiaryId )
                                    .Where(c => c.BeneficiaryType == beneficiaryId  ).ToList();
                add = new NameValueObject { Id = 0, Name = "--  Beneficiaries  --" };

                var jsonitem = parentTabs.Select(o => new NameValueObject { Id = o.BeneficiaryId, Name = o.FirstName + " " + o.LastName }).ToList();
                jsonitem.Insert(0, add); 
                return Json(jsonitem, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
            }
        }
        
        public ActionResult LoadRequiredExpenseLookupEnums(int? clientId, int? productId, int? productItemId)
        {
            var add = new NameValueObject { Id = 0, Name = "Empty List" };
            try
            {
                var userData = MvcApplication.GetUserData(User.Identity.Name);
                if (userData == null || userData.UserId < 1)
                {
                    return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
                }
                var searchObj = new ExpenseLookupSearchObj
                {
                    AdminUserId = userData.UserId,
                    ExpenseLookupId = 0,
                    Status = -2
                };
                var retVal = ExpenseLookUpServices.LoadExpenseLookups(searchObj, userData.Username);
                if (retVal?.Status == null)
                {
                    return Json(new List<NameValueObject> {add}, JsonRequestBehavior.AllowGet);
                }

                if (!retVal.Status.IsSuccessful)
                {
                    return Json(new List<NameValueObject> {add}, JsonRequestBehavior.AllowGet);
                }
                if (retVal.ExpenseLookups == null || !retVal.ExpenseLookups.Any())
                {
                    return Json(new List<NameValueObject> {add}, JsonRequestBehavior.AllowGet);
                }
                var expenseLookups = retVal.ExpenseLookups.OrderBy(m => m.ExpenseLookupId)
                    .Where(m => m.ClientId == clientId && m.ProductId == productId  && m.ProductItemId == productItemId).ToList();

                add = new NameValueObject { Id = 0, Name = "-- Select  lookup Item here--" };

               var jsonitem = new List<NameValueObject>();

               jsonitem = Enum.GetValues(typeof(ExpenseLookupItems)).Cast<ExpenseLookupItems>().Select(t => new NameValueObject { Id = ((int)t), Name = t.ToString() }).ToList();
                   foreach (var addedItems in expenseLookups){
                            jsonitem = jsonitem.Where(m => m.Name.Trim().ToStandardHash() != addedItems.LookupName.Trim().ToStandardHash()).ToList();
                   }
                   jsonitem.Insert(0, add);
                return Json(jsonitem.Where(m=>m.Id == (int)ExpenseLookupItems.ExpenseCategory && m.Id == (int)ExpenseLookupItems.ExpenseClassification && m.Id == (int)ExpenseLookupItems.ExpenseItem), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult LoadExpenseLookupEnums(int? clientId, int? productId, int? productItemId)
        {
            var add = new NameValueObject { Id = 0, Name = "Empty List" };
            add = new NameValueObject { Id = 0, Name = "-- Select  lookup Item here--" };

            var jsonitem = new List<NameValueObject>();

            try
            {
                var userData = MvcApplication.GetUserData(User.Identity.Name);
                if (userData == null || userData.UserId < 1)
                {
                    return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
                }
                var searchObj = new ExpenseLookupSearchObj
                {
                    AdminUserId = userData.UserId,
                    ExpenseLookupId = 0,
                    Status = -2
                };
                var retVal = ExpenseLookUpServices.LoadExpenseLookups(searchObj, userData.Username);
                if (retVal?.Status == null)
                {
                    jsonitem = Enum.GetValues(typeof(ExpenseLookupItems)).Cast<ExpenseLookupItems>().Select(t => new NameValueObject { Id = ((int)t), Name = t.ToString() }).ToList();

                    return Json(new List<NameValueObject> {add}, JsonRequestBehavior.AllowGet);
                }

                if (!retVal.Status.IsSuccessful)
                {
                    jsonitem.Insert(0, add);
                    jsonitem = Enum.GetValues(typeof(ExpenseLookupItems)).Cast<ExpenseLookupItems>().Select(t => new NameValueObject { Id = ((int)t), Name = t.ToString() }).ToList();
                    return Json(jsonitem, JsonRequestBehavior.AllowGet);
                    //return Json(new List<NameValueObject> {add}, JsonRequestBehavior.AllowGet);
                }
                if (retVal.ExpenseLookups == null || !retVal.ExpenseLookups.Any())
                {
                    return Json(new List<NameValueObject> {add}, JsonRequestBehavior.AllowGet);
                }
                var expenseLookups = retVal.ExpenseLookups.OrderBy(m => m.ExpenseLookupId)
                    .Where(m => m.ClientId == clientId && m.ProductId == productId  && m.ProductItemId == productItemId).ToList();

              
               jsonitem = Enum.GetValues(typeof(ExpenseLookupItems)).Cast<ExpenseLookupItems>().Select(t => new NameValueObject { Id = ((int)t), Name = t.ToString() }).ToList();
                   foreach (var addedItems in expenseLookups){
                            jsonitem = jsonitem.Where(m => m.Name.Trim().ToStandardHash() != addedItems.LookupName.Trim().ToStandardHash()).ToList();
                   }
                   jsonitem.Insert(0, add);
                return Json(jsonitem.Where(m => m.Id != 2 && m.Id != 3), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
            }

        }
        
        public ActionResult LoadAppUsers(int? clientId, int? productId)
        {

            var add = new NameValueObject { Id = 0, Name = "Empty List" };
            try
            {
                var userData = MvcApplication.GetUserData(User.Identity.Name);
                if (userData == null || userData.UserId < 1)
                {
                    return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
                }
                var searchObj2 = new UserSearchObj
                {
                    AdminUserId = userData.UserId,
                    Status = -2,
                    StopDate = "",
                    StartDate = "",
                    UserId = 0,

                };
                var retVal = new PortalUserManager().LoadUsers(searchObj2, userData.Username);

                var searchObj = new ClientUserSearchObj
                {
                    AdminUserId = userData.UserId,
                    ClientId = clientId ?? 0,
                    ProductId = productId??0
                };

                var retValForClientUsers = ClientService.LoadClientUsers(searchObj, userData.Username); 

                if (retVal?.Status == null)
                {
                    return Json(new List<NameValueObject> {add}, JsonRequestBehavior.AllowGet);
                }

                if (!retVal.Status.IsSuccessful)
                {
                    return Json(new List<NameValueObject> {add}, JsonRequestBehavior.AllowGet);
                }
                var allUsers = retVal.Users.OrderBy(m => m.UserId).ToList(); 
                //var clientUsers = retValForClientUsers.ClientUsers.OrderBy(m => m.ClientUserId).ToList(); 

                 var clientUsers2 = retValForClientUsers.ClientUsers.OrderBy(m => m.ClientUserId).Select(x => x.UserId).ToList();

                var jsonitem = allUsers.Where(x => clientUsers2.Contains(x.UserId)).Select(x => 
                new NameValueObject { Id = x.UserId, Name = x.FirstName + " " + x.LastName }).ToList();

                //List<NameValueObject> jsonitem = (from allU in allUsers
                //                 join clientU in clientUsers on allU.UserId equals clientU.UserId
                //                 where allU.UserId == clientU.UserId
                //                 select new NameValueObject { Id = clientU.UserId, Name = allU.FirstName + " " + allU.LastName }).ToList();


                add = new NameValueObject { Id = 0, Name = "-- Select Users  --" };

                jsonitem.Insert(0, add);
                return Json(jsonitem, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
            }

        }
        
        public ActionResult LoadRequestTypeEnums(int? clientId, int? productId, int? productItemId)
        {
            var add = new NameValueObject { Id = 0, Name = "Empty List" };

            var jsonitem = new List<NameValueObject>();

            jsonitem = Enum.GetValues(typeof(RequestType)).Cast<RequestType>().Select(t => new NameValueObject { Id = ((int)t), Name = t.ToString() }).ToList();

            try
            {
                var userData = MvcApplication.GetUserData(User.Identity.Name);
                if (userData == null || userData.UserId < 1)
                {
                    return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
                }
                var searchObj = new WorkflowSettingSearchObj
                {
                    AdminUserId = userData.UserId,
                    WorkflowSettingId = 0,
                    Status = 1,
                    StartDate = "",
                    StopDate = "",
                };
                var retVal = WorkflowSettingServices.LoadWorkflowSettings(searchObj, userData.Username);
                if (retVal?.Status == null)
                {
                    
                    return Json(new List<NameValueObject> {add}, JsonRequestBehavior.AllowGet);
                }

                if (!retVal.Status.IsSuccessful)
                {
                    //jsonitem.Insert(0, add);
                    return Json(jsonitem, JsonRequestBehavior.AllowGet);
                    //return Json(new List<NameValueObject> {add}, JsonRequestBehavior.AllowGet);
                }
                if (retVal.WorkflowSettings == null || !retVal.WorkflowSettings.Any())
                {
                    return Json(new List<NameValueObject> {add}, JsonRequestBehavior.AllowGet);
                }
                var workFlowList = retVal.WorkflowSettings.OrderBy(m => m.WorkflowSettingId)
                    .Where(m => m.ClientId == clientId && m.ProductId == productId  && m.ProductItemId == productItemId).ToList();

                add = new NameValueObject { Id = 0, Name = "-- Select Request type here--" };


                foreach (var addedItems in workFlowList)
               {
                   jsonitem = jsonitem.Where(m => m.Id != addedItems.RequestType).ToList();
               }
                jsonitem.Insert(0, add);
                return Json(jsonitem, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
            }

        }
        
        public ActionResult LoadBeneficiaryTypeEnums(int? clientId, int? productId, int? productItemId)
        {
            var add = new NameValueObject { Id = 0, Name = "Empty List" };
            try
            {
                var userData = MvcApplication.GetUserData(User.Identity.Name);
                if (userData == null || userData.UserId < 1)
                {
                    return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
                }
              
                add = new NameValueObject { Id = 0, Name = "-- Select Request type here--" };

               var jsonitem = new List<NameValueObject>();

               jsonitem = Enum.GetValues(typeof(BeneficiaryType)).Cast<BeneficiaryType>().Select(t => new NameValueObject { Id = ((int)t), Name = t.ToString() }).ToList();
               
                jsonitem.Insert(0, add);
                return Json(jsonitem, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
            }

        }


        public ActionResult LoadCurrentUser()
        {
            var add = new NameValueObject { Id = 0, Name = "-- Empty Client List --" };
            try
            {
                var userData = MvcApplication.GetUserData(User.Identity.Name);
                if (userData == null || userData.UserId < 1)
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


        /////++++++++++++++++++++Filtered lookups

        public ActionResult LoadFilteredDepartments(int clientId, int productId)
        {

            var B = User.Identity.Name;
            var add = new NameValueObject { Id = 0, Name = "Empty List" };
            try
            {
                var userData = MvcApplication.GetUserData(User.Identity.Name);
                if (userData == null || userData.UserId < 1)
                {
                    return Json(JsonRequestBehavior.AllowGet);
                }

                var searchObj = new FilteredLookupSearchObj
                {
                    AdminUserId = userData.UserId,
                    ProductId = productId,
                    ClientId = clientId,

                };


                var retVal = ExpenseLookUpServices.LoadFilteredDepartments(searchObj, userData.Username);
                if (retVal?.Status == null)
                {
                    return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
                }

                if (!retVal.Status.IsSuccessful)
                {
                    return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
                }

                if (!retVal.Departments.Any())
                {
                    return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
                }

                var parentTabs = retVal.Departments.Where(c => c.Status == 1).OrderBy(c => c.DepartmentId);
                add = new NameValueObject { Id = 0, Name = "-- Select Department  --" };

                var jsonitem = parentTabs.Select(o => new NameValueObject { Id = o.DepartmentId, Name = o.Name }).ToList();
                jsonitem.Insert(0, add);
                    return Json(jsonitem, JsonRequestBehavior.AllowGet);
                

            }
            catch (Exception ex)
            {
                UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
            }

        }
        public ActionResult LoadFilteredExpenseCategories(int clientId, int productId)
        {
            var add = new NameValueObject { Id = 0, Name = " Empty List " };
            try
            {
                var userData = MvcApplication.GetUserData(User.Identity.Name);
                if (userData == null || userData.UserId < 1)
                {
                    return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
                }

                var searchObj = new FilteredLookupSearchObj
                {
                    AdminUserId = userData.UserId,
                    ProductId = productId,
                    ClientId = clientId,

                };

                var retVal = ExpenseLookUpServices.LoadFilteredExpenseCategories(searchObj, userData.Username);
                if (retVal?.Status == null)
                {
                    return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
                }

                if (!retVal.Status.IsSuccessful)
                {
                    return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
                }

                if (!retVal.ExpenseCategories.Any())
                {
                    return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
                }

                var parentTabs = retVal.ExpenseCategories.Where(c => c.Status == 1).OrderBy(c => c.ExpenseCategoryId);
                add = new NameValueObject { Id = 0, Name = "-- Select Expense Category  --" };

                var jsonitem = parentTabs.Select(o => new NameValueObject { Id = o.ExpenseCategoryId, Name = o.Name }).ToList();


                jsonitem.Insert(0, add);
                return Json(jsonitem, JsonRequestBehavior.AllowGet);


            }
            catch (Exception ex)
            {
                UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
            }

        }
        
        public ActionResult LoadExpenseItemSettings(int clientId, int productId, int expenseItemId)
        {
            var add = new NameValueObject { Id = 0, Name = " Empty List " };
            try
            {
                var userData = MvcApplication.GetUserData(User.Identity.Name);
                if (userData == null || userData.UserId < 1)
                {
                    return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
                }

                var searchObj = new ExpenseItemSettingSearchObj
                {
                    AdminUserId = userData.UserId,
                    Status = 1,
                    ExpenseItemSettingId = 0,

                };
                
                var retVal = ExpenseItemSettingServices.LoadExpenseItemSettings(searchObj, userData.Username);
                var searchItemObj = new FilteredLookupSearchObj
                {
                    ClientId = clientId,
                    ProductId = productId,
                    AdminUserId = userData.UserId,
                }; 

                var retValForItems = ExpenseLookUpServices.LoadFilteredExpenseItems(searchItemObj, userData.Username);
                if (retVal?.Status == null)
                {
                    return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
                }

                if (!retVal.Status.IsSuccessful)
                {
                    return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
                }

                if (!retVal.ExpenseItemSettings.Any())
                {
                    return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
                }
                if (!retValForItems.ExpenseItems.Any())
                {
                    return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
                }
                var expenseItems = retValForItems.ExpenseItems.Where(c => c.Status == 1).OrderBy(c => c.ExpenseItemId).ToList();

                var expenseItemSettings = retVal.ExpenseItemSettings.Where(c => c.Status == 1).OrderBy(c => c.ExpenseItemSettingId).ToList();

                var currentItemProperties = expenseItemSettings.FirstOrDefault(m => m.ExpenseItemId == expenseItemId && m.ClientId == clientId && m.ProductId ==productId); 
               
                //List<NameValueObject> finalList =
                //    (from exi in expenseItems
                //    join exis in expenseItemSettings  on exi.ExpenseItemId equals exis.ExpenseItemId
                //    where exi.ExpenseItemId == exis.ExpenseItemId
                //    select new NameValueObject { Id = Convert.ToInt32(exis.ExpenseItemSettingId), Name = exi.Name }).ToList();

                return Json(currentItemProperties, JsonRequestBehavior.AllowGet);


            }
            catch (Exception ex)
            {
                UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult LoadFilteredExpenseClassifications(int clientId, int productId, int categoryId)
        {
            var add = new NameValueObject { Id = 0, Name = "Empty List" };
            try
            {
                var userData = MvcApplication.GetUserData(User.Identity.Name);
                if (userData == null || userData.UserId < 1)
                {
                    return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
                }

                var searchObj = new FilteredLookupSearchObj
                {
                    AdminUserId = userData.UserId,
                    ProductId = productId,
                    ClientId = clientId,

                };

                var retVal = ExpenseLookUpServices.LoadFilteredClassifications(searchObj, userData.Username);
                if (retVal?.Status == null)
                {
                    return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
                }

                if (!retVal.Status.IsSuccessful)
                {
                    return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
                }

                if (!retVal.ExpenseClassifications.Any())
                {
                    return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
                }

                add = new NameValueObject { Id = 0, Name = "-- Select Classification  --" };  
                
                var parentTabs = retVal.ExpenseClassifications.Where(c => c.Status == 1 && c.ExpenseCategoryId == categoryId).OrderBy(c => c.ExpenseCategoryId); 
                
                var jsonitem = parentTabs.Select(o => new NameValueObject { Id = o.ExpenseClassificationId, Name = o.Name }).ToList();
               
                jsonitem.Insert(0, add);
                return Json(jsonitem, JsonRequestBehavior.AllowGet);  
            }
            catch (Exception ex)
            {
                UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult LoadFilteredExpenseItems(int? clientId, int? productId, int? categoryId, int? classificationId, int? typeId, int? report)
        {
            var add = new NameValueObject { Id = 0, Name = " Empty List " };
            try
            {
                var userData = MvcApplication.GetUserData(User.Identity.Name);
                if (userData == null || userData.UserId < 1)
                {
                    return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
                }

                var searchObj = new FilteredLookupSearchObj
                {
                    AdminUserId = userData.UserId,
                    ProductId = productId??0,
                    ClientId = clientId??0,

                };


                var retVal = ExpenseLookUpServices.LoadFilteredExpenseItems(searchObj, userData.Username);
                if (retVal?.Status == null)
                {
                    return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
                }

                if (!retVal.Status.IsSuccessful)
                {
                    return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
                }

                if (!retVal.ExpenseItems.Any())
                {
                    return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
                }

                add = new NameValueObject { Id = 0, Name = "-- Select Expense Item  --" };
                IEnumerable<ExpenseItemObj> parentTabs;
                if (report == 10101)
                {
                    parentTabs = retVal.ExpenseItems.Where(c => c.Status == 1);
                }
                else
                {
                    parentTabs = retVal.ExpenseItems.Where(c => c.Status == 1 && c.ExpenseCategoryId == categoryId && c.ExpenseClassificationId == classificationId && c.ExpenseTypeId == typeId);
                }
                

                    var jsonitem = parentTabs.Select(o => new NameValueObject { Id = o.ExpenseItemId, Name = o.Name }).ToList();
                    jsonitem.Insert(0, add);
                return Json(jsonitem, JsonRequestBehavior.AllowGet);
                

            }
            catch (Exception ex)
            {
                UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult LoadFilteredExpenseTypes(int clientId, int productId)
        {
            var add = new NameValueObject { Id = 0, Name = "Empty List" };
            try
            {
                var userData = MvcApplication.GetUserData(User.Identity.Name);
                if (userData == null || userData.UserId < 1)
                {
                    return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
                }

                var searchObj = new FilteredLookupSearchObj
                {
                    AdminUserId = userData.UserId,
                    ProductId = productId,
                    ClientId = clientId,

                };

                var retVal = ExpenseLookUpServices.LoadFilteredExpenseTypes(searchObj, userData.Username);
                if (retVal?.Status == null)
                {
                    return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
                }

                if (!retVal.Status.IsSuccessful)
                {
                    return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
                }

                if (!retVal.ExpenseTypes.Any())
                {
                    return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
                }

                var parentTabs = retVal.ExpenseTypes.Where(c => c.Status == 1).OrderBy(c => c.ExpenseTypeId);
                add = new NameValueObject { Id = 0, Name = "-- Select Expense Type  --" };

                var jsonitem = parentTabs.Select(o => new NameValueObject { Id = o.ExpenseTypeId, Name = o.Name }).ToList();
                jsonitem.Insert(0, add);
                return Json(jsonitem, JsonRequestBehavior.AllowGet);
                
            }
            catch (Exception ex)
            {
                UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult LoadFilteredWorkflowStages(int clientId, int productId)
        {
            var add = new NameValueObject { Id = 0, Name = "Empty List" };
            try
            {
                var userData = MvcApplication.GetUserData(User.Identity.Name);
                if (userData == null || userData.UserId < 1)
                {
                    return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
                }

                var searchObj = new FilteredLookupSearchObj
                {
                    AdminUserId = userData.UserId,
                    ProductId = productId,
                    ClientId = clientId,

                };


                var retVal = ExpenseLookUpServices.LoadFilteredWorkFlowStages(searchObj, userData.Username);
                if (retVal?.Status == null)
                {
                    return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
                }

                if (!retVal.Status.IsSuccessful)
                {
                    return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
                }

                if (!retVal.WorkflowStages.Any())
                {
                    return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
                }

                var parentTabs = retVal.WorkflowStages.Where(c => c.Status == 1 && c.Name == "Initiation").OrderBy(c => c.WorkFlowStageId);
                add = new NameValueObject { Id = 0, Name = "-- Select Workflow Stage  --" };

                var jsonitem = parentTabs.Select(o => new NameValueObject { Id = o.WorkFlowStageId, Name = o.Name }).ToList();
                //jsonitem.Insert(0, add);
                return Json(jsonitem, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
            }

        }
 
        public ActionResult LoadConfiguredItems(int clientId, int productId, int categoryId, int classificationId, int typeId)
        {
            var add = new NameValueObject { Id = 0, Name = "Please Settup Items " };
            try
            {
                var userData = MvcApplication.GetUserData(User.Identity.Name);
                if (userData == null || userData.UserId < 1)
                {
                    return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
                }

                var searchObj = new ExpenseItemSettingSearchObj
                {
                    AdminUserId = userData.UserId, 
                    Status = 1,
                    ExpenseItemSettingId = 0,

                };


                var retVal = ExpenseItemSettingServices.LoadExpenseItemSettings(searchObj, userData.Username);

                var searchItemObj = new FilteredLookupSearchObj
                {
                    ClientId = clientId,
                    ProductId = productId,
                    AdminUserId = userData.UserId,
                };

                var retValForItems = ExpenseLookUpServices.LoadFilteredExpenseItems(searchItemObj, userData.Username);

                if (retVal?.Status == null)
                {
                    return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
                }

                if (!retVal.Status.IsSuccessful)
                {
                    return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
                }

                if (!retVal.ExpenseItemSettings.Any())
                {
                    return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
                }

                var parentTabs = retVal.ExpenseItemSettings.Where(c => c.Status == 1).OrderBy(c => c.ExpenseItemSettingId);
                add = new NameValueObject { Id = 0, Name = "-- Select Items  --" };

                var expenseItems = retValForItems.ExpenseItems.Where(c => c.Status == 1).OrderBy(c => c.ExpenseItemId).ToList();

                var expenseItemSettings = retVal.ExpenseItemSettings.Where(c => c.Status == 1 && c.ProductId == productId 
                        && c.ClientId == clientId
                        && c.ExpenseCategoryId ==categoryId
                        && c.ExpenseClassificationId == classificationId
                        && c.ExpenseTypeId == typeId)
                        .OrderBy(c => c.ExpenseItemSettingId);


                List<NameValueObject> jsonitem =
                    (from exi in expenseItems
                     join exis in expenseItemSettings on exi.ExpenseItemId equals exis.ExpenseItemId
                     where exi.ExpenseItemId == exis.ExpenseItemId
                     select new NameValueObject { Id = Convert.ToInt32(exi.ExpenseItemId), Name = exi.Name }).ToList();

                 
                jsonitem.Insert(0, add);
                return Json(jsonitem, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
            }

        }
 
        public ActionResult LoadFilteredWorkflowApprovalLevels(int clientId, int productId)
        {
            var add = new NameValueObject { Id = 0, Name = "Empty List" };
            try
            {
                var userData = MvcApplication.GetUserData(User.Identity.Name);
                if (userData == null || userData.UserId < 1)
                {
                    return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
                }

                var searchObj = new FilteredLookupSearchObj
                {
                    AdminUserId = userData.UserId,
                    ProductId = productId,
                    ClientId = clientId,

                };


                var retVal = ExpenseLookUpServices.LoadFilteredWorkFlowApprovalLevels(searchObj, userData.Username);
                if (retVal?.Status == null)
                {
                    return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
                }

                if (!retVal.Status.IsSuccessful)
                {
                    return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
                }

                if (!retVal.WorkflowApprovalLevels.Any())
                {
                    return Json(new List<NameValueObject> { add }, JsonRequestBehavior.AllowGet);
                }

                var parentTabs = retVal.WorkflowApprovalLevels.Where(c => c.Status == 1).OrderBy(c => c.WorkFlowApprovalLevelId);
                add = new NameValueObject { Id = 0, Name = "-- Select WorkFlowApprovalLevel  --" };

                var jsonitem = parentTabs.Select(o => new NameValueObject { Id = o.WorkFlowApprovalLevelId, Name = o.Name }).ToList();
                //jsonitem.Insert(0, add);
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