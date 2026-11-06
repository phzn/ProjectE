using System;
using System.Collections.Generic;
using Pathfinding.Drawing;
using Pathfinding.ECS;
using Pathfinding.ECS.RVO;
using Pathfinding.PID;
using Pathfinding.Serialization;
using Pathfinding.Util;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Profiling;
using Unity.Transforms;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x02000018 RID: 24
	[AddComponentMenu("Pathfinding/AI/Follower Entity (2D,3D)")]
	[UniqueComponent(tag = "ai")]
	[UniqueComponent(tag = "rvo")]
	[DisallowMultipleComponent]
	public sealed class FollowerEntity : VersionedMonoBehaviour, IAstarAI, ISerializationCallbackReceiver
	{
		// Token: 0x1700004C RID: 76
		// (get) Token: 0x06000136 RID: 310 RVA: 0x00007099 File Offset: 0x00005299
		public Entity entity
		{
			[IgnoredByDeepProfiler]
			get
			{
				return this.proxy.entity;
			}
		}

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x06000137 RID: 311 RVA: 0x000070A6 File Offset: 0x000052A6
		public World world
		{
			get
			{
				return this.proxy.world;
			}
		}

		// Token: 0x06000138 RID: 312 RVA: 0x000070B4 File Offset: 0x000052B4
		private void OnEnable()
		{
			this.FindComponents();
			Entity entity = FollowerEntity.CreateEntity(this.tr.position, this.tr.rotation, this.tr.localScale.x, ref this.shape, ref this.movement, ref this.autoRepathBacking, this.managedState, this.orientationBacking, this.movementPlaneSourceBacking, this.syncPosition, this.syncRotation, this.enableGravityBacking, this.enableLocalAvoidanceBacking, this.rvoSettingsBacking, this.managedSettings, base.gameObject.scene.GetPhysicsScene());
			this.proxy = new FollowerEntityProxy(World.DefaultGameObjectInjectionWorld, entity);
			BatchedEvents.Add<FollowerEntity>(this, BatchedEvents.Event.None, delegate(FollowerEntity[] components, int ev)
			{
			}, 0);
			IRuntimeBaker[] components2 = base.GetComponents<IRuntimeBaker>();
			for (int i = 0; i < components2.Length; i++)
			{
				if (((MonoBehaviour)components2[i]).enabled)
				{
					components2[i].OnCreatedEntity(World.DefaultGameObjectInjectionWorld, entity);
				}
			}
			if (this.didStart)
			{
				this.Start();
			}
		}

		// Token: 0x06000139 RID: 313 RVA: 0x000071D0 File Offset: 0x000053D0
		public static Entity CreateEntity(float3 position, quaternion rotation, float scale, ref AgentCylinderShape shape, ref MovementSettings movement, ref AutoRepathPolicy autoRepath, ManagedState managedState, OrientationMode orientation, MovementPlaneSource movementPlaneSource, bool updatePosition, bool updateRotation, bool enableGravity, bool enableLocalAvoidance, RVOAgent rvoSettings, ManagedSettings managedSettings, PhysicsScene physicsScene)
		{
			World defaultGameObjectInjectionWorld = World.DefaultGameObjectInjectionWorld;
			if (!FollowerEntity.archetype.Valid || FollowerEntity.archetypeWorld != defaultGameObjectInjectionWorld)
			{
				if (defaultGameObjectInjectionWorld == null)
				{
					throw new Exception("World.DefaultGameObjectInjectionWorld is null. Has the world been destroyed?");
				}
				FollowerEntity.archetypeWorld = defaultGameObjectInjectionWorld;
				FollowerEntity.archetype = defaultGameObjectInjectionWorld.EntityManager.CreateArchetype(new ComponentType[]
				{
					typeof(LocalTransform),
					typeof(MovementState),
					typeof(MovementSettings),
					typeof(AutoRepathPolicy),
					typeof(MovementControl),
					typeof(ManagedState),
					typeof(ManagedSettings),
					typeof(SearchState),
					typeof(MovementStatistics),
					typeof(AgentCylinderShape),
					typeof(ResolvedMovement),
					typeof(DestinationPoint),
					typeof(AgentMovementPlane),
					typeof(GravityState),
					typeof(SimulateMovement),
					typeof(SimulateMovementRepair),
					typeof(SimulateMovementControl),
					typeof(SimulateMovementFinalize),
					typeof(SyncPositionWithTransform),
					typeof(SyncRotationWithTransform),
					typeof(ReadyToTraverseOffMeshLink),
					typeof(AgentMovementPlaneSource),
					typeof(PhysicsSceneRef),
					typeof(AgentShouldRecalculatePath)
				});
			}
			if (orientation == OrientationMode.YAxisForward)
			{
				rotation = math.mul(rotation, SyncTransformsToEntitiesSystem.YAxisForwardToZAxisForward);
			}
			EntityManager entityManager = defaultGameObjectInjectionWorld.EntityManager;
			Entity entity = entityManager.CreateEntity(FollowerEntity.archetype);
			entityManager.SetComponentData<LocalTransform>(entity, LocalTransform.FromPositionRotationScale(position, rotation, scale));
			entityManager.SetComponentData<MovementState>(entity, new MovementState(position));
			entityManager.SetComponentData<DestinationPoint>(entity, new DestinationPoint
			{
				destination = new float3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity)
			});
			autoRepath.Reset();
			entityManager.SetComponentData<AutoRepathPolicy>(entity, autoRepath);
			entityManager.SetComponentData<MovementSettings>(entity, movement);
			if (!managedState.pathTracer.isCreated)
			{
				managedState.pathTracer = new PathTracer(Allocator.Persistent);
			}
			entityManager.SetComponentData(entity, managedState);
			entityManager.SetComponentData(entity, managedSettings);
			entityManager.SetComponentData<MovementStatistics>(entity, new MovementStatistics
			{
				estimatedVelocity = float3.zero,
				lastPosition = position
			});
			entityManager.SetComponentData<AgentCylinderShape>(entity, shape);
			entityManager.SetComponentEnabled<GravityState>(entity, enableGravity);
			if (orientation == OrientationMode.YAxisForward)
			{
				entityManager.AddComponent<OrientationYAxisForward>(entity);
			}
			entityManager.SetComponentEnabled<ReadyToTraverseOffMeshLink>(entity, false);
			entityManager.SetSharedComponent<AgentMovementPlaneSource>(entity, new AgentMovementPlaneSource
			{
				value = movementPlaneSource
			});
			entityManager.SetSharedComponent<PhysicsSceneRef>(entity, new PhysicsSceneRef
			{
				physicsScene = physicsScene
			});
			if (enableLocalAvoidance)
			{
				entityManager.AddComponentData<RVOAgent>(entity, rvoSettings);
			}
			FollowerEntityProxy.ToggleComponent<SyncPositionWithTransform>(defaultGameObjectInjectionWorld, entity, updatePosition, true);
			FollowerEntityProxy.ToggleComponent<SyncRotationWithTransform>(defaultGameObjectInjectionWorld, entity, updateRotation, true);
			ResolvedMovement resolvedMovement = default(ResolvedMovement);
			MovementControl movementControl = default(MovementControl);
			AgentMovementPlane agentMovementPlane = new AgentMovementPlane(rotation);
			FollowerEntityProxy.ResetControl(ref resolvedMovement, ref movementControl, ref agentMovementPlane, position, rotation, position);
			entityManager.SetComponentData<AgentMovementPlane>(entity, agentMovementPlane);
			entityManager.SetComponentData<ResolvedMovement>(entity, resolvedMovement);
			entityManager.SetComponentData<MovementControl>(entity, movementControl);
			entityManager.SetComponentEnabled<AgentShouldRecalculatePath>(entity, false);
			return entity;
		}

		// Token: 0x0600013A RID: 314 RVA: 0x0000760B File Offset: 0x0000580B
		internal void RegisterRuntimeBaker(IRuntimeBaker baker)
		{
			if (this.entityExists)
			{
				baker.OnCreatedEntity(World.DefaultGameObjectInjectionWorld, this.proxy.entity);
			}
		}

		// Token: 0x0600013B RID: 315 RVA: 0x0000762B File Offset: 0x0000582B
		private void Start()
		{
			this.didStart = true;
			this.Teleport(this.position, false);
		}

		// Token: 0x0600013C RID: 316 RVA: 0x00007641 File Offset: 0x00005841
		private void OnDisable()
		{
			BatchedEvents.Remove<FollowerEntity>(this);
			this.CancelCurrentPathRequest();
			this.proxy.Destroy();
			this.managedState.Dispose();
		}

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x0600013D RID: 317 RVA: 0x00007665 File Offset: 0x00005865
		// (set) Token: 0x0600013E RID: 318 RVA: 0x00007672 File Offset: 0x00005872
		public float radius
		{
			get
			{
				return this.shape.radius;
			}
			set
			{
				this.shape.radius = value;
				this.proxy.radius = value;
			}
		}

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x0600013F RID: 319 RVA: 0x0000768C File Offset: 0x0000588C
		// (set) Token: 0x06000140 RID: 320 RVA: 0x00007699 File Offset: 0x00005899
		public float height
		{
			get
			{
				return this.shape.height;
			}
			set
			{
				this.shape.height = value;
				this.proxy.height = value;
			}
		}

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x06000141 RID: 321 RVA: 0x000076B3 File Offset: 0x000058B3
		public ref PathRequestSettings pathfindingSettings
		{
			get
			{
				if (this.proxy.world != null)
				{
					FollowerEntityProxy.movementStateAccessRW.Update(this.proxy.world.EntityManager);
				}
				return ref this.managedSettings.pathfindingSettings;
			}
		}

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x06000142 RID: 322 RVA: 0x000076E7 File Offset: 0x000058E7
		// (set) Token: 0x06000143 RID: 323 RVA: 0x00007703 File Offset: 0x00005903
		public RVOAgent rvoSettings
		{
			get
			{
				if (!this.entityExists)
				{
					return this.rvoSettingsBacking;
				}
				return this.proxy.rvoSettings;
			}
			set
			{
				this.rvoSettingsBacking = value;
				this.proxy.rvoSettings = value;
			}
		}

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x06000144 RID: 324 RVA: 0x00007718 File Offset: 0x00005918
		// (set) Token: 0x06000145 RID: 325 RVA: 0x0000773C File Offset: 0x0000593C
		public Vector3 position
		{
			get
			{
				if (!this.entityExists)
				{
					return base.transform.position;
				}
				return this.proxy.position;
			}
			set
			{
				if (this.proxy.entityExists)
				{
					this.proxy.position = value;
					if (this.proxy.updatePosition)
					{
						base.transform.position = value;
						return;
					}
				}
				else
				{
					base.transform.position = value;
				}
			}
		}

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x06000146 RID: 326 RVA: 0x00007788 File Offset: 0x00005988
		public bool isTraversingOffMeshLink
		{
			get
			{
				return this.proxy.isTraversingOffMeshLink;
			}
		}

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x06000147 RID: 327 RVA: 0x00007795 File Offset: 0x00005995
		public OffMeshLinks.OffMeshLinkTracer offMeshLink
		{
			get
			{
				return this.proxy.offMeshLink;
			}
		}

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x06000148 RID: 328 RVA: 0x000077A2 File Offset: 0x000059A2
		// (set) Token: 0x06000149 RID: 329 RVA: 0x000077AF File Offset: 0x000059AF
		public IOffMeshLinkHandler onTraverseOffMeshLink
		{
			get
			{
				return this.managedSettings.onTraverseOffMeshLink;
			}
			set
			{
				if (this.proxy.world != null)
				{
					FollowerEntityProxy.managedSettingsAccessRW.Update(this.proxy.world.EntityManager);
				}
				this.managedSettings.onTraverseOffMeshLink = value;
			}
		}

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x0600014A RID: 330 RVA: 0x000077E4 File Offset: 0x000059E4
		public GraphNode currentNode
		{
			get
			{
				return this.proxy.currentNode;
			}
		}

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x0600014B RID: 331 RVA: 0x000077F1 File Offset: 0x000059F1
		public GraphHitInfo nearestNavmeshBorder
		{
			get
			{
				return this.proxy.nearestNavmeshBorder;
			}
		}

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x0600014C RID: 332 RVA: 0x00007800 File Offset: 0x00005A00
		// (set) Token: 0x0600014D RID: 333 RVA: 0x00007858 File Offset: 0x00005A58
		public Quaternion rotation
		{
			get
			{
				if (!this.proxy.entityExists)
				{
					return base.transform.rotation;
				}
				Quaternion quaternion = this.proxy.rotation;
				if (this.proxy.orientation == OrientationMode.YAxisForward)
				{
					quaternion = math.mul(quaternion, SyncTransformsToEntitiesSystem.ZAxisForwardToYAxisForward);
				}
				return quaternion;
			}
			set
			{
				if (this.proxy.entityExists)
				{
					if (this.syncRotation)
					{
						base.transform.rotation = value;
					}
					if (this.orientation == OrientationMode.YAxisForward)
					{
						value = math.mul(value, SyncTransformsToEntitiesSystem.YAxisForwardToZAxisForward);
					}
					this.proxy.rotation = value;
					return;
				}
				if (this.syncRotation)
				{
					base.transform.rotation = value;
					return;
				}
				Debug.LogWarning("Cannot set agent rotation because updateRotation is false and the FollowerEntity component has not been enabled yet. Therefore, the internal entity does not exist, and there's no rotation to set.", this);
			}
		}

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x0600014E RID: 334 RVA: 0x000078D3 File Offset: 0x00005AD3
		// (set) Token: 0x0600014F RID: 335 RVA: 0x000078DB File Offset: 0x00005ADB
		public MovementPlaneSource movementPlaneSource
		{
			get
			{
				return this.movementPlaneSourceBacking;
			}
			set
			{
				this.movementPlaneSourceBacking = value;
				this.proxy.movementPlaneSource = value;
			}
		}

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x06000150 RID: 336 RVA: 0x000078F0 File Offset: 0x00005AF0
		// (set) Token: 0x06000151 RID: 337 RVA: 0x000078FD File Offset: 0x00005AFD
		public LayerMask groundMask
		{
			get
			{
				return this.movement.groundMask;
			}
			set
			{
				this.movement.groundMask = value;
				this.proxy.groundMask = value;
			}
		}

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x06000152 RID: 338 RVA: 0x00007917 File Offset: 0x00005B17
		// (set) Token: 0x06000153 RID: 339 RVA: 0x00007924 File Offset: 0x00005B24
		public PIDMovement.DebugFlags debugFlags
		{
			get
			{
				return this.movement.debugFlags;
			}
			set
			{
				this.movement.debugFlags = value;
				this.proxy.debugFlags = value;
			}
		}

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x06000154 RID: 340 RVA: 0x0000793E File Offset: 0x00005B3E
		// (set) Token: 0x06000155 RID: 341 RVA: 0x00007950 File Offset: 0x00005B50
		public float maxSpeed
		{
			get
			{
				return this.movement.follower.speed;
			}
			set
			{
				this.movement.follower.speed = value;
				this.proxy.maxSpeed = value;
			}
		}

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x06000156 RID: 342 RVA: 0x0000796F File Offset: 0x00005B6F
		// (set) Token: 0x06000157 RID: 343 RVA: 0x00007981 File Offset: 0x00005B81
		public float rotationSpeed
		{
			get
			{
				return this.movement.follower.rotationSpeed;
			}
			set
			{
				this.movement.follower.rotationSpeed = value;
				this.proxy.rotationSpeed = value;
			}
		}

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x06000158 RID: 344 RVA: 0x000079A0 File Offset: 0x00005BA0
		// (set) Token: 0x06000159 RID: 345 RVA: 0x000079B2 File Offset: 0x00005BB2
		public float maxRotationSpeed
		{
			get
			{
				return this.movement.follower.maxRotationSpeed;
			}
			set
			{
				this.movement.follower.maxRotationSpeed = value;
				this.proxy.maxRotationSpeed = value;
			}
		}

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x0600015A RID: 346 RVA: 0x000079D1 File Offset: 0x00005BD1
		// (set) Token: 0x0600015B RID: 347 RVA: 0x000079DE File Offset: 0x00005BDE
		public Vector3 velocity
		{
			get
			{
				return this.proxy.velocity;
			}
			set
			{
				this.proxy.velocity = value;
			}
		}

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x0600015C RID: 348 RVA: 0x000079EC File Offset: 0x00005BEC
		public Vector3 desiredVelocity
		{
			get
			{
				return this.proxy.desiredVelocity;
			}
		}

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x0600015D RID: 349 RVA: 0x000079F9 File Offset: 0x00005BF9
		// (set) Token: 0x0600015E RID: 350 RVA: 0x00007A06 File Offset: 0x00005C06
		public Vector3 desiredVelocityWithoutLocalAvoidance
		{
			get
			{
				return this.proxy.desiredVelocityWithoutLocalAvoidance;
			}
			set
			{
				this.proxy.desiredVelocityWithoutLocalAvoidance = value;
			}
		}

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x0600015F RID: 351 RVA: 0x00007A14 File Offset: 0x00005C14
		public float remainingDistance
		{
			get
			{
				return this.proxy.remainingDistance;
			}
		}

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x06000160 RID: 352 RVA: 0x00007A21 File Offset: 0x00005C21
		// (set) Token: 0x06000161 RID: 353 RVA: 0x00007A2E File Offset: 0x00005C2E
		public float stopDistance
		{
			get
			{
				return this.movement.stopDistance;
			}
			set
			{
				if (this.movement.stopDistance != value)
				{
					this.movement.stopDistance = value;
					this.proxy.stopDistance = value;
				}
			}
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x06000162 RID: 354 RVA: 0x00007A56 File Offset: 0x00005C56
		// (set) Token: 0x06000163 RID: 355 RVA: 0x00007A63 File Offset: 0x00005C63
		public float positionSmoothing
		{
			get
			{
				return this.movement.positionSmoothing;
			}
			set
			{
				if (this.movement.positionSmoothing != value)
				{
					this.movement.positionSmoothing = value;
					this.proxy.positionSmoothing = value;
				}
			}
		}

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x06000164 RID: 356 RVA: 0x00007A8B File Offset: 0x00005C8B
		// (set) Token: 0x06000165 RID: 357 RVA: 0x00007A98 File Offset: 0x00005C98
		public float rotationSmoothing
		{
			get
			{
				return this.movement.rotationSmoothing;
			}
			set
			{
				if (this.movement.rotationSmoothing != value)
				{
					this.movement.rotationSmoothing = value;
					this.proxy.rotationSmoothing = value;
				}
			}
		}

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x06000166 RID: 358 RVA: 0x00007AC0 File Offset: 0x00005CC0
		public bool reachedDestination
		{
			get
			{
				return this.proxy.reachedDestination;
			}
		}

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x06000167 RID: 359 RVA: 0x00007ACD File Offset: 0x00005CCD
		public bool reachedEndOfPath
		{
			get
			{
				return this.proxy.reachedEndOfPath;
			}
		}

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x06000168 RID: 360 RVA: 0x00007ADA File Offset: 0x00005CDA
		public bool reachedCrowdedEndOfPath
		{
			get
			{
				return this.proxy.reachedCrowdedEndOfPath;
			}
		}

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x06000169 RID: 361 RVA: 0x00007AE7 File Offset: 0x00005CE7
		public Vector3 endOfPath
		{
			get
			{
				if (!this.entityExists)
				{
					return this.position;
				}
				return this.proxy.endOfPath;
			}
		}

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x0600016A RID: 362 RVA: 0x00007B03 File Offset: 0x00005D03
		// (set) Token: 0x0600016B RID: 363 RVA: 0x00007B10 File Offset: 0x00005D10
		public Vector3 destination
		{
			get
			{
				return this.proxy.destination;
			}
			set
			{
				this.proxy.destination = value;
			}
		}

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x0600016C RID: 364 RVA: 0x00007B1E File Offset: 0x00005D1E
		public Vector3 destinationFacingDirection
		{
			get
			{
				return this.proxy.destinationFacingDirection;
			}
		}

		// Token: 0x0600016D RID: 365 RVA: 0x00007B2B File Offset: 0x00005D2B
		public void SetDestination(float3 destination, float3 facingDirection = default(float3))
		{
			this.proxy.SetDestination(destination, facingDirection);
		}

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x0600016E RID: 366 RVA: 0x00007B3A File Offset: 0x00005D3A
		// (set) Token: 0x0600016F RID: 367 RVA: 0x00007B42 File Offset: 0x00005D42
		public AutoRepathPolicy autoRepath
		{
			get
			{
				return this.autoRepathBacking;
			}
			set
			{
				this.autoRepathBacking = value;
				this.proxy.autoRepath = value;
			}
		}

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x06000170 RID: 368 RVA: 0x00007B57 File Offset: 0x00005D57
		// (set) Token: 0x06000171 RID: 369 RVA: 0x00007B67 File Offset: 0x00005D67
		[Obsolete("This has been superseded by autoRepath.mode")]
		public bool canSearch
		{
			get
			{
				return this.autoRepathBacking.mode > AutoRepathPolicy.Mode.Never;
			}
			set
			{
				if (value)
				{
					if (this.autoRepathBacking.mode == AutoRepathPolicy.Mode.Never)
					{
						this.autoRepathBacking.mode = AutoRepathPolicy.Mode.EveryNSeconds;
					}
				}
				else
				{
					this.autoRepathBacking.mode = AutoRepathPolicy.Mode.Never;
				}
				this.autoRepath = this.autoRepathBacking;
			}
		}

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x06000172 RID: 370 RVA: 0x00007B9F File Offset: 0x00005D9F
		// (set) Token: 0x06000173 RID: 371 RVA: 0x00007BB6 File Offset: 0x00005DB6
		public bool simulateMovement
		{
			get
			{
				return !this.entityExists || this.proxy.simulateMovement;
			}
			set
			{
				this.proxy.simulateMovement = value;
			}
		}

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x06000174 RID: 372 RVA: 0x00007BC4 File Offset: 0x00005DC4
		// (set) Token: 0x06000175 RID: 373 RVA: 0x00007BCC File Offset: 0x00005DCC
		[Obsolete("Renamed to simulateMovement")]
		public bool canMove
		{
			get
			{
				return this.simulateMovement;
			}
			set
			{
				this.simulateMovement = value;
			}
		}

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x06000176 RID: 374 RVA: 0x00007BD5 File Offset: 0x00005DD5
		public NativeMovementPlane movementPlane
		{
			get
			{
				if (!this.entityExists)
				{
					return new NativeMovementPlane(this.rotation);
				}
				return this.proxy.movementPlane;
			}
		}

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x06000177 RID: 375 RVA: 0x00007BFB File Offset: 0x00005DFB
		// (set) Token: 0x06000178 RID: 376 RVA: 0x00007C17 File Offset: 0x00005E17
		public bool enableGravity
		{
			get
			{
				if (!this.entityExists)
				{
					return this.enableGravityBacking;
				}
				return this.proxy.enableGravity;
			}
			set
			{
				if (this.enableGravityBacking != value)
				{
					this.enableGravityBacking = value;
					this.proxy.enableGravity = value;
				}
			}
		}

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x06000179 RID: 377 RVA: 0x00007C35 File Offset: 0x00005E35
		// (set) Token: 0x0600017A RID: 378 RVA: 0x00007C54 File Offset: 0x00005E54
		public bool enableLocalAvoidance
		{
			get
			{
				if (!this.entityExists)
				{
					return this.enableLocalAvoidanceBacking;
				}
				return this.proxy.enableLocalAvoidance;
			}
			set
			{
				this.enableLocalAvoidanceBacking = value;
				if (this.entityExists)
				{
					EntityManager entityManager = this.proxy.world.EntityManager;
					if (entityManager.HasComponent<RVOAgent>(this.proxy.entity) != value)
					{
						if (value)
						{
							entityManager.AddComponentData<RVOAgent>(this.proxy.entity, this.rvoSettingsBacking);
							return;
						}
						entityManager.RemoveComponent<RVOAgent>(this.proxy.entity);
					}
				}
			}
		}

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x0600017B RID: 379 RVA: 0x00007CC6 File Offset: 0x00005EC6
		public bool localAvoidanceTemporarilyDisabled
		{
			get
			{
				return this.proxy.localAvoidanceTemporarilyDisabled;
			}
		}

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x0600017C RID: 380 RVA: 0x00007CD3 File Offset: 0x00005ED3
		// (set) Token: 0x0600017D RID: 381 RVA: 0x00007CDB File Offset: 0x00005EDB
		public bool updatePosition
		{
			get
			{
				return this.syncPosition;
			}
			set
			{
				this.syncPosition = value;
				this.proxy.updatePosition = value;
			}
		}

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x0600017E RID: 382 RVA: 0x00007CF0 File Offset: 0x00005EF0
		// (set) Token: 0x0600017F RID: 383 RVA: 0x00007CF8 File Offset: 0x00005EF8
		public bool updateRotation
		{
			get
			{
				return this.syncRotation;
			}
			set
			{
				this.syncRotation = value;
				this.proxy.updateRotation = value;
			}
		}

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x06000180 RID: 384 RVA: 0x00007D0D File Offset: 0x00005F0D
		// (set) Token: 0x06000181 RID: 385 RVA: 0x00007D15 File Offset: 0x00005F15
		public OrientationMode orientation
		{
			get
			{
				return this.orientationBacking;
			}
			set
			{
				if (this.orientationBacking != value)
				{
					this.orientationBacking = value;
					this.proxy.orientation = value;
				}
			}
		}

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x06000182 RID: 386 RVA: 0x00007D33 File Offset: 0x00005F33
		public bool hasPath
		{
			get
			{
				return this.proxy.hasPath;
			}
		}

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x06000183 RID: 387 RVA: 0x00007D40 File Offset: 0x00005F40
		public bool pathPending
		{
			get
			{
				return this.proxy.pathPending;
			}
		}

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x06000184 RID: 388 RVA: 0x00007D4D File Offset: 0x00005F4D
		// (set) Token: 0x06000185 RID: 389 RVA: 0x00007D6E File Offset: 0x00005F6E
		public bool isStopped
		{
			get
			{
				if (!this.entityExists)
				{
					return this.movement.isStopped;
				}
				return this.proxy.isStopped;
			}
			set
			{
				if (this.movement.isStopped != value)
				{
					this.movement.isStopped = value;
					this.proxy.isStopped = value;
				}
			}
		}

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x06000186 RID: 390 RVA: 0x00007D96 File Offset: 0x00005F96
		// (set) Token: 0x06000187 RID: 391 RVA: 0x00007D9E File Offset: 0x00005F9E
		public MovementSettings movementSettings
		{
			get
			{
				return this.movement;
			}
			set
			{
				this.movement = value;
				this.proxy.movementSettings = value;
			}
		}

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x06000188 RID: 392 RVA: 0x00007DB3 File Offset: 0x00005FB3
		public Vector3 steeringTarget
		{
			get
			{
				if (!this.entityExists)
				{
					return this.position;
				}
				return this.proxy.steeringTarget;
			}
		}

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x06000189 RID: 393 RVA: 0x00003792 File Offset: 0x00001992
		// (set) Token: 0x0600018A RID: 394 RVA: 0x00007DCF File Offset: 0x00005FCF
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

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x0600018B RID: 395 RVA: 0x00007DDB File Offset: 0x00005FDB
		public ManagedMovementOverrides movementOverrides
		{
			get
			{
				return this.proxy.movementOverrides;
			}
		}

		// Token: 0x0600018C RID: 396 RVA: 0x00007DE8 File Offset: 0x00005FE8
		void IAstarAI.FinalizeMovement(Vector3 nextPosition, Quaternion nextRotation)
		{
			throw new InvalidOperationException("The FollowerEntity component does not support FinalizeMovement. Use an ECS system to override movement instead, or use the movementOverrides property. If you just want to move the agent to a position, set ai.position or call ai.Teleport.");
		}

		// Token: 0x0600018D RID: 397 RVA: 0x00007DF4 File Offset: 0x00005FF4
		public void GetRemainingPath(List<Vector3> buffer, out bool stale)
		{
			this.proxy.GetRemainingPath(buffer, out stale);
		}

		// Token: 0x0600018E RID: 398 RVA: 0x00007E03 File Offset: 0x00006003
		public void GetRemainingPath(List<Vector3> buffer, List<PathPartWithLinkInfo> partsBuffer, out bool stale)
		{
			this.proxy.GetRemainingPath(buffer, partsBuffer, out stale);
		}

		// Token: 0x0600018F RID: 399 RVA: 0x00007E13 File Offset: 0x00006013
		public void Move(Vector3 deltaPosition)
		{
			this.position += deltaPosition;
		}

		// Token: 0x06000190 RID: 400 RVA: 0x00007E27 File Offset: 0x00006027
		void IAstarAI.MovementUpdate(float deltaTime, out Vector3 nextPosition, out Quaternion nextRotation)
		{
			throw new InvalidOperationException("The FollowerEntity component does not support MovementUpdate. Use an ECS system to override movement instead, or use the movementOverrides property");
		}

		// Token: 0x06000191 RID: 401 RVA: 0x00007E33 File Offset: 0x00006033
		public void SearchPath()
		{
			this.proxy.SearchPath();
		}

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x06000192 RID: 402 RVA: 0x00007E40 File Offset: 0x00006040
		public bool entityExists
		{
			get
			{
				return this.proxy.entityExists;
			}
		}

		// Token: 0x06000193 RID: 403 RVA: 0x00007E4D File Offset: 0x0000604D
		private void CancelCurrentPathRequest()
		{
			this.proxy.CancelCurrentPathRequest();
		}

		// Token: 0x06000194 RID: 404 RVA: 0x00007E5A File Offset: 0x0000605A
		public void SetPath(Path path, bool updateDestinationFromPath = true)
		{
			this.proxy.SetPath(path, updateDestinationFromPath);
		}

		// Token: 0x06000195 RID: 405 RVA: 0x00007E6C File Offset: 0x0000606C
		public void Teleport(Vector3 newPosition, bool clearPath = true)
		{
			if (this.entityExists)
			{
				this.proxy.Teleport(newPosition, clearPath);
				if (this.proxy.updatePosition)
				{
					base.transform.position = this.proxy.position;
					return;
				}
			}
			else
			{
				this.position = newPosition;
			}
		}

		// Token: 0x06000196 RID: 406 RVA: 0x00007EB9 File Offset: 0x000060B9
		private void FindComponents()
		{
			this.tr = base.transform;
		}

		// Token: 0x06000197 RID: 407 RVA: 0x00007EC8 File Offset: 0x000060C8
		public unsafe override void DrawGizmos()
		{
			if (Application.isPlaying)
			{
				return;
			}
			if (this.tr == null)
			{
				this.FindComponents();
			}
			LocalTransform localTransform = LocalTransform.FromPositionRotationScale(this.tr.position, (this.orientation == OrientationMode.YAxisForward) ? math.mul(this.tr.rotation, SyncTransformsToEntitiesSystem.ZAxisForwardToYAxisForward) : this.tr.rotation, this.tr.localScale.x);
			DestinationPoint destinationPoint = new DestinationPoint
			{
				destination = this.destination,
				facingDirection = this.destinationFacingDirection
			};
			CommandBuilder commandBuilder = *Draw.editor;
			JobDrawFollowerGizmosBase.DrawGizmos(ref commandBuilder, in localTransform, in this.shape, in destinationPoint, this.orientation == OrientationMode.YAxisForward);
		}

		// Token: 0x06000198 RID: 408 RVA: 0x00007FA0 File Offset: 0x000061A0
		protected unsafe override void OnUpgradeSerializedData(ref Migrations migrations, bool unityThread)
		{
			int num;
			if (migrations.TryMigrateFromLegacyFormat(out num) && this.pathfindingSettings.tagEntryCosts.Length != 0)
			{
				migrations.MarkMigrationFinished(1);
			}
			if (migrations.AddAndMaybeRunMigration(1, unityThread))
			{
				Seeker seeker;
				if (base.TryGetComponent<Seeker>(out seeker))
				{
					*this.pathfindingSettings = new PathRequestSettings
					{
						graphMask = seeker.graphMask,
						traversableTags = seeker.traversableTags,
						tagCostMultipliers = seeker.tagCostMultipliers,
						tagEntryCosts = seeker.tagEntryCosts
					};
					Object.DestroyImmediate(seeker);
				}
				else
				{
					*this.pathfindingSettings = PathRequestSettings.Default;
				}
			}
			migrations.AddAndMaybeRunMigration(2, true);
			migrations.AddAndMaybeRunMigration(4, true);
			if (migrations.AddAndMaybeRunMigration(8, true))
			{
				this.managedSettings.onTraverseOffMeshLink = this.managedState.onTraverseOffMeshLink;
				this.managedSettings.pathfindingSettings = this.managedState.pathfindingSettings;
				this.enableGravityBacking = this.managedState.enableGravity;
				this.enableLocalAvoidanceBacking = this.managedState.enableLocalAvoidance;
				this.rvoSettingsBacking = this.managedState.rvoSettings;
			}
		}

		// Token: 0x040000D7 RID: 215
		[SerializeField]
		private AgentCylinderShape shape = new AgentCylinderShape
		{
			height = 2f,
			radius = 0.5f
		};

		// Token: 0x040000D8 RID: 216
		[SerializeField]
		private MovementSettings movement = new MovementSettings
		{
			follower = new PIDMovement
			{
				rotationSpeed = 600f,
				speed = 5f,
				maxRotationSpeed = 720f,
				maxOnSpotRotationSpeed = 720f,
				slowdownTime = 0.5f,
				desiredWallDistance = 0.5f,
				allowRotatingOnSpot = true,
				leadInRadiusWhenApproachingDestination = 1f
			},
			stopDistance = 0.2f,
			rotationSmoothing = 0f,
			groundMask = -1,
			isStopped = false,
			debugFlags = PIDMovement.DebugFlags.Path
		};

		// Token: 0x040000D9 RID: 217
		[SerializeField]
		private ManagedState managedState = new ManagedState
		{
			enableLocalAvoidance = false,
			pathfindingSettings = PathRequestSettings.Default
		};

		// Token: 0x040000DA RID: 218
		[SerializeField]
		private ManagedSettings managedSettings = new ManagedSettings
		{
			pathfindingSettings = PathRequestSettings.Default
		};

		// Token: 0x040000DB RID: 219
		[SerializeField]
		private bool enableLocalAvoidanceBacking;

		// Token: 0x040000DC RID: 220
		[SerializeField]
		private bool enableGravityBacking = true;

		// Token: 0x040000DD RID: 221
		[SerializeField]
		private RVOAgent rvoSettingsBacking = RVOAgent.Default;

		// Token: 0x040000DE RID: 222
		[SerializeField]
		private AutoRepathPolicy autoRepathBacking = AutoRepathPolicy.Default;

		// Token: 0x040000DF RID: 223
		[SerializeField]
		private OrientationMode orientationBacking;

		// Token: 0x040000E0 RID: 224
		[SerializeField]
		private MovementPlaneSource movementPlaneSourceBacking;

		// Token: 0x040000E1 RID: 225
		[SerializeField]
		private bool syncPosition = true;

		// Token: 0x040000E2 RID: 226
		[SerializeField]
		private bool syncRotation = true;

		// Token: 0x040000E3 RID: 227
		private Transform tr;

		// Token: 0x040000E4 RID: 228
		private FollowerEntityProxy proxy;

		// Token: 0x040000E5 RID: 229
		private static EntityArchetype archetype;

		// Token: 0x040000E6 RID: 230
		private static World archetypeWorld;

		// Token: 0x040000E7 RID: 231
		private bool didStart;

		// Token: 0x02000019 RID: 25
		[Flags]
		private enum FollowerEntityMigrations
		{
			// Token: 0x040000E9 RID: 233
			MigratePathfindingSettings = 1,
			// Token: 0x040000EA RID: 234
			MigrateMovementPlaneSource = 2,
			// Token: 0x040000EB RID: 235
			MigrateAutoRepathPolicy = 4,
			// Token: 0x040000EC RID: 236
			MigrateManagedSettings = 8
		}
	}
}
