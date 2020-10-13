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


    public class WorkflowTaskServices
    {
        public static WorkflowTaskRespObj LoadWorkflowTasks(WorkflowTaskSearchObj regObj, string username)
        {
            var response = new WorkflowTaskRespObj
            {
                Status = new APIResponseStatus
                {
                    IsSuccessful = false,
                    Message = new APIResponseMessage(),
                },

            };
            try
            { 
                var apiResponse = new APIHelper(APIEndpoints.LOAD_WORKFLOW_TASKS_ENDPOINT, username, Method.POST).ProcessAPI<WorkflowTaskSearchObj, WorkflowTaskRespObj>(regObj, out var msg);
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

        public static WorkflowTaskRegRespObj UpdateWorkflowTask(EditWorkflowTaskObj regObj, string username)
        {
            var response = new WorkflowTaskRegRespObj
            {
                Status = new APIResponseStatus
                {
                    IsSuccessful = false,
                    Message = new APIResponseMessage(),
                },  
            };
            try
            {

                var apiResponse = new APIHelper(APIEndpoints.UPDATE_WORKFLOW_TASK_ENDPOINT, username, Method.POST).ProcessAPI<EditWorkflowTaskObj, WorkflowTaskRegRespObj>(regObj, out var msg);
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

        public static WorkflowTaskRegRespObj AddWorkflowTask(RegWorkflowTaskObj regObj, string username)
        {
            var response = new WorkflowTaskRegRespObj
            {
                Status = new APIResponseStatus
                {
                    IsSuccessful = false,
                    Message = new APIResponseMessage(),
                },

            };
            try
            {

                var apiResponse = new APIHelper(APIEndpoints.ADD_WORKFLOW_TASK_ENDPOINT, username, Method.POST).ProcessAPI<RegWorkflowTaskObj, WorkflowTaskRegRespObj>(regObj, out var msg);
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
