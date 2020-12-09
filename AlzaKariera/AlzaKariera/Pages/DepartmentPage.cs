using AlzaKariera.Classes;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Threading;

namespace AlzaKariera
{
    public class DepartmentPage : Page
    {
        public DepartmentPage(IWebDriver driver) : base(driver) { }

        public IWebElement OfferList => Driver.FindElement(By.XPath("//job-offer-list//*[@class='container']"));

        public Dictionary<string, Offer> GetJobOffers()
        {
            Thread.Sleep(1000);
            //new WebDriverWait(driver, TimeSpan.FromSeconds(10)).Until(d => ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").Equals("complete"));
            Dictionary<string, Offer> offers = new Dictionary<string, Offer>();
            foreach (IWebElement offer in OfferList.FindElements(By.XPath(".//a")))
            {
                string pathname = offer.GetAttribute("pathname");
                string jobTitle = offer.FindElement(By.XPath(".//h3[@class='job-title']")).Text;
                if (!offers.ContainsKey(pathname))
                {
                    offers.Add(pathname, new Offer(pathname, jobTitle));
                    logger.Info("Pathname linku: {0} a job-title: {1}", pathname, jobTitle);
                }
                else
                    new Exception("Duplicitní link");
            }
            return offers;
        }

        public JobPage SelectOfferDetail(Offer offer)
        {
            By xpathJobOffer = By.XPath(".//*[@href='" + offer.Pathname + "']");
            By xpathDepartmentTitle = By.XPath("//job-detail-header//h1[contains(text(), 'IT Development')]");

            if (ElementIsDisplayed(xpathJobOffer) && ElementIsDisplayed(xpathDepartmentTitle))
            {
                logger.Info("Trying to find element by xpath {0}", xpathJobOffer);
                IWebElement link = OfferList.FindElement(xpathJobOffer);
                link.Click();
                return new JobPage(Driver);
            }
            else
                throw new Exception("Cannot find element");
        }
    }
}
