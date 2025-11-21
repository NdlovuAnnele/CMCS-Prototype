using CMCS.data;
using CMCS.Models;
using Microsoft.AspNetCore.Mvc;

namespace CMCS.Controllers
{
    public class LecturerController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LecturerController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            // Show all claims for now (later: filter by lecturer)
            var claims = _context.Claims.OrderByDescending(c => c.SubmissionDate).ToList();
            return View(claims);
        }

        [HttpGet]
        public IActionResult SubmitClaim()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SubmitClaim(Claim claim)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (claim.SupportingDocument != null)
                    {
                        var allowedExtensions = new[] { ".pdf", ".docx", ".xlsx" };
                        var ext = Path.GetExtension(claim.SupportingDocument.FileName).ToLower();

                        if (!allowedExtensions.Contains(ext))
                        {
                            ModelState.AddModelError("SupportingDocument", "Invalid file type. Only PDF, DOCX, and XLSX are allowed.");
                            return View(claim);
                        }

                        if (claim.SupportingDocument.Length > 5 * 1024 * 1024)
                        {
                            ModelState.AddModelError("SupportingDocument", "File size exceeds 5 MB limit.");
                            return View(claim);
                        }

                        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                        if (!Directory.Exists(uploadsFolder))
                            Directory.CreateDirectory(uploadsFolder);

                        var uniqueFileName = Guid.NewGuid() + "_" + claim.SupportingDocument.FileName;
                        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            claim.SupportingDocument.CopyTo(fileStream);
                        }

                        claim.FileName = claim.SupportingDocument.FileName;
                    }

                    claim.SubmissionDate = DateTime.Now;
                    claim.Status = ClaimStatus.Pending;

                    _context.Claims.Add(claim);
                    _context.SaveChanges();

                    TempData["Success"] = $"Claim submitted successfully! {(claim.FileName != null ? $"Uploaded file: {claim.FileName}" : "")}";
                    return RedirectToAction("Confirmation");
                }

                return View(claim);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"An unexpected error occurred while submitting your claim: {ex.Message}";
                return RedirectToAction("Error", "Home");
            }
        }

        public IActionResult Confirmation()
        {
            return View();
        }
    }
}

