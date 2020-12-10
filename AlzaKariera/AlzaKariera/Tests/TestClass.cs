using NLog;

namespace AlzaKariera.Tests
{
    class TestClass
    {
        public static Logger logger;

        public TestClass()
        {
            if (logger == null)
            {
                LogManager.Configuration = new NLog.Config.XmlLoggingConfiguration(@"c:\Users\mezikk\source\repos\alza-kariera\AlzaKariera\AlzaKariera\Config\nlog.config");
                logger = LogManager.GetCurrentClassLogger();
            }
        }
    }
}
