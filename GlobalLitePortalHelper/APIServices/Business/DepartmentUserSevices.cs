using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GlobalLitePortalHelper.APICore;
using GlobalLitePortalHelper.APIObjs;
using PlugLitePortalHelper.APIObjs;
using RestSharp;
using XPLUG.WEBTOOLKIT;

namespace PlugLitePortalHelper.APIServices
{


    public class DepartmentUserServices
    {
        public static DepartmentUserRespObj LoadDepartmentUsers(DepartmentUserSearchObj regObj, string username)
        {
            var response = new DepartmentUserRespObj
            {
                Status = new APIResponseStatus
                {
                    IsSuccessful = false,
                    Message = new APIResponseMessage(),
                },

            };
            try
            {

                var apiResponse = new APIHelper(APIEndpoints.LOAD_DEPARTMENT_USER_ENDPOINT, username, Method.POST).ProcessAPI<DepartmentUserSearchObj, DepartmentUserRespObj>(regObj, out var msg);
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

        public static DepartmentUserRegRespObj UpdateDepartmentUser(EditDepartmentUserObj regObj, string username)
        {
            var response = new DepartmentUserRegRespObj
            {
                Status = new APIResponseStatus
                {
                    IsSuccessful = false,
                    Message = new APIResponseMessage(),
                },

            };
            try
            {

                var apiResponse = new APIHelper(APIEndpoints.UPDATE_DEPARTMENT_USERS_ENDPOINT, username, Method.POST).ProcessAPI<EditDepartmentUserObj, DepartmentUserRegRespObj>(regObj, out var msg);
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

        public static DepartmentUserRegRespObj AddDepartmentUser(RegDepartmentUserObj regObj, string username)
        {
            var response = new DepartmentUserRegRespObj
            {
                Status = new APIResponseStatus
                {
                    IsSuccessful = false,
                    Message = new APIResponseMessage(),
                },

            };
            try
            {

                var apiResponse = new APIHelper(APIEndpoints.ADD_DEPARTMENT_USERS_ENDPOINT, username, Method.POST).ProcessAPI<RegDepartmentUserObj, DepartmentUserRegRespObj>(regObj, out var msg);
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
