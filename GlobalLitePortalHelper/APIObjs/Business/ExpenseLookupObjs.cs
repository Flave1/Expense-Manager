using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 
using XPLUG.WEBTOOLKIT;

namespace PlugLitePortalHelper.APIObjs
{ 
        public class RegExpenseLookupObj :AdminObj
        { 

            [CheckNumber(0, ErrorMessage = "Client Id is required")]
            public int ClientId { get; set; }
            [CheckNumber(0, ErrorMessage = "ProductItem Id is required")]
            public int ProductItemId { get; set; }

            [CheckNumber(0, ErrorMessage = "Product Id is required")]
            public int ProductId { get; set; }

            [CheckNumber(0, ErrorMessage = "Channel Code is required")]
            public string ChannelCode { get; set; }

            [Column(TypeName = "varchar")]
            [StringLength(50, MinimumLength = 3, ErrorMessage = "LookupName must be between 3 and 50 characters")]
            [Required(AllowEmptyStrings = false, ErrorMessage = "LookupName is required")]
            public string LookupName { get; set; }

            [CheckNumber(0, ErrorMessage = "Request type is required")]
            public int LookupItem { get; set; }

            public bool IsEnabled { get; set; }

            [Column(TypeName = "varchar")]
            [StringLength(300, MinimumLength = 0, ErrorMessage = "InclusionList must be between 3 and 50 characters")]
            //[Required(AllowEmptyStrings = false, ErrorMessage = "InclusionList is required")]
            public string InclusionList { get; set; }

            [Column(TypeName = "varchar")]
            [StringLength(300, MinimumLength = 0, ErrorMessage = "ExclusionList must be between 3 and 50 characters")]
            //[Required(AllowEmptyStrings = false, ErrorMessage = "ExclusionList is required")]
            public string ExclusionList { get; set; }

            [CheckNumber(0, ErrorMessage = "Registered By Id  is required")]
            public int RegisteredBy { get; set; }

            [DisplayName("Time Stamp Registered")] public string TimeStampRegistered { get; set; }

            [CheckNumber(0, ErrorMessage = "Expense Status is required")]
            public int Status { get; set; }
            public bool StatusVal { get; set; }

            public  string ClientName { get; set; }
            public  string ProductName { get; set; }
            public  string ProductItemName { get; set; }
            
        
        
            //public ExpenseCategoryObj ExpenseCategoryObj { get; set; }
            //public ExpenseClassificationObj ExpenseClassificationObj { get; set; }
            //public ExpenseItemObj ExpenseItemObj { get; set; }
    }

    
        public class EditExpenseLookupObj :AdminObj
        {
            [CheckNumber(0, ErrorMessage = "ExpenseLookupId Id is required")]
        public int ExpenseLookupId { get; set; }



            [CheckNumber(0, ErrorMessage = "ProductItem Id is required")]
            public int ProductItemId { get; set; }

            [CheckNumber(0, ErrorMessage = "Client Id is required")]
            public int ClientId { get; set; }

            [CheckNumber(0, ErrorMessage = "Product Id is required")]
            public int ProductId { get; set; }

            [CheckNumber(0, ErrorMessage = "Channel Code is required")]
            public string ChannelCode { get; set; }

            [Column(TypeName = "varchar")]
            [StringLength(50, MinimumLength = 3, ErrorMessage = "LookupName must be between 3 and 50 characters")]
            [Required(AllowEmptyStrings = false, ErrorMessage = "LookupName is required")]
            public string LookupName { get; set; }

            [CheckNumber(0, ErrorMessage = "Request type is required")]
            public int LookupItem { get; set; }

            public bool IsEnabled { get; set; }

            [Column(TypeName = "varchar")]
            [StringLength(300, MinimumLength = 0, ErrorMessage = "InclusionList must be between 3 and 50 characters")]
            [Required(AllowEmptyStrings = false, ErrorMessage = "InclusionList is required")]
            public string InclusionList { get; set; }

            [Column(TypeName = "varchar")]
            [StringLength(300, MinimumLength = 0, ErrorMessage = "ExclusionList must be between 3 and 50 characters")]
            [Required(AllowEmptyStrings = false, ErrorMessage = "ExclusionList is required")]
            public string ExclusionList { get; set; }

            [CheckNumber(0, ErrorMessage = "Registered By Id  is required")]
            public int RegisteredBy { get; set; }

            [DisplayName("Time Stamp Registered")] public string TimeStampRegistered { get; set; }

            [CheckNumber(0, ErrorMessage = "Expense Status is required")]
            public int Status { get; set; }
            public bool StatusVal { get; set; }
    }


        public class ExpenseLookupObj
        { 
            public int ProductItemId { get; set; }
            public int ExpenseLookupId { get; set; }
            public int ClientId { get; set; }
            public int ProductId { get; set; }
            public string ChannelCode { get; set; }
            public string LookupName { get; set; }
            public int LookupItem { get; set; }
            public bool IsEnabled { get; set; }
            public string InclusionList { get; set; }
            public string ExclusionList { get; set; }
            public int RegisteredBy { get; set; }
            public int Status { get; set; }
            public bool StatusVal { get; set; }
            public string StatusLabel { get; set; }
         


            public int CurrentClientId { get; set; }
            public int CurrentProductId { get; set; }
            public int CurrentProductItemId { get; set; }
    }


        public class ExpenseLookupRespObj
        {
            public List<ExpenseLookupObj> ExpenseLookups { get; set; }
            public APIResponseStatus Status { get; set; }
        }
        public class ExpenseLookupRegRespObj
    {
            public int ExpenseLookupId { get; set; }
            public APIResponseStatus Status { get; set; }
        }

    public class ExpenseLookupSearchObj : AdminObj
        {
            public int ExpenseLookupId { get; set; }
            public int Status { get; set; }
        }
  } 