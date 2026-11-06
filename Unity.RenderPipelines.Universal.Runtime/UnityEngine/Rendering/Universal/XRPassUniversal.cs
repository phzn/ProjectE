using System;
using UnityEngine.Experimental.Rendering;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x02000106 RID: 262
	internal class XRPassUniversal : XRPass
	{
		// Token: 0x0600083D RID: 2109 RVA: 0x0003443F File Offset: 0x0003263F
		public static XRPass Create(XRPassCreateInfo createInfo)
		{
			XRPassUniversal xrpassUniversal = GenericPool<XRPassUniversal>.Get();
			xrpassUniversal.InitBase(createInfo);
			xrpassUniversal.isLateLatchEnabled = false;
			xrpassUniversal.canMarkLateLatch = false;
			xrpassUniversal.hasMarkedLateLatch = false;
			return xrpassUniversal;
		}

		// Token: 0x0600083E RID: 2110 RVA: 0x00034462 File Offset: 0x00032662
		public override void Release()
		{
			GenericPool<XRPassUniversal>.Release(this);
		}

		// Token: 0x17000203 RID: 515
		// (get) Token: 0x0600083F RID: 2111 RVA: 0x0003446A File Offset: 0x0003266A
		// (set) Token: 0x06000840 RID: 2112 RVA: 0x00034472 File Offset: 0x00032672
		internal bool isLateLatchEnabled { get; set; }

		// Token: 0x17000204 RID: 516
		// (get) Token: 0x06000841 RID: 2113 RVA: 0x0003447B File Offset: 0x0003267B
		// (set) Token: 0x06000842 RID: 2114 RVA: 0x00034483 File Offset: 0x00032683
		internal bool canMarkLateLatch { get; set; }

		// Token: 0x17000205 RID: 517
		// (get) Token: 0x06000843 RID: 2115 RVA: 0x0003448C File Offset: 0x0003268C
		// (set) Token: 0x06000844 RID: 2116 RVA: 0x00034494 File Offset: 0x00032694
		internal bool hasMarkedLateLatch { get; set; }
	}
}
