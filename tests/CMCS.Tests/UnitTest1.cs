using CMCS.Controllers;
using CMCS.data;
using Microsoft.EntityFrameworkCore;
using Xunit;

public class UnitTest1
{
    private ApplicationDbContext GetInMemoryContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDB")
            .Options;

        return new ApplicationDbContext(options);
    }

    [Fact]
    public void LecturerController_Should_Load_Index_View()
    {
        // Arrange
        var context = GetInMemoryContext();
        var controller = new LecturerController(context);

        // Act
        var result = controller.Index();

        // Assert
        Assert.NotNull(result);
    }
}
