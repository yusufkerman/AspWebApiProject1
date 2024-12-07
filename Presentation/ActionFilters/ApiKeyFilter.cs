using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Org.BouncyCastle.Asn1.Ocsp;
using Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.ActionFilters
{
    public class ApiKeyFilter : ActionFilterAttribute
    {
        private readonly IServiceManager _serviceManager;

        public ApiKeyFilter(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        //Endpointler çalışmadan önce çalışacak.
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            //Context içerisinden requeste erişir.
            var request = context.HttpContext.Request;

            //Request header kısmında api key gönderilmiş mi kontrol eder.
            if (!request.Headers.TryGetValue("X-API-KEY", out var apiKey))
            {
                context.Result = new UnauthorizedObjectResult("API key is missing"); //401
                return;
            }

            //Gelen api key için doğrulama yapar.
            var isValidApiKey = _serviceManager.ApiKeyValidationService.ValidateApiKey(apiKey);

            //Eğer doğru api key değil ise 401 döner.
            if (!isValidApiKey)
            {
                context.Result = new UnauthorizedObjectResult("Invalid API key"); //401
                return;
            }
        }
    }
}
