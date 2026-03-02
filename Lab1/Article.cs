using System;
using System.Collections.Generic;
using System.Text;

namespace Lab1
{
    public class Article
    {
        public Person Person { get; set; }
        public string Title { get; set; }
        public double Rating { get; set; }


        public Article(Person person, string title, double rating)
        {
            Person = person;
            Title = title;
            Rating = rating;
        }
        public Article() : this(new Person(), "NoTitle", 0.0) { }

        public override string ToString()
        {
            return $"Article: {Title}, Author: {Person}, Rating: {Rating}";
        }
    }
}
