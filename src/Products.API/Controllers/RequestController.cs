using Microsoft.AspNetCore.Mvc;
using Products.API.Models;
using Products.API.Services;
using Chat.API.Models;
namespace Products.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RequestController : ControllerBase
    {
        private readonly IRequestService _requestService;

        public RequestController(IRequestService requestService)
        {
            _requestService = requestService;
        }

        [HttpGet]
        public ActionResult<List<ProductRequest>> GetRequests(ProductRequestTypes type)
        {
            var requests = _requestService.GetRequests(type);
            return Ok(requests);
        }

        [HttpGet("requests/{id}")]
        public ActionResult<ProductRequest> GetRequest(Guid id)
        {
            var request = _requestService.GetRequestById(id);
            if (request == null)
            {
                return NotFound(StatusResponse.Failed("درخواست مورد نظر پیدا نشد."));
            }

            return Ok(request);
        }

        [HttpPut("requests/{id}")]
        public ActionResult<StatusResponse> AcceptRequest(Guid id)
        {
            var request = _requestService.GetRequestById(id);
            bool success;
            if (request == null)
                return NotFound(StatusResponse.Failed("درخواست مورد نظر پیدا نشد."));
            if (request.Type == ProductRequestTypes.AddRequest)
               success= _requestService.AcceptAddRequest(id);
            else success=_requestService.MergeUpdateRequest(id);
            if (success)
            {
                return Ok(StatusResponse.Success);
            }
            else return NotFound(StatusResponse.Failed("درخواست مورد نظر پیدا نشد."));
        }

        [HttpDelete("requests/{id}")]
        public ActionResult<StatusResponse> RejectRequest(Guid id)
        {
            bool success = _requestService.DeleteRequest(id);
            if (success)
            {
                return Ok(StatusResponse.Success);
            }

            return NotFound(StatusResponse.Failed("درخواست مورد نظر پیدا نشد."));
        }
    }

}