using System;
using Newtonsoft.Json;

namespace DA_Assets.FCU.Model
{
	// Token: 0x0200006C RID: 108
	public struct FlowStartingPoint
	{
		// Token: 0x17000156 RID: 342
		// (get) Token: 0x06000321 RID: 801 RVA: 0x00007902 File Offset: 0x00005B02
		// (set) Token: 0x06000322 RID: 802 RVA: 0x0000790A File Offset: 0x00005B0A
		[JsonProperty("nodeId")]
		public string NodeId { readonly get; set; }

		// Token: 0x17000157 RID: 343
		// (get) Token: 0x06000323 RID: 803 RVA: 0x00007913 File Offset: 0x00005B13
		// (set) Token: 0x06000324 RID: 804 RVA: 0x0000791B File Offset: 0x00005B1B
		[JsonProperty("name")]
		public string Name { readonly get; set; }
	}
}
