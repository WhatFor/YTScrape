using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace YTGraph.Selenium.DynamicPageHelper.Contracts
{
    public interface IDriverProvider
    {
        IWebDriver GetDriver(Action<ChromeOptions> configure = null);

        WebDriverWait GetWait(IWebDriver driver, TimeSpan? wait = null);
    }
}