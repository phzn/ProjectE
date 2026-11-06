using System;
using UnityEngine;

namespace DA_Assets.FCU.Drawers.CanvasDrawers
{
	// Token: 0x020003C2 RID: 962
	internal struct ScrollViewModel
	{
		// Token: 0x170004D0 RID: 1232
		// (get) Token: 0x060018AD RID: 6317 RVA: 0x0009D562 File Offset: 0x0009B762
		// (set) Token: 0x060018AE RID: 6318 RVA: 0x0009D56A File Offset: 0x0009B76A
		public GameObject Viewport { readonly get; set; }

		// Token: 0x170004D1 RID: 1233
		// (get) Token: 0x060018AF RID: 6319 RVA: 0x0009D573 File Offset: 0x0009B773
		// (set) Token: 0x060018B0 RID: 6320 RVA: 0x0009D57B File Offset: 0x0009B77B
		public GameObject Content { readonly get; set; }
	}
}
