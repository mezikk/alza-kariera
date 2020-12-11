using AlzaKariera.Classes;
using OpenQA.Selenium;
using System.Collections.Generic;

namespace AlzaKariera
{
    public class DepartmentPage : Page
    {
        readonly By JobOfferContainer = By.XPath("//job-offer-list//*[@class='container']");
        IWebElement JobOffers;

        public DepartmentPage(CustomDriver customDriver) : base(customDriver)
        {
            JobOffers = GetElement(JobOfferContainer);
        }

        public Dictionary<string, Offer> GetJobOffers(string department)
        {
            By jobOffersTitle = By.XPath("//career-position-detail-page//job-detail-header//h1[contains(text(), '" + department + "')]");
            GetElement(jobOffersTitle);

            Dictionary<string, Offer> offers = new Dictionary<string, Offer>();

            List<IWebElement> jobOfferList = GetElements(By.XPath(".//a"), JobOfferContainer, JobOffers);
            foreach (IWebElement jobOffer in jobOfferList)
            {
                string pathname = jobOffer.GetAttribute("pathname");
                string jobTitle = GetElement(By.XPath(".//a[@href='" + pathname + "']//h3[@class='job-title']"), JobOfferContainer, JobOffers).Text;
                if (offers.ContainsKey(pathname))
                    CustomDriver.GetLogger().Warn("Duplicitní link {0} u nabídky {1} a {2}", pathname, jobTitle, offers[pathname].JobTitle);
                else
                {
                    CustomDriver.GetLogger().Info("Pathname linku: {0} a job-title: {1}", pathname, jobTitle);
                    offers.Add(pathname, new Offer(pathname, jobTitle));
                }
            }
            return offers;
        }

        public JobPage OpenJobOfferDetail(Offer offer, string department)
        {
            By departmentTitle = By.XPath("//job-detail-header//h1[contains(text(), '" + department + "')]");
            By jobOfferPath = By.XPath(".//a[@href='" + offer.Pathname + "']");

            GetElement(departmentTitle);
            IWebElement jobOfferElement = GetElement(jobOfferPath, JobOfferContainer, JobOffers);
            ClickOn(jobOfferElement);
            return new JobPage(CustomDriver);
        }
    }
}