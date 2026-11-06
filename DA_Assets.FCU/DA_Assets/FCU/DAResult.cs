using System;

namespace DA_Assets.FCU
{
	// Token: 0x0200002C RID: 44
	public struct DAResult<T1>
	{
		// Token: 0x17000049 RID: 73
		// (get) Token: 0x0600009F RID: 159 RVA: 0x000036B2 File Offset: 0x000018B2
		// (set) Token: 0x060000A0 RID: 160 RVA: 0x000036BA File Offset: 0x000018BA
		public bool Success
		{
			get
			{
				return this.success;
			}
			set
			{
				this.success = value;
			}
		}

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x060000A1 RID: 161 RVA: 0x000036C3 File Offset: 0x000018C3
		// (set) Token: 0x060000A2 RID: 162 RVA: 0x000036CB File Offset: 0x000018CB
		public T1 Object { readonly get; set; }

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x060000A3 RID: 163 RVA: 0x000036D4 File Offset: 0x000018D4
		// (set) Token: 0x060000A4 RID: 164 RVA: 0x000036DC File Offset: 0x000018DC
		public WebError Error { readonly get; set; }

		// Token: 0x040002FD RID: 765
		private bool success;
	}
}
