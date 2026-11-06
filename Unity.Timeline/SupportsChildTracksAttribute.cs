using System;

namespace UnityEngine.Timeline
{
	// Token: 0x02000041 RID: 65
	[AttributeUsage(AttributeTargets.Class, Inherited = false)]
	internal class SupportsChildTracksAttribute : Attribute
	{
		// Token: 0x060002B6 RID: 694 RVA: 0x00009A9B File Offset: 0x00007C9B
		public SupportsChildTracksAttribute(Type childType = null, int levels = 2147483647)
		{
			this.childType = childType;
			this.levels = levels;
		}

		// Token: 0x040000F0 RID: 240
		public readonly Type childType;

		// Token: 0x040000F1 RID: 241
		public readonly int levels;
	}
}
