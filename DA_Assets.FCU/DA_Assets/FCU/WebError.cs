using System;
using Newtonsoft.Json;

namespace DA_Assets.FCU
{
	// Token: 0x02000030 RID: 48
	public struct WebError
	{
		// Token: 0x060000BF RID: 191 RVA: 0x000037A0 File Offset: 0x000019A0
		public WebError(int status = 0, string message = null, Exception ex = null)
		{
			this.Status = status;
			this.Message = message;
			this.Exception = ex;
		}

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x060000C0 RID: 192 RVA: 0x000037B7 File Offset: 0x000019B7
		// (set) Token: 0x060000C1 RID: 193 RVA: 0x000037BF File Offset: 0x000019BF
		public Exception Exception { readonly get; set; }

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x060000C2 RID: 194 RVA: 0x000037C8 File Offset: 0x000019C8
		// (set) Token: 0x060000C3 RID: 195 RVA: 0x000037D0 File Offset: 0x000019D0
		[JsonProperty("status")]
		public int Status { readonly get; set; }

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x060000C4 RID: 196 RVA: 0x000037D9 File Offset: 0x000019D9
		// (set) Token: 0x060000C5 RID: 197 RVA: 0x000037E1 File Offset: 0x000019E1
		[JsonProperty("err")]
		public string Message { readonly get; set; }
	}
}
