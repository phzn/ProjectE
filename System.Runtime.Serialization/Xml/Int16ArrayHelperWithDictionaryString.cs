using System;

namespace System.Xml
{
	// Token: 0x02000009 RID: 9
	internal class Int16ArrayHelperWithDictionaryString : ArrayHelper<XmlDictionaryString, short>
	{
		// Token: 0x0600001E RID: 30 RVA: 0x000022D9 File Offset: 0x000004D9
		protected override int ReadArray(XmlDictionaryReader reader, XmlDictionaryString localName, XmlDictionaryString namespaceUri, short[] array, int offset, int count)
		{
			return reader.ReadArray(localName, namespaceUri, array, offset, count);
		}

		// Token: 0x0600001F RID: 31 RVA: 0x000022E9 File Offset: 0x000004E9
		protected override void WriteArray(XmlDictionaryWriter writer, string prefix, XmlDictionaryString localName, XmlDictionaryString namespaceUri, short[] array, int offset, int count)
		{
			writer.WriteArray(prefix, localName, namespaceUri, array, offset, count);
		}

		// Token: 0x04000006 RID: 6
		public static readonly Int16ArrayHelperWithDictionaryString Instance = new Int16ArrayHelperWithDictionaryString();
	}
}
