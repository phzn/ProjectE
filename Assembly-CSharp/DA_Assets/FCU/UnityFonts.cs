using System;
using System.Collections.Generic;
using UnityEngine;

namespace DA_Assets.FCU
{
	// Token: 0x02000315 RID: 789
	[Serializable]
	public struct UnityFonts
	{
		// Token: 0x17000425 RID: 1061
		// (get) Token: 0x0600159F RID: 5535 RVA: 0x0008C002 File Offset: 0x0008A202
		// (set) Token: 0x060015A0 RID: 5536 RVA: 0x0008C00A File Offset: 0x0008A20A
		public List<FontStruct> Existing
		{
			get
			{
				return this.existing;
			}
			set
			{
				this.existing = value;
			}
		}

		// Token: 0x17000426 RID: 1062
		// (get) Token: 0x060015A1 RID: 5537 RVA: 0x0008C013 File Offset: 0x0008A213
		// (set) Token: 0x060015A2 RID: 5538 RVA: 0x0008C01B File Offset: 0x0008A21B
		public List<FontStruct> Missing
		{
			get
			{
				return this.missing;
			}
			set
			{
				this.missing = value;
			}
		}

		// Token: 0x0400173E RID: 5950
		[SerializeField]
		private List<FontStruct> missing;

		// Token: 0x0400173F RID: 5951
		[SerializeField]
		private List<FontStruct> existing;
	}
}
