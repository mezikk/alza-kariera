using AlzaKariera.Classes;
using NUnit.Framework;

namespace AlzaKariera.Tests
{
    public class TestClass
    {
        public Driver Driver;

        public TestClass()
        {
        }

        public void InitDriver(TestClass testClass)
        {
            Driver = new Driver(testClass);
        }

        [TearDown]
        public void TearDown()
        {
            Driver.GetWebDriver().Quit();
        }
    }
}