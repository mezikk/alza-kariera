using AlzaKariera.Classes;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Threading;

namespace AlzaKariera
{
    public class DepartmentPage : Page
    {
        readonly By JobOfferContainer = By.XPath("//job-offer-list//*[@class='container']");
        IWebElement JobOffers;

        public DepartmentPage(IWebDriver driver) : base(driver)
        {
            //logger.Info(this.GetType().FullName);
            //JobOffers = GetElement(JobOfferContainer);
            GetElement(By.XPath("//career-footer"));
        }

        public Dictionary<string, Offer> GetJobOffers()
        {
            Dictionary<string, Offer> offers = new Dictionary<string, Offer>();

            //JobOffers = GetElement(JobOfferContainer);
            //Thread.Sleep(1000);
            List<IWebElement> offerList = GetElements(By.XPath("//job-offer-list//*[@class='container']//a"));
            List<string> pathnameList = new List<string>();
            logger.Info(offerList.Count);
            //foreach (IWebElement offer in offerList)
            //{
                IWebElement aa = GetElement(By.XPath("//job-offer-list//*[@class='container']//a"));
            { 
            logger.Info("iterate");
                string pathname = aa.GetAttribute("pathname");
                logger.Info(pathname);
                pathnameList.Add(pathname);
            }

            foreach (string pathname in pathnameList)
            {
                //string jobTitle = GetElement(By.XPath("//job-offer-list//*[@class='container']//a[@href='" + pathname + "']//h3[@class='job-title']")).Text;
                string jobTitle = Driver.FindElement(By.XPath("//job-offer-list//*[@class='container']//a[@href='" + pathname + "']//h3[@class='job-title']")).Text;
                if (offers.ContainsKey(pathname))
                {
                    logger.Error("Duplicitní link {0} u nabídky {1} a {2}", pathname, jobTitle, offers[pathname].JobTitle);
                    throw new Exception();
                }
                else
                {
                    logger.Info("Pathname linku: {0} a job-title: {1}", pathname, jobTitle);
                    offers.Add(pathname, new Offer(pathname, jobTitle));
                }
            }
            return offers;
        }

        public JobPage SelectOfferDetail(Offer offer, string department)
        {
            By xpathDepartmentTitle = By.XPath("//job-detail-header//h1[contains(text(), '" + department + "')]");
            By xpathJobOffer = By.XPath(".//*[@href='" + offer.Pathname + "']");

            GetElement(xpathDepartmentTitle);
            IWebElement jobOfferElement = GetElement(xpathJobOffer, JobOfferContainer, JobOffers);
            jobOfferElement.Click();
            return new JobPage(Driver);
        }
    }
}
