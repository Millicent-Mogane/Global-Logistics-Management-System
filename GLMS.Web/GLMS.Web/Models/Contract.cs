using System.ComponentModel.DataAnnotations;

namespace GLMS.Web.Models
{
    public class Contract
    {
        public int ContractId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        [Required]
        public string Status { get; set; }

        [Required]
        public string ServiceLevel { get; set; }

        [Display(Name = "Signed Agreement")]
        public string? AgreementFilePath { get; set; }

        public int ClientId { get; set; }

        public Client? Client { get; set; }

        public ICollection<ServiceRequest>? ServiceRequests { get; set; }
    }
}