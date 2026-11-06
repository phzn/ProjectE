using System;
using System.Collections;
using UnityEngine;

namespace Features.NPC.SceneSystem
{
	// Token: 0x02000F22 RID: 3874
	[CreateAssetMenu(fileName = "RotateTowardAction", menuName = "NPC/Scene System/Actions/Rotate Toward")]
	public class RotateTowardAction : SceneActionSO
	{
		// Token: 0x0600747C RID: 29820 RVA: 0x002F4A15 File Offset: 0x002F2C15
		public override IEnumerator Execute(SceneContext context, string targetSlotId)
		{
			GameObject npc = context.GetNPC(targetSlotId);
			if (npc == null)
			{
				Debug.LogWarning("[SceneAction] RotateToward: No NPC for slot '" + targetSlotId + "'");
				yield break;
			}
			Vector3 targetPos = Vector3.zero;
			bool flag = false;
			switch (this.target)
			{
			case RotateTowardAction.RotateTarget.Waypoint:
			{
				Transform waypoint = context.GetWaypoint(this.waypointName);
				if (waypoint != null)
				{
					targetPos = waypoint.position;
					flag = true;
				}
				break;
			}
			case RotateTowardAction.RotateTarget.OtherNPC:
			{
				GameObject npc2 = context.GetNPC(this.otherSlotId);
				if (npc2 != null)
				{
					targetPos = npc2.transform.position;
					flag = true;
				}
				break;
			}
			case RotateTowardAction.RotateTarget.Player:
				if (context.PlayerTransform != null)
				{
					targetPos = context.PlayerTransform.position;
					flag = true;
				}
				break;
			}
			if (!flag)
			{
				Debug.LogWarning("[SceneAction] RotateToward: Target not found for slot '" + targetSlotId + "'");
				yield break;
			}
			float elapsed = 0f;
			while (!context.IsInterrupted && elapsed < this.timeout)
			{
				if (NPCSimulationPause.IsPaused)
				{
					yield return null;
				}
				else
				{
					Vector3 normalized = (targetPos - npc.transform.position).normalized;
					normalized.y = 0f;
					if (normalized.sqrMagnitude < 0.001f)
					{
						break;
					}
					Quaternion quaternion = Quaternion.LookRotation(normalized);
					if (Quaternion.Angle(npc.transform.rotation, quaternion) <= this.angleThreshold)
					{
						break;
					}
					npc.transform.rotation = Quaternion.RotateTowards(npc.transform.rotation, quaternion, this.rotationSpeed * NPCSimulationPause.SimulationDeltaTime);
					elapsed += NPCSimulationPause.SimulationDeltaTime;
					yield return null;
				}
			}
			yield break;
		}

		// Token: 0x0400773A RID: 30522
		[Header("Rotate Settings")]
		[Tooltip("What to rotate toward")]
		public RotateTowardAction.RotateTarget target;

		// Token: 0x0400773B RID: 30523
		[Tooltip("Waypoint name (when target = Waypoint)")]
		public string waypointName;

		// Token: 0x0400773C RID: 30524
		[Tooltip("Other NPC slot ID (when target = OtherNPC)")]
		public string otherSlotId;

		// Token: 0x0400773D RID: 30525
		[Tooltip("Rotation speed (degrees/second)")]
		public float rotationSpeed = 180f;

		// Token: 0x0400773E RID: 30526
		[Tooltip("Angle threshold to consider rotation complete (degrees)")]
		public float angleThreshold = 5f;

		// Token: 0x0400773F RID: 30527
		[Tooltip("Maximum time to wait for rotation (seconds)")]
		public float timeout = 5f;

		// Token: 0x02000F23 RID: 3875
		public enum RotateTarget
		{
			// Token: 0x04007741 RID: 30529
			Waypoint,
			// Token: 0x04007742 RID: 30530
			OtherNPC,
			// Token: 0x04007743 RID: 30531
			Player
		}
	}
}
