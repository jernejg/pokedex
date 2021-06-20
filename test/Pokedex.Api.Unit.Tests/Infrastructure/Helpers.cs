using Moq;
using Moq.Protected;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Pokedex.Unit.Tests
{
	public class Helpers
	{
		public static HttpClient GetHttpClient(StringContent content, HttpStatusCode statusCode = HttpStatusCode.OK)
		{
			var handler = new Mock<HttpMessageHandler>();
			handler.Protected()
				.Setup<Task<HttpResponseMessage>>(
					"SendAsync",
					ItExpr.IsAny<HttpRequestMessage>(),
					ItExpr.IsAny<CancellationToken>()
				)
				.ReturnsAsync(new HttpResponseMessage
				{
					StatusCode = statusCode,
					Content = content
				});
			var client = new HttpClient(handler.Object)
			{
				BaseAddress = new Uri("http://foo")
			};
			return client;
		}
	}
}
