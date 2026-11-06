using System;

namespace Unity.Entities
{
	// Token: 0x02000100 RID: 256
	internal struct EntitySharedComponentCommand
	{
		// Token: 0x0600040B RID: 1035 RVA: 0x00017201 File Offset: 0x00015401
		internal object GetBoxedObject()
		{
			if (this.GCNode.BoxedObject.IsAllocated)
			{
				return this.GCNode.BoxedObject.Target;
			}
			return null;
		}

		// Token: 0x040002AC RID: 684
		public EntityCommand Header;

		// Token: 0x040002AD RID: 685
		public TypeIndex ComponentTypeIndex;

		// Token: 0x040002AE RID: 686
		public int HashCode;

		// Token: 0x040002AF RID: 687
		public EntityComponentGCNode GCNode;
	}
}
