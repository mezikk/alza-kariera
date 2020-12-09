using AlzaKariera.Classes;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;

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
            HomePage homePage = new HomePage(webDriver);

            DepartmentPage departmentPage = homePage.SelectDepartment("it")
                                                    .SelectSubDepartment("IT Development");
            Dictionary<string, Offer> offers = departmentPage.GetJobOffers();

            foreach (KeyValuePair<string, Offer> offer in offers)
            {
                departmentPage = departmentPage.SelectOfferDetail(offer.Value)
                                               .CheckOffer(offer.Value)
                                               .Back();
            }
        }

        [TearDown]
        public void TearDown()
        {
            webDriver.Quit();
        }
    }
}
