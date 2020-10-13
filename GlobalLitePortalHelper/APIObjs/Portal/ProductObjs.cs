using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using XPLUG.WEBTOOLKIT;

namespace GlobalLitePortalHelper.APIObjs
{

    public class RegProductObj : AdminObj
    {
        public int ProductId { get; set; }

        [CheckNumber(0, ErrorMessage = "Client Information is required")]
        public int ClientId { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Product name is required")]
        [StringLength(150, MinimumLength = 2, ErrorMessage = "Product name must be between 2 and 150 characters")]
        public string Name { get; set; }


        [Required(AllowEmptyStrings = false, ErrorMessage = "Product description is required")]
        [StringLength(500, MinimumLength = 2, ErrorMessage = "Product description must be between 2 and 500 characters")]
        public string Description { get; set; }
        
        public int Status { get; set; }
        public bool StatusVal { get; set; }
        public string ClientName { get; set; }
    }


    public class EditProductObj : AdminObj
    {
        [CheckNumber(0, ErrorMessage = "Product Id is required")]
        public int ProductId { get; set; }

        [CheckNumber(0, ErrorMessage = "Client Information is required")]
        public int ClientId { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Product name is required")]
        [StringLength(150, MinimumLength = 2, ErrorMessage = "Product name must be between 2 and 150 characters")]
        public string Name { get; set; }


        [Required(AllowEmptyStrings = false, ErrorMessage = "Product description is required")]
        [StringLength(500, MinimumLength = 2, ErrorMessage = "Product description must be between 2 and 500 characters")]
        public string Description { get; set; }

        public int Status { get; set; }

    }


    public class DeleteProductObj : AdminObj
    {
        [CheckNumber(0, ErrorMessage = "ProductId is required")]
        public int ProductId { get; set; }
    }


    public class ProductSearchObj : AdminObj
    {
        public int ProductId { get; set; }
        public int ClientId { get; set; }
        public int Status { get; set; }

    }


    public class ProductRegRespObj
    {
        public int ProductId { get; set; }
        public APIResponseStatus Status { get; set; }
    }


    public class ProductObj
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int ClientId { get; set; }
        public string TimestampRegistered { get; set; }
        public int RegisteredBy { get; set; }
        public int Status { get; set; }
        public string StatusLabel { get; set; }
    }

    public class ProductUserItemObj
    {
        public int ClientId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
     
    }

    public class ProductRespObj
    {
        public List<ProductObj> Products { get; set; }
        public APIResponseStatus Status { get; set; }
    }

    //public class ProductObjInfo
    //{
    //    public ProductObj ProductInfo { get; set; }
    //    public List<ProductItemObj> ProductItems { get; set; }
       
    //}

    //public class ClientProductInfo
    //{
    //    public List<ProductObjInfo> Products { get; set; }
    //    public ClientObj ClientInfo { get; set; }
    //}

    public class ProductObjInfo
    {
        public ProductObj ProductInfo { get; set; }
        public List<ProductItemObj> ProductItems { get; set; }

    }

    public class ClientProductInfo
    {
        public List<ProductObjInfo> Products { get; set; }
        public ClientObj ClientInfo { get; set; }
    }
}
