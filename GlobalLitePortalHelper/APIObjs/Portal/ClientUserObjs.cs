using System.Collections.Generic;
using XPLUG.WEBTOOLKIT;

namespace GlobalLitePortalHelper.APIObjs
{
    public class RegClientUserObj :AdminObj
    {
        public int ClientId { get; set; }
        public int ProductId { get; set; }
        public int UserId { get; set; }
        
    }
    public class ClientUserSearchObj : AdminObj
    {
        [CheckNumber(0, ErrorMessage = "Client Id is required")]
        public int ClientId { get; set; }
        public int ProductId { get; set; }
    }

    public class DeleteClientUserObj : AdminObj
    {
        [CheckNumber(0, ErrorMessage = "Client Id is required")]
        public int ClientId { get; set; }
        [CheckNumber(0, ErrorMessage = "User Id is required")]
        public int UserId { get; set; }
        public int ProductId { get; set; }
    }

    public class ClientUserObj
    {
        public int ClientUserId { get; set; }
        public int ClientId { get; set; }
        public int ProductId { get; set; }
        public string ClientName { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
    }

    public class ClientItemObj
    {
        public int ClientId { get; set; }
        public string ClientName { get; set; }
       
    }

    public class ClientUserMgrObj
    {
        public int ClientId { get; set; }
        public string ClientName { get; set; }
        public List<ClientUserObj> ClientUsers { get; set; }
    }

    public class ClientUserRespObj
    {
        public List<ClientUserObj> ClientUsers { get; set; }
        public APIResponseStatus Status { get; set; }
    }
}
