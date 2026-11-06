using System;
using Newtonsoft.Json;

namespace DA_Assets.FCU.Model
{
	// Token: 0x02000060 RID: 96
	public struct FillGeometry
	{
		// Token: 0x17000133 RID: 307
		// (get) Token: 0x060002DB RID: 731 RVA: 0x000076AF File Offset: 0x000058AF
		// (set) Token: 0x060002DC RID: 732 RVA: 0x000076B7 File Offset: 0x000058B7
		[JsonProperty("path")]
		public string Path { readonly get; set; }
	}
}
