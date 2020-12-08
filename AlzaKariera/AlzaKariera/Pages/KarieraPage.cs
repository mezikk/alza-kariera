using OpenQA.Selenium;

namespace AlzaKariera
{
    public class KarieraPage
    {
        public KarieraPage(IWebDriver webDriver)
        {
            Driver = webDriver;
        }

        public IWebDriver Driver { get; }

        public IWebElement DepartmentsList => Driver.FindElement(By.ClassName("departments-list"));
        
        public IWebElement OfferList => Driver.FindElement(By.XPath("//job-offer-list//*[@class='container']"));

        public KarieraPage SelectDepartment(string department)
        {
            IWebElement input = DepartmentsList.FindElement(By.XPath("//div/input[@value='" + department + "']"));
            string order = input.GetAttribute("id");
            IWebElement img = DepartmentsList.FindElement(By.XPath("//*[@for='" + order + "']/img"));
            img.Click();
            return this;
        }

        public KarieraPage SelectSubDepartment(string subDepartment)
        {
            IWebElement lnk = OfferList.FindElement(By.XPath("//*[contains(text(), '" + subDepartment + "')]"));
            lnk.Click();
            return this;
        }

    }
}
