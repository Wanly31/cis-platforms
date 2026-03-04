using System;
using System.IO.Pipelines;
using System.Linq;

namespace Lab1
{
    public class MagazineOld
    {
        public string Title { get; set; }
        public Frequency Frequency { get; set; }
        public DateTime PublicationDate { get; set; }
        public int Circulation { get; set; }

        private Article[] Articles;

        public MagazineOld(string title, Frequency frequency, DateTime publicationDate, int circulation, Article[] articles)
        {
            Title = title;
            Frequency = frequency;
            PublicationDate = publicationDate;
            Circulation = circulation;
            Articles = articles ?? new Article[0];
        }

        public MagazineOld() : this("NoTitle", Frequency.Monthly, DateTime.Today, 0, new Article[0]) { }

        public double AverageRating => Articles.Length == 0 ? 0.0 : Articles.Average(a => a.Rating);

        public bool this[Frequency freq] => Frequency == freq;

        public void AddArticles(params Article[] newArticles)
        {
            int oldLength = Articles.Length;
            int newLength = oldLength + newArticles.Length;

            Article[] temp = new Article[newLength];

            for (int i = 0; i < oldLength; i++)
                temp[i] = Articles[i];

            for (int i = 0; i < newArticles.Length; i++)
                temp[oldLength + i] = newArticles[i];

            Articles = temp;
        }

        public override string ToString()
        {
            string articlesStr = Articles.Length > 0 ? string.Join("; ", Articles) : "No articles";
            return $"Title: {Title}, Frequency: {Frequency}, PublicationDate: {PublicationDate.ToShortDateString()}, Circulation: {Circulation}, Articles: {articlesStr}";
        }

        public virtual string ToShortString()
        {
            return $"Title: {Title}, Frequency: {Frequency}, PublicationDate: {PublicationDate.ToShortDateString()}, Circulation: {Circulation}, Average Rating: {AverageRating:F2}";
        }
    }
}