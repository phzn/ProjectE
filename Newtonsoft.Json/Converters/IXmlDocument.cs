using System;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x020000F1 RID: 241
	[NullableContext(1)]
	internal interface IXmlDocument : IXmlNode
	{
		// Token: 0x06000CAA RID: 3242
		IXmlNode CreateComment([Nullable(2)] string text);

		// Token: 0x06000CAB RID: 3243
		IXmlNode CreateTextNode([Nullable(2)] string text);

		// Token: 0x06000CAC RID: 3244
		IXmlNode CreateCDataSection([Nullable(2)] string data);

		// Token: 0x06000CAD RID: 3245
		IXmlNode CreateWhitespace([Nullable(2)] string text);

		// Token: 0x06000CAE RID: 3246
		IXmlNode CreateSignificantWhitespace([Nullable(2)] string text);

		// Token: 0x06000CAF RID: 3247
		IXmlNode CreateXmlDeclaration(string version, [Nullable(2)] string encoding, [Nullable(2)] string standalone);

		// Token: 0x06000CB0 RID: 3248
		[NullableContext(2)]
		[return: Nullable(1)]
		IXmlNode CreateXmlDocumentType([Nullable(1)] string name, string publicId, string systemId, string internalSubset);

		// Token: 0x06000CB1 RID: 3249
		IXmlNode CreateProcessingInstruction(string target, string data);

		// Token: 0x06000CB2 RID: 3250
		IXmlElement CreateElement(string elementName);

		// Token: 0x06000CB3 RID: 3251
		IXmlElement CreateElement(string qualifiedName, string namespaceUri);

		// Token: 0x06000CB4 RID: 3252
		IXmlNode CreateAttribute(string name, string value);

		// Token: 0x06000CB5 RID: 3253
		IXmlNode CreateAttribute(string qualifiedName, string namespaceUri, string value);

		// Token: 0x1700022D RID: 557
		// (get) Token: 0x06000CB6 RID: 3254
		[Nullable(2)]
		IXmlElement DocumentElement
		{
			[NullableContext(2)]
			get;
		}
	}
}
