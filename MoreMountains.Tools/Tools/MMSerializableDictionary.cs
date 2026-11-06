using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x02000106 RID: 262
	[Serializable]
	public class MMSerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
	{
		// Token: 0x060005AF RID: 1455 RVA: 0x0001BE48 File Offset: 0x0001A048
		public MMSerializableDictionary()
		{
		}

		// Token: 0x060005B0 RID: 1456 RVA: 0x0001BE66 File Offset: 0x0001A066
		public MMSerializableDictionary(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		// Token: 0x060005B1 RID: 1457 RVA: 0x0001BE88 File Offset: 0x0001A088
		public void OnBeforeSerialize()
		{
			this._keys.Clear();
			this._values.Clear();
			foreach (KeyValuePair<TKey, TValue> keyValuePair in this)
			{
				this._keys.Add(keyValuePair.Key);
				this._values.Add(keyValuePair.Value);
			}
		}

		// Token: 0x060005B2 RID: 1458 RVA: 0x0001BF0C File Offset: 0x0001A10C
		public void OnAfterDeserialize()
		{
			base.Clear();
			if (this._keys.Count != this._values.Count)
			{
				Debug.LogError(string.Concat(new string[]
				{
					"MMSerializableDictionary : there are ",
					this._keys.Count.ToString(),
					" keys and ",
					this._values.Count.ToString(),
					" values after deserialization. Counts need to match, make sure both key and value types are serializable."
				}));
			}
			for (int i = 0; i < this._keys.Count; i++)
			{
				base.Add(this._keys[i], this._values[i]);
			}
		}

		// Token: 0x040006F5 RID: 1781
		[SerializeField]
		protected List<TKey> _keys = new List<TKey>();

		// Token: 0x040006F6 RID: 1782
		[SerializeField]
		protected List<TValue> _values = new List<TValue>();
	}
}
