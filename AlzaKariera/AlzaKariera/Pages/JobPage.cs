using AlzaKariera.Classes;
using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;

namespace AlzaKariera
{
    public class JobPage : Page
    {
        public static Dictionary<string, Person> People = new Dictionary<string, Person>();

        readonly By JobHeader = By.XPath("//job-detail-header//h1");
        readonly By JobPeople = By.XPath("//job-people//*[@class='card-container']");
        readonly IWebElement PeopleContainer;

        public JobPage(IWebDriver webDriver) : base(webDriver)
        {
            GetElement(JobHeader);
            PeopleContainer = GetElement(JobPeople);
        }

        public JobPage CheckOffer(Offer offer)
        {
            By jobTitle = By.XPath("//job-detail-header//h1[contains(text(), '" + offer.JobTitle + "')]");
            GetElement(jobTitle);

            logger.Info("CheckOffer {0}", offer.Pathname);
            IWebElement webElement = GetElement(By.XPath("//job-detail-item"));
            if (webElement.Text.Length > 0)
                logger.Info("Description {0}", webElement.Text);
            else
                logger.Error("Nepodařilo se najít žádný popis pozice");

            foreach (IWebElement personElement in GetElements(By.XPath("./div"), JobPeople, PeopleContainer))
            {
                string name = GetElement(By.ClassName("subtitle"), personElement).Text;
                string description = GetElement(By.ClassName("description"), personElement).Text;
                string picture = GetElement(By.XPath(".//*[contains(@style,'background-image')]"), personElement).GetCssValue("background-image");

                if (People.TryGetValue(name, out Person person))
                {
                    if (!person.Picture.Equals(picture))
                        logger.Error("Osoba {0} má odlišné odkazy na fotografii {1} a {2}", name, person.Picture, picture);
                    if (!person.Description.Equals(description))
                        logger.Error("Osoba {0} má odlišné popisy {1} a {2}", name, person.Description, description);
                }
                else {
                    Assert.IsTrue(name.Length > 0, "Name of the person '" + name + "'is not filled");
                    Assert.IsTrue(description.Length > 0, "Description of the person '" + description + "' is not filled");
                    Assert.IsTrue(picture.Length > 0, "Picture of the person '" + picture + "' is not filled");
                    People.Add(name, new Person(name, description, picture));
                }
            }
            return this;
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

        public DepartmentPage BackToDepartmentPage()
        {
            logger.Info("Getting back from detail offer page");
            Driver.Navigate().Back();
            return new DepartmentPage(Driver);
        }
    }
}
