using Lararium.Video.Models.Options;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;

namespace Lararium.Video.ActionFilters
{
    internal class VideoUploadFilter : IAsyncActionFilter
    {
        private readonly VideoServiceOptions _options;

        public VideoUploadFilter(IOptions<VideoServiceOptions> options)
        {
            _options = options.Value;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var maxBytes = _options.MaxFileSizeMb * 1024L * 1024L;

            var feature = context.HttpContext.Features.Get<IHttpMaxRequestBodySizeFeature>();

            if (feature != null && !feature.IsReadOnly) 
            {
               feature.MaxRequestBodySize = maxBytes;
            }

            await next();
        }
    }

}
