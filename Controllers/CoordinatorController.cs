using CMCS.data;
using CMCS.Models;
using Microsoft.AspNetCore.Mvc;

namespace CMCS.Controllers
{
    public class CoordinatorController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CoordinatorController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            try
            {
                var pendingClaims = _context.Claims
                    .Where(c => c.Status == ClaimStatus.Pending)
                    .OrderBy(c => c.SubmissionDate)
                    .ToList();

                return View(pendingClaims);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error loading claims: {ex.Message}";
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        public IActionResult Approve(int id)
        {
            try
            {
                var claim = _context.Claims.FirstOrDefault(c => c.Id == id);
                if (claim != null)
                {
                    claim.Status = ClaimStatus.VerifiedByCoordinator;
                    _context.SaveChanges();

                    TempData["Message"] = $"Claim #{id} has been verified by Coordinator.";
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error approving claim #{id}: {ex.Message}";
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        public IActionResult Reject(int id)
        {
            try
            {
                var claim = _context.Claims.FirstOrDefault(c => c.Id == id);
                if (claim != null)
                {
                    claim.Status = ClaimStatus.Rejected;
                    _context.SaveChanges();

                    TempData["Message"] = $"Claim #{id} has been rejected by Coordinator.";
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error rejecting claim #{id}: {ex.Message}";
                return RedirectToAction("Error", "Home");
            }
        }
    }
}
