using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using XPLUG.WEBTOOLKIT;

namespace GlobalLitePortalHelper.APIObjs
{
    public class RegProductItemObj : AdminObj
    {

        [CheckNumber(0, ErrorMessage = "Client Information is required")]
        public int ClientId { get; set; }

        [CheckNumber(0, ErrorMessage = "Client Information is required")]
        public int ProductId { get; set; }

        public int ProductItemId { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Product Item name is required")]
        [StringLength(150, MinimumLength = 2, ErrorMessage = "Product Item name must be between 2 and 200 characters")]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Product Item Title is required")]
        [StringLength(300, MinimumLength = 2, ErrorMessage = "Product Item Title must be between 2 and 300 characters")]
        public string Title { get; set; }

        [Required(AllowEmptyStrings = true, ErrorMessage = "Product Item Description is required")]
        [StringLength(300, MinimumLength = 0, ErrorMessage = "Product Item Description must be between 0 and 300 characters")]
        public string Description { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Live API URL is required")]
        [StringLength(300, MinimumLength = 5, ErrorMessage = "Live API URL must be between 5 and 300 characters")]
        public string LiveAPIUrl { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Sandbox API URL is required")]
        [StringLength(300, MinimumLength = 5, ErrorMessage = "Sandbox API URL must be between 5 and 300 characters")]
        public string SandBoxAPIUrl { get; set; }
        public int CurrentImplementation { get; set; }
        public bool DisplaySummaryStat { get; set; }
        public bool DisplayDemographicStat { get; set; }
        public bool DisplayLocationStat { get; set; }
        public bool HasOtherStat { get; set; }
        public int Status { get; set; }

        public string ClientName { get; set; }
        public string ProductName { get; set; }
    }


    public class EditProductItemObj : AdminObj
    {
        [CheckNumber(0, ErrorMessage = "ProductItemId is required")]
        public int ProductItemId { get; set; }

        [CheckNumber(0, ErrorMessage = "Client Information is required")]
        public int ProductId { get; set; }

        [CheckNumber(0, ErrorMessage = "Client Information is required")]
        public int ClientId { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "ProductItem name is required")]
        [StringLength(150, MinimumLength = 2, ErrorMessage = "ProductItem name must be between 2 and 200 characters")]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "ProductItem Title is required")]
        [StringLength(300, MinimumLength = 2, ErrorMessage = "ProductItem Title must be between 2 and 300 characters")]
        public string Title { get; set; }

        [Required(AllowEmptyStrings = true, ErrorMessage = "ProductItem Description is required")]
        [StringLength(300, MinimumLength = 0, ErrorMessage = "ProductItem Description must be between 0 and 300 characters")]
        public string Description { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Live API URL is required")]
        [StringLength(300, MinimumLength = 5, ErrorMessage = "Live API URL must be between 5 and 300 characters")]
        public string LiveAPIUrl { get; set; }


        [Required(AllowEmptyStrings = false, ErrorMessage = "Sandbox API URL is required")]
        [StringLength(300, MinimumLength = 5, ErrorMessage = "Sandbox API URL must be between 5 and 300 characters")]
        public string SandBoxAPIUrl { get; set; }

        public int CurrentImplementation { get; set; }

        public bool DisplaySummaryStat { get; set; }
        public bool DisplayDemographicStat { get; set; }
        public bool DisplayLocationStat { get; set; }
        public bool HasOtherStat { get; set; }

        public int Status { get; set; }

        public string ClientName { get; set; }
        public string ProductName { get; set; }
    }


    public class DeleteProductItemObj : AdminObj
    {
        [CheckNumber(0, ErrorMessage = "Product Item Id is required")]
        public int ProductItemId { get; set; }
    }


    public class ProductItemSearchObj : AdminObj
    {
        public int ProductItemId { get; set; }
        public int ProductId { get; set; }
        public int ClientId { get; set; }
        public int Status { get; set; }

    }

    //public class ClientProdSearchObj : AdminObj
    //{
    //    public int UserId { get; set; }


    //}

    public class ProductItemRegRespObj
    {
        public int ProductItemId { get; set; }
        public APIResponseStatus Status { get; set; }
    }


    public class ProductItemObj
    {
        public int ProductItemId { get; set; }
        public int ProductId { get; set; }
        public int ClientId { get; set; }
        public string ClientName { get; set; }
        public string ProductName { get; set; }
        
        public string Name { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string LiveAPIUrl { get; set; }
        public string SandBoxAPIUrl { get; set; }
        public int CurrentImplementation { get; set; }
        public string TimestampRegistered { get; set; }
        public bool DisplaySummaryStat { get; set; }
        public bool DisplayDemographicStat { get; set; }
        public bool DisplayLocationStat { get; set; }
        public bool HasOtherStat { get; set; }
        public int ChannelCode { get; set; }
        public int RegisteredBy { get; set; }
        public int Status { get; set; }
        public string StatusLabel { get; set; }
    }


    public class ProductItemRespObj
    {
        public List<ProductItemObj> ProductItems { get; set; }
        public APIResponseStatus Status { get; set; }
    }

  
}
