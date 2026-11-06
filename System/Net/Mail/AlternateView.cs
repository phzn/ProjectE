using System;
using System.IO;
using System.Net.Mime;
using System.Text;

namespace System.Net.Mail
{
	/// <summary>Represents the format to view an email message.</summary>
	// Token: 0x020007F7 RID: 2039
	public class AlternateView : AttachmentBase
	{
		/// <summary>Initializes a new instance of <see cref="T:System.Net.Mail.AlternateView" /> with the specified file name.</summary>
		/// <param name="fileName">The name of the file that contains the content for this alternate view.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="fileName" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred, such as a disk error.</exception>
		/// <exception cref="T:System.UnauthorizedAccessException">The access requested is not permitted by the operating system for the specified file handle, such as when access is Write or ReadWrite and the file handle is set for read-only access.</exception>
		// Token: 0x0600412D RID: 16685 RVA: 0x000DF96D File Offset: 0x000DDB6D
		public AlternateView(string fileName)
			: base(fileName)
		{
			if (fileName == null)
			{
				throw new ArgumentNullException();
			}
		}

		/// <summary>Initializes a new instance of <see cref="T:System.Net.Mail.AlternateView" /> with the specified file name and content type.</summary>
		/// <param name="fileName">The name of the file that contains the content for this alternate view.</param>
		/// <param name="contentType">The type of the content.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="fileName" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="contentType" /> is not a valid value.</exception>
		/// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred, such as a disk error.</exception>
		/// <exception cref="T:System.UnauthorizedAccessException">The access requested is not permitted by the operating system for the specified file handle, such as when access is Write or ReadWrite and the file handle is set for read-only access.</exception>
		// Token: 0x0600412E RID: 16686 RVA: 0x000DF98A File Offset: 0x000DDB8A
		public AlternateView(string fileName, ContentType contentType)
			: base(fileName, contentType)
		{
			if (fileName == null)
			{
				throw new ArgumentNullException();
			}
		}

		/// <summary>Initializes a new instance of <see cref="T:System.Net.Mail.AlternateView" /> with the specified file name and media type.</summary>
		/// <param name="fileName">The name of the file that contains the content for this alternate view.</param>
		/// <param name="mediaType">The MIME media type of the content.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="fileName" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="mediaType" /> is not a valid value.</exception>
		/// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred, such as a disk error.</exception>
		/// <exception cref="T:System.UnauthorizedAccessException">The access requested is not permitted by the operating system for the specified file handle, such as when access is Write or ReadWrite and the file handle is set for read-only access.</exception>
		// Token: 0x0600412F RID: 16687 RVA: 0x000DF9A8 File Offset: 0x000DDBA8
		public AlternateView(string fileName, string mediaType)
			: base(fileName, mediaType)
		{
			if (fileName == null)
			{
				throw new ArgumentNullException();
			}
		}

		/// <summary>Initializes a new instance of <see cref="T:System.Net.Mail.AlternateView" /> with the specified <see cref="T:System.IO.Stream" />.</summary>
		/// <param name="contentStream">A stream that contains the content for this view.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="contentStream" /> is <see langword="null" />.</exception>
		// Token: 0x06004130 RID: 16688 RVA: 0x000DF9C6 File Offset: 0x000DDBC6
		public AlternateView(Stream contentStream)
			: base(contentStream)
		{
		}

		/// <summary>Initializes a new instance of <see cref="T:System.Net.Mail.AlternateView" /> with the specified <see cref="T:System.IO.Stream" /> and media type.</summary>
		/// <param name="contentStream">A stream that contains the content for this attachment.</param>
		/// <param name="mediaType">The MIME media type of the content.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="contentStream" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="mediaType" /> is not a valid value.</exception>
		// Token: 0x06004131 RID: 16689 RVA: 0x000DF9DA File Offset: 0x000DDBDA
		public AlternateView(Stream contentStream, string mediaType)
			: base(contentStream, mediaType)
		{
		}

		/// <summary>Initializes a new instance of <see cref="T:System.Net.Mail.AlternateView" /> with the specified <see cref="T:System.IO.Stream" /> and <see cref="T:System.Net.Mime.ContentType" />.</summary>
		/// <param name="contentStream">A stream that contains the content for this attachment.</param>
		/// <param name="contentType">The type of the content.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="contentStream" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="contentType" /> is not a valid value.</exception>
		// Token: 0x06004132 RID: 16690 RVA: 0x000DF9EF File Offset: 0x000DDBEF
		public AlternateView(Stream contentStream, ContentType contentType)
			: base(contentStream, contentType)
		{
		}

		/// <summary>Gets or sets the base URI to use for resolving relative URIs in the <see cref="T:System.Net.Mail.AlternateView" />.</summary>
		/// <returns>The base URI to use for resolving relative URIs in the <see cref="T:System.Net.Mail.AlternateView" />.</returns>
		// Token: 0x17000EA7 RID: 3751
		// (get) Token: 0x06004133 RID: 16691 RVA: 0x000DFA04 File Offset: 0x000DDC04
		// (set) Token: 0x06004134 RID: 16692 RVA: 0x000DFA0C File Offset: 0x000DDC0C
		public Uri BaseUri
		{
			get
			{
				return this.baseUri;
			}
			set
			{
				this.baseUri = value;
			}
		}

		/// <summary>Gets the set of embedded resources referred to by this attachment.</summary>
		/// <returns>A <see cref="T:System.Net.Mail.LinkedResourceCollection" /> object that stores the collection of linked resources to be sent as part of an email message.</returns>
		// Token: 0x17000EA8 RID: 3752
		// (get) Token: 0x06004135 RID: 16693 RVA: 0x000DFA15 File Offset: 0x000DDC15
		public LinkedResourceCollection LinkedResources
		{
			get
			{
				return this.linkedResources;
			}
		}

		/// <summary>Creates a <see cref="T:System.Net.Mail.AlternateView" /> of an email message using the content specified in a <see cref="T:System.String" />.</summary>
		/// <param name="content">The <see cref="T:System.String" /> that contains the content of the email message.</param>
		/// <returns>An <see cref="T:System.Net.Mail.AlternateView" /> object that represents an alternate view of an email message.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="content" /> is null.</exception>
		// Token: 0x06004136 RID: 16694 RVA: 0x000DFA1D File Offset: 0x000DDC1D
		public static AlternateView CreateAlternateViewFromString(string content)
		{
			if (content == null)
			{
				throw new ArgumentNullException();
			}
			return new AlternateView(new MemoryStream(Encoding.UTF8.GetBytes(content)))
			{
				TransferEncoding = TransferEncoding.QuotedPrintable
			};
		}

		/// <summary>Creates an <see cref="T:System.Net.Mail.AlternateView" /> of an email message using the content specified in a <see cref="T:System.String" /> and the specified MIME media type of the content.</summary>
		/// <param name="content">A <see cref="T:System.String" /> that contains the content for this attachment.</param>
		/// <param name="contentType">A <see cref="T:System.Net.Mime.ContentType" /> that describes the data in <paramref name="content" />.</param>
		/// <returns>An <see cref="T:System.Net.Mail.AlternateView" /> object that represents an alternate view of an email message.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="content" /> is null.</exception>
		// Token: 0x06004137 RID: 16695 RVA: 0x000DFA44 File Offset: 0x000DDC44
		public static AlternateView CreateAlternateViewFromString(string content, ContentType contentType)
		{
			if (content == null)
			{
				throw new ArgumentNullException("content");
			}
			return new AlternateView(new MemoryStream(((contentType.CharSet != null) ? Encoding.GetEncoding(contentType.CharSet) : Encoding.UTF8).GetBytes(content)), contentType)
			{
				TransferEncoding = TransferEncoding.QuotedPrintable
			};
		}

		/// <summary>Creates an <see cref="T:System.Net.Mail.AlternateView" /> of an email message using the content specified in a <see cref="T:System.String" />, the specified text encoding, and MIME media type of the content.</summary>
		/// <param name="content">A <see cref="T:System.String" /> that contains the content for this attachment.</param>
		/// <param name="contentEncoding">An <see cref="T:System.Text.Encoding" />. This value can be <see langword="null." /></param>
		/// <param name="mediaType">The MIME media type of the content.</param>
		/// <returns>An <see cref="T:System.Net.Mail.AlternateView" /> object that represents an alternate view of an email message.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="content" /> is null.</exception>
		// Token: 0x06004138 RID: 16696 RVA: 0x000DFA94 File Offset: 0x000DDC94
		public static AlternateView CreateAlternateViewFromString(string content, Encoding contentEncoding, string mediaType)
		{
			if (content == null)
			{
				throw new ArgumentNullException("content");
			}
			if (contentEncoding == null)
			{
				contentEncoding = Encoding.UTF8;
			}
			return new AlternateView(new MemoryStream(contentEncoding.GetBytes(content)), new ContentType
			{
				MediaType = mediaType,
				CharSet = contentEncoding.HeaderName
			})
			{
				TransferEncoding = TransferEncoding.QuotedPrintable
			};
		}

		/// <summary>Releases the unmanaged resources used by the <see cref="T:System.Net.Mail.AlternateView" /> and optionally releases the managed resources.</summary>
		/// <param name="disposing">
		///   <see langword="true" /> to release both managed and unmanaged resources; <see langword="false" /> to release only unmanaged resources.</param>
		// Token: 0x06004139 RID: 16697 RVA: 0x000DFAEC File Offset: 0x000DDCEC
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				foreach (LinkedResource linkedResource in this.linkedResources)
				{
					linkedResource.Dispose();
				}
			}
			base.Dispose(disposing);
		}

		// Token: 0x0400271C RID: 10012
		private Uri baseUri;

		// Token: 0x0400271D RID: 10013
		private LinkedResourceCollection linkedResources = new LinkedResourceCollection();
	}
}
