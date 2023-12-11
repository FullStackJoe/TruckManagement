using Microsoft.EntityFrameworkCore;
using WebApplication1.Shared;

namespace Tests;

[TestClass]
public class BackEndTests
{
    
    private readonly DatabaseContext context;
    private readonly DAO dao;

    public BackEndTests()
    {
        var options = new DbContextOptionsBuilder<DatabaseContext>()
            .UseInMemoryDatabase(databaseName: "database")
            .Options;
        this.context = new DatabaseContext();
        this.dao = new DAO(context);
    }
    

    // UNIT TEST OF ChargingCalculation.CalculateChargingHours
    [TestMethod]
    public void ChargingHours_CalculatesCorrectly()
    {
        // Arrange
        int percentage = 10;
        int batterySize = 400;
        int chargerAmpere = 60;
        
        int expected = 6;
        
        // Act
        int actual = ChargingCalculation.CalculateChargingHours(percentage, batterySize, chargerAmpere);

        // Assert
        Assert.AreEqual(expected, actual, 0.001, "Hours not calculated correctly");
    }
    
    [TestMethod]
    public void ChargingHours_CalculatesCorrectly_Zero()
    {
        // Arrange
        int percentage = 0;
        int batterySize = 400;
        int chargerAmpere = 60;
        
        int expected = 7;
        
        // Act
        int actual = ChargingCalculation.CalculateChargingHours(percentage, batterySize, chargerAmpere);

        // Assert
        Assert.AreEqual(expected, actual, 0.001, "Hours not calculated correctly");
    }
    
    [TestMethod]
    public void ChargingHours_CalculatesCorrectly_Minus()
    {
        // Arrange
        int percentage = -10;
        int batterySize = 400;
        int chargerAmpere = 60;
        
        // Act
        Func<int> actual = () => ChargingCalculation.CalculateChargingHours(percentage, batterySize, chargerAmpere);

        // Assert
        Assert.ThrowsException<System.ArgumentOutOfRangeException>(() => actual());
    }
    [TestMethod]
    public void ChargingHours_CalculatesCorrectly_Many()
    {
        // Arrange
        int percentage = 120;
        int batterySize = 400;
        int chargerAmpere = 60;
        
        // Act
        Func<int> actual = () => ChargingCalculation.CalculateChargingHours(percentage, batterySize, chargerAmpere);

        // Assert
        Assert.ThrowsException<System.ArgumentOutOfRangeException>(() => actual());
    }
    
    // Integration test of DAO.GetChargingDbSchedule
    // Prerequesites: ChargingDBSchedule must be empty
    [TestMethod]
    public async Task GetChargingDBSchedule_test()
    {
        // Arrange
        int expected = 5;
        await dao.DeleteChargingDBSchedule();
        
        // Create chargingDBSchedules
        int chargerId = 5;
        List<ChargingDBSchedule> schedule = new List<ChargingDBSchedule>();

        ChargingDBSchedule ScheduleOne = new ChargingDBSchedule
        {
            ChargerId = chargerId,
            TimeStart = new DateTime(2023, 12, 11, 10, 0, 0)
        };
        ChargingDBSchedule ScheduleTwo = new ChargingDBSchedule
        {
            ChargerId = chargerId,
            TimeStart = new DateTime(2023, 12, 11, 11, 0, 0)
        };
        
        schedule.Add(ScheduleOne);
        schedule.Add(ScheduleTwo);
        
        // Add chargingDBSchedules to DB
        await context.ChargingDBSchedule.AddRangeAsync(schedule);
        await context.SaveChangesAsync();
        
        // Act
        List<ChargingDBSchedule> tasks = await dao.GetChargingDbSchedule(chargerId);
        int actual = tasks.Count;

        // Assert
        Assert.AreEqual(expected, actual, 0.001, "Wrong amount of tasks in DB");
    } 
    
}
