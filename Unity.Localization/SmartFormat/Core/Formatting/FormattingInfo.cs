using System;
using System.Collections.Generic;
using UnityEngine.Localization.SmartFormat.Core.Extensions;
using UnityEngine.Localization.SmartFormat.Core.Parsing;

namespace UnityEngine.Localization.SmartFormat.Core.Formatting
{
	// Token: 0x0200008E RID: 142
	public class FormattingInfo : IFormattingInfo, ISelectorInfo
	{
		// Token: 0x060003C5 RID: 965 RVA: 0x0000E4DF File Offset: 0x0000C6DF
		public void Init(FormatDetails formatDetails, Format format, object currentValue)
		{
			this.Init(null, formatDetails, format, currentValue);
		}

		// Token: 0x060003C6 RID: 966 RVA: 0x0000E4EB File Offset: 0x0000C6EB
		public void Init(FormattingInfo parent, FormatDetails formatDetails, Format format, object currentValue)
		{
			this.Parent = parent;
			this.CurrentValue = currentValue;
			this.Format = format;
			this.FormatDetails = formatDetails;
		}

		// Token: 0x060003C7 RID: 967 RVA: 0x0000E50A File Offset: 0x0000C70A
		public void Init(FormattingInfo parent, FormatDetails formatDetails, Placeholder placeholder, object currentValue)
		{
			this.Parent = parent;
			this.FormatDetails = formatDetails;
			this.Placeholder = placeholder;
			this.Format = placeholder.Format;
			this.CurrentValue = currentValue;
		}

		// Token: 0x060003C8 RID: 968 RVA: 0x0000E538 File Offset: 0x0000C738
		public void ReleaseToPool()
		{
			this.Parent = null;
			this.FormatDetails = null;
			this.Placeholder = null;
			this.Format = null;
			this.CurrentValue = null;
			foreach (FormattingInfo formattingInfo in this.Children)
			{
				FormattingInfoPool.Release(formattingInfo);
			}
			this.Children.Clear();
		}

		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x060003C9 RID: 969 RVA: 0x0000E5B8 File Offset: 0x0000C7B8
		// (set) Token: 0x060003CA RID: 970 RVA: 0x0000E5C0 File Offset: 0x0000C7C0
		public FormattingInfo Parent { get; private set; }

		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x060003CB RID: 971 RVA: 0x0000E5C9 File Offset: 0x0000C7C9
		// (set) Token: 0x060003CC RID: 972 RVA: 0x0000E5D1 File Offset: 0x0000C7D1
		public Selector Selector { get; set; }

		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x060003CD RID: 973 RVA: 0x0000E5DA File Offset: 0x0000C7DA
		// (set) Token: 0x060003CE RID: 974 RVA: 0x0000E5E2 File Offset: 0x0000C7E2
		public FormatDetails FormatDetails { get; private set; }

		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x060003CF RID: 975 RVA: 0x0000E5EB File Offset: 0x0000C7EB
		// (set) Token: 0x060003D0 RID: 976 RVA: 0x0000E5F3 File Offset: 0x0000C7F3
		public object CurrentValue { get; set; }

		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x060003D1 RID: 977 RVA: 0x0000E5FC File Offset: 0x0000C7FC
		// (set) Token: 0x060003D2 RID: 978 RVA: 0x0000E604 File Offset: 0x0000C804
		public Placeholder Placeholder { get; private set; }

		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x060003D3 RID: 979 RVA: 0x0000E60D File Offset: 0x0000C80D
		public int Alignment
		{
			get
			{
				return this.Placeholder.Alignment;
			}
		}

		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x060003D4 RID: 980 RVA: 0x0000E61A File Offset: 0x0000C81A
		public string FormatterOptions
		{
			get
			{
				return this.Placeholder.FormatterOptions;
			}
		}

		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x060003D5 RID: 981 RVA: 0x0000E627 File Offset: 0x0000C827
		// (set) Token: 0x060003D6 RID: 982 RVA: 0x0000E62F File Offset: 0x0000C82F
		public Format Format { get; private set; }

		// Token: 0x170000DA RID: 218
		// (get) Token: 0x060003D7 RID: 983 RVA: 0x0000E638 File Offset: 0x0000C838
		public List<FormattingInfo> Children { get; } = new List<FormattingInfo>();

		// Token: 0x060003D8 RID: 984 RVA: 0x0000E640 File Offset: 0x0000C840
		public void Write(string text)
		{
			this.FormatDetails.Output.Write(text, this);
		}

		// Token: 0x060003D9 RID: 985 RVA: 0x0000E654 File Offset: 0x0000C854
		public void Write(string text, int startIndex, int length)
		{
			this.FormatDetails.Output.Write(text, startIndex, length, this);
		}

		// Token: 0x060003DA RID: 986 RVA: 0x0000E66C File Offset: 0x0000C86C
		public void Write(Format format, object value)
		{
			FormattingInfo formattingInfo = this.CreateChild(format, value);
			this.FormatDetails.Formatter.Format(formattingInfo);
		}

		// Token: 0x060003DB RID: 987 RVA: 0x0000E693 File Offset: 0x0000C893
		public FormattingException FormattingException(string issue, FormatItem problemItem = null, int startIndex = -1)
		{
			if (problemItem == null)
			{
				problemItem = this.Format;
			}
			if (startIndex == -1)
			{
				startIndex = problemItem.startIndex;
			}
			return new FormattingException(problemItem, issue, startIndex);
		}

		// Token: 0x170000DB RID: 219
		// (get) Token: 0x060003DC RID: 988 RVA: 0x0000E6B4 File Offset: 0x0000C8B4
		public string SelectorText
		{
			get
			{
				return this.Selector.RawText;
			}
		}

		// Token: 0x170000DC RID: 220
		// (get) Token: 0x060003DD RID: 989 RVA: 0x0000E6C1 File Offset: 0x0000C8C1
		public int SelectorIndex
		{
			get
			{
				return this.Selector.SelectorIndex;
			}
		}

		// Token: 0x170000DD RID: 221
		// (get) Token: 0x060003DE RID: 990 RVA: 0x0000E6CE File Offset: 0x0000C8CE
		public string SelectorOperator
		{
			get
			{
				return this.Selector.Operator;
			}
		}

		// Token: 0x170000DE RID: 222
		// (get) Token: 0x060003DF RID: 991 RVA: 0x0000E6DB File Offset: 0x0000C8DB
		// (set) Token: 0x060003E0 RID: 992 RVA: 0x0000E6E3 File Offset: 0x0000C8E3
		public object Result { get; set; }

		// Token: 0x060003E1 RID: 993 RVA: 0x0000E6EC File Offset: 0x0000C8EC
		private FormattingInfo CreateChild(Format format, object currentValue)
		{
			FormattingInfo formattingInfo = FormattingInfoPool.Get(this, this.FormatDetails, format, currentValue);
			this.Children.Add(formattingInfo);
			return formattingInfo;
		}

		// Token: 0x060003E2 RID: 994 RVA: 0x0000E718 File Offset: 0x0000C918
		public FormattingInfo CreateChild(Placeholder placeholder)
		{
			FormattingInfo formattingInfo = FormattingInfoPool.Get(this, this.FormatDetails, placeholder, this.CurrentValue);
			this.Children.Add(formattingInfo);
			return formattingInfo;
		}
	}
}
