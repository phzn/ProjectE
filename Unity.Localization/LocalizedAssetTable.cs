using System;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;

namespace UnityEngine.Localization
{
	// Token: 0x02000014 RID: 20
	[Serializable]
	public class LocalizedAssetTable : LocalizedTable<AssetTable, AssetTableEntry>
	{
		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000077 RID: 119 RVA: 0x00002C24 File Offset: 0x00000E24
		protected override LocalizedDatabase<AssetTable, AssetTableEntry> Database
		{
			get
			{
				return LocalizationSettings.AssetDatabase;
			}
		}

		// Token: 0x06000078 RID: 120 RVA: 0x00002C2B File Offset: 0x00000E2B
		public LocalizedAssetTable()
		{
		}

		// Token: 0x06000079 RID: 121 RVA: 0x00002C33 File Offset: 0x00000E33
		public LocalizedAssetTable(TableReference tableReference)
		{
			base.TableReference = tableReference;
		}
	}
}
