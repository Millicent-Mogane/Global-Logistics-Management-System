using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;

namespace GLMS.Web.Models
{
    public class Client
    {
        public int ClientId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string ContactDetails { get; set; }

        [Required]
        public string Region { get; set; }

        public ICollection<Contract>? Contracts { get; set; }
    }
}