using API.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace API.Base
{
    [ApiController]
    [Route("[controller]")]
    [TokenValidation]
    public abstract class BaseController<TController> : ControllerBase
    {
        protected readonly ILogger<TController> _logger;

        public BaseController(ILogger<TController> logger)
        {
            _logger = logger;
        }
    }
}
