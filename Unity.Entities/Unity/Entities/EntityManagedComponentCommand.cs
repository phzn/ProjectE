using System;

namespace Unity.Entities
{
	// Token: 0x020000FD RID: 253
	internal struct EntityManagedComponentCommand
	{
		// Token: 0x0600040A RID: 1034 RVA: 0x000171DB File Offset: 0x000153DB
		internal object GetBoxedObject()
		{
			if (this.GCNode.BoxedObject.IsAllocated)
			{
				return this.GCNode.BoxedObject.Target;
			}
			return null;
		}

		// Token: 0x040002A1 RID: 673
		public EntityCommand Header;

		// Token: 0x040002A2 RID: 674
		public TypeIndex ComponentTypeIndex;

		// Token: 0x040002A3 RID: 675
		public EntityComponentGCNode GCNode;
	}
}
