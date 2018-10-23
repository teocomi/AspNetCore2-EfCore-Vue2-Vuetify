namespace Microsoft.AspNetCore.Authentication
{
    public class AzureAdOptions
    {
        public string ClientId { get; set; }

        public string ClientSecret { get; set; }

        public string Instance { get; set; }

        public string Domain { get; set; }

        public string TenantId { get; set; }

        public string CallbackPath { get; set; }

       // public string Resource { get; set; }

        public string Authority
        {
            get
            {
                return $"{Instance}{TenantId}";
            }
        }

        public static AzureAdOptions Settings { set; get; }
    }
}
