using AlzaKariera.Tests;
using NLog;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.Events;
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
        public int PageOrder;

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

        public string GetLogDir()
        {
            return LogDir;
        }

        private void LoadProperties(string propertiesFile)
        {
            var deserializer = new DeserializerBuilder().WithNamingConvention(UnderscoredNamingConvention.Instance)
                                                        .Build();
            Properties = deserializer.Deserialize<Properties>(File.ReadAllText(propertiesFile));
        }

        public IWebDriver GetWebDriver()
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
            WebDriver = InitializeWebDriver();
            WebDriver.Navigate().GoToUrl(Properties.Apps[app].Url);
            WebDriver.Manage().Window.Maximize();
            WebDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);

            EventFiringWebDriver eventFiringWebDriver = new EventFiringWebDriver(WebDriver);
            eventFiringWebDriver.ElementClicking += EventFiringWebDriver_createScreenShot_click;
            eventFiringWebDriver.NavigatingForward += EventFiringWebDriver_createScreenShot_forward;
            eventFiringWebDriver.NavigatingBack += EventFiringWebDriver_createScreenShot_back;
            WebDriver = eventFiringWebDriver;
        }

        private void EventFiringWebDriver_createScreenShot_click(object sender, WebElementEventArgs args)
        {
            ((IJavaScriptExecutor)args.Driver).ExecuteScript("arguments[0].style.outline='4px solid red'", args.Element);
            string screenShotFile = GetLogDir() + "/screenshot_" + (++PageOrder).ToString().PadLeft(2, '0') + ".png";
            Utils.SaveScreenshotAsFile(GetWebDriver(), screenShotFile);
        }

        private void EventFiringWebDriver_createScreenShot_forward(object sender, WebDriverNavigationEventArgs args)
        {
            string screenShotFile = GetLogDir() + "/screenshot_" + (++PageOrder).ToString().PadLeft(2, '0') + ".png";
            Utils.SaveScreenshotAsFile(GetWebDriver(), screenShotFile);
        }

        private void EventFiringWebDriver_createScreenShot_back(object sender, WebDriverNavigationEventArgs args)
        {
            string screenShotFile = GetLogDir() + "/screenshot_" + (++PageOrder).ToString().PadLeft(2, '0') + ".png";
            Utils.SaveScreenshotAsFile(GetWebDriver(), screenShotFile);
        }

        public void Quit()
        {
            WebDriver.Quit();
        }

        private IWebDriver InitializeWebDriver()
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