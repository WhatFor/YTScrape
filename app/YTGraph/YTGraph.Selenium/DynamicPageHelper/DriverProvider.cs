using System;
using Microsoft.Extensions.Options;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using YTGraph.Selenium.DynamicPageHelper.Configuration;
using YTGraph.Selenium.DynamicPageHelper.Contracts;

namespace YTGraph.Selenium.DynamicPageHelper
{
    public class SeleniumDriverProvider : IDriverProvider
    {
        private readonly SeleniumConfiguration _config;

        public SeleniumDriverProvider(IOptionsMonitor<SeleniumConfiguration> config)
        {
            _config = config.CurrentValue;
        }

        public IWebDriver GetDriver(Action<ChromeOptions> configure = null)
        {
            
            var config = new ChromeOptions
            {
                LeaveBrowserRunning = true,
                BinaryLocation = _config.BinaryLocation,
            };

            configure?.Invoke(config);
            
            return new ChromeDriver(_config.DriverDirectory, config);
        }

        public WebDriverWait GetWait(IWebDriver driver, TimeSpan? wait = null) =>
            new (driver, wait ?? TimeSpan.FromMilliseconds(_config.DefaultPageWait));
    }
}