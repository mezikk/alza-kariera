using AlzaKariera.Classes;
using AlzaKariera.Tests;
using NLog;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;
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
            string app = Environment.GetEnvironmentVariable("TestApplication");
            WebDriver = GetWebDriver();
            WebDriver.Navigate().GoToUrl(Properties.Apps[app].Url);
            WebDriver.Manage().Window.Maximize();
            WebDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
        }

        public void Quit()
        {
            WebDriver.Quit();
        }

        private IWebDriver GetWebDriver()
        {
            string browser = Environment.GetEnvironmentVariable("TestBrowser");
            Uri uri = new Uri(Properties.Grid.Url);
            bool isRemote = false;
            try
            {
                isRemote = Boolean.Parse(Environment.GetEnvironmentVariable("TestIsRemote"));
                Logger.Info("Nastavuji hodnotu isRemote na '" + isRemote + "'");
            }
            catch (Exception)
            {
                Logger.Warn("Nepodarilo se naparsovat hodnotu, budu nastavovat isRemote=false");
            }

            if (isRemote)
            {
                Logger.Info("Poustim remote driver na url '" + uri + "'");
                if (browser.Equals("chrome"))
                    return new RemoteWebDriver(uri, new ChromeOptions());
                else if(browser.Equals("firefox"))
                    return new RemoteWebDriver(uri, new FirefoxOptions());
                else
                    return new RemoteWebDriver(uri, new ChromeOptions());
            }
            else
            {
                Logger.Info("Poustim local driver");
                if (browser.Equals("chrome"))
                    return new OpenQA.Selenium.Chrome.ChromeDriver();
                else if (browser.Equals("firefox"))
                {
                    FirefoxDriverService service = FirefoxDriverService.CreateDefaultService();
                    service.Host = "::1";
                    return new OpenQA.Selenium.Firefox.FirefoxDriver(service);
                }
                else
                    return new OpenQA.Selenium.Chrome.ChromeDriver();
            }
        }
    }
}