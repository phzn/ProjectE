using System;

namespace System.Xml
{
	// Token: 0x0200000B RID: 11
	internal class Int32ArrayHelperWithDictionaryString : ArrayHelper<XmlDictionaryString, int>
	{
		// Token: 0x06000026 RID: 38 RVA: 0x00002345 File Offset: 0x00000545
		protected override int ReadArray(XmlDictionaryReader reader, XmlDictionaryString localName, XmlDictionaryString namespaceUri, int[] array, int offset, int count)
		{
			return reader.ReadArray(localName, namespaceUri, array, offset, count);
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00002355 File Offset: 0x00000555
		protected override void WriteArray(XmlDictionaryWriter writer, string prefix, XmlDictionaryString localName, XmlDictionaryString namespaceUri, int[] array, int offset, int count)
		{
			writer.WriteArray(prefix, localName, namespaceUri, array, offset, count);
		}

		// Token: 0x04000008 RID: 8
		public static readonly Int32ArrayHelperWithDictionaryString Instance = new Int32ArrayHelperWithDictionaryString();
	}
}
