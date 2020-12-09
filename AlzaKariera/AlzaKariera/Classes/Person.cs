namespace AlzaKariera
{
    public class Person
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Picture { get; set; }

        public Person(string name, string decription, string picture)
        {
            Name = name;
            Description = decription;
            Picture = picture;
        }
    }
}