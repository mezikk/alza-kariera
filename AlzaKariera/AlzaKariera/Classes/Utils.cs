using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlzaKariera.Classes
{
    class Utils
    {
        public static void SaveScreenshotAsFile(IWebDriver driver, string path)
        {
            ((ITakesScreenshot)driver).GetScreenshot().SaveAsFile("screen.png", ScreenshotImageFormat.Png);
        }


    }
}
