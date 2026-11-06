using System;
using System.IO;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace System.Net.Http
{
	/// <summary>A base class representing an HTTP entity body and content headers.</summary>
	// Token: 0x0200001F RID: 31
	public abstract class HttpContent : IDisposable
	{
		/// <summary>Gets the HTTP content headers as defined in RFC 2616.</summary>
		/// <returns>The content headers as defined in RFC 2616.</returns>
		// Token: 0x1700004C RID: 76
		// (get) Token: 0x0600010E RID: 270 RVA: 0x00004F24 File Offset: 0x00003124
		public HttpContentHeaders Headers
		{
			get
			{
				HttpContentHeaders httpContentHeaders;
				if ((httpContentHeaders = this.headers) == null)
				{
					httpContentHeaders = (this.headers = new HttpContentHeaders(this));
				}
				return httpContentHeaders;
			}
		}

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x0600010F RID: 271 RVA: 0x00004F4C File Offset: 0x0000314C
		internal long? LoadedBufferLength
		{
			get
			{
				if (this.buffer != null)
				{
					return new long?(this.buffer.Length);
				}
				return null;
			}
		}

		// Token: 0x06000110 RID: 272 RVA: 0x00004F7B File Offset: 0x0000317B
		internal void CopyTo(Stream stream)
		{
			this.CopyToAsync(stream).Wait();
		}

		/// <summary>Serialize the HTTP content into a stream of bytes and copies it to the stream object provided as the <paramref name="stream" /> parameter.</summary>
		/// <param name="stream">The target stream.</param>
		/// <returns>The task object representing the asynchronous operation.</returns>
		// Token: 0x06000111 RID: 273 RVA: 0x00004F89 File Offset: 0x00003189
		public Task CopyToAsync(Stream stream)
		{
			return this.CopyToAsync(stream, null);
		}

		/// <summary>Serialize the HTTP content into a stream of bytes and copies it to the stream object provided as the <paramref name="stream" /> parameter.</summary>
		/// <param name="stream">The target stream.</param>
		/// <param name="context">Information about the transport (channel binding token, for example). This parameter may be <see langword="null" />.</param>
		/// <returns>The task object representing the asynchronous operation.</returns>
		// Token: 0x06000112 RID: 274 RVA: 0x00004F93 File Offset: 0x00003193
		public Task CopyToAsync(Stream stream, TransportContext context)
		{
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			if (this.buffer != null)
			{
				return this.buffer.CopyToAsync(stream);
			}
			return this.SerializeToStreamAsync(stream, context);
		}

		/// <summary>Serialize the HTTP content to a memory stream as an asynchronous operation.</summary>
		/// <returns>The task object representing the asynchronous operation.</returns>
		// Token: 0x06000113 RID: 275 RVA: 0x00004FC0 File Offset: 0x000031C0
		protected virtual async Task<Stream> CreateContentReadStreamAsync()
		{
			await this.LoadIntoBufferAsync().ConfigureAwait(false);
			return this.buffer;
		}

		// Token: 0x06000114 RID: 276 RVA: 0x00005003 File Offset: 0x00003203
		private static HttpContent.FixedMemoryStream CreateFixedMemoryStream(long maxBufferSize)
		{
			return new HttpContent.FixedMemoryStream(maxBufferSize);
		}

		/// <summary>Releases the unmanaged resources and disposes of the managed resources used by the <see cref="T:System.Net.Http.HttpContent" />.</summary>
		// Token: 0x06000115 RID: 277 RVA: 0x0000500B File Offset: 0x0000320B
		public void Dispose()
		{
			this.Dispose(true);
		}

		/// <summary>Releases the unmanaged resources used by the <see cref="T:System.Net.Http.HttpContent" /> and optionally disposes of the managed resources.</summary>
		/// <param name="disposing">
		///   <see langword="true" /> to release both managed and unmanaged resources; <see langword="false" /> to releases only unmanaged resources.</param>
		// Token: 0x06000116 RID: 278 RVA: 0x00005014 File Offset: 0x00003214
		protected virtual void Dispose(bool disposing)
		{
			if (disposing && !this.disposed)
			{
				this.disposed = true;
				if (this.buffer != null)
				{
					this.buffer.Dispose();
				}
			}
		}

		/// <summary>Serialize the HTTP content to a memory buffer as an asynchronous operation.</summary>
		/// <returns>The task object representing the asynchronous operation.</returns>
		// Token: 0x06000117 RID: 279 RVA: 0x0000503B File Offset: 0x0000323B
		public Task LoadIntoBufferAsync()
		{
			return this.LoadIntoBufferAsync(2147483647L);
		}

		/// <summary>Serialize the HTTP content to a memory buffer as an asynchronous operation.</summary>
		/// <param name="maxBufferSize">The maximum size, in bytes, of the buffer to use.</param>
		/// <returns>The task object representing the asynchronous operation.</returns>
		// Token: 0x06000118 RID: 280 RVA: 0x0000504C File Offset: 0x0000324C
		public async Task LoadIntoBufferAsync(long maxBufferSize)
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException(base.GetType().ToString());
			}
			if (this.buffer == null)
			{
				this.buffer = HttpContent.CreateFixedMemoryStream(maxBufferSize);
				await this.SerializeToStreamAsync(this.buffer, null).ConfigureAwait(false);
				this.buffer.Seek(0L, SeekOrigin.Begin);
			}
		}

		/// <summary>Serialize the HTTP content and return a stream that represents the content as an asynchronous operation.</summary>
		/// <returns>The task object representing the asynchronous operation.</returns>
		// Token: 0x06000119 RID: 281 RVA: 0x00005098 File Offset: 0x00003298
		public async Task<Stream> ReadAsStreamAsync()
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException(base.GetType().ToString());
			}
			Stream stream;
			if (this.buffer != null)
			{
				stream = new MemoryStream(this.buffer.GetBuffer(), 0, (int)this.buffer.Length, false);
			}
			else
			{
				if (this.stream == null)
				{
					Stream stream2 = await this.CreateContentReadStreamAsync().ConfigureAwait(false);
					this.stream = stream2;
				}
				stream = this.stream;
			}
			return stream;
		}

		/// <summary>Serialize the HTTP content to a byte array as an asynchronous operation.</summary>
		/// <returns>The task object representing the asynchronous operation.</returns>
		// Token: 0x0600011A RID: 282 RVA: 0x000050DC File Offset: 0x000032DC
		public async Task<byte[]> ReadAsByteArrayAsync()
		{
			await this.LoadIntoBufferAsync().ConfigureAwait(false);
			return this.buffer.ToArray();
		}

		/// <summary>Serialize the HTTP content to a string as an asynchronous operation.</summary>
		/// <returns>The task object representing the asynchronous operation.</returns>
		// Token: 0x0600011B RID: 283 RVA: 0x00005120 File Offset: 0x00003320
		public async Task<string> ReadAsStringAsync()
		{
			await this.LoadIntoBufferAsync().ConfigureAwait(false);
			string text;
			if (this.buffer.Length == 0L)
			{
				text = string.Empty;
			}
			else
			{
				byte[] array = this.buffer.GetBuffer();
				int num = (int)this.buffer.Length;
				int num2 = 0;
				Encoding encoding;
				if (this.headers != null && this.headers.ContentType != null && this.headers.ContentType.CharSet != null)
				{
					encoding = Encoding.GetEncoding(this.headers.ContentType.CharSet);
					num2 = HttpContent.StartsWith(array, num, encoding.GetPreamble());
				}
				else
				{
					encoding = HttpContent.GetEncodingFromBuffer(array, num, ref num2) ?? Encoding.UTF8;
				}
				text = encoding.GetString(array, num2, num - num2);
			}
			return text;
		}

		// Token: 0x0600011C RID: 284 RVA: 0x00005164 File Offset: 0x00003364
		private static Encoding GetEncodingFromBuffer(byte[] buffer, int length, ref int preambleLength)
		{
			foreach (Encoding encoding in new Encoding[]
			{
				Encoding.UTF8,
				Encoding.UTF32,
				Encoding.Unicode
			})
			{
				if ((preambleLength = HttpContent.StartsWith(buffer, length, encoding.GetPreamble())) != 0)
				{
					return encoding;
				}
			}
			return null;
		}

		// Token: 0x0600011D RID: 285 RVA: 0x000051BC File Offset: 0x000033BC
		private static int StartsWith(byte[] array, int length, byte[] value)
		{
			if (length < value.Length)
			{
				return 0;
			}
			for (int i = 0; i < value.Length; i++)
			{
				if (array[i] != value[i])
				{
					return 0;
				}
			}
			return value.Length;
		}

		// Token: 0x0600011E RID: 286 RVA: 0x000051EC File Offset: 0x000033EC
		internal Task SerializeToStreamAsync_internal(Stream stream, TransportContext context)
		{
			return this.SerializeToStreamAsync(stream, context);
		}

		/// <summary>Serialize the HTTP content to a stream as an asynchronous operation.</summary>
		/// <param name="stream">The target stream.</param>
		/// <param name="context">Information about the transport (channel binding token, for example). This parameter may be <see langword="null" />.</param>
		/// <returns>The task object representing the asynchronous operation.</returns>
		// Token: 0x0600011F RID: 287
		protected abstract Task SerializeToStreamAsync(Stream stream, TransportContext context);

		/// <summary>Determines whether the HTTP content has a valid length in bytes.</summary>
		/// <param name="length">The length in bytes of the HTTP content.</param>
		/// <returns>
		///   <see langword="true" /> if <paramref name="length" /> is a valid length; otherwise, <see langword="false" />.</returns>
		// Token: 0x06000120 RID: 288
		protected internal abstract bool TryComputeLength(out long length);

		// Token: 0x0400008F RID: 143
		private HttpContent.FixedMemoryStream buffer;

		// Token: 0x04000090 RID: 144
		private Stream stream;

		// Token: 0x04000091 RID: 145
		private bool disposed;

		// Token: 0x04000092 RID: 146
		private HttpContentHeaders headers;

		// Token: 0x02000020 RID: 32
		private sealed class FixedMemoryStream : MemoryStream
		{
			// Token: 0x06000122 RID: 290 RVA: 0x000051F6 File Offset: 0x000033F6
			public FixedMemoryStream(long maxSize)
			{
				this.maxSize = maxSize;
			}

			// Token: 0x06000123 RID: 291 RVA: 0x00005205 File Offset: 0x00003405
			private void CheckOverflow(int count)
			{
				if (this.Length + (long)count > this.maxSize)
				{
					throw new HttpRequestException(string.Format("Cannot write more bytes to the buffer than the configured maximum buffer size: {0}", this.maxSize));
				}
			}

			// Token: 0x06000124 RID: 292 RVA: 0x00005233 File Offset: 0x00003433
			public override void WriteByte(byte value)
			{
				this.CheckOverflow(1);
				base.WriteByte(value);
			}

			// Token: 0x06000125 RID: 293 RVA: 0x00005243 File Offset: 0x00003443
			public override void Write(byte[] buffer, int offset, int count)
			{
				this.CheckOverflow(count);
				base.Write(buffer, offset, count);
			}

			// Token: 0x04000093 RID: 147
			private readonly long maxSize;
		}
	}
}
