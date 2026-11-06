using System;
using System.Collections;
using UnityEngine;

namespace Features.NPC.SceneSystem
{
	// Token: 0x02000F2A RID: 3882
	[CreateAssetMenu(fileName = "SetPostureAction", menuName = "NPC/Scene System/Actions/Set Posture")]
	public class SetPostureAction : SceneActionSO
	{
		// Token: 0x06007490 RID: 29840 RVA: 0x002F4E08 File Offset: 0x002F3008
		public override IEnumerator Execute(SceneContext context, string targetSlotId)
		{
			AStarNPCController controller = context.GetController(targetSlotId);
			if (controller == null)
			{
				Debug.LogWarning("[SceneAction] SetPosture: No controller for slot '" + targetSlotId + "'");
				yield break;
			}
			controller.SetPosture(this.posture);
			if (this.setAction)
			{
				controller.SetAction(this.action);
			}
			yield return null;
			yield break;
		}

		// Token: 0x04007760 RID: 30560
		[Header("Posture Settings")]
		[Tooltip("Posture to set")]
		public NPCInterestPoint.PostureType posture;

		// Token: 0x04007761 RID: 30561
		[Tooltip("Action to set (None = no change)")]
		public NPCInterestPoint.ActionType action;

		// Token: 0x04007762 RID: 30562
		[Tooltip("If true, also sets the action. If false, only changes posture.")]
		public bool setAction = true;
	}
}
