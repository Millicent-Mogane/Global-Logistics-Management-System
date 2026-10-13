using System.ComponentModel.DataAnnotations;

namespace GLMS.Web.Models
{
    public class ServiceRequest
    {
        public int ServiceRequestId { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [Display(Name = "USD Amount")]
        public decimal UsdAmount { get; set; }

        [Display(Name = "Cost (ZAR)")]
        public decimal Cost { get; set; }

        [Required]
        public string Status { get; set; }

        public int ContractId { get; set; }

        public Contract? Contract { get; set; }
    }
}