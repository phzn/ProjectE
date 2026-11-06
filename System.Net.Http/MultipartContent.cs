using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace System.Net.Http
{
	/// <summary>Provides a collection of <see cref="T:System.Net.Http.HttpContent" /> objects that get serialized using the multipart/* content type specification.</summary>
	// Token: 0x0200002E RID: 46
	public class MultipartContent : HttpContent, IEnumerable<HttpContent>, IEnumerable
	{
		/// <summary>Creates a new instance of the <see cref="T:System.Net.Http.MultipartContent" /> class.</summary>
		// Token: 0x0600017A RID: 378 RVA: 0x00006012 File Offset: 0x00004212
		public MultipartContent()
			: this("mixed")
		{
		}

		/// <summary>Creates a new instance of the <see cref="T:System.Net.Http.MultipartContent" /> class.</summary>
		/// <param name="subtype">The subtype of the multipart content.</param>
		/// <exception cref="T:System.ArgumentException">The <paramref name="subtype" /> was <see langword="null" /> or contains only white space characters.</exception>
		// Token: 0x0600017B RID: 379 RVA: 0x00006020 File Offset: 0x00004220
		public MultipartContent(string subtype)
			: this(subtype, Guid.NewGuid().ToString("D", CultureInfo.InvariantCulture))
		{
		}

		/// <summary>Creates a new instance of the <see cref="T:System.Net.Http.MultipartContent" /> class.</summary>
		/// <param name="subtype">The subtype of the multipart content.</param>
		/// <param name="boundary">The boundary string for the multipart content.</param>
		/// <exception cref="T:System.ArgumentException">The <paramref name="subtype" /> was <see langword="null" /> or an empty string.  
		///  The <paramref name="boundary" /> was <see langword="null" /> or contains only white space characters.  
		///  -or-  
		///  The <paramref name="boundary" /> ends with a space character.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The length of the <paramref name="boundary" /> was greater than 70.</exception>
		// Token: 0x0600017C RID: 380 RVA: 0x0000604C File Offset: 0x0000424C
		public MultipartContent(string subtype, string boundary)
		{
			if (string.IsNullOrWhiteSpace(subtype))
			{
				throw new ArgumentException("boundary");
			}
			if (string.IsNullOrWhiteSpace(boundary))
			{
				throw new ArgumentException("boundary");
			}
			if (boundary.Length > 70)
			{
				throw new ArgumentOutOfRangeException("boundary");
			}
			if (boundary.Last<char>() == ' ' || !MultipartContent.IsValidRFC2049(boundary))
			{
				throw new ArgumentException("boundary");
			}
			this.boundary = boundary;
			this.nested_content = new List<HttpContent>(2);
			base.Headers.ContentType = new MediaTypeHeaderValue("multipart/" + subtype)
			{
				Parameters = 
				{
					new NameValueHeaderValue("boundary", "\"" + boundary + "\"")
				}
			};
		}

		// Token: 0x0600017D RID: 381 RVA: 0x0000610C File Offset: 0x0000430C
		private static bool IsValidRFC2049(string s)
		{
			foreach (char c in s)
			{
				if ((c < 'a' || c > 'z') && (c < 'A' || c > 'Z') && (c < '0' || c > '9'))
				{
					if (c <= ':')
					{
						switch (c)
						{
						case '\'':
						case '(':
						case ')':
						case '+':
						case ',':
						case '-':
						case '.':
						case '/':
							goto IL_0071;
						case '*':
							break;
						default:
							if (c == ':')
							{
								goto IL_0071;
							}
							break;
						}
					}
					else if (c == '=' || c == '?')
					{
						goto IL_0071;
					}
					return false;
				}
				IL_0071:;
			}
			return true;
		}

		/// <summary>Add multipart HTTP content to a collection of <see cref="T:System.Net.Http.HttpContent" /> objects that get serialized using the multipart/* content type specification.</summary>
		/// <param name="content">The HTTP content to add to the collection.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="content" /> was <see langword="null" />.</exception>
		// Token: 0x0600017E RID: 382 RVA: 0x00006198 File Offset: 0x00004398
		public virtual void Add(HttpContent content)
		{
			if (content == null)
			{
				throw new ArgumentNullException("content");
			}
			if (this.nested_content == null)
			{
				this.nested_content = new List<HttpContent>();
			}
			this.nested_content.Add(content);
		}

		/// <summary>Releases the unmanaged resources used by the <see cref="T:System.Net.Http.MultipartContent" /> and optionally disposes of the managed resources.</summary>
		/// <param name="disposing">
		///   <see langword="true" /> to release both managed and unmanaged resources; <see langword="false" /> to releases only unmanaged resources.</param>
		// Token: 0x0600017F RID: 383 RVA: 0x000061C8 File Offset: 0x000043C8
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				foreach (HttpContent httpContent in this.nested_content)
				{
					httpContent.Dispose();
				}
				this.nested_content = null;
			}
			base.Dispose(disposing);
		}

		/// <summary>Serialize the multipart HTTP content to a stream as an asynchronous operation.</summary>
		/// <param name="stream">The target stream.</param>
		/// <param name="context">Information about the transport (channel binding token, for example). This parameter may be <see langword="null" />.</param>
		/// <returns>The task object representing the asynchronous operation.</returns>
		// Token: 0x06000180 RID: 384 RVA: 0x0000622C File Offset: 0x0000442C
		protected override async Task SerializeToStreamAsync(Stream stream, TransportContext context)
		{
			StringBuilder sb = new StringBuilder();
			sb.Append('-').Append('-');
			sb.Append(this.boundary);
			sb.Append('\r').Append('\n');
			byte[] array;
			for (int i = 0; i < this.nested_content.Count; i++)
			{
				HttpContent c = this.nested_content[i];
				foreach (KeyValuePair<string, IEnumerable<string>> keyValuePair in c.Headers)
				{
					sb.Append(keyValuePair.Key);
					sb.Append(':').Append(' ');
					foreach (string text in keyValuePair.Value)
					{
						sb.Append(text);
					}
					sb.Append('\r').Append('\n');
				}
				sb.Append('\r').Append('\n');
				array = Encoding.ASCII.GetBytes(sb.ToString());
				sb.Clear();
				await stream.WriteAsync(array, 0, array.Length).ConfigureAwait(false);
				await c.SerializeToStreamAsync_internal(stream, context).ConfigureAwait(false);
				if (i != this.nested_content.Count - 1)
				{
					sb.Append('\r').Append('\n');
					sb.Append('-').Append('-');
					sb.Append(this.boundary);
					sb.Append('\r').Append('\n');
				}
				c = null;
			}
			sb.Append('\r').Append('\n');
			sb.Append('-').Append('-');
			sb.Append(this.boundary);
			sb.Append('-').Append('-');
			sb.Append('\r').Append('\n');
			array = Encoding.ASCII.GetBytes(sb.ToString());
			await stream.WriteAsync(array, 0, array.Length).ConfigureAwait(false);
		}

		/// <summary>Determines whether the HTTP multipart content has a valid length in bytes.</summary>
		/// <param name="length">The length in bytes of the HHTP content.</param>
		/// <returns>
		///   <see langword="true" /> if <paramref name="length" /> is a valid length; otherwise, <see langword="false" />.</returns>
		// Token: 0x06000181 RID: 385 RVA: 0x00006280 File Offset: 0x00004480
		protected internal override bool TryComputeLength(out long length)
		{
			length = (long)(12 + 2 * this.boundary.Length);
			for (int i = 0; i < this.nested_content.Count; i++)
			{
				HttpContent httpContent = this.nested_content[i];
				foreach (KeyValuePair<string, IEnumerable<string>> keyValuePair in httpContent.Headers)
				{
					length += (long)keyValuePair.Key.Length;
					length += 4L;
					foreach (string text in keyValuePair.Value)
					{
						length += (long)text.Length;
					}
				}
				long num;
				if (!httpContent.TryComputeLength(out num))
				{
					return false;
				}
				length += 2L;
				length += num;
				if (i != this.nested_content.Count - 1)
				{
					length += 6L;
					length += (long)this.boundary.Length;
				}
			}
			return true;
		}

		/// <summary>Returns an enumerator that iterates through the collection of <see cref="T:System.Net.Http.HttpContent" /> objects that get serialized using the multipart/* content type specification.</summary>
		/// <returns>An object that can be used to iterate through the collection.</returns>
		// Token: 0x06000182 RID: 386 RVA: 0x000063A8 File Offset: 0x000045A8
		public IEnumerator<HttpContent> GetEnumerator()
		{
			return this.nested_content.GetEnumerator();
		}

		/// <summary>The explicit implementation of the <see cref="M:System.Net.Http.MultipartContent.GetEnumerator" /> method.</summary>
		/// <returns>An object that can be used to iterate through the collection.</returns>
		// Token: 0x06000183 RID: 387 RVA: 0x000063A8 File Offset: 0x000045A8
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.nested_content.GetEnumerator();
		}

		// Token: 0x040000C9 RID: 201
		private List<HttpContent> nested_content;

		// Token: 0x040000CA RID: 202
		private readonly string boundary;
	}
}
