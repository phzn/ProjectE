using System;
using System.Collections;
using UnityEngine;

namespace Features.NPC.SceneSystem
{
	// Token: 0x02000F20 RID: 3872
	[CreateAssetMenu(fileName = "MoveToWaypointAction", menuName = "NPC/Scene System/Actions/Move To Waypoint")]
	public class MoveToWaypointAction : SceneActionSO
	{
		// Token: 0x06007473 RID: 29811 RVA: 0x002F471B File Offset: 0x002F291B
		public override IEnumerator Execute(SceneContext context, string targetSlotId)
		{
			AStarNPCController controller = context.GetController(targetSlotId);
			if (controller == null)
			{
				Debug.LogWarning("[SceneAction] MoveToWaypoint: No controller for slot '" + targetSlotId + "'");
				yield break;
			}
			Transform waypoint = context.GetWaypoint(this.waypointName);
			if (waypoint == null)
			{
				Debug.LogWarning("[SceneAction] MoveToWaypoint: Waypoint '" + this.waypointName + "' not found");
				yield break;
			}
			while (NPCSimulationPause.IsPaused && !context.IsInterrupted)
			{
				yield return null;
			}
			if (context.IsInterrupted)
			{
				yield break;
			}
			if (controller.IsNavigationLocked)
			{
				controller.UnlockNavigation(false);
			}
			if (controller.IsManualMode)
			{
				controller.ExitManualMode(false);
			}
			while ((NPCSimulationPause.IsPaused || !controller.IsNavigationReady) && !context.IsInterrupted)
			{
				yield return null;
			}
			if (context.IsInterrupted)
			{
				yield break;
			}
			controller.SetDestination(waypoint.position);
			float elapsed = 0f;
			while (!context.IsInterrupted)
			{
				if (NPCSimulationPause.IsPaused)
				{
					yield return null;
				}
				else
				{
					if (Vector3.Distance(controller.transform.position, waypoint.position) <= this.arrivalDistance || controller.HasReachedDestination())
					{
						break;
					}
					if (this.timeout > 0f && elapsed >= this.timeout)
					{
						Debug.LogWarning(string.Concat(new string[] { "[SceneAction] MoveToWaypoint: Timeout for slot '", targetSlotId, "' to reach '", this.waypointName, "'" }));
						break;
					}
					elapsed += NPCSimulationPause.SimulationDeltaTime;
					yield return null;
				}
			}
			controller.Stop();
			yield break;
		}

		// Token: 0x06007474 RID: 29812 RVA: 0x002F4738 File Offset: 0x002F2938
		public override void OnInterrupt(SceneContext context, string targetSlotId)
		{
			AStarNPCController controller = context.GetController(targetSlotId);
			if (controller == null)
			{
				return;
			}
			controller.Stop();
		}

		// Token: 0x0400772F RID: 30511
		[Header("Move Settings")]
		[Tooltip("Name of the waypoint Transform in the SceneTrigger")]
		public string waypointName;

		// Token: 0x04007730 RID: 30512
		[Tooltip("Distance threshold to consider destination reached")]
		public float arrivalDistance = 0.5f;

		// Token: 0x04007731 RID: 30513
		[Tooltip("Maximum time to wait for arrival before giving up (0 = no timeout)")]
		public float timeout = 30f;
	}
}
