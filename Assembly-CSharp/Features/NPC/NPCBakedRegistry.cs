using System;
using System.Collections.Generic;
using Bozo.AnimeCharacters;
using UnityEngine;

namespace Features.NPC
{
	// Token: 0x02000ED1 RID: 3793
	[CreateAssetMenu(fileName = "NPCBakedRegistry", menuName = "NPCs/Baked Registry")]
	public class NPCBakedRegistry : ScriptableObject
	{
		// Token: 0x17001289 RID: 4745
		// (get) Token: 0x06007198 RID: 29080 RVA: 0x002E25FD File Offset: 0x002E07FD
		public IReadOnlyList<NPCBakedRegistry.Entry> Entries
		{
			get
			{
				return this.entries;
			}
		}

		// Token: 0x1700128A RID: 4746
		// (get) Token: 0x06007199 RID: 29081 RVA: 0x002E2605 File Offset: 0x002E0805
		public int Count
		{
			get
			{
				return this.entries.Count;
			}
		}

		// Token: 0x0600719A RID: 29082 RVA: 0x002E2612 File Offset: 0x002E0812
		private void OnEnable()
		{
			this._cacheBuilt = false;
		}

		// Token: 0x0600719B RID: 29083 RVA: 0x002E261C File Offset: 0x002E081C
		private void EnsureCache()
		{
			if (this._cacheBuilt && this._cache != null)
			{
				return;
			}
			this._cache = new Dictionary<CharacterObject, GameObject>(this.entries.Count);
			foreach (NPCBakedRegistry.Entry entry in this.entries)
			{
				if (!(((entry != null) ? entry.characterData : null) == null) && !(entry.bakedPrefab == null))
				{
					this._cache[entry.characterData] = entry.bakedPrefab;
				}
			}
			this._cacheBuilt = true;
		}

		// Token: 0x0600719C RID: 29084 RVA: 0x002E26D0 File Offset: 0x002E08D0
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

		// Token: 0x0600719D RID: 29085 RVA: 0x002E26F4 File Offset: 0x002E08F4
		public bool Has(CharacterObject data)
		{
			GameObject gameObject;
			return this.TryGet(data, out gameObject);
		}

		// Token: 0x04007494 RID: 29844
		[SerializeField]
		private List<NPCBakedRegistry.Entry> entries = new List<NPCBakedRegistry.Entry>();

		// Token: 0x04007495 RID: 29845
		private Dictionary<CharacterObject, GameObject> _cache;

		// Token: 0x04007496 RID: 29846
		private bool _cacheBuilt;

		// Token: 0x02000ED2 RID: 3794
		[Serializable]
		public class Entry
		{
			// Token: 0x04007497 RID: 29847
			public CharacterObject characterData;

			// Token: 0x04007498 RID: 29848
			public GameObject bakedPrefab;
		}
	}
}
