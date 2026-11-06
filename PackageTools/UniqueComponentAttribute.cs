using System;

namespace Pathfinding
{
	// Token: 0x02000004 RID: 4
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	public class UniqueComponentAttribute : Attribute
	{
		// Token: 0x04000001 RID: 1
		public string tag;
	}
}
