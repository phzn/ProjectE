using System;

namespace System.Net.Mime
{
	/// <summary>Specifies the Content-Transfer-Encoding header information for an email message attachment.</summary>
	// Token: 0x020007E9 RID: 2025
	public enum TransferEncoding
	{
		/// <summary>Indicates that the transfer encoding is unknown.</summary>
		// Token: 0x040026DE RID: 9950
		Unknown = -1,
		/// <summary>Encodes data that consists of printable characters in the US-ASCII character set. See RFC 2406 Section 6.7.</summary>
		// Token: 0x040026DF RID: 9951
		QuotedPrintable,
		/// <summary>Encodes stream-based data. See RFC 2406 Section 6.8.</summary>
		// Token: 0x040026E0 RID: 9952
		Base64,
		/// <summary>Used for data that is not encoded. The data is in 7-bit US-ASCII characters with a total line length of no longer than 1000 characters. See RFC2406 Section 2.7.</summary>
		// Token: 0x040026E1 RID: 9953
		SevenBit,
		/// <summary>The data is in 8-bit characters that may represent international characters with a total line length of no longer than 1000 8-bit characters. For more information about this 8-bit MIME transport extension, see IETF RFC 6152.</summary>
		// Token: 0x040026E2 RID: 9954
		EightBit
	}
}
