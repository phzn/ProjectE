using System;
using Newtonsoft.Json;

namespace DA_Assets.FCU.Model
{
	// Token: 0x02000053 RID: 83
	public struct AuthResult
	{
		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x060001FB RID: 507 RVA: 0x00006F50 File Offset: 0x00005150
		// (set) Token: 0x060001FC RID: 508 RVA: 0x00006F58 File Offset: 0x00005158
		[JsonProperty("access_token")]
		public string AccessToken { readonly get; set; }

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x060001FD RID: 509 RVA: 0x00006F61 File Offset: 0x00005161
		// (set) Token: 0x060001FE RID: 510 RVA: 0x00006F69 File Offset: 0x00005169
		[JsonProperty("expires_in")]
		public string ExpiresIn { readonly get; set; }

		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x060001FF RID: 511 RVA: 0x00006F72 File Offset: 0x00005172
		// (set) Token: 0x06000200 RID: 512 RVA: 0x00006F7A File Offset: 0x0000517A
		[JsonProperty("refresh_token")]
		public string RefreshToken { readonly get; set; }
	}
}
