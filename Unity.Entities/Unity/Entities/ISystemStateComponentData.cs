using System;

namespace Unity.Entities
{
	// Token: 0x0200019D RID: 413
	[Obsolete("ISystemStateComponentData has been renamed to ICleanupComponentData. This old type has been kept for transition purposes, but it will not function correctly, so you should replace it with ICleanupComponentData immediately. (UnityUpgradable) -> ICleanupComponentData", true)]
	public interface ISystemStateComponentData : IComponentData, IQueryTypeParameter
	{
	}
}
