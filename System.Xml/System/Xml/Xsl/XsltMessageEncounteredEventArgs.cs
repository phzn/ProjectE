using System;

namespace System.Xml.Xsl
{
	/// <summary>Provides data for the <see cref="E:System.Xml.Xsl.XsltArgumentList.XsltMessageEncountered" /> event.</summary>
	// Token: 0x02000346 RID: 838
	public abstract class XsltMessageEncounteredEventArgs : EventArgs
	{
		/// <summary>Gets the contents of the xsl:message element.</summary>
		/// <returns>The contents of the xsl:message element.</returns>
		// Token: 0x170006E1 RID: 1761
		// (get) Token: 0x060022B8 RID: 8888
		public abstract string Message { get; }
	}
}
