using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;

namespace AlzaKariera.Tests
{
    class KarieraTest
    {
        IWebDriver webDriver = new ChromeDriver();

        [SetUp]
        public void Setup()
        {
            webDriver.Navigate().GoToUrl("https://www.alza.cz/kariera");
            webDriver.Manage().Window.Maximize();
            webDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
        }

        [Test]
        public void Test1()
        {
            HomePage karieraPage = new HomePage(webDriver);
            karieraPage.SelectDepartment("it")
                        //.SelectSubDepartment("Quality Assurance");
                        .SelectSubDepartment("IT Development")
                        .CheckJobOffers();
        }

        [TearDown]
        public void TearDown()
        {
            //webDriver.Quit();
        }
    }
}
