using System;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectExhibitionist.Minimap
{
	// Token: 0x020009B7 RID: 2487
	[CreateAssetMenu(fileName = "ProceduralMiniMapStyleAtlas", menuName = "MapGeneration/Minimap Style Atlas")]
	public sealed class ProceduralMiniMapStyleAtlas : ScriptableObject
	{
		// Token: 0x17000BD5 RID: 3029
		// (get) Token: 0x060045F1 RID: 17905 RVA: 0x001A7985 File Offset: 0x001A5B85
		public Texture2D Atlas
		{
			get
			{
				return this.atlas;
			}
		}

		// Token: 0x17000BD6 RID: 3030
		// (get) Token: 0x060045F2 RID: 17906 RVA: 0x001A798D File Offset: 0x001A5B8D
		public List<ProceduralMiniMapStyleAtlas.Entry> Entries
		{
			get
			{
				return this.entries;
			}
		}

		// Token: 0x17000BD7 RID: 3031
		// (get) Token: 0x060045F3 RID: 17907 RVA: 0x001A7995 File Offset: 0x001A5B95
		public bool HasEntries
		{
			get
			{
				return this.atlas != null && this.entries != null && this.entries.Count > 0;
			}
		}

		// Token: 0x17000BD8 RID: 3032
		// (get) Token: 0x060045F4 RID: 17908 RVA: 0x001A79BD File Offset: 0x001A5BBD
		// (set) Token: 0x060045F5 RID: 17909 RVA: 0x001A79C5 File Offset: 0x001A5BC5
		internal int Revision { get; private set; }

		// Token: 0x060045F6 RID: 17910 RVA: 0x001A79CE File Offset: 0x001A5BCE
		private void OnEnable()
		{
			this.RebuildLookup();
		}

		// Token: 0x060045F7 RID: 17911 RVA: 0x001A79D8 File Offset: 0x001A5BD8
		public bool TryGet(Object source, string runtimeName, out ProceduralMiniMapStyleAtlas.Entry entry)
		{
			this.EnsureLookup();
			ProceduralMiniMapStyleAtlas.SourceLookupKey sourceLookupKey = default(ProceduralMiniMapStyleAtlas.SourceLookupKey);
			bool flag = source != null;
			if (flag)
			{
				sourceLookupKey = new ProceduralMiniMapStyleAtlas.SourceLookupKey(source, runtimeName);
				if (this._sourceHits.TryGetValue(sourceLookupKey, out entry))
				{
					return true;
				}
				if (this._sourceMisses.Contains(sourceLookupKey))
				{
					entry = default(ProceduralMiniMapStyleAtlas.Entry);
					return false;
				}
			}
			if (source != null && this._byName.TryGetValue(ProceduralMiniMapStyleAtlas.Normalize(source.name), out entry))
			{
				this._sourceHits[sourceLookupKey] = entry;
				return true;
			}
			string text = ProceduralMiniMapStyleAtlas.Normalize(runtimeName);
			if (this._byName.TryGetValue(text, out entry))
			{
				if (flag)
				{
					this._sourceHits[sourceLookupKey] = entry;
				}
				return true;
			}
			foreach (KeyValuePair<string, ProceduralMiniMapStyleAtlas.Entry> keyValuePair in this._byName)
			{
				if (text.Contains(keyValuePair.Key) || keyValuePair.Key.Contains(text))
				{
					entry = keyValuePair.Value;
					if (flag)
					{
						this._sourceHits[sourceLookupKey] = entry;
					}
					return true;
				}
			}
			entry = default(ProceduralMiniMapStyleAtlas.Entry);
			if (flag)
			{
				this._sourceMisses.Add(sourceLookupKey);
			}
			return false;
		}

		// Token: 0x060045F8 RID: 17912 RVA: 0x001A7B34 File Offset: 0x001A5D34
		public void Configure(Texture2D texture, List<ProceduralMiniMapStyleAtlas.Entry> bakedEntries)
		{
			this.atlas = texture;
			this.entries = bakedEntries ?? new List<ProceduralMiniMapStyleAtlas.Entry>();
			this.RebuildLookup();
		}

		// Token: 0x060045F9 RID: 17913 RVA: 0x001A7B53 File Offset: 0x001A5D53
		private void EnsureLookup()
		{
			if (this._byName == null)
			{
				this.RebuildLookup();
			}
		}

		// Token: 0x060045FA RID: 17914 RVA: 0x001A7B64 File Offset: 0x001A5D64
		private void RebuildLookup()
		{
			int revision = this.Revision;
			this.Revision = revision + 1;
			this._byName = new Dictionary<string, ProceduralMiniMapStyleAtlas.Entry>(StringComparer.Ordinal);
			this._sourceHits = new Dictionary<ProceduralMiniMapStyleAtlas.SourceLookupKey, ProceduralMiniMapStyleAtlas.Entry>();
			this._sourceMisses = new HashSet<ProceduralMiniMapStyleAtlas.SourceLookupKey>();
			if (this.entries == null)
			{
				return;
			}
			for (int i = 0; i < this.entries.Count; i++)
			{
				ProceduralMiniMapStyleAtlas.Entry entry = this.entries[i];
				string text = ProceduralMiniMapStyleAtlas.Normalize(entry.sourceName);
				if (!string.IsNullOrEmpty(text))
				{
					this._byName[text] = entry;
				}
			}
		}

		// Token: 0x060045FB RID: 17915 RVA: 0x001A7BF4 File Offset: 0x001A5DF4
		private static string Normalize(string value)
		{
			if (string.IsNullOrEmpty(value))
			{
				return string.Empty;
			}
			return value.ToLowerInvariant().Replace("zone_", string.Empty).Replace("(clone)", string.Empty)
				.Replace(" ", string.Empty)
				.Replace("-", string.Empty)
				.Replace("_", string.Empty)
				.Trim();
		}

		// Token: 0x04004590 RID: 17808
		[SerializeField]
		private Texture2D atlas;

		// Token: 0x04004591 RID: 17809
		[SerializeField]
		private List<ProceduralMiniMapStyleAtlas.Entry> entries = new List<ProceduralMiniMapStyleAtlas.Entry>();

		// Token: 0x04004592 RID: 17810
		private Dictionary<string, ProceduralMiniMapStyleAtlas.Entry> _byName;

		// Token: 0x04004593 RID: 17811
		private Dictionary<ProceduralMiniMapStyleAtlas.SourceLookupKey, ProceduralMiniMapStyleAtlas.Entry> _sourceHits;

		// Token: 0x04004594 RID: 17812
		private HashSet<ProceduralMiniMapStyleAtlas.SourceLookupKey> _sourceMisses;

		// Token: 0x020009B8 RID: 2488
		[Serializable]
		public struct Entry
		{
			// Token: 0x04004596 RID: 17814
			public string sourceName;

			// Token: 0x04004597 RID: 17815
			public Rect uv;

			// Token: 0x04004598 RID: 17816
			public Vector2 worldSize;

			// Token: 0x04004599 RID: 17817
			public Vector2 centerOffset;
		}

		// Token: 0x020009B9 RID: 2489
		private readonly struct SourceLookupKey : IEquatable<ProceduralMiniMapStyleAtlas.SourceLookupKey>
		{
			// Token: 0x060045FD RID: 17917 RVA: 0x001A7C78 File Offset: 0x001A5E78
			public SourceLookupKey(Object source, string runtimeName)
			{
				this._sourceId = source.GetInstanceID();
				this._runtimeName = runtimeName;
			}

			// Token: 0x060045FE RID: 17918 RVA: 0x001A7C8D File Offset: 0x001A5E8D
			public bool Equals(ProceduralMiniMapStyleAtlas.SourceLookupKey other)
			{
				return this._sourceId == other._sourceId && string.Equals(this._runtimeName, other._runtimeName, StringComparison.Ordinal);
			}

			// Token: 0x060045FF RID: 17919 RVA: 0x001A7CB4 File Offset: 0x001A5EB4
			public override bool Equals(object obj)
			{
				if (obj is ProceduralMiniMapStyleAtlas.SourceLookupKey)
				{
					ProceduralMiniMapStyleAtlas.SourceLookupKey sourceLookupKey = (ProceduralMiniMapStyleAtlas.SourceLookupKey)obj;
					return this.Equals(sourceLookupKey);
				}
				return false;
			}

			// Token: 0x06004600 RID: 17920 RVA: 0x001A7CD9 File Offset: 0x001A5ED9
			public override int GetHashCode()
			{
				return (this._sourceId * 397) ^ ((this._runtimeName != null) ? this._runtimeName.GetHashCode() : 0);
			}

			// Token: 0x0400459A RID: 17818
			private readonly int _sourceId;

			// Token: 0x0400459B RID: 17819
			private readonly string _runtimeName;
		}
	}
}
