using System;
using System.Collections.Generic;
using UnityEngine.Localization.Metadata;
using UnityEngine.Serialization;

namespace UnityEngine.Localization.Tables
{
	// Token: 0x02000030 RID: 48
	public class SharedTableData : ScriptableObject, ISerializationCallbackReceiver
	{
		// Token: 0x17000051 RID: 81
		// (get) Token: 0x0600018D RID: 397 RVA: 0x00005F82 File Offset: 0x00004182
		// (set) Token: 0x0600018E RID: 398 RVA: 0x00005F8A File Offset: 0x0000418A
		public List<SharedTableData.SharedTableEntry> Entries
		{
			get
			{
				return this.m_Entries;
			}
			set
			{
				this.m_Entries = value;
				this.m_IdDictionary.Clear();
				this.m_KeyDictionary.Clear();
			}
		}

		// Token: 0x0600018F RID: 399 RVA: 0x00005FA9 File Offset: 0x000041A9
		public void Clear()
		{
			this.m_Entries.Clear();
			this.m_IdDictionary.Clear();
			this.m_KeyDictionary.Clear();
		}

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x06000190 RID: 400 RVA: 0x00005FCC File Offset: 0x000041CC
		// (set) Token: 0x06000191 RID: 401 RVA: 0x00005FD4 File Offset: 0x000041D4
		public string TableCollectionName
		{
			get
			{
				return this.m_TableCollectionName;
			}
			set
			{
				this.m_TableCollectionName = value;
			}
		}

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x06000192 RID: 402 RVA: 0x00005FDD File Offset: 0x000041DD
		// (set) Token: 0x06000193 RID: 403 RVA: 0x00005FE5 File Offset: 0x000041E5
		public Guid TableCollectionNameGuid
		{
			get
			{
				return this.m_TableCollectionNameGuid;
			}
			internal set
			{
				this.m_TableCollectionNameGuid = value;
			}
		}

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x06000194 RID: 404 RVA: 0x00005FEE File Offset: 0x000041EE
		// (set) Token: 0x06000195 RID: 405 RVA: 0x00005FF6 File Offset: 0x000041F6
		public MetadataCollection Metadata
		{
			get
			{
				return this.m_Metadata;
			}
			set
			{
				this.m_Metadata = value;
			}
		}

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x06000196 RID: 406 RVA: 0x00005FFF File Offset: 0x000041FF
		// (set) Token: 0x06000197 RID: 407 RVA: 0x00006007 File Offset: 0x00004207
		public IKeyGenerator KeyGenerator
		{
			get
			{
				return this.m_KeyGenerator;
			}
			set
			{
				this.m_KeyGenerator = value;
			}
		}

		// Token: 0x06000198 RID: 408 RVA: 0x00006010 File Offset: 0x00004210
		public string GetKey(long id)
		{
			SharedTableData.SharedTableEntry sharedTableEntry = this.FindWithId(id);
			if (sharedTableEntry == null)
			{
				return null;
			}
			return sharedTableEntry.Key;
		}

		// Token: 0x06000199 RID: 409 RVA: 0x00006024 File Offset: 0x00004224
		public long GetId(string key)
		{
			SharedTableData.SharedTableEntry sharedTableEntry = this.FindWithKey(key);
			if (sharedTableEntry == null)
			{
				return 0L;
			}
			return sharedTableEntry.Id;
		}

		// Token: 0x0600019A RID: 410 RVA: 0x0000603C File Offset: 0x0000423C
		public long GetId(string key, bool addNewKey)
		{
			SharedTableData.SharedTableEntry sharedTableEntry = this.FindWithKey(key);
			long num = 0L;
			if (sharedTableEntry != null)
			{
				num = sharedTableEntry.Id;
			}
			else if (addNewKey)
			{
				num = this.AddKeyInternal(key).Id;
			}
			return num;
		}

		// Token: 0x0600019B RID: 411 RVA: 0x00006071 File Offset: 0x00004271
		public SharedTableData.SharedTableEntry GetEntryFromReference(TableEntryReference tableEntryReference)
		{
			if (tableEntryReference.ReferenceType == TableEntryReference.Type.Name)
			{
				return this.GetEntry(tableEntryReference.Key);
			}
			return this.GetEntry(tableEntryReference.KeyId);
		}

		// Token: 0x0600019C RID: 412 RVA: 0x00006098 File Offset: 0x00004298
		public SharedTableData.SharedTableEntry GetEntry(long id)
		{
			return this.FindWithId(id);
		}

		// Token: 0x0600019D RID: 413 RVA: 0x000060A1 File Offset: 0x000042A1
		public SharedTableData.SharedTableEntry GetEntry(string key)
		{
			return this.FindWithKey(key);
		}

		// Token: 0x0600019E RID: 414 RVA: 0x000060AA File Offset: 0x000042AA
		public bool Contains(long id)
		{
			return this.FindWithId(id) != null;
		}

		// Token: 0x0600019F RID: 415 RVA: 0x000060B6 File Offset: 0x000042B6
		public bool Contains(string key)
		{
			return this.FindWithKey(key) != null;
		}

		// Token: 0x060001A0 RID: 416 RVA: 0x000060C2 File Offset: 0x000042C2
		public SharedTableData.SharedTableEntry AddKey(string key, long id)
		{
			if (this.Contains(id))
			{
				return null;
			}
			return this.AddKeyInternal(key, id);
		}

		// Token: 0x060001A1 RID: 417 RVA: 0x000060D8 File Offset: 0x000042D8
		public SharedTableData.SharedTableEntry AddKey(string key = null)
		{
			string text = (string.IsNullOrEmpty(key) ? "New Entry" : key);
			SharedTableData.SharedTableEntry sharedTableEntry = null;
			int num = 1;
			string text2 = text;
			while (sharedTableEntry == null)
			{
				if (this.Contains(text2))
				{
					text2 = string.Format("{0} {1}", text, num++);
				}
				else
				{
					sharedTableEntry = this.AddKeyInternal(text2);
				}
			}
			return sharedTableEntry;
		}

		// Token: 0x060001A2 RID: 418 RVA: 0x0000612C File Offset: 0x0000432C
		public void RemoveKey(long id)
		{
			SharedTableData.SharedTableEntry sharedTableEntry = this.FindWithId(id);
			if (sharedTableEntry != null)
			{
				this.RemoveKeyInternal(sharedTableEntry);
			}
		}

		// Token: 0x060001A3 RID: 419 RVA: 0x0000614C File Offset: 0x0000434C
		public void RemoveKey(string key)
		{
			SharedTableData.SharedTableEntry sharedTableEntry = this.FindWithKey(key);
			if (sharedTableEntry != null)
			{
				this.RemoveKeyInternal(sharedTableEntry);
			}
		}

		// Token: 0x060001A4 RID: 420 RVA: 0x0000616C File Offset: 0x0000436C
		public void RenameKey(long id, string newValue)
		{
			SharedTableData.SharedTableEntry sharedTableEntry = this.FindWithId(id);
			if (sharedTableEntry != null)
			{
				this.RenameKeyInternal(sharedTableEntry, newValue);
			}
		}

		// Token: 0x060001A5 RID: 421 RVA: 0x0000618C File Offset: 0x0000438C
		public void RenameKey(string oldValue, string newValue)
		{
			SharedTableData.SharedTableEntry sharedTableEntry = this.FindWithKey(oldValue);
			if (sharedTableEntry != null)
			{
				this.RenameKeyInternal(sharedTableEntry, newValue);
			}
		}

		// Token: 0x060001A6 RID: 422 RVA: 0x000061AC File Offset: 0x000043AC
		public bool RemapId(long currentId, long newId)
		{
			if (this.FindWithId(newId) != null)
			{
				return false;
			}
			SharedTableData.SharedTableEntry sharedTableEntry = this.FindWithId(currentId);
			if (sharedTableEntry == null)
			{
				return false;
			}
			sharedTableEntry.Id = newId;
			this.m_IdDictionary.Remove(currentId);
			this.m_IdDictionary[newId] = sharedTableEntry;
			return true;
		}

		// Token: 0x060001A7 RID: 423 RVA: 0x000061F4 File Offset: 0x000043F4
		[Obsolete("FindSimilarKey will be removed in the future, please use Unity Search. See TableEntrySearchData class for further details.")]
		public SharedTableData.SharedTableEntry FindSimilarKey(string text, out int distance)
		{
			SharedTableData.SharedTableEntry sharedTableEntry = null;
			distance = int.MaxValue;
			foreach (SharedTableData.SharedTableEntry sharedTableEntry2 in this.Entries)
			{
				int num = SharedTableData.ComputeLevenshteinDistance(text.ToLower(), sharedTableEntry2.Key.ToLower());
				if (num < distance)
				{
					sharedTableEntry = sharedTableEntry2;
					distance = num;
				}
			}
			return sharedTableEntry;
		}

		// Token: 0x060001A8 RID: 424 RVA: 0x0000626C File Offset: 0x0000446C
		private static int ComputeLevenshteinDistance(string a, string b)
		{
			int length = a.Length;
			int length2 = b.Length;
			int[,] array = new int[length + 1, length2 + 1];
			if (length == 0)
			{
				return length2;
			}
			if (length2 == 0)
			{
				return length;
			}
			int i = 0;
			while (i <= length)
			{
				array[i, 0] = i++;
			}
			int j = 0;
			while (j <= length2)
			{
				array[0, j] = j++;
			}
			for (int k = 1; k <= length; k++)
			{
				for (int l = 1; l <= length2; l++)
				{
					int num = ((b[l - 1] == a[k - 1]) ? 0 : 1);
					array[k, l] = Mathf.Min(Mathf.Min(array[k - 1, l] + 1, array[k, l - 1] + 1), array[k - 1, l - 1] + num);
				}
			}
			return array[length, length2];
		}

		// Token: 0x060001A9 RID: 425 RVA: 0x00006354 File Offset: 0x00004554
		private SharedTableData.SharedTableEntry AddKeyInternal(string key)
		{
			SharedTableData.SharedTableEntry sharedTableEntry = new SharedTableData.SharedTableEntry
			{
				Id = this.m_KeyGenerator.GetNextKey(),
				Key = key
			};
			while (this.FindWithId(sharedTableEntry.Id) != null)
			{
				sharedTableEntry.Id = this.m_KeyGenerator.GetNextKey();
			}
			this.Entries.Add(sharedTableEntry);
			if (this.m_IdDictionary.Count > 0)
			{
				this.m_IdDictionary[sharedTableEntry.Id] = sharedTableEntry;
			}
			if (this.m_KeyDictionary.Count > 0)
			{
				this.m_KeyDictionary[key] = sharedTableEntry;
			}
			return sharedTableEntry;
		}

		// Token: 0x060001AA RID: 426 RVA: 0x000063E8 File Offset: 0x000045E8
		private SharedTableData.SharedTableEntry AddKeyInternal(string key, long id)
		{
			SharedTableData.SharedTableEntry sharedTableEntry = new SharedTableData.SharedTableEntry
			{
				Id = id,
				Key = key
			};
			this.Entries.Add(sharedTableEntry);
			if (this.m_IdDictionary.Count > 0)
			{
				this.m_IdDictionary[sharedTableEntry.Id] = sharedTableEntry;
			}
			if (this.m_KeyDictionary.Count > 0)
			{
				this.m_KeyDictionary[key] = sharedTableEntry;
			}
			return sharedTableEntry;
		}

		// Token: 0x060001AB RID: 427 RVA: 0x00006451 File Offset: 0x00004651
		private void RenameKeyInternal(SharedTableData.SharedTableEntry entry, string newValue)
		{
			if (this.m_KeyDictionary.Count > 0)
			{
				this.m_KeyDictionary.Remove(entry.Key);
				this.m_KeyDictionary[newValue] = entry;
			}
			entry.Key = newValue;
		}

		// Token: 0x060001AC RID: 428 RVA: 0x00006488 File Offset: 0x00004688
		private void RemoveKeyInternal(SharedTableData.SharedTableEntry entry)
		{
			if (this.m_KeyDictionary.Count > 0)
			{
				this.m_KeyDictionary.Remove(entry.Key);
			}
			if (this.m_IdDictionary.Count > 0)
			{
				this.m_IdDictionary.Remove(entry.Id);
			}
			this.Entries.Remove(entry);
		}

		// Token: 0x060001AD RID: 429 RVA: 0x000064E4 File Offset: 0x000046E4
		private SharedTableData.SharedTableEntry FindWithId(long id)
		{
			if (id == 0L)
			{
				return null;
			}
			if (this.m_IdDictionary.Count == 0)
			{
				foreach (SharedTableData.SharedTableEntry sharedTableEntry in this.m_Entries)
				{
					this.m_IdDictionary[sharedTableEntry.Id] = sharedTableEntry;
				}
			}
			SharedTableData.SharedTableEntry sharedTableEntry2;
			this.m_IdDictionary.TryGetValue(id, out sharedTableEntry2);
			return sharedTableEntry2;
		}

		// Token: 0x060001AE RID: 430 RVA: 0x00006564 File Offset: 0x00004764
		private SharedTableData.SharedTableEntry FindWithKey(string key)
		{
			if (this.m_KeyDictionary.Count == 0)
			{
				foreach (SharedTableData.SharedTableEntry sharedTableEntry in this.m_Entries)
				{
					this.m_KeyDictionary[sharedTableEntry.Key] = sharedTableEntry;
				}
			}
			SharedTableData.SharedTableEntry sharedTableEntry2;
			this.m_KeyDictionary.TryGetValue(key, out sharedTableEntry2);
			return sharedTableEntry2;
		}

		// Token: 0x060001AF RID: 431 RVA: 0x000065E0 File Offset: 0x000047E0
		public override string ToString()
		{
			return this.TableCollectionName + "(Shared Table Data)";
		}

		// Token: 0x060001B0 RID: 432 RVA: 0x000065F2 File Offset: 0x000047F2
		public void OnBeforeSerialize()
		{
			this.m_TableCollectionNameGuidString = TableReference.StringFromGuid(this.m_TableCollectionNameGuid);
		}

		// Token: 0x060001B1 RID: 433 RVA: 0x00006605 File Offset: 0x00004805
		public void OnAfterDeserialize()
		{
			this.m_IdDictionary.Clear();
			this.m_KeyDictionary.Clear();
			this.m_TableCollectionNameGuid = (string.IsNullOrEmpty(this.m_TableCollectionNameGuidString) ? Guid.Empty : Guid.Parse(this.m_TableCollectionNameGuidString));
		}

		// Token: 0x0400005D RID: 93
		public const long EmptyId = 0L;

		// Token: 0x0400005E RID: 94
		internal const string NewEntryKey = "New Entry";

		// Token: 0x0400005F RID: 95
		[FormerlySerializedAs("m_TableName")]
		[SerializeField]
		private string m_TableCollectionName;

		// Token: 0x04000060 RID: 96
		[FormerlySerializedAs("m_TableNameGuidString")]
		[SerializeField]
		private string m_TableCollectionNameGuidString;

		// Token: 0x04000061 RID: 97
		[SerializeField]
		private List<SharedTableData.SharedTableEntry> m_Entries = new List<SharedTableData.SharedTableEntry>();

		// Token: 0x04000062 RID: 98
		[SerializeField]
		[MetadataType(MetadataType.SharedTableData)]
		private MetadataCollection m_Metadata = new MetadataCollection();

		// Token: 0x04000063 RID: 99
		[SerializeReference]
		private IKeyGenerator m_KeyGenerator = new DistributedUIDGenerator();

		// Token: 0x04000064 RID: 100
		private Guid m_TableCollectionNameGuid;

		// Token: 0x04000065 RID: 101
		private Dictionary<long, SharedTableData.SharedTableEntry> m_IdDictionary = new Dictionary<long, SharedTableData.SharedTableEntry>();

		// Token: 0x04000066 RID: 102
		private Dictionary<string, SharedTableData.SharedTableEntry> m_KeyDictionary = new Dictionary<string, SharedTableData.SharedTableEntry>();

		// Token: 0x0200011C RID: 284
		[Serializable]
		public class SharedTableEntry
		{
			// Token: 0x17000176 RID: 374
			// (get) Token: 0x06000710 RID: 1808 RVA: 0x00017BC6 File Offset: 0x00015DC6
			// (set) Token: 0x06000711 RID: 1809 RVA: 0x00017BCE File Offset: 0x00015DCE
			public long Id
			{
				get
				{
					return this.m_Id;
				}
				internal set
				{
					this.m_Id = value;
				}
			}

			// Token: 0x17000177 RID: 375
			// (get) Token: 0x06000712 RID: 1810 RVA: 0x00017BD7 File Offset: 0x00015DD7
			// (set) Token: 0x06000713 RID: 1811 RVA: 0x00017BDF File Offset: 0x00015DDF
			public string Key
			{
				get
				{
					return this.m_Key;
				}
				internal set
				{
					this.m_Key = value;
				}
			}

			// Token: 0x17000178 RID: 376
			// (get) Token: 0x06000714 RID: 1812 RVA: 0x00017BE8 File Offset: 0x00015DE8
			// (set) Token: 0x06000715 RID: 1813 RVA: 0x00017BF0 File Offset: 0x00015DF0
			public MetadataCollection Metadata
			{
				get
				{
					return this.m_Metadata;
				}
				set
				{
					this.m_Metadata = value;
				}
			}

			// Token: 0x06000716 RID: 1814 RVA: 0x00017BF9 File Offset: 0x00015DF9
			public override string ToString()
			{
				return string.Format("{0} - {1}", this.Id, this.Key);
			}

			// Token: 0x0400026E RID: 622
			[SerializeField]
			private long m_Id;

			// Token: 0x0400026F RID: 623
			[SerializeField]
			private string m_Key;

			// Token: 0x04000270 RID: 624
			[SerializeField]
			private MetadataCollection m_Metadata = new MetadataCollection();
		}
	}
}
