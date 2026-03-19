namespace Lab1
{

    public class Edition : IComparable, IComparer<Edition>
    {
        public Edition(string title, DateTime releaseDate, int circulation)
        {
            Title = title;
            ReleaseDate = releaseDate;
            Circulation = circulation;
        }

        public Edition() : this("NoTitle", DateTime.Today, 1) { }

        protected string title;
        protected DateTime releaseDate;
        protected int circulation;

        public string Title 
        { 
            get => title; 
            init => title = value; 
        }
        public DateTime ReleaseDate 
        { 
            get => releaseDate; 
            init => releaseDate = value; 
        }
        public int Circulation
        {
            get => circulation;
            init
            {
                if (value <= 0)
                {
                    throw new ArgumentException("Circulation must be greater than 0.");
                }
                circulation = value;
            }
        }

        public virtual object DeepCopy()
        {
            return new Edition(Title, ReleaseDate, Circulation);
        }
        
        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;
            
            Edition other = (Edition)obj;
            return Title == other.Title &&
                ReleaseDate == other.ReleaseDate &&
                 Circulation == other.Circulation;
        }

        public static bool operator ==(Edition e1, Edition e2)
        {
            if (ReferenceEquals(e1, e2))
            {
                return true;
            }
            if (e1 is null || e2 is null)
            {
                return false;
            }
            return e1.Equals(e2);
        }

        public static bool operator !=(Edition e1, Edition e2) => !(e1 == e2);

        public override int GetHashCode()
        {
            return HashCode.Combine(Title, ReleaseDate, Circulation);
        }

        public override string ToString()
        {
            return $"{Title}, released on {ReleaseDate:yyyy-MM-dd}, circulation: {Circulation}";
        }

        public int CompareTo(object? obj)
        {
            if (obj == null)
            {
                return 1;
            }
            if (obj is Edition other)
            {
                return this.Title.CompareTo(other.Title);
            }

            throw new ArgumentException("Object is not an Edition");
        }

        public int Compare(Edition? x, Edition? y)
        {
            if (x == null && y == null) 
            { 
                return 0; 
            }
            if (x == null)
            { 
                return -1; 
            }
            if (y == null) 
            { 
                return 1; 
            }

            return DateTime.Compare(x.ReleaseDate, y.ReleaseDate);
        }
    }


}
