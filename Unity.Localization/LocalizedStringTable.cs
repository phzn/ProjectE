using System;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;

namespace UnityEngine.Localization
{
	// Token: 0x02000019 RID: 25
	[Serializable]
	public class LocalizedStringTable : LocalizedTable<StringTable, StringTableEntry>
	{
		// Token: 0x17000026 RID: 38
		// (get) Token: 0x060000D5 RID: 213 RVA: 0x0000406D File Offset: 0x0000226D
		protected override LocalizedDatabase<StringTable, StringTableEntry> Database
		{
			get
			{
				return LocalizationSettings.StringDatabase;
			}
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x00004074 File Offset: 0x00002274
		public LocalizedStringTable()
		{
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x0000407C File Offset: 0x0000227C
		public LocalizedStringTable(TableReference tableReference)
		{
			base.TableReference = tableReference;
		}
	}
}
