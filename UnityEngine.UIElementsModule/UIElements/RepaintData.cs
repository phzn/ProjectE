using System;

namespace UnityEngine.UIElements
{
	// Token: 0x02000290 RID: 656
	internal class RepaintData
	{
		// Token: 0x170003C7 RID: 967
		// (get) Token: 0x06001244 RID: 4676 RVA: 0x000416DC File Offset: 0x0003F8DC
		// (set) Token: 0x06001245 RID: 4677 RVA: 0x000416E4 File Offset: 0x0003F8E4
		public Matrix4x4 currentOffset { get; set; } = Matrix4x4.identity;

		// Token: 0x170003C8 RID: 968
		// (get) Token: 0x06001246 RID: 4678 RVA: 0x000416ED File Offset: 0x0003F8ED
		// (set) Token: 0x06001247 RID: 4679 RVA: 0x000416F5 File Offset: 0x0003F8F5
		public Vector2 mousePosition { get; set; }

		// Token: 0x170003C9 RID: 969
		// (get) Token: 0x06001248 RID: 4680 RVA: 0x000416FE File Offset: 0x0003F8FE
		// (set) Token: 0x06001249 RID: 4681 RVA: 0x00041706 File Offset: 0x0003F906
		public Rect currentWorldClip { get; set; }

		// Token: 0x170003CA RID: 970
		// (get) Token: 0x0600124A RID: 4682 RVA: 0x0004170F File Offset: 0x0003F90F
		// (set) Token: 0x0600124B RID: 4683 RVA: 0x00041717 File Offset: 0x0003F917
		public Event repaintEvent { get; set; }
	}
}
