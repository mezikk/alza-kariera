using AlzaKariera.Classes;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;

namespace AlzaKariera
{
    /// <summary>Předpis pro všechny stránky</summary>
    public class Page
    {
        /// <summary><see cref="Driver"/></summary>
        protected Driver Driver;

        /// <summary>Constructor pro <see cref="Page"/></summary>
        /// <param name="driver"><see cref="Driver"/></param>
        public Page(Driver driver)
        {
            Driver = driver;
            PageIsLoaded();
        }


        //public IWebElement GetVisibleElement(By by)
        //{
        //    return GetVisibleElement(by, null);
        //}

        //public IWebElement GetVisibleElement(By by, IWebElement parentWebElement)
        //{
        //    IWebElement webElement = GetElement(by, parentWebElement);
        //    if (!webElement.Displayed)
        //    {
        //        logger.Error("Element by {0} is not visible", by);
        //        throw new Exception();
        //        //get screenshot
        //    }
        //    return webElement;
        //}

        /// <summary>Hledá element dle <paramref name="by"/></summary>
        /// <param name="by">Podle čeho se bude hledat <see cref="By"/></param>
        /// <returns>Hledaný element <see cref="IWebElement"/></returns>
        public IWebElement GetElement(By by)
        {
            return getElements(by, null, null)[0];
        }

        /// <summary>Hledá element dle <paramref name="by"/> a parenta <paramref name="parentWebElement"/></summary>
        /// <param name="by">Podle čeho se bude hledat <see cref="By"/></param>
        /// <param name="parentWebElement">Parent element <see cref="IWebElement"/></param>
        /// <returns>Hledaný element <see cref="IWebElement"/></returns>
        public IWebElement GetElement(By by, IWebElement parentWebElement)
        {
            return getElements(by, null, parentWebElement)[0];
        }

        /// <summary>
        /// Hledá element dle <paramref name="by"/>, parenta <paramref name="parentWebElement"/> a <paramref name="parent"/> (<see cref="By"/>)
        /// </summary>
        /// <param name="by">Podle čeho se bude hledat <see cref="By"/></param>
        /// <param name="parent">Podle čeho se bude případně parent znovu hledat <see cref="By"/></param>
        /// <param name="parentWebElement">Parent element <see cref="IWebElement"/></param>
        /// <returns>Hledaný element <see cref="IWebElement"/></returns>
        public IWebElement GetElement(By by, By parent, IWebElement parentWebElement)
        {
            return getElements(by, parent, parentWebElement)[0];
        }

        /// <summary>
        /// Hledá všechny elementy dle <paramref name="by"/>, parenta <paramref name="parentWebElement"/> a <paramref name="parent"/> (<see cref="By"/>)
        /// </summary>
        /// <param name="by">Podle čeho se bude hledat <see cref="By"/></param>
        /// <param name="parent">Podle čeho se bude případně parent znovu hledat <see cref="By"/></param>
        /// <param name="parentWebElement">Parent element <see cref="IWebElement"/></param>
        /// <returns>Seznam nalezených elementů <see cref="List{IWebElement}"/></returns>
        public List<IWebElement> GetElements(By by, By parent, IWebElement parentWebElement)
        {
            return getElements(by, parent, parentWebElement);
        }

        /// <summary>
        /// Hledá všechny elementy dle <paramref name="by"/>, parenta <paramref name="parentWebElement"/> a <paramref name="parent"/> (<see cref="By"/>)
        /// </summary>
        /// <param name="by">Podle čeho se bude hledat <see cref="By"/></param>
        /// <param name="parent">Podle čeho se bude případně parent znovu hledat <see cref="By"/></param>
        /// <param name="parentWebElement">Parent element <see cref="IWebElement"/></param>
        /// <returns>Seznam nalezených elementů <see cref="List{IWebElement}"/></returns>
        /// <exception cref="CustomException">Pokud se nenajde žádný element</exception>
        private List<IWebElement> getElements(By by, By parent, IWebElement parentWebElement)
        {
            WebDriverWait wait = new WebDriverWait(Driver.GetWebDriver(), TimeSpan.FromSeconds(10));
            List<IWebElement> webElements;
            webElements = wait.Until(conditions =>
            {
                Driver.GetLogger().Info("Hledám element '{0}'", by);
                int attempts = 0;
                while (attempts < 4)
                {
                    try
                    {
                        if (parentWebElement == null)
                        {
                            return new List<IWebElement>(Driver.GetWebDriver().FindElements(by));
                        }
                        else
                        {
                            return new List<IWebElement>(parentWebElement.FindElements(by));
                        }
                    }
                    catch (StaleElementReferenceException)
                    {
                        Driver.GetLogger().Error("StaleElementReferenceException, opakuji hledání");
                        if (!(parentWebElement == null))
                            parentWebElement = Driver.GetWebDriver().FindElement(parent);
                    }
                    attempts++;

                }
                throw new CustomException(Driver, "Došlo k chybě při hledání elementu '" + by + "'");
            });
            if (webElements.Count > 0)
                return webElements;
            else
                throw new CustomException(Driver, "Nepodařilo se nalézt element '" + by + "'");
        }

        /// <summary>Volá metodu <see cref="IWebElement.Click()"/> na elementu</summary>
        /// <param name="element"><see cref="IWebElement"/></param>
        public void ClickOn(IWebElement element)
        {
            Driver.GetLogger().Info("Klikám na element '{0}'", element.Text);
            element.Click();
        }

        /// <summary>Zjišťuje, zda je stránka načtená. Použito v konstruktoru <see cref="Page"/></summary>
        /// <returns><c>true</c> stránka je ve stavu <c>complete</c>, <c>false</c> v opačném případě</returns>
        public bool PageIsLoaded()
        {
            WebDriverWait wait = new WebDriverWait(Driver.GetWebDriver(), TimeSpan.FromSeconds(10));
            wait.Until(conditions =>
            {
                return ((IJavaScriptExecutor)Driver.GetWebDriver()).ExecuteScript("return document.readyState").Equals("complete");
            });
            return false;
        }
    }
}