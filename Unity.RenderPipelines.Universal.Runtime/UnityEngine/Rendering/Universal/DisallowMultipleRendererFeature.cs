using System;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x020000C6 RID: 198
	[AttributeUsage(AttributeTargets.Class)]
	public class DisallowMultipleRendererFeature : Attribute
	{
		// Token: 0x17000189 RID: 393
		// (get) Token: 0x0600062D RID: 1581 RVA: 0x00025DE9 File Offset: 0x00023FE9
		// (set) Token: 0x0600062C RID: 1580 RVA: 0x00025DE0 File Offset: 0x00023FE0
		public string customTitle { get; private set; }

		// Token: 0x0600062E RID: 1582 RVA: 0x00025DF1 File Offset: 0x00023FF1
		public DisallowMultipleRendererFeature(string customTitle = null)
		{
			this.customTitle = customTitle;
		}
	}
}
