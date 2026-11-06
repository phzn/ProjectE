using System;
using Unity;

namespace System.Text
{
	/// <summary>Provides basic information about an encoding.</summary>
	// Token: 0x020003A6 RID: 934
	[Serializable]
	public sealed class EncodingInfo
	{
		// Token: 0x06002648 RID: 9800 RVA: 0x00087B44 File Offset: 0x00085D44
		internal EncodingInfo(int codePage, string name, string displayName)
		{
			this.iCodePage = codePage;
			this.strEncodingName = name;
			this.strDisplayName = displayName;
		}

		/// <summary>Gets the code page identifier of the encoding.</summary>
		/// <returns>The code page identifier of the encoding.</returns>
		// Token: 0x170004B3 RID: 1203
		// (get) Token: 0x06002649 RID: 9801 RVA: 0x00087B61 File Offset: 0x00085D61
		public int CodePage
		{
			get
			{
				return this.iCodePage;
			}
		}

		/// <summary>Gets the name registered with the Internet Assigned Numbers Authority (IANA) for the encoding.</summary>
		/// <returns>The IANA name for the encoding. For more information about the IANA, see www.iana.org.</returns>
		// Token: 0x170004B4 RID: 1204
		// (get) Token: 0x0600264A RID: 9802 RVA: 0x00087B69 File Offset: 0x00085D69
		public string Name
		{
			get
			{
				return this.strEncodingName;
			}
		}

		/// <summary>Gets the human-readable description of the encoding.</summary>
		/// <returns>The human-readable description of the encoding.</returns>
		// Token: 0x170004B5 RID: 1205
		// (get) Token: 0x0600264B RID: 9803 RVA: 0x00087B71 File Offset: 0x00085D71
		public string DisplayName
		{
			get
			{
				return this.strDisplayName;
			}
		}

		/// <summary>Returns a <see cref="T:System.Text.Encoding" /> object that corresponds to the current <see cref="T:System.Text.EncodingInfo" /> object.</summary>
		/// <returns>A <see cref="T:System.Text.Encoding" /> object that corresponds to the current <see cref="T:System.Text.EncodingInfo" /> object.</returns>
		// Token: 0x0600264C RID: 9804 RVA: 0x00087B79 File Offset: 0x00085D79
		public Encoding GetEncoding()
		{
			return Encoding.GetEncoding(this.iCodePage);
		}

		/// <summary>Gets a value indicating whether the specified object is equal to the current <see cref="T:System.Text.EncodingInfo" /> object.</summary>
		/// <param name="value">An object to compare to the current <see cref="T:System.Text.EncodingInfo" /> object.</param>
		/// <returns>
		///   <see langword="true" /> if <paramref name="value" /> is a <see cref="T:System.Text.EncodingInfo" /> object and is equal to the current <see cref="T:System.Text.EncodingInfo" /> object; otherwise, <see langword="false" />.</returns>
		// Token: 0x0600264D RID: 9805 RVA: 0x00087B88 File Offset: 0x00085D88
		public override bool Equals(object value)
		{
			EncodingInfo encodingInfo = value as EncodingInfo;
			return encodingInfo != null && this.CodePage == encodingInfo.CodePage;
		}

		/// <summary>Returns the hash code for the current <see cref="T:System.Text.EncodingInfo" /> object.</summary>
		/// <returns>A 32-bit signed integer hash code.</returns>
		// Token: 0x0600264E RID: 9806 RVA: 0x00087BAF File Offset: 0x00085DAF
		public override int GetHashCode()
		{
			return this.CodePage;
		}

		// Token: 0x0600264F RID: 9807 RVA: 0x000173AD File Offset: 0x000155AD
		internal EncodingInfo()
		{
			ThrowStub.ThrowNotSupportedException();
		}

		// Token: 0x04001DC4 RID: 7620
		private int iCodePage;

		// Token: 0x04001DC5 RID: 7621
		private string strEncodingName;

		// Token: 0x04001DC6 RID: 7622
		private string strDisplayName;
	}
}
