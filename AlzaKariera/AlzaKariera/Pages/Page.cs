using NLog;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;

namespace AlzaKariera
{
    public class Page
    {
        public static Logger logger;

        public Page(IWebDriver webDriver)
        {
            if (logger == null)
            {
                LogManager.Configuration = new NLog.Config.XmlLoggingConfiguration(@"c:\Users\mezikk\source\repos\alza-kariera\AlzaKariera\AlzaKariera\Config\nlog.config");
                logger = LogManager.GetCurrentClassLogger();
            }
            Driver = webDriver;
        }

        public IWebDriver Driver
        {
            get;
        }

        public bool ElementIsDisplayed(By by)
        {
            WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
            return wait.Until(conditions =>
            {
                try
                {
                    IWebElement elementToBeDisplayed = Driver.FindElement(by);
                    return elementToBeDisplayed.Displayed;
                }
                catch (Exception)
                {
                    return false;
                }
            });
        }
    }
}