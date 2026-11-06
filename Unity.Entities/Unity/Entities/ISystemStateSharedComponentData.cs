using System;

namespace Unity.Entities
{
	// Token: 0x020001A1 RID: 417
	[Obsolete("ISystemStateSharedComponentData has been renamed to ICleanupSharedComponentData. This old type has been kept for transition purposes, but it will not function correctly, so you should replace it with ICleanupSharedComponentData immediately. (UnityUpgradable) -> ICleanupSharedComponentData", true)]
	public interface ISystemStateSharedComponentData : IBufferElementData
	{
	}
}
