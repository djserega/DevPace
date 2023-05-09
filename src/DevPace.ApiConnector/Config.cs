namespace DevPace.ApiConnector
{
    internal class Config
    {
        public Config()
        {
            Server = "http://localhost:5000";
            ApiCustomerVersion = "api/v1";
            ApiCustomerName = "customers";

            GetCustomerList = ApiCustomerVersion + "/" + ApiCustomerName;
            GetCustomer = ApiCustomerVersion + "/" + ApiCustomerName;
            PostCustomer = ApiCustomerVersion + "/" + ApiCustomerName;
            PutCustomer = ApiCustomerVersion + "/" + ApiCustomerName;
            DeleteCustomer = ApiCustomerVersion + "/" + ApiCustomerName;
        }

        internal string Server { get; init; }

        internal string ApiCustomerVersion { get; init; }
        internal string ApiCustomerName { get; init; }
        internal string GetCustomerList { get; init; }
        internal string GetCustomer { get; init; }
        internal string PostCustomer { get; init; }
        internal string PutCustomer { get; init; }
        internal string DeleteCustomer { get; init; }
    }
}
