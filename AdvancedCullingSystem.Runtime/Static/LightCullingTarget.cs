using System;
using UnityEngine;

namespace NGS.AdvancedCullingSystem.Static
{
	// Token: 0x02000010 RID: 16
	public class LightCullingTarget : CullingTarget
	{
		// Token: 0x06000052 RID: 82 RVA: 0x00003069 File Offset: 0x00001269
		private void Awake()
		{
			this._light = base.GetComponent<Light>();
		}

		// Token: 0x06000053 RID: 83 RVA: 0x00003077 File Offset: 0x00001277
		protected override void MakeVisible()
		{
			this._light.enabled = true;
		}

		// Token: 0x06000054 RID: 84 RVA: 0x00003085 File Offset: 0x00001285
		protected override void MakeInvisible()
		{
			this._light.enabled = false;
		}

		// Token: 0x0400001D RID: 29
		private Light _light;
	}
}
