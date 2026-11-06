using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine.Localization.SmartFormat.Core.Extensions;
using UnityEngine.Localization.SmartFormat.Core.Parsing;

namespace UnityEngine.Localization.SmartFormat.Extensions
{
	// Token: 0x02000070 RID: 112
	[Serializable]
	public class IsMatchFormatter : FormatterBase, IFormatterLiteralExtractor
	{
		// Token: 0x060002E4 RID: 740 RVA: 0x0000B1AD File Offset: 0x000093AD
		public IsMatchFormatter()
		{
			base.Names = this.DefaultNames;
		}

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x060002E5 RID: 741 RVA: 0x0000B1C1 File Offset: 0x000093C1
		public override string[] DefaultNames
		{
			get
			{
				return new string[] { "ismatch" };
			}
		}

		// Token: 0x060002E6 RID: 742 RVA: 0x0000B1D4 File Offset: 0x000093D4
		public override bool TryEvaluateFormat(IFormattingInfo formattingInfo)
		{
			string formatterOptions = formattingInfo.FormatterOptions;
			IList<Format> list = formattingInfo.Format.Split('|');
			if (list.Count == 0)
			{
				return true;
			}
			if (list.Count != 2)
			{
				throw new FormatException("Exactly 2 format options are required.");
			}
			if (new Regex(formatterOptions, this.RegexOptions).IsMatch(formattingInfo.CurrentValue.ToString()))
			{
				formattingInfo.Write(list[0], formattingInfo.CurrentValue);
			}
			else if (list.Count == 2)
			{
				formattingInfo.Write(list[1], formattingInfo.CurrentValue);
			}
			return true;
		}

		// Token: 0x060002E7 RID: 743 RVA: 0x0000B264 File Offset: 0x00009464
		public void WriteAllLiterals(IFormattingInfo formattingInfo)
		{
			IList<Format> list = formattingInfo.Format.Split('|');
			if (list.Count == 0)
			{
				return;
			}
			if (list.Count != 2)
			{
				return;
			}
			formattingInfo.Write(list[0], formattingInfo.CurrentValue);
			formattingInfo.Write(list[1], formattingInfo.CurrentValue);
		}

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x060002E8 RID: 744 RVA: 0x0000B2B8 File Offset: 0x000094B8
		// (set) Token: 0x060002E9 RID: 745 RVA: 0x0000B2C0 File Offset: 0x000094C0
		public RegexOptions RegexOptions { get; set; }
	}
}
