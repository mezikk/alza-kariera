using AlzaKariera.Classes;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Collections.Generic;

namespace AlzaKariera
{
    /// <summary>
    /// Webová stránka s detailem nabídky zaměstnání
    /// </summary>
    public class JobPage : Page
    {
        private static Dictionary<string, Person> People = new Dictionary<string, Person>();

        readonly By JobHeader = By.XPath("//job-detail-header//h1");
        readonly By JobPeople = By.XPath("//job-people//*[@class='card-container']");
        IWebElement PeopleContainer;

        /// <summary>
        /// Constructor pro <see cref="JobPage"/>
        /// </summary>
        /// <param name="driver"><see cref="Driver"/></param>
        public JobPage(Driver driver) : base(driver)
        {
            GetElement(JobHeader);
            PeopleContainer = GetElement(JobPeople);
        }

        /// <summary>
        /// Ověřuje jednu konkrétní nabídku
        /// </summary>
        /// <param name="offer">Detail s nabídkou zaměstnání</param>
        /// <returns><see cref="JobPage"/></returns>
        public JobPage CheckOffer(Offer offer)
        {
            Driver.GetLogger().Info("Ověřuji parametry nabídky '{0}' ('{1}')", offer.JobTitle, offer.Pathname);
            By jobTitle = By.XPath("//job-detail-header//h1[contains(text(), '" + offer.JobTitle + "')]");
            GetElement(jobTitle);

            IWebElement webElement = GetElement(By.XPath("//job-detail-item"));
            if (webElement.Text.Length > 0)
                Driver.GetLogger().Info("Popis nabídky je '{0}'", webElement.Text);
            else
                Driver.GetLogger().Error("Nepodařilo se najít žádný popis pozice");

            foreach (IWebElement personElement in GetElements(By.XPath("./div"), JobPeople, PeopleContainer))
            {
                string name = GetElement(By.ClassName("subtitle"), personElement).Text;
                string description = GetElement(By.ClassName("description"), personElement).Text;
                string picture = GetElement(By.XPath(".//*[contains(@style,'background-image')]"), personElement).GetCssValue("background-image");

                if (People.TryGetValue(name, out Person person))
                {
                    if (!person.Picture.Equals(picture))
                        Driver.GetLogger().Error("Osoba '{0}' má odlišné odkazy na fotografii '{1}' a '{2}'", name, person.Picture, picture);
                    if (!person.Description.Equals(description))
                        Driver.GetLogger().Error("Osoba '{0}' má odlišné popisy '{1}' a '{2}'", name, person.Description, description);
                }
                else {
                    Assert.IsTrue(name.Length > 0, "Jméno a příjmení osoby '" + name + "' není vyplněn");
                    Assert.IsTrue(description.Length > 0, "Popis osoby '" + description + "' není vyplněn");
                    Assert.IsTrue(picture.Length > 0, "Obrázek osoby '" + picture + "' není vyplněn");
                    People.Add(name, new Person(name, description, picture));
                }
            }
            return this;
        }

        /// <summary>
        /// Zaloguje seznam všech osob, které se můžou účastnit pohovoru
        /// </summary>
        /// <returns><see cref="JobPage"/></returns>
        public JobPage LogPersonList()
        {
            foreach (KeyValuePair<string, Person> person in People)
            {
                Driver.GetLogger().Info(person.Value.Name);
                Driver.GetLogger().Info(person.Value.Description);
                Driver.GetLogger().Info(person.Value.Picture);
            }
            return this;
        }

        /// <summary>
        /// Vrací se na stránku se seznamem pozic konkrétního oddělení
        /// </summary>
        /// <returns><see cref="DepartmentPage"/></returns>
        public DepartmentPage BackToDepartmentPage()
        {
            Driver.GetLogger().Info("Otevírám stránku se seznamem nabídek");
            Driver.GetWebDriver().Navigate().Back();
            return new DepartmentPage(Driver);
        }
    }
}
