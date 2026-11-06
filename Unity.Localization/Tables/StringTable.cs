using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.SmartFormat;

namespace UnityEngine.Localization.Tables
{
	// Token: 0x02000032 RID: 50
	public class StringTable : DetailedLocalizationTable<StringTableEntry>
	{
		// Token: 0x060001C1 RID: 449 RVA: 0x000069C8 File Offset: 0x00004BC8
		public string GenerateCharacterSet()
		{
			return string.Concat<char>(from c in this.CollectLiteralCharacters().Distinct<char>()
				orderby c
				select c);
		}

		// Token: 0x060001C2 RID: 450 RVA: 0x00006A00 File Offset: 0x00004C00
		internal IEnumerable<char> CollectLiteralCharacters()
		{
			IEnumerable<char> enumerable = "";
			LocalizedStringDatabase stringDatabase = LocalizationSettings.StringDatabase;
			SmartFormatterLiteralCharacterExtractor smartFormatterLiteralCharacterExtractor = new SmartFormatterLiteralCharacterExtractor((stringDatabase != null) ? stringDatabase.SmartFormatter : null);
			foreach (StringTableEntry stringTableEntry in base.Values)
			{
				if (stringTableEntry.IsSmart)
				{
					enumerable = enumerable.Concat<char>(smartFormatterLiteralCharacterExtractor.ExtractLiteralsCharacters(stringTableEntry.LocalizedValue));
				}
				else
				{
					enumerable = enumerable.Concat<char>(stringTableEntry.LocalizedValue.AsEnumerable<char>());
				}
			}
			return enumerable;
		}

		// Token: 0x060001C3 RID: 451 RVA: 0x00006A94 File Offset: 0x00004C94
		public override StringTableEntry CreateTableEntry()
		{
			return new StringTableEntry
			{
				Table = this,
				Data = new TableEntryData()
			};
		}
	}
}
