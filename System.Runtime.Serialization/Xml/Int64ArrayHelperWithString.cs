using System;

namespace System.Xml
{
	// Token: 0x0200000C RID: 12
	internal class Int64ArrayHelperWithString : ArrayHelper<string, long>
	{
		// Token: 0x0600002A RID: 42 RVA: 0x0000237B File Offset: 0x0000057B
		protected override int ReadArray(XmlDictionaryReader reader, string localName, string namespaceUri, long[] array, int offset, int count)
		{
			return reader.ReadArray(localName, namespaceUri, array, offset, count);
		}

		// Token: 0x0600002B RID: 43 RVA: 0x0000238B File Offset: 0x0000058B
		protected override void WriteArray(XmlDictionaryWriter writer, string prefix, string localName, string namespaceUri, long[] array, int offset, int count)
		{
			writer.WriteArray(prefix, localName, namespaceUri, array, offset, count);
		}

		// Token: 0x04000009 RID: 9
		public static readonly Int64ArrayHelperWithString Instance = new Int64ArrayHelperWithString();
	}
}
