using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NHibernateTDD.Core
{
    public class Rental : Entity
    {
        public virtual Video Video { get; set; }
        public virtual decimal Price { get; set; }

        public Rental(Video _video, decimal p)
        {
            // TODO: Complete member initialization
            this.Video = _video;
            this.Price = p;
        }

        protected Rental()
        {
        }
    }
}
