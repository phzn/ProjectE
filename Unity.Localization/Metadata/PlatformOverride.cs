using System;
using System.Collections.Generic;
using UnityEngine.Localization.Tables;

namespace UnityEngine.Localization.Metadata
{
	// Token: 0x020000E3 RID: 227
	[Metadata(AllowedTypes = MetadataType.StringTableEntry | MetadataType.AssetTableEntry | MetadataType.SharedStringTableEntry | MetadataType.SharedAssetTableEntry, AllowMultiple = false)]
	[Serializable]
	public class PlatformOverride : IEntryOverride, IMetadata, ISerializationCallbackReceiver
	{
		// Token: 0x06000612 RID: 1554 RVA: 0x00014F68 File Offset: 0x00013168
		public void AddPlatformTableOverride(RuntimePlatform platform, TableReference table)
		{
			this.AddPlatformOverride(platform, table, default(TableEntryReference), EntryOverrideType.Table);
		}

		// Token: 0x06000613 RID: 1555 RVA: 0x00014F88 File Offset: 0x00013188
		public void AddPlatformEntryOverride(RuntimePlatform platform, TableEntryReference entry)
		{
			this.AddPlatformOverride(platform, default(TableReference), entry, EntryOverrideType.Entry);
		}

		// Token: 0x06000614 RID: 1556 RVA: 0x00014FA8 File Offset: 0x000131A8
		public void AddPlatformOverride(RuntimePlatform platform, TableReference table, TableEntryReference entry, EntryOverrideType entryOverrideType = EntryOverrideType.TableAndEntry)
		{
			PlatformOverride.PlatformOverrideData platformOverrideData = null;
			for (int i = 0; i < this.m_PlatformOverrides.Count; i++)
			{
				if (this.m_PlatformOverrides[i].platform == platform)
				{
					platformOverrideData = this.m_PlatformOverrides[i];
					break;
				}
			}
			if (platformOverrideData == null)
			{
				platformOverrideData = new PlatformOverride.PlatformOverrideData
				{
					platform = platform
				};
				this.m_PlatformOverrides.Add(platformOverrideData);
			}
			platformOverrideData.entryOverrideType = entryOverrideType;
			platformOverrideData.tableReference = table;
			platformOverrideData.tableEntryReference = entry;
		}

		// Token: 0x06000615 RID: 1557 RVA: 0x00015024 File Offset: 0x00013224
		public bool RemovePlatformOverride(RuntimePlatform platform)
		{
			for (int i = 0; i < this.m_PlatformOverrides.Count; i++)
			{
				if (this.m_PlatformOverrides[i].platform == platform)
				{
					this.m_PlatformOverrides.RemoveAt(i);
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000616 RID: 1558 RVA: 0x0001506C File Offset: 0x0001326C
		public EntryOverrideType GetOverride(out TableReference tableReference, out TableEntryReference tableEntryReference)
		{
			if (this.m_PlayerPlatformOverride == null)
			{
				tableReference = default(TableReference);
				tableEntryReference = default(TableEntryReference);
				return EntryOverrideType.None;
			}
			tableReference = this.m_PlayerPlatformOverride.tableReference;
			tableEntryReference = this.m_PlayerPlatformOverride.tableEntryReference;
			return this.m_PlayerPlatformOverride.entryOverrideType;
		}

		// Token: 0x06000617 RID: 1559 RVA: 0x000150C0 File Offset: 0x000132C0
		public EntryOverrideType GetOverride(out TableReference tableReference, out TableEntryReference tableEntryReference, RuntimePlatform platform)
		{
			for (int i = 0; i < this.m_PlatformOverrides.Count; i++)
			{
				if (this.m_PlatformOverrides[i].platform == platform)
				{
					PlatformOverride.PlatformOverrideData platformOverrideData = this.m_PlatformOverrides[i];
					tableReference = platformOverrideData.tableReference;
					tableEntryReference = platformOverrideData.tableEntryReference;
					return platformOverrideData.entryOverrideType;
				}
			}
			tableReference = default(TableReference);
			tableEntryReference = default(TableEntryReference);
			return EntryOverrideType.None;
		}

		// Token: 0x06000618 RID: 1560 RVA: 0x00015132 File Offset: 0x00013332
		public void OnBeforeSerialize()
		{
		}

		// Token: 0x06000619 RID: 1561 RVA: 0x00015134 File Offset: 0x00013334
		public void OnAfterDeserialize()
		{
			for (int i = 0; i < this.m_PlatformOverrides.Count; i++)
			{
				if (this.m_PlatformOverrides[i].platform == Application.platform)
				{
					this.m_PlayerPlatformOverride = this.m_PlatformOverrides[i];
					return;
				}
			}
		}

		// Token: 0x04000218 RID: 536
		[SerializeField]
		private List<PlatformOverride.PlatformOverrideData> m_PlatformOverrides = new List<PlatformOverride.PlatformOverrideData>();

		// Token: 0x04000219 RID: 537
		private PlatformOverride.PlatformOverrideData m_PlayerPlatformOverride;

		// Token: 0x0200015E RID: 350
		[Serializable]
		private class PlatformOverrideData
		{
			// Token: 0x06000812 RID: 2066 RVA: 0x000196D8 File Offset: 0x000178D8
			public override string ToString()
			{
				switch (this.entryOverrideType)
				{
				case EntryOverrideType.Table:
					return string.Format("{0}: {1}", this.platform, this.tableReference);
				case EntryOverrideType.Entry:
					return string.Format("{0}: {1}", this.platform, this.tableEntryReference);
				case EntryOverrideType.TableAndEntry:
					return string.Format("{0}: {1}/{2}", this.platform, this.tableReference, this.tableEntryReference);
				default:
					return string.Format("{0}: None", this.platform);
				}
			}

			// Token: 0x04000312 RID: 786
			public RuntimePlatform platform;

			// Token: 0x04000313 RID: 787
			public EntryOverrideType entryOverrideType;

			// Token: 0x04000314 RID: 788
			public TableReference tableReference;

			// Token: 0x04000315 RID: 789
			public TableEntryReference tableEntryReference;
		}
	}
}
