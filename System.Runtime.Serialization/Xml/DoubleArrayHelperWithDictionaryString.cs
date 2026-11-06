using System;

namespace System.Xml
{
	// Token: 0x02000011 RID: 17
	internal class DoubleArrayHelperWithDictionaryString : ArrayHelper<XmlDictionaryString, double>
	{
		// Token: 0x0600003E RID: 62 RVA: 0x00002489 File Offset: 0x00000689
		protected override int ReadArray(XmlDictionaryReader reader, XmlDictionaryString localName, XmlDictionaryString namespaceUri, double[] array, int offset, int count)
		{
			return reader.ReadArray(localName, namespaceUri, array, offset, count);
		}

		// Token: 0x0600003F RID: 63 RVA: 0x00002499 File Offset: 0x00000699
		protected override void WriteArray(XmlDictionaryWriter writer, string prefix, XmlDictionaryString localName, XmlDictionaryString namespaceUri, double[] array, int offset, int count)
		{
			writer.WriteArray(prefix, localName, namespaceUri, array, offset, count);
		}

		// Token: 0x0400000E RID: 14
		public static readonly DoubleArrayHelperWithDictionaryString Instance = new DoubleArrayHelperWithDictionaryString();
	}
}
