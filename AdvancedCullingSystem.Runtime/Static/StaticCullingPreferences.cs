using System;
using UnityEngine;

namespace NGS.AdvancedCullingSystem.Static
{
	// Token: 0x02000022 RID: 34
	public static class StaticCullingPreferences
	{
		// Token: 0x17000023 RID: 35
		// (get) Token: 0x060000C7 RID: 199 RVA: 0x0000560D File Offset: 0x0000380D
		public static string LayerName
		{
			get
			{
				return "ACSCulling";
			}
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x060000C8 RID: 200 RVA: 0x00005614 File Offset: 0x00003814
		public static int Layer
		{
			get
			{
				return LayerMask.NameToLayer(StaticCullingPreferences.LayerName);
			}
		}
	}
}
