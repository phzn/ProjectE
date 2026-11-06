using System;

namespace UnityEngine.Rendering.Universal.Internal
{
	// Token: 0x02000116 RID: 278
	internal static class DeferredConfig
	{
		// Token: 0x1700020E RID: 526
		// (get) Token: 0x060008CA RID: 2250 RVA: 0x00038735 File Offset: 0x00036935
		// (set) Token: 0x060008CB RID: 2251 RVA: 0x0003873C File Offset: 0x0003693C
		internal static bool IsOpenGL { get; set; }

		// Token: 0x1700020F RID: 527
		// (get) Token: 0x060008CC RID: 2252 RVA: 0x00038744 File Offset: 0x00036944
		// (set) Token: 0x060008CD RID: 2253 RVA: 0x0003874B File Offset: 0x0003694B
		internal static bool IsDX10 { get; set; }
	}
}
