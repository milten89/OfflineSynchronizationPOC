using System.Text;
using Dotmim.Sync;
using Dotmim.Sync.Web.Server;
using Microsoft.AspNetCore.Mvc;

namespace OfflineSynchronizationPOC.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SyncController : ControllerBase
    {
        private readonly WebServerAgent _webServerAgent;
        private readonly IWebHostEnvironment _env;

        // Injected thanks to Dependency Injection
        public SyncController(WebServerAgent webServerAgent, 
                              IWebHostEnvironment env)
        {
            _webServerAgent = webServerAgent ?? throw new ArgumentNullException(nameof(webServerAgent));
            _env = env ?? throw new ArgumentNullException(nameof(env));
        }

        /// <summary>
        /// This POST handler is mandatory to handle all the sync process
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Task Post(CancellationToken cancellationToken)
            => _webServerAgent.HandleRequestAsync(HttpContext, cancellationToken);

        /// <summary>
        /// This GET handler is optional. It allows you to see the configuration hosted on the server
        /// </summary>
        /// <param name="cancellationToken"></param>
        [HttpGet]
        public async Task Get(CancellationToken cancellationToken)
        {
            if (_env.IsDevelopment())
            {
                await HttpContext.WriteHelloAsync(_webServerAgent, cancellationToken);
            }
            else
            {
                var stringBuilder = new StringBuilder();

                stringBuilder.AppendLine("<!doctype html>");
                stringBuilder.AppendLine("<html>");
                stringBuilder.AppendLine("<title>Web Server properties</title>");
                stringBuilder.AppendLine("<body>");
                stringBuilder.AppendLine(" PRODUCTION MODE. HIDDEN INFO ");
                stringBuilder.AppendLine("</body>");
                await HttpContext.Response.WriteAsync(stringBuilder.ToString(), cancellationToken);
            }
        }
    }
}