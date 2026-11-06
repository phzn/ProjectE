using System;

namespace Unity.Entities
{
	// Token: 0x020000FC RID: 252
	internal struct EntityBufferCommand
	{
		// Token: 0x0400029C RID: 668
		public EntityCommand Header;

		// Token: 0x0400029D RID: 669
		public TypeIndex ComponentTypeIndex;

		// Token: 0x0400029E RID: 670
		public short ComponentSize;

		// Token: 0x0400029F RID: 671
		public byte ValueRequiresEntityFixup;

		// Token: 0x040002A0 RID: 672
		public BufferHeaderNode BufferNode;
	}
}
