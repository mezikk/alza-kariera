using System.Collections.Generic;

namespace AlzaKariera.Classes
{
    public class Properties
    {
        public Drivers Drivers = new Drivers();
        public Grid Grid = new Grid();
        public Logs Logs = new Logs();
        public Dictionary<string, App> Apps = new Dictionary<string, App>();
    }

    public class Drivers
    {
        public ChromeDriver Chrome = new ChromeDriver();
        public FirefoxDriver Firefox = new FirefoxDriver();
    }

    public class ChromeDriver
    {
        public string Driver { get; set; }
        public string Bin { get; set; }
    }

    public class FirefoxDriver
    {
        public string Driver { get; set; }
        public string Bin { get; set; }
    }

    public class Grid
    {
        public string Url { get; set; }
    }

    public class Logs
    {
        public string Config { get; set; }
        public string Folder { get; set; }
    }

    public class App
    {
        public string Url { get; set; }
    }

}