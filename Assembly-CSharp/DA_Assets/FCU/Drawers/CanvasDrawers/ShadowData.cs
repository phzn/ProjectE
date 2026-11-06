using System;
using DA_Assets.FCU.Model;
using UnityEngine;

namespace DA_Assets.FCU.Drawers.CanvasDrawers
{
	// Token: 0x020003C4 RID: 964
	internal struct ShadowData
	{
		// Token: 0x170004D2 RID: 1234
		// (get) Token: 0x060018B5 RID: 6325 RVA: 0x0009D650 File Offset: 0x0009B850
		// (set) Token: 0x060018B6 RID: 6326 RVA: 0x0009D658 File Offset: 0x0009B858
		public EffectType EffectType { readonly get; set; }

		// Token: 0x170004D3 RID: 1235
		// (get) Token: 0x060018B7 RID: 6327 RVA: 0x0009D661 File Offset: 0x0009B861
		// (set) Token: 0x060018B8 RID: 6328 RVA: 0x0009D669 File Offset: 0x0009B869
		public Vector2 Offset { readonly get; set; }

		// Token: 0x170004D4 RID: 1236
		// (get) Token: 0x060018B9 RID: 6329 RVA: 0x0009D672 File Offset: 0x0009B872
		// (set) Token: 0x060018BA RID: 6330 RVA: 0x0009D67A File Offset: 0x0009B87A
		public float Angle { readonly get; set; }

		// Token: 0x170004D5 RID: 1237
		// (get) Token: 0x060018BB RID: 6331 RVA: 0x0009D683 File Offset: 0x0009B883
		// (set) Token: 0x060018BC RID: 6332 RVA: 0x0009D68B File Offset: 0x0009B88B
		public float Distance { readonly get; set; }

		// Token: 0x170004D6 RID: 1238
		// (get) Token: 0x060018BD RID: 6333 RVA: 0x0009D694 File Offset: 0x0009B894
		// (set) Token: 0x060018BE RID: 6334 RVA: 0x0009D69C File Offset: 0x0009B89C
		public float Spread { readonly get; set; }

		// Token: 0x170004D7 RID: 1239
		// (get) Token: 0x060018BF RID: 6335 RVA: 0x0009D6A5 File Offset: 0x0009B8A5
		// (set) Token: 0x060018C0 RID: 6336 RVA: 0x0009D6AD File Offset: 0x0009B8AD
		public Color Color { readonly get; set; }

		// Token: 0x170004D8 RID: 1240
		// (get) Token: 0x060018C1 RID: 6337 RVA: 0x0009D6B6 File Offset: 0x0009B8B6
		// (set) Token: 0x060018C2 RID: 6338 RVA: 0x0009D6BE File Offset: 0x0009B8BE
		public float Radius { readonly get; set; }
	}
}
