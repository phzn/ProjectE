using System;
using System.Collections.Generic;
using Bozo.AnimeCharacters;
using UnityEngine;

namespace Features.NPC
{
	// Token: 0x02000ED9 RID: 3801
	[CreateAssetMenu(fileName = "NPCPoliceBakedRegistry", menuName = "NPCs/Police Baked Registry")]
	public class NPCPoliceBakedRegistry : ScriptableObject
	{
		// Token: 0x17001295 RID: 4757
		// (get) Token: 0x060071C4 RID: 29124 RVA: 0x002E39EE File Offset: 0x002E1BEE
		public IReadOnlyList<NPCPoliceBakedRegistry.Entry> Entries
		{
			get
			{
				return this.entries;
			}
		}

		// Token: 0x17001296 RID: 4758
		// (get) Token: 0x060071C5 RID: 29125 RVA: 0x002E39F6 File Offset: 0x002E1BF6
		public int Count
		{
			get
			{
				return this.entries.Count;
			}
		}

		// Token: 0x060071C6 RID: 29126 RVA: 0x002E3A03 File Offset: 0x002E1C03
		private void OnEnable()
		{
			this._cacheBuilt = false;
		}

		// Token: 0x060071C7 RID: 29127 RVA: 0x002E3A0C File Offset: 0x002E1C0C
		private void EnsureCache()
		{
			if (this._cacheBuilt && this._cache != null)
			{
				return;
			}
			this._cache = new Dictionary<CharacterObject, GameObject>(this.entries.Count);
			foreach (NPCPoliceBakedRegistry.Entry entry in this.entries)
			{
				if (!(((entry != null) ? entry.characterData : null) == null) && !(entry.bakedPrefab == null))
				{
					this._cache[entry.characterData] = entry.bakedPrefab;
				}
			}
			this._cacheBuilt = true;
		}

		// Token: 0x060071C8 RID: 29128 RVA: 0x002E3AC0 File Offset: 0x002E1CC0
		public bool TryGet(CharacterObject data, out GameObject prefab)
		{
			this.EnsureCache();
			if (data == null)
			{
				prefab = null;
				return false;
			}
			return this._cache.TryGetValue(data, out prefab);
		}

		// Token: 0x060071C9 RID: 29129 RVA: 0x002E3AE4 File Offset: 0x002E1CE4
		public bool Has(CharacterObject data)
		{
			GameObject gameObject;
			return this.TryGet(data, out gameObject);
		}

		// Token: 0x060071CA RID: 29130 RVA: 0x002E3AFC File Offset: 0x002E1CFC
		public GameObject GetRandomBakedPrefab()
		{
			if (this.entries == null || this.entries.Count == 0)
			{
				return null;
			}
			for (int i = 0; i < 8; i++)
			{
				NPCPoliceBakedRegistry.Entry entry = this.entries[Random.Range(0, this.entries.Count)];
				if (((entry != null) ? entry.bakedPrefab : null) != null)
				{
					return entry.bakedPrefab;
				}
			}
			foreach (NPCPoliceBakedRegistry.Entry entry2 in this.entries)
			{
				if (((entry2 != null) ? entry2.bakedPrefab : null) != null)
				{
					return entry2.bakedPrefab;
				}
			}
			return null;
		}

		// Token: 0x040074AF RID: 29871
		[SerializeField]
		private List<NPCPoliceBakedRegistry.Entry> entries = new List<NPCPoliceBakedRegistry.Entry>();

		// Token: 0x040074B0 RID: 29872
		private Dictionary<CharacterObject, GameObject> _cache;

		// Token: 0x040074B1 RID: 29873
		private bool _cacheBuilt;

		// Token: 0x02000EDA RID: 3802
		[Serializable]
		public class Entry
		{
			// Token: 0x040074B2 RID: 29874
			public CharacterObject characterData;

			// Token: 0x040074B3 RID: 29875
			public GameObject bakedPrefab;
		}
	}
}
