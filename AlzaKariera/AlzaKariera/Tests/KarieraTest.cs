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

        [Test]
        public void Test1()
        {
            HomePage homePage = new HomePage(webDriver);

            string department = "IT Development";
            //string department = "Quality Assurance";
            DepartmentPage departmentPage = homePage.SelectDepartment("it")
                                                    .SelectSubDepartment(department);
            Dictionary<string, Offer> jobOffers = departmentPage.GetJobOffers(department);

            foreach (KeyValuePair<string, Offer> offer in jobOffers)
            {
                departmentPage = departmentPage.OpenJobOfferDetail(offer.Value, department)
                                               .CheckOffer(offer.Value)
                                               .BackToDepartmentPage();
            }
        }

        //[Test]
        public void Test2()
        {
            HomePage homePage = new HomePage(webDriver);

            string department = "Quality Assurance";
            DepartmentPage departmentPage = homePage.SelectDepartment("it")
                                                    .SelectSubDepartment(department);
            Dictionary<string, Offer> jobOffers = departmentPage.GetJobOffers(department);

            foreach (KeyValuePair<string, Offer> offer in jobOffers)
            {
                departmentPage = departmentPage.OpenJobOfferDetail(offer.Value, department)
                                               .CheckOffer(offer.Value)
                                               .BackToDepartmentPage();
            }
        }
    }
}
