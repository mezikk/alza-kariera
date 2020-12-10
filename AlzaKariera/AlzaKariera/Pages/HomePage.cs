using OpenQA.Selenium;

namespace AlzaKariera
{
    public class HomePage : Page
    {
        readonly By Departments = By.ClassName("departments-list");
        readonly By SubDepartments = By.XPath("//job-offer-list//*[@class='container']");
        readonly IWebElement DepartmentsList;

        public HomePage(IWebDriver webDriver) : base(webDriver)
        {
            //logger.Info(this.GetType().FullName);
            DepartmentsList = GetElement(Departments);
        }

        public HomePage SelectDepartment(string department)
        {
            IWebElement input = GetElement(By.XPath("//div/input[@value='" + department + "']"));
            IWebElement img = GetElement(By.XPath(".//*[@for='" + input.GetAttribute("id") + "']/img"), DepartmentsList);
            img.Click();
            return this;
        }

        public DepartmentPage SelectSubDepartment(string subDepartment)
        {
            IWebElement subDepartmentsElement = GetElement(SubDepartments);
            IWebElement subDepartmetnLinkElement = GetElement(By.XPath("//*[contains(text(), '" + subDepartment + "')]"), subDepartmentsElement);
            subDepartmetnLinkElement.Click();
            return new DepartmentPage(Driver);
        }
    }
}
