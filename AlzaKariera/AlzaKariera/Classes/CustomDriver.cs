using AlzaKariera.Classes;
using NLog;
using OpenQA.Selenium;
using System;
using System.IO;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace AlzaKariera.Classes
{
    public class CustomDriver
    {
        IWebDriver WebDriver;
        Logger Logger;
        Properties Properties;

        public CustomDriver()
        {
            LoadConfig(@"c:\Users\mezikk\source\repos\alza-kariera\AlzaKariera\AlzaKariera\Config\properties.yaml");
            InitLogging();
            InitWebDriver();
        }

        public Logger GetLogger()
        {
            return Logger;
        }

        private void LoadConfig(string configFile)
        {
            var deserializer = new DeserializerBuilder().WithNamingConvention(UnderscoredNamingConvention.Instance)
                                                        .Build();
            Properties = deserializer.Deserialize<Properties>(File.ReadAllText(configFile));
        }

        public IWebDriver GetDriver()
        {
            return WebDriver;
        }
        private void InitLogging()
        {
            if (Logger == null)
            {
                LogManager.Configuration = new NLog.Config.XmlLoggingConfiguration(Properties.Logs.Config);
                LogManager.Configuration.Variables["basedir"] = Properties.Logs.Folder;
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