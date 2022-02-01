using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace PokemonInfoService.Services.Tests.Helpers
{
    public class FakeHttpMessageHandler : HttpMessageHandler
    {
        public HttpResponseMessage HttpResponseMessage { get; set; }

        public HttpRequestMessage HttpRequestMessage { get; private set; }

        public virtual HttpResponseMessage Send(HttpRequestMessage request)
        {
            return HttpResponseMessage;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            HttpRequestMessage = request;
            return Task.FromResult(Send(request));
        }
    }
}
