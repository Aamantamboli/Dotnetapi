using KubernetesAutoClusterAPI.Model.Request;
using KubernetesAutoClusterAPI.Model.Response;

using MediatR;

namespace KubernetesAutoClusterAPI.Commands
{

    public class GetCustomerDataCommand : IRequest<GetCustomerData_Res>
    {
        public GetCustomerData_Req GetCustomerData_Req { get; set; }
    }
}
