using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Diagnostics;

namespace AlzaKariera
{
    public class JobPage : Page
    {
        public JobPage(IWebDriver webDriver) : base(webDriver) { }

        private string link;
        public JobPage CheckOffer(string relink)
        {
            link = relink;
            if (ElementIsDisplayed(By.XPath("//job-detail-item")))
            {
                IWebElement webElement = Driver.FindElement(By.XPath("//job-detail-item"));
                if (webElement.Text.Length > 0)
                    logger.Info("Description {0}", webElement.Text);
                else
                    logger.Error("Nepodařilo se najít žádný popis pozice");
            }
            return this;
        }

        public DepartmentPage Back()
        {
            logger.Info("Getting back from {0}", link);
            Driver.Navigate().Back();
            return new DepartmentPage(Driver);
        }
    }
}
