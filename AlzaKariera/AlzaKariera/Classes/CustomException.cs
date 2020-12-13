using System;
using System.IO;

namespace AlzaKariera.Classes
{
    /// <summary>Třída slouží k zalogování chyby/vytvoření screenshotu</summary>
    class CustomException : Exception
    {
        public CustomException(Driver driver, string message) : base(message)
        {
            if (driver.GetWebDriver() == null)
            {
                driver.GetLogger().Error(message);
            }
            else
            {
                string screenShotFile = driver.GetLogDir() + "/screenshot_error.png";
                string sourceCodeFile = driver.GetLogDir() + "/page_source.html";

                //screenshot pouze viditelne casti okna (pripadne nastaveni vysky/sirky okna na vyssi hodnoty)
                Utils.SaveScreenshotAsFile(driver.GetWebDriver(), screenShotFile);
                driver.GetLogger().Debug("Screenshot uložen do souboru '{0}'", screenShotFile);
                File.WriteAllText(sourceCodeFile, driver.GetWebDriver().PageSource);
                driver.GetLogger().Debug("Zdrojový kód stránky uložen do souboru '{0}'", sourceCodeFile);
            }
        }
    }
}
