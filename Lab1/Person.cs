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

        public static bool operator == (Person p1, Person p2)
        {
            if(ReferenceEquals(p1, p2))
            {
                return true;
            }

            if(p1 is null || p2 is null)
            {
                return false;
            }

            return p1.Equals(p2);
        }
        
        public static bool operator !=(Person p1, Person p2) => !(p1 == p2);

        public override int GetHashCode()
        {
            return HashCode.Combine(FirstName, LastName, BirthDate);
        }

        public virtual object DeepCopy()
        {
            //return new Person(FirstName, LastName, BirthDate);
            return MemberwiseClone();
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;
            
            Person other = (Person)obj;

            return FirstName == other.FirstName &&
                LastName == other.LastName &&
                 BirthDate == other.BirthDate;
        }
        
        public override string ToString()
        {
            return $"{FirstName} {LastName}, born on {BirthDate:yyyy-MM-dd}";
        }

        public virtual string ToShortString() => $"{FirstName} {LastName}";

    }
        
}