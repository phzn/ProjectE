using System;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;

namespace Pathfinding.ECS
{
	// Token: 0x0200034F RID: 847
	public struct JobStartOffMeshLinkTransition
	{
		// Token: 0x06001531 RID: 5425 RVA: 0x00079917 File Offset: 0x00077B17
		public static IEnumerable<object> DefaultOnTraverseOffMeshLink(AgentOffMeshLinkTraversalContext ctx)
		{
			OffMeshLinks.OffMeshLinkTracer linkInfo = ctx.link;
			float3 @float = ctx.movementPlane.ToWorld(float2.zero, 1f);
			float3 float2 = ctx.movementPlane.ToWorld(ctx.movementPlane.ToPlane(linkInfo.relativeEnd - linkInfo.relativeStart), 0f);
			quaternion rot = quaternion.LookRotationSafe(float2, @float);
			while (!ctx.MoveTowards(linkInfo.relativeStart, rot, true, false).reached)
			{
				yield return null;
			}
			ctx.DisableLocalAvoidance();
			while (!ctx.MoveTowards(linkInfo.relativeEnd, rot, true, false).reached)
			{
				yield return null;
			}
			yield break;
		}

		// Token: 0x04000ED0 RID: 3792
		public EntityCommandBuffer commandBuffer;
	}
}
