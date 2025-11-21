using CMCS.data;
using CMCS.Models;
using Microsoft.AspNetCore.Mvc;

namespace CMCS.Controllers
{
    public class ManagerController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ManagerController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            try
            {
                var claims = _context.Claims
                    .Where(c => c.Status == ClaimStatus.VerifiedByCoordinator)
                    .OrderBy(c => c.SubmissionDate)
                    .ToList();

                return View(claims);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error loading manager claims: {ex.Message}";
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
                    claim.Status = ClaimStatus.ApprovedByManager;
                    _context.SaveChanges();

                    TempData["Message"] = $"Claim #{id} approved by Academic Manager.";
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

                    TempData["Message"] = $"Claim #{id} rejected by Academic Manager.";
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

