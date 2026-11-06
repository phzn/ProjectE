using System;
using Unity.Entities;

namespace Unity.Transforms
{
	// Token: 0x02000019 RID: 25
	[InternalBufferCapacity(0)]
	[Serializable]
	public struct Child : ICleanupBufferElementData, IBufferElementData
	{
		// Token: 0x0400004D RID: 77
		public Entity Value;
	}
}
