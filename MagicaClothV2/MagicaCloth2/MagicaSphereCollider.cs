using System;
using UnityEngine;

namespace MagicaCloth2
{
	// Token: 0x02000027 RID: 39
	[AddComponentMenu("MagicaCloth2/MagicaSphereCollider")]
	[HelpURL("https://magicasoft.jp/en/mc2_spherecollidercomponent/")]
	public class MagicaSphereCollider : ColliderComponent
	{
		// Token: 0x060000BB RID: 187 RVA: 0x0000706A File Offset: 0x0000526A
		public override ColliderManager.ColliderType GetColliderType()
		{
			return ColliderManager.ColliderType.Sphere;
		}

		// Token: 0x060000BC RID: 188 RVA: 0x000079F3 File Offset: 0x00005BF3
		public override void DataValidate()
		{
			this.size.x = Mathf.Max(this.size.x, 0.001f);
		}

		// Token: 0x060000BD RID: 189 RVA: 0x00007A15 File Offset: 0x00005C15
		public void SetSize(float radius)
		{
			base.SetSize(new Vector3(radius, 0f, 0f));
		}
	}
}
