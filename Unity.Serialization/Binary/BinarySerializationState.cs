using System;

namespace Unity.Serialization.Binary
{
	// Token: 0x020000A3 RID: 163
	public class BinarySerializationState
	{
		// Token: 0x17000075 RID: 117
		// (get) Token: 0x0600040D RID: 1037 RVA: 0x000108C2 File Offset: 0x0000EAC2
		internal bool IsLocked
		{
			get
			{
				return (this.m_BinaryPropertyWriter != null && this.m_BinaryPropertyWriter.IsLocked) || (this.m_BinaryPropertyReader != null && this.m_BinaryPropertyReader.IsLocked);
			}
		}

		// Token: 0x0600040E RID: 1038 RVA: 0x000108F0 File Offset: 0x0000EAF0
		internal BinaryPropertyWriter GetBinaryPropertyWriter()
		{
			if (this.m_BinaryPropertyWriter == null)
			{
				this.m_BinaryPropertyWriter = new BinaryPropertyWriter();
				return this.m_BinaryPropertyWriter;
			}
			if (!this.m_BinaryPropertyWriter.IsLocked)
			{
				return this.m_BinaryPropertyWriter;
			}
			return new BinaryPropertyWriter();
		}

		// Token: 0x0600040F RID: 1039 RVA: 0x00010925 File Offset: 0x0000EB25
		internal BinaryPropertyReader GetBinaryPropertyReader()
		{
			if (this.m_BinaryPropertyReader == null)
			{
				this.m_BinaryPropertyReader = new BinaryPropertyReader();
				return this.m_BinaryPropertyReader;
			}
			if (!this.m_BinaryPropertyReader.IsLocked)
			{
				return this.m_BinaryPropertyReader;
			}
			return new BinaryPropertyReader();
		}

		// Token: 0x06000410 RID: 1040 RVA: 0x0001095C File Offset: 0x0000EB5C
		internal SerializedReferences GetSerializedReferences()
		{
			SerializedReferences serializedReferences;
			if ((serializedReferences = this.m_SerializedReferences) == null)
			{
				serializedReferences = (this.m_SerializedReferences = new SerializedReferences());
			}
			return serializedReferences;
		}

		// Token: 0x06000411 RID: 1041 RVA: 0x00010981 File Offset: 0x0000EB81
		internal void ClearSerializedReferences()
		{
			SerializedReferences serializedReferences = this.m_SerializedReferences;
			if (serializedReferences == null)
			{
				return;
			}
			serializedReferences.Clear();
		}

		// Token: 0x040001F1 RID: 497
		private BinaryPropertyWriter m_BinaryPropertyWriter;

		// Token: 0x040001F2 RID: 498
		private BinaryPropertyReader m_BinaryPropertyReader;

		// Token: 0x040001F3 RID: 499
		private SerializedReferences m_SerializedReferences;
	}
}
