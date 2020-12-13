namespace AlzaKariera.Classes
{
    /// <summary>Předpis pro vytvoření nabídky zaměstnání</summary>
    public class Offer
    {
        /// <summary>Odkaz na nabídku</summary>
        public string Pathname { get; set; }

        /// <summary>Název pozice</summary>
        public string JobTitle { get; set; }

        /// <summary><see cref="Offer"/></summary>
        /// <param name="pathname"><see cref="Pathname"/></param>
        /// <param name="jobTitle"><see cref="JobTitle"/></param>
        public Offer(string pathname, string jobTitle)
        {
            Pathname = pathname;
            JobTitle = jobTitle;
        }
    }
}