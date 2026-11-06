using System;
using System.Reflection;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Metadata
{
	/// <summary>Customizes SOAP generation and processing for a method. This class cannot be inherited.</summary>
	// Token: 0x020005DA RID: 1498
	[AttributeUsage(AttributeTargets.Method)]
	[ComVisible(true)]
	public sealed class SoapMethodAttribute : SoapAttribute
	{
		/// <summary>Gets or sets the XML element name to use for the method response to the target method.</summary>
		/// <returns>The XML element name to use for the method response to the target method.</returns>
		// Token: 0x17000827 RID: 2087
		// (get) Token: 0x06003904 RID: 14596 RVA: 0x000CB03C File Offset: 0x000C923C
		// (set) Token: 0x06003905 RID: 14597 RVA: 0x000CB044 File Offset: 0x000C9244
		public string ResponseXmlElementName
		{
			get
			{
				return this._responseElement;
			}
			set
			{
				this._responseElement = value;
			}
		}

		/// <summary>Gets or sets the XML element namesapce used for method response to the target method.</summary>
		/// <returns>The XML element namesapce used for method response to the target method.</returns>
		// Token: 0x17000828 RID: 2088
		// (get) Token: 0x06003906 RID: 14598 RVA: 0x000CB04D File Offset: 0x000C924D
		// (set) Token: 0x06003907 RID: 14599 RVA: 0x000CB055 File Offset: 0x000C9255
		public string ResponseXmlNamespace
		{
			get
			{
				return this._responseNamespace;
			}
			set
			{
				this._responseNamespace = value;
			}
		}

		/// <summary>Gets or sets the XML element name used for the return value from the target method.</summary>
		/// <returns>The XML element name used for the return value from the target method.</returns>
		// Token: 0x17000829 RID: 2089
		// (get) Token: 0x06003908 RID: 14600 RVA: 0x000CB05E File Offset: 0x000C925E
		// (set) Token: 0x06003909 RID: 14601 RVA: 0x000CB066 File Offset: 0x000C9266
		public string ReturnXmlElementName
		{
			get
			{
				return this._returnElement;
			}
			set
			{
				this._returnElement = value;
			}
		}

		/// <summary>Gets or sets the SOAPAction header field used with HTTP requests sent with this method. This property is currently not implemented.</summary>
		/// <returns>The SOAPAction header field used with HTTP requests sent with this method.</returns>
		// Token: 0x1700082A RID: 2090
		// (get) Token: 0x0600390A RID: 14602 RVA: 0x000CB06F File Offset: 0x000C926F
		// (set) Token: 0x0600390B RID: 14603 RVA: 0x000CB077 File Offset: 0x000C9277
		public string SoapAction
		{
			get
			{
				return this._soapAction;
			}
			set
			{
				this._soapAction = value;
			}
		}

		/// <summary>Gets or sets a value indicating whether the target of the current attribute will be serialized as an XML attribute instead of an XML field.</summary>
		/// <returns>The current implementation always returns <see langword="false" />.</returns>
		/// <exception cref="T:System.Runtime.Remoting.RemotingException">An attempt was made to set the current property.</exception>
		// Token: 0x1700082B RID: 2091
		// (get) Token: 0x0600390C RID: 14604 RVA: 0x000CB080 File Offset: 0x000C9280
		// (set) Token: 0x0600390D RID: 14605 RVA: 0x000CB088 File Offset: 0x000C9288
		public override bool UseAttribute
		{
			get
			{
				return this._useAttribute;
			}
			set
			{
				this._useAttribute = value;
			}
		}

		/// <summary>Gets or sets the XML namespace that is used during serialization of remote method calls of the target method.</summary>
		/// <returns>The XML namespace that is used during serialization of remote method calls of the target method.</returns>
		// Token: 0x1700082C RID: 2092
		// (get) Token: 0x0600390E RID: 14606 RVA: 0x000CB091 File Offset: 0x000C9291
		// (set) Token: 0x0600390F RID: 14607 RVA: 0x000CB099 File Offset: 0x000C9299
		public override string XmlNamespace
		{
			get
			{
				return this._namespace;
			}
			set
			{
				this._namespace = value;
			}
		}

		// Token: 0x06003910 RID: 14608 RVA: 0x000CB0A4 File Offset: 0x000C92A4
		internal override void SetReflectionObject(object reflectionObject)
		{
			MethodBase methodBase = (MethodBase)reflectionObject;
			if (this._responseElement == null)
			{
				this._responseElement = methodBase.Name + "Response";
			}
			if (this._responseNamespace == null)
			{
				this._responseNamespace = SoapServices.GetXmlNamespaceForMethodResponse(methodBase);
			}
			if (this._returnElement == null)
			{
				this._returnElement = "return";
			}
			if (this._soapAction == null)
			{
				this._soapAction = SoapServices.GetXmlNamespaceForMethodCall(methodBase) + "#" + methodBase.Name;
			}
			if (this._namespace == null)
			{
				this._namespace = SoapServices.GetXmlNamespaceForMethodCall(methodBase);
			}
		}

		// Token: 0x04002605 RID: 9733
		private string _responseElement;

		// Token: 0x04002606 RID: 9734
		private string _responseNamespace;

		// Token: 0x04002607 RID: 9735
		private string _returnElement;

		// Token: 0x04002608 RID: 9736
		private string _soapAction;

		// Token: 0x04002609 RID: 9737
		private bool _useAttribute;

		// Token: 0x0400260A RID: 9738
		private string _namespace;
	}
}
