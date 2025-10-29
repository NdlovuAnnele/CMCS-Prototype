using System.Threading;
using CMCS.Controllers;
using CMCS.Models;
using Microsoft.AspNetCore.Mvc;
using Xunit;
using Moq;

namespace CMCS.Tests
{
    public class LecturerControllerTests
    {
        [Fact]
        public void SubmitClaim_ValidClaim_RedirectsToConfirmation()
        {
            // Arrange
            var controller = new LecturerController();

            // ✅ Mock TempData so it’s not null
            controller.TempData = new Microsoft.AspNetCore.Mvc.ViewFeatures.TempDataDictionary(
                new Microsoft.AspNetCore.Http.DefaultHttpContext(),
                Mock.Of<Microsoft.AspNetCore.Mvc.ViewFeatures.ITempDataProvider>()
            );

            var claim = new Claim
            {
                HoursWorked = 10,
                HourlyRate = 200,
                Notes = "Guest lecture"
            };

            // Act
            var result = controller.SubmitClaim(claim) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Confirmation", result.ActionName);
        }

        [Fact]
        public void SubmitClaim_InvalidClaim_ReturnsView()
        {
            // Arrange
            var controller = new LecturerController();
            controller.ModelState.AddModelError("HoursWorked", "Required");
            var claim = new Claim();

            // Act
            var result = controller.SubmitClaim(claim) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("SubmitClaim", result.ViewName ?? "SubmitClaim");
        }
    }
}
