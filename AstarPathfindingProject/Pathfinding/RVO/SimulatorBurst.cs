using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Pathfinding.Collections;
using Pathfinding.Drawing;
using Pathfinding.ECS.RVO;
using Pathfinding.Jobs;
using Pathfinding.Sync;
using Pathfinding.Util;
using Unity.Burst.CompilerServices;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace Pathfinding.RVO
{
	// Token: 0x020002C7 RID: 711
	public class SimulatorBurst
	{
		// Token: 0x0600118F RID: 4495 RVA: 0x0006B25B File Offset: 0x0006945B
		public SimulatorBurst.AgentNeighbourLookup GetAgentNeighbourLookup()
		{
			return new SimulatorBurst.AgentNeighbourLookup(this.temporaryAgentData.neighbours);
		}

		// Token: 0x17000298 RID: 664
		// (get) Token: 0x06001190 RID: 4496 RVA: 0x0006B26D File Offset: 0x0006946D
		// (set) Token: 0x06001191 RID: 4497 RVA: 0x0006B275 File Offset: 0x00069475
		public float DesiredDeltaTime
		{
			get
			{
				return this.desiredDeltaTime;
			}
			set
			{
				this.desiredDeltaTime = Math.Max(value, 0f);
			}
		}

		// Token: 0x17000299 RID: 665
		// (get) Token: 0x06001192 RID: 4498 RVA: 0x0006B288 File Offset: 0x00069488
		// (set) Token: 0x06001193 RID: 4499 RVA: 0x0006B290 File Offset: 0x00069490
		public float SymmetryBreakingBias { get; set; }

		// Token: 0x1700029A RID: 666
		// (get) Token: 0x06001194 RID: 4500 RVA: 0x0006B299 File Offset: 0x00069499
		// (set) Token: 0x06001195 RID: 4501 RVA: 0x0006B2A1 File Offset: 0x000694A1
		public bool HardCollisions { get; set; }

		// Token: 0x1700029B RID: 667
		// (get) Token: 0x06001196 RID: 4502 RVA: 0x0006B2AA File Offset: 0x000694AA
		// (set) Token: 0x06001197 RID: 4503 RVA: 0x0006B2B2 File Offset: 0x000694B2
		public bool UseNavmeshAsObstacle { get; set; }

		// Token: 0x1700029C RID: 668
		// (get) Token: 0x06001198 RID: 4504 RVA: 0x0006B2BC File Offset: 0x000694BC
		public Rect AgentBounds
		{
			get
			{
				this.rwLock.ReadSync().Unlock();
				return this.quadtree.bounds;
			}
		}

		// Token: 0x1700029D RID: 669
		// (get) Token: 0x06001199 RID: 4505 RVA: 0x0006B2E7 File Offset: 0x000694E7
		public int AgentCount
		{
			get
			{
				return this.numAgents;
			}
		}

		// Token: 0x1700029E RID: 670
		// (get) Token: 0x0600119A RID: 4506 RVA: 0x0006B2EF File Offset: 0x000694EF
		public MovementPlane MovementPlane
		{
			get
			{
				return this.movementPlane;
			}
		}

		// Token: 0x0600119B RID: 4507 RVA: 0x0006B2F8 File Offset: 0x000694F8
		public void BlockUntilSimulationStepDone()
		{
			this.rwLock.WriteSync().Unlock();
		}

		// Token: 0x0600119C RID: 4508 RVA: 0x0006B318 File Offset: 0x00069518
		public SimulatorBurst(MovementPlane movementPlane)
		{
			this.DesiredDeltaTime = 1f;
			this.movementPlane = movementPlane;
			this.AllocateAgentSpace();
			this.quadtree.BuildJob(this.simulationData.position, this.simulationData.version, this.simulationData.desiredSpeed, this.simulationData.radius, 0, movementPlane).Run<RVOQuadtreeBurst.JobBuild>();
		}

		// Token: 0x0600119D RID: 4509 RVA: 0x0006B3BC File Offset: 0x000695BC
		public void ClearAgents()
		{
			this.BlockUntilSimulationStepDone();
			for (int i = 0; i < this.agentDestroyCallbacks.Length; i++)
			{
				Action action = this.agentDestroyCallbacks[i];
				if (action != null)
				{
					action();
				}
			}
			this.numAgents = 0;
		}

		// Token: 0x0600119E RID: 4510 RVA: 0x0006B3FC File Offset: 0x000695FC
		public void OnDestroy()
		{
			this.debugDrawingScope.Dispose();
			this.BlockUntilSimulationStepDone();
			this.ClearAgents();
			this.simulationData.Dispose();
			this.temporaryAgentData.Dispose();
			this.outputData.Dispose();
			this.quadtree.Dispose();
			this.horizonAgentData.Dispose();
		}

		// Token: 0x0600119F RID: 4511 RVA: 0x0006B458 File Offset: 0x00069658
		private void AllocateAgentSpace()
		{
			if (this.numAgents > this.agentPreCalculationCallbacks.Length || this.agentPreCalculationCallbacks.Length == 0)
			{
				int length = this.simulationData.version.Length;
				int num = Mathf.Max(64, Mathf.Max(this.numAgents, this.agentPreCalculationCallbacks.Length * 2));
				this.simulationData.Realloc(num, Allocator.Persistent);
				this.temporaryAgentData.Realloc(num, Allocator.Persistent);
				this.outputData.Realloc(num, Allocator.Persistent);
				this.horizonAgentData.Realloc(num, Allocator.Persistent);
				Memory.Realloc<Action>(ref this.agentPreCalculationCallbacks, num);
				Memory.Realloc<Action>(ref this.agentDestroyCallbacks, num);
				for (int i = length; i < num; i++)
				{
					this.simulationData.version[i] = new AgentIndex(0, i);
				}
			}
		}

		// Token: 0x1700029F RID: 671
		// (get) Token: 0x060011A0 RID: 4512 RVA: 0x0006B51D File Offset: 0x0006971D
		public bool anyAgentsInSimulation
		{
			get
			{
				return this.numAgents > this.freeAgentIndices.Count;
			}
		}

		// Token: 0x060011A1 RID: 4513 RVA: 0x0006B534 File Offset: 0x00069734
		public IAgent AddAgent(Vector3 position)
		{
			AgentIndex agentIndex = this.AddAgentBurst(position);
			return new SimulatorBurst.Agent
			{
				simulator = this,
				agentIndex = agentIndex
			};
		}

		// Token: 0x060011A2 RID: 4514 RVA: 0x0006B56C File Offset: 0x0006976C
		public AgentIndex AddAgentBurst(float3 position)
		{
			this.BlockUntilSimulationStepDone();
			int num;
			if (this.freeAgentIndices.Count > 0)
			{
				num = this.freeAgentIndices.Pop();
			}
			else
			{
				int num2 = this.numAgents;
				this.numAgents = num2 + 1;
				num = num2;
				this.AllocateAgentSpace();
			}
			AgentIndex agentIndex = this.simulationData.version[num].WithIncrementedVersion();
			this.simulationData.version[num] = agentIndex;
			this.simulationData.radius[num] = 5f;
			this.simulationData.height[num] = 5f;
			this.simulationData.desiredSpeed[num] = 0f;
			this.simulationData.maxSpeed[num] = 1f;
			this.simulationData.agentTimeHorizon[num] = 2f;
			this.simulationData.obstacleTimeHorizon[num] = 2f;
			this.simulationData.locked[num] = false;
			this.simulationData.maxNeighbours[num] = 10;
			this.simulationData.layer[num] = RVOLayer.DefaultAgent;
			this.simulationData.collidesWith[num] = (RVOLayer)(-1);
			this.simulationData.flowFollowingStrength[num] = 0f;
			this.simulationData.position[num] = position;
			this.simulationData.collisionNormal[num] = float3.zero;
			this.simulationData.manuallyControlled[num] = false;
			this.simulationData.priority[num] = 0.5f;
			this.simulationData.debugFlags[num] = AgentDebugFlags.Nothing;
			this.simulationData.targetPoint[num] = position;
			this.simulationData.movementPlane[num] = new NativeMovementPlane(((this.movementPlane == MovementPlane.XY) ? SimpleMovementPlane.XYPlane : SimpleMovementPlane.XZPlane).rotation);
			this.simulationData.allowedVelocityDeviationAngles[num] = float2.zero;
			this.simulationData.endOfPath[num] = new float3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity);
			this.simulationData.agentObstacleMapping[num] = -1;
			this.simulationData.hierarchicalNodeIndex[num] = -1;
			this.outputData.speed[num] = 0f;
			this.outputData.numNeighbours[num] = 0;
			this.outputData.targetPoint[num] = position;
			this.outputData.blockedByAgents[num * 7] = -1;
			this.outputData.effectivelyReachedDestination[num] = ReachedEndOfPath.NotReached;
			this.temporaryAgentData.neighbours[num * 50] = -1;
			this.horizonAgentData.horizonSide[num] = 0;
			this.agentPreCalculationCallbacks[num] = null;
			this.agentDestroyCallbacks[num] = null;
			return agentIndex;
		}

		// Token: 0x060011A3 RID: 4515 RVA: 0x00003792 File Offset: 0x00001992
		[Obsolete("Use AddAgent(Vector3) instead", true)]
		public IAgent AddAgent(IAgent agent)
		{
			return null;
		}

		// Token: 0x060011A4 RID: 4516 RVA: 0x0006B864 File Offset: 0x00069A64
		public void RemoveAgent(IAgent agent)
		{
			if (agent == null)
			{
				throw new ArgumentNullException("agent");
			}
			SimulatorBurst.Agent agent2 = (SimulatorBurst.Agent)agent;
			this.RemoveAgent(agent2.agentIndex, false);
		}

		// Token: 0x060011A5 RID: 4517 RVA: 0x0006B894 File Offset: 0x00069A94
		public void RemoveAgent(AgentIndex agent, bool okIfMissing = false)
		{
			this.BlockUntilSimulationStepDone();
			int num;
			if (agent.TryGetIndex(ref this.simulationData, out num))
			{
				this.simulationData.version[num] = this.simulationData.version[num].WithIncrementedVersion().WithDeleted();
				this.agentPreCalculationCallbacks[num] = null;
				try
				{
					if (this.agentDestroyCallbacks[num] != null)
					{
						this.agentDestroyCallbacks[num]();
					}
				}
				catch (Exception ex)
				{
					Debug.LogException(ex);
				}
				this.agentDestroyCallbacks[num] = null;
				this.freeAgentIndices.Push(num);
				return;
			}
			if (okIfMissing)
			{
				return;
			}
			throw new InvalidOperationException("Trying to remove agent which does not exist");
		}

		// Token: 0x060011A6 RID: 4518 RVA: 0x0006B948 File Offset: 0x00069B48
		private void PreCalculation(JobHandle dependency)
		{
			bool flag = false;
			for (int i = 0; i < this.numAgents; i++)
			{
				Action action = this.agentPreCalculationCallbacks[i];
				if (action != null)
				{
					if (!flag)
					{
						dependency.Complete();
						this.rwLock.ReadSync().Unlock();
						flag = true;
					}
					action();
				}
			}
		}

		// Token: 0x060011A7 RID: 4519 RVA: 0x0006B99C File Offset: 0x00069B9C
		public JobHandle Update(JobHandle dependency, float dt, bool drawGizmos, Allocator allocator)
		{
			if (false)
			{
				default(JobRVO<XYMovementPlane>).ScheduleBatch(0, 0, default(JobHandle));
				default(JobRVO<XZMovementPlane>).ScheduleBatch(0, 0, default(JobHandle));
				default(JobRVO<ArbitraryMovementPlane>).ScheduleBatch(0, 0, default(JobHandle));
				default(JobRVOPreprocess<XYMovementPlane>).Schedule(default(JobHandle));
				default(JobRVOPreprocess<XZMovementPlane>).Schedule(default(JobHandle));
				default(JobRVOPreprocess<ArbitraryMovementPlane>).Schedule(default(JobHandle));
				default(JobHorizonAvoidancePhase1<XYMovementPlane>).ScheduleBatch(0, 0, default(JobHandle));
				default(JobHorizonAvoidancePhase1<XZMovementPlane>).ScheduleBatch(0, 0, default(JobHandle));
				default(JobHorizonAvoidancePhase1<ArbitraryMovementPlane>).ScheduleBatch(0, 0, default(JobHandle));
				default(JobHorizonAvoidancePhase2<XYMovementPlane>).ScheduleBatch(0, 0, default(JobHandle));
				default(JobHorizonAvoidancePhase2<XZMovementPlane>).ScheduleBatch(0, 0, default(JobHandle));
				default(JobHorizonAvoidancePhase2<ArbitraryMovementPlane>).ScheduleBatch(0, 0, default(JobHandle));
				default(JobRVOCalculateNeighbours<XYMovementPlane>).ScheduleBatch(0, 0, default(JobHandle));
				default(JobRVOCalculateNeighbours<XZMovementPlane>).ScheduleBatch(0, 0, default(JobHandle));
				default(JobRVOCalculateNeighbours<ArbitraryMovementPlane>).ScheduleBatch(0, 0, default(JobHandle));
				default(JobHardCollisions<XYMovementPlane>).ScheduleBatch(0, 0, default(JobHandle));
				default(JobHardCollisions<XZMovementPlane>).ScheduleBatch(0, 0, default(JobHandle));
				default(JobHardCollisions<ArbitraryMovementPlane>).ScheduleBatch(0, 0, default(JobHandle));
				default(JobDestinationReached<XYMovementPlane>).Schedule(default(JobHandle));
				default(JobDestinationReached<XZMovementPlane>).Schedule(default(JobHandle));
				default(JobDestinationReached<ArbitraryMovementPlane>).Schedule(default(JobHandle));
			}
			if (this.movementPlane == MovementPlane.XY)
			{
				return this.UpdateInternal<XYMovementPlane>(dependency, dt, drawGizmos, allocator);
			}
			if (this.movementPlane == MovementPlane.XZ)
			{
				return this.UpdateInternal<XZMovementPlane>(dependency, dt, drawGizmos, allocator);
			}
			return this.UpdateInternal<ArbitraryMovementPlane>(dependency, dt, drawGizmos, allocator);
		}

		// Token: 0x060011A8 RID: 4520 RVA: 0x0006BC0B File Offset: 0x00069E0B
		public RWLock.ReadLockAsync LockSimulationDataReadOnly()
		{
			return this.rwLock.Read();
		}

		// Token: 0x060011A9 RID: 4521 RVA: 0x0006BC18 File Offset: 0x00069E18
		public RWLock.WriteLockAsync LockSimulationDataReadWrite()
		{
			return this.rwLock.Write();
		}

		// Token: 0x060011AA RID: 4522 RVA: 0x0006BC28 File Offset: 0x00069E28
		private JobHandle UpdateInternal<T>(JobHandle dependency, float deltaTime, bool drawGizmos, Allocator allocator) where T : struct, IMovementPlaneWrapper
		{
			if (!this.anyAgentsInSimulation)
			{
				return dependency;
			}
			deltaTime = math.max(deltaTime, 0.0005f);
			this.PreCalculation(dependency);
			RWLock.WriteLockAsync writeLockAsync = this.rwLock.Write();
			dependency = JobHandle.CombineDependencies(dependency, writeLockAsync.dependency);
			JobHandle jobHandle = this.quadtree.BuildJob(this.simulationData.position, this.simulationData.version, this.outputData.speed, this.simulationData.radius, this.numAgents, this.movementPlane).Schedule(dependency);
			JobHandle jobHandle2 = new JobRVOPreprocess<T>
			{
				agentData = this.simulationData,
				previousOutput = this.outputData,
				temporaryAgentData = this.temporaryAgentData,
				startIndex = 0,
				endIndex = this.numAgents
			}.Schedule(dependency);
			int num = math.max(this.numAgents / 64, 8);
			JobHandle jobHandle3 = new JobRVOCalculateNeighbours<T>
			{
				agentData = this.simulationData,
				quadtree = this.quadtree,
				outNeighbours = this.temporaryAgentData.neighbours,
				output = this.outputData
			}.ScheduleBatch(this.numAgents, num, JobHandle.CombineDependencies(jobHandle2, jobHandle));
			JobHandle.ScheduleBatchedJobs();
			JobHandle jobHandle4 = JobHandle.CombineDependencies(jobHandle2, jobHandle3);
			JobHandle jobHandle5 = new JobHorizonAvoidancePhase1<T>
			{
				agentData = this.simulationData,
				neighbours = this.temporaryAgentData.neighbours,
				desiredTargetPointInVelocitySpace = this.temporaryAgentData.desiredTargetPointInVelocitySpace,
				horizonAgentData = this.horizonAgentData
			}.ScheduleBatch(this.numAgents, num, jobHandle4);
			JobHandle jobHandle6 = new JobHorizonAvoidancePhase2<T>
			{
				neighbours = this.temporaryAgentData.neighbours,
				versions = this.simulationData.version,
				desiredVelocity = this.temporaryAgentData.desiredVelocity,
				desiredTargetPointInVelocitySpace = this.temporaryAgentData.desiredTargetPointInVelocitySpace,
				horizonAgentData = this.horizonAgentData,
				movementPlane = this.simulationData.movementPlane
			}.ScheduleBatch(this.numAgents, num, jobHandle5);
			JobHandle jobHandle7 = new JobHardCollisions<T>
			{
				agentData = this.simulationData,
				neighbours = this.temporaryAgentData.neighbours,
				collisionVelocityOffsets = this.temporaryAgentData.collisionVelocityOffsets,
				deltaTime = deltaTime,
				enabled = this.HardCollisions
			}.ScheduleBatch(this.numAgents, num, jobHandle4);
			bool flag = AstarPath.active != null;
			RWLock.CombinedReadLockAsync combinedReadLockAsync;
			NavmeshEdges.NavmeshBorderData navmeshBorderData;
			if (flag)
			{
				navmeshBorderData = AstarPath.active.GetNavmeshBorderData(out combinedReadLockAsync);
			}
			else
			{
				navmeshBorderData = NavmeshEdges.NavmeshBorderData.CreateEmpty(allocator);
				combinedReadLockAsync = default(RWLock.CombinedReadLockAsync);
			}
			JobRVO<T> jobRVO = new JobRVO<T>
			{
				agentData = this.simulationData,
				temporaryAgentData = this.temporaryAgentData,
				navmeshEdgeData = navmeshBorderData,
				output = this.outputData,
				deltaTime = deltaTime,
				symmetryBreakingBias = Mathf.Max(0f, this.SymmetryBreakingBias),
				useNavmeshAsObstacle = this.UseNavmeshAsObstacle,
				priorityMultiplier = 1f
			};
			jobHandle4 = JobHandle.CombineDependencies(jobHandle6, jobHandle7, combinedReadLockAsync.dependency);
			JobHandle jobHandle8 = jobRVO.ScheduleBatch(this.numAgents, num, jobHandle4);
			if (flag)
			{
				combinedReadLockAsync.UnlockAfter(jobHandle8);
			}
			else
			{
				navmeshBorderData.DisposeEmpty(jobHandle8);
			}
			JobHandle jobHandle9 = new JobDestinationReached<T>
			{
				agentData = this.simulationData,
				temporaryAgentData = this.temporaryAgentData,
				output = this.outputData,
				numAgents = this.numAgents
			}.Schedule(jobHandle8);
			JobHandle jobHandle10 = this.simulationData.collisionNormal.MemSet(float3.zero).Schedule(jobHandle9);
			JobHandle jobHandle11 = this.simulationData.manuallyControlled.MemSet(false).Schedule(jobHandle9);
			JobHandle jobHandle12 = this.simulationData.hierarchicalNodeIndex.MemSet(-1).Schedule(jobHandle9);
			dependency = JobHandle.CombineDependencies(jobHandle9, jobHandle10, jobHandle11);
			dependency = JobHandle.CombineDependencies(dependency, jobHandle12);
			writeLockAsync.UnlockAfter(dependency);
			return dependency;
		}

		// Token: 0x04000CD3 RID: 3283
		private float desiredDeltaTime = 0.05f;

		// Token: 0x04000CD4 RID: 3284
		private int numAgents;

		// Token: 0x04000CD5 RID: 3285
		private RedrawScope debugDrawingScope;

		// Token: 0x04000CD6 RID: 3286
		public RVOQuadtreeBurst quadtree;

		// Token: 0x04000CD7 RID: 3287
		public bool drawQuadtree;

		// Token: 0x04000CD8 RID: 3288
		private Action[] agentPreCalculationCallbacks = new Action[0];

		// Token: 0x04000CD9 RID: 3289
		private Action[] agentDestroyCallbacks = new Action[0];

		// Token: 0x04000CDA RID: 3290
		private Stack<int> freeAgentIndices = new Stack<int>();

		// Token: 0x04000CDB RID: 3291
		private SimulatorBurst.TemporaryAgentData temporaryAgentData;

		// Token: 0x04000CDC RID: 3292
		private SimulatorBurst.HorizonAgentData horizonAgentData;

		// Token: 0x04000CDD RID: 3293
		public SimulatorBurst.AgentData simulationData;

		// Token: 0x04000CDE RID: 3294
		public SimulatorBurst.AgentOutputData outputData;

		// Token: 0x04000CDF RID: 3295
		public const int MaxNeighbourCount = 50;

		// Token: 0x04000CE0 RID: 3296
		public const int MaxBlockingAgentCount = 7;

		// Token: 0x04000CE1 RID: 3297
		public const int MaxObstacleVertices = 256;

		// Token: 0x04000CE5 RID: 3301
		public readonly MovementPlane movementPlane;

		// Token: 0x04000CE6 RID: 3302
		private RWLock rwLock = new RWLock();

		// Token: 0x020002C8 RID: 712
		public struct AgentNeighbourLookup
		{
			// Token: 0x060011AB RID: 4523 RVA: 0x0006C046 File Offset: 0x0006A246
			public AgentNeighbourLookup(NativeArray<int> neighbours)
			{
				this.neighbours = neighbours;
			}

			// Token: 0x060011AC RID: 4524 RVA: 0x0006C050 File Offset: 0x0006A250
			public UnsafeSpan<int> GetNeighbours(int agentIndex)
			{
				int num = agentIndex * 50;
				int num2 = num;
				while (this.neighbours[num2] != -1)
				{
					num2++;
				}
				return this.neighbours.AsUnsafeReadOnlySpan<int>().Slice(num, num2 - num);
			}

			// Token: 0x04000CE7 RID: 3303
			[ReadOnly]
			[NativeDisableParallelForRestriction]
			private NativeArray<int> neighbours;
		}

		// Token: 0x020002C9 RID: 713
		private struct Agent : IAgent
		{
			// Token: 0x170002A0 RID: 672
			// (get) Token: 0x060011AD RID: 4525 RVA: 0x0006C090 File Offset: 0x0006A290
			public int AgentIndex
			{
				get
				{
					return this.agentIndex.Index;
				}
			}

			// Token: 0x170002A1 RID: 673
			// (get) Token: 0x060011AE RID: 4526 RVA: 0x0006C09D File Offset: 0x0006A29D
			// (set) Token: 0x060011AF RID: 4527 RVA: 0x0006C0BF File Offset: 0x0006A2BF
			public Vector3 Position
			{
				get
				{
					return this.simulator.simulationData.position[this.AgentIndex];
				}
				set
				{
					this.simulator.simulationData.position[this.AgentIndex] = value;
				}
			}

			// Token: 0x170002A2 RID: 674
			// (get) Token: 0x060011B0 RID: 4528 RVA: 0x0006C0E2 File Offset: 0x0006A2E2
			// (set) Token: 0x060011B1 RID: 4529 RVA: 0x0006C0FF File Offset: 0x0006A2FF
			public bool Locked
			{
				get
				{
					return this.simulator.simulationData.locked[this.AgentIndex];
				}
				set
				{
					this.simulator.simulationData.locked[this.AgentIndex] = value;
				}
			}

			// Token: 0x170002A3 RID: 675
			// (get) Token: 0x060011B2 RID: 4530 RVA: 0x0006C11D File Offset: 0x0006A31D
			// (set) Token: 0x060011B3 RID: 4531 RVA: 0x0006C13A File Offset: 0x0006A33A
			public float Radius
			{
				get
				{
					return this.simulator.simulationData.radius[this.AgentIndex];
				}
				set
				{
					this.simulator.simulationData.radius[this.AgentIndex] = value;
				}
			}

			// Token: 0x170002A4 RID: 676
			// (get) Token: 0x060011B4 RID: 4532 RVA: 0x0006C158 File Offset: 0x0006A358
			// (set) Token: 0x060011B5 RID: 4533 RVA: 0x0006C175 File Offset: 0x0006A375
			public float Height
			{
				get
				{
					return this.simulator.simulationData.height[this.AgentIndex];
				}
				set
				{
					this.simulator.simulationData.height[this.AgentIndex] = value;
				}
			}

			// Token: 0x170002A5 RID: 677
			// (get) Token: 0x060011B6 RID: 4534 RVA: 0x0006C193 File Offset: 0x0006A393
			// (set) Token: 0x060011B7 RID: 4535 RVA: 0x0006C1B0 File Offset: 0x0006A3B0
			public float AgentTimeHorizon
			{
				get
				{
					return this.simulator.simulationData.agentTimeHorizon[this.AgentIndex];
				}
				set
				{
					this.simulator.simulationData.agentTimeHorizon[this.AgentIndex] = value;
				}
			}

			// Token: 0x170002A6 RID: 678
			// (get) Token: 0x060011B8 RID: 4536 RVA: 0x0006C1CE File Offset: 0x0006A3CE
			// (set) Token: 0x060011B9 RID: 4537 RVA: 0x0006C1EB File Offset: 0x0006A3EB
			public float ObstacleTimeHorizon
			{
				get
				{
					return this.simulator.simulationData.obstacleTimeHorizon[this.AgentIndex];
				}
				set
				{
					this.simulator.simulationData.obstacleTimeHorizon[this.AgentIndex] = value;
				}
			}

			// Token: 0x170002A7 RID: 679
			// (get) Token: 0x060011BA RID: 4538 RVA: 0x0006C209 File Offset: 0x0006A409
			// (set) Token: 0x060011BB RID: 4539 RVA: 0x0006C226 File Offset: 0x0006A426
			public int MaxNeighbours
			{
				get
				{
					return this.simulator.simulationData.maxNeighbours[this.AgentIndex];
				}
				set
				{
					this.simulator.simulationData.maxNeighbours[this.AgentIndex] = value;
				}
			}

			// Token: 0x170002A8 RID: 680
			// (get) Token: 0x060011BC RID: 4540 RVA: 0x0006C244 File Offset: 0x0006A444
			// (set) Token: 0x060011BD RID: 4541 RVA: 0x0006C261 File Offset: 0x0006A461
			public RVOLayer Layer
			{
				get
				{
					return this.simulator.simulationData.layer[this.AgentIndex];
				}
				set
				{
					this.simulator.simulationData.layer[this.AgentIndex] = value;
				}
			}

			// Token: 0x170002A9 RID: 681
			// (get) Token: 0x060011BE RID: 4542 RVA: 0x0006C27F File Offset: 0x0006A47F
			// (set) Token: 0x060011BF RID: 4543 RVA: 0x0006C29C File Offset: 0x0006A49C
			public RVOLayer CollidesWith
			{
				get
				{
					return this.simulator.simulationData.collidesWith[this.AgentIndex];
				}
				set
				{
					this.simulator.simulationData.collidesWith[this.AgentIndex] = value;
				}
			}

			// Token: 0x170002AA RID: 682
			// (get) Token: 0x060011C0 RID: 4544 RVA: 0x0006C2BA File Offset: 0x0006A4BA
			// (set) Token: 0x060011C1 RID: 4545 RVA: 0x0006C2D7 File Offset: 0x0006A4D7
			public float FlowFollowingStrength
			{
				get
				{
					return this.simulator.simulationData.flowFollowingStrength[this.AgentIndex];
				}
				set
				{
					this.simulator.simulationData.flowFollowingStrength[this.AgentIndex] = value;
				}
			}

			// Token: 0x170002AB RID: 683
			// (get) Token: 0x060011C2 RID: 4546 RVA: 0x0006C2F5 File Offset: 0x0006A4F5
			// (set) Token: 0x060011C3 RID: 4547 RVA: 0x0006C312 File Offset: 0x0006A512
			public AgentDebugFlags DebugFlags
			{
				get
				{
					return this.simulator.simulationData.debugFlags[this.AgentIndex];
				}
				set
				{
					this.simulator.simulationData.debugFlags[this.AgentIndex] = value;
				}
			}

			// Token: 0x170002AC RID: 684
			// (get) Token: 0x060011C4 RID: 4548 RVA: 0x0006C330 File Offset: 0x0006A530
			// (set) Token: 0x060011C5 RID: 4549 RVA: 0x0006C34D File Offset: 0x0006A54D
			public float Priority
			{
				get
				{
					return this.simulator.simulationData.priority[this.AgentIndex];
				}
				set
				{
					this.simulator.simulationData.priority[this.AgentIndex] = value;
				}
			}

			// Token: 0x170002AD RID: 685
			// (get) Token: 0x060011C6 RID: 4550 RVA: 0x0006C36B File Offset: 0x0006A56B
			// (set) Token: 0x060011C7 RID: 4551 RVA: 0x0006C388 File Offset: 0x0006A588
			public int HierarchicalNodeIndex
			{
				get
				{
					return this.simulator.simulationData.hierarchicalNodeIndex[this.AgentIndex];
				}
				set
				{
					this.simulator.simulationData.hierarchicalNodeIndex[this.AgentIndex] = value;
				}
			}

			// Token: 0x170002AE RID: 686
			// (get) Token: 0x060011C8 RID: 4552 RVA: 0x0006C3A6 File Offset: 0x0006A5A6
			// (set) Token: 0x060011C9 RID: 4553 RVA: 0x0006C3D2 File Offset: 0x0006A5D2
			public SimpleMovementPlane MovementPlane
			{
				get
				{
					return new SimpleMovementPlane(this.simulator.simulationData.movementPlane[this.AgentIndex].rotation);
				}
				set
				{
					this.simulator.simulationData.movementPlane[this.AgentIndex] = new NativeMovementPlane(value);
				}
			}

			// Token: 0x170002AF RID: 687
			// (set) Token: 0x060011CA RID: 4554 RVA: 0x0006C3F5 File Offset: 0x0006A5F5
			public Action PreCalculationCallback
			{
				set
				{
					this.simulator.agentPreCalculationCallbacks[this.AgentIndex] = value;
				}
			}

			// Token: 0x170002B0 RID: 688
			// (set) Token: 0x060011CB RID: 4555 RVA: 0x0006C40A File Offset: 0x0006A60A
			public Action DestroyedCallback
			{
				set
				{
					this.simulator.agentDestroyCallbacks[this.AgentIndex] = value;
				}
			}

			// Token: 0x170002B1 RID: 689
			// (get) Token: 0x060011CC RID: 4556 RVA: 0x0006C41F File Offset: 0x0006A61F
			public Vector3 CalculatedTargetPoint
			{
				get
				{
					this.simulator.BlockUntilSimulationStepDone();
					return this.simulator.outputData.targetPoint[this.AgentIndex];
				}
			}

			// Token: 0x170002B2 RID: 690
			// (get) Token: 0x060011CD RID: 4557 RVA: 0x0006C44C File Offset: 0x0006A64C
			public float CalculatedSpeed
			{
				get
				{
					this.simulator.BlockUntilSimulationStepDone();
					return this.simulator.outputData.speed[this.AgentIndex];
				}
			}

			// Token: 0x170002B3 RID: 691
			// (get) Token: 0x060011CE RID: 4558 RVA: 0x0006C474 File Offset: 0x0006A674
			public ReachedEndOfPath CalculatedEffectivelyReachedDestination
			{
				get
				{
					this.simulator.BlockUntilSimulationStepDone();
					return this.simulator.outputData.effectivelyReachedDestination[this.AgentIndex];
				}
			}

			// Token: 0x170002B4 RID: 692
			// (get) Token: 0x060011CF RID: 4559 RVA: 0x0006C49C File Offset: 0x0006A69C
			public int NeighbourCount
			{
				get
				{
					this.simulator.BlockUntilSimulationStepDone();
					return this.simulator.outputData.numNeighbours[this.AgentIndex];
				}
			}

			// Token: 0x170002B5 RID: 693
			// (get) Token: 0x060011D0 RID: 4560 RVA: 0x0006C4C4 File Offset: 0x0006A6C4
			public bool AvoidingAnyAgents
			{
				get
				{
					this.simulator.BlockUntilSimulationStepDone();
					return this.simulator.outputData.blockedByAgents[this.AgentIndex * 7] != -1;
				}
			}

			// Token: 0x060011D1 RID: 4561 RVA: 0x0006C4F4 File Offset: 0x0006A6F4
			public void SetObstacleQuery(GraphNode sourceNode)
			{
				this.HierarchicalNodeIndex = ((sourceNode != null && !sourceNode.Destroyed && sourceNode.Walkable) ? sourceNode.HierarchicalNodeIndex : (-1));
			}

			// Token: 0x060011D2 RID: 4562 RVA: 0x0006C518 File Offset: 0x0006A718
			public void SetTarget(Vector3 targetPoint, float desiredSpeed, float maxSpeed, Vector3 endOfPath)
			{
				this.simulator.simulationData.SetTarget(this.AgentIndex, targetPoint, desiredSpeed, maxSpeed, endOfPath);
			}

			// Token: 0x060011D3 RID: 4563 RVA: 0x0006C53F File Offset: 0x0006A73F
			public void SetCollisionNormal(Vector3 normal)
			{
				this.simulator.simulationData.collisionNormal[this.AgentIndex] = normal;
			}

			// Token: 0x060011D4 RID: 4564 RVA: 0x0006C564 File Offset: 0x0006A764
			public void ForceSetVelocity(Vector3 velocity)
			{
				this.simulator.simulationData.targetPoint[this.AgentIndex] = this.simulator.simulationData.position[this.AgentIndex] + velocity * 1000f;
				this.simulator.simulationData.desiredSpeed[this.AgentIndex] = velocity.magnitude;
				this.simulator.simulationData.allowedVelocityDeviationAngles[this.AgentIndex] = float2.zero;
				this.simulator.simulationData.manuallyControlled[this.AgentIndex] = true;
			}

			// Token: 0x04000CE8 RID: 3304
			public SimulatorBurst simulator;

			// Token: 0x04000CE9 RID: 3305
			public AgentIndex agentIndex;
		}

		// Token: 0x020002CA RID: 714
		public struct ObstacleData
		{
			// Token: 0x060011D5 RID: 4565 RVA: 0x0006C61C File Offset: 0x0006A81C
			public void Init(Allocator allocator)
			{
				if (!this.obstacles.IsCreated)
				{
					this.obstacles = new NativeList<UnmanagedObstacle>(0, allocator);
				}
				if (!this.obstacleVertexGroups.IsCreated)
				{
					this.obstacleVertexGroups = new SlabAllocator<ObstacleVertexGroup>(4, allocator);
				}
				if (!this.obstacleVertices.IsCreated)
				{
					this.obstacleVertices = new SlabAllocator<float3>(16, allocator);
				}
			}

			// Token: 0x060011D6 RID: 4566 RVA: 0x0006C687 File Offset: 0x0006A887
			public void Dispose()
			{
				if (this.obstacleVertexGroups.IsCreated)
				{
					this.obstacleVertexGroups.Dispose();
					this.obstacleVertices.Dispose();
					this.obstacles.Dispose();
				}
			}

			// Token: 0x04000CEA RID: 3306
			public SlabAllocator<ObstacleVertexGroup> obstacleVertexGroups;

			// Token: 0x04000CEB RID: 3307
			public SlabAllocator<float3> obstacleVertices;

			// Token: 0x04000CEC RID: 3308
			public NativeList<UnmanagedObstacle> obstacles;
		}

		// Token: 0x020002CB RID: 715
		public struct AgentData
		{
			// Token: 0x060011D7 RID: 4567 RVA: 0x0006C6B8 File Offset: 0x0006A8B8
			public void Realloc(int size, Allocator allocator)
			{
				Memory.Realloc<AgentIndex>(ref this.version, size, allocator, NativeArrayOptions.UninitializedMemory);
				Memory.Realloc<float>(ref this.radius, size, allocator, NativeArrayOptions.UninitializedMemory);
				Memory.Realloc<float>(ref this.height, size, allocator, NativeArrayOptions.UninitializedMemory);
				Memory.Realloc<float>(ref this.desiredSpeed, size, allocator, NativeArrayOptions.UninitializedMemory);
				Memory.Realloc<float>(ref this.maxSpeed, size, allocator, NativeArrayOptions.UninitializedMemory);
				Memory.Realloc<float>(ref this.agentTimeHorizon, size, allocator, NativeArrayOptions.UninitializedMemory);
				Memory.Realloc<float>(ref this.obstacleTimeHorizon, size, allocator, NativeArrayOptions.UninitializedMemory);
				Memory.Realloc<bool>(ref this.locked, size, allocator, NativeArrayOptions.UninitializedMemory);
				Memory.Realloc<int>(ref this.maxNeighbours, size, allocator, NativeArrayOptions.UninitializedMemory);
				Memory.Realloc<RVOLayer>(ref this.layer, size, allocator, NativeArrayOptions.UninitializedMemory);
				Memory.Realloc<RVOLayer>(ref this.collidesWith, size, allocator, NativeArrayOptions.UninitializedMemory);
				Memory.Realloc<float>(ref this.flowFollowingStrength, size, allocator, NativeArrayOptions.UninitializedMemory);
				Memory.Realloc<float3>(ref this.position, size, allocator, NativeArrayOptions.UninitializedMemory);
				Memory.Realloc<float3>(ref this.collisionNormal, size, allocator, NativeArrayOptions.UninitializedMemory);
				Memory.Realloc<bool>(ref this.manuallyControlled, size, allocator, NativeArrayOptions.UninitializedMemory);
				Memory.Realloc<float>(ref this.priority, size, allocator, NativeArrayOptions.UninitializedMemory);
				Memory.Realloc<AgentDebugFlags>(ref this.debugFlags, size, allocator, NativeArrayOptions.UninitializedMemory);
				Memory.Realloc<float3>(ref this.targetPoint, size, allocator, NativeArrayOptions.UninitializedMemory);
				Memory.Realloc<NativeMovementPlane>(ref this.movementPlane, size, allocator, NativeArrayOptions.UninitializedMemory);
				Memory.Realloc<float2>(ref this.allowedVelocityDeviationAngles, size, allocator, NativeArrayOptions.UninitializedMemory);
				Memory.Realloc<float3>(ref this.endOfPath, size, allocator, NativeArrayOptions.UninitializedMemory);
				Memory.Realloc<int>(ref this.agentObstacleMapping, size, allocator, NativeArrayOptions.UninitializedMemory);
				Memory.Realloc<int>(ref this.hierarchicalNodeIndex, size, allocator, NativeArrayOptions.UninitializedMemory);
			}

			// Token: 0x060011D8 RID: 4568 RVA: 0x0006C808 File Offset: 0x0006AA08
			public void SetTarget(int agentIndex, float3 targetPoint, float desiredSpeed, float maxSpeed, float3 endOfPath)
			{
				maxSpeed = math.max(maxSpeed, 0f);
				desiredSpeed = math.clamp(desiredSpeed, 0f, maxSpeed);
				this.targetPoint[agentIndex] = targetPoint;
				this.desiredSpeed[agentIndex] = desiredSpeed;
				this.maxSpeed[agentIndex] = maxSpeed;
				this.endOfPath[agentIndex] = endOfPath;
			}

			// Token: 0x060011D9 RID: 4569 RVA: 0x0006C868 File Offset: 0x0006AA68
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public bool HasDebugFlag(int agentIndex, AgentDebugFlags flag)
			{
				return Hint.Unlikely((this.debugFlags[agentIndex] & flag) > AgentDebugFlags.Nothing);
			}

			// Token: 0x060011DA RID: 4570 RVA: 0x0006C880 File Offset: 0x0006AA80
			public void Dispose()
			{
				this.version.Dispose();
				this.radius.Dispose();
				this.height.Dispose();
				this.desiredSpeed.Dispose();
				this.maxSpeed.Dispose();
				this.agentTimeHorizon.Dispose();
				this.obstacleTimeHorizon.Dispose();
				this.locked.Dispose();
				this.maxNeighbours.Dispose();
				this.layer.Dispose();
				this.collidesWith.Dispose();
				this.flowFollowingStrength.Dispose();
				this.position.Dispose();
				this.collisionNormal.Dispose();
				this.manuallyControlled.Dispose();
				this.priority.Dispose();
				this.debugFlags.Dispose();
				this.targetPoint.Dispose();
				this.movementPlane.Dispose();
				this.allowedVelocityDeviationAngles.Dispose();
				this.endOfPath.Dispose();
				this.agentObstacleMapping.Dispose();
				this.hierarchicalNodeIndex.Dispose();
			}

			// Token: 0x04000CED RID: 3309
			public NativeArray<AgentIndex> version;

			// Token: 0x04000CEE RID: 3310
			public NativeArray<float> radius;

			// Token: 0x04000CEF RID: 3311
			public NativeArray<float> height;

			// Token: 0x04000CF0 RID: 3312
			public NativeArray<float> desiredSpeed;

			// Token: 0x04000CF1 RID: 3313
			public NativeArray<float> maxSpeed;

			// Token: 0x04000CF2 RID: 3314
			public NativeArray<float> agentTimeHorizon;

			// Token: 0x04000CF3 RID: 3315
			public NativeArray<float> obstacleTimeHorizon;

			// Token: 0x04000CF4 RID: 3316
			public NativeArray<bool> locked;

			// Token: 0x04000CF5 RID: 3317
			public NativeArray<int> maxNeighbours;

			// Token: 0x04000CF6 RID: 3318
			public NativeArray<RVOLayer> layer;

			// Token: 0x04000CF7 RID: 3319
			public NativeArray<RVOLayer> collidesWith;

			// Token: 0x04000CF8 RID: 3320
			public NativeArray<float> flowFollowingStrength;

			// Token: 0x04000CF9 RID: 3321
			public NativeArray<float3> position;

			// Token: 0x04000CFA RID: 3322
			public NativeArray<float3> collisionNormal;

			// Token: 0x04000CFB RID: 3323
			public NativeArray<bool> manuallyControlled;

			// Token: 0x04000CFC RID: 3324
			public NativeArray<float> priority;

			// Token: 0x04000CFD RID: 3325
			public NativeArray<AgentDebugFlags> debugFlags;

			// Token: 0x04000CFE RID: 3326
			public NativeArray<float3> targetPoint;

			// Token: 0x04000CFF RID: 3327
			public NativeArray<float2> allowedVelocityDeviationAngles;

			// Token: 0x04000D00 RID: 3328
			public NativeArray<NativeMovementPlane> movementPlane;

			// Token: 0x04000D01 RID: 3329
			public NativeArray<float3> endOfPath;

			// Token: 0x04000D02 RID: 3330
			public NativeArray<int> agentObstacleMapping;

			// Token: 0x04000D03 RID: 3331
			public NativeArray<int> hierarchicalNodeIndex;
		}

		// Token: 0x020002CC RID: 716
		public struct AgentOutputData
		{
			// Token: 0x060011DB RID: 4571 RVA: 0x0006C98C File Offset: 0x0006AB8C
			public void Realloc(int size, Allocator allocator)
			{
				Memory.Realloc<float3>(ref this.targetPoint, size, allocator, NativeArrayOptions.UninitializedMemory);
				Memory.Realloc<float>(ref this.speed, size, allocator, NativeArrayOptions.UninitializedMemory);
				Memory.Realloc<int>(ref this.numNeighbours, size, allocator, NativeArrayOptions.UninitializedMemory);
				Memory.Realloc<int>(ref this.blockedByAgents, size * 7, allocator, NativeArrayOptions.UninitializedMemory);
				Memory.Realloc<ReachedEndOfPath>(ref this.effectivelyReachedDestination, size, allocator, NativeArrayOptions.UninitializedMemory);
				Memory.Realloc<float>(ref this.forwardClearance, size, allocator, NativeArrayOptions.UninitializedMemory);
			}

			// Token: 0x060011DC RID: 4572 RVA: 0x0006C9F0 File Offset: 0x0006ABF0
			public void Move(int fromIndex, int toIndex)
			{
				this.targetPoint[toIndex] = this.targetPoint[fromIndex];
				this.speed[toIndex] = this.speed[fromIndex];
				this.numNeighbours[toIndex] = this.numNeighbours[fromIndex];
				this.effectivelyReachedDestination[toIndex] = this.effectivelyReachedDestination[fromIndex];
				for (int i = 0; i < 7; i++)
				{
					this.blockedByAgents[toIndex * 7 + i] = this.blockedByAgents[fromIndex * 7 + i];
				}
				this.forwardClearance[toIndex] = this.forwardClearance[fromIndex];
			}

			// Token: 0x060011DD RID: 4573 RVA: 0x0006CAA4 File Offset: 0x0006ACA4
			public void Dispose()
			{
				this.targetPoint.Dispose();
				this.speed.Dispose();
				this.numNeighbours.Dispose();
				this.blockedByAgents.Dispose();
				this.effectivelyReachedDestination.Dispose();
				this.forwardClearance.Dispose();
			}

			// Token: 0x04000D04 RID: 3332
			public NativeArray<float3> targetPoint;

			// Token: 0x04000D05 RID: 3333
			public NativeArray<float> speed;

			// Token: 0x04000D06 RID: 3334
			public NativeArray<int> numNeighbours;

			// Token: 0x04000D07 RID: 3335
			[NativeDisableParallelForRestriction]
			public NativeArray<int> blockedByAgents;

			// Token: 0x04000D08 RID: 3336
			public NativeArray<ReachedEndOfPath> effectivelyReachedDestination;

			// Token: 0x04000D09 RID: 3337
			public NativeArray<float> forwardClearance;
		}

		// Token: 0x020002CD RID: 717
		public struct HorizonAgentData
		{
			// Token: 0x060011DE RID: 4574 RVA: 0x0006CAF3 File Offset: 0x0006ACF3
			public void Realloc(int size, Allocator allocator)
			{
				Memory.Realloc<int>(ref this.horizonSide, size, allocator, NativeArrayOptions.UninitializedMemory);
				Memory.Realloc<float>(ref this.horizonMinAngle, size, allocator, NativeArrayOptions.UninitializedMemory);
				Memory.Realloc<float>(ref this.horizonMaxAngle, size, allocator, NativeArrayOptions.UninitializedMemory);
			}

			// Token: 0x060011DF RID: 4575 RVA: 0x0006CB1F File Offset: 0x0006AD1F
			public void Move(int fromIndex, int toIndex)
			{
				this.horizonSide[toIndex] = this.horizonSide[fromIndex];
			}

			// Token: 0x060011E0 RID: 4576 RVA: 0x0006CB39 File Offset: 0x0006AD39
			public void Dispose()
			{
				this.horizonSide.Dispose();
				this.horizonMinAngle.Dispose();
				this.horizonMaxAngle.Dispose();
			}

			// Token: 0x04000D0A RID: 3338
			public NativeArray<int> horizonSide;

			// Token: 0x04000D0B RID: 3339
			public NativeArray<float> horizonMinAngle;

			// Token: 0x04000D0C RID: 3340
			public NativeArray<float> horizonMaxAngle;
		}

		// Token: 0x020002CE RID: 718
		public struct TemporaryAgentData
		{
			// Token: 0x060011E1 RID: 4577 RVA: 0x0006CB5C File Offset: 0x0006AD5C
			public void Realloc(int size, Allocator allocator)
			{
				Memory.Realloc<float2>(ref this.desiredTargetPointInVelocitySpace, size, allocator, NativeArrayOptions.UninitializedMemory);
				Memory.Realloc<float3>(ref this.desiredVelocity, size, allocator, NativeArrayOptions.UninitializedMemory);
				Memory.Realloc<float3>(ref this.currentVelocity, size, allocator, NativeArrayOptions.UninitializedMemory);
				Memory.Realloc<float2>(ref this.collisionVelocityOffsets, size, allocator, NativeArrayOptions.UninitializedMemory);
				Memory.Realloc<int>(ref this.neighbours, size * 50, allocator, NativeArrayOptions.UninitializedMemory);
			}

			// Token: 0x060011E2 RID: 4578 RVA: 0x0006CBB2 File Offset: 0x0006ADB2
			public void Dispose()
			{
				this.desiredTargetPointInVelocitySpace.Dispose();
				this.desiredVelocity.Dispose();
				this.currentVelocity.Dispose();
				this.neighbours.Dispose();
				this.collisionVelocityOffsets.Dispose();
			}

			// Token: 0x04000D0D RID: 3341
			public NativeArray<float2> desiredTargetPointInVelocitySpace;

			// Token: 0x04000D0E RID: 3342
			public NativeArray<float3> desiredVelocity;

			// Token: 0x04000D0F RID: 3343
			public NativeArray<float3> currentVelocity;

			// Token: 0x04000D10 RID: 3344
			public NativeArray<float2> collisionVelocityOffsets;

			// Token: 0x04000D11 RID: 3345
			public NativeArray<int> neighbours;
		}
	}
}
