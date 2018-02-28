using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernateTDD;
using NHibernateTDD.Core;

namespace NHibernateTDD.Tests
{
    [TestFixture]
    public class MovieTests : BaseTest
    {
        [Test]
        public void CanCreateMovie()
        {

            Movie superTroopers = new Movie("Super Troopers", MovieCategory.Comedy, 2001);

            Assert.AreEqual("Super Troopers", superTroopers.Name, "Name wasn’t set");

            Assert.AreEqual(MovieCategory.Comedy, superTroopers.Category, "Category wasn’t set");

            Assert.AreEqual(2001, superTroopers.Year, "Year wasn’t set");

        }

        [Test]

        public void CanCreateVideo()
        {
            Movie caddyShack = new Movie("Caddy Shack", MovieCategory.Comedy, 1980);

            Video v = new Video(caddyShack, VideoFormat.DVD);

            Assert.AreEqual(v.Movie, caddyShack, "Movie wasn’t set.");

            Assert.AreEqual(VideoFormat.DVD, v.Format, "Format wasn’t set.");

        }
    }
}
