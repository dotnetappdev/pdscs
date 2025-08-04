using Microsoft.AspNetCore.Mvc;
using System.Net;
using UKParliament.CodeTest.Services;

namespace UKParliament.CodeTest.Web.Controllers
{
    public class BaseApiController : ControllerBase
    {
        protected ActionResult HandleApiResponse(ApiResponse response)
        {
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return Ok();
            }
            if (response.StatusCode == HttpStatusCode.Created)
            {
                return Ok();
            }
            if (response.StatusCode == HttpStatusCode.Accepted)
            {
                return Accepted();
            }
            
            // Handle errors
            var errors = response.GetType().GetProperty("Errors")?.GetValue(response);
            if (errors is IDictionary<string, string[]> fieldErrors && fieldErrors.Count > 0)
            {
                return BadRequest(new { message = response.Message, errors = fieldErrors });
            }
            
            // If error is a string, treat as general error
            if (errors is string msg && !string.IsNullOrWhiteSpace(msg))
            {
                var generalErrors = new Dictionary<string, string[]> { { "General", new[] { msg } } };
                return BadRequest(new { message = response.Message, errors = generalErrors });
            }
            
            return BadRequest(new { message = response.Message });
        }
    }
}