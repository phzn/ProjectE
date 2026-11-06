using System;
using DA_Assets.DAI;
using UnityEngine;

namespace DA_Assets.FCU
{
	// Token: 0x020002BE RID: 702
	[Serializable]
	public class DelegateHolder : MonoBehaviourLinkerRuntime<FigmaConverterUnity>
	{
		// Token: 0x170003C8 RID: 968
		// (get) Token: 0x060013BE RID: 5054 RVA: 0x0007F55F File Offset: 0x0007D75F
		// (set) Token: 0x060013BF RID: 5055 RVA: 0x0007F567 File Offset: 0x0007D767
		public Action<PreImportInput, Action<PreImportOutput>> ShowDifferenceChecker { get; set; }

		// Token: 0x170003C9 RID: 969
		// (get) Token: 0x060013C0 RID: 5056 RVA: 0x0007F570 File Offset: 0x0007D770
		// (set) Token: 0x060013C1 RID: 5057 RVA: 0x0007F578 File Offset: 0x0007D778
		public Func<Vector2, bool> SetGameViewSize { get; set; }

		// Token: 0x170003CA RID: 970
		// (get) Token: 0x060013C2 RID: 5058 RVA: 0x0007F581 File Offset: 0x0007D781
		// (set) Token: 0x060013C3 RID: 5059 RVA: 0x0007F589 File Offset: 0x0007D789
		public Action<Sprite, Vector4> SetSpriteRects { get; set; }

		// Token: 0x170003CB RID: 971
		// (get) Token: 0x060013C4 RID: 5060 RVA: 0x0007F592 File Offset: 0x0007D792
		// (set) Token: 0x060013C5 RID: 5061 RVA: 0x0007F59A File Offset: 0x0007D79A
		public Action UpdateScrollContent { get; set; }
	}
}
