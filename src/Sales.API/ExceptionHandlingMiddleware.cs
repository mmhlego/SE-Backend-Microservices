using System.Net;
using Chat.API.Models;
using Newtonsoft.Json;

namespace Middleware {
    public class ExceptionHandlingMiddleware {
        public RequestDelegate requestDelegate;
        public ExceptionHandlingMiddleware(RequestDelegate requestDelegate) {
            this.requestDelegate = requestDelegate;
        }

        public async Task Invoke(HttpContext context) {
            try {
                await requestDelegate(context);
            } catch (Exception ex) {
                await HandleException(context, ex);
            }
        }
        private Task HandleException(HttpContext context, Exception ex) {
            var errorMessageObject = StatusResponse.Failed(ex.Message);

            var errorMessage = JsonConvert.SerializeObject(errorMessageObject);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            return context.Response.WriteAsync(errorMessage);
        }
    }
}