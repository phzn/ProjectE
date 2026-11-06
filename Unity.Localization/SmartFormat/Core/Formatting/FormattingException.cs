using System;
using UnityEngine.Localization.SmartFormat.Core.Parsing;

namespace UnityEngine.Localization.SmartFormat.Core.Formatting
{
	// Token: 0x0200008D RID: 141
	public class FormattingException : Exception
	{
		// Token: 0x060003BE RID: 958 RVA: 0x0000E410 File Offset: 0x0000C610
		public FormattingException(FormatItem errorItem, Exception formatException, int index)
		{
			this.Format = errorItem.baseString;
			this.ErrorItem = errorItem;
			this.Issue = formatException.Message;
			this.Index = index;
		}

		// Token: 0x060003BF RID: 959 RVA: 0x0000E43E File Offset: 0x0000C63E
		public FormattingException(FormatItem errorItem, string issue, int index)
		{
			this.Format = errorItem.baseString;
			this.ErrorItem = errorItem;
			this.Issue = issue;
			this.Index = index;
		}

		// Token: 0x170000CD RID: 205
		// (get) Token: 0x060003C0 RID: 960 RVA: 0x0000E467 File Offset: 0x0000C667
		public string Format { get; }

		// Token: 0x170000CE RID: 206
		// (get) Token: 0x060003C1 RID: 961 RVA: 0x0000E46F File Offset: 0x0000C66F
		public FormatItem ErrorItem { get; }

		// Token: 0x170000CF RID: 207
		// (get) Token: 0x060003C2 RID: 962 RVA: 0x0000E477 File Offset: 0x0000C677
		public string Issue { get; }

		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x060003C3 RID: 963 RVA: 0x0000E47F File Offset: 0x0000C67F
		public int Index { get; }

		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x060003C4 RID: 964 RVA: 0x0000E488 File Offset: 0x0000C688
		public override string Message
		{
			get
			{
				return string.Format("Error parsing format string: {0} at {1}\n{2}\n{3}", new object[]
				{
					this.Issue,
					this.Index,
					this.Format,
					new string('-', this.Index) + "^"
				});
			}
		}
	}
}
