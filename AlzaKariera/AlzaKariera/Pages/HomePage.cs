using AlzaKariera.Classes;
using OpenQA.Selenium;

namespace AlzaKariera
{
    public class HomePage : Page
    {
        readonly By Departments = By.ClassName("departments-list");
        readonly By SubDepartments = By.XPath("//job-offer-list//*[@class='container']");
        readonly IWebElement DepartmentsList;

        public HomePage(CustomDriver customDriver) : base(customDriver)
        {
            DepartmentsList = GetElement(Departments);
        }

        public HomePage SelectDepartment(string department)
        {
            IWebElement input = GetElement(By.XPath("//div/input[@value='" + department + "']"));
            IWebElement img = GetElement(By.XPath(".//*[@for='" + input.GetAttribute("id") + "']/img"), DepartmentsList);
            ClickOn(img);
            return this;
        }

        public DepartmentPage SelectSubDepartment(string subDepartment)
        {
            IWebElement subDepartments = GetElement(SubDepartments);
            IWebElement subDepartmentLink = GetElement(By.XPath(".//*[contains(text(), '" + subDepartment + "')]"), subDepartments);
            ClickOn(subDepartmentLink);
            return new DepartmentPage(CustomDriver);
        }
    }
}