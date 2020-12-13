using System.Collections.Generic;

namespace AlzaKariera.Classes
{
    /// <summary>Třída definuje strukturu yaml souborů s nastavením pro test</summary>
    public class Properties
    {
        /// <summary><see cref="Drivers"/></summary>
        public Drivers Drivers = new Drivers();
        
        /// <summary><see cref="Grid"/></summary>
        public Grid Grid = new Grid();

        /// <summary><see cref="Logs"/></summary>
        public Logs Logs = new Logs();

        /// <summary>Údaje o aplikacích (apps)</summary>
        public Dictionary<string, App> Apps = new Dictionary<string, App>();
    }

    /// <summary>Údaje o driverech (drivers)</summary>
    public class Drivers
    {
        /// <summary><see cref="ChromeDriver"/></summary>
        public ChromeDriver Chrome = new ChromeDriver();

        /// <summary><see cref="FirefoxDriver"/></summary>
        public FirefoxDriver Firefox = new FirefoxDriver();
    }

    /// <summary>Údaje k driveru</summary>
    public class ChromeDriver
    {
        /// <summary>Cesta k driveru (driver)</summary>
        public string Driver { get; set; }

        /// <summary>Cesta k prohlížeči (bin)</summary>
        public string Bin { get; set; }
    }

    /// <summary>Údaje k driveru</summary>
    public class FirefoxDriver
    {
        /// <summary>Cesta k driveru (driver)</summary>
        public string Driver { get; set; }

        /// <summary>Cesta k prohlížeči (bin)</summary>
        public string Bin { get; set; }
    }

    /// <summary>Údaje o selenium gridu (grid)</summary>
    public class Grid
    {
        /// <summary>Url selenium hubu</summary>
        public string Url { get; set; }
    }

    /// <summary>Údaje k logování (logs)</summary>
    public class Logs
    {
        /// <summary>Umístění config souboru pro logování</summary>
        public string Config { get; set; }

        /// <summary>Root složka, kam se budou ukládat logy</summary>
        public string Folder { get; set; }
    }

    /// <summary>Údaje o testované aplikaci</summary>
    public class App
    {
        /// <summary>Url testované aplikace</summary>
        public string Url { get; set; }
    }

}