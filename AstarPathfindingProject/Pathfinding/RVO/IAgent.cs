using System;
using Pathfinding.Util;
using UnityEngine;

namespace Pathfinding.RVO
{
	// Token: 0x020002C0 RID: 704
	public interface IAgent
	{
		// Token: 0x17000282 RID: 642
		// (get) Token: 0x06001167 RID: 4455
		int AgentIndex { get; }

		// Token: 0x17000283 RID: 643
		// (get) Token: 0x06001168 RID: 4456
		// (set) Token: 0x06001169 RID: 4457
		Vector3 Position { get; set; }

		// Token: 0x17000284 RID: 644
		// (get) Token: 0x0600116A RID: 4458
		Vector3 CalculatedTargetPoint { get; }

		// Token: 0x17000285 RID: 645
		// (get) Token: 0x0600116B RID: 4459
		bool AvoidingAnyAgents { get; }

		// Token: 0x17000286 RID: 646
		// (get) Token: 0x0600116C RID: 4460
		float CalculatedSpeed { get; }

		// Token: 0x0600116D RID: 4461
		void SetTarget(Vector3 targetPoint, float desiredSpeed, float maxSpeed, Vector3 endOfPath);

		// Token: 0x17000287 RID: 647
		// (get) Token: 0x0600116E RID: 4462
		// (set) Token: 0x0600116F RID: 4463
		SimpleMovementPlane MovementPlane { get; set; }

		// Token: 0x17000288 RID: 648
		// (get) Token: 0x06001170 RID: 4464
		// (set) Token: 0x06001171 RID: 4465
		bool Locked { get; set; }

		// Token: 0x17000289 RID: 649
		// (get) Token: 0x06001172 RID: 4466
		// (set) Token: 0x06001173 RID: 4467
		float Radius { get; set; }

		// Token: 0x1700028A RID: 650
		// (get) Token: 0x06001174 RID: 4468
		// (set) Token: 0x06001175 RID: 4469
		float Height { get; set; }

		// Token: 0x1700028B RID: 651
		// (get) Token: 0x06001176 RID: 4470
		// (set) Token: 0x06001177 RID: 4471
		float AgentTimeHorizon { get; set; }

		// Token: 0x1700028C RID: 652
		// (get) Token: 0x06001178 RID: 4472
		// (set) Token: 0x06001179 RID: 4473
		float ObstacleTimeHorizon { get; set; }

		// Token: 0x1700028D RID: 653
		// (get) Token: 0x0600117A RID: 4474
		// (set) Token: 0x0600117B RID: 4475
		int MaxNeighbours { get; set; }

		// Token: 0x1700028E RID: 654
		// (get) Token: 0x0600117C RID: 4476
		int NeighbourCount { get; }

		// Token: 0x1700028F RID: 655
		// (get) Token: 0x0600117D RID: 4477
		// (set) Token: 0x0600117E RID: 4478
		RVOLayer Layer { get; set; }

		// Token: 0x17000290 RID: 656
		// (get) Token: 0x0600117F RID: 4479
		// (set) Token: 0x06001180 RID: 4480
		RVOLayer CollidesWith { get; set; }

		// Token: 0x17000291 RID: 657
		// (get) Token: 0x06001181 RID: 4481
		// (set) Token: 0x06001182 RID: 4482
		float FlowFollowingStrength { get; set; }

		// Token: 0x17000292 RID: 658
		// (get) Token: 0x06001183 RID: 4483
		// (set) Token: 0x06001184 RID: 4484
		AgentDebugFlags DebugFlags { get; set; }

		// Token: 0x17000293 RID: 659
		// (get) Token: 0x06001185 RID: 4485
		// (set) Token: 0x06001186 RID: 4486
		float Priority { get; set; }

		// Token: 0x17000294 RID: 660
		// (get) Token: 0x06001187 RID: 4487
		// (set) Token: 0x06001188 RID: 4488
		int HierarchicalNodeIndex { get; set; }

		// Token: 0x17000295 RID: 661
		// (set) Token: 0x06001189 RID: 4489
		Action PreCalculationCallback { set; }

		// Token: 0x17000296 RID: 662
		// (set) Token: 0x0600118A RID: 4490
		Action DestroyedCallback { set; }

		// Token: 0x0600118B RID: 4491
		void SetCollisionNormal(Vector3 normal);

		// Token: 0x0600118C RID: 4492
		void ForceSetVelocity(Vector3 velocity);

		// Token: 0x17000297 RID: 663
		// (get) Token: 0x0600118D RID: 4493
		ReachedEndOfPath CalculatedEffectivelyReachedDestination { get; }

		// Token: 0x0600118E RID: 4494
		void SetObstacleQuery(GraphNode sourceNode);
	}
}
