using System;
using Pathfinding.Drawing;
using Pathfinding.Serialization;
using Pathfinding.Util;
using UnityEngine;
using UnityEngine.Serialization;

namespace Pathfinding.RVO
{
	// Token: 0x020002D7 RID: 727
	[AddComponentMenu("Pathfinding/Local Avoidance/RVO Controller")]
	[UniqueComponent(tag = "rvo")]
	[HelpURL("https://arongranberg.com/astar/documentation/stable/rvocontroller.html")]
	public class RVOController : VersionedMonoBehaviour
	{
		// Token: 0x170002B7 RID: 695
		// (get) Token: 0x06001203 RID: 4611 RVA: 0x0006F89D File Offset: 0x0006DA9D
		// (set) Token: 0x06001204 RID: 4612 RVA: 0x0006F8B9 File Offset: 0x0006DAB9
		public float radius
		{
			get
			{
				if (this.ai != null)
				{
					return this.ai.radius;
				}
				return this.radiusBackingField;
			}
			set
			{
				if (this.ai != null)
				{
					this.ai.radius = value;
				}
				this.radiusBackingField = value;
			}
		}

		// Token: 0x170002B8 RID: 696
		// (get) Token: 0x06001205 RID: 4613 RVA: 0x0006F8D6 File Offset: 0x0006DAD6
		// (set) Token: 0x06001206 RID: 4614 RVA: 0x0006F8F2 File Offset: 0x0006DAF2
		public float height
		{
			get
			{
				if (this.ai != null)
				{
					return this.ai.height;
				}
				return this.heightBackingField;
			}
			set
			{
				if (this.ai != null)
				{
					this.ai.height = value;
				}
				this.heightBackingField = value;
			}
		}

		// Token: 0x170002B9 RID: 697
		// (get) Token: 0x06001207 RID: 4615 RVA: 0x0006F90F File Offset: 0x0006DB0F
		// (set) Token: 0x06001208 RID: 4616 RVA: 0x0006F931 File Offset: 0x0006DB31
		public float center
		{
			get
			{
				if (this.ai != null)
				{
					return this.ai.height / 2f;
				}
				return this.centerBackingField;
			}
			set
			{
				this.centerBackingField = value;
			}
		}

		// Token: 0x170002BA RID: 698
		// (get) Token: 0x06001209 RID: 4617 RVA: 0x0006F93A File Offset: 0x0006DB3A
		public MovementPlane movementPlaneMode
		{
			get
			{
				SimulatorBurst simulator = this.simulator;
				if (simulator != null)
				{
					return simulator.MovementPlane;
				}
				RVOSimulator active = RVOSimulator.active;
				if (active == null)
				{
					return MovementPlane.XZ;
				}
				return active.movementPlane;
			}
		}

		// Token: 0x170002BB RID: 699
		// (get) Token: 0x0600120A RID: 4618 RVA: 0x0006F95C File Offset: 0x0006DB5C
		// (set) Token: 0x0600120B RID: 4619 RVA: 0x0006F9D0 File Offset: 0x0006DBD0
		public SimpleMovementPlane movementPlane
		{
			get
			{
				SimulatorBurst simulator = this.simulator;
				MovementPlane? movementPlane;
				if (simulator == null)
				{
					RVOSimulator active = RVOSimulator.active;
					movementPlane = ((active != null) ? new MovementPlane?(active.movementPlane) : null);
				}
				else
				{
					movementPlane = new MovementPlane?(simulator.MovementPlane);
				}
				MovementPlane? movementPlane2 = movementPlane;
				if (movementPlane2 != null)
				{
					if (movementPlane2.Value == MovementPlane.Arbitrary)
					{
						return this.movementPlaneBackingField;
					}
					if (movementPlane2.Value == MovementPlane.XY)
					{
						return SimpleMovementPlane.XYPlane;
					}
				}
				return SimpleMovementPlane.XZPlane;
			}
			set
			{
				SimulatorBurst simulator = this.simulator;
				MovementPlane? movementPlane;
				if (simulator == null)
				{
					RVOSimulator active = RVOSimulator.active;
					movementPlane = ((active != null) ? new MovementPlane?(active.movementPlane) : null);
				}
				else
				{
					movementPlane = new MovementPlane?(simulator.MovementPlane);
				}
				MovementPlane? movementPlane2 = movementPlane;
				if (movementPlane2 != null && movementPlane2.Value != MovementPlane.Arbitrary)
				{
					throw new InvalidOperationException("Cannot set the movement plane unless the RVOSimulator's movement plane setting is set to Arbitrary.");
				}
				this.movementPlaneBackingField = value;
			}
		}

		// Token: 0x170002BC RID: 700
		// (get) Token: 0x0600120C RID: 4620 RVA: 0x0006FA37 File Offset: 0x0006DC37
		// (set) Token: 0x0600120D RID: 4621 RVA: 0x0006FA3F File Offset: 0x0006DC3F
		public IAgent rvoAgent { get; private set; }

		// Token: 0x170002BD RID: 701
		// (get) Token: 0x0600120E RID: 4622 RVA: 0x0006FA48 File Offset: 0x0006DC48
		// (set) Token: 0x0600120F RID: 4623 RVA: 0x0006FA50 File Offset: 0x0006DC50
		private SimulatorBurst simulator { get; set; }

		// Token: 0x170002BE RID: 702
		// (get) Token: 0x06001210 RID: 4624 RVA: 0x0006FA59 File Offset: 0x0006DC59
		// (set) Token: 0x06001211 RID: 4625 RVA: 0x0006FA7B File Offset: 0x0006DC7B
		protected IAstarAI ai
		{
			get
			{
				if (this.aiBackingField as MonoBehaviour == null)
				{
					this.aiBackingField = null;
				}
				return this.aiBackingField;
			}
			set
			{
				this.aiBackingField = value;
			}
		}

		// Token: 0x170002BF RID: 703
		// (get) Token: 0x06001212 RID: 4626 RVA: 0x0006FA84 File Offset: 0x0006DC84
		public Vector3 position
		{
			get
			{
				this.simulator.BlockUntilSimulationStepDone();
				return this.rvoAgent.Position;
			}
		}

		// Token: 0x170002C0 RID: 704
		// (get) Token: 0x06001213 RID: 4627 RVA: 0x0006FA9C File Offset: 0x0006DC9C
		// (set) Token: 0x06001214 RID: 4628 RVA: 0x0006FACF File Offset: 0x0006DCCF
		public Vector3 velocity
		{
			get
			{
				float num = ((Time.deltaTime > 0.0001f) ? Time.deltaTime : 0.02f);
				return this.CalculateMovementDelta(num) / num;
			}
			set
			{
				this.simulator.BlockUntilSimulationStepDone();
				this.rvoAgent.ForceSetVelocity(value);
			}
		}

		// Token: 0x06001215 RID: 4629 RVA: 0x0006FAE8 File Offset: 0x0006DCE8
		public Vector3 CalculateMovementDelta(float deltaTime)
		{
			return this.CalculateMovementDelta((this.ai != null) ? this.ai.position : this.tr.position, deltaTime);
		}

		// Token: 0x06001216 RID: 4630 RVA: 0x0006FB14 File Offset: 0x0006DD14
		public Vector3 CalculateMovementDelta(Vector3 position, float deltaTime)
		{
			if (this.rvoAgent == null)
			{
				return Vector3.zero;
			}
			Vector2 vector = this.movementPlane.ToPlane(this.rvoAgent.CalculatedTargetPoint - position);
			return this.movementPlane.ToWorld(Vector2.ClampMagnitude(vector, this.rvoAgent.CalculatedSpeed * deltaTime), 0f);
		}

		// Token: 0x170002C1 RID: 705
		// (get) Token: 0x06001217 RID: 4631 RVA: 0x0006FB75 File Offset: 0x0006DD75
		public bool AvoidingAnyAgents
		{
			get
			{
				return this.rvoAgent != null && this.rvoAgent.AvoidingAnyAgents;
			}
		}

		// Token: 0x06001218 RID: 4632 RVA: 0x0006FB8C File Offset: 0x0006DD8C
		public void SetCollisionNormal(Vector3 normal)
		{
			this.simulator.BlockUntilSimulationStepDone();
			this.rvoAgent.SetCollisionNormal(normal);
		}

		// Token: 0x06001219 RID: 4633 RVA: 0x0006FBA5 File Offset: 0x0006DDA5
		public void SetObstacleQuery(GraphNode sourceNode)
		{
			this.obstacleQuery = sourceNode;
		}

		// Token: 0x0600121A RID: 4634 RVA: 0x0006FBB0 File Offset: 0x0006DDB0
		public Vector2 To2D(Vector3 p)
		{
			return this.movementPlane.ToPlane(p);
		}

		// Token: 0x0600121B RID: 4635 RVA: 0x0006FBCC File Offset: 0x0006DDCC
		public Vector2 To2D(Vector3 p, out float elevation)
		{
			return this.movementPlane.ToPlane(p, out elevation);
		}

		// Token: 0x0600121C RID: 4636 RVA: 0x0006FBEC File Offset: 0x0006DDEC
		public Vector3 To3D(Vector2 p, float elevationCoordinate)
		{
			return this.movementPlane.ToWorld(p, elevationCoordinate);
		}

		// Token: 0x0600121D RID: 4637 RVA: 0x0006FC09 File Offset: 0x0006DE09
		private void OnDisable()
		{
			if (this.simulator == null)
			{
				return;
			}
			this.simulator.RemoveAgent(this.rvoAgent);
			this.simulator = null;
			this.rvoAgent = null;
		}

		// Token: 0x0600121E RID: 4638 RVA: 0x0006FC34 File Offset: 0x0006DE34
		private void OnEnable()
		{
			this.tr = base.transform;
			this.ai = base.GetComponent<IAstarAI>();
			AIBase aibase = this.ai as AIBase;
			if (aibase != null)
			{
				aibase.FindComponents();
			}
			if (RVOSimulator.active == null)
			{
				Debug.LogError("No RVOSimulator component found in the scene. Please add one.");
				base.enabled = false;
				return;
			}
			this.simulator = RVOSimulator.active.GetSimulator();
			this.rvoAgent = this.simulator.AddAgent(Vector3.zero);
			this.rvoAgent.PreCalculationCallback = new Action(this.UpdateAgentProperties);
			this.rvoAgent.DestroyedCallback = new Action(this.OnAgentDestroyed);
		}

		// Token: 0x0600121F RID: 4639 RVA: 0x0006FCE1 File Offset: 0x0006DEE1
		private void OnAgentDestroyed()
		{
			if (base.gameObject.activeInHierarchy)
			{
				this.simulator = null;
				this.rvoAgent = null;
				base.enabled = false;
			}
		}

		// Token: 0x06001220 RID: 4640 RVA: 0x0006FD08 File Offset: 0x0006DF08
		protected void UpdateAgentProperties()
		{
			Vector3 localScale = this.tr.localScale;
			this.rvoAgent.Radius = Mathf.Max(0.001f, this.radius * Mathf.Abs(localScale.x));
			this.rvoAgent.AgentTimeHorizon = this.agentTimeHorizon;
			this.rvoAgent.ObstacleTimeHorizon = this.obstacleTimeHorizon;
			this.rvoAgent.Locked = this.locked;
			this.rvoAgent.MaxNeighbours = this.maxNeighbours;
			this.rvoAgent.DebugFlags = this.debug;
			this.rvoAgent.Layer = this.layer;
			this.rvoAgent.CollidesWith = this.collidesWith;
			SimpleMovementPlane movementPlane = this.movementPlane;
			this.rvoAgent.MovementPlane = movementPlane;
			float num;
			Vector2 vector = movementPlane.ToPlane((this.ai != null) ? this.ai.position : this.tr.position, out num);
			if (this.movementPlaneMode == MovementPlane.XY)
			{
				this.rvoAgent.Height = 1f;
				this.rvoAgent.Position = movementPlane.ToWorld(vector, 0f);
			}
			else
			{
				this.rvoAgent.Height = this.height * localScale.y;
				this.rvoAgent.Position = movementPlane.ToWorld(vector, num + (this.center - 0.5f * this.height) * localScale.y);
			}
			ReachedEndOfPath calculatedEffectivelyReachedDestination = this.rvoAgent.CalculatedEffectivelyReachedDestination;
			float num2 = this.priority * this.priorityMultiplier;
			float num3 = this.flowFollowingStrength;
			if (calculatedEffectivelyReachedDestination == ReachedEndOfPath.Reached)
			{
				num3 = 1f;
				num2 *= 0.3f;
			}
			else if (calculatedEffectivelyReachedDestination == ReachedEndOfPath.ReachedSoon)
			{
				num3 = 1f;
				num2 *= 0.45f;
			}
			this.rvoAgent.Priority = num2;
			this.rvoAgent.FlowFollowingStrength = num3;
			this.rvoAgent.SetObstacleQuery(this.obstacleQuery);
			this.obstacleQuery = null;
		}

		// Token: 0x06001221 RID: 4641 RVA: 0x0006FEFA File Offset: 0x0006E0FA
		public void SetTarget(Vector3 pos, float speed, float maxSpeed, Vector3 endOfPath)
		{
			if (this.rvoAgent == null)
			{
				return;
			}
			this.simulator.BlockUntilSimulationStepDone();
			this.rvoAgent.SetTarget(pos, speed, maxSpeed, endOfPath);
			if (this.lockWhenNotMoving)
			{
				this.locked = speed < 0.001f;
			}
		}

		// Token: 0x06001222 RID: 4642 RVA: 0x0006FF38 File Offset: 0x0006E138
		public void Move(Vector3 velocity)
		{
			if (this.rvoAgent == null)
			{
				return;
			}
			this.simulator.BlockUntilSimulationStepDone();
			float magnitude = this.movementPlane.ToPlane(velocity).magnitude;
			this.rvoAgent.SetTarget(((this.ai != null) ? this.ai.position : this.tr.position) + velocity, magnitude, magnitude, new Vector3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity));
			if (this.lockWhenNotMoving)
			{
				this.locked = magnitude < 0.001f;
			}
		}

		// Token: 0x06001223 RID: 4643 RVA: 0x0006FFD0 File Offset: 0x0006E1D0
		public override void DrawGizmos()
		{
			this.tr = base.transform;
			if (this.ai == null)
			{
				Color color = AIBase.ShapeGizmoColor * (this.locked ? 0.5f : 1f);
				Vector3 position = base.transform.position;
				Vector3 localScale = this.tr.localScale;
				if (this.movementPlaneMode == MovementPlane.XY)
				{
					Draw.WireCylinder(position, Vector3.forward, 0f, this.radius * localScale.x, color);
					return;
				}
				Draw.WireCylinder(position + this.To3D(Vector2.zero, this.center - this.height * 0.5f) * localScale.y, this.To3D(Vector2.zero, 1f), this.height * localScale.y, this.radius * localScale.x, color);
			}
		}

		// Token: 0x06001224 RID: 4644 RVA: 0x000700C8 File Offset: 0x0006E2C8
		protected override void OnUpgradeSerializedData(ref Migrations migrations, bool unityThread)
		{
			int num;
			if (migrations.TryMigrateFromLegacyFormat(out num) && num > 1)
			{
				migrations.MarkMigrationFinished(0);
			}
			if (migrations.AddAndMaybeRunMigration(0, unityThread))
			{
				if (base.transform.localScale.y != 0f)
				{
					this.centerBackingField /= Mathf.Abs(base.transform.localScale.y);
				}
				if (base.transform.localScale.y != 0f)
				{
					this.heightBackingField /= Mathf.Abs(base.transform.localScale.y);
				}
				if (base.transform.localScale.x != 0f)
				{
					this.radiusBackingField /= Mathf.Abs(base.transform.localScale.x);
				}
			}
		}

		// Token: 0x04000D46 RID: 3398
		[SerializeField]
		[FormerlySerializedAs("radius")]
		internal float radiusBackingField = 0.5f;

		// Token: 0x04000D47 RID: 3399
		[SerializeField]
		[FormerlySerializedAs("height")]
		private float heightBackingField = 2f;

		// Token: 0x04000D48 RID: 3400
		[SerializeField]
		[FormerlySerializedAs("center")]
		private float centerBackingField = 1f;

		// Token: 0x04000D49 RID: 3401
		[Tooltip("A locked unit cannot move. Other units will still avoid it. But avoidance quality is not the best")]
		public bool locked;

		// Token: 0x04000D4A RID: 3402
		[Tooltip("Automatically set #locked to true when desired velocity is approximately zero")]
		public bool lockWhenNotMoving;

		// Token: 0x04000D4B RID: 3403
		[Tooltip("How far into the future to look for collisions with other agents (in seconds)")]
		public float agentTimeHorizon = 2f;

		// Token: 0x04000D4C RID: 3404
		[Tooltip("How far into the future to look for collisions with obstacles (in seconds)")]
		public float obstacleTimeHorizon = 0.5f;

		// Token: 0x04000D4D RID: 3405
		[Tooltip("Max number of other agents to take into account.\nA smaller value can reduce CPU load, a higher value can lead to better local avoidance quality.")]
		public int maxNeighbours = 10;

		// Token: 0x04000D4E RID: 3406
		public RVOLayer layer = RVOLayer.DefaultAgent;

		// Token: 0x04000D4F RID: 3407
		[EnumFlag]
		public RVOLayer collidesWith = (RVOLayer)(-1);

		// Token: 0x04000D50 RID: 3408
		[HideInInspector]
		[Obsolete]
		public float wallAvoidForce = 1f;

		// Token: 0x04000D51 RID: 3409
		[HideInInspector]
		[Obsolete]
		public float wallAvoidFalloff = 1f;

		// Token: 0x04000D52 RID: 3410
		[Tooltip("How strongly other agents will avoid this agent")]
		[Range(0f, 1f)]
		public float priority = 0.5f;

		// Token: 0x04000D53 RID: 3411
		[NonSerialized]
		public float priorityMultiplier = 1f;

		// Token: 0x04000D54 RID: 3412
		[NonSerialized]
		public float flowFollowingStrength;

		// Token: 0x04000D55 RID: 3413
		private GraphNode obstacleQuery;

		// Token: 0x04000D58 RID: 3416
		protected Transform tr;

		// Token: 0x04000D59 RID: 3417
		[SerializeField]
		[FormerlySerializedAs("ai")]
		private IAstarAI aiBackingField;

		// Token: 0x04000D5A RID: 3418
		internal SimpleMovementPlane movementPlaneBackingField = GraphTransform.xzPlane.ToSimpleMovementPlane();

		// Token: 0x04000D5B RID: 3419
		public AgentDebugFlags debug;

		// Token: 0x020002D8 RID: 728
		[Flags]
		private enum RVOControllerMigrations
		{
			// Token: 0x04000D5D RID: 3421
			MigrateScale = 0
		}
	}
}
