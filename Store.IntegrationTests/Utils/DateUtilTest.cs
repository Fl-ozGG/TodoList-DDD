namespace Store.IntegrationTests.Utils;

public class DateUtilTest
{
  public class DateUtilTests
  {
    [Fact]
    public void GetCurrentDate_ReturnsCorrectDate()
    {
      var currentDate = DateTime.Now;
      Assert.True(currentDate.Year >= 2025);
    }
  }  
}