using System;

namespace Features.NeckSigns
{
	// Token: 0x02000F74 RID: 3956
	[Serializable]
	public sealed class NeckSignDesignData
	{
		// Token: 0x0600771C RID: 30492 RVA: 0x00306CAE File Offset: 0x00304EAE
		public NeckSignDesignData Clone()
		{
			return new NeckSignDesignData
			{
				partId = this.partId,
				pngBase64 = (this.pngBase64 ?? string.Empty)
			};
		}

		// Token: 0x04007A5E RID: 31326
		public string partId;

		// Token: 0x04007A5F RID: 31327
		public string pngBase64 = string.Empty;
	}
}
