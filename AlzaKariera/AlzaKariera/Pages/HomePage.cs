using OpenQA.Selenium;
using System;

namespace AlzaKariera
{
    public class HomePage : Page
    {
        public HomePage(IWebDriver driver) : base(driver) { }

        public IWebElement DepartmentsList => Driver.FindElement(By.ClassName("departments-list"));

        public IWebElement OfferList => Driver.FindElement(By.XPath("//job-offer-list//*[@class='container']"));

        public HomePage SelectDepartment(string department)
        {
            By xpath = null;
            xpath = By.XPath("//div/input[@value='" + department + "']");
            logger.Info("Trying to find elemeent by xpath {0}", xpath);
            IWebElement input = DepartmentsList.FindElement(xpath);
            string order = input.GetAttribute("id");

            logger.Info("Trying to find elemeent by xpath {0}", xpath);
            xpath = By.XPath(".//*[@for='" + order + "']/img");
            IWebElement img = DepartmentsList.FindElement(xpath);
            img.Click();
            return this;
        }

        public DepartmentPage SelectSubDepartment(string subDepartment)
        {
            By xpath = By.XPath(".//*[contains(text(), '" + subDepartment + "')]");
            logger.Info("Trying to find elemeent by xpath {0}", xpath);
            IWebElement lnk = OfferList.FindElement(xpath);
            lnk.Click();
            return new DepartmentPage(Driver);
        }

    }
}
