using System;
using GlobalLitePortalHelper.APICore;
using GlobalLitePortalHelper.APIObjs;
using RestSharp;
using XPLUG.WEBTOOLKIT;

namespace GlobalLitePortalHelper.APIServices
{
    public class LookupVersionService
    {
        public static LookupStatRespObj LoadLookupStats( string username)
        {
            var response = new LookupStatRespObj
            {
                Status = new APIResponseStatus
                {
                    IsSuccessful = false,
                    Message = new APIResponseMessage(),
                },

            };
            try
            {

                var apiResponse = new APIHelper(APIEndpoints.LOAD_LOOKUP_STATS_ENDPOINT, username, Method.GET).ProcessAPI<LookupStatRespObj>(out var msg);
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
