using System;
using Newtonsoft.Json;

namespace DA_Assets.FCU.Model
{
	// Token: 0x02000067 RID: 103
	public struct Constraint
	{
		// Token: 0x17000144 RID: 324
		// (get) Token: 0x060002FD RID: 765 RVA: 0x000077D0 File Offset: 0x000059D0
		// (set) Token: 0x060002FE RID: 766 RVA: 0x000077D8 File Offset: 0x000059D8
		[JsonProperty("type")]
		public ConstraintType Type { readonly get; set; }

		// Token: 0x17000145 RID: 325
		// (get) Token: 0x060002FF RID: 767 RVA: 0x000077E1 File Offset: 0x000059E1
		// (set) Token: 0x06000300 RID: 768 RVA: 0x000077E9 File Offset: 0x000059E9
		[JsonProperty("value")]
		public float Value { readonly get; set; }
	}
}
