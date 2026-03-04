using System;
using System.Collections.Generic;
using System.Text;

namespace Lab1
{
    public class Article : IRateAndCopy
    {
        public Person Author { get; set; }
        public string Title { get; set; }
        public double Rating { get; set; }

        public Article(Person author, string title, double rating)
        {
            Author = author;
            Title = title;
            Rating = rating;
        }

        public Article() : this(new Person(), "NoTitle", 0.0) { }

        public override string ToString()
        {
            return $"Article: {Title}, Author: {Author}, Rating: {Rating}";
        }

        public virtual object DeepCopy()
        {
            return new Article(
                (Person)Author.DeepCopy(),
                Title,
                Rating
            );
        }
    }
}
