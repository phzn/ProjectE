using System;
using System.Collections.Generic;
using UnityEngine.Localization.SmartFormat.Core.Output;
using UnityEngine.Localization.SmartFormat.Core.Parsing;
using UnityEngine.Localization.SmartFormat.Core.Settings;

namespace UnityEngine.Localization.SmartFormat.Core.Formatting
{
	// Token: 0x0200008C RID: 140
	public class FormatDetails
	{
		// Token: 0x060003AC RID: 940 RVA: 0x0000E322 File Offset: 0x0000C522
		public void Init(SmartFormatter formatter, Format originalFormat, IList<object> originalArgs, FormatCache formatCache, IFormatProvider provider, IOutput output)
		{
			this.Formatter = formatter;
			this.OriginalFormat = originalFormat;
			this.OriginalArgs = originalArgs;
			this.FormatCache = formatCache;
			this.Provider = provider;
			this.Output = output;
		}

		// Token: 0x060003AD RID: 941 RVA: 0x0000E351 File Offset: 0x0000C551
		internal void Clear()
		{
			this.Formatter = null;
			this.OriginalFormat = null;
			this.OriginalArgs = null;
			this.FormatCache = null;
			this.Provider = null;
			this.Output = null;
			this.FormattingException = null;
		}

		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x060003AE RID: 942 RVA: 0x0000E384 File Offset: 0x0000C584
		// (set) Token: 0x060003AF RID: 943 RVA: 0x0000E38C File Offset: 0x0000C58C
		public SmartFormatter Formatter { get; private set; }

		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x060003B0 RID: 944 RVA: 0x0000E395 File Offset: 0x0000C595
		// (set) Token: 0x060003B1 RID: 945 RVA: 0x0000E39D File Offset: 0x0000C59D
		public Format OriginalFormat { get; private set; }

		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x060003B2 RID: 946 RVA: 0x0000E3A6 File Offset: 0x0000C5A6
		// (set) Token: 0x060003B3 RID: 947 RVA: 0x0000E3AE File Offset: 0x0000C5AE
		public IList<object> OriginalArgs { get; private set; }

		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x060003B4 RID: 948 RVA: 0x0000E3B7 File Offset: 0x0000C5B7
		// (set) Token: 0x060003B5 RID: 949 RVA: 0x0000E3BF File Offset: 0x0000C5BF
		public FormatCache FormatCache { get; private set; }

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x060003B6 RID: 950 RVA: 0x0000E3C8 File Offset: 0x0000C5C8
		// (set) Token: 0x060003B7 RID: 951 RVA: 0x0000E3D0 File Offset: 0x0000C5D0
		public IFormatProvider Provider { get; private set; }

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x060003B8 RID: 952 RVA: 0x0000E3D9 File Offset: 0x0000C5D9
		// (set) Token: 0x060003B9 RID: 953 RVA: 0x0000E3E1 File Offset: 0x0000C5E1
		public IOutput Output { get; private set; }

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x060003BA RID: 954 RVA: 0x0000E3EA File Offset: 0x0000C5EA
		// (set) Token: 0x060003BB RID: 955 RVA: 0x0000E3F2 File Offset: 0x0000C5F2
		public FormattingException FormattingException { get; set; }

		// Token: 0x170000CC RID: 204
		// (get) Token: 0x060003BC RID: 956 RVA: 0x0000E3FB File Offset: 0x0000C5FB
		public SmartSettings Settings
		{
			get
			{
				return this.Formatter.Settings;
			}
		}
	}
}
