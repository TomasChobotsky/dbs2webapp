using Microsoft.JSInterop;
using System.Net.Http.Headers;

namespace dbs2webapp.Client.Services
{
    public class JwtAuthorizationMessageHandler : DelegatingHandler
    {
        private readonly IJSRuntime _js;

        public JwtAuthorizationMessageHandler(IJSRuntime js) => _js = js;

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // Look first in localStorage, then sessionStorage
            var token = await _js.InvokeAsync<string>("localStorage.getItem", "authToken")
                        ?? await _js.InvokeAsync<string>("sessionStorage.getItem", "authToken");

            if (!string.IsNullOrWhiteSpace(token))
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
