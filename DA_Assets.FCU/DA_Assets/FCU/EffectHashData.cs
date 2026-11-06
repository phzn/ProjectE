using System;
using System.Collections.Generic;

namespace DA_Assets.FCU
{
	// Token: 0x02000038 RID: 56
	public struct EffectHashData
	{
		// Token: 0x0600012C RID: 300 RVA: 0x00003BB8 File Offset: 0x00001DB8
		public EffectHashData(string name, List<FieldHashData> data)
		{
			this.name = name;
			this.data = data;
		}

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x0600012D RID: 301 RVA: 0x00003BC8 File Offset: 0x00001DC8
		public string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x0600012E RID: 302 RVA: 0x00003BD0 File Offset: 0x00001DD0
		public List<FieldHashData> Data
		{
			get
			{
				return this.data;
			}
		}

		// Token: 0x04000334 RID: 820
		private string name;

		// Token: 0x04000335 RID: 821
		private List<FieldHashData> data;
	}
}
