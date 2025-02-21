using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MpexWebApi.Controllers
{
    [ApiController]
    public class BaseController : ControllerBase
    {
        public static bool IsGuidValid(string? id, ref Guid parsedGuid)
        {
            // Non-existing parameter in the URL
            if (String.IsNullOrWhiteSpace(id))
            {
                return false;
            }

            // Invalid parameter in the URL
            bool isGuidValid = Guid.TryParse(id, out parsedGuid);
            if (!isGuidValid)
            {
                return false;
            }

            return true;
        }
    }
}
