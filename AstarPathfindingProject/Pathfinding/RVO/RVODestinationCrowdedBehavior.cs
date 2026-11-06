using System;
using Pathfinding.Jobs;
using Unity.Burst;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

namespace Pathfinding.RVO
{
	// Token: 0x020002AD RID: 685
	[Serializable]
	public struct RVODestinationCrowdedBehavior
	{
		// Token: 0x06001117 RID: 4375 RVA: 0x00066C88 File Offset: 0x00064E88
		public void ReadJobResult(ref RVODestinationCrowdedBehavior.JobDensityCheck jobResult, int index)
		{
			bool flag = jobResult.outThresholdResult[index];
			this.progressAverage = jobResult.progressAverage[index];
			this.lastJobDensityResult = flag;
			this.shouldStopDelayTimer = Mathf.Lerp(this.shouldStopDelayTimer, (float)(flag ? 1 : 0), Time.deltaTime);
			flag = flag && this.shouldStopDelayTimer > 0.1f;
			this.lastShouldStopResult = flag;
			this.lastShouldStopDestination = jobResult.data[index].agentDestination;
		}

		// Token: 0x06001118 RID: 4376 RVA: 0x00066D10 File Offset: 0x00064F10
		public RVODestinationCrowdedBehavior(bool enabled, float densityFraction, bool returnAfterBeingPushedAway)
		{
			this.wasEnabled = enabled;
			this.enabled = enabled;
			this.densityThreshold = densityFraction;
			this.returnAfterBeingPushedAway = returnAfterBeingPushedAway;
			this.lastJobDensityResult = false;
			this.progressAverage = 0f;
			this.wasStopped = false;
			this.lastShouldStopDestination = new Vector3(float.NaN, float.NaN, float.NaN);
			this.reachedDestinationPoint = new Vector3(float.NaN, float.NaN, float.NaN);
			this.timer1 = 0f;
			this.shouldStopDelayTimer = 0f;
			this.reachedDestination = false;
			this.lastShouldStopResult = false;
		}

		// Token: 0x06001119 RID: 4377 RVA: 0x00066DAC File Offset: 0x00064FAC
		public void ClearDestinationReached()
		{
			this.wasStopped = false;
			this.progressAverage = 1f;
			this.reachedDestination = false;
		}

		// Token: 0x0600111A RID: 4378 RVA: 0x00066DC7 File Offset: 0x00064FC7
		public void OnDestinationChanged(Vector3 newDestination, bool reachedDestination)
		{
			this.timer1 = float.PositiveInfinity;
			this.reachedDestination = reachedDestination;
		}

		// Token: 0x17000277 RID: 631
		// (get) Token: 0x0600111B RID: 4379 RVA: 0x00066DDB File Offset: 0x00064FDB
		// (set) Token: 0x0600111C RID: 4380 RVA: 0x00066DE3 File Offset: 0x00064FE3
		public bool reachedDestination { readonly get; private set; }

		// Token: 0x0600111D RID: 4381 RVA: 0x00066DEC File Offset: 0x00064FEC
		public void Update(bool rvoControllerEnabled, bool reachedDestination, ref bool isStopped, ref float rvoPriorityMultiplier, ref float rvoFlowFollowingStrength, Vector3 agentPosition)
		{
			if (!this.enabled || !rvoControllerEnabled)
			{
				if (this.wasEnabled)
				{
					this.wasEnabled = false;
					rvoPriorityMultiplier = 1f;
					rvoFlowFollowingStrength = 0f;
					this.timer1 = float.PositiveInfinity;
					this.progressAverage = 1f;
				}
				return;
			}
			this.wasEnabled = true;
			if (reachedDestination)
			{
				float sqrMagnitude = (agentPosition - this.reachedDestinationPoint).sqrMagnitude;
				if ((this.lastShouldStopDestination - this.reachedDestinationPoint).sqrMagnitude > sqrMagnitude)
				{
					this.reachedDestination = false;
				}
			}
			if (reachedDestination || this.lastShouldStopResult)
			{
				this.timer1 = 0f;
				this.reachedDestination = true;
				this.reachedDestinationPoint = this.lastShouldStopDestination;
				rvoPriorityMultiplier = Mathf.Lerp(rvoPriorityMultiplier, 0.1f, Time.deltaTime * 2f);
				rvoFlowFollowingStrength = Mathf.Lerp(rvoFlowFollowingStrength, 1f, Time.deltaTime * 4f);
				this.wasStopped |= math.abs(this.progressAverage) < 0.1f;
				isStopped |= this.wasStopped;
				return;
			}
			if (isStopped)
			{
				this.timer1 = 0f;
				this.reachedDestination = false;
				rvoPriorityMultiplier = Mathf.Lerp(rvoPriorityMultiplier, 0.1f, Time.deltaTime * 2f);
				rvoFlowFollowingStrength = Mathf.Lerp(rvoFlowFollowingStrength, 1f, Time.deltaTime * 4f);
				this.wasStopped |= math.abs(this.progressAverage) < 0.1f;
				return;
			}
			if (!this.reachedDestination)
			{
				rvoPriorityMultiplier = Mathf.Lerp(rvoPriorityMultiplier, 1f, Time.deltaTime * 4f);
				rvoFlowFollowingStrength = 0f;
				isStopped = false;
				this.wasStopped = false;
				return;
			}
			this.timer1 += Time.deltaTime;
			if (this.timer1 > 3f && this.returnAfterBeingPushedAway)
			{
				rvoPriorityMultiplier = Mathf.Lerp(rvoPriorityMultiplier, 0.5f, Time.deltaTime * 2f);
				rvoFlowFollowingStrength = 0f;
				isStopped = false;
				this.wasStopped = false;
				return;
			}
			rvoPriorityMultiplier = Mathf.Lerp(rvoPriorityMultiplier, 0.1f, Time.deltaTime * 2f);
			rvoFlowFollowingStrength = Mathf.Lerp(rvoFlowFollowingStrength, 1f, Time.deltaTime * 4f);
			this.wasStopped |= math.abs(this.progressAverage) < 0.1f;
			isStopped = this.wasStopped;
		}

		// Token: 0x04000C45 RID: 3141
		public bool enabled;

		// Token: 0x04000C46 RID: 3142
		[Range(0f, 1f)]
		public float densityThreshold;

		// Token: 0x04000C47 RID: 3143
		public bool returnAfterBeingPushedAway;

		// Token: 0x04000C48 RID: 3144
		public float progressAverage;

		// Token: 0x04000C49 RID: 3145
		private bool wasEnabled;

		// Token: 0x04000C4A RID: 3146
		private float timer1;

		// Token: 0x04000C4B RID: 3147
		private float shouldStopDelayTimer;

		// Token: 0x04000C4C RID: 3148
		private bool lastShouldStopResult;

		// Token: 0x04000C4D RID: 3149
		private Vector3 lastShouldStopDestination;

		// Token: 0x04000C4E RID: 3150
		private Vector3 reachedDestinationPoint;

		// Token: 0x04000C4F RID: 3151
		public bool lastJobDensityResult;

		// Token: 0x04000C50 RID: 3152
		private const float MaximumCirclePackingDensity = 0.9069f;

		// Token: 0x04000C52 RID: 3154
		private bool wasStopped;

		// Token: 0x04000C53 RID: 3155
		private const float DefaultPriority = 1f;

		// Token: 0x04000C54 RID: 3156
		private const float StoppedPriority = 0.1f;

		// Token: 0x04000C55 RID: 3157
		private const float MoveBackPriority = 0.5f;

		// Token: 0x020002AE RID: 686
		[BurstCompile(CompileSynchronously = false, FloatMode = FloatMode.Fast)]
		public struct JobDensityCheck : IJobParallelForBatched
		{
			// Token: 0x17000278 RID: 632
			// (get) Token: 0x0600111E RID: 4382 RVA: 0x0001AA07 File Offset: 0x00018C07
			public bool allowBoundsChecks
			{
				get
				{
					return false;
				}
			}

			// Token: 0x0600111F RID: 4383 RVA: 0x00067060 File Offset: 0x00065260
			public JobDensityCheck(int size, float deltaTime, SimulatorBurst simulator)
			{
				this.agentPosition = simulator.simulationData.position;
				this.agentTargetPoint = simulator.simulationData.targetPoint;
				this.agentRadius = simulator.simulationData.radius;
				this.agentDesiredSpeed = simulator.simulationData.desiredSpeed;
				this.agentOutputTargetPoint = simulator.outputData.targetPoint;
				this.agentOutputSpeed = simulator.outputData.speed;
				this.quadtree = simulator.quadtree;
				this.data = new NativeArray<RVODestinationCrowdedBehavior.JobDensityCheck.QueryData>(size, Allocator.TempJob, NativeArrayOptions.UninitializedMemory);
				this.outThresholdResult = new NativeArray<bool>(size, Allocator.TempJob, NativeArrayOptions.UninitializedMemory);
				this.progressAverage = new NativeArray<float>(size, Allocator.TempJob, NativeArrayOptions.UninitializedMemory);
				this.deltaTime = deltaTime;
			}

			// Token: 0x06001120 RID: 4384 RVA: 0x00067110 File Offset: 0x00065310
			public void Dispose()
			{
				this.data.Dispose();
				this.outThresholdResult.Dispose();
				this.progressAverage.Dispose();
			}

			// Token: 0x06001121 RID: 4385 RVA: 0x00067134 File Offset: 0x00065334
			public void Set(int index, int rvoAgentIndex, float3 destination, float densityThreshold, float progressAverage)
			{
				this.data[index] = new RVODestinationCrowdedBehavior.JobDensityCheck.QueryData
				{
					agentDestination = destination,
					densityThreshold = densityThreshold,
					agentIndex = rvoAgentIndex
				};
				this.progressAverage[index] = progressAverage;
			}

			// Token: 0x06001122 RID: 4386 RVA: 0x00067180 File Offset: 0x00065380
			void IJobParallelForBatched.Execute(int start, int count)
			{
				for (int i = start; i < start + count; i++)
				{
					this.Execute(i);
				}
			}

			// Token: 0x06001123 RID: 4387 RVA: 0x000671A2 File Offset: 0x000653A2
			private float AgentDensityInCircle(float3 position, float radius)
			{
				return this.quadtree.QueryArea(position, radius) / (radius * radius * 3.1415927f);
			}

			// Token: 0x06001124 RID: 4388 RVA: 0x000671BC File Offset: 0x000653BC
			private void Execute(int i)
			{
				RVODestinationCrowdedBehavior.JobDensityCheck.QueryData queryData = this.data[i];
				float3 @float = this.agentPosition[queryData.agentIndex];
				float num = this.agentRadius[queryData.agentIndex];
				float3 float2 = math.normalizesafe(this.agentTargetPoint[queryData.agentIndex] - @float, default(float3));
				float num2;
				if (this.agentDesiredSpeed[queryData.agentIndex] > 0.01f)
				{
					num2 = math.dot(float2, math.normalizesafe(this.agentOutputTargetPoint[queryData.agentIndex] - @float, default(float3)) * this.agentOutputSpeed[queryData.agentIndex]) / math.max(0.001f, math.min(this.agentDesiredSpeed[queryData.agentIndex], this.agentRadius[queryData.agentIndex]));
					num2 = math.clamp(num2, -1f, 1f);
				}
				else
				{
					num2 = 1f;
				}
				this.progressAverage[i] = math.lerp(this.progressAverage[i], num2, 2f * this.deltaTime);
				if (math.any(math.isinf(queryData.agentDestination)))
				{
					this.outThresholdResult[i] = true;
					return;
				}
				float num3 = math.length(queryData.agentDestination - @float);
				float num4 = num * 5f;
				if (num3 > num4 && this.AgentDensityInCircle(queryData.agentDestination, num4) < 0.9069f * queryData.densityThreshold)
				{
					this.outThresholdResult[i] = false;
					return;
				}
				this.outThresholdResult[i] = this.AgentDensityInCircle(queryData.agentDestination, num3) > 0.9069f * queryData.densityThreshold;
			}

			// Token: 0x04000C56 RID: 3158
			[ReadOnly]
			private RVOQuadtreeBurst quadtree;

			// Token: 0x04000C57 RID: 3159
			[ReadOnly]
			public NativeArray<RVODestinationCrowdedBehavior.JobDensityCheck.QueryData> data;

			// Token: 0x04000C58 RID: 3160
			[ReadOnly]
			public NativeArray<float3> agentPosition;

			// Token: 0x04000C59 RID: 3161
			[ReadOnly]
			private NativeArray<float3> agentTargetPoint;

			// Token: 0x04000C5A RID: 3162
			[ReadOnly]
			private NativeArray<float> agentRadius;

			// Token: 0x04000C5B RID: 3163
			[ReadOnly]
			private NativeArray<float> agentDesiredSpeed;

			// Token: 0x04000C5C RID: 3164
			[ReadOnly]
			private NativeArray<float3> agentOutputTargetPoint;

			// Token: 0x04000C5D RID: 3165
			[ReadOnly]
			private NativeArray<float> agentOutputSpeed;

			// Token: 0x04000C5E RID: 3166
			[WriteOnly]
			public NativeArray<bool> outThresholdResult;

			// Token: 0x04000C5F RID: 3167
			public NativeArray<float> progressAverage;

			// Token: 0x04000C60 RID: 3168
			public float deltaTime;

			// Token: 0x020002AF RID: 687
			public struct QueryData
			{
				// Token: 0x04000C61 RID: 3169
				public float3 agentDestination;

				// Token: 0x04000C62 RID: 3170
				public int agentIndex;

				// Token: 0x04000C63 RID: 3171
				public float densityThreshold;
			}
		}
	}
}
