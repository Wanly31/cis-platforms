namespace Lab1
{
    public class Person
    {
        public Person(string firstName, string lastName, DateTime birthDate)
        {
            FirstName = firstName;
            LastName = lastName;
            BirthDate = birthDate;
        }

        public Person() : this("NoFirstName", "NoLastName", DateTime.Today) { }

        public string FirstName { get; init; }

        public string LastName { get; init; }

        public DateTime BirthDate { get; private set; }

        public int Year
        {
            get => BirthDate.Year;
            set => BirthDate = new DateTime(value, BirthDate.Month, BirthDate.Day);
        }


        
        public override string ToString()
        {
            return $"{FirstName} {LastName}, born on {BirthDate:yyyy-MM-dd}";
        }

        public virtual string ToShortString() => $"{FirstName} {LastName}";

    }
        
}