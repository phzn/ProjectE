using System;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Metadata.W3cXsd2001
{
	/// <summary>Wraps an XML <see langword="normalizedString" /> type.</summary>
	// Token: 0x020005F8 RID: 1528
	[ComVisible(true)]
	[Serializable]
	public sealed class SoapNormalizedString : ISoapXsd
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Runtime.Remoting.Metadata.W3cXsd2001.SoapNormalizedString" /> class.</summary>
		// Token: 0x060039E3 RID: 14819 RVA: 0x0000259F File Offset: 0x0000079F
		public SoapNormalizedString()
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Runtime.Remoting.Metadata.W3cXsd2001.SoapNormalizedString" /> class with a normalized string.</summary>
		/// <param name="value">A <see cref="T:System.String" /> object that contains a normalized string.</param>
		/// <exception cref="T:System.Runtime.Remoting.RemotingException">
		///   <paramref name="value" /> contains invalid characters (0xD, 0xA, or 0x9).</exception>
		// Token: 0x060039E4 RID: 14820 RVA: 0x000CBF98 File Offset: 0x000CA198
		public SoapNormalizedString(string value)
		{
			this._value = SoapHelper.Normalize(value);
		}

		/// <summary>Gets or sets a normalized string.</summary>
		/// <returns>A <see cref="T:System.String" /> object that contains a normalized string.</returns>
		/// <exception cref="T:System.Runtime.Remoting.RemotingException">
		///   <paramref name="value" /> contains invalid characters (0xD, 0xA, or 0x9).</exception>
		// Token: 0x17000863 RID: 2147
		// (get) Token: 0x060039E5 RID: 14821 RVA: 0x000CBFAC File Offset: 0x000CA1AC
		// (set) Token: 0x060039E6 RID: 14822 RVA: 0x000CBFB4 File Offset: 0x000CA1B4
		public string Value
		{
			get
			{
				return this._value;
			}
			set
			{
				this._value = value;
			}
		}

		/// <summary>Gets the XML Schema definition language (XSD) of the current SOAP type.</summary>
		/// <returns>A <see cref="T:System.String" /> that indicates the XSD of the current SOAP type.</returns>
		// Token: 0x17000864 RID: 2148
		// (get) Token: 0x060039E7 RID: 14823 RVA: 0x000CBFBD File Offset: 0x000CA1BD
		public static string XsdType
		{
			get
			{
				return "normalizedString";
			}
		}

		/// <summary>Returns the XML Schema definition language (XSD) of the current SOAP type.</summary>
		/// <returns>A <see cref="T:System.String" /> that indicates the XSD of the current SOAP type.</returns>
		// Token: 0x060039E8 RID: 14824 RVA: 0x000CBFC4 File Offset: 0x000CA1C4
		public string GetXsdType()
		{
			return SoapNormalizedString.XsdType;
		}

		/// <summary>Converts the specified <see cref="T:System.String" /> into a <see cref="T:System.Runtime.Remoting.Metadata.W3cXsd2001.SoapNormalizedString" /> object.</summary>
		/// <param name="value">The <see langword="String" /> to convert.</param>
		/// <returns>A <see cref="T:System.Runtime.Remoting.Metadata.W3cXsd2001.SoapNormalizedString" /> object obtained from <paramref name="value" />.</returns>
		/// <exception cref="T:System.Runtime.Remoting.RemotingException">
		///   <paramref name="value" /> contains invalid characters (0xD, 0xA, or 0x9).</exception>
		// Token: 0x060039E9 RID: 14825 RVA: 0x000CBFCB File Offset: 0x000CA1CB
		public static SoapNormalizedString Parse(string value)
		{
			return new SoapNormalizedString(value);
		}

		/// <summary>Returns <see cref="P:System.Runtime.Remoting.Metadata.W3cXsd2001.SoapNormalizedString.Value" /> as a <see cref="T:System.String" />.</summary>
		/// <returns>A <see cref="T:System.String" /> that is obtained from <see cref="P:System.Runtime.Remoting.Metadata.W3cXsd2001.SoapNormalizedString.Value" /> in the format "&lt;![CDATA[" + <see cref="P:System.Runtime.Remoting.Metadata.W3cXsd2001.SoapNormalizedString.Value" /> + "]]&gt;".</returns>
		// Token: 0x060039EA RID: 14826 RVA: 0x000CBFAC File Offset: 0x000CA1AC
		public override string ToString()
		{
			return this._value;
		}

		// Token: 0x0400263B RID: 9787
		private string _value;
	}
}
