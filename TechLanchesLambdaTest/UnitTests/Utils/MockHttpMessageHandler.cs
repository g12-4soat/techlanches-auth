using System.Net;
using System.Text;

namespace TechLanchesLambdaTest.UnitTests.Utils
{
    public class MockHttpMessageHandler : HttpMessageHandler
    {
        private readonly string _response;
        private readonly HttpStatusCode _statusCode;
        private readonly Exception _exceptionToThrow;

        public string Input { get; private set; }
        public int NumberOfCalls { get; private set; }

        public MockHttpMessageHandler(string response, HttpStatusCode statusCode)
        {
            _response = response;
            _statusCode = statusCode;
        }

        public MockHttpMessageHandler(Exception exceptionToThrow)
        {
            _exceptionToThrow = exceptionToThrow ?? throw new ArgumentNullException(nameof(exceptionToThrow));
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            NumberOfCalls++;

            if (_exceptionToThrow != null)
            {
                throw _exceptionToThrow;
            }

            if (request.Content != null) // Could be a GET-request without a body
            {
                Input = await request.Content.ReadAsStringAsync();
            }
            return new HttpResponseMessage
            {
                StatusCode = _statusCode,
                Content = new StringContent(_response, Encoding.UTF8, "application/json")
            };
        }
    }
}
