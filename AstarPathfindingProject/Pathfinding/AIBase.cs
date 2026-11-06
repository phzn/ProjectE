using System;
using Pathfinding.Drawing;
using Pathfinding.Jobs;
using Pathfinding.RVO;
using Pathfinding.Serialization;
using Pathfinding.Sync;
using Pathfinding.Util;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Jobs;
using UnityEngine.Serialization;

namespace Pathfinding
{
	// Token: 0x02000015 RID: 21
	[RequireComponent(typeof(Seeker))]
	public abstract class AIBase : VersionedMonoBehaviour
	{
		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000091 RID: 145 RVA: 0x000049EC File Offset: 0x00002BEC
		// (set) Token: 0x06000092 RID: 146 RVA: 0x000049F9 File Offset: 0x00002BF9
		public float repathRate
		{
			get
			{
				return this.autoRepath.period;
			}
			set
			{
				this.autoRepath.period = value;
			}
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000093 RID: 147 RVA: 0x00004A07 File Offset: 0x00002C07
		// (set) Token: 0x06000094 RID: 148 RVA: 0x00004A17 File Offset: 0x00002C17
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
					if (this.autoRepath.mode == AutoRepathPolicy.Mode.Never)
					{
						this.autoRepath.mode = AutoRepathPolicy.Mode.EveryNSeconds;
						return;
					}
				}
				else
				{
					this.autoRepath.mode = AutoRepathPolicy.Mode.Never;
				}
			}
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000095 RID: 149 RVA: 0x00004A42 File Offset: 0x00002C42
		// (set) Token: 0x06000096 RID: 150 RVA: 0x00004A4A File Offset: 0x00002C4A
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

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000097 RID: 151 RVA: 0x00004A53 File Offset: 0x00002C53
		public Vector3 position
		{
			get
			{
				if (!this.updatePosition)
				{
					return this.simulatedPosition;
				}
				return this.tr.position;
			}
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000098 RID: 152 RVA: 0x00004A6F File Offset: 0x00002C6F
		// (set) Token: 0x06000099 RID: 153 RVA: 0x00004A8B File Offset: 0x00002C8B
		public virtual Quaternion rotation
		{
			get
			{
				if (!this.updateRotation)
				{
					return this.simulatedRotation;
				}
				return this.tr.rotation;
			}
			set
			{
				if (this.updateRotation)
				{
					this.tr.rotation = value;
					return;
				}
				this.simulatedRotation = value;
			}
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x0600009A RID: 154 RVA: 0x00004AA9 File Offset: 0x00002CA9
		// (set) Token: 0x0600009B RID: 155 RVA: 0x00004AB1 File Offset: 0x00002CB1
		public bool updatePosition { get; set; } = true;

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x0600009C RID: 156 RVA: 0x00004ABA File Offset: 0x00002CBA
		// (set) Token: 0x0600009D RID: 157 RVA: 0x00004AC2 File Offset: 0x00002CC2
		public bool updateRotation { get; set; } = true;

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x0600009E RID: 158 RVA: 0x00004ACB File Offset: 0x00002CCB
		// (set) Token: 0x0600009F RID: 159 RVA: 0x00004AD3 File Offset: 0x00002CD3
		protected bool usingGravity { get; set; }

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x060000A0 RID: 160 RVA: 0x00004ADC File Offset: 0x00002CDC
		// (set) Token: 0x060000A1 RID: 161 RVA: 0x00004AE4 File Offset: 0x00002CE4
		public Vector3 destination
		{
			get
			{
				return this.destinationBackingField;
			}
			set
			{
				if (this.rvoDensityBehavior.enabled && !(value == this.destinationBackingField) && (!float.IsPositiveInfinity(value.x) || !float.IsPositiveInfinity(this.destinationBackingField.x)))
				{
					this.destinationBackingField = value;
					this.rvoDensityBehavior.OnDestinationChanged(value, this.reachedDestination);
					return;
				}
				this.destinationBackingField = value;
			}
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x060000A2 RID: 162 RVA: 0x00004B4C File Offset: 0x00002D4C
		public Vector3 velocity
		{
			get
			{
				if (this.lastDeltaTime <= 1E-06f)
				{
					return Vector3.zero;
				}
				return (this.prevPosition1 - this.prevPosition2) / this.lastDeltaTime;
			}
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x060000A3 RID: 163 RVA: 0x00004B7D File Offset: 0x00002D7D
		public Vector3 desiredVelocity
		{
			get
			{
				if (this.lastDeltaTime <= 1E-05f)
				{
					return Vector3.zero;
				}
				return this.movementPlane.ToWorld(this.lastDeltaPosition / this.lastDeltaTime, this.verticalVelocity);
			}
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x060000A4 RID: 164 RVA: 0x00004BB4 File Offset: 0x00002DB4
		// (set) Token: 0x060000A5 RID: 165 RVA: 0x00004BCD File Offset: 0x00002DCD
		public Vector3 desiredVelocityWithoutLocalAvoidance
		{
			get
			{
				return this.movementPlane.ToWorld(this.velocity2D, this.verticalVelocity);
			}
			set
			{
				this.velocity2D = this.movementPlane.ToPlane(value, out this.verticalVelocity);
			}
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x060000A6 RID: 166
		public abstract Vector3 endOfPath { get; }

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x060000A7 RID: 167
		public abstract bool reachedDestination { get; }

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x060000A8 RID: 168 RVA: 0x00004BE7 File Offset: 0x00002DE7
		// (set) Token: 0x060000A9 RID: 169 RVA: 0x00004BEF File Offset: 0x00002DEF
		public bool isStopped { get; set; }

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x060000AA RID: 170 RVA: 0x00004BF8 File Offset: 0x00002DF8
		// (set) Token: 0x060000AB RID: 171 RVA: 0x00004C00 File Offset: 0x00002E00
		public Action onSearchPath { get; set; }

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x060000AC RID: 172 RVA: 0x00004C09 File Offset: 0x00002E09
		protected virtual bool shouldRecalculatePath
		{
			get
			{
				return !this.waitingForPathCalculation && this.autoRepath.ShouldRecalculatePath(this.position, this.radius, this.destination, Time.time);
			}
		}

		// Token: 0x060000AD RID: 173 RVA: 0x00004C38 File Offset: 0x00002E38
		public virtual void FindComponents()
		{
			this.tr = base.transform;
			if (!this.seeker)
			{
				base.TryGetComponent<Seeker>(out this.seeker);
			}
			if (!this.rvoController)
			{
				base.TryGetComponent<RVOController>(out this.rvoController);
			}
			if (!this.controller)
			{
				base.TryGetComponent<CharacterController>(out this.controller);
			}
			if (!this.rigid)
			{
				base.TryGetComponent<Rigidbody>(out this.rigid);
			}
			if (!this.rigid2D)
			{
				base.TryGetComponent<Rigidbody2D>(out this.rigid2D);
			}
		}

		// Token: 0x060000AE RID: 174 RVA: 0x00004CD4 File Offset: 0x00002ED4
		protected virtual void OnEnable()
		{
			this.FindComponents();
			this.onPathComplete = new OnPathDelegate(this.OnPathComplete);
			this.Init();
			BatchedEvents.Add<AIBase>(this, (this.rigid != null || this.rigid2D != null) ? BatchedEvents.Event.FixedUpdate : BatchedEvents.Event.Update, new Action<AIBase[], int, TransformAccessArray, BatchedEvents.Event>(AIBase.OnUpdate), 0);
		}

		// Token: 0x060000AF RID: 175 RVA: 0x00004D38 File Offset: 0x00002F38
		private static void OnUpdate(AIBase[] components, int count, TransformAccessArray transforms, BatchedEvents.Event ev)
		{
			if (AstarPath.active != null && AstarPath.active.IsNavigationSuspended)
			{
				return;
			}
			Physics.SyncTransforms();
			Physics2D.SyncTransforms();
			float num = ((ev == BatchedEvents.Event.FixedUpdate) ? Time.fixedDeltaTime : Time.deltaTime);
			RVOSimulator active = RVOSimulator.active;
			SimulatorBurst simulatorBurst = ((active != null) ? active.GetSimulator() : null);
			if (simulatorBurst != null)
			{
				int num2 = 0;
				for (int i = 0; i < count; i++)
				{
					num2 += ((components[i].rvoController != null && components[i].rvoController.enabled) ? 1 : 0);
				}
				RVODestinationCrowdedBehavior.JobDensityCheck jobDensityCheck = new RVODestinationCrowdedBehavior.JobDensityCheck(num2, num, simulatorBurst);
				int j = 0;
				int num3 = 0;
				while (j < count)
				{
					AIBase aibase = components[j];
					if (aibase.rvoController != null && aibase.rvoController.enabled)
					{
						jobDensityCheck.Set(num3, aibase.rvoController.rvoAgent.AgentIndex, aibase.endOfPath, aibase.rvoDensityBehavior.densityThreshold, aibase.rvoDensityBehavior.progressAverage);
						num3++;
					}
					j++;
				}
				RWLock.ReadLockAsync readLockAsync = simulatorBurst.LockSimulationDataReadOnly();
				JobHandle jobHandle = jobDensityCheck.ScheduleBatch(num2, num2 / 16, readLockAsync.dependency);
				readLockAsync.UnlockAfter(jobHandle);
				jobHandle.Complete();
				int k = 0;
				int num4 = 0;
				while (k < count)
				{
					AIBase aibase2 = components[k];
					if (aibase2.rvoController != null && aibase2.rvoController.enabled)
					{
						aibase2.rvoDensityBehavior.ReadJobResult(ref jobDensityCheck, num4);
						num4++;
					}
					k++;
				}
				jobDensityCheck.Dispose();
			}
			for (int l = 0; l < count; l++)
			{
				components[l].OnUpdate(num);
			}
			if (count > 0 && components[0] is AIPathAlignedToSurface)
			{
				AIPathAlignedToSurface.UpdateMovementPlanes(components as AIPathAlignedToSurface[], count);
			}
			Physics.SyncTransforms();
			Physics2D.SyncTransforms();
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x00004F10 File Offset: 0x00003110
		protected virtual void OnUpdate(float dt)
		{
			this.usingGravity = !(this.gravity == Vector3.zero) && (!this.updatePosition || ((this.rigid == null || this.rigid.isKinematic) && (this.rigid2D == null || this.rigid2D.bodyType == RigidbodyType2D.Kinematic)));
			if (this.shouldRecalculatePath)
			{
				this.SearchPath();
			}
			if (this.simulateMovement)
			{
				Vector3 vector;
				Quaternion quaternion;
				this.MovementUpdate(dt, out vector, out quaternion);
				this.FinalizeMovement(vector, quaternion);
			}
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x00004FA9 File Offset: 0x000031A9
		protected virtual void Start()
		{
			this.startHasRun = true;
			this.Init();
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x00004FB8 File Offset: 0x000031B8
		private void Init()
		{
			if (this.startHasRun)
			{
				if (this.simulateMovement)
				{
					this.Teleport(this.position, false);
				}
				this.autoRepath.Reset();
				if (this.shouldRecalculatePath)
				{
					this.SearchPath();
				}
			}
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x00004FF0 File Offset: 0x000031F0
		public virtual void Teleport(Vector3 newPosition, bool clearPath = true)
		{
			if (clearPath)
			{
				this.ClearPath();
			}
			this.simulatedPosition = newPosition;
			this.prevPosition2 = newPosition;
			this.prevPosition1 = newPosition;
			if (this.updatePosition)
			{
				this.tr.position = newPosition;
			}
			if (this.rvoController != null)
			{
				this.rvoController.Move(Vector3.zero);
			}
			if (clearPath)
			{
				this.SearchPath();
			}
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x0000505A File Offset: 0x0000325A
		protected void CancelCurrentPathRequest()
		{
			this.waitingForPathCalculation = false;
			if (this.seeker != null)
			{
				this.seeker.CancelCurrentPathRequest(true);
			}
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x0000507D File Offset: 0x0000327D
		protected virtual void OnDisable()
		{
			BatchedEvents.Remove<AIBase>(this);
			this.ClearPath();
			this.velocity2D = Vector3.zero;
			this.accumulatedMovementDelta = Vector3.zero;
			this.verticalVelocity = 0f;
			this.lastDeltaTime = 0f;
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x000050BC File Offset: 0x000032BC
		public void MovementUpdate(float deltaTime, out Vector3 nextPosition, out Quaternion nextRotation)
		{
			this.lastDeltaTime = deltaTime;
			this.MovementUpdateInternal(deltaTime, out nextPosition, out nextRotation);
		}

		// Token: 0x060000B7 RID: 183
		protected abstract void MovementUpdateInternal(float deltaTime, out Vector3 nextPosition, out Quaternion nextRotation);

		// Token: 0x060000B8 RID: 184 RVA: 0x000050CE File Offset: 0x000032CE
		protected virtual void CalculatePathRequestEndpoints(out Vector3 start, out Vector3 end)
		{
			start = this.GetFeetPosition();
			end = this.destination;
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x000050E8 File Offset: 0x000032E8
		public virtual void SearchPath()
		{
			if (float.IsPositiveInfinity(this.destination.x))
			{
				return;
			}
			if (this.onSearchPath != null)
			{
				this.onSearchPath();
			}
			Vector3 vector;
			Vector3 vector2;
			this.CalculatePathRequestEndpoints(out vector, out vector2);
			ABPath abpath = ABPath.Construct(vector, vector2, null);
			this.SetPath(abpath, false);
		}

		// Token: 0x060000BA RID: 186 RVA: 0x00005136 File Offset: 0x00003336
		public virtual Vector3 GetFeetPosition()
		{
			return this.position;
		}

		// Token: 0x060000BB RID: 187
		protected abstract void OnPathComplete(Path newPath);

		// Token: 0x060000BC RID: 188
		protected abstract void ClearPath();

		// Token: 0x060000BD RID: 189 RVA: 0x00005140 File Offset: 0x00003340
		public void SetPath(Path path, bool updateDestinationFromPath = true)
		{
			if (updateDestinationFromPath)
			{
				ABPath abpath = path as ABPath;
				if (abpath != null && abpath.endPointKnownBeforeCalculation)
				{
					this.destination = abpath.originalEndPoint;
				}
			}
			if (path == null)
			{
				this.CancelCurrentPathRequest();
				this.ClearPath();
				return;
			}
			if (path.PipelineState == PathState.Created)
			{
				this.waitingForPathCalculation = true;
				this.seeker.CancelCurrentPathRequest(true);
				this.seeker.StartPath(path, this.onPathComplete);
				this.autoRepath.DidRecalculatePath(this.destination, Time.time);
				return;
			}
			if (path.PipelineState >= PathState.Returning)
			{
				if (this.seeker.GetCurrentPath() != path)
				{
					this.seeker.CancelCurrentPathRequest(true);
				}
				this.OnPathComplete(path);
				return;
			}
			throw new ArgumentException("You must call the SetPath method with a path that either has been completely calculated or one whose path calculation has not been started at all. It looks like the path calculation for the path you tried to use has been started, but is not yet finished.");
		}

		// Token: 0x060000BE RID: 190 RVA: 0x000051F8 File Offset: 0x000033F8
		protected virtual void ApplyGravity(float deltaTime)
		{
			if (this.usingGravity)
			{
				float num;
				this.velocity2D += this.movementPlane.ToPlane(deltaTime * (float.IsNaN(this.gravity.x) ? Physics.gravity : this.gravity), out num);
				this.verticalVelocity += num;
				return;
			}
			this.verticalVelocity = 0f;
		}

		// Token: 0x060000BF RID: 191 RVA: 0x0000526C File Offset: 0x0000346C
		protected Vector2 CalculateDeltaToMoveThisFrame(Vector3 position, float distanceToEndOfPath, float deltaTime)
		{
			if (this.rvoController != null && this.rvoController.enabled)
			{
				return this.movementPlane.ToPlane(this.rvoController.CalculateMovementDelta(position, deltaTime));
			}
			return Vector2.ClampMagnitude(this.velocity2D * deltaTime, distanceToEndOfPath);
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x000052BF File Offset: 0x000034BF
		public Quaternion SimulateRotationTowards(Vector3 direction, float maxDegrees)
		{
			return this.SimulateRotationTowards(this.movementPlane.ToPlane(direction), maxDegrees, maxDegrees);
		}

		// Token: 0x060000C1 RID: 193 RVA: 0x000052D8 File Offset: 0x000034D8
		protected Quaternion SimulateRotationTowards(Vector2 direction, float maxDegreesMainAxis, float maxDegreesOffAxis = float.PositiveInfinity)
		{
			Quaternion quaternion;
			if (this.movementPlane.isXY || this.movementPlane.isXZ)
			{
				if (direction == Vector2.zero)
				{
					return this.simulatedRotation;
				}
				quaternion = Quaternion.LookRotation(this.movementPlane.ToWorld(direction, 0f), this.movementPlane.ToWorld(Vector2.zero, 1f));
				maxDegreesOffAxis = maxDegreesMainAxis;
			}
			else
			{
				Vector2 vector = this.movementPlane.ToPlane(this.rotation * ((this.orientation == OrientationMode.YAxisForward) ? Vector3.up : Vector3.forward));
				if (vector == Vector2.zero)
				{
					vector = Vector2.right;
				}
				Vector2 vector2 = VectorMath.ComplexMultiplyConjugate(direction, vector);
				float num = Mathf.Atan2(vector2.y, vector2.x) * 57.29578f;
				Quaternion quaternion2 = Quaternion.AngleAxis(-Mathf.Min(Mathf.Abs(num), maxDegreesMainAxis) * Mathf.Sign(num), Vector3.up);
				quaternion = Quaternion.LookRotation(this.movementPlane.ToWorld(vector, 0f), this.movementPlane.ToWorld(Vector2.zero, 1f));
				quaternion *= quaternion2;
			}
			if (this.orientation == OrientationMode.YAxisForward)
			{
				quaternion *= Quaternion.Euler(90f, 0f, 0f);
			}
			return Quaternion.RotateTowards(this.simulatedRotation, quaternion, maxDegreesOffAxis);
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x0000542C File Offset: 0x0000362C
		public virtual void Move(Vector3 deltaPosition)
		{
			this.accumulatedMovementDelta += deltaPosition;
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x00005440 File Offset: 0x00003640
		public virtual void FinalizeMovement(Vector3 nextPosition, Quaternion nextRotation)
		{
			if (this.enableRotation)
			{
				this.FinalizeRotation(nextRotation);
			}
			this.FinalizePosition(nextPosition);
		}

		// Token: 0x060000C4 RID: 196 RVA: 0x00005458 File Offset: 0x00003658
		private void FinalizeRotation(Quaternion nextRotation)
		{
			this.simulatedRotation = nextRotation;
			if (this.updateRotation)
			{
				if (this.rigid != null)
				{
					this.rigid.MoveRotation(nextRotation);
					return;
				}
				if (this.rigid2D != null)
				{
					this.rigid2D.MoveRotation(nextRotation.eulerAngles.z);
					return;
				}
				this.tr.rotation = nextRotation;
			}
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x000054C4 File Offset: 0x000036C4
		private void FinalizePosition(Vector3 nextPosition)
		{
			Vector3 vector = this.simulatedPosition;
			bool flag = false;
			if (this.controller != null && this.controller.enabled && this.updatePosition)
			{
				this.tr.position = vector;
				this.controller.Move(nextPosition - vector + this.accumulatedMovementDelta);
				vector = this.tr.position;
				if (this.controller.isGrounded)
				{
					this.verticalVelocity = 0f;
				}
			}
			else
			{
				float num;
				this.movementPlane.ToPlane(vector, out num);
				vector = nextPosition + this.accumulatedMovementDelta;
				if (this.usingGravity)
				{
					vector = this.RaycastPosition(vector, num);
				}
				flag = true;
			}
			bool flag2 = false;
			vector = this.ClampToNavmesh(vector, out flag2);
			if ((flag || flag2) && this.updatePosition)
			{
				if (this.rigid != null)
				{
					this.rigid.MovePosition(vector);
				}
				else if (this.rigid2D != null)
				{
					this.rigid2D.MovePosition(vector);
				}
				else
				{
					this.tr.position = vector;
				}
			}
			this.accumulatedMovementDelta = Vector3.zero;
			this.simulatedPosition = vector;
			this.UpdateVelocity();
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x000055F6 File Offset: 0x000037F6
		protected void UpdateVelocity()
		{
			this.prevPosition2 = this.prevPosition1;
			this.prevPosition1 = this.position;
		}

		// Token: 0x060000C7 RID: 199 RVA: 0x00005610 File Offset: 0x00003810
		protected virtual Vector3 ClampToNavmesh(Vector3 position, out bool positionChanged)
		{
			positionChanged = false;
			return position;
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x00005618 File Offset: 0x00003818
		protected Vector3 RaycastPosition(Vector3 position, float lastElevation)
		{
			float num;
			this.movementPlane.ToPlane(position, out num);
			float num2 = this.tr.localScale.y * this.height * 0.5f + Mathf.Max(0f, lastElevation - num);
			Vector3 vector = this.movementPlane.ToWorld(Vector2.zero, num2);
			if (Physics.Raycast(position + vector, -vector, out this.lastRaycastHit, num2, this.groundMask, QueryTriggerInteraction.Ignore))
			{
				this.verticalVelocity *= Math.Max(0f, 1f - 5f * this.lastDeltaTime);
				return this.lastRaycastHit.point;
			}
			return position;
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x000056CF File Offset: 0x000038CF
		protected virtual void OnDrawGizmosSelected()
		{
			if (Application.isPlaying)
			{
				this.FindComponents();
			}
		}

		// Token: 0x060000CA RID: 202 RVA: 0x000056E0 File Offset: 0x000038E0
		public unsafe override void DrawGizmos()
		{
			if (!Application.isPlaying || !base.enabled || this.tr == null)
			{
				this.FindComponents();
			}
			Color color = AIBase.ShapeGizmoColor;
			if (this.rvoController != null && this.rvoController.locked)
			{
				color *= 0.5f;
			}
			if (this.orientation == OrientationMode.YAxisForward)
			{
				Draw.WireCylinder(this.position, Vector3.forward, 0f, this.radius * this.tr.localScale.x, color);
			}
			else
			{
				Draw.WireCylinder(this.position, this.rotation * Vector3.up, this.tr.localScale.y * this.height, this.radius * this.tr.localScale.x, color);
			}
			if (!float.IsPositiveInfinity(this.destination.x) && Application.isPlaying)
			{
				Draw.Circle(this.destination, this.movementPlane.rotation * Vector3.up, 0.2f, Color.blue);
			}
			this.autoRepath.DrawGizmos(*Draw.editor, this.position, this.radius, new NativeMovementPlane(this.movementPlane.rotation));
		}

		// Token: 0x060000CB RID: 203 RVA: 0x00005856 File Offset: 0x00003A56
		protected override void Reset()
		{
			this.ResetShape();
			base.Reset();
		}

		// Token: 0x060000CC RID: 204 RVA: 0x00005864 File Offset: 0x00003A64
		private void ResetShape()
		{
			CharacterController characterController;
			if (base.TryGetComponent<CharacterController>(out characterController))
			{
				this.radius = characterController.radius;
				this.height = Mathf.Max(this.radius * 2f, characterController.height);
			}
		}

		// Token: 0x060000CD RID: 205 RVA: 0x000058A4 File Offset: 0x00003AA4
		protected override void OnUpgradeSerializedData(ref Migrations migrations, bool unityThread)
		{
			int num;
			if (migrations.TryMigrateFromLegacyFormat(out num))
			{
				if (num <= 2 || num == 5)
				{
					this.rvoDensityBehavior.enabled = false;
				}
				if (num <= 3)
				{
					this.repathRate = this.repathRateCompatibility;
					this.canSearch = this.canSearchCompability;
				}
			}
		}

		// Token: 0x04000080 RID: 128
		public float radius = 0.5f;

		// Token: 0x04000081 RID: 129
		public float height = 2f;

		// Token: 0x04000082 RID: 130
		[FormerlySerializedAs("canMove")]
		public bool simulateMovement = true;

		// Token: 0x04000083 RID: 131
		[FormerlySerializedAs("speed")]
		public float maxSpeed = 1f;

		// Token: 0x04000084 RID: 132
		public Vector3 gravity = new Vector3(float.NaN, float.NaN, float.NaN);

		// Token: 0x04000085 RID: 133
		public LayerMask groundMask = -1;

		// Token: 0x04000086 RID: 134
		public float endReachedDistance = 0.2f;

		// Token: 0x04000087 RID: 135
		public CloseToDestinationMode whenCloseToDestination;

		// Token: 0x04000088 RID: 136
		public RVODestinationCrowdedBehavior rvoDensityBehavior = new RVODestinationCrowdedBehavior(true, 0.5f, false);

		// Token: 0x04000089 RID: 137
		[SerializeField]
		[HideInInspector]
		[FormerlySerializedAs("repathRate")]
		private float repathRateCompatibility = float.NaN;

		// Token: 0x0400008A RID: 138
		[SerializeField]
		[HideInInspector]
		[FormerlySerializedAs("canSearch")]
		[FormerlySerializedAs("repeatedlySearchPaths")]
		private bool canSearchCompability;

		// Token: 0x0400008B RID: 139
		[FormerlySerializedAs("rotationIn2D")]
		public OrientationMode orientation;

		// Token: 0x0400008C RID: 140
		public bool enableRotation = true;

		// Token: 0x0400008D RID: 141
		protected Vector3 simulatedPosition;

		// Token: 0x0400008E RID: 142
		protected Quaternion simulatedRotation;

		// Token: 0x0400008F RID: 143
		protected Vector3 accumulatedMovementDelta = Vector3.zero;

		// Token: 0x04000090 RID: 144
		protected Vector2 velocity2D;

		// Token: 0x04000091 RID: 145
		protected float verticalVelocity;

		// Token: 0x04000092 RID: 146
		protected Seeker seeker;

		// Token: 0x04000093 RID: 147
		protected Transform tr;

		// Token: 0x04000094 RID: 148
		protected Rigidbody rigid;

		// Token: 0x04000095 RID: 149
		protected Rigidbody2D rigid2D;

		// Token: 0x04000096 RID: 150
		protected CharacterController controller;

		// Token: 0x04000097 RID: 151
		protected RVOController rvoController;

		// Token: 0x04000098 RID: 152
		public SimpleMovementPlane movementPlane = new SimpleMovementPlane(Quaternion.identity);

		// Token: 0x0400009B RID: 155
		public AutoRepathPolicy autoRepath = new AutoRepathPolicy();

		// Token: 0x0400009D RID: 157
		protected float lastDeltaTime;

		// Token: 0x0400009E RID: 158
		protected Vector3 prevPosition1;

		// Token: 0x0400009F RID: 159
		protected Vector3 prevPosition2;

		// Token: 0x040000A0 RID: 160
		protected Vector2 lastDeltaPosition;

		// Token: 0x040000A1 RID: 161
		protected bool waitingForPathCalculation;

		// Token: 0x040000A2 RID: 162
		protected float lastRepath = float.NegativeInfinity;

		// Token: 0x040000A3 RID: 163
		protected bool startHasRun;

		// Token: 0x040000A4 RID: 164
		private Vector3 destinationBackingField = new Vector3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity);

		// Token: 0x040000A7 RID: 167
		protected OnPathDelegate onPathComplete;

		// Token: 0x040000A8 RID: 168
		protected RaycastHit lastRaycastHit;

		// Token: 0x040000A9 RID: 169
		public static readonly Color ShapeGizmoColor = new Color(0.9411765f, 0.8352941f, 0.11764706f);
	}
}
