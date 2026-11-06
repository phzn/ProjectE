using System;

namespace Unity.Entities
{
	// Token: 0x02000355 RID: 853
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
	public sealed class WorldSystemFilterAttribute : Attribute
	{
		// Token: 0x060016DD RID: 5853 RVA: 0x00056C37 File Offset: 0x00054E37
		public WorldSystemFilterAttribute(WorldSystemFilterFlags flags, WorldSystemFilterFlags childDefaultFlags = WorldSystemFilterFlags.Default)
		{
			this.FilterFlags = flags;
			this.ChildDefaultFilterFlags = childDefaultFlags;
		}

		// Token: 0x040008A2 RID: 2210
		public WorldSystemFilterFlags FilterFlags;

		// Token: 0x040008A3 RID: 2211
		public WorldSystemFilterFlags ChildDefaultFilterFlags;
	}
}
