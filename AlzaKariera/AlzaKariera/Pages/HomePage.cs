using AlzaKariera.Classes;
using OpenQA.Selenium;

namespace AlzaKariera
{
    /// <summary>Webová stránka se seznamem oddělení <see href="https://www.alza.cz/kariera"/></summary>
    public class HomePage : Page
    {
        readonly By Departments = By.ClassName("departments-list");
        readonly By SubDepartments = By.XPath("//job-offer-list//*[@class='container']");
        IWebElement DepartmentsList;

        /// <summary>Constructor pro <see cref="HomePage"/></summary>
        /// <param name="driver"><see cref="Driver"/></param>
        public HomePage(Driver driver) : base(driver)
        {
            DepartmentsList = GetElement(Departments);
        }

        /// <summary>Vybere oddělení z tabulky/seznamu všech oddělení</summary>
        /// <param name="department">Název oddělení</param>
        /// <returns><see cref="HomePage"/></returns>
        public HomePage SelectDepartment(string department)
        {
            Driver.GetLogger().Info("Filtruji nabídky oddělení '{0}", department);
            IWebElement input = GetElement(By.XPath("//div/input[@value='" + department + "']"));
            IWebElement img = GetElement(By.XPath(".//*[@for='" + input.GetAttribute("id") + "']/img"), DepartmentsList);
            ClickOn(img);
            return this;
        }

        /// <summary>Otevírá stránku se seznamem pozic vybraného oddělení</summary>
        /// <param name="subDepartment">Název oddělení</param>
        /// <returns>Stránka se seznamem pozic vybraného oddělení</returns>
        public DepartmentPage SelectSubDepartment(string subDepartment)
        {
            Driver.GetLogger().Info("Otevírám stránku s nabídkami oddělení '{0}'", subDepartment);
            IWebElement subDepartments = GetElement(SubDepartments);
            IWebElement subDepartmentLink = GetElement(By.XPath(".//*[contains(text(), '" + subDepartment + "')]"), subDepartments);
            ClickOn(subDepartmentLink);
            return new DepartmentPage(Driver);
        }
    }
}