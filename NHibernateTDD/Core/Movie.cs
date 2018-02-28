using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NHibernateTDD.Core
{
    public enum MovieCategory
    {
        Comedy
    }

    public class Movie : Entity
    {

        public Movie(string p1, MovieCategory movieCategory, int p2)
        {
            // TODO: Complete member initialization
            this.Name = p1;
            this.Category = movieCategory;
            this.Year = p2;
        }

        protected Movie()
        {
        }

        public virtual string Name { get; set; }

        public virtual MovieCategory Category { get; set; }

        public virtual int Year { get; set; }
    }

}
