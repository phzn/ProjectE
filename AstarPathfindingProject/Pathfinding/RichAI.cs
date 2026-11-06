using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding.Drawing;
using Pathfinding.Util;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x0200001C RID: 28
	[AddComponentMenu("Pathfinding/AI/RichAI (3D, for navmesh)")]
	[UniqueComponent(tag = "ai")]
	[DisallowMultipleComponent]
	public class RichAI : AIBase, IAstarAI
	{
		// Token: 0x17000097 RID: 151
		// (get) Token: 0x060001CA RID: 458 RVA: 0x00008222 File Offset: 0x00006422
		// (set) Token: 0x060001CB RID: 459 RVA: 0x0000822A File Offset: 0x0000642A
		public bool traversingOffMeshLink { get; protected set; }

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x060001CC RID: 460 RVA: 0x00008233 File Offset: 0x00006433
		public float remainingDistance
		{
			get
			{
				return this.distanceToSteeringTarget + Vector3.Distance(this.steeringTarget, this.richPath.Endpoint);
			}
		}

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x060001CD RID: 461 RVA: 0x00008252 File Offset: 0x00006452
		public bool reachedEndOfPath
		{
			get
			{
				return this.approachingPathEndpoint && this.distanceToSteeringTarget < this.endReachedDistance;
			}
		}

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x060001CE RID: 462 RVA: 0x0000826C File Offset: 0x0000646C
		public override bool reachedDestination
		{
			get
			{
				if (!this.reachedEndOfPath)
				{
					return false;
				}
				if (this.distanceToSteeringTarget + this.movementPlane.ToPlane(this.steeringTarget - this.richPath.Endpoint).magnitude + this.movementPlane.ToPlane(base.destination - this.richPath.Endpoint).magnitude > this.endReachedDistance)
				{
					return false;
				}
				if (this.orientation != OrientationMode.YAxisForward)
				{
					float num;
					this.movementPlane.ToPlane(base.destination - base.position, out num);
					float num2 = this.tr.localScale.y * this.height;
					if (num > num2 || (double)num < (double)(-(double)num2) * 0.5)
					{
						return false;
					}
				}
				return true;
			}
		}

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x060001CF RID: 463 RVA: 0x0000833F File Offset: 0x0000653F
		public bool hasPath
		{
			get
			{
				return this.richPath.GetCurrentPart() != null;
			}
		}

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x060001D0 RID: 464 RVA: 0x0000834F File Offset: 0x0000654F
		public bool pathPending
		{
			get
			{
				return this.waitingForPathCalculation || this.delayUpdatePath;
			}
		}

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x060001D1 RID: 465 RVA: 0x00008361 File Offset: 0x00006561
		// (set) Token: 0x060001D2 RID: 466 RVA: 0x00008369 File Offset: 0x00006569
		public Vector3 steeringTarget { get; protected set; }

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x060001D3 RID: 467 RVA: 0x00006843 File Offset: 0x00004A43
		// (set) Token: 0x060001D4 RID: 468 RVA: 0x0000684B File Offset: 0x00004A4B
		float IAstarAI.radius
		{
			get
			{
				return this.radius;
			}
			set
			{
				this.radius = value;
			}
		}

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x060001D5 RID: 469 RVA: 0x00006854 File Offset: 0x00004A54
		// (set) Token: 0x060001D6 RID: 470 RVA: 0x0000685C File Offset: 0x00004A5C
		float IAstarAI.height
		{
			get
			{
				return this.height;
			}
			set
			{
				this.height = value;
			}
		}

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x060001D7 RID: 471 RVA: 0x00006865 File Offset: 0x00004A65
		// (set) Token: 0x060001D8 RID: 472 RVA: 0x0000686D File Offset: 0x00004A6D
		float IAstarAI.maxSpeed
		{
			get
			{
				return this.maxSpeed;
			}
			set
			{
				this.maxSpeed = value;
			}
		}

		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x060001D9 RID: 473 RVA: 0x00006876 File Offset: 0x00004A76
		// (set) Token: 0x060001DA RID: 474 RVA: 0x0000687E File Offset: 0x00004A7E
		bool IAstarAI.canSearch
		{
			get
			{
				return base.canSearch;
			}
			set
			{
				base.canSearch = value;
			}
		}

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x060001DB RID: 475 RVA: 0x00004A42 File Offset: 0x00002C42
		// (set) Token: 0x060001DC RID: 476 RVA: 0x00004A4A File Offset: 0x00002C4A
		bool IAstarAI.simulateMovement
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

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x060001DD RID: 477 RVA: 0x00006887 File Offset: 0x00004A87
		NativeMovementPlane IAstarAI.movementPlane
		{
			get
			{
				return new NativeMovementPlane(this.movementPlane);
			}
		}

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x060001DE RID: 478 RVA: 0x00008372 File Offset: 0x00006572
		public bool approachingPartEndpoint
		{
			get
			{
				return this.lastCorner && this.nextCorners.Count == 1;
			}
		}

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x060001DF RID: 479 RVA: 0x0000838C File Offset: 0x0000658C
		public bool approachingPathEndpoint
		{
			get
			{
				return this.approachingPartEndpoint && this.richPath.IsLastPart;
			}
		}

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x060001E0 RID: 480 RVA: 0x000083A3 File Offset: 0x000065A3
		public override Vector3 endOfPath
		{
			get
			{
				if (this.hasPath)
				{
					return this.richPath.Endpoint;
				}
				if (float.IsFinite(base.destination.x))
				{
					return base.destination;
				}
				return base.position;
			}
		}

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x060001E1 RID: 481 RVA: 0x000083D8 File Offset: 0x000065D8
		// (set) Token: 0x060001E2 RID: 482 RVA: 0x000083E0 File Offset: 0x000065E0
		public override Quaternion rotation
		{
			get
			{
				return base.rotation;
			}
			set
			{
				base.rotation = value;
				this.rotationFilterState = Vector2.zero;
				this.rotationFilterState2 = Vector2.zero;
			}
		}

		// Token: 0x060001E3 RID: 483 RVA: 0x000083FF File Offset: 0x000065FF
		public override void Teleport(Vector3 newPosition, bool clearPath = true)
		{
			base.Teleport(this.ClampPositionToGraph(newPosition), clearPath);
		}

		// Token: 0x060001E4 RID: 484 RVA: 0x00008410 File Offset: 0x00006610
		protected virtual Vector3 ClampPositionToGraph(Vector3 newPosition)
		{
			NNInfo nninfo = ((AstarPath.active != null) ? AstarPath.active.GetNearest(newPosition) : default(NNInfo));
			float num;
			this.movementPlane.ToPlane(newPosition, out num);
			return this.movementPlane.ToWorld(this.movementPlane.ToPlane((nninfo.node != null) ? nninfo.position : newPosition), num);
		}

		// Token: 0x060001E5 RID: 485 RVA: 0x00008478 File Offset: 0x00006678
		protected override void OnDisable()
		{
			base.OnDisable();
			this.traversingOffMeshLink = false;
			base.StopAllCoroutines();
			this.rotationFilterState = Vector2.zero;
			this.rotationFilterState2 = Vector2.zero;
		}

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x060001E6 RID: 486 RVA: 0x000084A3 File Offset: 0x000066A3
		protected override bool shouldRecalculatePath
		{
			get
			{
				return base.shouldRecalculatePath && !this.traversingOffMeshLink;
			}
		}

		// Token: 0x060001E7 RID: 487 RVA: 0x000084B8 File Offset: 0x000066B8
		public override void SearchPath()
		{
			if (this.traversingOffMeshLink)
			{
				this.delayUpdatePath = true;
				return;
			}
			base.SearchPath();
		}

		// Token: 0x060001E8 RID: 488 RVA: 0x000084D0 File Offset: 0x000066D0
		protected override void OnPathComplete(Path p)
		{
			this.waitingForPathCalculation = false;
			p.Claim(this);
			if (p.error)
			{
				p.Release(this, false);
				return;
			}
			if (this.traversingOffMeshLink)
			{
				this.delayUpdatePath = true;
			}
			else
			{
				ABPath abpath = p as ABPath;
				if (abpath != null && !abpath.endPointKnownBeforeCalculation)
				{
					base.destination = abpath.originalEndPoint;
				}
				this.richPath.Initialize(this.seeker, p, true, this.funnelSimplification);
				RichFunnel richFunnel = this.richPath.GetCurrentPart() as RichFunnel;
				if (richFunnel != null)
				{
					if (base.updatePosition)
					{
						this.simulatedPosition = this.tr.position;
					}
					Vector2 vector = this.movementPlane.ToPlane(this.UpdateTarget(richFunnel));
					this.steeringTarget = this.nextCorners[0];
					Vector2 vector2 = this.movementPlane.ToPlane(this.steeringTarget);
					this.distanceToSteeringTarget = (vector2 - vector).magnitude;
					if (this.lastCorner && this.nextCorners.Count == 1 && this.distanceToSteeringTarget <= this.endReachedDistance)
					{
						this.NextPart();
					}
				}
			}
			p.Release(this, false);
		}

		// Token: 0x060001E9 RID: 489 RVA: 0x000085F6 File Offset: 0x000067F6
		protected override void ClearPath()
		{
			base.CancelCurrentPathRequest();
			this.richPath.Clear();
			this.lastCorner = false;
			this.delayUpdatePath = false;
			this.distanceToSteeringTarget = float.PositiveInfinity;
		}

		// Token: 0x060001EA RID: 490 RVA: 0x00008624 File Offset: 0x00006824
		protected void NextPart()
		{
			if (!this.richPath.CompletedAllParts)
			{
				if (!this.richPath.IsLastPart)
				{
					this.lastCorner = false;
				}
				this.richPath.NextPart();
				if (this.richPath.CompletedAllParts)
				{
					this.OnTargetReached();
				}
			}
		}

		// Token: 0x060001EB RID: 491 RVA: 0x00008670 File Offset: 0x00006870
		public void GetRemainingPath(List<Vector3> buffer, out bool stale)
		{
			this.richPath.GetRemainingPath(buffer, null, this.simulatedPosition, out stale);
		}

		// Token: 0x060001EC RID: 492 RVA: 0x00008686 File Offset: 0x00006886
		public void GetRemainingPath(List<Vector3> buffer, List<PathPartWithLinkInfo> partsBuffer, out bool stale)
		{
			this.richPath.GetRemainingPath(buffer, partsBuffer, this.simulatedPosition, out stale);
		}

		// Token: 0x060001ED RID: 493 RVA: 0x00003788 File Offset: 0x00001988
		protected virtual void OnTargetReached()
		{
		}

		// Token: 0x060001EE RID: 494 RVA: 0x0000869C File Offset: 0x0000689C
		protected virtual Vector3 UpdateTarget(RichFunnel fn)
		{
			this.nextCorners.Clear();
			bool flag;
			Vector3 vector = fn.Update(this.simulatedPosition, this.nextCorners, 2, out this.lastCorner, out flag);
			if (flag && !this.waitingForPathCalculation && base.canSearch)
			{
				this.SearchPath();
			}
			return vector;
		}

		// Token: 0x060001EF RID: 495 RVA: 0x000086E8 File Offset: 0x000068E8
		protected override void MovementUpdateInternal(float deltaTime, out Vector3 nextPosition, out Quaternion nextRotation)
		{
			if (base.updatePosition)
			{
				this.simulatedPosition = this.tr.position;
			}
			if (base.updateRotation)
			{
				this.simulatedRotation = this.tr.rotation;
			}
			RichPathPart currentPart = this.richPath.GetCurrentPart();
			if (currentPart is RichSpecial)
			{
				if (!this.traversingOffMeshLink && !this.richPath.CompletedAllParts)
				{
					base.StartCoroutine(this.TraverseSpecial(currentPart as RichSpecial));
				}
				nextPosition = (this.steeringTarget = this.simulatedPosition);
				nextRotation = this.rotation;
				return;
			}
			RichFunnel richFunnel = currentPart as RichFunnel;
			bool flag = base.isStopped || (this.reachedDestination && this.whenCloseToDestination == CloseToDestinationMode.Stop);
			if (this.rvoController != null)
			{
				this.rvoDensityBehavior.Update(this.rvoController.enabled, this.reachedDestination, ref flag, ref this.rvoController.priorityMultiplier, ref this.rvoController.flowFollowingStrength, this.simulatedPosition);
			}
			if (richFunnel != null && !flag)
			{
				this.TraverseFunnel(richFunnel, deltaTime, out nextPosition, out nextRotation);
				return;
			}
			this.velocity2D -= Vector2.ClampMagnitude(this.velocity2D, this.acceleration * deltaTime);
			this.FinalMovement(this.simulatedPosition, deltaTime, float.PositiveInfinity, 1f, out nextPosition, out nextRotation);
			if (richFunnel == null || base.isStopped)
			{
				this.steeringTarget = this.simulatedPosition;
			}
		}

		// Token: 0x060001F0 RID: 496 RVA: 0x0000885C File Offset: 0x00006A5C
		private void TraverseFunnel(RichFunnel fn, float deltaTime, out Vector3 nextPosition, out Quaternion nextRotation)
		{
			Vector3 vector = this.UpdateTarget(fn);
			float num;
			Vector2 vector2 = this.movementPlane.ToPlane(vector, out num);
			if (Time.frameCount % 5 == 0 && this.wallForce > 0f && this.wallDist > 0f)
			{
				this.wallBuffer.Clear();
				fn.FindWalls(this.wallBuffer, this.wallDist);
			}
			this.steeringTarget = this.nextCorners[0];
			Vector2 vector3 = this.movementPlane.ToPlane(this.steeringTarget);
			Vector2 vector4 = vector3 - vector2;
			Vector2 vector5 = VectorMath.Normalize(vector4, out this.distanceToSteeringTarget);
			Vector2 vector6 = this.CalculateWallForce(vector2, num, vector5);
			Vector2 vector7;
			if (this.approachingPartEndpoint)
			{
				vector7 = ((this.slowdownTime > 0f) ? Vector2.zero : (vector5 * this.maxSpeed));
				vector6 *= Math.Min(this.distanceToSteeringTarget / 0.5f, 1f);
				if (this.distanceToSteeringTarget <= this.endReachedDistance)
				{
					this.NextPart();
				}
			}
			else
			{
				vector7 = (((this.nextCorners.Count > 1) ? this.movementPlane.ToPlane(this.nextCorners[1]) : (vector2 + 2f * vector4)) - vector3).normalized * this.maxSpeed;
			}
			Vector2 vector8 = this.movementPlane.ToPlane(this.simulatedRotation * ((this.orientation == OrientationMode.YAxisForward) ? Vector3.up : Vector3.forward));
			Vector2 vector9 = MovementUtilities.CalculateAccelerationToReachPoint(vector3 - vector2, vector7, this.velocity2D, this.acceleration, this.rotationSpeed, this.maxSpeed, vector8);
			this.velocity2D += (vector9 + vector6 * this.wallForce) * deltaTime;
			float num2 = this.distanceToSteeringTarget + Vector3.Distance(this.steeringTarget, fn.exactEnd);
			float num3 = ((num2 < this.maxSpeed * this.slowdownTime) ? Mathf.Sqrt(num2 / (this.maxSpeed * this.slowdownTime)) : 1f);
			this.FinalMovement(vector, deltaTime, num2, num3, out nextPosition, out nextRotation);
		}

		// Token: 0x060001F1 RID: 497 RVA: 0x00008A98 File Offset: 0x00006C98
		private void FinalMovement(Vector3 position3D, float deltaTime, float distanceToEndOfPath, float speedLimitFactor, out Vector3 nextPosition, out Quaternion nextRotation)
		{
			Vector2 vector = this.movementPlane.ToPlane(this.simulatedRotation * ((this.orientation == OrientationMode.YAxisForward) ? Vector3.up : Vector3.forward));
			this.ApplyGravity(deltaTime);
			this.velocity2D = MovementUtilities.ClampVelocity(this.velocity2D, this.maxSpeed, speedLimitFactor, this.slowWhenNotFacingTarget && this.enableRotation, this.preventMovingBackwards, vector);
			bool flag = false;
			if (this.rvoController != null && this.rvoController.enabled)
			{
				Vector3 vector2 = position3D + this.movementPlane.ToWorld(Vector2.ClampMagnitude(this.velocity2D, distanceToEndOfPath), 0f);
				this.rvoController.SetTarget(vector2, this.velocity2D.magnitude, this.maxSpeed, this.endOfPath);
				flag = this.rvoController.AvoidingAnyAgents;
			}
			Vector2 vector3 = (this.lastDeltaPosition = base.CalculateDeltaToMoveThisFrame(position3D, distanceToEndOfPath, deltaTime));
			if (this.enableRotation)
			{
				float num = this.radius * this.tr.localScale.x * 0.2f;
				float num2 = MovementUtilities.FilterRotationDirection(ref this.rotationFilterState, ref this.rotationFilterState2, vector3, num, deltaTime, flag);
				nextRotation = base.SimulateRotationTowards(this.rotationFilterState, this.rotationSpeed * deltaTime * num2, this.rotationSpeed * deltaTime);
			}
			else
			{
				nextRotation = this.simulatedRotation;
			}
			nextPosition = position3D + this.movementPlane.ToWorld(vector3, this.verticalVelocity * deltaTime);
		}

		// Token: 0x060001F2 RID: 498 RVA: 0x00008C24 File Offset: 0x00006E24
		protected override Vector3 ClampToNavmesh(Vector3 position, out bool positionChanged)
		{
			if (this.richPath != null)
			{
				RichFunnel richFunnel = this.richPath.GetCurrentPart() as RichFunnel;
				if (richFunnel != null)
				{
					Vector3 vector = richFunnel.ClampToNavmesh(position);
					if (this.rvoController != null && this.rvoController.enabled)
					{
						this.rvoController.SetObstacleQuery(richFunnel.CurrentNode);
					}
					Vector2 vector2 = this.movementPlane.ToPlane(vector - position);
					float sqrMagnitude = vector2.sqrMagnitude;
					if (sqrMagnitude > 1.0000001E-06f)
					{
						this.velocity2D -= vector2 * Vector2.Dot(vector2, this.velocity2D) / sqrMagnitude;
						positionChanged = true;
						return position + this.movementPlane.ToWorld(vector2, 0f);
					}
				}
			}
			positionChanged = false;
			return position;
		}

		// Token: 0x060001F3 RID: 499 RVA: 0x00008CF4 File Offset: 0x00006EF4
		private Vector2 CalculateWallForce(Vector2 position, float elevation, Vector2 directionToTarget)
		{
			if (this.wallForce <= 0f || this.wallDist <= 0f)
			{
				return Vector2.zero;
			}
			float num = 0f;
			float num2 = 0f;
			Vector3 vector = this.movementPlane.ToWorld(position, elevation);
			for (int i = 0; i < this.wallBuffer.Count; i += 2)
			{
				float sqrMagnitude = (VectorMath.ClosestPointOnSegment(this.wallBuffer[i], this.wallBuffer[i + 1], vector) - vector).sqrMagnitude;
				if (sqrMagnitude <= this.wallDist * this.wallDist)
				{
					Vector2 normalized = this.movementPlane.ToPlane(this.wallBuffer[i + 1] - this.wallBuffer[i]).normalized;
					float num3 = Vector2.Dot(directionToTarget, normalized);
					float num4 = 1f - Math.Max(0f, 2f * (sqrMagnitude / (this.wallDist * this.wallDist)) - 1f);
					if (num3 > 0f)
					{
						num2 = Math.Max(num2, num3 * num4);
					}
					else
					{
						num = Math.Max(num, -num3 * num4);
					}
				}
			}
			return new Vector2(directionToTarget.y, -directionToTarget.x) * (num2 - num);
		}

		// Token: 0x060001F4 RID: 500 RVA: 0x00008E47 File Offset: 0x00007047
		protected virtual IEnumerator TraverseSpecial(RichSpecial link)
		{
			this.traversingOffMeshLink = true;
			this.velocity2D = Vector3.zero;
			IEnumerator enumerator = ((this.onTraverseOffMeshLink != null) ? this.onTraverseOffMeshLink(link) : this.TraverseOffMeshLinkFallback(link));
			yield return base.StartCoroutine(enumerator);
			this.traversingOffMeshLink = false;
			this.NextPart();
			if (this.delayUpdatePath)
			{
				this.delayUpdatePath = false;
				if (base.canSearch)
				{
					this.SearchPath();
				}
			}
			yield break;
		}

		// Token: 0x060001F5 RID: 501 RVA: 0x00008E5D File Offset: 0x0000705D
		protected IEnumerator TraverseOffMeshLinkFallback(RichSpecial link)
		{
			float duration = ((this.maxSpeed > 0f) ? (Vector3.Distance(link.second.position, link.first.position) / this.maxSpeed) : 1f);
			float startTime = Time.time;
			for (;;)
			{
				Vector3 vector = Vector3.Lerp(link.first.position, link.second.position, Mathf.InverseLerp(startTime, startTime + duration, Time.time));
				if (base.updatePosition)
				{
					this.tr.position = vector;
				}
				else
				{
					this.simulatedPosition = vector;
				}
				if (Time.time >= startTime + duration)
				{
					break;
				}
				yield return null;
			}
			yield break;
		}

		// Token: 0x060001F6 RID: 502 RVA: 0x00008E74 File Offset: 0x00007074
		public override void DrawGizmos()
		{
			base.DrawGizmos();
			if (this.tr != null)
			{
				Vector3 vector = base.position;
				for (int i = 0; i < this.nextCorners.Count; i++)
				{
					Draw.Line(vector, this.nextCorners[i], RichAI.GizmoColorPath);
					vector = this.nextCorners[i];
				}
			}
		}

		// Token: 0x040000EF RID: 239
		public float acceleration = 5f;

		// Token: 0x040000F0 RID: 240
		public float rotationSpeed = 360f;

		// Token: 0x040000F1 RID: 241
		public float slowdownTime = 0.5f;

		// Token: 0x040000F2 RID: 242
		public float wallForce = 3f;

		// Token: 0x040000F3 RID: 243
		public float wallDist = 1f;

		// Token: 0x040000F4 RID: 244
		public bool funnelSimplification;

		// Token: 0x040000F5 RID: 245
		public bool slowWhenNotFacingTarget = true;

		// Token: 0x040000F6 RID: 246
		public bool preventMovingBackwards;

		// Token: 0x040000F7 RID: 247
		public Func<RichSpecial, IEnumerator> onTraverseOffMeshLink;

		// Token: 0x040000F8 RID: 248
		protected readonly RichPath richPath = new RichPath();

		// Token: 0x040000F9 RID: 249
		protected bool delayUpdatePath;

		// Token: 0x040000FA RID: 250
		protected bool lastCorner;

		// Token: 0x040000FB RID: 251
		private Vector2 rotationFilterState;

		// Token: 0x040000FC RID: 252
		private Vector2 rotationFilterState2;

		// Token: 0x040000FD RID: 253
		protected float distanceToSteeringTarget = float.PositiveInfinity;

		// Token: 0x040000FE RID: 254
		protected readonly List<Vector3> nextCorners = new List<Vector3>();

		// Token: 0x040000FF RID: 255
		protected readonly List<Vector3> wallBuffer = new List<Vector3>();

		// Token: 0x04000102 RID: 258
		protected static readonly Color GizmoColorPath = new Color(0.03137255f, 0.30588236f, 0.7607843f);
	}
}
