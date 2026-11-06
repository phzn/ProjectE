using System;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net.Http
{
	/// <summary>A base type for handlers which only do some small processing of request and/or response messages.</summary>
	// Token: 0x0200002C RID: 44
	public abstract class MessageProcessingHandler : DelegatingHandler
	{
		/// <summary>Creates an instance of a <see cref="T:System.Net.Http.MessageProcessingHandler" /> class.</summary>
		// Token: 0x06000172 RID: 370 RVA: 0x00005E9A File Offset: 0x0000409A
		protected MessageProcessingHandler()
		{
		}

		/// <summary>Creates an instance of a <see cref="T:System.Net.Http.MessageProcessingHandler" /> class with a specific inner handler.</summary>
		/// <param name="innerHandler">The inner handler which is responsible for processing the HTTP response messages.</param>
		// Token: 0x06000173 RID: 371 RVA: 0x00005EA2 File Offset: 0x000040A2
		protected MessageProcessingHandler(HttpMessageHandler innerHandler)
			: base(innerHandler)
		{
		}

		/// <summary>Performs processing on each request sent to the server.</summary>
		/// <param name="request">The HTTP request message to process.</param>
		/// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
		/// <returns>The HTTP request message that was processed.</returns>
		// Token: 0x06000174 RID: 372
		protected abstract HttpRequestMessage ProcessRequest(HttpRequestMessage request, CancellationToken cancellationToken);

		/// <summary>Perform processing on each response from the server.</summary>
		/// <param name="response">The HTTP response message to process.</param>
		/// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
		/// <returns>The HTTP response message that was processed.</returns>
		// Token: 0x06000175 RID: 373
		protected abstract HttpResponseMessage ProcessResponse(HttpResponseMessage response, CancellationToken cancellationToken);

		/// <summary>Sends an HTTP request to the inner handler to send to the server as an asynchronous operation.</summary>
		/// <param name="request">The HTTP request message to send to the server.</param>
		/// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
		/// <returns>The task object representing the asynchronous operation.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="request" /> was <see langword="null" />.</exception>
		// Token: 0x06000176 RID: 374 RVA: 0x00005EAC File Offset: 0x000040AC
		protected internal sealed override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			request = this.ProcessRequest(request, cancellationToken);
			HttpResponseMessage httpResponseMessage = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
			return this.ProcessResponse(httpResponseMessage, cancellationToken);
		}
	}
}
