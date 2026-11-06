using System;
using UnityEngine;

namespace DA_Assets.FCU
{
	// Token: 0x02000042 RID: 66
	public struct ResolutionData
	{
		// Token: 0x170000AE RID: 174
		// (get) Token: 0x0600016E RID: 366 RVA: 0x00003E69 File Offset: 0x00002069
		// (set) Token: 0x0600016F RID: 367 RVA: 0x00003E71 File Offset: 0x00002071
		public bool GameViewSizeReceived { readonly get; set; }

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x06000170 RID: 368 RVA: 0x00003E7A File Offset: 0x0000207A
		// (set) Token: 0x06000171 RID: 369 RVA: 0x00003E82 File Offset: 0x00002082
		public Vector2 GameViewSize { readonly get; set; }
	}
}
