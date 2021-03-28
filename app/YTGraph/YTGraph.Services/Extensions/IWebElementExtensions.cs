using System.Linq;
using OpenQA.Selenium;
using YTGraph.Services.Constants;

namespace YTGraph.Services.Extensions
{
    public static class IWebElementExtensions
    {
        public static IWebElement FindYouTubeButton(this IWebElement ele, string text) =>
            ele
                .FindElements(By.ClassName(Scrape.YouTubeButtonClass))
                .SingleOrDefault(x => x.Text == text && x.TagName == Tags.Anchor);
    }
}