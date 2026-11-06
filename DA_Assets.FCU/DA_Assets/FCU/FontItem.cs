using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace DA_Assets.FCU
{
	// Token: 0x0200002E RID: 46
	public struct FontItem
	{
		// Token: 0x1700004C RID: 76
		// (get) Token: 0x060000A9 RID: 169 RVA: 0x000036E5 File Offset: 0x000018E5
		// (set) Token: 0x060000AA RID: 170 RVA: 0x000036ED File Offset: 0x000018ED
		[JsonProperty("family")]
		public string Family { readonly get; set; }

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x060000AB RID: 171 RVA: 0x000036F6 File Offset: 0x000018F6
		// (set) Token: 0x060000AC RID: 172 RVA: 0x000036FE File Offset: 0x000018FE
		[JsonProperty("variants")]
		public List<string> Variants { readonly get; set; }

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x060000AD RID: 173 RVA: 0x00003707 File Offset: 0x00001907
		// (set) Token: 0x060000AE RID: 174 RVA: 0x0000370F File Offset: 0x0000190F
		[JsonProperty("subsets")]
		public List<string> Subsets { readonly get; set; }

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x060000AF RID: 175 RVA: 0x00003718 File Offset: 0x00001918
		// (set) Token: 0x060000B0 RID: 176 RVA: 0x00003720 File Offset: 0x00001920
		[JsonProperty("version")]
		public string Version { readonly get; set; }

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x060000B1 RID: 177 RVA: 0x00003729 File Offset: 0x00001929
		// (set) Token: 0x060000B2 RID: 178 RVA: 0x00003731 File Offset: 0x00001931
		[JsonProperty("lastModified")]
		public string LastModified { readonly get; set; }

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x060000B3 RID: 179 RVA: 0x0000373A File Offset: 0x0000193A
		// (set) Token: 0x060000B4 RID: 180 RVA: 0x00003742 File Offset: 0x00001942
		[JsonProperty("files")]
		public Dictionary<string, string> Files { readonly get; set; }

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x060000B5 RID: 181 RVA: 0x0000374B File Offset: 0x0000194B
		// (set) Token: 0x060000B6 RID: 182 RVA: 0x00003753 File Offset: 0x00001953
		[JsonProperty("category")]
		public string Category { readonly get; set; }

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x060000B7 RID: 183 RVA: 0x0000375C File Offset: 0x0000195C
		// (set) Token: 0x060000B8 RID: 184 RVA: 0x00003764 File Offset: 0x00001964
		[JsonProperty("kind")]
		public string Kind { readonly get; set; }

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x060000B9 RID: 185 RVA: 0x0000376D File Offset: 0x0000196D
		// (set) Token: 0x060000BA RID: 186 RVA: 0x00003775 File Offset: 0x00001975
		[JsonProperty("menu")]
		public string Menu { readonly get; set; }
	}
}
