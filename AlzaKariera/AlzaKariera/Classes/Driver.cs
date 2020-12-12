using AlzaKariera.Classes;
using AlzaKariera.Tests;
using NLog;
using OpenQA.Selenium;
using System;
using System.IO;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace AlzaKariera.Classes
{
    public class Driver
    {
        IWebDriver WebDriver;
        Logger Logger;
        Properties Properties;
        TestClass TestClass;
        string LogDir;

        public Driver(TestClass testClass)
        {
            var propertiesFile = Environment.GetEnvironmentVariable("PropertiesFile");
            TestClass = testClass;
            LoadProperties(propertiesFile);
            InitLogging();
            InitWebDriver();
        }

        public Logger GetLogger()
        {
            return Logger;
        }

        private void LoadProperties(string propertiesFile)
        {
            var deserializer = new DeserializerBuilder().WithNamingConvention(UnderscoredNamingConvention.Instance)
                                                        .Build();
            Properties = deserializer.Deserialize<Properties>(File.ReadAllText(propertiesFile));
        }

        public IWebDriver GetDriver()
        {
            return WebDriver;
        }
        private void InitLogging()
        {
            if (Logger == null)
            {
                string testFolder = TestClass.GetType().FullName.Replace(".", "/") + "/" + DateTime.Now.ToString("yyyy-MM-dd_hh-mm-ss");
                LogDir = Properties.Logs.Folder + "/" + testFolder;
                LogManager.Configuration = new NLog.Config.XmlLoggingConfiguration(Properties.Logs.Config);
                LogManager.Configuration.Variables["basedir"] = Properties.Logs.Folder;
                LogManager.Configuration.Variables["testfolder"] = testFolder;
                Logger = LogManager.GetCurrentClassLogger();
            }
        }

        private void InitWebDriver()
        {
            WebDriver = new OpenQA.Selenium.Chrome.ChromeDriver();
            WebDriver.Navigate().GoToUrl(Properties.Apps["kariera"].Url);
            WebDriver.Manage().Window.Maximize();
            WebDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
        }

        public void Quit()
        {
            WebDriver.Quit();
        }
    }
}