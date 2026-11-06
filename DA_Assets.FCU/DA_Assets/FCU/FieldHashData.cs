using System;

namespace DA_Assets.FCU
{
	// Token: 0x02000037 RID: 55
	public struct FieldHashData
	{
		// Token: 0x06000129 RID: 297 RVA: 0x00003B98 File Offset: 0x00001D98
		public FieldHashData(string name, object data)
		{
			this.name = name;
			this.data = data;
		}

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x0600012A RID: 298 RVA: 0x00003BA8 File Offset: 0x00001DA8
		public string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x0600012B RID: 299 RVA: 0x00003BB0 File Offset: 0x00001DB0
		public object Data
		{
			get
			{
				return this.data;
			}
		}

		// Token: 0x04000332 RID: 818
		private string name;

		// Token: 0x04000333 RID: 819
		private object data;
	}
}
