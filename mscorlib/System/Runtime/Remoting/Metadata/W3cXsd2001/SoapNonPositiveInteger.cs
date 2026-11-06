using System;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Metadata.W3cXsd2001
{
	/// <summary>Wraps an XSD <see langword="nonPositiveInteger" /> type.</summary>
	// Token: 0x020005F7 RID: 1527
	[ComVisible(true)]
	[Serializable]
	public sealed class SoapNonPositiveInteger : ISoapXsd
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Runtime.Remoting.Metadata.W3cXsd2001.SoapNonPositiveInteger" /> class.</summary>
		// Token: 0x060039DB RID: 14811 RVA: 0x0000259F File Offset: 0x0000079F
		public SoapNonPositiveInteger()
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Runtime.Remoting.Metadata.W3cXsd2001.SoapNonPositiveInteger" /> class with a <see cref="T:System.Decimal" /> value.</summary>
		/// <param name="value">A <see cref="T:System.Decimal" /> value to initialize the current instance.</param>
		/// <exception cref="T:System.Runtime.Remoting.RemotingException">
		///   <paramref name="value" /> is greater than zero.</exception>
		// Token: 0x060039DC RID: 14812 RVA: 0x000CBF2B File Offset: 0x000CA12B
		public SoapNonPositiveInteger(decimal value)
		{
			if (value > 0m)
			{
				throw SoapHelper.GetException(this, "invalid " + value.ToString());
			}
			this._value = value;
		}

		/// <summary>Gets or sets the numeric value of the current instance.</summary>
		/// <returns>A <see cref="T:System.Decimal" /> that indicates the numeric value of the current instance.</returns>
		/// <exception cref="T:System.Runtime.Remoting.RemotingException">
		///   <paramref name="value" /> is greater than zero.</exception>
		// Token: 0x17000861 RID: 2145
		// (get) Token: 0x060039DD RID: 14813 RVA: 0x000CBF5F File Offset: 0x000CA15F
		// (set) Token: 0x060039DE RID: 14814 RVA: 0x000CBF67 File Offset: 0x000CA167
		public decimal Value
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
		// Token: 0x17000862 RID: 2146
		// (get) Token: 0x060039DF RID: 14815 RVA: 0x000CBF70 File Offset: 0x000CA170
		public static string XsdType
		{
			get
			{
				return "nonPositiveInteger";
			}
		}

		/// <summary>Returns the XML Schema definition language (XSD) of the current SOAP type.</summary>
		/// <returns>A <see cref="T:System.String" /> that indicates the XSD of the current SOAP type.</returns>
		// Token: 0x060039E0 RID: 14816 RVA: 0x000CBF77 File Offset: 0x000CA177
		public string GetXsdType()
		{
			return SoapNonPositiveInteger.XsdType;
		}

		/// <summary>Converts the specified <see cref="T:System.String" /> into a <see cref="T:System.Runtime.Remoting.Metadata.W3cXsd2001.SoapNonPositiveInteger" /> object.</summary>
		/// <param name="value">The <see langword="String" /> to convert.</param>
		/// <returns>A <see cref="T:System.Runtime.Remoting.Metadata.W3cXsd2001.SoapNonPositiveInteger" /> object that is obtained from <paramref name="value" /></returns>
		// Token: 0x060039E1 RID: 14817 RVA: 0x000CBF7E File Offset: 0x000CA17E
		public static SoapNonPositiveInteger Parse(string value)
		{
			return new SoapNonPositiveInteger(decimal.Parse(value));
		}

		/// <summary>Returns <see cref="P:System.Runtime.Remoting.Metadata.W3cXsd2001.SoapNonPositiveInteger.Value" /> as a <see cref="T:System.String" />.</summary>
		/// <returns>A <see cref="T:System.String" /> that is obtained from <see langword="Value" />.</returns>
		// Token: 0x060039E2 RID: 14818 RVA: 0x000CBF8B File Offset: 0x000CA18B
		public override string ToString()
		{
			return this._value.ToString();
		}

		// Token: 0x0400263A RID: 9786
		private decimal _value;
	}
}
