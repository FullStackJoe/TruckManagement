using WebApplication1.Shared;

namespace Tests;

[TestClass]
public class BackEndTests
{
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
}