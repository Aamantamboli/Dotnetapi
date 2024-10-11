using KubernetesAutoClusterAPI.Commands;
using KubernetesAutoClusterAPI.DataAccess.Repository;
using KubernetesAutoClusterAPI.Model.Response;
using MediatR;

namespace KubernetesAutoClusterAPI.Handlers
{
    


    public class GetCustomerDataHandler : IRequestHandler<GetCustomerDataCommand, GetCustomerData_Res>
    {
        IKubernetesAutoClusterRepo _kubernetesautoclusterRepository;
        ILogger<GetCustomerDataHandler> _logger;

        public GetCustomerDataHandler(IKubernetesAutoClusterRepo kubernetesautoclusterRepository, ILogger<GetCustomerDataHandler> logger)
        {
            _kubernetesautoclusterRepository = kubernetesautoclusterRepository;
            _logger = logger;
        }

        public async Task<GetCustomerData_Res> Handle(GetCustomerDataCommand command, CancellationToken cancellationToken)
        {
            GetCustomerData_Res getCustomerDataResponse = new();
            var result = await _kubernetesautoclusterRepository.getCustomerData(command.GetCustomerData_Req);
            if (result != null && result.Any())
            {
                getCustomerDataResponse = new GetCustomerData_Res
                {
                    GetCustomerDataData = result[0].GetCustomerDataData,
                    GetCustomerDataStatus = result[0].GetCustomerDataStatus,
                };
            }
            return getCustomerDataResponse;
        }
    }


}
