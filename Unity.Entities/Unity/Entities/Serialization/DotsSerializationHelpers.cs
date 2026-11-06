using System;

namespace Unity.Entities.Serialization
{
	// Token: 0x0200037E RID: 894
	internal static class DotsSerializationHelpers
	{
		// Token: 0x060017CA RID: 6090 RVA: 0x00059519 File Offset: 0x00057719
		public static StringTableWriterHandle CreateStringTableNode(this DotsSerializationWriter writer, Hash128 id)
		{
			return new StringTableWriterHandle(writer.CreateNode<DotsSerialization.StringTableNode>(id));
		}

		// Token: 0x060017CB RID: 6091 RVA: 0x00059527 File Offset: 0x00057727
		public static StringTableReaderHandle OpenStringTableNode(this DotsSerializationReader reader, DotsSerializationReader.NodeHandle stringTableNode)
		{
			return new StringTableReaderHandle(stringTableNode);
		}
	}
}
