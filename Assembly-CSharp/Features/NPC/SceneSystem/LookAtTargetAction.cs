using System;
using System.Collections;
using UnityEngine;

namespace Features.NPC.SceneSystem
{
	// Token: 0x02000F1D RID: 3869
	[CreateAssetMenu(fileName = "LookAtTargetAction", menuName = "NPC/Scene System/Actions/Look At Target")]
	public class LookAtTargetAction : SceneActionSO
	{
		// Token: 0x0600746A RID: 29802 RVA: 0x002F44C0 File Offset: 0x002F26C0
		public override IEnumerator Execute(SceneContext context, string targetSlotId)
		{
			GameObject npc = context.GetNPC(targetSlotId);
			if (npc == null)
			{
				Debug.LogWarning("[SceneAction] LookAtTarget: No NPC for slot '" + targetSlotId + "'");
				yield break;
			}
			NPCSimpleLookAt lookAt = npc.GetComponent<NPCSimpleLookAt>();
			if (lookAt == null)
			{
				Debug.LogWarning("[SceneAction] LookAtTarget: No NPCSimpleLookAt on slot '" + targetSlotId + "'");
				yield break;
			}
			switch (this.target)
			{
			case LookAtTargetAction.LookTarget.Waypoint:
			{
				Transform waypoint = context.GetWaypoint(this.waypointName);
				if (waypoint != null)
				{
					lookAt.StartLookingAt(waypoint);
				}
				else
				{
					Debug.LogWarning("[SceneAction] LookAtTarget: Waypoint '" + this.waypointName + "' not found");
				}
				break;
			}
			case LookAtTargetAction.LookTarget.OtherNPC:
			{
				GameObject npc2 = context.GetNPC(this.otherSlotId);
				if (npc2 != null)
				{
					lookAt.StartLookingAt(npc2.transform);
				}
				else
				{
					Debug.LogWarning("[SceneAction] LookAtTarget: Other NPC slot '" + this.otherSlotId + "' not found");
				}
				break;
			}
			case LookAtTargetAction.LookTarget.Player:
				if (context.PlayerTransform != null)
				{
					lookAt.StartLookingAt(context.PlayerTransform);
				}
				break;
			case LookAtTargetAction.LookTarget.StopLooking:
				lookAt.StopLooking();
				yield break;
			}
			if (this.duration > 0f)
			{
				float elapsed = 0f;
				while (elapsed < this.duration && !context.IsInterrupted)
				{
					elapsed += NPCSimulationPause.SimulationDeltaTime;
					yield return null;
				}
				lookAt.StopLooking();
			}
			yield break;
		}

		// Token: 0x0600746B RID: 29803 RVA: 0x002F44DD File Offset: 0x002F26DD
		public override void OnInterrupt(SceneContext context, string targetSlotId)
		{
			GameObject npc = context.GetNPC(targetSlotId);
			NPCSimpleLookAt npcsimpleLookAt = ((npc != null) ? npc.GetComponent<NPCSimpleLookAt>() : null);
			if (npcsimpleLookAt == null)
			{
				return;
			}
			npcsimpleLookAt.StopLooking();
		}

		// Token: 0x0400771F RID: 30495
		[Header("Look At Settings")]
		[Tooltip("What to look at")]
		public LookAtTargetAction.LookTarget target = LookAtTargetAction.LookTarget.Player;

		// Token: 0x04007720 RID: 30496
		[Tooltip("Waypoint name (when target = Waypoint)")]
		public string waypointName;

		// Token: 0x04007721 RID: 30497
		[Tooltip("Other NPC slot ID (when target = OtherNPC)")]
		public string otherSlotId;

		// Token: 0x04007722 RID: 30498
		[Tooltip("Duration to maintain look-at (0 = indefinite, action completes immediately)")]
		public float duration;

		// Token: 0x02000F1E RID: 3870
		public enum LookTarget
		{
			// Token: 0x04007724 RID: 30500
			Waypoint,
			// Token: 0x04007725 RID: 30501
			OtherNPC,
			// Token: 0x04007726 RID: 30502
			Player,
			// Token: 0x04007727 RID: 30503
			StopLooking
		}
	}
}
