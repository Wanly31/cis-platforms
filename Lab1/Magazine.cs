using System.Collections;
using System.Text;

namespace Lab1
{
    public class Magazine : Edition, IRateAndCopy
    {
        private Frequency frequency;
        private List<Person> editors;
        private List<Article> articles;

        public Magazine(string title, DateTime releaseDate, int circulation, Frequency frequency)
            : base(title, releaseDate, circulation)
        {
            this.frequency = frequency;
            editors = new List<Person>();
            articles = new List<Article>();
        }

        public Magazine() : this("NoTitle", DateTime.Today, 1, Frequency.Monthly) { }

        public Frequency Frequency
        {
            get => frequency;
            set => frequency = value;
        }

        public List<Article> Articles
        {
            get => articles;
            set => articles = value;
        }

        public List<Person> Editors
        {
            get => editors;
            set => editors = value;
        }

        public double Rating => AverageRating;

        public double AverageRating
        {
            get
            {
                if (articles.Count == 0) return 0;

                double sum = 0;
                foreach (Article a in articles)
                    sum += a.Rating;

                return sum / articles.Count;
            }
        }

        public Edition EditionData
        {
            get => new Edition(Title, ReleaseDate, Circulation);
            init
            {
                title = value.Title;
                releaseDate = value.ReleaseDate;
                circulation = value.Circulation;
            }
        }
        
        public void AddArticles(params Article[] newArticles)
        {
            if (newArticles != null)
            {
                articles.AddRange(newArticles);
            }
        }

        public void AddEditors(params Person[] newEditors)
        {
            if (newEditors != null)
            {
                editors.AddRange(newEditors);
            }
        }

        public override object DeepCopy()
        {
            Magazine copy = new Magazine(Title, ReleaseDate, Circulation, frequency);

            if (editors != null)
            {
                foreach (Person e in editors)
                    copy.editors.Add((Person)e.DeepCopy());
            }

            if (articles != null)
            {
                foreach (Article a in articles)
                    copy.articles.Add((Article)a.DeepCopy());
            }
            return copy;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Magazine: {Title}, Frequency: {Frequency}, Released: {ReleaseDate:yyyy-MM-dd}, Circulation: {Circulation}");

            sb.Append("Editors: ");
            if (editors.Count > 0)
                sb.AppendLine(string.Join("; ", editors.ToArray()));
            else
                sb.AppendLine("No editors");

            sb.Append("Articles: ");
            if (articles.Count > 0)
                sb.AppendLine(string.Join("; ", articles.ToArray()));
            else
                sb.AppendLine("No articles");

            return sb.ToString();
        }

        public virtual string ToShortString()
        {
            return $"Magazine: {Title}, Frequency: {Frequency}, Released: {ReleaseDate:yyyy-MM-dd}, Circulation: {Circulation}, Average Rating: {AverageRating:F2}";
        }

        public IEnumerable ArticlesWithRatingAbove(double minRating)
        {
            //null
            if (articles != null)
            {
                foreach (Article a in articles)
                {
                    if (a.Rating > minRating)
                        yield return a;
                }
            }
        }

        public IEnumerable ArticlesWithTitleContaining(string substring)
        {
            if (articles != null)
            {
                foreach (Article a in articles)
                {
                    if (a.Title.Contains(substring, StringComparison.OrdinalIgnoreCase))
                        yield return a;
                }
            }
        }
    }
}
