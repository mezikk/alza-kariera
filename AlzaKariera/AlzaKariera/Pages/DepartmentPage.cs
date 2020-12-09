using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace AlzaKariera
{
    public class DepartmentPage : Page
    {
        public DepartmentPage(IWebDriver driver) : base(driver) { }

        public IWebElement OfferList => Driver.FindElement(By.XPath("//job-offer-list//*[@class='container']"));

        public void CheckJobOffers()
        {
            Thread.Sleep(1000);
            //new WebDriverWait(driver, TimeSpan.FromSeconds(10)).Until(d => ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").Equals("complete"));
            List<string> links = new List<string>();
            foreach (IWebElement offer in OfferList.FindElements(By.XPath(".//a")))
            {
                string path = offer.GetAttribute("pathname");
                if (!links.Contains(path))
                {
                    links.Add(path);
                    logger.Info("Pathname linku: {0}", path);
                }
                else
                    new Exception("Duplicitní link");
            }
            foreach(string jobLink in links)
            {
                By xpath = By.XPath(".//*[@href='" + jobLink + "']");
                logger.Info("Trying to find elemeent by xpath {0}", xpath);
                IWebElement link = OfferList.FindElement(xpath);
                link.Click();
                JobPage jobPage = new JobPage(Driver);
                jobPage.CheckOffer(jobLink).Back();
            }
        }
    }
}
