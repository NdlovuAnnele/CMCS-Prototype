using Microsoft.AspNetCore.Mvc;
using CMCS.Models;

namespace CMCS.Controllers
{
    public class LecturerController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
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

                        var uniqueFileName = Guid.NewGuid().ToString() + "_" + claim.SupportingDocument.FileName;
                        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            claim.SupportingDocument.CopyTo(fileStream);
                        }

                        claim.FileName = claim.SupportingDocument.FileName;
                    }

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
