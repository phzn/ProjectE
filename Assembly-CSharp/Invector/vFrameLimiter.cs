using System;
using UnityEngine;

namespace Invector
{
	// Token: 0x02000499 RID: 1177
	[vClassHeader("Frame Limiter", false, "icon_v2", false, "")]
	public class vFrameLimiter : vMonoBehaviour
	{
		// Token: 0x06001CEC RID: 7404 RVA: 0x000B2C34 File Offset: 0x000B0E34
		private void Awake()
		{
			Application.targetFrameRate = this.desiredFPS;
			QualitySettings.vSyncCount = 0;
		}

		// Token: 0x04001E0B RID: 7691
		public int desiredFPS = 60;
	}
}
