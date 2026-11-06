using System;
using UnityEngine.Localization.SmartFormat.Core.Extensions;

namespace UnityEngine.Localization.SmartFormat.Extensions
{
	// Token: 0x02000076 RID: 118
	[Serializable]
	public class SubStringFormatter : FormatterBase
	{
		// Token: 0x1700009D RID: 157
		// (get) Token: 0x0600031C RID: 796 RVA: 0x0000C159 File Offset: 0x0000A359
		// (set) Token: 0x0600031D RID: 797 RVA: 0x0000C161 File Offset: 0x0000A361
		public SubStringFormatter.SubStringOutOfRangeBehavior OutOfRangeBehavior
		{
			get
			{
				return this.m_OutOfRangeBehavior;
			}
			set
			{
				this.m_OutOfRangeBehavior = value;
			}
		}

		// Token: 0x0600031E RID: 798 RVA: 0x0000C16A File Offset: 0x0000A36A
		public SubStringFormatter()
		{
			base.Names = this.DefaultNames;
		}

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x0600031F RID: 799 RVA: 0x0000C191 File Offset: 0x0000A391
		public override string[] DefaultNames
		{
			get
			{
				return new string[] { "substr" };
			}
		}

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x06000320 RID: 800 RVA: 0x0000C1A1 File Offset: 0x0000A3A1
		// (set) Token: 0x06000321 RID: 801 RVA: 0x0000C1A9 File Offset: 0x0000A3A9
		public char ParameterDelimiter
		{
			get
			{
				return this.m_ParameterDelimiter;
			}
			set
			{
				this.m_ParameterDelimiter = value;
			}
		}

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x06000322 RID: 802 RVA: 0x0000C1B2 File Offset: 0x0000A3B2
		// (set) Token: 0x06000323 RID: 803 RVA: 0x0000C1BA File Offset: 0x0000A3BA
		public string NullDisplayString
		{
			get
			{
				return this.m_NullDisplayString;
			}
			set
			{
				this.m_NullDisplayString = value;
			}
		}

		// Token: 0x06000324 RID: 804 RVA: 0x0000C1C4 File Offset: 0x0000A3C4
		public override bool TryEvaluateFormat(IFormattingInfo formattingInfo)
		{
			if (formattingInfo.FormatterOptions == string.Empty)
			{
				return false;
			}
			string[] array = formattingInfo.FormatterOptions.Split(this.ParameterDelimiter, StringSplitOptions.None);
			object currentValue = formattingInfo.CurrentValue;
			string text = ((currentValue != null) ? currentValue.ToString() : null);
			if (text == null)
			{
				formattingInfo.Write(this.NullDisplayString);
				return true;
			}
			int num = int.Parse(array[0]);
			int num2 = ((array.Length > 1) ? int.Parse(array[1]) : 0);
			if (num < 0)
			{
				num = text.Length + num;
			}
			if (num > text.Length)
			{
				num = text.Length;
			}
			if (num2 < 0)
			{
				num2 = text.Length - num + num2;
			}
			SubStringFormatter.SubStringOutOfRangeBehavior outOfRangeBehavior = this.OutOfRangeBehavior;
			if (outOfRangeBehavior != SubStringFormatter.SubStringOutOfRangeBehavior.ReturnEmptyString)
			{
				if (outOfRangeBehavior == SubStringFormatter.SubStringOutOfRangeBehavior.ReturnStartIndexToEndOfString)
				{
					if (num > text.Length)
					{
						num = text.Length;
					}
					if (num + num2 > text.Length)
					{
						num2 = text.Length - num;
					}
				}
			}
			else if (num + num2 > text.Length)
			{
				num2 = 0;
			}
			string text2 = ((array.Length > 1) ? text.Substring(num, num2) : text.Substring(num));
			formattingInfo.Write(text2);
			return true;
		}

		// Token: 0x040000CB RID: 203
		[SerializeField]
		private char m_ParameterDelimiter = ',';

		// Token: 0x040000CC RID: 204
		[SerializeField]
		private string m_NullDisplayString = "(null)";

		// Token: 0x040000CD RID: 205
		[SerializeField]
		private SubStringFormatter.SubStringOutOfRangeBehavior m_OutOfRangeBehavior;

		// Token: 0x0200013B RID: 315
		public enum SubStringOutOfRangeBehavior
		{
			// Token: 0x040002CC RID: 716
			ReturnEmptyString,
			// Token: 0x040002CD RID: 717
			ReturnStartIndexToEndOfString,
			// Token: 0x040002CE RID: 718
			ThrowException
		}
	}
}
