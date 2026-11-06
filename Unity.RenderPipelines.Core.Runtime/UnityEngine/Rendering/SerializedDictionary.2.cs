using System;
using System.Collections.Generic;

namespace UnityEngine.Rendering
{
	// Token: 0x02000058 RID: 88
	[Serializable]
	public abstract class SerializedDictionary<K, V, SK, SV> : Dictionary<K, V>, ISerializationCallbackReceiver
	{
		// Token: 0x060002D0 RID: 720
		public abstract SK SerializeKey(K key);

		// Token: 0x060002D1 RID: 721
		public abstract SV SerializeValue(V value);

		// Token: 0x060002D2 RID: 722
		public abstract K DeserializeKey(SK serializedKey);

		// Token: 0x060002D3 RID: 723
		public abstract V DeserializeValue(SV serializedValue);

		// Token: 0x060002D4 RID: 724 RVA: 0x0000C950 File Offset: 0x0000AB50
		public void OnBeforeSerialize()
		{
			this.m_Keys.Clear();
			this.m_Values.Clear();
			foreach (KeyValuePair<K, V> keyValuePair in this)
			{
				this.m_Keys.Add(this.SerializeKey(keyValuePair.Key));
				this.m_Values.Add(this.SerializeValue(keyValuePair.Value));
			}
		}

		// Token: 0x060002D5 RID: 725 RVA: 0x0000C9E0 File Offset: 0x0000ABE0
		public void OnAfterDeserialize()
		{
			for (int i = 0; i < this.m_Keys.Count; i++)
			{
				base.Add(this.DeserializeKey(this.m_Keys[i]), this.DeserializeValue(this.m_Values[i]));
			}
			this.m_Keys.Clear();
			this.m_Values.Clear();
		}

		// Token: 0x040001A1 RID: 417
		[SerializeField]
		private List<SK> m_Keys = new List<SK>();

		// Token: 0x040001A2 RID: 418
		[SerializeField]
		private List<SV> m_Values = new List<SV>();
	}
}
