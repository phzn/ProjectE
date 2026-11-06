using System;
using System.Collections.Generic;
using UnityEngine;

namespace DA_Assets.Tools
{
	// Token: 0x02000016 RID: 22
	[Serializable]
	public class SerializedDictionary<K, V> : Dictionary<K, V>, ISerializationCallbackReceiver
	{
		// Token: 0x0600004E RID: 78 RVA: 0x00003110 File Offset: 0x00001310
		public void OnBeforeSerialize()
		{
			this.m_Keys.Clear();
			this.m_Values.Clear();
			foreach (KeyValuePair<K, V> keyValuePair in this)
			{
				this.m_Keys.Add(keyValuePair.Key);
				this.m_Values.Add(keyValuePair.Value);
			}
		}

		// Token: 0x0600004F RID: 79 RVA: 0x00003194 File Offset: 0x00001394
		public void OnAfterDeserialize()
		{
			for (int i = 0; i < this.m_Keys.Count; i++)
			{
				try
				{
					base.Add(this.m_Keys[i], this.m_Values[i]);
				}
				catch
				{
				}
			}
			this.m_Keys.Clear();
			this.m_Values.Clear();
		}

		// Token: 0x04000030 RID: 48
		[SerializeField]
		private List<K> m_Keys = new List<K>();

		// Token: 0x04000031 RID: 49
		[SerializeField]
		private List<V> m_Values = new List<V>();
	}
}
