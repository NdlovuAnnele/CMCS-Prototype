using Microsoft.AspNetCore.Mvc;
using CMCS.Models;

namespace CMCS.Controllers
{
    public class ManagerController : Controller
    {
        // Mock data for testing
        public static List<Claim> PendingClaims = new List<Claim>
        {
            new Claim { Id = 3, HoursWorked = 12, HourlyRate = 280, Notes = "Moderator duty", SubmissionDate = DateTime.Now },
            new Claim { Id = 4, HoursWorked = 10, HourlyRate = 300, Notes = "Marking", SubmissionDate = DateTime.Now.AddDays(-1) }
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
                claim.Status = ClaimStatus.ApprovedByManager;
                TempData["Message"] = $"Claim #{id} approved by Academic Manager.";
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
                TempData["Message"] = $"Claim #{id} rejected by Academic Manager.";
            }
            return RedirectToAction("Index");
        }

    }
}
