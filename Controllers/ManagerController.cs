using CMCS.data;
using CMCS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
                // Manager sees only claims verified by Coordinator
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
                    if (claim.Status != ClaimStatus.VerifiedByCoordinator)
                    {
                        TempData["Error"] = $"Claim #{id} is not verified by Coordinator and cannot be approved.";
                    }
                    else
                    {
                        claim.Status = ClaimStatus.ApprovedByManager;
                        _context.SaveChanges();
                        TempData["Message"] = $"Claim #{id} approved by Manager.";
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error approving claim: {ex.Message}";
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
                    if (claim.Status != ClaimStatus.VerifiedByCoordinator)
                    {
                        TempData["Error"] = $"Claim #{id} is not verified by Coordinator and cannot be rejected by Manager.";
                    }
                    else
                    {
                        claim.Status = ClaimStatus.Rejected;
                        _context.SaveChanges();
                        TempData["Message"] = $"Claim #{id} rejected by Manager.";
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error rejecting claim: {ex.Message}";
            }

            return RedirectToAction("Index");
        }

    }
}

