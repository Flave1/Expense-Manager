
namespace GlobalLitePortalHelper.APICore
{
    internal class APIEndpoints
    {

        //Product
        internal const string ADD_PRODUCT_ENDPOINT = "/PortalManager/PortalService/AddProduct";
        internal const string UPDATE_PRODUCT_ENDPOINT = "/PortalManager/PortalService/UpdateProduct";
        internal const string DELETE_PRODUCT_ENDPOINT = "/PortalManager/PortalService/DeleteProduct";
        internal const string LOAD_PRODUCTS_ENDPOINT = "/PortalManager/PortalService/LoadProducts";


        //Product Item
        internal const string ADD_PRODUCT_ITEM_ENDPOINT = "/PortalManager/PortalService/AddProductItem";
        internal const string UPDATE_PRODUCT_ITEM_ENDPOINT = "/PortalManager/PortalService/UpdateProductItem";
        internal const string DELETE_PRODUCT_ITEM_ENDPOINT = "/PortalManager/PortalService/DeleteProductItem";
        internal const string LOAD_PRODUCT_ITEMS_ENDPOINT = "/PortalManager/PortalService/LoadProductItems";

        //Client
        internal const string ADD_CLIENT_ENDPOINT = "/PortalManager/PortalService/AddClient";
        internal const string DELETE_CLIENT_ENDPOINT = "/PortalManager/PortalService/DeleteClient";
        internal const string LOAD_CLIENTS_ENDPOINT = "/PortalManager/PortalService/LoadClients";
        internal const string UPDATE_CLIENT_ENDPOINT = "/PortalManager/PortalService/UpdateClientItem";
        internal const string LOAD_CLIENT_PRODUCTS_ENDPOINT = "/PortalManager/PortalService/LoadClientProductList";

        internal const string ADD_CLIENT_USER_ENDPOINT = "/PortalManager/PortalService/AddClientUser";
        internal const string DELETE_CLIENT_USER_ENDPOINT = "/PortalManager/PortalService/DeleteClientUser";
        internal const string LOAD_CLIENT_USERS_ENDPOINT = "/PortalManager/PortalService/LoadClientUsers";

        //*******************************************************************************************************************

        //Alert Item
        internal const string ADD_ALERT_ITEM_ENDPOINT = "/LRGlobalLookup/AddAlertItem";
        internal const string DELETE_ALERT_ITEM_ENDPOINT = "/LRGlobalLookup/DeleteAlertItem"; 
        internal const string LOAD_ALERT_ITEMS_ENDPOINT = "/LRGlobalLookup/LoadAlertItems";
        internal const string UPDATE_ALERT_ITEM_ENDPOINT = "/LRGlobalLookup/EditAlertItem";

        //Alert Provider Item
        internal const string ADD_ALERT_PROVIDER_ITEM_ENDPOINT = "/LRGlobalLookup/AddAlertProviderItem";
        internal const string UPDATE_ALERT_PROVIDER_ITEM_ENDPOINT = "/LRGlobalLookup/EditAlertProviderItem";
        internal const string DELETE_ALERT_PROVIDER_ITEM_ENDPOINT = "/LRGlobalLookup/DeleteAlertProviderItem";
        internal const string LOAD_ALERT_PROVIDER_ITEMS_ENDPOINT = "/LRGlobalLookup/LoadAlertProviderItems";
        

        //Alert Provider
        internal const string ADD_ALERT_PROVIDER_ENDPOINT = "/LRGlobalLookup/AddAlertProvider";
        internal const string UPDATE_ALERT_PROVIDER_ENDPOINT = "/LRGlobalLookup/EditAlertProvider";
        internal const string DELETE_ALERT_PROVIDER_ENDPOINT = "/LRGlobalLookup/DeleteAlertProvider";
        internal const string LOAD_ALERT_PROVIDERS_ENDPOINT = "/LRGlobalLookup/LoadAlertProviders";
        internal const string LOAD_NEW_ALERT_PROVIDERS_ENDPOINT = "/LRGlobalLookup/LoadNewAlertProviders";

        
        //Alert Network
        internal const string ADD_ALERT_NETWORK_ENDPOINT = "/LRGlobalLookup/AddAlertNetwork";
        internal const string UPDATE_ALERT_NETWORK_ENDPOINT = "/LRGlobalLookup/EditAlertNetwork";
        internal const string DELETE_ALERT_NETWORK_ENDPOINT = "/LRGlobalLookup/DeleteAlertNetwork";
        internal const string LOAD_ALERT_NETWORKS_ENDPOINT = "/LRGlobalLookup/LoadAlertNetworks";
        

        //Alert Route
        internal const string ADD_ALERT_ROUTE_ENDPOINT = "/LRGlobalLookup/AddAlertRoute";
        internal const string UPDATE_ALERT_ROUTE_ENDPOINT = "/LRGlobalLookup/EditAlertRoute";
        internal const string DELETE_ALERT_ROUTE_ENDPOINT = "/LRGlobalLookup/DeleteAlertRoute";
        internal const string LOAD_ALERT_ROUTES_ENDPOINT = "/LRGlobalLookup/LoadAlertRoutes";
        
        //Alert Schedule
        internal const string ADD_ALERT_SCHEDULE_ENDPOINT = "/LRGlobalLookup/AddAlertSchedule";
        internal const string UPDATE_ALERT_SCHEDULE_ENDPOINT = "/LRGlobalLookup/EditAlertSchedule";
        internal const string DELETE_ALERT_SCHEDULE_ENDPOINT = "/LRGlobalLookup/DeleteAlertSchedule";
        internal const string LOAD_ALERT_SCHEDULES_ENDPOINT = "/LRGlobalLookup/LoadAlertSchedules";

        //Alert Type
        internal const string ADD_ALERT_TYPE_ENDPOINT = "/LRGlobalLookup/AddAlertType";
        internal const string UPDATE_ALERT_TYPE_ENDPOINT = "/LRGlobalLookup/EditAlertType";
        internal const string DELETE_ALERT_TYPE_ENDPOINT = "/LRGlobalLookup/DeleteAlertType";
        internal const string LOAD_ALERT_TYPES_ENDPOINT = "/LRGlobalLookup/LoadAlertTypes";

        //SMS Provider
        internal const string ADD_SMS_PROVIDER_ENDPOINT = "/LRGlobalLookup/AddSMSProvider";
        internal const string UPDATE_SMS_PROVIDER_ENDPOINT = "/LRGlobalLookup/EditSMSProvider";
        internal const string DELETE_SMS_PROVIDER_ENDPOINT = "/LRGlobalLookup/DeleteSMSProvider";
        internal const string LOAD_SMS_PROVIDERS_ENDPOINT = "/LRGlobalLookup/LoadSMSProviders";

        //SMS Route
        internal const string ADD_SMS_ROUTE_ENDPOINT = "/LRGlobalLookup/AddSMSRoute";
        internal const string UPDATE_SMS_ROUTE_ENDPOINT = "/LRGlobalLookup/EditSMSRoute";
        internal const string DELETE_SMS_ROUTE_ENDPOINT = "/LRGlobalLookup/DeleteSMSRoute";
        internal const string LOAD_SMS_ROUTES_ENDPOINT = "/LRGlobalLookup/LoadSMSRoutes";

        //Lookup Stats
        internal const string LOAD_LOOKUP_STATS_ENDPOINT = "/LRGlobalLookup/LoadLookupStats";

        //EXPENSE EXPAT
         
        //Expense 
        internal const string ADD_EXPENSE_LOOKUP_ENDPOINT = "/main/BusinessService/GlobalCloudStackAdmin/AddToExpenseLookup";
        internal const string UPDATE_EXPENSE_LOOKUP_ENDPOINT = "/main/BusinessService/GlobalCloudStackAdmin/UpdateExpenseLookup";
        internal const string LOAD_EXPENSES_LOOKUP_ENDPOINT = "/main/BusinessService/GlobalCloudStackAdmin/LoadExpenseLookups";

        //Expense 
        internal const string ADD_EXPENSE_ENDPOINT = "/main/BusinessService/GlobalCloudStackAdmin/AddToExpense";
        internal const string UPDATE_EXPENSE_ENDPOINT = "/main/BusinessService/GlobalCloudStackAdmin/UpdateExpense";
        internal const string LOAD_EXPENSES_ENDPOINT = "/main/BusinessService/GlobalCloudStackAdmin/LoadExpenses";

        //Expense Item Settings
        internal const string ADD_EXPENSE_ITEM_SETTING_ENDPOINT = "/main/BusinessService/GlobalCloudStackAdmin/AddToExpenseItemSetting";
        internal const string UPDATE_EXPENSE_ITEM_SETTING_ENDPOINT = "/main/BusinessService/GlobalCloudStackAdmin/UpdateExpenseItemSetting";
        internal const string LOAD_EXPENSE_ITEM_SETTINGS_ENDPOINT = "/main/BusinessService/GlobalCloudStackAdmin/LoadExpenseItemSettings";

        //Expense  Requisition
        internal const string ADD_EXPENSE_REQUISITION_ENDPOINT = "/main/BusinessService/GlobalCloudStackAdmin/AddToExpenseRequisition";
        internal const string UPDATE_EXPENSE_REQUISITION_ENDPOINT = "/main/BusinessService/GlobalCloudStackAdmin/UpdateExpenseRequisition";
        internal const string LOAD_EXPENSE_REQUISITIONS_ENDPOINT = "/main/BusinessService/GlobalCloudStackAdmin/LoadExpenseRequisitions";

        //Expense Requisition Item  
        internal const string ADD_EXPENSE_REQUISITION_ITEM_ENDPOINT = "/main/BusinessService/BusinessSetupAdmin/AddToExpenseRequisitionItem";
        internal const string UPDATE_EXPENSE_REQUISITION_ITEM_ENDPOINT = "/main/BusinessService/BusinessSetupAdmin/UpdateExpenseRequisitionItem";
        internal const string LOAD_EXPENSE_REQUISITION_ITEMS_ENDPOINT = "/main/BusinessService/GlobalCloudStackAdmin/LoadExpenseRequisitionItems";

        //Work Flow Detail
        internal const string ADD_WORKFLOW_DETAIL_ENDPOINT = "/main/BusinessService/GlobalCloudStackAdmin/AddToWorkflowDetail";
        internal const string UPDATE_WORKFLOW_DETAIL_ENDPOINT = "/main/BusinessService/GlobalCloudStackAdmin/UpdateWorkflowDetail";
        internal const string LOAD_WORKFLOW_DETAILS_ENDPOINT = "/main/BusinessService/GlobalCloudStackAdmin/LoadWorkflowDetails";

        //Expense Workflow Item  
        internal const string ADD_WORKFLOW_ITEM_ENDPOINT = "/main/BusinessService/BusinessSetupAdmin/AddToWorkflowItem";
        internal const string UPDATE_WORKFLOW_ITEM_ENDPOINT = "/main/BusinessService/BusinessSetupAdmin/UpdateWorkflowItem";
        internal const string LOAD_WORKFLOW_ITEMS_ENDPOINT = "/main/BusinessService/GlobalCloudStackAdmin/LoadWorkflowItems";

        //Expense Work flow settings
        internal const string ADD_WORKFLOW_SETTING_ENDPOINT = "/main/BusinessService/GlobalCloudStackAdmin/AddToWorkflowSetting";
        internal const string UPDATE_WORKFLOW_SETTING_ENDPOINT = "/main/BusinessService/GlobalCloudStackAdmin/UpdateWorkflowSetting";
        internal const string LOAD_WORKFLOW_SETTINGS_ENDPOINT = "/main/BusinessService/GlobalCloudStackAdmin/LoadWorkflowSettings";

        //Expense Work flow Task
        internal const string ADD_WORKFLOW_TASK_ENDPOINT = "/main/BusinessService/GlobalCloudStackAdmin/AddToWorkflowTask";
        internal const string UPDATE_WORKFLOW_TASK_ENDPOINT = "/main/BusinessService/GlobalCloudStackAdmin/UpdateWorkflowTask";
        internal const string LOAD_WORKFLOW_TASKS_ENDPOINT = "/main/BusinessService/GlobalCloudStackAdmin/LoadWorkflowTasks";
        
        //Beneficiaries
        internal const string ADD_BENEFICIARY_ENDPOINT = "/main/BusinessService/GlobalCloudStackAdmin/AddToBeneficiary";
        internal const string UPDATE_BENEFICIARY_ENDPOINT = "/main/BusinessService/GlobalCloudStackAdmin/UpdateBeneficiary";
        internal const string LOAD_BENEFICIARY_ENDPOINT = "/main/BusinessService/GlobalCloudStackAdmin/LoadBeneficiaries";

        //DISBURSEMENT
        internal const string ADD_DISBURSEMENT_ENDPOINT = "/main/BusinessService/GlobalCloudStackAdmin/AddToDisbursement";
        internal const string UPDATE_DISBURSEMENT_ENDPOINT = "/main/BusinessService/GlobalCloudStackAdmin/UpdateDisbursement";
        internal const string LOAD_DISBURSEMENT_ENDPOINT = "/main/BusinessService/GlobalCloudStackAdmin/LoadDisbursements";

        //DEPARTMENT USER
        internal const string ADD_DEPARTMENT_USERS_ENDPOINT = "/main/BusinessService/GlobalCloudStackAdmin/AddToDepartmentUser";
        internal const string UPDATE_DEPARTMENT_USERS_ENDPOINT = "/main/BusinessService/GlobalCloudStackAdmin/UpdateDepartmentUser";
        internal const string LOAD_DEPARTMENT_USER_ENDPOINT = "/main/BusinessService/GlobalCloudStackAdmin/LoadDepartmentUsers";
    
        //Filtered LKPS
        //WORK FLOW ROLE 
        internal const string LOAD_FILTERED_WORK_FLOW_ROLE_ENDPOINT = "/main/BusinessService/GlobalCloudStackAdmin/LoadFilteredWorkFlowRoles";
        //WORK FLOW STAGE 
        internal const string LOAD_FILTERED_WORK_FLOW_STAGE_ENDPOINT = "/main/BusinessService/GlobalCloudStackAdmin/LoadFilteredWorkFlowStages";
        //WORK FLOW APPROVAL LEVEL 
        internal const string LOAD_FILTERED_WORK_FLOW_APPROVAL_LEVEL_ENDPOINT = "/main/BusinessService/GlobalCloudStackAdmin/LoadFilteredWorkFlowApprovalLevels";
        //DEPARTMENT 
        internal const string LOAD_FILTERED_DEPARTMENT_ENDPOINT = "/main/BusinessService/GlobalCloudStackAdmin/LoadFilteredDepartments";
        //EXPENSE CATEGORY 
        internal const string LOAD_FILTERED_EXPENSE_CATEGORY_ENDPOINT = "/main/BusinessService/GlobalCloudStackAdmin/LoadFilteredExpenseCategories";
        //EXPENSE CLASSIFICATION 
        internal const string LOAD_FILTERED_EXPENSE_CLASSIFICATIONS_ENDPOINT = "/main/BusinessService/GlobalCloudStackAdmin/LoadFilteredExpenseClassifications";
        //EXPENSE TYPE 
        internal const string LOAD_FILTERED_EXPENSE_TYPE_ENDPOINT = "/main/BusinessService/GlobalCloudStackAdmin/LoadFilteredExpenseTypes";
        //EXPENSE ITEM 
        internal const string LOAD_FILTERED_EXPENSE_ITEM_ENDPOINT = "/main/BusinessService/GlobalCloudStackAdmin/LoadFilteredExpenseItems";

        internal const string LOAD_FILTERED_DEPARTMENTS_ENDPOINT = "/main/BusinessService/GlobalCloudStackAdmin/LoadFilteredDepartments";

        //LKPS 
        ////WORK FLOW ROLE 
        //internal const string LOAD_WORK_FLOW_ROLE_ENDPOINT = "/lookup/BizService/BizLookupAdmin/LoadWorkflowRoles";
        ////WORK FLOW STAGE 
        //internal const string LOAD_WORK_FLOW_STAGE_ENDPOINT = "/lookup/BizService/BizLookupAdmin/LoadWorkflowStages";
        ////WORK FLOW APPROVAL LEVEL 
        //internal const string LOAD_WORK_FLOW_APPROVAL_LEVEL_ENDPOINT = "/lookup/BizService/BizLookupAdmin/LoadWorkflowApprovalLevels";
        ////DEPARTMENT 
        //internal const string LOAD_DEPARTMENT_ENDPOINT = "/lookup/BizService/BizLookupAdmin/LoadDepartments";
        ////EXPENSE CATEGORY 
        //internal const string LOAD_EXPENSE_CATEGORY_ENDPOINT = "/lookup/BizService/BizLookupAdmin/LoadExpenseCategorys";
        ////EXPENSE CLASSIFICATION 
        //internal const string LOAD_EXPENSE_CLASSIFICATIONS_ENDPOINT = "/lookup/BizService/BizLookupAdmin/LoadExpenseClassifications";
        ////EXPENSE TYPE 
        //internal const string LOAD_EXPENSE_TYPE_ENDPOINT = "/lookup/BizService/BizLookupAdmin/LoadExpenseTypes";
        ////EXPENSE ITEM 
        //internal const string LOAD_EXPENSE_ITEM_ENDPOINT = "/lookup/BizService/BizLookupAdmin/LoadExpenseItems";

        //internal const string LOAD_DEPARTMENTS_ENDPOINT = "/lookup/BizService/BizLookupAdmin/LoadDepartments";

        //WORK FLOW ROLE 
        internal const string LOAD_WORK_FLOW_ROLE_ENDPOINT = "/lookup/BizService/BizLookupAdmin/CheckAndGetLatestWorkflowRoles";
        //WORK FLOW STAGE 
        internal const string LOAD_WORK_FLOW_STAGE_ENDPOINT = "/lookup/BizService/BizLookupAdmin/CheckAndGetLatestWorkflowStages";
        //WORK FLOW APPROVAL LEVEL 
        internal const string LOAD_WORK_FLOW_APPROVAL_LEVEL_ENDPOINT = "/lookup/BizService/BizLookupAdmin/CheckAndGetLatestWorkflowApprovalLevels";
        //DEPARTMENT 
        internal const string LOAD_DEPARTMENT_ENDPOINT = "/lookup/BizService/BizLookupAdmin/CheckAndGetLatestDepartments";
        //EXPENSE CATEGORY 
        internal const string LOAD_EXPENSE_CATEGORY_ENDPOINT = "/lookup/BizService/BizLookupAdmin/CheckAndGetLatestExpenseCategorys";
        //EXPENSE CLASSIFICATION 
        internal const string LOAD_EXPENSE_CLASSIFICATIONS_ENDPOINT = "/lookup/BizService/BizLookupAdmin/CheckAndGetLatestExpenseClassifications";
        //EXPENSE TYPE 
        internal const string LOAD_EXPENSE_TYPE_ENDPOINT = "/lookup/BizService/BizLookupAdmin/CheckAndGetLatestExpenseTypes";
        //EXPENSE ITEM 
        internal const string LOAD_EXPENSE_ITEM_ENDPOINT = "/lookup/BizService/BizLookupAdmin/CheckAndGetLatestExpenseItems";

        internal const string LOAD_DEPARTMENTS_ENDPOINT = "/lookup/BizService/BizLookupAdmin/CheckAndGetLatestDepartments";
        
        internal const string LOAD_DASHBOARD_STATS_ENDPOINT = "/main/BusinessService/GlobalCloudStackAdmin/LoadAppStats";
       
        internal const string ADD_RETIREMENT_ENDPOINT = "/main/BusinessService/GlobalCloudStackAdmin/AddToRetirement";
        internal const string LOAD_RETIREMENTS_ENDPOINT = "/main/BusinessService/GlobalCloudStackAdmin/LoadRetirements";

        //REQUEST TYPE SETTINGS
        internal const string ADD_REQUEST_TYPE_SETTINGS_ENDPOINT = "/main/BusinessService/GlobalCloudStackAdmin/AddToRequestTypeSetting";
        internal const string UPDATE_REQUEST_TYPE_SETTINGS_ENDPOINT = "/main/BusinessService/GlobalCloudStackAdmin/UpdateRequestTypeSetting";
        internal const string LOAD_REQUEST_TYPE_SETTINGS_ENDPOINT = "/main/BusinessService/GlobalCloudStackAdmin/LoadRequestTypeSettings";

        internal const string GENERATE_VOUCHER_NUMBER_ENDPOINT = "/main/BusinessService/GlobalCloudStackAdmin/GetVoucheNumber";
    }
}
