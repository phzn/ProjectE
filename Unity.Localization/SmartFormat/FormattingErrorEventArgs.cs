using System;

namespace UnityEngine.Localization.SmartFormat
{
	// Token: 0x02000036 RID: 54
	public class FormattingErrorEventArgs : EventArgs
	{
		// Token: 0x060001F9 RID: 505 RVA: 0x00007406 File Offset: 0x00005606
		internal FormattingErrorEventArgs(string rawText, int errorIndex, bool ignoreError)
		{
			this.Placeholder = rawText;
			this.ErrorIndex = errorIndex;
			this.IgnoreError = ignoreError;
		}

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x060001FA RID: 506 RVA: 0x00007423 File Offset: 0x00005623
		// (set) Token: 0x060001FB RID: 507 RVA: 0x0000742B File Offset: 0x0000562B
		public string Placeholder { get; internal set; }

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x060001FC RID: 508 RVA: 0x00007434 File Offset: 0x00005634
		// (set) Token: 0x060001FD RID: 509 RVA: 0x0000743C File Offset: 0x0000563C
		public int ErrorIndex { get; internal set; }

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x060001FE RID: 510 RVA: 0x00007445 File Offset: 0x00005645
		// (set) Token: 0x060001FF RID: 511 RVA: 0x0000744D File Offset: 0x0000564D
		public bool IgnoreError { get; internal set; }
	}
}
