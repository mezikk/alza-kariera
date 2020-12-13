using AlzaKariera.Classes;
using OpenQA.Selenium;
using System.Collections.Generic;

namespace AlzaKariera
{
    /// <summary>Webová stránka se seznamem nabídek zaměstnání daného oddělení</summary>
    public class DepartmentPage : Page
    {
        readonly By JobOfferContainer = By.XPath("//job-offer-list//*[@class='container']");
        IWebElement JobOffers;

        /// <summary>Constructor pro <see cref="DepartmentPage"/></summary>
        /// <param name="driver"><see cref="Driver"/></param>
        public DepartmentPage(Driver driver) : base(driver)
        {
            JobOffers = GetElement(JobOfferContainer);
        }

        /// <summary>Metoda vrací seznam nabídek zaměstnání pouze z načtené stránky. Další nabídky (tlačítko Další) nejsou implementovány</summary>
        /// <param name="department">Název oddělení</param>
        /// <returns>Mapa se seznamem nabídek</returns>
        public Dictionary<string, Offer> GetJobOffers(string department)
        {
            Driver.GetLogger().Info("Získávám seznam nabídek oddělení '{0}'", department);

            By jobOffersTitle = By.XPath("//career-position-detail-page//job-detail-header//h1[contains(text(), '" + department + "')]");
            GetElement(jobOffersTitle);

            Dictionary<string, Offer> offers = new Dictionary<string, Offer>();

            List<IWebElement> jobOfferList = GetElements(By.XPath(".//a"), JobOfferContainer, JobOffers);
            foreach (IWebElement jobOffer in jobOfferList)
            {
                string pathname = jobOffer.GetAttribute("pathname");
                string jobTitle = GetElement(By.XPath(".//a[@href='" + pathname + "']//h3[@class='job-title']"), JobOfferContainer, JobOffers).Text;
                if (offers.ContainsKey(pathname))
                    Driver.GetLogger().Warn("Duplicitní link {0} u nabídky {1} a {2}", pathname, jobTitle, offers[pathname].JobTitle);
                else
                {
                    Driver.GetLogger().Debug("Pathname linku: {0} a job-title: {1}", pathname, jobTitle);
                    offers.Add(pathname, new Offer(pathname, jobTitle));
                }
            }
            return offers;
        }

        /// <summary>Otevírá stránku s detailem konkrétní nabídky zaměstnání</summary>
        /// <param name="offer">Konkrétní nabídka ze seznamu nabídek</param>
        /// <param name="department">Název oddělení</param>
        /// <returns></returns>
        public JobPage OpenJobOfferDetail(Offer offer, string department)
        {
            Driver.GetLogger().Info("Otevírám stránku s detailem nabídky '{0}' oddělení '{1}'", offer.JobTitle, department);
            By departmentTitle = By.XPath("//job-detail-header//h1[contains(text(), '" + department + "')]");
            By jobOfferPath = By.XPath(".//a[@href='" + offer.Pathname + "']");

            GetElement(departmentTitle);
            IWebElement jobOfferElement = GetElement(jobOfferPath, JobOfferContainer, JobOffers);
            ClickOn(jobOfferElement);
            return new JobPage(Driver);
        }
    }
}