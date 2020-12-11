using System;
using System.Collections.Generic;
using System.Text;

namespace AlzaKariera.Classes
{
    class CustomException : Exception
    {
        public CustomException(string message) : base(message)
        {
            //Utils.SaveScreenshotAsFile(Driver, this.GetType().Name + ".png");
        }
    }
}
