using CMCS.data;
using CMCS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

                    TempData["Message"] = $"Claim #{id} verified successfully.";
                }
            }
            catch (Exception ex)
            {
                TempData["Message"] = "Error approving claim: " + ex.Message;
            }

            return RedirectToAction("Index");
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

                    TempData["Message"] = $"Claim #{id} rejected successfully.";
                }
            }
            catch (Exception ex)
            {
                TempData["Message"] = "Error rejecting claim: " + ex.Message;
            }

            return RedirectToAction("Index");
        }
    }
}
