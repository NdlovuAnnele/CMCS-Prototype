using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace CMCS.Models
{
    public class Lecturer
    {
        public int Id { get; set; }

        [Required]
        public string FullName { get; set; } = string.Empty;

        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Department { get; set; }

        public ICollection<Claim> Claims { get; set; } = new List<Claim>();
    }
}
