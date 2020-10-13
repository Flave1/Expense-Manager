using System;
using GlobalLitePortalHelper.APICore;
using GlobalLitePortalHelper.APIObjs;
using RestSharp;
using XPLUG.WEBTOOLKIT;

namespace GlobalLitePortalHelper.APIServices
{
    public  class SMSRouteService
    {
        public static SMSRouteRegRespObj AddSMSRoute(RegSMSRouteObj regObj, string username)
        {
            var response = new SMSRouteRegRespObj
            {
                Status = new APIResponseStatus
                {
                    IsSuccessful = false,
                    Message = new APIResponseMessage(),
                },

            };
            try
            {

                var apiResponse = new APIHelper(APIEndpoints.ADD_SMS_ROUTE_ENDPOINT, username, Method.POST).ProcessAPI<RegSMSRouteObj, SMSRouteRegRespObj>(regObj, out var msg);
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

        public static SMSRouteRegRespObj UpdateSMSRoute(EditSMSRouteObj regObj, string username)
        {
            var response = new SMSRouteRegRespObj
            {
                Status = new APIResponseStatus
                {
                    IsSuccessful = false,
                    Message = new APIResponseMessage(),
                },

            };
            try
            {

                var apiResponse = new APIHelper(APIEndpoints.UPDATE_SMS_ROUTE_ENDPOINT, username, Method.POST).ProcessAPI<EditSMSRouteObj, SMSRouteRegRespObj>(regObj, out var msg);
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



        public static SMSRouteRegRespObj DeleteSMSRoute(DeleteSMSRouteObj regObj, string username)
        {
            var response = new SMSRouteRegRespObj
            {
                Status = new APIResponseStatus
                {
                    IsSuccessful = false,
                    Message = new APIResponseMessage(),
                },

            };
            try
            {

                var apiResponse = new APIHelper(APIEndpoints.DELETE_SMS_ROUTE_ENDPOINT, username, Method.POST).ProcessAPI<DeleteSMSRouteObj, SMSRouteRegRespObj>(regObj, out var msg);
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



        public static SMSRouteRespObj LoadSMSRoutes(SMSRouteSearchObj regObj, string username)
        {
            var response = new SMSRouteRespObj
            {
                Status = new APIResponseStatus
                {
                    IsSuccessful = false,
                    Message = new APIResponseMessage(),
                },

            };
            try
            {

                var apiResponse = new APIHelper(APIEndpoints.LOAD_SMS_ROUTES_ENDPOINT, username, Method.POST).ProcessAPI<SMSRouteSearchObj, SMSRouteRespObj>(regObj, out var msg);
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
