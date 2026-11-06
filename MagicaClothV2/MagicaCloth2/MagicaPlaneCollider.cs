using System;
using UnityEngine;

namespace MagicaCloth2
{
	// Token: 0x02000026 RID: 38
	[AddComponentMenu("MagicaCloth2/MagicaPlaneCollider")]
	[HelpURL("https://magicasoft.jp/en/mc2_planecollidercomponent/")]
	public class MagicaPlaneCollider : ColliderComponent
	{
		// Token: 0x060000B8 RID: 184 RVA: 0x000079E8 File Offset: 0x00005BE8
		public override ColliderManager.ColliderType GetColliderType()
		{
			return ColliderManager.ColliderType.Plane;
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x0000706D File Offset: 0x0000526D
		public override void DataValidate()
		{
		}
	}
}
