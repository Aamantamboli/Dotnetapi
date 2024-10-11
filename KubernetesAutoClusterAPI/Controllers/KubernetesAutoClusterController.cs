using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using KubernetesAutoClusterAPI.Model.Request;
using KubernetesAutoClusterAPI.Commands;
using KubernetesAutoClusterAPI.Model.Response;

namespace KubernetesAutoClusterAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KubernetesAutoClusterController : ControllerBase
    {
        private IMediator _mediator;
        private readonly ILogger<KubernetesAutoClusterController> _logger;

        public KubernetesAutoClusterController(ILogger<KubernetesAutoClusterController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        //to get customer info from DB
        [HttpPost]
        public async Task<ActionResult> GetCustomerData(GetCustomerData_Req GetCustomerData)
        {
            var result = await _mediator.Send(new GetCustomerDataCommand { GetCustomerData_Req = GetCustomerData });
            return Ok(result);
        }

        


    }
}
