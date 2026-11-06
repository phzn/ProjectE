using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace DA_Assets.FCU.Model
{
	// Token: 0x02000056 RID: 86
	public struct FigmaProject
	{
		// Token: 0x170000FE RID: 254
		// (get) Token: 0x06000271 RID: 625 RVA: 0x0000733B File Offset: 0x0000553B
		// (set) Token: 0x06000272 RID: 626 RVA: 0x00007343 File Offset: 0x00005543
		[JsonProperty("document")]
		public FObject Document { readonly get; set; }

		// Token: 0x170000FF RID: 255
		// (get) Token: 0x06000273 RID: 627 RVA: 0x0000734C File Offset: 0x0000554C
		// (set) Token: 0x06000274 RID: 628 RVA: 0x00007354 File Offset: 0x00005554
		[JsonProperty("name")]
		public string Name { readonly get; set; }

		// Token: 0x17000100 RID: 256
		// (get) Token: 0x06000275 RID: 629 RVA: 0x0000735D File Offset: 0x0000555D
		// (set) Token: 0x06000276 RID: 630 RVA: 0x00007365 File Offset: 0x00005565
		[JsonProperty("components")]
		public Dictionary<string, FigmaComponent> Components { readonly get; set; }

		// Token: 0x17000101 RID: 257
		// (get) Token: 0x06000277 RID: 631 RVA: 0x0000736E File Offset: 0x0000556E
		// (set) Token: 0x06000278 RID: 632 RVA: 0x00007376 File Offset: 0x00005576
		[JsonProperty("nodes")]
		public Dictionary<string, FigmaProject> Nodes { readonly get; set; }
	}
}
