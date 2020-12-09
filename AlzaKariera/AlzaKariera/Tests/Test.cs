using NLog;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlzaKariera.Tests
{
    class Test
    {
        public static Logger logger;

        public Test()
        {
            if (logger == null)
            {
                logger = LogManager.GetCurrentClassLogger();
            }

        }

    }

}
