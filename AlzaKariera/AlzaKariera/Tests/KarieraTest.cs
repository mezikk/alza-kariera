using AlzaKariera.Classes;
using NUnit.Framework;
using System.Collections.Generic;

namespace AlzaKariera.Tests
{
    class KarieraTest : TestClass
    {
        /// <summary>
        /// Test ověřuje u všech nabídek oddělení IT Development, že jsou správně vyplněné (vyplněný popis pozice, fotografie lidí,
        /// které potkáme na pohovoru a jejich krátký popisek) a zjisťuje, zda lidé, které v průběhu výběrového řízení potkáme,
        /// jsou shodní (fotka / popisek) pro všechny inzeráty na daného oddělení
        /// </summary>
        [Test]
        public void TestITDevelopment()
        {
            InitDriver(this);
            HomePage homePage = new HomePage(Driver);

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

        /// <summary>
        /// Test ověřuje u všech nabídek oddělení QA, že jsou správně vyplněné (vyplněný popis pozice, fotografie lidí,
        /// které potkáme na pohovoru a jejich krátký popisek) a zjisťuje, zda lidé, které v průběhu výběrového řízení potkáme,
        /// jsou shodní (fotka / popisek) pro všechny inzeráty na daného oddělení
        /// </summary>
        [Test]
        public void TestQA()
        {
            InitDriver(this);
            HomePage homePage = new HomePage(Driver);

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
