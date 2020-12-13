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
    /// <summary>
    /// Třída slouží k vytvoření instance webdriveru, nastavení logování, načtení properties během inicializace testu
    /// </summary>
    public class Driver
    {
        IWebDriver WebDriver;
        Logger Logger;
        Properties Properties;
        TestClass TestClass;
        string LogDir;
        int PageOrder;

        /// <summary>
        /// Inicializace Driveru
        /// </summary>
        /// <param name="testClass">Testovací třída, která Driver inicializuje</param>
        public Driver(TestClass testClass)
        {
            var propertiesFile = Environment.GetEnvironmentVariable("PropertiesFile");
            TestClass = testClass;
            LoadProperties(propertiesFile);
            InitLogging();
            InitWebDriver();
        }

        /// <summary>
        /// Vrací instanci objektu typu <see cref="Logger"/> určeného pro logování
        /// </summary>
        /// <returns><see cref="Logger"/></returns>
        public Logger GetLogger()
        {
            return Logger;
        }

        /// <summary>
        /// Vrací složku, do které se ukládají logy testu
        /// </summary>
        /// <returns>Název složky</returns>
        public string GetLogDir()
        {
            return LogDir;
        }

        /// <summary>
        /// Načítá properties uložené v yaml souboru
        /// </summary>
        /// <param name="propertiesFile">Plná cesta souboru</param>
        private void LoadProperties(string propertiesFile)
        {
            Logger.Info("Načítám properties z souboru '" + propertiesFile + "'");
            var deserializer = new DeserializerBuilder().WithNamingConvention(UnderscoredNamingConvention.Instance)
                                                        .Build();
            Properties = deserializer.Deserialize<Properties>(File.ReadAllText(propertiesFile));
        }

        /// <summary>
        /// Vrací instanci objektu webdriveru
        /// </summary>
        /// <returns><see cref="IWebDriver"/></returns>
        public IWebDriver GetWebDriver()
        {
            return WebDriver;
        }

        /// <summary>
        /// Medota nastavuje základní parametry logovaní, logovací složku a inicializuje <see cref="Logger"/>
        /// </summary>
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
                Logger.Info("Inicializuji logger");
            }
        }

        /// <summary>
        /// Medota inicializuje <see cref="IWebDriver"/> a nastavuje event handlery pro akce v <see cref="IWebDriver"/>
        /// </summary>
        private void InitWebDriver()
        {
            Logger.Info("Inicializuji WebDriver");
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

        /// <summary>
        /// Metoda určena k exekuci akcí po události <see cref="EventFiringWebDriver.ElementClicking"/>
        /// Element se pokusí zvýraznit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void EventFiringWebDriver_createScreenShot_click(object sender, WebElementEventArgs args)
        {
            ((IJavaScriptExecutor)args.Driver).ExecuteScript("arguments[0].style.outline='4px solid red'", args.Element);
            string screenShotFile = GetLogDir() + "/screenshot_" + (++PageOrder).ToString().PadLeft(2, '0') + ".png";
            Logger.Info("Ukládám screenshot do souboru '" + screenShotFile + "'");
            Utils.SaveScreenshotAsFile(GetWebDriver(), screenShotFile);
        }

        /// <summary>
        /// Metoda určena k exekuci akcí po události <see cref="EventFiringWebDriver.NavigatingForward"/>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void EventFiringWebDriver_createScreenShot_forward(object sender, WebDriverNavigationEventArgs args)
        {
            string screenShotFile = GetLogDir() + "/screenshot_" + (++PageOrder).ToString().PadLeft(2, '0') + ".png";
            Logger.Info("Ukládám screenshot do souboru '" + screenShotFile + "'");
            Utils.SaveScreenshotAsFile(GetWebDriver(), screenShotFile);
        }

        /// <summary>
        /// Metoda určena k exekuci akcí po události <see cref="EventFiringWebDriver.NavigatingBack"/>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void EventFiringWebDriver_createScreenShot_back(object sender, WebDriverNavigationEventArgs args)
        {
            string screenShotFile = GetLogDir() + "/screenshot_" + (++PageOrder).ToString().PadLeft(2, '0') + ".png";
            Logger.Info("Ukládám screenshot do souboru '" + screenShotFile + "'");
            Utils.SaveScreenshotAsFile(GetWebDriver(), screenShotFile);
        }

        /// <summary>
        /// Ruší instanci driveru
        /// </summary>
        public void Quit()
        {
            Logger.Info("Ukončuji WebDriver");
            WebDriver.Quit();
        }

        /// <summary>
        /// Medota inicializuje <see cref="IWebDriver"/>
        /// </summary>
        /// <returns><see cref="IWebDriver"/></returns>
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
                Logger.Warn("Nepodařilo se naparsovat hodnotu, budu nastavovat 'isRemote=false'");
            }

            if (isRemote)
            {
                Logger.Info("Pouštím remote driver na url '" + uri + "'");
                if (browser.Equals("chrome"))
                    return new RemoteWebDriver(uri, new ChromeOptions());
                else if (browser.Equals("firefox"))
                    return new RemoteWebDriver(uri, new FirefoxOptions());
                else
                    return new RemoteWebDriver(uri, new ChromeOptions());
            }
            else
            {
                Logger.Info("Pouštím local driver");
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