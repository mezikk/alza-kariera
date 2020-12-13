namespace AlzaKariera
{
    /// <summary>Předpis pro vytvoření osoby, která se účastní některé fáze pohovoru</summary>
    public class Person
    {
        /// <summary>Jméno a příjmení účastníka řízení</summary>
        public string Name { get; set; }

        /// <summary>Krátký popis účastníka řízení</summary>
        public string Description { get; set; }

        /// <summary>Link na obrázek účastníka řízení</summary>
        public string Picture { get; set; }

        /// <summary><see cref="Person"/></summary>
        /// <param name="name"><see cref="Name"/></param>
        /// <param name="decription"><see cref="Description"/></param>
        /// <param name="picture"><see cref="Picture"/></param>
        public Person(string name, string decription, string picture)
        {
            Name = name;
            Description = decription;
            Picture = picture;
        }
    }
}