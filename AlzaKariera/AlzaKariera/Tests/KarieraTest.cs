using AlzaKariera.Classes;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;

namespace AlzaKariera.Tests
{
    class KarieraTest : TestClass
    {
        IWebDriver webDriver = new ChromeDriver();

        [SetUp]
        public void Setup()
        {
            webDriver.Navigate().GoToUrl("https://www.alza.cz/kariera");
            webDriver.Manage().Window.Maximize();
            webDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
        }

        [Test]
        public void Test1()
        {
            HomePage homePage = new HomePage(webDriver);

            string department = "IT Development";
            DepartmentPage departmentPage = homePage.SelectDepartment("it")
                                                    .SelectSubDepartment(department);
            Dictionary<string, Offer> jobOffers = departmentPage.GetJobOffers();

            foreach (KeyValuePair<string, Offer> offer in jobOffers)
            {
                departmentPage = departmentPage.SelectOfferDetail(offer.Value, department)
                                               .CheckOffer(offer.Value)
                                               .Back();
            }
        }

        [TearDown]
        public void TearDown()
        {
            //webDriver.Quit();
        }
    }
}
