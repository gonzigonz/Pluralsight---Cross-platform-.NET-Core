using System.Linq;
using XPlat;
using Xunit;

namespace XPlatTests
{
    public class UnitTest
    {
        [Fact]
        public void WithoutHttpAtStartOfLink_NoLinks()
        {
            var links = LinkChecker.GetLinks("<a href=\"google.com\" />");
            Assert.Equal(links.Count(), 0);
        }

        [Fact]
        public void WithHttpAtStartOfLink_LinkParses()
        {
            var links = LinkChecker.GetLinks("<a href=\"http://google.com\" />");
            Assert.Equal(links.Count(), 1);
            Assert.Equal(links.First(), "http://google.com");
        }
    }
}
