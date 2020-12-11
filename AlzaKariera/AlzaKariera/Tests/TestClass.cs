using AlzaKariera.Classes;
using NUnit.Framework;

namespace AlzaKariera.Tests
{
    public class TestClass
    {
        public CustomDriver CustomDriver;

        public TestClass()
        {
        }

        [SetUp]
        public void InitDriver()
        {
            CustomDriver = new CustomDriver();
        }

        [TearDown]
        public void TearDown()
        {
            CustomDriver.GetDriver().Quit();
        }
    }
}