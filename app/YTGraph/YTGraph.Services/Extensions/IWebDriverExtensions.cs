using System;
using System.Threading.Tasks;
using OpenQA.Selenium;
using Serilog;
using YTGraph.Services.Constants;

namespace YTGraph.Services.Extensions
{
    public static class IWebDriverExtensions
    {
        public static async Task<IWebDriver> HandleYouTubeInit(this IWebDriver driver)
        {
            // Wait for page load
            await Task.Delay(3_000);

            // If we're sent to the YT Consent page, continue on
            if (driver.Title == Scrape.YouTubeConsent)
            {
                Log.Information("YouTube Scrape >> Found Consent page. Attempting tp bypass...");
                driver.FindElement(By.TagName(Tags.Form)).Submit();
                await Task.Delay(1000);
            }

            try
            {
                // If the upsell popup appears, dismiss it
                var upsell = driver.FindElement(By.Id(Scrape.YouTubeUpsellId));
                Log.Information("YouTube Scrape >> Found Consent upsell. Attempting tp bypass...");
                upsell.FindYouTubeButton(Scrape.YouTubeButtonDeclineText).Click();
            }
            catch (NoSuchElementException _) { }
            catch (Exception e)
            {
                Log.Error(e, "Something went wrong.");
            }
            
            try
            {
                // If we're given the consent bump, bypass
                _ = driver.FindElement(By.ClassName(Scrape.YouTubeConsentBumpClass));
                Log.Information("YouTube Scrape >> Found Consent bump. Attempting tp bypass...");
                
                // Switch to the consent iframe
                driver.SwitchTo().Frame("iframe");
                driver.FindElement(By.Id(Scrape.YouTubeConsentFormId)).Click();
                
                // Switch back to initial page
                driver.SwitchTo().ParentFrame();
            }
            catch (NoSuchElementException _) { }
            catch (Exception e)
            {
                Log.Error(e, "Something went wrong.");
            }

            return driver;
        }
    }
}