using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlzaKariera.Classes
{
    class Utils
    {
        public static void SaveScreenshotAsFile(IWebDriver driver, string fileName)
        {
            ((ITakesScreenshot)driver).GetScreenshot().SaveAsFile(fileName, ScreenshotImageFormat.Png);
        }
    }
}
