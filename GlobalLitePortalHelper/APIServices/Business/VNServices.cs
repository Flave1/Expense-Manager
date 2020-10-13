using GlobalLitePortalHelper.APICore;
using GlobalLitePortalHelper.APIObjs;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XPLUG.WEBTOOLKIT;

namespace GlobalLitePortalHelper.APIServices.Business
{
    public class  VNServices
    {
        public static VoucherRequestRespObj GenerateVoucherNumbers(VoucherNumberSearchObj regObj)
        {
            var response = new VoucherRequestRespObj
            {
                Status = new APIResponseStatus
                {
                    IsSuccessful = false,
                    Message = new APIResponseMessage(),
                },

            };
            try
            {

                var apiResponse = new APIHelper(APIEndpoints.GENERATE_VOUCHER_NUMBER_ENDPOINT, Method.POST).ProcessAPI<VoucherNumberSearchObj, VoucherRequestRespObj>(regObj, out var msg);
                if (msg.Code == 0 && string.IsNullOrEmpty(msg.TechMessage) && string.IsNullOrEmpty(msg.Message))
                {
                    return apiResponse;
                }

                response.Status.Message.FriendlyMessage = msg.Message;
                response.Status.Message.TechnicalMessage = msg.TechMessage;
                return response;


            }
            catch (Exception ex)
            {
                UtilTools.LogE(ex.StackTrace, ex.Source, ex.GetBaseException().Message);
                response.Status.Message.FriendlyMessage = "Error Occurred! Please try again later";
                response.Status.Message.TechnicalMessage = "Error: " + ex.GetBaseException().Message;
                return response;
            }
        }

    }
}
