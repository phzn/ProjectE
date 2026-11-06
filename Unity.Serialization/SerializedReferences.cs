using System;
using System.Collections.Generic;
using Unity.Properties;

namespace Unity.Serialization
{
	// Token: 0x02000015 RID: 21
	internal class SerializedReferences
	{
		// Token: 0x0600002B RID: 43 RVA: 0x0000264C File Offset: 0x0000084C
		internal int AddSerializedReference(object value)
		{
			int count;
			if (!this.m_SerializationIndex.TryGetValue(value, out count))
			{
				count = this.m_SerializationIndex.Count;
				this.m_SerializationIndex.Add(value, count);
			}
			return count;
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00002683 File Offset: 0x00000883
		internal bool TryGetSerializedReference(object value, out int id)
		{
			return this.m_SerializationIndex.TryGetValue(value, out id);
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00002692 File Offset: 0x00000892
		internal void AddDeserializedReference<T>(int id, T value)
		{
			if (!TypeTraits<T>.IsValueType)
			{
				this.m_DeserializationIndex[id] = value;
			}
		}

		// Token: 0x0600002E RID: 46 RVA: 0x000026AD File Offset: 0x000008AD
		internal void AddDeserializedReference<T>(T value)
		{
			if (!TypeTraits<T>.IsValueType)
			{
				this.m_DeserializationIndex[this.m_DeserializationIndex.Count] = value;
			}
		}

		// Token: 0x0600002F RID: 47 RVA: 0x000026D4 File Offset: 0x000008D4
		internal object GetDeserializedReference(int id)
		{
			object obj;
			this.m_DeserializationIndex.TryGetValue(id, out obj);
			return obj;
		}

		// Token: 0x06000030 RID: 48 RVA: 0x000026F1 File Offset: 0x000008F1
		internal bool SetVisited(object value)
		{
			return this.m_References.Add(value);
		}

		// Token: 0x06000031 RID: 49 RVA: 0x000026FF File Offset: 0x000008FF
		internal bool SetSerialized(object value)
		{
			return this.m_Serialized.Add(value);
		}

		// Token: 0x06000032 RID: 50 RVA: 0x0000270D File Offset: 0x0000090D
		internal void Clear()
		{
			this.m_References.Clear();
			this.m_Serialized.Clear();
			this.m_SerializationIndex.Clear();
			this.m_DeserializationIndex.Clear();
		}

		// Token: 0x04000010 RID: 16
		private readonly Dictionary<int, object> m_DeserializationIndex = new Dictionary<int, object>();

		// Token: 0x04000011 RID: 17
		private readonly Dictionary<object, int> m_SerializationIndex = new Dictionary<object, int>();

		// Token: 0x04000012 RID: 18
		private readonly HashSet<object> m_References = new HashSet<object>();

		// Token: 0x04000013 RID: 19
		private readonly HashSet<object> m_Serialized = new HashSet<object>();
	}
}
