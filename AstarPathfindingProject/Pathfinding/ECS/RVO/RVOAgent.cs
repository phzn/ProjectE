using System;
using Pathfinding.RVO;
using Unity.Entities;
using UnityEngine;

namespace Pathfinding.ECS.RVO
{
	// Token: 0x020003B3 RID: 947
	[Serializable]
	public struct RVOAgent : IComponentData, IQueryTypeParameter
	{
		// Token: 0x04000FC9 RID: 4041
		[Tooltip("How far into the future to look for collisions with other agents (in seconds)")]
		public float agentTimeHorizon;

		// Token: 0x04000FCA RID: 4042
		[Tooltip("How far into the future to look for collisions with obstacles (in seconds)")]
		public float obstacleTimeHorizon;

		// Token: 0x04000FCB RID: 4043
		[Tooltip("Max number of other agents to take into account.\nA smaller value can reduce CPU load, a higher value can lead to better local avoidance quality.")]
		public int maxNeighbours;

		// Token: 0x04000FCC RID: 4044
		public RVOLayer layer;

		// Token: 0x04000FCD RID: 4045
		[EnumFlag]
		public RVOLayer collidesWith;

		// Token: 0x04000FCE RID: 4046
		[Tooltip("How strongly other agents will avoid this agent")]
		[Range(0f, 1f)]
		public float priority;

		// Token: 0x04000FCF RID: 4047
		[NonSerialized]
		public float priorityMultiplier;

		// Token: 0x04000FD0 RID: 4048
		[NonSerialized]
		public float flowFollowingStrength;

		// Token: 0x04000FD1 RID: 4049
		public AgentDebugFlags debug;

		// Token: 0x04000FD2 RID: 4050
		[Tooltip("A locked unit cannot move. Other units will still avoid it. But avoidance quality is not the best")]
		public bool locked;

		// Token: 0x04000FD3 RID: 4051
		public static readonly RVOAgent Default = new RVOAgent
		{
			locked = false,
			agentTimeHorizon = 1f,
			obstacleTimeHorizon = 0.5f,
			maxNeighbours = 10,
			layer = RVOLayer.DefaultAgent,
			collidesWith = (RVOLayer)(-1),
			priority = 0.5f,
			priorityMultiplier = 1f,
			flowFollowingStrength = 0f,
			debug = AgentDebugFlags.Nothing
		};
	}
}
