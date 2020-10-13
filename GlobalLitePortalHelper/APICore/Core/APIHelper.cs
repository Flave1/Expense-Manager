using System;
using GlobalLitePortalHelper.APIObjs;
using PluglexHelper.PortalObjs;
using PluglexHelper.RemoteService;
using RestSharp;
using XPLUG.WEBTOOLKIT;

namespace GlobalLitePortalHelper.APICore
{

    public struct APIOutMessage
    {
        public int Code;
        public string Message;
        public string TechMessage;
    }

    internal class APIHelper : RemoteCore
    {
        private readonly string _username;
        private readonly string _endpoint;
        private readonly Method _method;
        public APIHelper(string endpoint, string username, Method method) : base(endpoint, username, method)
        {
            _username = username;
            _endpoint = endpoint;
            _method = method;
        }
        public APIHelper(string endpoint, Method method) : base(endpoint, method)
        {

            _endpoint = endpoint;
            _method = method;
        }

        internal object ProcessAPI<T1, T2>(VoucherNumber regObj, out APIOutMessage msg)
        {
            throw new NotImplementedException();
        }

        public TR ProcessAPI<T, TR>(T serviceObj, out APIOutMessage msg) where T : class where TR : new()
        {
            try
            {
                _request.AddJsonBody(serviceObj);
                var apiResponse = _client.Execute(_request);
                var responseCode = (int)apiResponse.StatusCode;
                msg.Code = responseCode;
                if (responseCode >= 200 && responseCode < 300)
                {
                    var deserializedResponse = new RequestResponseHelper().Deserialize<TR>(apiResponse, null, out msg.TechMessage);
                    if (deserializedResponse == null)
                    {
                        msg.Message = "Unable to process response from the server. Please re-check your transaction status";
                        msg.Code = -1;
                        return default;
                    }

                    msg.Message = "";
                    msg.TechMessage = "";
                    msg.Code = 0;
                    return deserializedResponse;
                }

                if (msg.Code == 400)
                {
                    msg.Message = "Validation error occurred! At least one of the supplied data is invalid";
                    msg.Code = -4;
                    msg.TechMessage = apiResponse.Content + " " + apiResponse.ErrorMessage;
                    return default;
                }

                if (msg.Code == 401)
                {
                    if (!GlobalTokenParam.RefreshTokenDirect(_username))
                    {
                        msg.Message = "We are unable to complete your request at this time due to system authorization error. Please try again later";
                        msg.Code = -3;
                        msg.TechMessage = "Authorization Failed";
                        return default;
                    }

                    //Re-Initialize and invoke the service again
                    ReInit(_endpoint, _username, _method);
                    apiResponse = _client.Execute(_request);
                    responseCode = (int)apiResponse.StatusCode;
                    msg.Code = responseCode;
                    if (responseCode < 200 || responseCode >= 300)
                    {
                        //Other HttpStatusCode returned
                        msg.Message = "We are unable to complete your request at this time due to system error. Please try again later";
                        msg.TechMessage = "Authorization Failed";
                        return default;
                    }

                    var deserializedResponse = new RequestResponseHelper().Deserialize<TR>(apiResponse, null, out msg.TechMessage);
                    if (deserializedResponse == null)
                    {
                        msg.Message = "Unable to process response from the server. Please re-check your transaction status";
                        msg.Code = -1;
                        return default;
                    }

                    msg.Message = "";
                    msg.TechMessage = "";
                    msg.Code = 0;
                    return deserializedResponse;
                }

                msg.Message = "We are unable to complete your request at this time. Please try again later";
                msg.TechMessage = "Other Http Status Code";
                return default;
            }
            catch (Exception ex)
            {
                UtilTools.LogE(ex.StackTrace, ex.Source, ex.GetBaseException().Message);
                msg.TechMessage = ex.GetBaseException().Message;
                msg.Message = "We are unable to complete your request at this time. Please try again later";
                msg.Code = -2;
                return default;
            }
        }

        public TR ProcessAPI<TR>( out APIOutMessage msg)  where TR : new()
        {
            try
            {
                
                var apiResponse = _client.Execute(_request);
                var responseCode = (int)apiResponse.StatusCode;
                msg.Code = responseCode;
                if (responseCode >= 200 && responseCode < 300)
                {
                    var deserializedResponse = new RequestResponseHelper().Deserialize<TR>(apiResponse, null, out msg.TechMessage);
                    if (deserializedResponse == null)
                    {
                        msg.Message = "Unable to process response from the server. Please re-check your transaction status";
                        msg.Code = -1;
                        return default;
                    }

                    msg.Message = "";
                    msg.TechMessage = "";
                    msg.Code = 0;
                    return deserializedResponse;
                }

                if (msg.Code == 400)
                {
                    msg.Message = "Validation error occurred! At least one of the supplied data is invalid";
                    msg.Code = -4;
                    msg.TechMessage = apiResponse.Content + " " + apiResponse.ErrorMessage;
                    return default;
                }

                if (msg.Code == 401)
                {
                    if (!GlobalTokenParam.RefreshTokenDirect(_username))
                    {
                        msg.Message = "We are unable to complete your request at this time due to system authorization error. Please try again later";
                        msg.Code = -3;
                        msg.TechMessage = "Authorization Failed";
                        return default;
                    }

                    //Re-Initialize and invoke the service again
                    ReInit(_endpoint, _username, _method);
                    apiResponse = _client.Execute(_request);
                    responseCode = (int)apiResponse.StatusCode;
                    msg.Code = responseCode;
                    if (responseCode < 200 || responseCode >= 300)
                    {
                        //Other HttpStatusCode returned
                        msg.Message = "We are unable to complete your request at this time due to system error. Please try again later";
                        msg.TechMessage = "Authorization Failed";
                        return default;
                    }

                    var deserializedResponse = new RequestResponseHelper().Deserialize<TR>(apiResponse, null, out msg.TechMessage);
                    if (deserializedResponse == null)
                    {
                        msg.Message = "Unable to process response from the server. Please re-check your transaction status";
                        msg.Code = -1;
                        return default;
                    }

                    msg.Message = "";
                    msg.TechMessage = "";
                    msg.Code = 0;
                    return deserializedResponse;
                }

                msg.Message = "We are unable to complete your request at this time. Please try again later";
                msg.TechMessage = "Other Http Status Code";
                return default;
            }
            catch (Exception ex)
            {
                UtilTools.LogE(ex.StackTrace, ex.Source, ex.GetBaseException().Message);
                msg.TechMessage = ex.GetBaseException().Message;
                msg.Message = "We are unable to complete your request at this time. Please try again later";
                msg.Code = -2;
                return default;
            }
        }
    }
}
