using System;

namespace Unity.Entities
{
	// Token: 0x020000F4 RID: 244
	internal struct MultipleEntitiesComponentCommandWithObject
	{
		// Token: 0x06000409 RID: 1033 RVA: 0x000171B5 File Offset: 0x000153B5
		internal object GetBoxedObject()
		{
			if (this.GCNode.BoxedObject.IsAllocated)
			{
				return this.GCNode.BoxedObject.Target;
			}
			return null;
		}

		// Token: 0x04000288 RID: 648
		public MultipleEntitiesCommand Header;

		// Token: 0x04000289 RID: 649
		public TypeIndex ComponentTypeIndex;

		// Token: 0x0400028A RID: 650
		public int HashCode;

		// Token: 0x0400028B RID: 651
		public EntityComponentGCNode GCNode;
	}
}
