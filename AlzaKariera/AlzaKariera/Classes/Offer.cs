using System;
using System.Collections.Generic;
using System.Text;

namespace AlzaKariera.Classes
{
    public class Offer
    {
        public string Pathname { get; set; }
        public string JobTitle { get; set; }

        public Offer(string pathname, string jobTitle)
        {
            Pathname = pathname;
            JobTitle = jobTitle;
        }
    }
}
