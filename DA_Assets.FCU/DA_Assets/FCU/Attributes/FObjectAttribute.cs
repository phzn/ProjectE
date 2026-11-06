using System;

namespace DA_Assets.FCU.Attributes
{
	// Token: 0x02000051 RID: 81
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Field, AllowMultiple = false)]
	public class FObjectAttribute : Attribute
	{
		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x060001F7 RID: 503 RVA: 0x00006F22 File Offset: 0x00005122
		public string Name { get; }

		// Token: 0x060001F8 RID: 504 RVA: 0x00006F2A File Offset: 0x0000512A
		public FObjectAttribute(string name)
		{
			this.Name = name;
		}
	}
}
