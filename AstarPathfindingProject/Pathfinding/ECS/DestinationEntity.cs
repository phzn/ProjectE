using System;
using Unity.Entities;
using Unity.Properties;

namespace Pathfinding.ECS
{
	// Token: 0x020002F1 RID: 753
	[GeneratePropertyBag]
	[Serializable]
	public struct DestinationEntity : IComponentData, IQueryTypeParameter, IEnableableComponent
	{
		// Token: 0x04000DC0 RID: 3520
		public Entity destination;

		// Token: 0x04000DC1 RID: 3521
		public bool useRotation;
	}
}
