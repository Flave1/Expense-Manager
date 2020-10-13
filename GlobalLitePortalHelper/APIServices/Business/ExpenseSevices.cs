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


    public class ExpenseRequisitionServices
    {
        public static ExpenseRespObj LoadExpenses(ExpenseSearchObj regObj, string username)
        {
            var response = new ExpenseRespObj
            {
                Status = new APIResponseStatus
                {
                    IsSuccessful = false,
                    Message = new APIResponseMessage(),
                },

            };
            try
            {

                var apiResponse = new APIHelper(APIEndpoints.LOAD_EXPENSES_ENDPOINT, username, Method.POST).ProcessAPI<ExpenseSearchObj, ExpenseRespObj>(regObj, out var msg);
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

        public static ExpenseRegRespObj UpdateExpense(EditExpenseObj regObj, string username)
        {
            var response = new ExpenseRegRespObj
            {
                Status = new APIResponseStatus
                {
                    IsSuccessful = false,
                    Message = new APIResponseMessage(),
                },

            };
            try
            {

                var apiResponse = new APIHelper(APIEndpoints.UPDATE_EXPENSE_ENDPOINT, username, Method.POST).ProcessAPI<EditExpenseObj, ExpenseRegRespObj>(regObj, out var msg);
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

        public static ExpenseRegRespObj AddExpense(RegExpenseObj regObj, string username)
        {
            var response = new ExpenseRegRespObj
            {
                Status = new APIResponseStatus
                {
                    IsSuccessful = false,
                    Message = new APIResponseMessage(),
                },

            };
            try
            {

                var apiResponse = new APIHelper(APIEndpoints.ADD_EXPENSE_ENDPOINT, username, Method.POST).ProcessAPI<RegExpenseObj, ExpenseRegRespObj>(regObj, out var msg);
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
