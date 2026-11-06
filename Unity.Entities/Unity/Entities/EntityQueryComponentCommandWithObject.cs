using System;

namespace Unity.Entities
{
	// Token: 0x020000F0 RID: 240
	internal struct EntityQueryComponentCommandWithObject
	{
		// Token: 0x06000408 RID: 1032 RVA: 0x0001718F File Offset: 0x0001538F
		internal object GetBoxedObject()
		{
			if (this.GCNode.BoxedObject.IsAllocated)
			{
				return this.GCNode.BoxedObject.Target;
			}
			return null;
		}

		// Token: 0x04000276 RID: 630
		public EntityQueryComponentCommand Header;

		// Token: 0x04000277 RID: 631
		public int HashCode;

		// Token: 0x04000278 RID: 632
		public EntityComponentGCNode GCNode;
	}
}
