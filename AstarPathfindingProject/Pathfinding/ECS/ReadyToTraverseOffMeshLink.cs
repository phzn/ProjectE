using System;
using Unity.Entities;

namespace Pathfinding.ECS
{
	// Token: 0x02000304 RID: 772
	[Serializable]
	public struct ReadyToTraverseOffMeshLink : IComponentData, IQueryTypeParameter, IEnableableComponent
	{
	}
}
