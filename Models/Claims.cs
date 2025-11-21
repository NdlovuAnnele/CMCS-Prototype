using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMCS.Models
{
    public enum ClaimStatus
    {
        Pending,
        VerifiedByCoordinator,
        ApprovedByManager,
        Rejected
    }

    public class Claim
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter the lecturer name.")]
        public string LecturerName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter hours worked.")]
        [Range(1, 200, ErrorMessage = "Hours must be between 1 and 200.")]
        public double HoursWorked { get; set; }

        [Required(ErrorMessage = "Please enter hourly rate.")]
        [Range(50, 2000, ErrorMessage = "Hourly rate must be reasonable.")]
        public decimal HourlyRate { get; set; }

        [Display(Name = "Additional Notes")]
        [StringLength(300)]
        public string? Notes { get; set; }

        [NotMapped] // ⚡ EF must NOT try to store the file itself
        [Display(Name = "Upload Supporting Document")]
        public IFormFile? SupportingDocument { get; set; }

        public string? FileName { get; set; }   // stored in DB
        public DateTime SubmissionDate { get; set; } = DateTime.Now;

        public ClaimStatus Status { get; set; } = ClaimStatus.Pending;
    }
}
