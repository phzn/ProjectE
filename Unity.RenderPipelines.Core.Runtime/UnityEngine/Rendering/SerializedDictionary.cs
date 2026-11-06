using System;

namespace UnityEngine.Rendering
{
	// Token: 0x02000057 RID: 87
	[Serializable]
	public class SerializedDictionary<K, V> : SerializedDictionary<K, V, K, V>
	{
		// Token: 0x060002CB RID: 715 RVA: 0x0000C93C File Offset: 0x0000AB3C
		public override K SerializeKey(K key)
		{
			return key;
		}

		// Token: 0x060002CC RID: 716 RVA: 0x0000C93F File Offset: 0x0000AB3F
		public override V SerializeValue(V val)
		{
			return val;
		}

		// Token: 0x060002CD RID: 717 RVA: 0x0000C942 File Offset: 0x0000AB42
		public override K DeserializeKey(K key)
		{
			return key;
		}

		// Token: 0x060002CE RID: 718 RVA: 0x0000C945 File Offset: 0x0000AB45
		public override V DeserializeValue(V val)
		{
			return val;
		}
	}
}
