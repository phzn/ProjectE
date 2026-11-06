using System;

namespace Unity.Entities
{
	// Token: 0x02000198 RID: 408
	[AttributeUsage(AttributeTargets.Struct)]
	public class InternalBufferCapacityAttribute : Attribute
	{
		// Token: 0x06000A10 RID: 2576 RVA: 0x00032168 File Offset: 0x00030368
		public InternalBufferCapacityAttribute(int capacity)
		{
			this.Capacity = capacity;
		}

		// Token: 0x040004C2 RID: 1218
		public readonly int Capacity;
	}
}
