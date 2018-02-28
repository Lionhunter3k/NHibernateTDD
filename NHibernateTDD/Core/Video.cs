using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NHibernateTDD.Core
{
    public enum VideoFormat
    {
        DVD
    }

    public class Video : Entity
    {
     
        public Video(Movie caddyShack, VideoFormat videoFormat)
        {
            // TODO: Complete member initialization
            this.Movie = caddyShack;
            this.Format = videoFormat;
        }

        protected Video()
        {
        }

        public virtual Movie Movie { get; set; }

        public virtual VideoFormat Format { get; set; }
    }
}
