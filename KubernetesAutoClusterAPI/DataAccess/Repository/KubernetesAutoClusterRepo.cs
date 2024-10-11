using Dapper.Oracle;
using KubernetesAutoClusterAPI.Model.Request;
using KubernetesAutoClusterAPI.Model.Response;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using Dapper;
using Azure;

namespace KubernetesAutoClusterAPI.DataAccess.Repository
{
    public class KubernetesAutoClusterRepo : IKubernetesAutoClusterRepo
    {
        ILogger<KubernetesAutoClusterRepo> _logger;
        private readonly IDbConnection _db;

        public KubernetesAutoClusterRepo(IConfiguration configuration, ILogger<KubernetesAutoClusterRepo> logger)
        {
            _logger = logger;
            _db = new OracleConnection(configuration.GetConnectionString("TransactionDB"));
        }

        public async Task<List<GetCustomerData_Res>> getCustomerData(GetCustomerData_Req request)
        {


            _logger.LogInformation("In KubernetesAutoClusterRepo:getCustomerData  Repository");
            try
            {
                var response = new GetCustomerData_Res();
                using (IDbConnection conn = new OracleConnection(_db.ConnectionString))
                {
                    OracleDynamicParameters parameters = new OracleDynamicParameters();

                    foreach (var prop in typeof(GetCustomerData_Req).GetProperties())
                    {
                        parameters.Add(name: $"v_{prop.Name.ToLower()}", value: prop.GetValue(request));
                    }
                    parameters.Add(name: "V_status_cur", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
                    parameters.Add(name: "V_data_cur", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
                    var reader = conn.QueryMultiple("temp_shreyash.Get_CustomerData", parameters, commandType: CommandType.StoredProcedure);
                    var status = reader.Read<GetCustomerDataStatus>().ToList();
                    try
                    {
                        var Data = reader.Read<GetCustomerDataData>().ToList();
                        int result = -1;
                        string message = "Null";
                        foreach (var status1 in status)
                        {
                            result = status1.Result;
                            message = status1.Message;
                        }
                        if (result >= 0)
                        {
                            response = new GetCustomerData_Res
                            {
                                GetCustomerDataStatus = status,
                                GetCustomerDataData = Data
                            };
                        }
                        else
                        {
                            response = new GetCustomerData_Res
                            {
                                GetCustomerDataStatus = status,
                                GetCustomerDataData = null
                            };
                        }
                    }
                    catch (Exception ex)
                    {
                        response = new GetCustomerData_Res
                        {
                            GetCustomerDataStatus = status,
                            GetCustomerDataData = null
                        };
                    }
                    return new List<GetCustomerData_Res> { response };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in GetCustomerData DB Response: {ex.Message}");
                return null;
            }
        }

        

    }
}
