using System;
using System.Net.Http;
using Splat;
namespace Heartstone
{
    public class HeartstoneHttpClientHandler : HttpClientHandler, IEnableLogger
    {
        public HeartstoneHttpClientHandler()
        {
        }

        protected override async System.Threading.Tasks.Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
        {

            this.Log().Debug("Request: " + request);

            var response = await base.SendAsync(request, cancellationToken);

            this.Log().Debug("Response: " + await response.Content.ReadAsStringAsync());
            this.Log().Debug("Status Code: " + response.StatusCode);

            return response;
        }

    }
}
