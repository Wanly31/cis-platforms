
using System.Text;

namespace Lab1
{
    public class MagazineCollection
    {
        private readonly List<Magazine> magazineList;

        public MagazineCollection()
        {
            magazineList = new List<Magazine>();
        }

        public void AddMagazines(params Magazine[] newMagazines)
        {
            if(newMagazines != null)
            {
                magazineList.AddRange(newMagazines);
            }
        }

        public void AddDefaults()
        {
            Magazine m1 = new Magazine("Science Today", DateTime.Now, 1000, Frequency.Monthly);
            m1.AddArticles(new Article(new Person("John", "Doe", new DateTime(2000, 7, 6)), "Physics", 4.5));
            m1.AddEditors(new Person("Anna", "Smith", new DateTime(2005, 4, 3)));

            Magazine m2 = new Magazine("Tech Weekly", DateTime.Now.AddMonths(-1), 500, Frequency.Weekly);
            m2.AddArticles(new Article(new Person("Bob", "Jones", new DateTime(1999, 5, 17)), "AI trends", 3.8));
            m2.AddEditors(new Person("Mike", "Brown", new DateTime(1995, 3, 22)));

            AddMagazines(m1, m2);
        }

        public void SortByTitle()
        {
            magazineList.Sort();
        }

        public void SortByDate()
        {
            magazineList.Sort(new Edition());
        }

        public void SortByCirculation()
        {
            magazineList.Sort(new EditionCirculationComparer());
        }

        public double MaxAverageRating
        {
            get
            {
                if (magazineList.Count == 0)
                {
                    return 0;
                }

                return magazineList.Max(m => m.AverageRating);
            }

        }

        public IEnumerable<Magazine> MonthlyMagazines
        {
            get
            {
                return magazineList.Where(m => m.Frequency == Frequency.Monthly);
            }
        }

        public List<Magazine> RatingGroup(double value)
        {
            return magazineList
            .GroupBy(m => m.AverageRating >= value)
            .Where(g => g.Key == true)
            .SelectMany(g => g)
            .ToList();
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach(Magazine mag in magazineList)
            {
                sb.AppendLine($"Title: {mag.Title}, Frequency: {mag.Frequency}" +
                    $"Released: {mag.ReleaseDate:yyy-MM-dd}, Circulation: {mag.Circulation}" +
                    $"Avg Rating: {mag.AverageRating}, Editors count: {mag.Editors.Count}" +
                    $"Articles count: {mag.Articles.Count}");
            }

            return sb.ToString();
        }

        public virtual string ToShortString()
        {
            StringBuilder sb = new StringBuilder();

            foreach(Magazine mag in magazineList)
            {
                sb.AppendLine($"Avg Rating: {mag.AverageRating}, Articles count: {mag.Articles.Count}");
            }

            return sb.ToString();
        }
    }
}
