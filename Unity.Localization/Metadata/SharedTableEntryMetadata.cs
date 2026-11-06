using System;
using System.Collections.Generic;
using UnityEngine.Localization.Tables;

namespace UnityEngine.Localization.Metadata
{
	// Token: 0x020000E6 RID: 230
	[Serializable]
	public abstract class SharedTableEntryMetadata : IMetadata, ISerializationCallbackReceiver
	{
		// Token: 0x1700015D RID: 349
		// (get) Token: 0x06000628 RID: 1576 RVA: 0x0001539A File Offset: 0x0001359A
		internal int Count
		{
			get
			{
				return this.m_EntriesLookup.Count;
			}
		}

		// Token: 0x06000629 RID: 1577 RVA: 0x000153A7 File Offset: 0x000135A7
		internal bool IsRegistered(TableEntry entry)
		{
			return this.m_EntriesLookup.Contains(entry.Data.Id);
		}

		// Token: 0x0600062A RID: 1578 RVA: 0x000153BF File Offset: 0x000135BF
		internal void Register(TableEntry entry)
		{
			this.m_EntriesLookup.Add(entry.Data.Id);
		}

		// Token: 0x0600062B RID: 1579 RVA: 0x000153D8 File Offset: 0x000135D8
		internal void Unregister(TableEntry entry)
		{
			this.m_EntriesLookup.Remove(entry.Data.Id);
		}

		// Token: 0x0600062C RID: 1580 RVA: 0x000153F4 File Offset: 0x000135F4
		public void OnBeforeSerialize()
		{
			this.m_Entries = null;
			this.m_SharedEntries.Clear();
			foreach (long num in this.m_EntriesLookup)
			{
				this.m_SharedEntries.Add(new SharedTableEntryMetadata.Entry
				{
					id = num
				});
			}
		}

		// Token: 0x0600062D RID: 1581 RVA: 0x00015470 File Offset: 0x00013670
		public void OnAfterDeserialize()
		{
			if (this.m_EntriesLookup == null)
			{
				this.m_EntriesLookup = new HashSet<long>();
			}
			else
			{
				this.m_EntriesLookup.Clear();
			}
			if (this.m_Entries != null && this.m_Entries.Count > 0)
			{
				foreach (long num in this.m_Entries)
				{
					this.m_EntriesLookup.Add(num);
				}
				this.m_Entries = null;
			}
			if (this.m_SharedEntries != null && this.m_SharedEntries.Count > 0)
			{
				foreach (SharedTableEntryMetadata.Entry entry in this.m_SharedEntries)
				{
					this.m_EntriesLookup.Add(entry.id);
				}
			}
		}

		// Token: 0x0400021D RID: 541
		[SerializeField]
		private List<long> m_Entries;

		// Token: 0x0400021E RID: 542
		[SerializeField]
		private List<SharedTableEntryMetadata.Entry> m_SharedEntries = new List<SharedTableEntryMetadata.Entry>();

		// Token: 0x0400021F RID: 543
		private HashSet<long> m_EntriesLookup = new HashSet<long>();

		// Token: 0x02000161 RID: 353
		[Serializable]
		private struct Entry
		{
			// Token: 0x0400031B RID: 795
			public long id;
		}
	}
}
