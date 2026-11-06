using System;
using System.Collections.Generic;
using System.Linq;

namespace UnityEngine.Localization.Metadata
{
	// Token: 0x020000E5 RID: 229
	[Serializable]
	public abstract class SharedTableCollectionMetadata : IMetadata, ISerializationCallbackReceiver
	{
		// Token: 0x1700015B RID: 347
		// (get) Token: 0x0600061E RID: 1566 RVA: 0x000151AE File Offset: 0x000133AE
		// (set) Token: 0x0600061F RID: 1567 RVA: 0x000151B6 File Offset: 0x000133B6
		public Dictionary<long, HashSet<string>> EntriesLookup { get; set; } = new Dictionary<long, HashSet<string>>();

		// Token: 0x1700015C RID: 348
		// (get) Token: 0x06000620 RID: 1568 RVA: 0x000151BF File Offset: 0x000133BF
		public bool IsEmpty
		{
			get
			{
				return this.EntriesLookup.Count == 0;
			}
		}

		// Token: 0x06000621 RID: 1569 RVA: 0x000151CF File Offset: 0x000133CF
		public bool Contains(long keyId)
		{
			return this.EntriesLookup.ContainsKey(keyId);
		}

		// Token: 0x06000622 RID: 1570 RVA: 0x000151E0 File Offset: 0x000133E0
		public bool Contains(long keyId, string code)
		{
			HashSet<string> hashSet;
			return this.EntriesLookup.TryGetValue(keyId, out hashSet) && hashSet.Contains(code);
		}

		// Token: 0x06000623 RID: 1571 RVA: 0x00015208 File Offset: 0x00013408
		public void AddEntry(long keyId, string code)
		{
			HashSet<string> hashSet;
			this.EntriesLookup.TryGetValue(keyId, out hashSet);
			if (hashSet == null)
			{
				hashSet = new HashSet<string>();
				this.EntriesLookup[keyId] = hashSet;
			}
			hashSet.Add(code);
		}

		// Token: 0x06000624 RID: 1572 RVA: 0x00015244 File Offset: 0x00013444
		public void RemoveEntry(long keyId, string code)
		{
			HashSet<string> hashSet;
			if (this.EntriesLookup.TryGetValue(keyId, out hashSet))
			{
				hashSet.Remove(code);
				if (hashSet.Count == 0)
				{
					this.EntriesLookup.Remove(keyId);
				}
			}
		}

		// Token: 0x06000625 RID: 1573 RVA: 0x00015280 File Offset: 0x00013480
		public virtual void OnBeforeSerialize()
		{
			this.m_Entries.Clear();
			foreach (KeyValuePair<long, HashSet<string>> keyValuePair in this.EntriesLookup)
			{
				this.m_Entries.Add(new SharedTableCollectionMetadata.Item
				{
					KeyId = keyValuePair.Key,
					Tables = keyValuePair.Value.ToList<string>()
				});
			}
		}

		// Token: 0x06000626 RID: 1574 RVA: 0x00015308 File Offset: 0x00013508
		public virtual void OnAfterDeserialize()
		{
			this.EntriesLookup = new Dictionary<long, HashSet<string>>();
			foreach (SharedTableCollectionMetadata.Item item in this.m_Entries)
			{
				this.EntriesLookup[item.KeyId] = new HashSet<string>(item.Tables);
			}
		}

		// Token: 0x0400021B RID: 539
		[SerializeField]
		[HideInInspector]
		private List<SharedTableCollectionMetadata.Item> m_Entries = new List<SharedTableCollectionMetadata.Item>();

		// Token: 0x02000160 RID: 352
		[Serializable]
		private class Item
		{
			// Token: 0x17000194 RID: 404
			// (get) Token: 0x06000814 RID: 2068 RVA: 0x0001978D File Offset: 0x0001798D
			// (set) Token: 0x06000815 RID: 2069 RVA: 0x00019795 File Offset: 0x00017995
			public long KeyId
			{
				get
				{
					return this.m_KeyId;
				}
				set
				{
					this.m_KeyId = value;
				}
			}

			// Token: 0x17000195 RID: 405
			// (get) Token: 0x06000816 RID: 2070 RVA: 0x0001979E File Offset: 0x0001799E
			// (set) Token: 0x06000817 RID: 2071 RVA: 0x000197A6 File Offset: 0x000179A6
			public List<string> Tables
			{
				get
				{
					return this.m_TableCodes;
				}
				set
				{
					this.m_TableCodes = value;
				}
			}

			// Token: 0x04000319 RID: 793
			[SerializeField]
			private long m_KeyId;

			// Token: 0x0400031A RID: 794
			[SerializeField]
			private List<string> m_TableCodes = new List<string>();
		}
	}
}
