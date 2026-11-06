using System;
using System.Collections.Generic;
using Pathfinding.Drawing;
using Pathfinding.Serialization;
using Pathfinding.Util;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;

namespace Pathfinding
{
	// Token: 0x02000016 RID: 22
	[RequireComponent(typeof(Seeker))]
	[AddComponentMenu("Pathfinding/AI/AILerp (2D,3D)")]
	[UniqueComponent(tag = "ai")]
	[DisallowMultipleComponent]
	[HelpURL("https://arongranberg.com/astar/documentation/stable/ailerp.html")]
	public class AILerp : VersionedMonoBehaviour, IAstarAI
	{
		// Token: 0x17000024 RID: 36
		// (get) Token: 0x060000D0 RID: 208 RVA: 0x000059F0 File Offset: 0x00003BF0
		// (set) Token: 0x060000D1 RID: 209 RVA: 0x000059FD File Offset: 0x00003BFD
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

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x060000D2 RID: 210 RVA: 0x00005A0B File Offset: 0x00003C0B
		// (set) Token: 0x060000D3 RID: 211 RVA: 0x00005A1B File Offset: 0x00003C1B
		public bool canSearch
		{
			get
			{
				return this.autoRepath.mode > AutoRepathPolicy.Mode.Never;
			}
			set
			{
				this.autoRepath.mode = (value ? AutoRepathPolicy.Mode.EveryNSeconds : AutoRepathPolicy.Mode.Never);
			}
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x060000D4 RID: 212 RVA: 0x00005A2F File Offset: 0x00003C2F
		// (set) Token: 0x060000D5 RID: 213 RVA: 0x00005A37 File Offset: 0x00003C37
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

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x060000D6 RID: 214 RVA: 0x00005A40 File Offset: 0x00003C40
		// (set) Token: 0x060000D7 RID: 215 RVA: 0x00005A48 File Offset: 0x00003C48
		public bool reachedEndOfPath { get; private set; }

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x060000D8 RID: 216 RVA: 0x00005A54 File Offset: 0x00003C54
		public bool reachedDestination
		{
			get
			{
				if (!this.reachedEndOfPath || !this.interpolator.valid)
				{
					return false;
				}
				Vector3 vector = this.destination - this.interpolator.endPoint;
				if (this.orientation == OrientationMode.YAxisForward)
				{
					vector.z = 0f;
				}
				else
				{
					vector.y = 0f;
				}
				return this.remainingDistance + vector.magnitude < 0.05f;
			}
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x060000D9 RID: 217 RVA: 0x00005ACA File Offset: 0x00003CCA
		// (set) Token: 0x060000DA RID: 218 RVA: 0x00005AD2 File Offset: 0x00003CD2
		public Vector3 destination { get; set; }

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x060000DB RID: 219 RVA: 0x00005ADC File Offset: 0x00003CDC
		public NativeMovementPlane movementPlane
		{
			get
			{
				if (this.path != null && this.path.path.Count > 0)
				{
					NavGraph graph = this.path.path[0].Graph;
					NavmeshBase navmeshBase = graph as NavmeshBase;
					if (navmeshBase != null)
					{
						return new NativeMovementPlane(navmeshBase.transform.ToSimpleMovementPlane());
					}
					GridGraph gridGraph = graph as GridGraph;
					if (gridGraph != null)
					{
						return new NativeMovementPlane(gridGraph.transform.ToSimpleMovementPlane());
					}
				}
				return new NativeMovementPlane(quaternion.identity);
			}
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x060000DC RID: 220 RVA: 0x00005B5B File Offset: 0x00003D5B
		// (set) Token: 0x060000DD RID: 221 RVA: 0x00005B63 File Offset: 0x00003D63
		public bool updatePosition { get; set; } = true;

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x060000DE RID: 222 RVA: 0x00005B6C File Offset: 0x00003D6C
		// (set) Token: 0x060000DF RID: 223 RVA: 0x00005B74 File Offset: 0x00003D74
		public bool updateRotation { get; set; } = true;

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x060000E0 RID: 224 RVA: 0x00005B7D File Offset: 0x00003D7D
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

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x060000E1 RID: 225 RVA: 0x00005B99 File Offset: 0x00003D99
		// (set) Token: 0x060000E2 RID: 226 RVA: 0x00005BB5 File Offset: 0x00003DB5
		public Quaternion rotation
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

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x060000E3 RID: 227 RVA: 0x00005BD3 File Offset: 0x00003DD3
		public Vector3 endOfPath
		{
			get
			{
				if (this.interpolator.valid)
				{
					return this.interpolator.endPoint;
				}
				if (float.IsFinite(this.destination.x))
				{
					return this.destination;
				}
				return this.position;
			}
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x00003788 File Offset: 0x00001988
		void IAstarAI.Move(Vector3 deltaPosition)
		{
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x060000E5 RID: 229 RVA: 0x00005C0D File Offset: 0x00003E0D
		// (set) Token: 0x060000E6 RID: 230 RVA: 0x00003788 File Offset: 0x00001988
		float IAstarAI.radius
		{
			get
			{
				return 0f;
			}
			set
			{
			}
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x060000E7 RID: 231 RVA: 0x00005C0D File Offset: 0x00003E0D
		// (set) Token: 0x060000E8 RID: 232 RVA: 0x00003788 File Offset: 0x00001988
		float IAstarAI.height
		{
			get
			{
				return 0f;
			}
			set
			{
			}
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x060000E9 RID: 233 RVA: 0x00005C14 File Offset: 0x00003E14
		// (set) Token: 0x060000EA RID: 234 RVA: 0x00005C1C File Offset: 0x00003E1C
		float IAstarAI.maxSpeed
		{
			get
			{
				return this.speed;
			}
			set
			{
				this.speed = value;
			}
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x060000EB RID: 235 RVA: 0x00005C25 File Offset: 0x00003E25
		// (set) Token: 0x060000EC RID: 236 RVA: 0x00005C2D File Offset: 0x00003E2D
		bool IAstarAI.canSearch
		{
			get
			{
				return this.canSearch;
			}
			set
			{
				this.canSearch = value;
			}
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x060000ED RID: 237 RVA: 0x00005A2F File Offset: 0x00003C2F
		// (set) Token: 0x060000EE RID: 238 RVA: 0x00005A37 File Offset: 0x00003C37
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

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x060000EF RID: 239 RVA: 0x00005C36 File Offset: 0x00003E36
		public Vector3 velocity
		{
			get
			{
				if (Time.deltaTime <= 1E-05f)
				{
					return Vector3.zero;
				}
				return (this.previousPosition1 - this.previousPosition2) / Time.deltaTime;
			}
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x060000F0 RID: 240 RVA: 0x00005C65 File Offset: 0x00003E65
		Vector3 IAstarAI.desiredVelocity
		{
			get
			{
				return ((IAstarAI)this).velocity;
			}
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x060000F1 RID: 241 RVA: 0x00005C65 File Offset: 0x00003E65
		// (set) Token: 0x060000F2 RID: 242 RVA: 0x00005C6D File Offset: 0x00003E6D
		Vector3 IAstarAI.desiredVelocityWithoutLocalAvoidance
		{
			get
			{
				return ((IAstarAI)this).velocity;
			}
			set
			{
				throw new InvalidOperationException("The AILerp component does not support setting the desiredVelocityWithoutLocalAvoidance property since it does not make sense for how its movement works.");
			}
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x060000F3 RID: 243 RVA: 0x00005C79 File Offset: 0x00003E79
		Vector3 IAstarAI.steeringTarget
		{
			get
			{
				if (!this.interpolator.valid)
				{
					return this.simulatedPosition;
				}
				return this.interpolator.position + this.interpolator.tangent;
			}
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x060000F4 RID: 244 RVA: 0x00005CAA File Offset: 0x00003EAA
		// (set) Token: 0x060000F5 RID: 245 RVA: 0x00005CD4 File Offset: 0x00003ED4
		public float remainingDistance
		{
			get
			{
				if (!this.interpolator.valid)
				{
					return float.PositiveInfinity;
				}
				return Mathf.Max(this.interpolator.remainingDistance, 0f);
			}
			set
			{
				if (!this.interpolator.valid)
				{
					throw new InvalidOperationException("Cannot set the remaining distance on the AILerp component because it doesn't have a path to follow.");
				}
				this.interpolator.remainingDistance = Mathf.Max(value, 0f);
			}
		}

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x060000F6 RID: 246 RVA: 0x00005D04 File Offset: 0x00003F04
		public bool hasPath
		{
			get
			{
				return this.interpolator.valid;
			}
		}

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x060000F7 RID: 247 RVA: 0x00005D11 File Offset: 0x00003F11
		public bool pathPending
		{
			get
			{
				return !this.canSearchAgain;
			}
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x060000F8 RID: 248 RVA: 0x00005D1C File Offset: 0x00003F1C
		// (set) Token: 0x060000F9 RID: 249 RVA: 0x00005D24 File Offset: 0x00003F24
		public bool isStopped { get; set; }

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x060000FA RID: 250 RVA: 0x00005D2D File Offset: 0x00003F2D
		// (set) Token: 0x060000FB RID: 251 RVA: 0x00005D35 File Offset: 0x00003F35
		public Action onSearchPath { get; set; }

		// Token: 0x060000FC RID: 252 RVA: 0x00005D40 File Offset: 0x00003F40
		protected AILerp()
		{
			this.destination = new Vector3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity);
		}

		// Token: 0x060000FD RID: 253 RVA: 0x00005DD9 File Offset: 0x00003FD9
		protected override void Awake()
		{
			base.Awake();
			this.tr = base.transform;
			this.seeker = base.GetComponent<Seeker>();
			this.seeker.startEndModifier.adjustStartPoint = () => this.simulatedPosition;
		}

		// Token: 0x060000FE RID: 254 RVA: 0x00005E15 File Offset: 0x00004015
		protected virtual void Start()
		{
			this.startHasRun = true;
			this.Init();
		}

		// Token: 0x060000FF RID: 255 RVA: 0x00005E24 File Offset: 0x00004024
		protected virtual void OnEnable()
		{
			this.onPathComplete = new OnPathDelegate(this.OnPathComplete);
			this.Init();
		}

		// Token: 0x06000100 RID: 256 RVA: 0x00005E3F File Offset: 0x0000403F
		private void Init()
		{
			if (this.startHasRun)
			{
				this.Teleport(this.position, false);
				this.autoRepath.Reset();
				if (this.shouldRecalculatePath)
				{
					this.SearchPath();
				}
			}
		}

		// Token: 0x06000101 RID: 257 RVA: 0x00005E6F File Offset: 0x0000406F
		public void OnDisable()
		{
			this.ClearPath();
		}

		// Token: 0x06000102 RID: 258 RVA: 0x00005E78 File Offset: 0x00004078
		public void GetRemainingPath(List<Vector3> buffer, out bool stale)
		{
			buffer.Clear();
			if (!this.interpolator.valid)
			{
				buffer.Add(this.position);
				stale = true;
				return;
			}
			stale = false;
			this.interpolator.GetRemainingPath(buffer);
			buffer[0] = this.position;
		}

		// Token: 0x06000103 RID: 259 RVA: 0x00005EC4 File Offset: 0x000040C4
		public void GetRemainingPath(List<Vector3> buffer, List<PathPartWithLinkInfo> partsBuffer, out bool stale)
		{
			this.GetRemainingPath(buffer, out stale);
			if (partsBuffer != null)
			{
				partsBuffer.Clear();
				partsBuffer.Add(new PathPartWithLinkInfo
				{
					startIndex = 0,
					endIndex = buffer.Count - 1
				});
			}
		}

		// Token: 0x06000104 RID: 260 RVA: 0x00005F08 File Offset: 0x00004108
		public void Teleport(Vector3 position, bool clearPath = true)
		{
			if (clearPath)
			{
				this.ClearPath();
			}
			this.previousPosition2 = position;
			this.previousPosition1 = position;
			this.simulatedPosition = position;
			if (this.updatePosition)
			{
				this.tr.position = position;
			}
			this.reachedEndOfPath = false;
			if (clearPath)
			{
				this.SearchPath();
			}
		}

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x06000105 RID: 261 RVA: 0x00005F5B File Offset: 0x0000415B
		protected virtual bool shouldRecalculatePath
		{
			get
			{
				return this.canSearchAgain && this.autoRepath.ShouldRecalculatePath(this.position, 0f, this.destination, Time.time);
			}
		}

		// Token: 0x06000106 RID: 262 RVA: 0x00005F88 File Offset: 0x00004188
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
			Vector3 feetPosition = this.GetFeetPosition();
			this.canSearchAgain = false;
			this.SetPath(ABPath.Construct(feetPosition, this.destination, null), false);
		}

		// Token: 0x06000107 RID: 263 RVA: 0x00003788 File Offset: 0x00001988
		public virtual void OnTargetReached()
		{
		}

		// Token: 0x06000108 RID: 264 RVA: 0x00005FE0 File Offset: 0x000041E0
		protected virtual void OnPathComplete(Path _p)
		{
			ABPath abpath = _p as ABPath;
			if (abpath == null)
			{
				throw new Exception("This function only handles ABPaths, do not use special path types");
			}
			this.canSearchAgain = true;
			abpath.Claim(this);
			if (abpath.error)
			{
				abpath.Release(this, false);
				return;
			}
			if (this.interpolatePathSwitches)
			{
				this.ConfigurePathSwitchInterpolation();
			}
			ABPath abpath2 = this.path;
			this.path = abpath;
			this.reachedEndOfPath = false;
			RandomPath randomPath = this.path as RandomPath;
			if (randomPath != null)
			{
				this.destination = randomPath.originalEndPoint;
			}
			else
			{
				MultiTargetPath multiTargetPath = this.path as MultiTargetPath;
				if (multiTargetPath != null)
				{
					this.destination = multiTargetPath.originalEndPoint;
				}
			}
			if (this.path.vectorPath != null && this.path.vectorPath.Count == 1)
			{
				this.path.vectorPath.Insert(0, this.GetFeetPosition());
			}
			this.ConfigureNewPath();
			if (abpath2 != null)
			{
				abpath2.Release(this, false);
			}
			if (this.interpolator.remainingDistance < 0.0001f && !this.reachedEndOfPath)
			{
				this.reachedEndOfPath = true;
				this.OnTargetReached();
			}
		}

		// Token: 0x06000109 RID: 265 RVA: 0x000060EC File Offset: 0x000042EC
		protected virtual void ClearPath()
		{
			if (this.seeker != null)
			{
				this.seeker.CancelCurrentPathRequest(true);
			}
			this.canSearchAgain = true;
			this.reachedEndOfPath = false;
			if (this.path != null)
			{
				this.path.Release(this, false);
			}
			this.path = null;
			this.interpolatorPath.SetPath(null);
		}

		// Token: 0x0600010A RID: 266 RVA: 0x0000614C File Offset: 0x0000434C
		public void SetPath(Path path, bool updateDestinationFromPath = true)
		{
			if (updateDestinationFromPath)
			{
				ABPath abpath = path as ABPath;
				if (abpath != null && !(path is RandomPath))
				{
					this.destination = abpath.originalEndPoint;
				}
			}
			if (path == null)
			{
				this.ClearPath();
				return;
			}
			if (path.PipelineState == PathState.Created)
			{
				this.canSearchAgain = false;
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

		// Token: 0x0600010B RID: 267 RVA: 0x00006200 File Offset: 0x00004400
		protected virtual void ConfigurePathSwitchInterpolation()
		{
			bool flag = this.interpolator.valid && this.interpolator.remainingDistance < 0.0001f;
			if (this.interpolator.valid && !flag)
			{
				this.previousMovementOrigin = this.interpolator.position;
				this.previousMovementDirection = this.interpolator.tangent.normalized * this.interpolator.remainingDistance;
				this.pathSwitchInterpolationTime = 0f;
				return;
			}
			this.previousMovementOrigin = Vector3.zero;
			this.previousMovementDirection = Vector3.zero;
			this.pathSwitchInterpolationTime = float.PositiveInfinity;
		}

		// Token: 0x0600010C RID: 268 RVA: 0x000062A7 File Offset: 0x000044A7
		public virtual Vector3 GetFeetPosition()
		{
			return this.position;
		}

		// Token: 0x0600010D RID: 269 RVA: 0x000062B0 File Offset: 0x000044B0
		protected virtual void ConfigureNewPath()
		{
			bool valid = this.interpolator.valid;
			Vector3 vector = (valid ? this.interpolator.tangent : Vector3.zero);
			this.interpolatorPath.SetPath(this.path.vectorPath);
			this.interpolator = this.interpolatorPath.start;
			this.interpolator.MoveToClosestPoint(this.GetFeetPosition());
			if (this.interpolatePathSwitches && this.switchPathInterpolationSpeed > 0.01f && valid)
			{
				float num = Mathf.Max(-Vector3.Dot(vector.normalized, this.interpolator.tangent.normalized), 0f);
				this.interpolator.distance = this.interpolator.distance - this.speed * num * (1f / this.switchPathInterpolationSpeed);
			}
		}

		// Token: 0x0600010E RID: 270 RVA: 0x00006388 File Offset: 0x00004588
		protected virtual void Update()
		{
			if (this.shouldRecalculatePath)
			{
				this.SearchPath();
			}
			if (this.simulateMovement)
			{
				Vector3 vector;
				Quaternion quaternion;
				this.MovementUpdate(Time.deltaTime, out vector, out quaternion);
				this.FinalizeMovement(vector, quaternion);
			}
		}

		// Token: 0x0600010F RID: 271 RVA: 0x000063C4 File Offset: 0x000045C4
		public void MovementUpdate(float deltaTime, out Vector3 nextPosition, out Quaternion nextRotation)
		{
			if (this.updatePosition)
			{
				this.simulatedPosition = this.tr.position;
			}
			if (this.updateRotation)
			{
				this.simulatedRotation = this.tr.rotation;
			}
			Vector3 vector;
			nextPosition = this.CalculateNextPosition(out vector, this.isStopped ? 0f : deltaTime);
			if (this.enableRotation)
			{
				nextRotation = this.SimulateRotationTowards(vector, deltaTime);
				return;
			}
			nextRotation = this.simulatedRotation;
		}

		// Token: 0x06000110 RID: 272 RVA: 0x00006444 File Offset: 0x00004644
		public void FinalizeMovement(Vector3 nextPosition, Quaternion nextRotation)
		{
			this.previousPosition2 = this.previousPosition1;
			this.simulatedPosition = nextPosition;
			this.previousPosition1 = nextPosition;
			this.simulatedRotation = nextRotation;
			if (this.updatePosition)
			{
				this.tr.position = nextPosition;
			}
			if (this.updateRotation)
			{
				this.tr.rotation = nextRotation;
			}
		}

		// Token: 0x06000111 RID: 273 RVA: 0x0000649C File Offset: 0x0000469C
		private Quaternion SimulateRotationTowards(Vector3 direction, float deltaTime)
		{
			if (direction != Vector3.zero)
			{
				Quaternion quaternion = Quaternion.LookRotation(direction, (this.orientation == OrientationMode.YAxisForward) ? Vector3.back : Vector3.up);
				if (this.orientation == OrientationMode.YAxisForward)
				{
					quaternion *= Quaternion.Euler(90f, 0f, 0f);
				}
				return Quaternion.Slerp(this.simulatedRotation, quaternion, deltaTime * this.rotationSpeed);
			}
			return this.simulatedRotation;
		}

		// Token: 0x06000112 RID: 274 RVA: 0x00006514 File Offset: 0x00004714
		protected virtual Vector3 CalculateNextPosition(out Vector3 direction, float deltaTime)
		{
			if (!this.interpolator.valid)
			{
				direction = Vector3.zero;
				return this.simulatedPosition;
			}
			this.interpolator.distance = this.interpolator.distance + deltaTime * this.speed;
			if (this.interpolator.remainingDistance < 0.0001f && !this.reachedEndOfPath)
			{
				this.reachedEndOfPath = true;
				this.OnTargetReached();
			}
			direction = this.interpolator.tangent;
			this.pathSwitchInterpolationTime += deltaTime;
			float num = this.switchPathInterpolationSpeed * this.pathSwitchInterpolationTime;
			if (this.interpolatePathSwitches && num < 1f)
			{
				return Vector3.Lerp(this.previousMovementOrigin + Vector3.ClampMagnitude(this.previousMovementDirection, this.speed * this.pathSwitchInterpolationTime), this.interpolator.position, num);
			}
			return this.interpolator.position;
		}

		// Token: 0x06000113 RID: 275 RVA: 0x00006600 File Offset: 0x00004800
		protected override void OnUpgradeSerializedData(ref Migrations migrations, bool unityThread)
		{
			int num;
			if (migrations.TryMigrateFromLegacyFormat(out num) && num <= 3)
			{
				this.repathRate = this.repathRateCompatibility;
				this.canSearch = this.canSearchCompability;
			}
		}

		// Token: 0x06000114 RID: 276 RVA: 0x00006634 File Offset: 0x00004834
		public unsafe override void DrawGizmos()
		{
			this.tr = base.transform;
			this.autoRepath.DrawGizmos(*Draw.editor, this.position, 0f, new NativeMovementPlane((this.orientation == OrientationMode.YAxisForward) ? Quaternion.Euler(-90f, 0f, 0f) : Quaternion.identity));
		}

		// Token: 0x040000AA RID: 170
		public AutoRepathPolicy autoRepath = new AutoRepathPolicy();

		// Token: 0x040000AB RID: 171
		[FormerlySerializedAs("canMove")]
		public bool simulateMovement = true;

		// Token: 0x040000AC RID: 172
		public float speed = 3f;

		// Token: 0x040000AD RID: 173
		[FormerlySerializedAs("rotationIn2D")]
		public OrientationMode orientation;

		// Token: 0x040000AE RID: 174
		public bool enableRotation = true;

		// Token: 0x040000AF RID: 175
		public float rotationSpeed = 10f;

		// Token: 0x040000B0 RID: 176
		public bool interpolatePathSwitches = true;

		// Token: 0x040000B1 RID: 177
		public float switchPathInterpolationSpeed = 5f;

		// Token: 0x040000B6 RID: 182
		protected OnPathDelegate onPathComplete;

		// Token: 0x040000B9 RID: 185
		protected Seeker seeker;

		// Token: 0x040000BA RID: 186
		protected Transform tr;

		// Token: 0x040000BB RID: 187
		protected ABPath path;

		// Token: 0x040000BC RID: 188
		protected bool canSearchAgain = true;

		// Token: 0x040000BD RID: 189
		protected Vector3 previousMovementOrigin;

		// Token: 0x040000BE RID: 190
		protected Vector3 previousMovementDirection;

		// Token: 0x040000BF RID: 191
		protected float pathSwitchInterpolationTime;

		// Token: 0x040000C0 RID: 192
		protected PathInterpolator.Cursor interpolator;

		// Token: 0x040000C1 RID: 193
		protected PathInterpolator interpolatorPath = new PathInterpolator();

		// Token: 0x040000C2 RID: 194
		private bool startHasRun;

		// Token: 0x040000C3 RID: 195
		private Vector3 previousPosition1;

		// Token: 0x040000C4 RID: 196
		private Vector3 previousPosition2;

		// Token: 0x040000C5 RID: 197
		private Vector3 simulatedPosition;

		// Token: 0x040000C6 RID: 198
		private Quaternion simulatedRotation;

		// Token: 0x040000C7 RID: 199
		[SerializeField]
		[HideInInspector]
		[FormerlySerializedAs("repathRate")]
		private float repathRateCompatibility = float.NaN;

		// Token: 0x040000C8 RID: 200
		[SerializeField]
		[HideInInspector]
		[FormerlySerializedAs("canSearch")]
		private bool canSearchCompability;
	}
}
