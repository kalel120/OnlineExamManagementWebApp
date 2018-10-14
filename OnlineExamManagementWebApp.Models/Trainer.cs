using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OnlineExamManagementWebApp.Models {
    public class Trainer {
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string Name { get; set; }

        [Required]
        [MaxLength(20)]
        public string Contact { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Address { get; set; }

        public string AlternateAddress { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string PostalCode { get; set; }

        [Required]
        public string Country { get; set; }                

        public byte[] Image { get; set; }

        public Organization Organization { get; set; }

        public int OrganizationId { get; set; }
    }
}