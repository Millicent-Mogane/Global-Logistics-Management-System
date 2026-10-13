namespace GLMS.Web.Models
{
    public class DashboardViewModel
    {
        public int TotalClients { get; set; }

        public int TotalContracts { get; set; }

        public int ActiveContracts { get; set; }

        public int ExpiredContracts { get; set; }

        public int TotalServiceRequests { get; set; }
    }
}