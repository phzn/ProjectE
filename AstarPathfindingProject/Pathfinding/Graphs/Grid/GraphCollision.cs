using System;
using Pathfinding.Graphs.Grid.Jobs;
using Pathfinding.Jobs;
using Pathfinding.Util;
using Unity.Collections;
using UnityEngine;

namespace Pathfinding.Graphs.Grid
{
	// Token: 0x02000201 RID: 513
	[Serializable]
	public class GraphCollision
	{
		// Token: 0x06000DB3 RID: 3507 RVA: 0x00053708 File Offset: 0x00051908
		public void Initialize(GraphTransform transform, float scale)
		{
			this.up = (transform.Transform(Vector3.up) - transform.Transform(Vector3.zero)).normalized;
			this.upheight = this.up * this.height;
			this.finalRadius = this.diameter * scale * 0.5f;
			this.finalRaycastRadius = this.thickRaycastDiameter * scale * 0.5f;
			this.contactFilter = new ContactFilter2D
			{
				layerMask = this.mask,
				useDepth = false,
				useLayerMask = true,
				useNormalAngle = false,
				useTriggers = false
			};
		}

		// Token: 0x06000DB4 RID: 3508 RVA: 0x000537BC File Offset: 0x000519BC
		public bool Check(Vector3 position)
		{
			if (!this.collisionCheck)
			{
				return true;
			}
			if (this.use2D)
			{
				ColliderType colliderType = this.type;
				if (colliderType <= ColliderType.Capsule)
				{
					return Physics2D.OverlapCircle(position, this.finalRadius, this.contactFilter, GraphCollision.dummyArray) == 0;
				}
				return Physics2D.OverlapPoint(position, this.contactFilter, GraphCollision.dummyArray) == 0;
			}
			else
			{
				position += this.up * this.collisionOffset;
				ColliderType colliderType = this.type;
				if (colliderType == ColliderType.Sphere)
				{
					return !Physics.CheckSphere(position, this.finalRadius, this.mask, QueryTriggerInteraction.Ignore);
				}
				if (colliderType == ColliderType.Capsule)
				{
					return !Physics.CheckCapsule(position, position + this.upheight, this.finalRadius, this.mask, QueryTriggerInteraction.Ignore);
				}
				return !Physics.Raycast(position, this.up, this.height, this.mask, QueryTriggerInteraction.Ignore) && !Physics.Raycast(position + this.upheight, -this.up, this.height, this.mask, QueryTriggerInteraction.Ignore);
			}
		}

		// Token: 0x06000DB5 RID: 3509 RVA: 0x000538E0 File Offset: 0x00051AE0
		public Vector3 CheckHeight(Vector3 position)
		{
			RaycastHit raycastHit;
			bool flag;
			return this.CheckHeight(position, out raycastHit, out flag);
		}

		// Token: 0x06000DB6 RID: 3510 RVA: 0x000538F8 File Offset: 0x00051AF8
		public Vector3 CheckHeight(Vector3 position, out RaycastHit hit, out bool walkable)
		{
			walkable = true;
			if (!this.heightCheck || this.use2D)
			{
				hit = default(RaycastHit);
				return position;
			}
			if (this.thickRaycast)
			{
				Ray ray = new Ray(position + this.up * this.fromHeight, -this.up);
				if (Physics.SphereCast(ray, this.finalRaycastRadius, out hit, this.fromHeight + 0.005f, this.heightMask, QueryTriggerInteraction.Ignore))
				{
					return VectorMath.ClosestPointOnLine(ray.origin, ray.origin + ray.direction, hit.point);
				}
				walkable &= !this.unwalkableWhenNoGround;
			}
			else
			{
				if (Physics.Raycast(position + this.up * this.fromHeight, -this.up, out hit, this.fromHeight + 0.005f, this.heightMask, QueryTriggerInteraction.Ignore))
				{
					return hit.point;
				}
				walkable &= !this.unwalkableWhenNoGround;
			}
			return position;
		}

		// Token: 0x06000DB7 RID: 3511 RVA: 0x00053A0C File Offset: 0x00051C0C
		public RaycastHit[] CheckHeightAll(Vector3 position, out int numHits)
		{
			if (!this.heightCheck || this.use2D)
			{
				this.hitBuffer[0] = new RaycastHit
				{
					point = position,
					distance = 0f
				};
				numHits = 1;
				return this.hitBuffer;
			}
			numHits = Physics.RaycastNonAlloc(position + this.up * this.fromHeight, -this.up, this.hitBuffer, this.fromHeight + 0.005f, this.heightMask, QueryTriggerInteraction.Ignore);
			if (numHits == this.hitBuffer.Length)
			{
				this.hitBuffer = new RaycastHit[this.hitBuffer.Length * 2];
				return this.CheckHeightAll(position, out numHits);
			}
			return this.hitBuffer;
		}

		// Token: 0x06000DB8 RID: 3512 RVA: 0x00053AD4 File Offset: 0x00051CD4
		public void JobCollisionRay(NativeArray<Vector3> nodePositions, NativeArray<bool> collisionCheckResult, Vector3 up, Allocator allocationMethod, JobDependencyTracker dependencyTracker)
		{
			NativeArray<RaycastCommand> nativeArray = dependencyTracker.NewNativeArray<RaycastCommand>(nodePositions.Length, allocationMethod, NativeArrayOptions.ClearMemory);
			NativeArray<RaycastCommand> nativeArray2 = dependencyTracker.NewNativeArray<RaycastCommand>(nodePositions.Length, allocationMethod, NativeArrayOptions.ClearMemory);
			NativeArray<RaycastHit> nativeArray3 = dependencyTracker.NewNativeArray<RaycastHit>(nodePositions.Length, allocationMethod, NativeArrayOptions.ClearMemory);
			NativeArray<RaycastHit> nativeArray4 = dependencyTracker.NewNativeArray<RaycastHit>(nodePositions.Length, allocationMethod, NativeArrayOptions.ClearMemory);
			new JobPrepareRaycasts
			{
				origins = nodePositions,
				originOffset = up * (this.height + this.collisionOffset),
				direction = -up,
				distance = this.height,
				mask = this.mask,
				physicsScene = Physics.defaultPhysicsScene,
				raycastCommands = nativeArray
			}.Schedule(dependencyTracker);
			new JobPrepareRaycasts
			{
				origins = nodePositions,
				originOffset = up * this.collisionOffset,
				direction = up,
				distance = this.height,
				mask = this.mask,
				physicsScene = Physics.defaultPhysicsScene,
				raycastCommands = nativeArray2
			}.Schedule(dependencyTracker);
			dependencyTracker.ScheduleBatch(nativeArray, nativeArray3, 2048);
			dependencyTracker.ScheduleBatch(nativeArray2, nativeArray4, 2048);
			new JobMergeRaycastCollisionHits
			{
				hit1 = nativeArray3,
				hit2 = nativeArray4,
				result = collisionCheckResult
			}.Schedule(dependencyTracker);
		}

		// Token: 0x06000DB9 RID: 3513 RVA: 0x00053C44 File Offset: 0x00051E44
		public void JobCollisionCapsule(NativeArray<Vector3> nodePositions, NativeArray<bool> collisionCheckResult, Vector3 up, Allocator allocationMethod, JobDependencyTracker dependencyTracker)
		{
			NativeArray<OverlapCapsuleCommand> nativeArray = dependencyTracker.NewNativeArray<OverlapCapsuleCommand>(nodePositions.Length, allocationMethod, NativeArrayOptions.ClearMemory);
			NativeArray<ColliderHit> nativeArray2 = dependencyTracker.NewNativeArray<ColliderHit>(nodePositions.Length, allocationMethod, NativeArrayOptions.ClearMemory);
			new JobPrepareCapsuleCommands
			{
				origins = nodePositions,
				originOffset = up * this.collisionOffset,
				direction = up * this.height,
				radius = this.finalRadius,
				mask = this.mask,
				commands = nativeArray,
				physicsScene = Physics.defaultPhysicsScene
			}.Schedule(dependencyTracker);
			dependencyTracker.ScheduleBatch(nativeArray, nativeArray2, 2048);
			new JobColliderHitsToBooleans
			{
				hits = nativeArray2,
				result = collisionCheckResult
			}.Schedule(dependencyTracker);
		}

		// Token: 0x06000DBA RID: 3514 RVA: 0x00053D14 File Offset: 0x00051F14
		public void JobCollisionSphere(NativeArray<Vector3> nodePositions, NativeArray<bool> collisionCheckResult, Vector3 up, Allocator allocationMethod, JobDependencyTracker dependencyTracker)
		{
			NativeArray<OverlapSphereCommand> nativeArray = dependencyTracker.NewNativeArray<OverlapSphereCommand>(nodePositions.Length, allocationMethod, NativeArrayOptions.ClearMemory);
			NativeArray<ColliderHit> nativeArray2 = dependencyTracker.NewNativeArray<ColliderHit>(nodePositions.Length, allocationMethod, NativeArrayOptions.ClearMemory);
			new JobPrepareSphereCommands
			{
				origins = nodePositions,
				originOffset = up * this.collisionOffset,
				radius = this.finalRadius,
				mask = this.mask,
				commands = nativeArray,
				physicsScene = Physics.defaultPhysicsScene
			}.Schedule(dependencyTracker);
			dependencyTracker.ScheduleBatch(nativeArray, nativeArray2, 2048);
			new JobColliderHitsToBooleans
			{
				hits = nativeArray2,
				result = collisionCheckResult
			}.Schedule(dependencyTracker);
		}

		// Token: 0x040009BF RID: 2495
		public ColliderType type = ColliderType.Capsule;

		// Token: 0x040009C0 RID: 2496
		public float diameter = 1f;

		// Token: 0x040009C1 RID: 2497
		public float height = 2f;

		// Token: 0x040009C2 RID: 2498
		public float collisionOffset;

		// Token: 0x040009C3 RID: 2499
		[Obsolete("Only the Both mode is supported now")]
		public RayDirection rayDirection = RayDirection.Both;

		// Token: 0x040009C4 RID: 2500
		public LayerMask mask;

		// Token: 0x040009C5 RID: 2501
		public LayerMask heightMask = -1;

		// Token: 0x040009C6 RID: 2502
		public float fromHeight = 100f;

		// Token: 0x040009C7 RID: 2503
		public bool thickRaycast;

		// Token: 0x040009C8 RID: 2504
		public float thickRaycastDiameter = 1f;

		// Token: 0x040009C9 RID: 2505
		public bool unwalkableWhenNoGround = true;

		// Token: 0x040009CA RID: 2506
		public bool use2D;

		// Token: 0x040009CB RID: 2507
		public bool collisionCheck = true;

		// Token: 0x040009CC RID: 2508
		public bool heightCheck = true;

		// Token: 0x040009CD RID: 2509
		public Vector3 up;

		// Token: 0x040009CE RID: 2510
		private Vector3 upheight;

		// Token: 0x040009CF RID: 2511
		private ContactFilter2D contactFilter;

		// Token: 0x040009D0 RID: 2512
		private static Collider2D[] dummyArray = new Collider2D[1];

		// Token: 0x040009D1 RID: 2513
		private float finalRadius;

		// Token: 0x040009D2 RID: 2514
		private float finalRaycastRadius;

		// Token: 0x040009D3 RID: 2515
		public const float RaycastErrorMargin = 0.005f;

		// Token: 0x040009D4 RID: 2516
		private RaycastHit[] hitBuffer = new RaycastHit[8];
	}
}
