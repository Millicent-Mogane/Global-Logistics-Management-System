using GLMS.Web.Models;

namespace GLMS.Web.Factories
{
    public static class ServiceRequestFactory
    {
        public static ServiceRequest Create(
            string description,
            decimal usdAmount,
            decimal cost,
            string status,
            int contractId)
        {
            return new ServiceRequest
            {
                Description = description,
                UsdAmount = usdAmount,
                Cost = cost,
                Status = status,
                ContractId = contractId
            };
        }
    }
}