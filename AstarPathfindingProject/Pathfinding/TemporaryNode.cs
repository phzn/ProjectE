using System;

namespace Pathfinding
{
	// Token: 0x020000BE RID: 190
	public struct TemporaryNode
	{
		// Token: 0x04000408 RID: 1032
		public uint associatedNode;

		// Token: 0x04000409 RID: 1033
		public Int3 position;

		// Token: 0x0400040A RID: 1034
		public int targetIndex;

		// Token: 0x0400040B RID: 1035
		public TemporaryNodeType type;
	}
}
