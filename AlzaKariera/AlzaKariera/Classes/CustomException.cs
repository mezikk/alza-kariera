using System;
using System.IO;

namespace AlzaKariera.Classes
{
    /// <summary>
    /// Třída slouží k zalogování chyby/vytvoření screenshotu
    /// </summary>
    class CustomException : Exception
    {
        public CustomException(Driver driver, string message) : base(message)
        {
            string screenShotFile = driver.GetLogDir() + "/screenshot_error.png";
            string sourceCodeFile = driver.GetLogDir() + "/page_source.html";

            //screenshot pouze viditelne casti okna (pripadne nastaveni vysky/sirky na vyssi hodnoty)
            Utils.SaveScreenshotAsFile(driver.GetWebDriver(), screenShotFile);
            driver.GetLogger().Info("Screenshot uložen do souboru '" + screenShotFile + "'");
            File.WriteAllText(sourceCodeFile, driver.GetWebDriver().PageSource);
            driver.GetLogger().Info("Zdrojový kód stránky uložen do souboru '" + sourceCodeFile + "'");
        }
    }
}
