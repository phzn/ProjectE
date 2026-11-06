using System;
using Newtonsoft.Json;

namespace DA_Assets.FCU.Model
{
	// Token: 0x02000068 RID: 104
	public struct ExportSetting
	{
		// Token: 0x17000146 RID: 326
		// (get) Token: 0x06000301 RID: 769 RVA: 0x000077F2 File Offset: 0x000059F2
		// (set) Token: 0x06000302 RID: 770 RVA: 0x000077FA File Offset: 0x000059FA
		[JsonProperty("suffix")]
		public string Suffix { readonly get; set; }

		// Token: 0x17000147 RID: 327
		// (get) Token: 0x06000303 RID: 771 RVA: 0x00007803 File Offset: 0x00005A03
		// (set) Token: 0x06000304 RID: 772 RVA: 0x0000780B File Offset: 0x00005A0B
		[JsonProperty("format")]
		public string Format { readonly get; set; }

		// Token: 0x17000148 RID: 328
		// (get) Token: 0x06000305 RID: 773 RVA: 0x00007814 File Offset: 0x00005A14
		// (set) Token: 0x06000306 RID: 774 RVA: 0x0000781C File Offset: 0x00005A1C
		[JsonProperty("constraint")]
		public Constraint Constraint { readonly get; set; }
	}
}
