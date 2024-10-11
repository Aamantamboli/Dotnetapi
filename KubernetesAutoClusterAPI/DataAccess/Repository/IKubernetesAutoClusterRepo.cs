using KubernetesAutoClusterAPI.Model.Request;
using KubernetesAutoClusterAPI.Model.Response;

namespace KubernetesAutoClusterAPI.DataAccess.Repository
{
    public interface IKubernetesAutoClusterRepo
    {
        Task<List<GetCustomerData_Res>> getCustomerData(GetCustomerData_Req request);


    }
}
