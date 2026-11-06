using System;
using DA_Assets.FCU.Model;

namespace DA_Assets.FCU
{
	// Token: 0x02000364 RID: 868
	public struct FigmaSessionItem
	{
		// Token: 0x1700043A RID: 1082
		// (get) Token: 0x060016B3 RID: 5811 RVA: 0x0009510E File Offset: 0x0009330E
		// (set) Token: 0x060016B4 RID: 5812 RVA: 0x00095116 File Offset: 0x00093316
		public AuthType AuthType { readonly get; set; }

		// Token: 0x1700043B RID: 1083
		// (get) Token: 0x060016B5 RID: 5813 RVA: 0x0009511F File Offset: 0x0009331F
		// (set) Token: 0x060016B6 RID: 5814 RVA: 0x00095127 File Offset: 0x00093327
		public AuthResult AuthResult { readonly get; set; }

		// Token: 0x1700043C RID: 1084
		// (get) Token: 0x060016B7 RID: 5815 RVA: 0x00095130 File Offset: 0x00093330
		// (set) Token: 0x060016B8 RID: 5816 RVA: 0x00095138 File Offset: 0x00093338
		public FigmaUser User { readonly get; set; }
	}
}
