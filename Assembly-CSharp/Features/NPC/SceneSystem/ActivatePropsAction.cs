using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Features.NPC.SceneSystem
{
	// Token: 0x02000F1B RID: 3867
	[CreateAssetMenu(fileName = "ActivatePropsAction", menuName = "NPC/Scene System/Actions/Activate Props")]
	public class ActivatePropsAction : SceneActionSO
	{
		// Token: 0x06007461 RID: 29793 RVA: 0x002F42DA File Offset: 0x002F24DA
		public override IEnumerator Execute(SceneContext context, string targetSlotId)
		{
			GameObject npc = context.GetNPC(targetSlotId);
			if (npc == null)
			{
				Debug.LogWarning("[SceneAction] ActivateProps: No NPC for slot '" + targetSlotId + "'");
				yield break;
			}
			NPCEquipmentManager component = npc.GetComponent<NPCEquipmentManager>();
			if (component == null)
			{
				Debug.LogWarning("[SceneAction] ActivateProps: No NPCEquipmentManager on slot '" + targetSlotId + "'");
				yield break;
			}
			if (this.propsToActivate.Count > 0)
			{
				component.ActivateProps(this.propsToActivate);
			}
			foreach (string text in this.propsToDeactivate)
			{
				if (!string.IsNullOrEmpty(text))
				{
					component.DeactivateProp(text);
				}
			}
			yield return null;
			yield break;
		}

		// Token: 0x06007462 RID: 29794 RVA: 0x002F42F8 File Offset: 0x002F24F8
		public override void OnInterrupt(SceneContext context, string targetSlotId)
		{
			GameObject npc = context.GetNPC(targetSlotId);
			NPCEquipmentManager npcequipmentManager = ((npc != null) ? npc.GetComponent<NPCEquipmentManager>() : null);
			if (npcequipmentManager != null)
			{
				foreach (string text in this.propsToActivate)
				{
					if (!string.IsNullOrEmpty(text))
					{
						npcequipmentManager.DeactivateProp(text);
					}
				}
			}
		}

		// Token: 0x04007718 RID: 30488
		[Header("Props Settings")]
		[Tooltip("List of prop names to activate")]
		public List<string> propsToActivate = new List<string>();

		// Token: 0x04007719 RID: 30489
		[Tooltip("List of prop names to deactivate")]
		public List<string> propsToDeactivate = new List<string>();
	}
}
