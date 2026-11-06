using System;

namespace Unity.Entities
{
	// Token: 0x020001A6 RID: 422
	[InternalBufferCapacity(0)]
	public struct LinkedEntityGroup : IBufferElementData
	{
		// Token: 0x06000A13 RID: 2579 RVA: 0x00032188 File Offset: 0x00030388
		public static implicit operator LinkedEntityGroup(Entity e)
		{
			return new LinkedEntityGroup
			{
				Value = e
			};
		}

		// Token: 0x040004C4 RID: 1220
		public Entity Value;
	}
}
