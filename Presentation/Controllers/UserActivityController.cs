using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Presentation.ActionFilters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/useractivity")]
    public class UserActivityController
    {
        [HttpGet]
        [Authorize]
        [ServiceFilter(typeof(ApiKeyFilter))]
        public IActionResult GetActiveUsers()
        {
            return null;
        }

        [HttpPost]
        [Authorize]
        [ServiceFilter(typeof(ApiKeyFilter))]
        public IActionResult SendFileTransferRequestToUser([FromBody] string userTopic)
        {
            return null;
        }
    }
}
