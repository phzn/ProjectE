using System;
using System.Collections.Generic;
using Bozo.AnimeCharacters;
using UnityEngine;

namespace Features.NPC
{
	// Token: 0x02000EE2 RID: 3810
	[CreateAssetMenu(fileName = "NPCReporterBakedRegistry", menuName = "NPCs/Reporter Baked Registry")]
	public class NPCReporterBakedRegistry : ScriptableObject
	{
		// Token: 0x170012A0 RID: 4768
		// (get) Token: 0x06007220 RID: 29216 RVA: 0x002E707C File Offset: 0x002E527C
		public IReadOnlyList<NPCReporterBakedRegistry.Entry> Entries
		{
			get
			{
				return this.entries;
			}
		}

		// Token: 0x170012A1 RID: 4769
		// (get) Token: 0x06007221 RID: 29217 RVA: 0x002E7084 File Offset: 0x002E5284
		public int Count
		{
			get
			{
				return this.entries.Count;
			}
		}

		// Token: 0x06007222 RID: 29218 RVA: 0x002E7091 File Offset: 0x002E5291
		private void OnEnable()
		{
			this._cacheBuilt = false;
		}

		// Token: 0x06007223 RID: 29219 RVA: 0x002E709C File Offset: 0x002E529C
		private void EnsureCache()
		{
			if (this._cacheBuilt && this._cache != null)
			{
				return;
			}
			this._cache = new Dictionary<CharacterObject, GameObject>(this.entries.Count);
			foreach (NPCReporterBakedRegistry.Entry entry in this.entries)
			{
				if (!(((entry != null) ? entry.characterData : null) == null) && !(entry.bakedPrefab == null))
				{
					this._cache[entry.characterData] = entry.bakedPrefab;
				}
			}
			this._cacheBuilt = true;
		}

		// Token: 0x06007224 RID: 29220 RVA: 0x002E7150 File Offset: 0x002E5350
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

		// Token: 0x06007225 RID: 29221 RVA: 0x002E7174 File Offset: 0x002E5374
		public bool Has(CharacterObject data)
		{
			GameObject gameObject;
			return this.TryGet(data, out gameObject);
		}

		// Token: 0x04007541 RID: 30017
		[SerializeField]
		private List<NPCReporterBakedRegistry.Entry> entries = new List<NPCReporterBakedRegistry.Entry>();

		// Token: 0x04007542 RID: 30018
		private Dictionary<CharacterObject, GameObject> _cache;

		// Token: 0x04007543 RID: 30019
		private bool _cacheBuilt;

		// Token: 0x02000EE3 RID: 3811
		[Serializable]
		public class Entry
		{
			// Token: 0x04007544 RID: 30020
			public CharacterObject characterData;

			// Token: 0x04007545 RID: 30021
			public GameObject bakedPrefab;
		}
	}
}
