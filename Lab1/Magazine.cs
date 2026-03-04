using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Lab1
{
    public class Magazine : Edition, IRateAndCopy
    {
        private Frequency frequency;
        private ArrayList editors;
        private ArrayList articles;

        public Magazine(string title, DateTime releaseDate, int circulation, Frequency frequency)
            : base(title, releaseDate, circulation)
        {
            this.frequency = frequency;
            editors = new ArrayList();
            articles = new ArrayList();
        }

        public Magazine() : this("NoTitle", DateTime.Today, 1, Frequency.Monthly) { }

        public Frequency Frequency
        {
            get => frequency;
            set => frequency = value;
        }

        public ArrayList Articles
        {
            get => articles;
            set => articles = value;
        }

        public ArrayList Editors
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
            articles.AddRange(newArticles);
        }

        public void AddEditors(params Person[] newEditors)
        {
            editors.AddRange(newEditors);
        }

        public override object DeepCopy()
        {
            Magazine copy = new Magazine(Title, ReleaseDate, Circulation, frequency);

            foreach (Person e in editors)
                copy.editors.Add(e.DeepCopy());

            foreach (Article a in articles)
                copy.articles.Add(a.DeepCopy());

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
            foreach (Article a in articles)
            {
                if (a.Rating > minRating)
                    yield return a;
            }
        }

        public IEnumerable ArticlesWithTitleContaining(string substring)
        {
            foreach (Article a in articles)
            {
                if (a.Title.Contains(substring, StringComparison.OrdinalIgnoreCase))
                    yield return a;
            }
        }
    }
}
