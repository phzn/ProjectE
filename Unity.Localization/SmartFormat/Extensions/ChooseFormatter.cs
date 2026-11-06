using System;
using System.Collections.Generic;
using UnityEngine.Localization.SmartFormat.Core.Extensions;
using UnityEngine.Localization.SmartFormat.Core.Parsing;

namespace UnityEngine.Localization.SmartFormat.Extensions
{
	// Token: 0x0200006A RID: 106
	[Serializable]
	public class ChooseFormatter : FormatterBase, IFormatterLiteralExtractor
	{
		// Token: 0x1700008C RID: 140
		// (get) Token: 0x060002CF RID: 719 RVA: 0x0000A62E File Offset: 0x0000882E
		// (set) Token: 0x060002D0 RID: 720 RVA: 0x0000A636 File Offset: 0x00008836
		public char SplitChar
		{
			get
			{
				return this.m_SplitChar;
			}
			set
			{
				this.m_SplitChar = value;
			}
		}

		// Token: 0x060002D1 RID: 721 RVA: 0x0000A63F File Offset: 0x0000883F
		public ChooseFormatter()
		{
			base.Names = this.DefaultNames;
		}

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x060002D2 RID: 722 RVA: 0x0000A65B File Offset: 0x0000885B
		public override string[] DefaultNames
		{
			get
			{
				return new string[] { "choose", "c" };
			}
		}

		// Token: 0x060002D3 RID: 723 RVA: 0x0000A674 File Offset: 0x00008874
		public override bool TryEvaluateFormat(IFormattingInfo formattingInfo)
		{
			if (formattingInfo.FormatterOptions == "")
			{
				return false;
			}
			string[] array = formattingInfo.FormatterOptions.Split(this.SplitChar, StringSplitOptions.None);
			IList<Format> list = formattingInfo.Format.Split(this.SplitChar);
			if (list.Count < 2)
			{
				return false;
			}
			Format format = ChooseFormatter.DetermineChosenFormat(formattingInfo, list, array);
			formattingInfo.Write(format, formattingInfo.CurrentValue);
			return true;
		}

		// Token: 0x060002D4 RID: 724 RVA: 0x0000A6DC File Offset: 0x000088DC
		private static Format DetermineChosenFormat(IFormattingInfo formattingInfo, IList<Format> choiceFormats, string[] chooseOptions)
		{
			object currentValue = formattingInfo.CurrentValue;
			string text = ((currentValue == null) ? "null" : currentValue.ToString());
			int num = Array.IndexOf<string>(chooseOptions, text);
			if (choiceFormats.Count < chooseOptions.Length)
			{
				throw formattingInfo.FormattingException("You must specify at least " + chooseOptions.Length.ToString() + " choices", null, -1);
			}
			if (choiceFormats.Count > chooseOptions.Length + 1)
			{
				throw formattingInfo.FormattingException("You cannot specify more than " + (chooseOptions.Length + 1).ToString() + " choices", null, -1);
			}
			if (num == -1 && choiceFormats.Count == chooseOptions.Length)
			{
				throw formattingInfo.FormattingException("\"" + text + "\" is not a valid choice, and a \"default\" choice was not supplied", null, -1);
			}
			if (num == -1)
			{
				num = choiceFormats.Count - 1;
			}
			return choiceFormats[num];
		}

		// Token: 0x060002D5 RID: 725 RVA: 0x0000A7A8 File Offset: 0x000089A8
		public void WriteAllLiterals(IFormattingInfo formattingInfo)
		{
			if (formattingInfo.FormatterOptions == "")
			{
				return;
			}
			IList<Format> list = formattingInfo.Format.Split(this.SplitChar);
			if (list.Count < 2)
			{
				return;
			}
			for (int i = 0; i < list.Count; i++)
			{
				formattingInfo.Write(list[i], null);
			}
		}

		// Token: 0x040000BD RID: 189
		[SerializeField]
		private char m_SplitChar = '|';
	}
}
