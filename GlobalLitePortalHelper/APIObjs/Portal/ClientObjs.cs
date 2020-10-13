using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XPLUG.WEBTOOLKIT;

namespace GlobalLitePortalHelper.APIObjs
{
    public class RegClientObj
    {

        [Required]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "Client Name  must be between 2 and 200 characters")]
        public string ClientName { get; set; }
        [Required]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "Address  must be between 2 and 200 characters")]
        public string Address { get; set; }
        [Required]
        [StringLength(15, MinimumLength = 7, ErrorMessage = "Mobile Number  must be between 7 and 15 characters")]
        public string MobileNumber { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "Email  must be between 15 and 50 characters")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }
        public int AdminUserId { get; set; }
    }
    public class EditClientObj : AdminObj
    {
        [CheckNumber(0, ErrorMessage = "ClientId is required")]
        public int ClientId { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Client's name is required")]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "Client name must be between 2 and 200 characters")]
        public string ClientName { get; set; }


        [Required(AllowEmptyStrings = false, ErrorMessage = "Client's address is required")]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "Client name must be between 2 and 200 characters")]
        public string Address { get; set; }



        [Required(AllowEmptyStrings = false, ErrorMessage = "Client's mobile number is required")]
        [StringLength(15, MinimumLength = 7, ErrorMessage = "Client's mobile number must be between 7 and 15 characters")]
        public string MobileNumber { get; set; }


        [Required(AllowEmptyStrings = false, ErrorMessage = "Client's email address is required")]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "Client's email address is too short or too long (5 - 50)")]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Client's email address is invalid")]
        public string Email { get; set; }

        public int Status { get; set; }

    }

    public class RegResetObj : AdminObj
    {

        [CheckNumber(0, ErrorMessage = "Client Id is required")]
        public int ClientId { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Client's name is required")]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "Client name must be between 2 and 200 characters")]
        public string ClientName { get; set; }


        [Required(AllowEmptyStrings = false, ErrorMessage = "Client's address is required")]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "Client name must be between 2 and 200 characters")]
        public string Address { get; set; }



        [Required(AllowEmptyStrings = false, ErrorMessage = "Client's mobile number is required")]
        [StringLength(15, MinimumLength = 7, ErrorMessage = "Client's mobile number must be between 7 and 15 characters")]
        public string MobileNumber { get; set; }


        [Required(AllowEmptyStrings = false, ErrorMessage = "Client's email address is required")]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "Client's email address is too short or too long (5 - 50)")]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Client's email address is invalid")]
        public string Email { get; set; }

        public int Status { get; set; }
    }

    public class ClientRegRespObj
    {
        public int ClientId { get; set; }
        public APIResponseStatus Status { get; set; }

    }

    public class DeleteClientObj
    {
        public int ClientId { get; set; }
        public int AdminUserId { get; set; }
    }
    public class ClientSearchObj
    {
        public int ClientId { get; set; }
        public int Status { get; set; }
        public int AdminUserId { get; set; }
    }
    public class ClientObj
    {
        public int ClientId { get; set; }
        public string ClientName { get; set; }
        public string Address { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public string TimestampRegistered { get; set; }
        public int RegisteredBy { get; set; }
        public int Status { get; set; }
        public string StatusLabel { get; set; }
        public bool StatusVal { get; set; }
    }
    public class ClientRespObj
    {
        public List<ClientObj> Clients { get; set; }
        public APIResponseStatus Status { get; set; }
    }
    public class ClientProdSearchObj : AdminObj
    {
        public int UserId { get; set; }


    }
    public class ClientProdsRespObj
    {
        public List<ClientProductInfo> ClientProductList { get; set; }
        public APIResponseStatus Status { get; set; }
    }

    public class ClientProductInfoObj
    {
        public int CurrentClientId { get; set; }
        public int CurrentProductId { get; set; }
        public int CurrentProductItemId { get; set; }
        //public ProductObj ProductInfo { get; set; }
        public ProductItemObj ProductInfo { get; set; }
        public AppStatsObj Appstats { get; set; }


    }
}
