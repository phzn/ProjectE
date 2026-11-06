using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Localization.Metadata;

namespace UnityEngine.Localization.Tables
{
	// Token: 0x0200002B RID: 43
	public abstract class DetailedLocalizationTable<TEntry> : LocalizationTable, IDictionary<long, TEntry>, ICollection<KeyValuePair<long, TEntry>>, IEnumerable<KeyValuePair<long, TEntry>>, IEnumerable, ISerializationCallbackReceiver where TEntry : TableEntry
	{
		// Token: 0x17000043 RID: 67
		// (get) Token: 0x06000146 RID: 326 RVA: 0x000054AF File Offset: 0x000036AF
		ICollection<long> IDictionary<long, TEntry>.Keys
		{
			get
			{
				return this.m_TableEntries.Keys;
			}
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x06000147 RID: 327 RVA: 0x000054BC File Offset: 0x000036BC
		public ICollection<TEntry> Values
		{
			get
			{
				return this.m_TableEntries.Values;
			}
		}

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x06000148 RID: 328 RVA: 0x000054C9 File Offset: 0x000036C9
		public int Count
		{
			get
			{
				return this.m_TableEntries.Count;
			}
		}

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x06000149 RID: 329 RVA: 0x000054D6 File Offset: 0x000036D6
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000047 RID: 71
		public TEntry this[long key]
		{
			get
			{
				return this.m_TableEntries[key];
			}
			set
			{
				if (key == 0L)
				{
					throw new ArgumentException("Key Id value 0, is not valid. All Key Id's must be non-zero.");
				}
				if (value.Table != this)
				{
					throw new ArgumentException("Table entry does not belong to this table. Table entries can not be shared across tables.");
				}
				this.RemoveEntry(value.Data.Id);
				value.Data.Id = key;
				this.m_TableEntries[key] = value;
			}
		}

		// Token: 0x17000048 RID: 72
		public TEntry this[string keyName]
		{
			get
			{
				return this.GetEntry(keyName);
			}
			set
			{
				if (value.Table != this)
				{
					throw new ArgumentException("Table entry does not belong to this table. Table entries can not be shared across tables.");
				}
				long num = base.FindKeyId(keyName, true);
				this[num] = value;
			}
		}

		// Token: 0x0600014E RID: 334
		public abstract TEntry CreateTableEntry();

		// Token: 0x0600014F RID: 335 RVA: 0x0000559C File Offset: 0x0000379C
		internal TEntry CreateTableEntry(TableEntryData data)
		{
			TEntry tentry = this.CreateTableEntry();
			tentry.Data = data;
			return tentry;
		}

		// Token: 0x06000150 RID: 336 RVA: 0x000055B0 File Offset: 0x000037B0
		public override void CreateEmpty(TableEntryReference entryReference)
		{
			this.AddEntryFromReference(entryReference, string.Empty);
		}

		// Token: 0x06000151 RID: 337 RVA: 0x000055C0 File Offset: 0x000037C0
		public TEntry AddEntry(string key, string localized)
		{
			long num = base.FindKeyId(key, true);
			if (num != 0L)
			{
				return this.AddEntry(num, localized);
			}
			return default(TEntry);
		}

		// Token: 0x06000152 RID: 338 RVA: 0x000055EC File Offset: 0x000037EC
		public virtual TEntry AddEntry(long keyId, string localized)
		{
			if (keyId == 0L)
			{
				throw new ArgumentException(string.Format("Key Id value {0}({1}), is not valid. All Key Id's must be non-zero.", "EmptyId", 0L), "keyId");
			}
			TEntry tentry;
			if (!this.m_TableEntries.TryGetValue(keyId, out tentry))
			{
				tentry = this.CreateTableEntry();
				tentry.Data = new TableEntryData(keyId);
				this.m_TableEntries[keyId] = tentry;
			}
			tentry.Data.Localized = localized;
			return tentry;
		}

		// Token: 0x06000153 RID: 339 RVA: 0x00005664 File Offset: 0x00003864
		public TEntry AddEntryFromReference(TableEntryReference entryReference, string localized)
		{
			if (entryReference.ReferenceType == TableEntryReference.Type.Id)
			{
				return this.AddEntry(entryReference.KeyId, localized);
			}
			if (entryReference.ReferenceType == TableEntryReference.Type.Name)
			{
				return this.AddEntry(entryReference.Key, localized);
			}
			throw new ArgumentException("TableEntryReference should not be Empty", "entryReference");
		}

		// Token: 0x06000154 RID: 340 RVA: 0x000056B4 File Offset: 0x000038B4
		public bool RemoveEntry(string key)
		{
			long num = base.FindKeyId(key, false);
			return num != 0L && this.RemoveEntry(num);
		}

		// Token: 0x06000155 RID: 341 RVA: 0x000056D8 File Offset: 0x000038D8
		public virtual bool RemoveEntry(long keyId)
		{
			TEntry tentry;
			if (this.m_TableEntries.TryGetValue(keyId, out tentry))
			{
				for (int i = 0; i < base.MetadataEntries.Count; i++)
				{
					SharedTableEntryMetadata sharedTableEntryMetadata = base.MetadataEntries[i] as SharedTableEntryMetadata;
					if (sharedTableEntryMetadata != null)
					{
						sharedTableEntryMetadata.Unregister(tentry);
						if (sharedTableEntryMetadata.Count == 0)
						{
							base.MetadataEntries.RemoveAt(i);
							i--;
						}
					}
				}
				int num = 0;
				for (;;)
				{
					int num2 = num;
					SharedTableData sharedData = base.SharedData;
					int? num3 = ((sharedData != null) ? new int?(sharedData.Metadata.MetadataEntries.Count) : null);
					if (!((num2 < num3.GetValueOrDefault()) & (num3 != null)))
					{
						break;
					}
					SharedTableCollectionMetadata sharedTableCollectionMetadata = base.SharedData.Metadata.MetadataEntries[num] as SharedTableCollectionMetadata;
					if (sharedTableCollectionMetadata != null)
					{
						sharedTableCollectionMetadata.RemoveEntry(keyId, base.LocaleIdentifier.Code);
						if (sharedTableCollectionMetadata.IsEmpty)
						{
							base.SharedData.Metadata.MetadataEntries.RemoveAt(num);
							num--;
						}
					}
					num++;
				}
				tentry.Data.Id = 0L;
				tentry.Table = null;
				return this.m_TableEntries.Remove(keyId);
			}
			return false;
		}

		// Token: 0x06000156 RID: 342 RVA: 0x0000581C File Offset: 0x00003A1C
		public TEntry GetEntryFromReference(TableEntryReference entryReference)
		{
			if (entryReference.ReferenceType == TableEntryReference.Type.Id)
			{
				return this.GetEntry(entryReference.KeyId);
			}
			if (entryReference.ReferenceType == TableEntryReference.Type.Name)
			{
				return this.GetEntry(entryReference.Key);
			}
			return default(TEntry);
		}

		// Token: 0x06000157 RID: 343 RVA: 0x00005864 File Offset: 0x00003A64
		public TEntry GetEntry(string key)
		{
			long num = base.FindKeyId(key, false);
			if (num != 0L)
			{
				return this.GetEntry(num);
			}
			return default(TEntry);
		}

		// Token: 0x06000158 RID: 344 RVA: 0x00005890 File Offset: 0x00003A90
		public virtual TEntry GetEntry(long keyId)
		{
			TEntry tentry;
			this.m_TableEntries.TryGetValue(keyId, out tentry);
			return tentry;
		}

		// Token: 0x06000159 RID: 345 RVA: 0x000058AD File Offset: 0x00003AAD
		public void Add(long keyId, TEntry value)
		{
			this[keyId] = value;
		}

		// Token: 0x0600015A RID: 346 RVA: 0x000058B7 File Offset: 0x00003AB7
		public void Add(KeyValuePair<long, TEntry> item)
		{
			this[item.Key] = item.Value;
		}

		// Token: 0x0600015B RID: 347 RVA: 0x000058CD File Offset: 0x00003ACD
		public bool ContainsKey(long keyId)
		{
			return this.m_TableEntries.ContainsKey(keyId);
		}

		// Token: 0x0600015C RID: 348 RVA: 0x000058DC File Offset: 0x00003ADC
		public bool ContainsValue(string localized)
		{
			using (Dictionary<long, TEntry>.ValueCollection.Enumerator enumerator = this.m_TableEntries.Values.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.Data.Localized == localized)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600015D RID: 349 RVA: 0x0000594C File Offset: 0x00003B4C
		public bool Contains(KeyValuePair<long, TEntry> item)
		{
			return this.m_TableEntries.Contains(item);
		}

		// Token: 0x0600015E RID: 350 RVA: 0x0000595A File Offset: 0x00003B5A
		public bool Remove(long keyId)
		{
			return this.RemoveEntry(keyId);
		}

		// Token: 0x0600015F RID: 351 RVA: 0x00005963 File Offset: 0x00003B63
		public bool Remove(KeyValuePair<long, TEntry> item)
		{
			if (this.Contains(item))
			{
				this.RemoveEntry(item.Key);
				return true;
			}
			return false;
		}

		// Token: 0x06000160 RID: 352 RVA: 0x00005980 File Offset: 0x00003B80
		public IList<TEntry> CheckForMissingSharedTableDataEntries(MissingEntryAction action = MissingEntryAction.Nothing)
		{
			TEntry[] array = (from e in this.m_TableEntries
				where !base.SharedData.Contains(e.Key)
				select e.Value).ToArray<TEntry>();
			if (array.Length == 0)
			{
				return array;
			}
			if (action == MissingEntryAction.AddEntriesToSharedData)
			{
				for (int i = 0; i < array.Length; i++)
				{
					SharedTableData.SharedTableEntry sharedTableEntry = base.SharedData.AddKey(null);
					base.SharedData.RemapId(sharedTableEntry.Id, array[i].KeyId);
				}
			}
			else if (action == MissingEntryAction.RemoveEntriesFromTable)
			{
				for (int j = 0; j < array.Length; j++)
				{
					this.RemoveEntry(array[j].KeyId);
				}
			}
			return array;
		}

		// Token: 0x06000161 RID: 353 RVA: 0x00005A43 File Offset: 0x00003C43
		public bool TryGetValue(long keyId, out TEntry value)
		{
			return this.m_TableEntries.TryGetValue(keyId, out value);
		}

		// Token: 0x06000162 RID: 354 RVA: 0x00005A52 File Offset: 0x00003C52
		public void Clear()
		{
			base.TableData.Clear();
			this.m_TableEntries.Clear();
		}

		// Token: 0x06000163 RID: 355 RVA: 0x00005A6C File Offset: 0x00003C6C
		public void CopyTo(KeyValuePair<long, TEntry>[] array, int arrayIndex)
		{
			foreach (KeyValuePair<long, TEntry> keyValuePair in this.m_TableEntries)
			{
				array[arrayIndex++] = keyValuePair;
			}
		}

		// Token: 0x06000164 RID: 356 RVA: 0x00005AC8 File Offset: 0x00003CC8
		public IEnumerator<KeyValuePair<long, TEntry>> GetEnumerator()
		{
			return this.m_TableEntries.GetEnumerator();
		}

		// Token: 0x06000165 RID: 357 RVA: 0x00005ADA File Offset: 0x00003CDA
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.m_TableEntries.GetEnumerator();
		}

		// Token: 0x06000166 RID: 358 RVA: 0x00005AEC File Offset: 0x00003CEC
		public override string ToString()
		{
			return string.Format("{0}({1})", base.TableCollectionName, base.LocaleIdentifier);
		}

		// Token: 0x06000167 RID: 359 RVA: 0x00005B0C File Offset: 0x00003D0C
		public void OnBeforeSerialize()
		{
			base.TableData.Clear();
			foreach (KeyValuePair<long, TEntry> keyValuePair in this)
			{
				keyValuePair.Value.Data.Id = keyValuePair.Key;
				base.TableData.Add(keyValuePair.Value.Data);
			}
		}

		// Token: 0x06000168 RID: 360 RVA: 0x00005B94 File Offset: 0x00003D94
		public void OnAfterDeserialize()
		{
			try
			{
				this.m_TableEntries = base.TableData.ToDictionary<TableEntryData, long, TEntry>((TableEntryData o) => o.Id, new Func<TableEntryData, TEntry>(this.CreateTableEntry));
			}
			catch (Exception ex)
			{
				Debug.LogError(string.Format("Error Deserializing Table Data \"{0}({1})\".\n{2}\n{3}", new object[] { base.TableCollectionName, base.LocaleIdentifier, ex.Message, ex.InnerException }), this);
			}
		}

		// Token: 0x0400004E RID: 78
		private Dictionary<long, TEntry> m_TableEntries = new Dictionary<long, TEntry>();
	}
}
