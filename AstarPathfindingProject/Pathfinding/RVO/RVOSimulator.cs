using System;
using UnityEngine;

namespace Pathfinding.RVO
{
	// Token: 0x020002DC RID: 732
	[ExecuteInEditMode]
	[AddComponentMenu("Pathfinding/Local Avoidance/RVO Simulator")]
	[HelpURL("https://arongranberg.com/astar/documentation/stable/rvosimulator.html")]
	public class RVOSimulator : VersionedMonoBehaviour
	{
		// Token: 0x170002C6 RID: 710
		// (get) Token: 0x0600122C RID: 4652 RVA: 0x00070262 File Offset: 0x0006E462
		// (set) Token: 0x0600122D RID: 4653 RVA: 0x00070269 File Offset: 0x0006E469
		public static RVOSimulator active { get; private set; }

		// Token: 0x0600122E RID: 4654 RVA: 0x00070271 File Offset: 0x0006E471
		public SimulatorBurst GetSimulator()
		{
			if (this.simulatorBurst == null && Application.isPlaying)
			{
				this.simulatorBurst = new SimulatorBurst(this.movementPlane);
			}
			return this.simulatorBurst;
		}

		// Token: 0x0600122F RID: 4655 RVA: 0x0007029C File Offset: 0x0006E49C
		private void OnEnable()
		{
			if (RVOSimulator.active != null)
			{
				if (RVOSimulator.active != this && Application.isPlaying)
				{
					if (base.enabled)
					{
						Debug.LogWarning("Another RVOSimulator component is already in the scene. More than one RVOSimulator component cannot be active at the same time. Disabling this one.", this);
					}
					base.enabled = false;
				}
				return;
			}
			RVOSimulator.active = this;
		}

		// Token: 0x06001230 RID: 4656 RVA: 0x000702EC File Offset: 0x0006E4EC
		private void Update()
		{
			if (!Application.isPlaying)
			{
				return;
			}
			if (AstarPath.active != null && AstarPath.active.IsNavigationSuspended)
			{
				return;
			}
			if (this.desiredSimulationFPS < 1)
			{
				this.desiredSimulationFPS = 1;
			}
			SimulatorBurst simulator = this.GetSimulator();
			simulator.DesiredDeltaTime = 1f / (float)this.desiredSimulationFPS;
			simulator.SymmetryBreakingBias = this.symmetryBreakingBias;
			simulator.HardCollisions = this.hardCollisions;
			simulator.drawQuadtree = this.drawQuadtree;
			simulator.UseNavmeshAsObstacle = this.useNavmeshAsObstacle;
		}

		// Token: 0x06001231 RID: 4657 RVA: 0x00070373 File Offset: 0x0006E573
		private void OnDisable()
		{
			if (RVOSimulator.active == this)
			{
				RVOSimulator.active = null;
			}
			if (this.simulatorBurst != null)
			{
				this.simulatorBurst.OnDestroy();
				this.simulatorBurst = null;
			}
		}

		// Token: 0x06001232 RID: 4658 RVA: 0x00003788 File Offset: 0x00001988
		public override void DrawGizmos()
		{
		}

		// Token: 0x04000D65 RID: 3429
		[Tooltip("Desired FPS for rvo simulation. It is usually not necessary to run a crowd simulation at a very high fps.\nUsually 10-30 fps is enough, but can be increased for better quality.\nThe rvo simulation will never run at a higher fps than the game")]
		public int desiredSimulationFPS = 20;

		// Token: 0x04000D66 RID: 3430
		[Tooltip("Number of RVO worker threads. If set to None, no multithreading will be used.")]
		[Obsolete("The number of worker threads is now set by the unity job system", true)]
		public ThreadCount workerThreads = ThreadCount.Two;

		// Token: 0x04000D67 RID: 3431
		[Tooltip("Calculate local avoidance in between frames.\nThis can increase jitter in the agents' movement so use it only if you really need the performance boost. It will also reduce the responsiveness of the agents to the commands you send to them.")]
		[Obsolete("Double buffering has been removed")]
		public bool doubleBuffering;

		// Token: 0x04000D68 RID: 3432
		public bool hardCollisions = true;

		// Token: 0x04000D69 RID: 3433
		[Tooltip("Bias agents to pass each other on the right side.\nIf the desired velocity of an agent puts it on a collision course with another agent or an obstacle its desired velocity will be rotated this number of radians (1 radian is approximately 57°) to the right. This helps to break up symmetries and makes it possible to resolve some situations much faster.\n\nWhen many agents have the same goal this can however have the side effect that the group clustered around the target point may as a whole start to spin around the target point.")]
		[Range(0f, 0.2f)]
		public float symmetryBreakingBias = 0.1f;

		// Token: 0x04000D6A RID: 3434
		[Tooltip("Determines if the XY (2D) or XZ (3D) plane is used for movement")]
		public MovementPlane movementPlane;

		// Token: 0x04000D6B RID: 3435
		public bool useNavmeshAsObstacle;

		// Token: 0x04000D6C RID: 3436
		public bool drawQuadtree;

		// Token: 0x04000D6D RID: 3437
		private SimulatorBurst simulatorBurst;
	}
}
