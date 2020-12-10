using AlzaKariera.Classes;
using NLog;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;

namespace AlzaKariera
{
    public class Page
    {
        public static Logger logger;

        public Page(IWebDriver webDriver)
        {
            Driver = webDriver;

            if (logger == null)
            {
                LogManager.Configuration = new NLog.Config.XmlLoggingConfiguration(@"c:\Users\mezikk\source\repos\alza-kariera\AlzaKariera\AlzaKariera\Config\nlog.config");
                logger = LogManager.GetCurrentClassLogger();
            }
        }

        public IWebDriver Driver
        {
            get;
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
            return _getElements(by, null, null)[0];
        }

        public IWebElement GetElement(By by, IWebElement parentWebElement)
        {
            return _getElements(by, null, parentWebElement)[0];
        }

        public IWebElement GetElement(By by, By parent, IWebElement parentWebElement)
        {
            return _getElements(by, parent, parentWebElement)[0];
        }

        public List<IWebElement> GetElements(By by)
        {
            return _getElements(by, null, null);
        }

        public List<IWebElement> GetElements(By by, IWebElement parentWebElement)
        {
            return _getElements(by, null, parentWebElement);
        }

        public List<IWebElement> GetElements(By by, By parent, IWebElement parentWebElement)
        {
            return _getElements(by, parent, parentWebElement);
        }

        private List<IWebElement> _getElements(By by, By parent, IWebElement parentWebElement)
        {
            WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
            List<IWebElement> webElements;
            webElements = wait.Until(conditions =>
            {
                logger.Info("Trying to find element by {0}", by);
                int attempts = 0;
                while (attempts < 1)
                {
                    try
                    {
                        logger.Info("1");
                        if (parentWebElement == null)
                        {
                            return new List<IWebElement>(Driver.FindElements(by));
                            //logger.Info(webElements.Count);
                        }
                        else
                        {
                            return new List<IWebElement>(parentWebElement.FindElements(by));
                            //webElements.AddRange(parentWebElement.FindElements(by));
                            //logger.Info(webElements.Count);
                        }
                        //return webElements;
                    }
                    catch (StaleElementReferenceException)
                    {
                        logger.Error("StaleElementReferenceException");
                        if (!(parentWebElement == null))
                            parentWebElement = Driver.FindElement(parent);
                    }
                    //catch (NoSuchElementException ne)
                    //{
                    //    logger.Error("Cannot find element by {0}", by);
                    //    logger.Error(ne);
                    //    //get screenshot
                    //    Utils.SaveScreenshotAsFile(Driver, this.GetType().Name + ".png");
                    //}
                    attempts++;
                    
                }
                throw new Exception("Doslo k chybe hledani elementu");
            });
            if (webElements.Count > 0)
                return webElements;
            else
                throw new NoSuchElementException();
        }
    }
}