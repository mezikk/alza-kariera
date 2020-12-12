using AlzaKariera.Classes;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;

namespace AlzaKariera
{
    public abstract class Page
    {
        protected Driver Driver;

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

        public IWebElement GetElement(By by)
        {
            return getElements(by, null, null)[0];
        }

        public IWebElement GetElement(By by, IWebElement parentWebElement)
        {
            return getElements(by, null, parentWebElement)[0];
        }

        public IWebElement GetElement(By by, By parent, IWebElement parentWebElement)
        {
            return getElements(by, parent, parentWebElement)[0];
        }

        public List<IWebElement> GetElements(By by)
        {
            return getElements(by, null, null);
        }

        public List<IWebElement> GetElements(By by, IWebElement parentWebElement)
        {
            return getElements(by, null, parentWebElement);
        }

        public List<IWebElement> GetElements(By by, By parent, IWebElement parentWebElement)
        {
            return getElements(by, parent, parentWebElement);
        }

        private List<IWebElement> getElements(By by, By parent, IWebElement parentWebElement)
        {
            WebDriverWait wait = new WebDriverWait(Driver.GetWebDriver(), TimeSpan.FromSeconds(10));
            List<IWebElement> webElements;
            webElements = wait.Until(conditions =>
            {
                Driver.GetLogger().Info("Trying to find element by {0}", by);
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
                throw new CustomException(Driver, "Doslo k chybe při hledání elementu '" + by + "'");
            });
            if (webElements.Count > 0)
                return webElements;
            else
                throw new CustomException(Driver, "Nepodařilo se nalézt element '" + by + "'");
        }

        public void ClickOn(IWebElement element)
        {
            Driver.GetLogger().Info("Clicking on element '" + element.Text + "'");
            element.Click();
        }

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