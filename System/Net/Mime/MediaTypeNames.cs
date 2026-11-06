using System;

namespace System.Net.Mime
{
	/// <summary>Specifies the media type information for an email message attachment.</summary>
	// Token: 0x020007D5 RID: 2005
	public static class MediaTypeNames
	{
		/// <summary>Specifies the type of text data in an email message attachment.</summary>
		// Token: 0x020007D6 RID: 2006
		public static class Text
		{
			/// <summary>Specifies that the <see cref="T:System.Net.Mime.MediaTypeNames.Text" /> data is in plain text format.</summary>
			// Token: 0x04002677 RID: 9847
			public const string Plain = "text/plain";

			/// <summary>Specifies that the <see cref="T:System.Net.Mime.MediaTypeNames.Text" /> data is in HTML format.</summary>
			// Token: 0x04002678 RID: 9848
			public const string Html = "text/html";

			/// <summary>Specifies that the <see cref="T:System.Net.Mime.MediaTypeNames.Text" /> data is in XML format.</summary>
			// Token: 0x04002679 RID: 9849
			public const string Xml = "text/xml";

			/// <summary>Specifies that the <see cref="T:System.Net.Mime.MediaTypeNames.Text" /> data is in Rich Text Format (RTF).</summary>
			// Token: 0x0400267A RID: 9850
			public const string RichText = "text/richtext";
		}

		/// <summary>Specifies the kind of application data in an email message attachment.</summary>
		// Token: 0x020007D7 RID: 2007
		public static class Application
		{
			/// <summary>Specifies that the <see cref="T:System.Net.Mime.MediaTypeNames.Application" /> data is a SOAP document.</summary>
			// Token: 0x0400267B RID: 9851
			public const string Soap = "application/soap+xml";

			/// <summary>Specifies that the <see cref="T:System.Net.Mime.MediaTypeNames.Application" /> data is not interpreted.</summary>
			// Token: 0x0400267C RID: 9852
			public const string Octet = "application/octet-stream";

			/// <summary>Specifies that the <see cref="T:System.Net.Mime.MediaTypeNames.Application" /> data is in Rich Text Format (RTF).</summary>
			// Token: 0x0400267D RID: 9853
			public const string Rtf = "application/rtf";

			/// <summary>Specifies that the <see cref="T:System.Net.Mime.MediaTypeNames.Application" /> data is in Portable Document Format (PDF).</summary>
			// Token: 0x0400267E RID: 9854
			public const string Pdf = "application/pdf";

			/// <summary>Specifies that the <see cref="T:System.Net.Mime.MediaTypeNames.Application" /> data is compressed.</summary>
			// Token: 0x0400267F RID: 9855
			public const string Zip = "application/zip";

			// Token: 0x04002680 RID: 9856
			public const string Json = "application/json";

			// Token: 0x04002681 RID: 9857
			public const string Xml = "application/xml";
		}

		/// <summary>Specifies the type of image data in an email message attachment.</summary>
		// Token: 0x020007D8 RID: 2008
		public static class Image
		{
			/// <summary>Specifies that the <see cref="T:System.Net.Mime.MediaTypeNames.Image" /> data is in Graphics Interchange Format (GIF).</summary>
			// Token: 0x04002682 RID: 9858
			public const string Gif = "image/gif";

			/// <summary>Specifies that the <see cref="T:System.Net.Mime.MediaTypeNames.Image" /> data is in Tagged Image File Format (TIFF).</summary>
			// Token: 0x04002683 RID: 9859
			public const string Tiff = "image/tiff";

			/// <summary>Specifies that the <see cref="T:System.Net.Mime.MediaTypeNames.Image" /> data is in Joint Photographic Experts Group (JPEG) format.</summary>
			// Token: 0x04002684 RID: 9860
			public const string Jpeg = "image/jpeg";
		}
	}
}
