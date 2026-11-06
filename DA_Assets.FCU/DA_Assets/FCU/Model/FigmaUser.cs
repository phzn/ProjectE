using System;
using Newtonsoft.Json;

namespace DA_Assets.FCU.Model
{
	// Token: 0x0200007C RID: 124
	[Serializable]
	public struct FigmaUser
	{
		// Token: 0x1700015A RID: 346
		// (get) Token: 0x06000329 RID: 809 RVA: 0x00007946 File Offset: 0x00005B46
		// (set) Token: 0x0600032A RID: 810 RVA: 0x0000794E File Offset: 0x00005B4E
		[JsonProperty("id")]
		public string Id { readonly get; set; }

		// Token: 0x1700015B RID: 347
		// (get) Token: 0x0600032B RID: 811 RVA: 0x00007957 File Offset: 0x00005B57
		// (set) Token: 0x0600032C RID: 812 RVA: 0x0000795F File Offset: 0x00005B5F
		[JsonProperty("handle")]
		public string Name { readonly get; set; }

		// Token: 0x1700015C RID: 348
		// (get) Token: 0x0600032D RID: 813 RVA: 0x00007968 File Offset: 0x00005B68
		// (set) Token: 0x0600032E RID: 814 RVA: 0x00007970 File Offset: 0x00005B70
		[JsonProperty("email")]
		public string Email { readonly get; set; }

		// Token: 0x1700015D RID: 349
		// (get) Token: 0x0600032F RID: 815 RVA: 0x00007979 File Offset: 0x00005B79
		// (set) Token: 0x06000330 RID: 816 RVA: 0x00007981 File Offset: 0x00005B81
		[JsonProperty("img_url")]
		public string ImgUrl { readonly get; set; }
	}
}
