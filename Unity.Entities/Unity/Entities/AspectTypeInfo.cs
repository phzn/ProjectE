using System;
using Unity.Collections.LowLevel.Unsafe;

namespace Unity.Entities
{
	// Token: 0x020002E2 RID: 738
	internal struct AspectTypeInfo : IDisposable
	{
		// Token: 0x06001418 RID: 5144 RVA: 0x0004D878 File Offset: 0x0004BA78
		public void Dispose()
		{
			if (this.AspectTypes.IsCreated)
			{
				this.AspectTypes.Dispose();
			}
			if (this.AspectRequiredComponents.IsCreated)
			{
				this.AspectRequiredComponents.Dispose();
			}
			if (this.AspectExcludedComponents.IsCreated)
			{
				this.AspectExcludedComponents.Dispose();
			}
		}

		// Token: 0x04000759 RID: 1881
		public UnsafeList<AspectType> AspectTypes;

		// Token: 0x0400075A RID: 1882
		public UnsafeParallelMultiHashMap<AspectType, ComponentType> AspectRequiredComponents;

		// Token: 0x0400075B RID: 1883
		public UnsafeParallelMultiHashMap<AspectType, ComponentType> AspectExcludedComponents;
	}
}
