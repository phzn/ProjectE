using System;

namespace Unity.Entities
{
	// Token: 0x02000199 RID: 409
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
	public class MaximumChunkCapacityAttribute : Attribute
	{
		// Token: 0x06000A11 RID: 2577 RVA: 0x00032177 File Offset: 0x00030377
		public MaximumChunkCapacityAttribute(int capacity)
		{
			this.Capacity = capacity;
		}

		// Token: 0x040004C3 RID: 1219
		public readonly int Capacity;
	}
}
