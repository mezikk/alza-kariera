using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlzaKariera.Classes
{
    class Utils
    {
        /// <summary>
        /// Vytváří a ukládá screenshot do logovacího adresáře
        /// </summary>
        /// <param name="driver"><see cref="Driver"/></param>
        /// <param name="fileName">Úplný název souboru, který se bude ukládat</param>
        public static void SaveScreenshotAsFile(IWebDriver driver, string fileName)
        {
            ((ITakesScreenshot)driver).GetScreenshot().SaveAsFile(fileName, ScreenshotImageFormat.Png);
        }
    }
}
