using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Localization.SmartFormat.Core.Extensions;
using UnityEngine.Localization.SmartFormat.Core.Formatting;
using UnityEngine.Localization.SmartFormat.Core.Parsing;

namespace UnityEngine.Localization.SmartFormat
{
	// Token: 0x0200003A RID: 58
	internal class SmartFormatterLiteralCharacterExtractor : SmartFormatter
	{
		// Token: 0x06000230 RID: 560 RVA: 0x00007EA8 File Offset: 0x000060A8
		public SmartFormatterLiteralCharacterExtractor(SmartFormatter parent)
		{
			base.Settings = parent.Settings;
			base.Parser = parent.Parser;
			base.SourceExtensions.AddRange(parent.SourceExtensions);
			base.FormatterExtensions.AddRange(parent.FormatterExtensions);
		}

		// Token: 0x06000231 RID: 561 RVA: 0x00007EF5 File Offset: 0x000060F5
		public IEnumerable<char> ExtractLiteralsCharacters(string value)
		{
			this.m_Characters = "";
			base.Format(value, null);
			return this.m_Characters;
		}

		// Token: 0x06000232 RID: 562 RVA: 0x00007F14 File Offset: 0x00006114
		public override void Format(FormattingInfo formattingInfo)
		{
			foreach (FormatItem formatItem in formattingInfo.Format.Items)
			{
				if (formatItem is LiteralText)
				{
					this.m_Characters = this.m_Characters.Concat<char>(formatItem.ToEnumerable());
				}
				else
				{
					Placeholder placeholder = (Placeholder)formatItem;
					FormattingInfo formattingInfo2 = formattingInfo.CreateChild(placeholder);
					string formatterName = formattingInfo2.Placeholder.FormatterName;
					foreach (IFormatter formatter in base.FormatterExtensions)
					{
						IFormatterLiteralExtractor formatterLiteralExtractor = formatter as IFormatterLiteralExtractor;
						if (formatterLiteralExtractor != null && formatter.Names.Contains(formatterName))
						{
							formatterLiteralExtractor.WriteAllLiterals(formattingInfo2);
						}
					}
				}
			}
		}

		// Token: 0x04000081 RID: 129
		private IEnumerable<char> m_Characters;
	}
}
