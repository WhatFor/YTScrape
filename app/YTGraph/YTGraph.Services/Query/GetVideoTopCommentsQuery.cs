using System;
using System.Collections.Generic;
using System.Linq;
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
    public record GetVideoTopCommentsQuery(string Url) : IRequest<IEnumerable<CommentResult>>;

    public struct CommentResult
    {
        public string Comment { get; set; }
        
        public string User { get; set; }
    }
    
    public class GetVideoTopCommentsQueryHandler : IRequestHandler<GetVideoTopCommentsQuery, IEnumerable<CommentResult>>
    {
        private readonly IDriverProvider _driverProvider;
        private readonly ILogger<GetVideoTitleQueryHandler> _logger;

        public GetVideoTopCommentsQueryHandler(
            IDriverProvider driverProvider,
            ILogger<GetVideoTitleQueryHandler> logger)
        {
            _driverProvider = driverProvider;
            _logger = logger;
        }

        public async Task<IEnumerable<CommentResult>> Handle(GetVideoTopCommentsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var driver = _driverProvider.GetDriver();
                _logger.LogInformation("YouTube Scrape >> Loading...");
                driver.Navigate().GoToUrl(request.Url);
                await driver.HandleYouTubeInit();

                var commentElements = driver
                    .FindElements(By.ClassName(Scrape.YouTubeCommentClass))
                    .Where(x => (x.TagName == "ytd-expander" || x.TagName == "h3") &&
                                !string.IsNullOrWhiteSpace(x.Text))
                    .ToList();

                var users = commentElements
                    .Select((element, i) => i % 2 == 0 ? element : null)
                    .Where(x => x != null);
                
                var comments = commentElements
                    .Select((element, i) => i % 2 == 0 ? null : element)
                    .Where(x => x != null);

                return users.Select((element, i) => new CommentResult
                {
                    User = element.Text,
                    Comment = comments.ElementAt(i).Text,
                });
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failed to scrape title. URL: '{0}'.", request.Url);
                return Enumerable.Empty<CommentResult>();
            }
        }
    }
}