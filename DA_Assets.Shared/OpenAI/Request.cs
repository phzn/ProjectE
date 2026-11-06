using System;

namespace DA_Assets.OpenAI
{
	// Token: 0x02000020 RID: 32
	[Serializable]
	public struct Request
	{
		// Token: 0x04000047 RID: 71
		public string model;

		// Token: 0x04000048 RID: 72
		public RequestMessage[] messages;
	}
}
