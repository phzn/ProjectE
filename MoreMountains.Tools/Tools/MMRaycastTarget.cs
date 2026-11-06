using System;
using UnityEngine;
using UnityEngine.UI;

namespace MoreMountains.Tools
{
	// Token: 0x02000054 RID: 84
	[AddComponentMenu("More Mountains/Tools/GUI/MMRaycastTarget")]
	public class MMRaycastTarget : Graphic
	{
		// Token: 0x060001D5 RID: 469 RVA: 0x0000938D File Offset: 0x0000758D
		public override void SetVerticesDirty()
		{
		}

		// Token: 0x060001D6 RID: 470 RVA: 0x0000938F File Offset: 0x0000758F
		public override void SetMaterialDirty()
		{
		}

		// Token: 0x060001D7 RID: 471 RVA: 0x00009391 File Offset: 0x00007591
		protected override void OnPopulateMesh(VertexHelper vh)
		{
			vh.Clear();
		}
	}
}
