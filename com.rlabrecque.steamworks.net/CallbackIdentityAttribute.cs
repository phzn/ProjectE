using System;

namespace Steamworks
{
	// Token: 0x02000178 RID: 376
	[AttributeUsage(AttributeTargets.Struct, AllowMultiple = false)]
	internal class CallbackIdentityAttribute : Attribute
	{
		// Token: 0x17000026 RID: 38
		// (get) Token: 0x060008C4 RID: 2244 RVA: 0x0000D09B File Offset: 0x0000B29B
		// (set) Token: 0x060008C5 RID: 2245 RVA: 0x0000D0A3 File Offset: 0x0000B2A3
		public int Identity { get; set; }

		// Token: 0x060008C6 RID: 2246 RVA: 0x0000D0AC File Offset: 0x0000B2AC
		public CallbackIdentityAttribute(int callbackNum)
		{
			this.Identity = callbackNum;
		}
	}
}
