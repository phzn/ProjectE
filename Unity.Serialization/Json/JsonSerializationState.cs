using System;
using System.Collections.Generic;

namespace Unity.Serialization.Json
{
	// Token: 0x0200002F RID: 47
	internal class JsonSerializationState
	{
		// Token: 0x17000012 RID: 18
		// (get) Token: 0x060000D0 RID: 208 RVA: 0x00004008 File Offset: 0x00002208
		internal bool IsLocked
		{
			get
			{
				return (this.m_JsonPropertyWriter != null && this.m_JsonPropertyWriter.IsLocked) || (this.m_JsonPropertyReader != null && this.m_JsonPropertyReader.IsLocked);
			}
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x00004036 File Offset: 0x00002236
		internal JsonPropertyWriter GetJsonPropertyWriter()
		{
			if (this.m_JsonPropertyWriter == null)
			{
				this.m_JsonPropertyWriter = new JsonPropertyWriter();
				return this.m_JsonPropertyWriter;
			}
			if (!this.m_JsonPropertyWriter.IsLocked)
			{
				return this.m_JsonPropertyWriter;
			}
			return new JsonPropertyWriter();
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x0000406B File Offset: 0x0000226B
		internal JsonPropertyReader GetJsonPropertyReader()
		{
			if (this.m_JsonPropertyReader == null)
			{
				this.m_JsonPropertyReader = new JsonPropertyReader();
				return this.m_JsonPropertyReader;
			}
			if (!this.m_JsonPropertyReader.IsLocked)
			{
				return this.m_JsonPropertyReader;
			}
			return new JsonPropertyReader();
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x000040A0 File Offset: 0x000022A0
		internal SerializedReferences GetSerializedReferences()
		{
			SerializedReferences serializedReferences;
			if ((serializedReferences = this.m_SerializedReferences) == null)
			{
				serializedReferences = (this.m_SerializedReferences = new SerializedReferences());
			}
			return serializedReferences;
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x000040C8 File Offset: 0x000022C8
		internal SerializedReferenceVisitor GetSerializedReferenceVisitor()
		{
			SerializedReferenceVisitor serializedReferenceVisitor;
			if ((serializedReferenceVisitor = this.m_SerializedReferenceVisitor) == null)
			{
				serializedReferenceVisitor = (this.m_SerializedReferenceVisitor = new SerializedReferenceVisitor());
			}
			return serializedReferenceVisitor;
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x000040F0 File Offset: 0x000022F0
		internal List<DeserializationEvent> GetDeserializationEvents()
		{
			List<DeserializationEvent> list;
			if ((list = this.m_DeserializationEvents) == null)
			{
				list = (this.m_DeserializationEvents = new List<DeserializationEvent>());
			}
			return list;
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x00004115 File Offset: 0x00002315
		internal void ClearSerializedReferences()
		{
			SerializedReferences serializedReferences = this.m_SerializedReferences;
			if (serializedReferences == null)
			{
				return;
			}
			serializedReferences.Clear();
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x00004127 File Offset: 0x00002327
		internal void ClearDeserializationEvents()
		{
			List<DeserializationEvent> deserializationEvents = this.m_DeserializationEvents;
			if (deserializationEvents == null)
			{
				return;
			}
			deserializationEvents.Clear();
		}

		// Token: 0x0400004C RID: 76
		private JsonPropertyWriter m_JsonPropertyWriter;

		// Token: 0x0400004D RID: 77
		private JsonPropertyReader m_JsonPropertyReader;

		// Token: 0x0400004E RID: 78
		private SerializedReferences m_SerializedReferences;

		// Token: 0x0400004F RID: 79
		private SerializedReferenceVisitor m_SerializedReferenceVisitor;

		// Token: 0x04000050 RID: 80
		private List<DeserializationEvent> m_DeserializationEvents;
	}
}
