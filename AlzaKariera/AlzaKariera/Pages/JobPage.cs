using AlzaKariera.Classes;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;

namespace AlzaKariera
{
    public class JobPage : Page
    {
        public static Dictionary<string, Person> People = new Dictionary<string, Person>();

        public JobPage(IWebDriver webDriver) : base(webDriver) { }

        public JobPage CheckOffer(Offer offer)
        {
            By xpathJobTitle = By.XPath("//job-detail-header//h1[contains(text(), '" + offer.JobTitle + "')]");
            By xpathPeople = By.XPath("//job-people//*[@class='card-container']");
            logger.Info("CheckOffer {0}", offer.Pathname);
            if (ElementIsDisplayed(xpathJobTitle) && ElementIsDisplayed(xpathPeople))
            {
                IWebElement webElement = Driver.FindElement(By.XPath("//job-detail-item"));
                if (webElement.Text.Length > 0)
                    logger.Info("Description {0}", webElement.Text);
                else
                    logger.Error("Nepodařilo se najít žádný popis pozice");

                webElement = Driver.FindElement(xpathPeople);

                foreach (IWebElement personWebElement in webElement.FindElements(By.XPath("./div")))
                {
                    string name = personWebElement.FindElement(By.ClassName("subtitle")).Text;
                    string description = personWebElement.FindElement(By.ClassName("description")).Text;
                    string picture = personWebElement.FindElement(By.XPath(".//*[contains(@style,'background-image')]")).GetCssValue("background-image");
                    if (People.TryGetValue(name, out Person person))
                    {
                        if (!person.Picture.Equals(picture))
                            logger.Error("Osoba {0} má odlišné odkazy na fotografii {1} a {2}", name, person.Picture, picture);
                        if (!person.Description.Equals(description))
                            logger.Error("Osoba {0} má odlišné popisy {1} a {2}", name, person.Description, description);
                    }
                    else
                        People.Add(name, new Person(name, description, picture));

                }
                return this;
            }
            else
                throw new Exception("Cannot find element");
        }

        public JobPage GetPersonList()
        {
            foreach (KeyValuePair<string, Person> person in People)
            {
                logger.Info(person.Value.Name);
                logger.Info(person.Value.Description);
                logger.Info(person.Value.Picture);
            }
            return this;
        }

        public DepartmentPage Back()
        {
            logger.Info("Getting back from detail offer page");
            Driver.Navigate().Back();
            return new DepartmentPage(Driver);
        }
    }
}
