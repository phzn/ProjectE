using System;
using UnityEngine.Scripting.APIUpdating;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x02000038 RID: 56
	[AddComponentMenu("Rendering/2D/Composite Shadow Caster 2D")]
	[MovedFrom("UnityEngine.Experimental.Rendering.Universal")]
	[ExecuteInEditMode]
	public class CompositeShadowCaster2D : ShadowCasterGroup2D
	{
		// Token: 0x0600022E RID: 558 RVA: 0x00011925 File Offset: 0x0000FB25
		protected void OnEnable()
		{
			ShadowCasterGroup2DManager.AddGroup(this);
		}

		// Token: 0x0600022F RID: 559 RVA: 0x0001192D File Offset: 0x0000FB2D
		protected void OnDisable()
		{
			ShadowCasterGroup2DManager.RemoveGroup(this);
		}
	}
}
