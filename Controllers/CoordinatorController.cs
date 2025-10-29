using Microsoft.AspNetCore.Mvc;
using CMCS.Models;

namespace CMCS.Controllers
{
    public class CoordinatorController : Controller
    {
        // Mock data for testing
        public static List<Claim> PendingClaims = new List<Claim>
        {
            new Claim { Id = 1, HoursWorked = 15, HourlyRate = 250, Notes = "Guest lecture", SubmissionDate = DateTime.Now },
            new Claim { Id = 2, HoursWorked = 20, HourlyRate = 300, Notes = "Extra tutorials", SubmissionDate = DateTime.Now.AddDays(-2) }
        };

        public IActionResult Index()
        {
            return View(PendingClaims);
        }

        [HttpPost]
        public IActionResult Approve(int id)
        {
            var claim = PendingClaims.FirstOrDefault(c => c.Id == id);
            if (claim != null)
            {
                claim.Status = ClaimStatus.VerifiedByCoordinator;
                TempData["Message"] = $"Claim #{id} has been verified by Coordinator.";
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Reject(int id)
        {
            var claim = PendingClaims.FirstOrDefault(c => c.Id == id);
            if (claim != null)
            {
                claim.Status = ClaimStatus.Rejected;
                TempData["Message"] = $"Claim #{id} has been rejected by Coordinator.";
            }
            return RedirectToAction("Index");
        }

    }
}
