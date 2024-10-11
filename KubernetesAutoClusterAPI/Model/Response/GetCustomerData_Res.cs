namespace KubernetesAutoClusterAPI.Model.Response
{

    public class GetCustomerData_Res
    {
        public List<GetCustomerDataStatus> GetCustomerDataStatus { get; set; }
        public List<GetCustomerDataData> GetCustomerDataData { get; set; }
    }
    public class GetCustomerDataStatus
    {
        public int Result { get; set; }
        public string Message { get; set; } = "";
    }
    public class GetCustomerDataData
    {
        public int cust_id { get; set; }
        public string cust_name { get; set; } = "";
        public string cust_dob { get; set; } = "";
    }
}
