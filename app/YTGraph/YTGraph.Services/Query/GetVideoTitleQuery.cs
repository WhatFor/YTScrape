using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using OpenQA.Selenium;
using YTGraph.Selenium.DynamicPageHelper.Contracts;
using YTGraph.Services.Constants;
using YTGraph.Services.Extensions;

namespace YTGraph.Services.Query
{
    public record GetVideoTitleQuery(string Url) : IRequest<string>;
    
    public class GetVideoTitleQueryHandler : IRequestHandler<GetVideoTitleQuery, string>
    {
        private readonly IDriverProvider _driverProvider;
        private readonly ILogger<GetVideoTitleQueryHandler> _logger;

        public GetVideoTitleQueryHandler(
            IDriverProvider driverProvider,
            ILogger<GetVideoTitleQueryHandler> logger)
        {
            _driverProvider = driverProvider;
            _logger = logger;
        }

        public async Task<string> Handle(GetVideoTitleQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var driver = _driverProvider.GetDriver();

                _logger.LogInformation("YouTube Scrape >> Loading...");
                driver.Navigate().GoToUrl(request.Url);
                
                await driver.HandleYouTubeInit();
                
                var title = driver.FindElement(By.ClassName("title"))?.Text;
                return title ?? Scrape.NOT_FOUND;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failed to scrape title. URL: '{0}'.", request.Url);
                return Scrape.ERROR;
            }
        }
    }
}