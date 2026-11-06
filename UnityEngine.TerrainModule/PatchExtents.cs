using System;
using UnityEngine.Scripting;

namespace UnityEngine
{
	// Token: 0x02000013 RID: 19
	[UsedByNativeCode]
	public struct PatchExtents
	{
		// Token: 0x17000059 RID: 89
		// (get) Token: 0x060000E1 RID: 225 RVA: 0x00002FCC File Offset: 0x000011CC
		// (set) Token: 0x060000E2 RID: 226 RVA: 0x00002FE4 File Offset: 0x000011E4
		public float min
		{
			get
			{
				return this.m_min;
			}
			set
			{
				this.m_min = value;
			}
		}

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x060000E3 RID: 227 RVA: 0x00002FF0 File Offset: 0x000011F0
		// (set) Token: 0x060000E4 RID: 228 RVA: 0x00003008 File Offset: 0x00001208
		public float max
		{
			get
			{
				return this.m_max;
			}
			set
			{
				this.m_max = value;
			}
		}

		// Token: 0x0400004D RID: 77
		internal float m_min;

		// Token: 0x0400004E RID: 78
		internal float m_max;
	}
}
