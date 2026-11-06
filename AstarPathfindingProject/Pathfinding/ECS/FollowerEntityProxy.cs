using System;
using System.Collections.Generic;
using Pathfinding.Collections;
using Pathfinding.ECS.RVO;
using Pathfinding.PID;
using Pathfinding.RVO;
using Pathfinding.Util;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Profiling;
using Unity.Transforms;
using UnityEngine;

namespace Pathfinding.ECS
{
	// Token: 0x020002E2 RID: 738
	public struct FollowerEntityProxy : IAstarAI
	{
		// Token: 0x170002CD RID: 717
		// (get) Token: 0x0600124C RID: 4684 RVA: 0x000708DE File Offset: 0x0006EADE
		// (set) Token: 0x0600124D RID: 4685 RVA: 0x000708E6 File Offset: 0x0006EAE6
		public Entity entity
		{
			[IgnoredByDeepProfiler]
			readonly get;
			private set; }

		// Token: 0x170002CE RID: 718
		// (get) Token: 0x0600124E RID: 4686 RVA: 0x000708EF File Offset: 0x0006EAEF
		// (set) Token: 0x0600124F RID: 4687 RVA: 0x000708F7 File Offset: 0x0006EAF7
		public World world
		{
			[IgnoredByDeepProfiler]
			readonly get;
			private set; }

		// Token: 0x06001250 RID: 4688 RVA: 0x00070900 File Offset: 0x0006EB00
		private static void InitScratchData()
		{
			if (!FollowerEntityProxy.nextCornersScratch.IsCreated)
			{
				if (!Application.isPlaying)
				{
					throw new InvalidOperationException("Cannot initialize scratch data outside of play mode.");
				}
				FollowerEntityProxy.nextCornersScratch = new NativeList<float3>(16, Allocator.Persistent);
				FollowerEntityProxy.indicesScratch = new NativeArray<int>(16, Allocator.Persistent, NativeArrayOptions.ClearMemory);
				Application.quitting += FollowerEntityProxy.DisposeScratchData;
			}
		}

		// Token: 0x06001251 RID: 4689 RVA: 0x0007095C File Offset: 0x0006EB5C
		private static void DisposeScratchData()
		{
			if (FollowerEntityProxy.nextCornersScratch.IsCreated)
			{
				FollowerEntityProxy.nextCornersScratch.Dispose();
			}
			if (FollowerEntityProxy.indicesScratch.IsCreated)
			{
				FollowerEntityProxy.indicesScratch.Dispose();
			}
			Application.quitting -= FollowerEntityProxy.DisposeScratchData;
		}

		// Token: 0x06001252 RID: 4690 RVA: 0x0007099B File Offset: 0x0006EB9B
		public FollowerEntityProxy(World world, Entity entity)
		{
			this.world = world;
			this.entity = entity;
		}

		// Token: 0x170002CF RID: 719
		// (get) Token: 0x06001253 RID: 4691 RVA: 0x000709AC File Offset: 0x0006EBAC
		public bool entityExists
		{
			get
			{
				return this.world != null && this.world.IsCreated && this.world.EntityManager.Exists(this.entity);
			}
		}

		// Token: 0x170002D0 RID: 720
		// (get) Token: 0x06001254 RID: 4692 RVA: 0x000709EC File Offset: 0x0006EBEC
		internal bool likelyHasReasonableComponents
		{
			get
			{
				return this.entityExists && this.world.EntityManager.HasComponent<MovementState>(this.entity) && this.world.EntityManager.HasComponent<MovementSettings>(this.entity);
			}
		}

		// Token: 0x06001255 RID: 4693 RVA: 0x00070A38 File Offset: 0x0006EC38
		private void AssertEntityExists()
		{
			if (this.world == null || !this.world.IsCreated || !this.world.EntityManager.Exists(this.entity))
			{
				throw new InvalidOperationException("Entity does not exist. You can only access this if the component is active and enabled.");
			}
		}

		// Token: 0x170002D1 RID: 721
		// (get) Token: 0x06001256 RID: 4694 RVA: 0x00070A80 File Offset: 0x0006EC80
		// (set) Token: 0x06001257 RID: 4695 RVA: 0x00070AC0 File Offset: 0x0006ECC0
		public float radius
		{
			get
			{
				ComponentRef<AgentCylinderShape> componentRef;
				if (!FollowerEntityProxy.entityStorageCache.GetComponentData<AgentCylinderShape>(this.world, this.entity, ref FollowerEntityProxy.agentCylinderShapeAccessRW, out componentRef))
				{
					return 0f;
				}
				return componentRef.value.radius;
			}
			set
			{
				ComponentRef<AgentCylinderShape> componentRef;
				if (FollowerEntityProxy.entityStorageCache.GetComponentData<AgentCylinderShape>(this.world, this.entity, ref FollowerEntityProxy.agentCylinderShapeAccessRW, out componentRef))
				{
					componentRef.value.radius = value;
				}
			}
		}

		// Token: 0x170002D2 RID: 722
		// (get) Token: 0x06001258 RID: 4696 RVA: 0x00070AFC File Offset: 0x0006ECFC
		// (set) Token: 0x06001259 RID: 4697 RVA: 0x00070B3C File Offset: 0x0006ED3C
		public float height
		{
			get
			{
				ComponentRef<AgentCylinderShape> componentRef;
				if (!FollowerEntityProxy.entityStorageCache.GetComponentData<AgentCylinderShape>(this.world, this.entity, ref FollowerEntityProxy.agentCylinderShapeAccessRO, out componentRef))
				{
					return 0f;
				}
				return componentRef.value.height;
			}
			set
			{
				ComponentRef<AgentCylinderShape> componentRef;
				if (FollowerEntityProxy.entityStorageCache.GetComponentData<AgentCylinderShape>(this.world, this.entity, ref FollowerEntityProxy.agentCylinderShapeAccessRW, out componentRef))
				{
					componentRef.value.height = value;
				}
			}
		}

		// Token: 0x170002D3 RID: 723
		// (get) Token: 0x0600125A RID: 4698 RVA: 0x00070B78 File Offset: 0x0006ED78
		public ref PathRequestSettings pathfindingSettings
		{
			get
			{
				ManagedSettings managedSettings;
				if (FollowerEntityProxy.entityStorageCache.GetComponentData<ManagedSettings>(this.world, this.entity, ref FollowerEntityProxy.managedSettingsAccessRW, out managedSettings))
				{
					return ref managedSettings.pathfindingSettings;
				}
				throw new FollowerEntityProxy.EntityDoesNotExistException();
			}
		}

		// Token: 0x170002D4 RID: 724
		// (get) Token: 0x0600125B RID: 4699 RVA: 0x00070BB0 File Offset: 0x0006EDB0
		// (set) Token: 0x0600125C RID: 4700 RVA: 0x00070C0C File Offset: 0x0006EE0C
		public unsafe RVOAgent rvoSettings
		{
			get
			{
				EntityManager entityManager;
				EntityStorageInfo entityStorageInfo;
				if (FollowerEntityProxy.entityStorageCache.Update(this.world, this.entity, out entityManager, out entityStorageInfo))
				{
					FollowerEntityProxy.rvoSettingsAccessRO.Update(entityManager);
					if (FollowerEntityProxy.rvoSettingsAccessRO.HasComponent(entityStorageInfo))
					{
						return *FollowerEntityProxy.rvoSettingsAccessRO[entityStorageInfo];
					}
				}
				throw new Exception("Local avoidance is not enabled for the entity, cannot get local avoidance settings. The RVOAgent component is missing on the entity.");
			}
			set
			{
				EntityManager entityManager;
				EntityStorageInfo entityStorageInfo;
				if (FollowerEntityProxy.entityStorageCache.Update(this.world, this.entity, out entityManager, out entityStorageInfo))
				{
					FollowerEntityProxy.rvoSettingsAccessRW.Update(entityManager);
					if (FollowerEntityProxy.rvoSettingsAccessRW.HasComponent(entityStorageInfo))
					{
						*FollowerEntityProxy.rvoSettingsAccessRW[entityStorageInfo] = value;
					}
				}
			}
		}

		// Token: 0x170002D5 RID: 725
		// (get) Token: 0x0600125D RID: 4701 RVA: 0x00070C60 File Offset: 0x0006EE60
		// (set) Token: 0x0600125E RID: 4702 RVA: 0x00070CA8 File Offset: 0x0006EEA8
		public Vector3 position
		{
			get
			{
				ComponentRef<LocalTransform> componentRef;
				return FollowerEntityProxy.entityStorageCache.GetComponentData<LocalTransform>(this.world, this.entity, ref FollowerEntityProxy.localTransformAccessRO, out componentRef) ? componentRef.value.Position : default(float3);
			}
			set
			{
				EntityManager entityManager;
				EntityStorageInfo entityStorageInfo;
				if (FollowerEntityProxy.entityStorageCache.Update(World.DefaultGameObjectInjectionWorld, this.entity, out entityManager, out entityStorageInfo))
				{
					FollowerEntityProxy.movementStateAccessRW.Update(entityManager);
					FollowerEntityProxy.managedStateAccessRW.Update(entityManager);
					FollowerEntityProxy.agentCylinderShapeAccessRO.Update(entityManager);
					FollowerEntityProxy.movementSettingsAccessRO.Update(entityManager);
					FollowerEntityProxy.destinationPointAccessRO.Update(entityManager);
					FollowerEntityProxy.movementPlaneAccessRO.Update(entityManager);
					FollowerEntityProxy.localTransformAccessRW.Update(entityManager);
					FollowerEntityProxy.readyToTraverseOffMeshLinkRW.Update(entityManager);
					FollowerEntityProxy.autoRepathPolicyRW.Update(entityManager);
					ref LocalTransform ptr = ref FollowerEntityProxy.localTransformAccessRW[entityStorageInfo];
					ptr.Position = value;
					ref MovementState ptr2 = ref FollowerEntityProxy.movementStateAccessRW[entityStorageInfo];
					ptr2.positionOffset = float3.zero;
					ManagedState managedState = FollowerEntityProxy.managedStateAccessRW[entityStorageInfo];
					if (managedState.pathTracer.hasPath)
					{
						ref AgentMovementPlane ptr3 = ref FollowerEntityProxy.movementPlaneAccessRO[entityStorageInfo];
						ushort version = managedState.pathTracer.version;
						managedState.pathTracer.UpdateStart(value, PathTracer.RepairQuality.High, ptr3.value);
						if (managedState.pathTracer.version != version)
						{
							ref AgentCylinderShape ptr4 = ref FollowerEntityProxy.agentCylinderShapeAccessRO[entityStorageInfo];
							ref MovementSettings ptr5 = ref FollowerEntityProxy.movementSettingsAccessRO[entityStorageInfo];
							ref DestinationPoint ptr6 = ref FollowerEntityProxy.destinationPointAccessRO[entityStorageInfo];
							ref AutoRepathPolicy ptr7 = ref FollowerEntityProxy.autoRepathPolicyRW[entityStorageInfo];
							EnabledRefRW<ReadyToTraverseOffMeshLink> enabledRefRW = entityStorageInfo.Chunk.GetEnabledMask<ReadyToTraverseOffMeshLink>(ref FollowerEntityProxy.readyToTraverseOffMeshLinkRW.handle).GetEnabledRefRW<ReadyToTraverseOffMeshLink>(entityStorageInfo.IndexInChunk);
							FollowerEntityProxy.InitScratchData();
							JobRepairPath.Execute(ref ptr, ref ptr2, ref ptr4, ref ptr3, ref ptr7, ref ptr6, enabledRefRW, managedState, in ptr5, FollowerEntityProxy.nextCornersScratch, ref FollowerEntityProxy.indicesScratch, Allocator.Persistent, false);
						}
					}
				}
			}
		}

		// Token: 0x170002D6 RID: 726
		// (get) Token: 0x0600125F RID: 4703 RVA: 0x00070E48 File Offset: 0x0006F048
		public bool isTraversingOffMeshLink
		{
			get
			{
				return this.entityExists && this.world.EntityManager.HasComponent<AgentOffMeshLinkTraversal>(this.entity);
			}
		}

		// Token: 0x170002D7 RID: 727
		// (get) Token: 0x06001260 RID: 4704 RVA: 0x00070E78 File Offset: 0x0006F078
		public unsafe OffMeshLinks.OffMeshLinkTracer offMeshLink
		{
			get
			{
				EntityManager entityManager;
				EntityStorageInfo entityStorageInfo;
				if (FollowerEntityProxy.entityStorageCache.Update(World.DefaultGameObjectInjectionWorld, this.entity, out entityManager, out entityStorageInfo) && entityManager.HasComponent<AgentOffMeshLinkTraversal>(this.entity))
				{
					FollowerEntityProxy.agentOffMeshLinkTraversalRO.Update(entityManager);
					AgentOffMeshLinkTraversal agentOffMeshLinkTraversal = *FollowerEntityProxy.agentOffMeshLinkTraversalRO[entityStorageInfo];
					return new OffMeshLinks.OffMeshLinkTracer(entityManager.GetComponentData<ManagedAgentOffMeshLinkTraversal>(this.entity).context.concreteLink, agentOffMeshLinkTraversal.relativeStart, agentOffMeshLinkTraversal.relativeEnd, agentOffMeshLinkTraversal.isReverse);
				}
				return default(OffMeshLinks.OffMeshLinkTracer);
			}
		}

		// Token: 0x170002D8 RID: 728
		// (get) Token: 0x06001261 RID: 4705 RVA: 0x00070F0C File Offset: 0x0006F10C
		// (set) Token: 0x06001262 RID: 4706 RVA: 0x00070F40 File Offset: 0x0006F140
		public IOffMeshLinkHandler onTraverseOffMeshLink
		{
			get
			{
				ManagedSettings managedSettings;
				if (!FollowerEntityProxy.entityStorageCache.GetComponentData<ManagedSettings>(this.world, this.entity, ref FollowerEntityProxy.managedSettingsAccessRO, out managedSettings))
				{
					return null;
				}
				return managedSettings.onTraverseOffMeshLink;
			}
			set
			{
				ManagedSettings managedSettings;
				if (FollowerEntityProxy.entityStorageCache.GetComponentData<ManagedSettings>(this.world, this.entity, ref FollowerEntityProxy.managedSettingsAccessRO, out managedSettings))
				{
					managedSettings.onTraverseOffMeshLink = value;
				}
			}
		}

		// Token: 0x170002D9 RID: 729
		// (get) Token: 0x06001263 RID: 4707 RVA: 0x00070F74 File Offset: 0x0006F174
		public GraphNode currentNode
		{
			get
			{
				ManagedState managedState;
				if (!FollowerEntityProxy.entityStorageCache.GetComponentData<ManagedState>(this.world, this.entity, ref FollowerEntityProxy.managedStateAccessRO, out managedState))
				{
					return null;
				}
				GraphNode startNode = managedState.pathTracer.startNode;
				if (startNode == null || startNode.Destroyed)
				{
					return null;
				}
				return startNode;
			}
		}

		// Token: 0x170002DA RID: 730
		// (get) Token: 0x06001264 RID: 4708 RVA: 0x00070FBC File Offset: 0x0006F1BC
		public GraphHitInfo nearestNavmeshBorder
		{
			get
			{
				if (AstarPath.active != null)
				{
					GraphHitInfo graphHitInfo;
					AstarPath.active.GetNearestBorder(this.position, this.currentNode, out graphHitInfo);
					return graphHitInfo;
				}
				return new GraphHitInfo
				{
					origin = this.position,
					point = Vector3.positiveInfinity
				};
			}
		}

		// Token: 0x170002DB RID: 731
		// (get) Token: 0x06001265 RID: 4709 RVA: 0x00071014 File Offset: 0x0006F214
		// (set) Token: 0x06001266 RID: 4710 RVA: 0x00071058 File Offset: 0x0006F258
		public Quaternion rotation
		{
			get
			{
				ComponentRef<LocalTransform> componentRef;
				if (!FollowerEntityProxy.entityStorageCache.GetComponentData<LocalTransform>(this.world, this.entity, ref FollowerEntityProxy.localTransformAccessRO, out componentRef))
				{
					return Quaternion.identity;
				}
				return componentRef.value.Rotation;
			}
			set
			{
				ComponentRef<LocalTransform> componentRef;
				if (FollowerEntityProxy.entityStorageCache.GetComponentData<LocalTransform>(this.world, this.entity, ref FollowerEntityProxy.localTransformAccessRW, out componentRef))
				{
					componentRef.value.Rotation = value;
				}
			}
		}

		// Token: 0x170002DC RID: 732
		// (get) Token: 0x06001267 RID: 4711 RVA: 0x00071098 File Offset: 0x0006F298
		// (set) Token: 0x06001268 RID: 4712 RVA: 0x000710D0 File Offset: 0x0006F2D0
		public MovementPlaneSource movementPlaneSource
		{
			get
			{
				if (!this.entityExists)
				{
					return MovementPlaneSource.Graph;
				}
				return this.world.EntityManager.GetSharedComponent<AgentMovementPlaneSource>(this.entity).value;
			}
			set
			{
				if (this.entityExists)
				{
					this.world.EntityManager.SetSharedComponent<AgentMovementPlaneSource>(this.entity, new AgentMovementPlaneSource
					{
						value = value
					});
				}
			}
		}

		// Token: 0x170002DD RID: 733
		// (get) Token: 0x06001269 RID: 4713 RVA: 0x00071110 File Offset: 0x0006F310
		// (set) Token: 0x0600126A RID: 4714 RVA: 0x00071154 File Offset: 0x0006F354
		public LayerMask groundMask
		{
			get
			{
				ComponentRef<MovementSettings> componentRef;
				if (!FollowerEntityProxy.entityStorageCache.GetComponentData<MovementSettings>(this.world, this.entity, ref FollowerEntityProxy.movementSettingsAccessRO, out componentRef))
				{
					return default(LayerMask);
				}
				return componentRef.value.groundMask;
			}
			set
			{
				ComponentRef<MovementSettings> componentRef;
				if (FollowerEntityProxy.entityStorageCache.GetComponentData<MovementSettings>(this.world, this.entity, ref FollowerEntityProxy.movementSettingsAccessRW, out componentRef))
				{
					componentRef.value.groundMask = value;
				}
			}
		}

		// Token: 0x170002DE RID: 734
		// (get) Token: 0x0600126B RID: 4715 RVA: 0x00071190 File Offset: 0x0006F390
		// (set) Token: 0x0600126C RID: 4716 RVA: 0x000711CC File Offset: 0x0006F3CC
		public PIDMovement.DebugFlags debugFlags
		{
			get
			{
				ComponentRef<MovementSettings> componentRef;
				if (!FollowerEntityProxy.entityStorageCache.GetComponentData<MovementSettings>(this.world, this.entity, ref FollowerEntityProxy.movementSettingsAccessRO, out componentRef))
				{
					return PIDMovement.DebugFlags.Nothing;
				}
				return componentRef.value.debugFlags;
			}
			set
			{
				ComponentRef<MovementSettings> componentRef;
				if (FollowerEntityProxy.entityStorageCache.GetComponentData<MovementSettings>(this.world, this.entity, ref FollowerEntityProxy.movementSettingsAccessRW, out componentRef))
				{
					componentRef.value.debugFlags = value;
				}
			}
		}

		// Token: 0x170002DF RID: 735
		// (get) Token: 0x0600126D RID: 4717 RVA: 0x00071208 File Offset: 0x0006F408
		// (set) Token: 0x0600126E RID: 4718 RVA: 0x0007124C File Offset: 0x0006F44C
		public float maxSpeed
		{
			get
			{
				ComponentRef<MovementSettings> componentRef;
				if (!FollowerEntityProxy.entityStorageCache.GetComponentData<MovementSettings>(this.world, this.entity, ref FollowerEntityProxy.movementSettingsAccessRO, out componentRef))
				{
					return 0f;
				}
				return componentRef.value.follower.speed;
			}
			set
			{
				ComponentRef<MovementSettings> componentRef;
				if (FollowerEntityProxy.entityStorageCache.GetComponentData<MovementSettings>(this.world, this.entity, ref FollowerEntityProxy.movementSettingsAccessRW, out componentRef))
				{
					componentRef.value.follower.speed = value;
				}
			}
		}

		// Token: 0x170002E0 RID: 736
		// (get) Token: 0x0600126F RID: 4719 RVA: 0x0007128C File Offset: 0x0006F48C
		// (set) Token: 0x06001270 RID: 4720 RVA: 0x000712D0 File Offset: 0x0006F4D0
		public float rotationSpeed
		{
			get
			{
				ComponentRef<MovementSettings> componentRef;
				if (!FollowerEntityProxy.entityStorageCache.GetComponentData<MovementSettings>(this.world, this.entity, ref FollowerEntityProxy.movementSettingsAccessRO, out componentRef))
				{
					return 0f;
				}
				return componentRef.value.follower.rotationSpeed;
			}
			set
			{
				ComponentRef<MovementSettings> componentRef;
				if (FollowerEntityProxy.entityStorageCache.GetComponentData<MovementSettings>(this.world, this.entity, ref FollowerEntityProxy.movementSettingsAccessRW, out componentRef))
				{
					componentRef.value.follower.rotationSpeed = value;
				}
			}
		}

		// Token: 0x170002E1 RID: 737
		// (get) Token: 0x06001271 RID: 4721 RVA: 0x00071310 File Offset: 0x0006F510
		// (set) Token: 0x06001272 RID: 4722 RVA: 0x00071354 File Offset: 0x0006F554
		public float maxRotationSpeed
		{
			get
			{
				ComponentRef<MovementSettings> componentRef;
				if (!FollowerEntityProxy.entityStorageCache.GetComponentData<MovementSettings>(this.world, this.entity, ref FollowerEntityProxy.movementSettingsAccessRO, out componentRef))
				{
					return 0f;
				}
				return componentRef.value.follower.maxRotationSpeed;
			}
			set
			{
				ComponentRef<MovementSettings> componentRef;
				if (FollowerEntityProxy.entityStorageCache.GetComponentData<MovementSettings>(this.world, this.entity, ref FollowerEntityProxy.movementSettingsAccessRW, out componentRef))
				{
					componentRef.value.follower.maxRotationSpeed = value;
				}
			}
		}

		// Token: 0x170002E2 RID: 738
		// (get) Token: 0x06001273 RID: 4723 RVA: 0x00071394 File Offset: 0x0006F594
		// (set) Token: 0x06001274 RID: 4724 RVA: 0x000713D8 File Offset: 0x0006F5D8
		public Vector3 velocity
		{
			get
			{
				ComponentRef<MovementStatistics> componentRef;
				if (!FollowerEntityProxy.entityStorageCache.GetComponentData<MovementStatistics>(this.world, this.entity, ref FollowerEntityProxy.movementStatisticsAccessRO, out componentRef))
				{
					return Vector3.zero;
				}
				return componentRef.value.estimatedVelocity;
			}
			set
			{
				ComponentRef<MovementStatistics> componentRef;
				if (FollowerEntityProxy.entityStorageCache.GetComponentData<MovementStatistics>(this.world, this.entity, ref FollowerEntityProxy.movementStatisticsAccessRW, out componentRef))
				{
					componentRef.value.estimatedVelocity = value;
				}
			}
		}

		// Token: 0x170002E3 RID: 739
		// (get) Token: 0x06001275 RID: 4725 RVA: 0x00071418 File Offset: 0x0006F618
		public Vector3 desiredVelocity
		{
			get
			{
				ComponentRef<ResolvedMovement> componentRef;
				if (FollowerEntityProxy.entityStorageCache.GetComponentData<ResolvedMovement>(this.world, this.entity, ref FollowerEntityProxy.resolvedMovementAccessRO, out componentRef))
				{
					float num = Mathf.Max(Time.deltaTime, 0.0001f);
					return Vector3.ClampMagnitude(componentRef.value.targetPoint - this.position, num * componentRef.value.speed) / num;
				}
				return Vector3.zero;
			}
		}

		// Token: 0x170002E4 RID: 740
		// (get) Token: 0x06001276 RID: 4726 RVA: 0x00071490 File Offset: 0x0006F690
		// (set) Token: 0x06001277 RID: 4727 RVA: 0x00071507 File Offset: 0x0006F707
		public Vector3 desiredVelocityWithoutLocalAvoidance
		{
			get
			{
				ComponentRef<MovementControl> componentRef;
				if (FollowerEntityProxy.entityStorageCache.GetComponentData<MovementControl>(this.world, this.entity, ref FollowerEntityProxy.movementControlAccessRO, out componentRef))
				{
					float num = Mathf.Max(Time.deltaTime, 0.0001f);
					return Vector3.ClampMagnitude(componentRef.value.targetPoint - this.position, num * componentRef.value.speed) / num;
				}
				return Vector3.zero;
			}
			set
			{
				throw new NotImplementedException("The FollowerEntity does not support setting this property. If you want to override the movement, you'll need to write a custom entity component system.");
			}
		}

		// Token: 0x170002E5 RID: 741
		// (get) Token: 0x06001278 RID: 4728 RVA: 0x00071514 File Offset: 0x0006F714
		public float remainingDistance
		{
			get
			{
				EntityManager entityManager;
				EntityStorageInfo entityStorageInfo;
				if (!FollowerEntityProxy.entityStorageCache.Update(World.DefaultGameObjectInjectionWorld, this.entity, out entityManager, out entityStorageInfo))
				{
					return float.PositiveInfinity;
				}
				FollowerEntityProxy.movementStateAccessRO.Update(entityManager);
				FollowerEntityProxy.managedStateAccessRO.Update(entityManager);
				ManagedState managedState = FollowerEntityProxy.managedStateAccessRO[entityStorageInfo];
				if (managedState.pathTracer.hasPath && !managedState.pathTracer.isStale)
				{
					return FollowerEntityProxy.movementStateAccessRO[entityStorageInfo].remainingDistanceToEndOfPart + Vector3.Distance(managedState.pathTracer.endPointOfFirstPart, managedState.pathTracer.endPoint);
				}
				return float.PositiveInfinity;
			}
		}

		// Token: 0x170002E6 RID: 742
		// (get) Token: 0x06001279 RID: 4729 RVA: 0x000715B0 File Offset: 0x0006F7B0
		// (set) Token: 0x0600127A RID: 4730 RVA: 0x000715F0 File Offset: 0x0006F7F0
		public float stopDistance
		{
			get
			{
				ComponentRef<MovementSettings> componentRef;
				if (!FollowerEntityProxy.entityStorageCache.GetComponentData<MovementSettings>(this.world, this.entity, ref FollowerEntityProxy.movementSettingsAccessRO, out componentRef))
				{
					return 0f;
				}
				return componentRef.value.stopDistance;
			}
			set
			{
				ComponentRef<MovementSettings> componentRef;
				if (FollowerEntityProxy.entityStorageCache.GetComponentData<MovementSettings>(this.world, this.entity, ref FollowerEntityProxy.movementSettingsAccessRW, out componentRef))
				{
					componentRef.value.stopDistance = value;
				}
			}
		}

		// Token: 0x170002E7 RID: 743
		// (get) Token: 0x0600127B RID: 4731 RVA: 0x0007162C File Offset: 0x0006F82C
		// (set) Token: 0x0600127C RID: 4732 RVA: 0x0007166C File Offset: 0x0006F86C
		public float positionSmoothing
		{
			get
			{
				ComponentRef<MovementSettings> componentRef;
				if (!FollowerEntityProxy.entityStorageCache.GetComponentData<MovementSettings>(this.world, this.entity, ref FollowerEntityProxy.movementSettingsAccessRO, out componentRef))
				{
					return 0f;
				}
				return componentRef.value.positionSmoothing;
			}
			set
			{
				ComponentRef<MovementSettings> componentRef;
				if (FollowerEntityProxy.entityStorageCache.GetComponentData<MovementSettings>(this.world, this.entity, ref FollowerEntityProxy.movementSettingsAccessRW, out componentRef))
				{
					componentRef.value.positionSmoothing = value;
				}
			}
		}

		// Token: 0x170002E8 RID: 744
		// (get) Token: 0x0600127D RID: 4733 RVA: 0x000716A8 File Offset: 0x0006F8A8
		// (set) Token: 0x0600127E RID: 4734 RVA: 0x000716E8 File Offset: 0x0006F8E8
		public float rotationSmoothing
		{
			get
			{
				ComponentRef<MovementSettings> componentRef;
				if (!FollowerEntityProxy.entityStorageCache.GetComponentData<MovementSettings>(this.world, this.entity, ref FollowerEntityProxy.movementSettingsAccessRO, out componentRef))
				{
					return 0f;
				}
				return componentRef.value.rotationSmoothing;
			}
			set
			{
				ComponentRef<MovementSettings> componentRef;
				if (FollowerEntityProxy.entityStorageCache.GetComponentData<MovementSettings>(this.world, this.entity, ref FollowerEntityProxy.movementSettingsAccessRW, out componentRef))
				{
					componentRef.value.rotationSmoothing = value;
				}
			}
		}

		// Token: 0x170002E9 RID: 745
		// (get) Token: 0x0600127F RID: 4735 RVA: 0x00071724 File Offset: 0x0006F924
		public bool reachedDestination
		{
			get
			{
				ComponentRef<MovementState> componentRef;
				return FollowerEntityProxy.entityStorageCache.GetComponentData<MovementState>(this.world, this.entity, ref FollowerEntityProxy.movementStateAccessRW, out componentRef) && componentRef.value.reachedDestinationAndOrientation;
			}
		}

		// Token: 0x170002EA RID: 746
		// (get) Token: 0x06001280 RID: 4736 RVA: 0x00071760 File Offset: 0x0006F960
		public bool reachedEndOfPath
		{
			get
			{
				ComponentRef<MovementState> componentRef;
				return FollowerEntityProxy.entityStorageCache.GetComponentData<MovementState>(this.world, this.entity, ref FollowerEntityProxy.movementStateAccessRW, out componentRef) && componentRef.value.reachedEndOfPathAndOrientation;
			}
		}

		// Token: 0x170002EB RID: 747
		// (get) Token: 0x06001281 RID: 4737 RVA: 0x0007179C File Offset: 0x0006F99C
		public bool reachedCrowdedEndOfPath
		{
			get
			{
				if (this.reachedEndOfPath)
				{
					return true;
				}
				if (!this.hasPath)
				{
					return false;
				}
				EntityManager entityManager = this.world.EntityManager;
				if (RVOSimulator.active != null && entityManager.HasComponent<AgentIndex>(this.entity))
				{
					AgentIndex componentData = entityManager.GetComponentData<AgentIndex>(this.entity);
					SimulatorBurst simulator = RVOSimulator.active.GetSimulator();
					int num;
					if (componentData.TryGetIndex(ref simulator.simulationData, out num) && simulator.outputData.effectivelyReachedDestination[num] == ReachedEndOfPath.Reached)
					{
						FollowerEntityProxy.managedStateAccessRO.Update(entityManager);
						ManagedState managedState = FollowerEntityProxy.managedStateAccessRO[entityManager.GetStorageInfo(this.entity)];
						Vector3 vector = simulator.simulationData.endOfPath[num];
						Vector3 endPoint = managedState.pathTracer.endPoint;
						if ((vector - endPoint).sqrMagnitude < (endPoint - this.position).sqrMagnitude * 0.1f * 0.1f)
						{
							return true;
						}
					}
				}
				return false;
			}
		}

		// Token: 0x170002EC RID: 748
		// (get) Token: 0x06001282 RID: 4738 RVA: 0x000718AC File Offset: 0x0006FAAC
		public Vector3 endOfPath
		{
			get
			{
				ManagedState managedState;
				if (FollowerEntityProxy.entityStorageCache.GetComponentData<ManagedState>(this.world, this.entity, ref FollowerEntityProxy.managedStateAccessRO, out managedState))
				{
					if (managedState.pathTracer.hasPath)
					{
						return managedState.pathTracer.endPoint;
					}
					Vector3 destination = this.destination;
					if (float.IsFinite(destination.x))
					{
						return destination;
					}
				}
				return this.position;
			}
		}

		// Token: 0x170002ED RID: 749
		// (get) Token: 0x06001283 RID: 4739 RVA: 0x00071910 File Offset: 0x0006FB10
		// (set) Token: 0x06001284 RID: 4740 RVA: 0x00071954 File Offset: 0x0006FB54
		public Vector3 destination
		{
			get
			{
				ComponentRef<DestinationPoint> componentRef;
				if (!FollowerEntityProxy.entityStorageCache.GetComponentData<DestinationPoint>(this.world, this.entity, ref FollowerEntityProxy.destinationPointAccessRO, out componentRef))
				{
					return Vector3.positiveInfinity;
				}
				return componentRef.value.destination;
			}
			set
			{
				this.SetDestination(value, default(float3));
			}
		}

		// Token: 0x170002EE RID: 750
		// (get) Token: 0x06001285 RID: 4741 RVA: 0x00071978 File Offset: 0x0006FB78
		public Vector3 destinationFacingDirection
		{
			get
			{
				ComponentRef<DestinationPoint> componentRef;
				if (!FollowerEntityProxy.entityStorageCache.GetComponentData<DestinationPoint>(this.world, this.entity, ref FollowerEntityProxy.destinationPointAccessRO, out componentRef))
				{
					return Vector3.zero;
				}
				return componentRef.value.facingDirection;
			}
		}

		// Token: 0x06001286 RID: 4742 RVA: 0x000719BC File Offset: 0x0006FBBC
		public unsafe void SetDestination(float3 destination, float3 facingDirection = default(float3))
		{
			this.AssertEntityExists();
			EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
			FollowerEntityProxy.movementStateAccessRW.Update(entityManager);
			FollowerEntityProxy.managedStateAccessRW.Update(entityManager);
			FollowerEntityProxy.agentCylinderShapeAccessRO.Update(entityManager);
			FollowerEntityProxy.movementSettingsAccessRO.Update(entityManager);
			FollowerEntityProxy.localTransformAccessRO.Update(entityManager);
			FollowerEntityProxy.autoRepathPolicyRW.Update(entityManager);
			FollowerEntityProxy.destinationPointAccessRW.Update(entityManager);
			FollowerEntityProxy.movementPlaneAccessRO.Update(entityManager);
			FollowerEntityProxy.readyToTraverseOffMeshLinkRW.Update(entityManager);
			EntityStorageInfo storageInfo = entityManager.GetStorageInfo(this.entity);
			*FollowerEntityProxy.destinationPointAccessRW[storageInfo] = new DestinationPoint
			{
				destination = destination,
				facingDirection = facingDirection
			};
			ManagedState managedState = FollowerEntityProxy.managedStateAccessRW[storageInfo];
			if (managedState.pathTracer.hasPath)
			{
				ref AgentMovementPlane ptr = ref FollowerEntityProxy.movementPlaneAccessRO[storageInfo];
				managedState.pathTracer.UpdateEnd(destination, PathTracer.RepairQuality.High, ptr.value);
				ref MovementState ptr2 = ref FollowerEntityProxy.movementStateAccessRW[storageInfo];
				if (ptr2.pathTracerVersion != managedState.pathTracer.version)
				{
					ref AgentCylinderShape ptr3 = ref FollowerEntityProxy.agentCylinderShapeAccessRO[storageInfo];
					ref MovementSettings ptr4 = ref FollowerEntityProxy.movementSettingsAccessRO[storageInfo];
					ref LocalTransform ptr5 = ref FollowerEntityProxy.localTransformAccessRO[storageInfo];
					ref AutoRepathPolicy ptr6 = ref FollowerEntityProxy.autoRepathPolicyRW[storageInfo];
					ref DestinationPoint ptr7 = ref FollowerEntityProxy.destinationPointAccessRW[storageInfo];
					EnabledRefRW<ReadyToTraverseOffMeshLink> enabledRefRW = storageInfo.Chunk.GetEnabledMask<ReadyToTraverseOffMeshLink>(ref FollowerEntityProxy.readyToTraverseOffMeshLinkRW.handle).GetEnabledRefRW<ReadyToTraverseOffMeshLink>(storageInfo.IndexInChunk);
					FollowerEntityProxy.InitScratchData();
					JobRepairPath.Execute(ref ptr5, ref ptr2, ref ptr3, ref ptr, ref ptr6, ref ptr7, enabledRefRW, managedState, in ptr4, FollowerEntityProxy.nextCornersScratch, ref FollowerEntityProxy.indicesScratch, Allocator.Persistent, false);
				}
			}
		}

		// Token: 0x170002EF RID: 751
		// (get) Token: 0x06001287 RID: 4743 RVA: 0x00071B68 File Offset: 0x0006FD68
		// (set) Token: 0x06001288 RID: 4744 RVA: 0x00071BAC File Offset: 0x0006FDAC
		public unsafe AutoRepathPolicy autoRepath
		{
			get
			{
				ComponentRef<AutoRepathPolicy> componentRef;
				if (!FollowerEntityProxy.entityStorageCache.GetComponentData<AutoRepathPolicy>(this.world, this.entity, ref FollowerEntityProxy.autoRepathPolicyRW, out componentRef))
				{
					return default(AutoRepathPolicy);
				}
				return *componentRef.value;
			}
			set
			{
				ComponentRef<AutoRepathPolicy> componentRef;
				if (FollowerEntityProxy.entityStorageCache.GetComponentData<AutoRepathPolicy>(this.world, this.entity, ref FollowerEntityProxy.autoRepathPolicyRW, out componentRef))
				{
					*componentRef.value = value;
				}
			}
		}

		// Token: 0x170002F0 RID: 752
		// (get) Token: 0x06001289 RID: 4745 RVA: 0x00071BE5 File Offset: 0x0006FDE5
		// (set) Token: 0x0600128A RID: 4746 RVA: 0x00071BF8 File Offset: 0x0006FDF8
		[Obsolete("This has been superseded by autoRepath.mode")]
		public bool canSearch
		{
			get
			{
				return this.autoRepath.mode > AutoRepathPolicy.Mode.Never;
			}
			set
			{
				if (value)
				{
					AutoRepathPolicy autoRepath = this.autoRepath;
					if (autoRepath.mode == AutoRepathPolicy.Mode.Never)
					{
						autoRepath.mode = AutoRepathPolicy.Mode.EveryNSeconds;
						this.autoRepath = autoRepath;
						return;
					}
				}
				else
				{
					AutoRepathPolicy autoRepath2 = this.autoRepath;
					autoRepath2.mode = AutoRepathPolicy.Mode.Never;
					this.autoRepath = autoRepath2;
				}
			}
		}

		// Token: 0x170002F1 RID: 753
		// (get) Token: 0x0600128B RID: 4747 RVA: 0x00071C40 File Offset: 0x0006FE40
		// (set) Token: 0x0600128C RID: 4748 RVA: 0x00071C70 File Offset: 0x0006FE70
		public bool simulateMovement
		{
			get
			{
				return this.entityExists && this.world.EntityManager.HasComponent<SimulateMovement>(this.entity);
			}
			set
			{
				FollowerEntityProxy.ToggleComponent<SimulateMovement>(this.world, this.entity, value, true);
			}
		}

		// Token: 0x170002F2 RID: 754
		// (get) Token: 0x0600128D RID: 4749 RVA: 0x00071C88 File Offset: 0x0006FE88
		public NativeMovementPlane movementPlane
		{
			get
			{
				ComponentRef<AgentMovementPlane> componentRef;
				if (!FollowerEntityProxy.entityStorageCache.GetComponentData<AgentMovementPlane>(this.world, this.entity, ref FollowerEntityProxy.movementPlaneAccessRO, out componentRef))
				{
					return new NativeMovementPlane(this.rotation);
				}
				return componentRef.value.value;
			}
		}

		// Token: 0x170002F3 RID: 755
		// (get) Token: 0x0600128E RID: 4750 RVA: 0x00071CD4 File Offset: 0x0006FED4
		// (set) Token: 0x0600128F RID: 4751 RVA: 0x00071D04 File Offset: 0x0006FF04
		public bool enableGravity
		{
			get
			{
				return this.entityExists && this.world.EntityManager.IsComponentEnabled<GravityState>(this.entity);
			}
			set
			{
				FollowerEntityProxy.ToggleComponentEnabled<GravityState>(this.world, this.entity, value, false);
			}
		}

		// Token: 0x170002F4 RID: 756
		// (get) Token: 0x06001290 RID: 4752 RVA: 0x00071D1C File Offset: 0x0006FF1C
		public bool enableLocalAvoidance
		{
			get
			{
				return this.entityExists && this.world.EntityManager.HasComponent<RVOAgent>(this.entity);
			}
		}

		// Token: 0x170002F5 RID: 757
		// (get) Token: 0x06001291 RID: 4753 RVA: 0x00071D4C File Offset: 0x0006FF4C
		public bool localAvoidanceTemporarilyDisabled
		{
			get
			{
				return this.entityExists && this.world.EntityManager.HasComponent<AgentOffMeshLinkLocalAvoidanceDisabled>(this.entity) && this.world.EntityManager.IsComponentEnabled<AgentOffMeshLinkLocalAvoidanceDisabled>(this.entity);
			}
		}

		// Token: 0x170002F6 RID: 758
		// (get) Token: 0x06001292 RID: 4754 RVA: 0x00071D98 File Offset: 0x0006FF98
		// (set) Token: 0x06001293 RID: 4755 RVA: 0x00071DC8 File Offset: 0x0006FFC8
		public bool updatePosition
		{
			get
			{
				return this.entityExists && this.world.EntityManager.HasComponent<SyncPositionWithTransform>(this.entity);
			}
			set
			{
				FollowerEntityProxy.ToggleComponent<SyncPositionWithTransform>(this.world, this.entity, value, false);
			}
		}

		// Token: 0x170002F7 RID: 759
		// (get) Token: 0x06001294 RID: 4756 RVA: 0x00071DE0 File Offset: 0x0006FFE0
		// (set) Token: 0x06001295 RID: 4757 RVA: 0x00071E10 File Offset: 0x00070010
		public bool updateRotation
		{
			get
			{
				return this.entityExists && this.world.EntityManager.HasComponent<SyncRotationWithTransform>(this.entity);
			}
			set
			{
				FollowerEntityProxy.ToggleComponent<SyncRotationWithTransform>(this.world, this.entity, value, false);
			}
		}

		// Token: 0x170002F8 RID: 760
		// (get) Token: 0x06001296 RID: 4758 RVA: 0x00071E28 File Offset: 0x00070028
		// (set) Token: 0x06001297 RID: 4759 RVA: 0x00071E5B File Offset: 0x0007005B
		public OrientationMode orientation
		{
			get
			{
				if (!this.entityExists || !this.world.EntityManager.HasComponent<OrientationYAxisForward>(this.entity))
				{
					return OrientationMode.ZAxisForward;
				}
				return OrientationMode.YAxisForward;
			}
			set
			{
				FollowerEntityProxy.ToggleComponent<OrientationYAxisForward>(this.world, this.entity, value == OrientationMode.YAxisForward, false);
			}
		}

		// Token: 0x170002F9 RID: 761
		// (get) Token: 0x06001298 RID: 4760 RVA: 0x00071E74 File Offset: 0x00070074
		public bool hasPath
		{
			get
			{
				ManagedState managedState;
				return FollowerEntityProxy.entityStorageCache.GetComponentData<ManagedState>(this.world, this.entity, ref FollowerEntityProxy.managedStateAccessRO, out managedState) && !managedState.pathTracer.isStale;
			}
		}

		// Token: 0x170002FA RID: 762
		// (get) Token: 0x06001299 RID: 4761 RVA: 0x00071EB0 File Offset: 0x000700B0
		public bool pathPending
		{
			get
			{
				ManagedState managedState;
				return FollowerEntityProxy.entityStorageCache.GetComponentData<ManagedState>(this.world, this.entity, ref FollowerEntityProxy.managedStateAccessRO, out managedState) && managedState.pendingPath != null;
			}
		}

		// Token: 0x170002FB RID: 763
		// (get) Token: 0x0600129A RID: 4762 RVA: 0x00071EE8 File Offset: 0x000700E8
		// (set) Token: 0x0600129B RID: 4763 RVA: 0x00071F24 File Offset: 0x00070124
		public bool isStopped
		{
			get
			{
				ComponentRef<MovementSettings> componentRef;
				return FollowerEntityProxy.entityStorageCache.GetComponentData<MovementSettings>(this.world, this.entity, ref FollowerEntityProxy.movementSettingsAccessRO, out componentRef) && componentRef.value.isStopped;
			}
			set
			{
				ComponentRef<MovementSettings> componentRef;
				if (FollowerEntityProxy.entityStorageCache.GetComponentData<MovementSettings>(this.world, this.entity, ref FollowerEntityProxy.movementSettingsAccessRW, out componentRef))
				{
					componentRef.value.isStopped = value;
				}
			}
		}

		// Token: 0x170002FC RID: 764
		// (get) Token: 0x0600129C RID: 4764 RVA: 0x00071F60 File Offset: 0x00070160
		// (set) Token: 0x0600129D RID: 4765 RVA: 0x00071FA4 File Offset: 0x000701A4
		public unsafe MovementSettings movementSettings
		{
			get
			{
				ComponentRef<MovementSettings> componentRef;
				if (!FollowerEntityProxy.entityStorageCache.GetComponentData<MovementSettings>(this.world, this.entity, ref FollowerEntityProxy.movementSettingsAccessRO, out componentRef))
				{
					return default(MovementSettings);
				}
				return *componentRef.value;
			}
			set
			{
				ComponentRef<MovementSettings> componentRef;
				if (FollowerEntityProxy.entityStorageCache.GetComponentData<MovementSettings>(this.world, this.entity, ref FollowerEntityProxy.movementSettingsAccessRW, out componentRef))
				{
					*componentRef.value = value;
				}
			}
		}

		// Token: 0x170002FD RID: 765
		// (get) Token: 0x0600129E RID: 4766 RVA: 0x00071FE0 File Offset: 0x000701E0
		public Vector3 steeringTarget
		{
			get
			{
				ComponentRef<MovementState> componentRef;
				if (!FollowerEntityProxy.entityStorageCache.GetComponentData<MovementState>(this.world, this.entity, ref FollowerEntityProxy.movementStateAccessRO, out componentRef))
				{
					return this.position;
				}
				return componentRef.value.nextCorner;
			}
		}

		// Token: 0x170002FE RID: 766
		// (get) Token: 0x0600129F RID: 4767 RVA: 0x00072024 File Offset: 0x00070224
		public ManagedMovementOverrides movementOverrides
		{
			get
			{
				return new ManagedMovementOverrides(this.entity, this.world);
			}
		}

		// Token: 0x170002FF RID: 767
		// (get) Token: 0x060012A0 RID: 4768 RVA: 0x00003792 File Offset: 0x00001992
		// (set) Token: 0x060012A1 RID: 4769 RVA: 0x00007DCF File Offset: 0x00005FCF
		Action IAstarAI.onSearchPath
		{
			get
			{
				return null;
			}
			set
			{
				throw new NotImplementedException("The FollowerEntity does not support this property.");
			}
		}

		// Token: 0x060012A2 RID: 4770 RVA: 0x00007DE8 File Offset: 0x00005FE8
		void IAstarAI.FinalizeMovement(Vector3 nextPosition, Quaternion nextRotation)
		{
			throw new InvalidOperationException("The FollowerEntity component does not support FinalizeMovement. Use an ECS system to override movement instead, or use the movementOverrides property. If you just want to move the agent to a position, set ai.position or call ai.Teleport.");
		}

		// Token: 0x060012A3 RID: 4771 RVA: 0x00072037 File Offset: 0x00070237
		public void GetRemainingPath(List<Vector3> buffer, out bool stale)
		{
			this.GetRemainingPath(buffer, null, out stale);
		}

		// Token: 0x060012A4 RID: 4772 RVA: 0x00072044 File Offset: 0x00070244
		public void GetRemainingPath(List<Vector3> buffer, List<PathPartWithLinkInfo> partsBuffer, out bool stale)
		{
			buffer.Clear();
			if (partsBuffer != null)
			{
				partsBuffer.Clear();
			}
			if (!this.entityExists)
			{
				buffer.Add(this.position);
				if (partsBuffer != null)
				{
					partsBuffer.Add(new PathPartWithLinkInfo
					{
						startIndex = 0,
						endIndex = 0
					});
				}
				stale = true;
				return;
			}
			ManagedState componentData = this.world.EntityManager.GetComponentData<ManagedState>(this.entity);
			stale = false;
			if (componentData.pathTracer.hasPath)
			{
				NativeList<float3> nativeList = new NativeList<float3>(Allocator.Temp);
				NativeArray<int> nativeArray = new NativeArray<int>(8, Allocator.Temp, NativeArrayOptions.ClearMemory);
				componentData.pathTracer.GetNextCorners(nativeList, int.MaxValue, ref nativeArray, Allocator.Temp, true);
				if (partsBuffer != null)
				{
					partsBuffer.Add(new PathPartWithLinkInfo(0, nativeList.Length - 1, default(OffMeshLinks.OffMeshLinkTracer)));
				}
				if (componentData.pathTracer.partCount > 1)
				{
					PathTracer pathTracer = componentData.pathTracer.Clone();
					while (pathTracer.partCount > 1)
					{
						pathTracer.PopParts(1);
						int length = nativeList.Length;
						if (pathTracer.GetPartType(0) == Funnel.PartType.NodeSequence)
						{
							pathTracer.GetNextCorners(nativeList, int.MaxValue, ref nativeArray, Allocator.Temp, true);
							if (partsBuffer != null)
							{
								partsBuffer.Add(new PathPartWithLinkInfo(length, nativeList.Length - 1, default(OffMeshLinks.OffMeshLinkTracer)));
							}
						}
						else
						{
							if (pathTracer.PartContainsDestroyedNodes(0))
							{
								stale = true;
								break;
							}
							if (partsBuffer != null)
							{
								partsBuffer.Add(new PathPartWithLinkInfo(length - 1, length, pathTracer.GetLinkInfo(0)));
							}
						}
						stale |= pathTracer.isStale;
					}
					pathTracer.Dispose();
				}
				nativeList.AsUnsafeSpan<float3>().Reinterpret<Vector3>().CopyTo(buffer);
			}
			else
			{
				buffer.Add(this.position);
				if (partsBuffer != null)
				{
					partsBuffer.Add(new PathPartWithLinkInfo
					{
						startIndex = 0,
						endIndex = 0
					});
				}
			}
			stale |= componentData.pathTracer.isStale;
		}

		// Token: 0x060012A5 RID: 4773 RVA: 0x0007222B File Offset: 0x0007042B
		public void Move(Vector3 deltaPosition)
		{
			this.position += deltaPosition;
		}

		// Token: 0x060012A6 RID: 4774 RVA: 0x00007E27 File Offset: 0x00006027
		void IAstarAI.MovementUpdate(float deltaTime, out Vector3 nextPosition, out Quaternion nextRotation)
		{
			throw new InvalidOperationException("The FollowerEntity component does not support MovementUpdate. Use an ECS system to override movement instead, or use the movementOverrides property");
		}

		// Token: 0x060012A7 RID: 4775 RVA: 0x00072240 File Offset: 0x00070440
		public void SearchPath()
		{
			Vector3 destination = this.destination;
			if (!float.IsFinite(destination.x))
			{
				return;
			}
			ManagedSettings managedSettings;
			if (FollowerEntityProxy.entityStorageCache.GetComponentData<ManagedSettings>(this.world, this.entity, ref FollowerEntityProxy.managedSettingsAccessRO, out managedSettings))
			{
				NativeMovementPlane movementPlane = this.movementPlane;
				ABPath abpath = ABPath.Construct(this.position, destination, null);
				abpath.UseSettings(managedSettings.pathfindingSettings);
				this.SetPath(abpath, false);
			}
		}

		// Token: 0x060012A8 RID: 4776 RVA: 0x000722AC File Offset: 0x000704AC
		internal void CancelCurrentPathRequest()
		{
			ManagedState managedState;
			if (FollowerEntityProxy.entityStorageCache.GetComponentData<ManagedState>(this.world, this.entity, ref FollowerEntityProxy.managedStateAccessRO, out managedState))
			{
				managedState.CancelCurrentPathRequest();
			}
		}

		// Token: 0x060012A9 RID: 4777 RVA: 0x000722E0 File Offset: 0x000704E0
		internal void ClearPath()
		{
			EntityManager entityManager;
			EntityStorageInfo entityStorageInfo;
			if (FollowerEntityProxy.entityStorageCache.Update(this.world, this.entity, out entityManager, out entityStorageInfo))
			{
				FollowerEntityProxy.agentOffMeshLinkTraversalRO.Update(entityManager);
				if (FollowerEntityProxy.agentOffMeshLinkTraversalRO.HasComponent(entityStorageInfo))
				{
					ManagedAgentOffMeshLinkTraversal componentData = entityManager.GetComponentData<ManagedAgentOffMeshLinkTraversal>(this.entity);
					if (componentData.stateMachine != null)
					{
						componentData.stateMachine.OnAbortTraversingOffMeshLink();
					}
					componentData.context.Restore();
					entityManager.RemoveComponent<AgentOffMeshLinkTraversal>(this.entity);
					entityManager.RemoveComponent<ManagedAgentOffMeshLinkTraversal>(this.entity);
					FollowerEntityProxy.entityStorageCache.Update(this.world, this.entity, out entityManager, out entityStorageInfo);
				}
				entityManager.SetComponentEnabled<ReadyToTraverseOffMeshLink>(this.entity, false);
				FollowerEntityProxy.managedStateAccessRW.Update(entityManager);
				FollowerEntityProxy.movementStateAccessRW.Update(entityManager);
				FollowerEntityProxy.localTransformAccessRO.Update(entityManager);
				FollowerEntityProxy.movementPlaneAccessRO.Update(entityManager);
				FollowerEntityProxy.resolvedMovementAccessRW.Update(entityManager);
				FollowerEntityProxy.movementControlAccessRW.Update(entityManager);
				ref MovementState ptr = ref FollowerEntityProxy.movementStateAccessRW[entityStorageInfo];
				ref LocalTransform ptr2 = ref FollowerEntityProxy.localTransformAccessRO[entityStorageInfo];
				ref AgentMovementPlane ptr3 = ref FollowerEntityProxy.movementPlaneAccessRO[entityStorageInfo];
				ref ResolvedMovement ptr4 = ref FollowerEntityProxy.resolvedMovementAccessRW[entityStorageInfo];
				ref MovementControl ptr5 = ref FollowerEntityProxy.movementControlAccessRW[entityStorageInfo];
				ManagedState managedState = FollowerEntityProxy.managedStateAccessRW[entityStorageInfo];
				managedState.ClearPath();
				managedState.CancelCurrentPathRequest();
				ptr.SetPathIsEmpty(ptr2.Position);
				FollowerEntityProxy.ResetControl(ref ptr4, ref ptr5, ref ptr3, ptr2.Position, ptr2.Rotation, ptr.endOfPath);
			}
		}

		// Token: 0x060012AA RID: 4778 RVA: 0x0007245C File Offset: 0x0007065C
		internal static void ToggleComponent<T>(World world, Entity entity, bool enabled, bool mustExist) where T : struct, IComponentData
		{
			if (world == null || !world.IsCreated || !world.EntityManager.Exists(entity))
			{
				if (!mustExist)
				{
					throw new InvalidOperationException("Entity does not exist. You can only access this if the component is active and enabled.");
				}
				return;
			}
			else
			{
				if (enabled)
				{
					world.EntityManager.AddComponent<T>(entity);
					return;
				}
				world.EntityManager.RemoveComponent<T>(entity);
				return;
			}
		}

		// Token: 0x060012AB RID: 4779 RVA: 0x000724B8 File Offset: 0x000706B8
		internal static void ToggleComponentEnabled<T>(World world, Entity entity, bool enabled, bool mustExist) where T : struct, IComponentData, IEnableableComponent
		{
			if (world != null && world.IsCreated && world.EntityManager.Exists(entity))
			{
				world.EntityManager.SetComponentEnabled<T>(entity, enabled);
				return;
			}
			if (!mustExist)
			{
				throw new InvalidOperationException("Entity does not exist. You can only access this if the component is active and enabled.");
			}
		}

		// Token: 0x060012AC RID: 4780 RVA: 0x00072500 File Offset: 0x00070700
		internal static void ResetControl(ref ResolvedMovement resolvedMovement, ref MovementControl controlOutput, ref AgentMovementPlane movementPlane, float3 position, quaternion rotation, float3 endOfPath)
		{
			resolvedMovement.targetPoint = position;
			resolvedMovement.speed = 0f;
			resolvedMovement.targetRotation = (resolvedMovement.targetRotationHint = (controlOutput.targetRotation = (controlOutput.targetRotationHint = movementPlane.value.ToPlane(rotation))));
			controlOutput.endOfPath = endOfPath;
			controlOutput.speed = 0f;
			controlOutput.targetPoint = position;
			controlOutput.hierarchicalNodeIndex = -1;
		}

		// Token: 0x060012AD RID: 4781 RVA: 0x00072570 File Offset: 0x00070770
		public void SetPath(Path path, bool updateDestinationFromPath = true)
		{
			EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
			if (!entityManager.Exists(this.entity))
			{
				throw new InvalidOperationException("Entity does not exist. You can only assign a path if the component is active and enabled.");
			}
			FollowerEntityProxy.managedStateAccessRW.Update(entityManager);
			FollowerEntityProxy.movementPlaneAccessRO.Update(entityManager);
			FollowerEntityProxy.agentOffMeshLinkTraversalRO.Update(entityManager);
			FollowerEntityProxy.movementStateAccessRW.Update(entityManager);
			FollowerEntityProxy.localTransformAccessRO.Update(entityManager);
			FollowerEntityProxy.destinationPointAccessRW.Update(entityManager);
			FollowerEntityProxy.autoRepathPolicyRW.Update(entityManager);
			EntityStorageInfo storageInfo = entityManager.GetStorageInfo(this.entity);
			if (FollowerEntityProxy.agentOffMeshLinkTraversalRO.HasComponent(storageInfo))
			{
				return;
			}
			if (path == null)
			{
				this.ClearPath();
				return;
			}
			ManagedState managedState = FollowerEntityProxy.managedStateAccessRW[storageInfo];
			ref AgentMovementPlane ptr = ref FollowerEntityProxy.movementPlaneAccessRO[storageInfo];
			ref MovementState ptr2 = ref FollowerEntityProxy.movementStateAccessRW[storageInfo];
			ref LocalTransform ptr3 = ref FollowerEntityProxy.localTransformAccessRO[storageInfo];
			ref DestinationPoint ptr4 = ref FollowerEntityProxy.destinationPointAccessRW[storageInfo];
			ref AutoRepathPolicy ptr5 = ref FollowerEntityProxy.autoRepathPolicyRW[storageInfo];
			if (updateDestinationFromPath)
			{
				ABPath abpath = path as ABPath;
				if (abpath != null)
				{
					if (abpath.endPointKnownBeforeCalculation || abpath.IsDone())
					{
						ref DestinationPoint ptr6 = ref ptr4;
						DestinationPoint destinationPoint = new DestinationPoint
						{
							destination = abpath.originalEndPoint,
							facingDirection = default(float3)
						};
						ptr6 = destinationPoint;
					}
					else
					{
						ref DestinationPoint ptr7 = ref ptr4;
						DestinationPoint destinationPoint = new DestinationPoint
						{
							destination = Vector3.positiveInfinity,
							facingDirection = default(float3)
						};
						ptr7 = destinationPoint;
					}
				}
			}
			path.nearestNodeDistanceMetric = DistanceMetric.ClosestAsSeenFromAboveSoft(ptr.value.up);
			ptr5.OnScheduledPathRecalculation(ptr4.destination, (float)World.DefaultGameObjectInjectionWorld.Time.ElapsedTime);
			if (path.IsDone())
			{
				ptr5.OnPathCalculated(path.error);
			}
			ManagedState.SetPath(path, managedState, in ptr, ref ptr4);
			if (managedState.activePath == path)
			{
				FollowerEntityProxy.agentCylinderShapeAccessRO.Update(entityManager);
				FollowerEntityProxy.movementSettingsAccessRO.Update(entityManager);
				FollowerEntityProxy.readyToTraverseOffMeshLinkRW.Update(entityManager);
				managedState.pathTracer.UpdateStart(ptr3.Position, PathTracer.RepairQuality.High, ptr.value);
				managedState.pathTracer.UpdateEnd(ptr4.destination, PathTracer.RepairQuality.High, ptr.value);
				if (ptr2.pathTracerVersion != managedState.pathTracer.version)
				{
					FollowerEntityProxy.InitScratchData();
					ref AgentCylinderShape ptr8 = ref FollowerEntityProxy.agentCylinderShapeAccessRO[storageInfo];
					ref MovementSettings ptr9 = ref FollowerEntityProxy.movementSettingsAccessRO[storageInfo];
					EnabledRefRW<ReadyToTraverseOffMeshLink> enabledRefRW = storageInfo.Chunk.GetEnabledMask<ReadyToTraverseOffMeshLink>(ref FollowerEntityProxy.readyToTraverseOffMeshLinkRW.handle).GetEnabledRefRW<ReadyToTraverseOffMeshLink>(storageInfo.IndexInChunk);
					JobRepairPath.Execute(ref ptr3, ref ptr2, ref ptr8, ref ptr, ref ptr5, ref ptr4, enabledRefRW, managedState, in ptr9, FollowerEntityProxy.nextCornersScratch, ref FollowerEntityProxy.indicesScratch, Allocator.Persistent, false);
				}
			}
		}

		// Token: 0x060012AE RID: 4782 RVA: 0x00072828 File Offset: 0x00070A28
		public void Teleport(Vector3 newPosition, bool clearPath = true)
		{
			if (!this.entityExists)
			{
				return;
			}
			if (clearPath)
			{
				this.ClearPath();
			}
			EntityManager entityManager = this.world.EntityManager;
			FollowerEntityProxy.movementOutputAccessRW.Update(entityManager);
			FollowerEntityProxy.managedStateAccessRW.Update(entityManager);
			FollowerEntityProxy.managedSettingsAccessRO.Update(entityManager);
			FollowerEntityProxy.movementPlaneAccessRW.Update(entityManager);
			FollowerEntityProxy.resolvedMovementAccessRW.Update(entityManager);
			FollowerEntityProxy.movementControlAccessRW.Update(entityManager);
			EntityStorageInfo storageInfo = entityManager.GetStorageInfo(this.entity);
			FollowerEntityProxy.movementOutputAccessRW[storageInfo].lastPosition = newPosition;
			ManagedState managedState = FollowerEntityProxy.managedStateAccessRW[storageInfo];
			ManagedSettings managedSettings = FollowerEntityProxy.managedSettingsAccessRO[storageInfo];
			if (clearPath)
			{
				managedState.CancelCurrentPathRequest();
			}
			if (!managedState.pathTracer.hasPath && AstarPath.active != null)
			{
				NNInfo nearest = AstarPath.active.GetNearest(newPosition, managedSettings.pathfindingSettings.ToNearestNodeConstraint());
				if (nearest.node != null)
				{
					ref AgentMovementPlane ptr = ref FollowerEntityProxy.movementPlaneAccessRW[storageInfo];
					ref ResolvedMovement ptr2 = ref FollowerEntityProxy.resolvedMovementAccessRW[storageInfo];
					ref MovementControl ptr3 = ref FollowerEntityProxy.movementControlAccessRW[storageInfo];
					if (this.movementPlaneSource == MovementPlaneSource.Graph)
					{
						quaternion quaternion = ptr.value.ToWorldRotation(ptr2.targetRotation);
						quaternion quaternion2 = ptr.value.ToWorldRotation(ptr2.targetRotationHint);
						quaternion quaternion3 = ptr.value.ToWorldRotation(ptr3.targetRotation);
						quaternion quaternion4 = ptr.value.ToWorldRotation(ptr3.targetRotationHint);
						ptr = new AgentMovementPlane(MovementPlaneFromGraphSystem.MovementPlaneFromGraph(nearest.node.Graph));
						ptr2.targetRotation = ptr.value.ToPlane(quaternion);
						ptr2.targetRotationHint = ptr.value.ToPlane(quaternion2);
						ptr3.targetRotation = ptr.value.ToPlane(quaternion3);
						ptr3.targetRotationHint = ptr.value.ToPlane(quaternion4);
					}
					managedState.pathTracer.SetFromSingleNode(nearest.node, nearest.position, ptr.value, managedSettings.pathfindingSettings.ToTraversalConstraint(), managedSettings.pathfindingSettings.ToTraversalCosts());
				}
			}
			this.position = newPosition;
		}

		// Token: 0x060012AF RID: 4783 RVA: 0x00072A54 File Offset: 0x00070C54
		public void Destroy()
		{
			if (this.entityExists)
			{
				this.world.EntityManager.DestroyEntity(this.entity);
			}
			this = default(FollowerEntityProxy);
		}

		// Token: 0x04000D80 RID: 3456
		private static NativeList<float3> nextCornersScratch;

		// Token: 0x04000D81 RID: 3457
		private static NativeArray<int> indicesScratch;

		// Token: 0x04000D82 RID: 3458
		internal static EntityAccess<DestinationPoint> destinationPointAccessRW = new EntityAccess<DestinationPoint>(false);

		// Token: 0x04000D83 RID: 3459
		internal static EntityAccess<DestinationPoint> destinationPointAccessRO = new EntityAccess<DestinationPoint>(true);

		// Token: 0x04000D84 RID: 3460
		internal static EntityAccess<AgentMovementPlane> movementPlaneAccessRW = new EntityAccess<AgentMovementPlane>(false);

		// Token: 0x04000D85 RID: 3461
		internal static EntityAccess<AgentMovementPlane> movementPlaneAccessRO = new EntityAccess<AgentMovementPlane>(true);

		// Token: 0x04000D86 RID: 3462
		internal static EntityAccess<MovementState> movementStateAccessRW = new EntityAccess<MovementState>(false);

		// Token: 0x04000D87 RID: 3463
		internal static EntityAccess<MovementState> movementStateAccessRO = new EntityAccess<MovementState>(true);

		// Token: 0x04000D88 RID: 3464
		internal static EntityAccess<MovementStatistics> movementOutputAccessRW = new EntityAccess<MovementStatistics>(false);

		// Token: 0x04000D89 RID: 3465
		internal static EntityAccess<ResolvedMovement> resolvedMovementAccessRO = new EntityAccess<ResolvedMovement>(true);

		// Token: 0x04000D8A RID: 3466
		internal static EntityAccess<ResolvedMovement> resolvedMovementAccessRW = new EntityAccess<ResolvedMovement>(false);

		// Token: 0x04000D8B RID: 3467
		internal static EntityAccess<MovementControl> movementControlAccessRO = new EntityAccess<MovementControl>(true);

		// Token: 0x04000D8C RID: 3468
		internal static EntityAccess<MovementControl> movementControlAccessRW = new EntityAccess<MovementControl>(false);

		// Token: 0x04000D8D RID: 3469
		internal static EntityAccess<MovementStatistics> movementStatisticsAccessRW = new EntityAccess<MovementStatistics>(false);

		// Token: 0x04000D8E RID: 3470
		internal static EntityAccess<MovementStatistics> movementStatisticsAccessRO = new EntityAccess<MovementStatistics>(true);

		// Token: 0x04000D8F RID: 3471
		internal static ManagedEntityAccess<ManagedState> managedStateAccessRO = new ManagedEntityAccess<ManagedState>(true);

		// Token: 0x04000D90 RID: 3472
		internal static ManagedEntityAccess<ManagedState> managedStateAccessRW = new ManagedEntityAccess<ManagedState>(false);

		// Token: 0x04000D91 RID: 3473
		internal static ManagedEntityAccess<ManagedSettings> managedSettingsAccessRO = new ManagedEntityAccess<ManagedSettings>(true);

		// Token: 0x04000D92 RID: 3474
		internal static ManagedEntityAccess<ManagedSettings> managedSettingsAccessRW = new ManagedEntityAccess<ManagedSettings>(false);

		// Token: 0x04000D93 RID: 3475
		internal static EntityAccess<AutoRepathPolicy> autoRepathPolicyRW = new EntityAccess<AutoRepathPolicy>(false);

		// Token: 0x04000D94 RID: 3476
		internal static EntityAccess<LocalTransform> localTransformAccessRO = new EntityAccess<LocalTransform>(true);

		// Token: 0x04000D95 RID: 3477
		internal static EntityAccess<LocalTransform> localTransformAccessRW = new EntityAccess<LocalTransform>(false);

		// Token: 0x04000D96 RID: 3478
		internal static EntityAccess<AgentCylinderShape> agentCylinderShapeAccessRO = new EntityAccess<AgentCylinderShape>(true);

		// Token: 0x04000D97 RID: 3479
		internal static EntityAccess<AgentCylinderShape> agentCylinderShapeAccessRW = new EntityAccess<AgentCylinderShape>(false);

		// Token: 0x04000D98 RID: 3480
		internal static EntityAccess<MovementSettings> movementSettingsAccessRO = new EntityAccess<MovementSettings>(true);

		// Token: 0x04000D99 RID: 3481
		internal static EntityAccess<MovementSettings> movementSettingsAccessRW = new EntityAccess<MovementSettings>(false);

		// Token: 0x04000D9A RID: 3482
		internal static EntityAccess<AgentOffMeshLinkTraversal> agentOffMeshLinkTraversalRO = new EntityAccess<AgentOffMeshLinkTraversal>(true);

		// Token: 0x04000D9B RID: 3483
		internal static EntityAccess<ReadyToTraverseOffMeshLink> readyToTraverseOffMeshLinkRW = new EntityAccess<ReadyToTraverseOffMeshLink>(false);

		// Token: 0x04000D9C RID: 3484
		internal static EntityAccess<RVOAgent> rvoSettingsAccessRO = new EntityAccess<RVOAgent>(true);

		// Token: 0x04000D9D RID: 3485
		internal static EntityAccess<RVOAgent> rvoSettingsAccessRW = new EntityAccess<RVOAgent>(false);

		// Token: 0x04000D9E RID: 3486
		internal static EntityStorageCache entityStorageCache;

		// Token: 0x020002E3 RID: 739
		private class EntityDoesNotExistException : Exception
		{
			// Token: 0x060012B1 RID: 4785 RVA: 0x00072BCD File Offset: 0x00070DCD
			public EntityDoesNotExistException()
				: base("The entity does not exist. This can happen if the component is not enabled, or if the game is not running.")
			{
			}
		}
	}
}
