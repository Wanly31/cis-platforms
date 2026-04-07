using System.Collections;
using System.Text;
using System.Text.Json;

namespace Lab1
{
    [Serializable]
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

        //JsonSerializer
        public Magazine DeepCopy()
        {
            using (var ms = new MemoryStream())
            {
                JsonSerializer.Serialize(ms, this);
                ms.Position = 0;
                return JsonSerializer.Deserialize<Magazine>(ms)
                    ?? throw new Exception("Deserialization failed");
            }
        } 

        public bool Save(string filename)
        {
            try
            {
                using (FileStream fstream = new FileStream(filename, FileMode.Create))
                {
                    JsonSerializer.Serialize(fstream, this);

                    return true;
                }
            }
            catch
            {
                return false;
            }

        }

        public bool Load(string filename)
        {
            try
            {
                using (FileStream fstream = new FileStream(filename, FileMode.Open))
                {
                    var deserialize = JsonSerializer.Deserialize<Magazine>(fstream);

                    if (deserialize != null)
                    {
                        this.articles = deserialize.articles;
                        this.circulation = deserialize.circulation;
                        this.frequency = deserialize.frequency;
                        this.editors = deserialize.editors;
                        this.releaseDate = deserialize.releaseDate;
                        this.title = deserialize.title;
                        return true;
                    }

                    return false;
                }

            }
            catch
            {
                return false;
            }
        }


        public static bool Save(string filename, Magazine obj)
        {
            try
            {
                using (FileStream fstream = new FileStream(filename, FileMode.Create))
                {
                    JsonSerializer.Serialize(fstream, obj);

                    return true;
                }

            }
            catch
            {
                return false;
            }
            
        }

        public static bool Load(string filename, Magazine obj)
        {
            try
            {
                using(FileStream fstream = new FileStream(filename, FileMode.Open))
                {
                    var deserialize = JsonSerializer.Deserialize<Magazine>(fstream);
                    if(deserialize != null)
                    {
                        obj.articles = deserialize.articles;
                        obj.circulation = deserialize.circulation;
                        obj.frequency = deserialize.frequency;
                        obj.editors = deserialize.editors;
                        obj.releaseDate = deserialize.releaseDate;
                        obj.title = deserialize.title;
                        return true;
                    }
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        public bool AddFromConsole()
        {
            Console.WriteLine("Введіть (через |): Назва|Ім'я|Прізвище|Дата|Рейтинг");
            var str = Console.ReadLine();

            if(str != null)
            {
                var articleSplit = str.Split('|');

                if(articleSplit.Length != 5)
                {
                    return false;
                }

                if (DateTime.TryParse(articleSplit[3], out DateTime birthDate) == false)
                {
                    return false;
                }

                if (double.TryParse(articleSplit[4], out double rating) == false)
                {
                    return false;
                }

                Person person = new Person(articleSplit[1], articleSplit[2], birthDate);
                Article article = new Article(person, articleSplit[0], rating);

                AddArticles(article);
                
                return true;

            }


            
            return false;
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
