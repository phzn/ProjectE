using System;

namespace DA_Assets.Singleton
{
	// Token: 0x02000008 RID: 8
	[AttributeUsage(AttributeTargets.Class)]
	public class ResourcePathAttribute : Attribute
	{
		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000021 RID: 33 RVA: 0x000025B4 File Offset: 0x000007B4
		// (set) Token: 0x06000022 RID: 34 RVA: 0x000025BC File Offset: 0x000007BC
		public string[] Path { get; private set; }

		// Token: 0x06000023 RID: 35 RVA: 0x000025C5 File Offset: 0x000007C5
		public ResourcePathAttribute(params string[] path)
		{
			this.Path = path;
		}
	}
}
