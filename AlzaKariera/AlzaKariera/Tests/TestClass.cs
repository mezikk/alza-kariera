using NLog;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;

namespace AlzaKariera.Tests
{
    class TestClass
    {
        public static Logger logger;
        public IWebDriver webDriver;

        public TestClass()
        {
            if (logger == null)
            {
                LogManager.Configuration = new NLog.Config.XmlLoggingConfiguration(@"c:\Users\mezikk\source\repos\alza-kariera\AlzaKariera\AlzaKariera\Config\nlog.config");
                logger = LogManager.GetCurrentClassLogger();
            }
        }

        [SetUp]
        public void LoadConfigFile()
        {
            webDriver = new ChromeDriver();
            logger.Info("load config");
            webDriver.Navigate().GoToUrl("https://www.alza.cz/kariera");
            //webDriver.Manage().Window.Maximize();
            webDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
        }

        [TearDown]
        public void TearDown()
        {
            webDriver.Quit();
        }
    }
}
