using System;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectExhibitionist.Interactions
{
	// Token: 0x02000914 RID: 2324
	[CreateAssetMenu(fileName = "NewInteractionConfig", menuName = "Interactions/Interaction Config")]
	public class InteractionConfigSO : ScriptableObject
	{
		// Token: 0x17000A94 RID: 2708
		// (get) Token: 0x06003EE9 RID: 16105 RVA: 0x00175A0B File Offset: 0x00173C0B
		public bool HasSecondaryAction
		{
			get
			{
				return !string.IsNullOrEmpty(this.secondaryActionName) && !string.IsNullOrEmpty(this.secondaryKeyDisplayLabel);
			}
		}

		// Token: 0x04003EDF RID: 16095
		[Header("Object Info")]
		public string objectName = "Object";

		// Token: 0x04003EE0 RID: 16096
		[Tooltip("Color of the object-name label in the world prompt. White = default look; loot pickups tint it with the item's rarity color (see RaritySettingsSO).")]
		public Color objectNameColor = Color.white;

		// Token: 0x04003EE1 RID: 16097
		[Header("Main Interaction")]
		public string mainActionName = "Use";

		// Token: 0x04003EE2 RID: 16098
		public string mainKeyDisplayLabel = "E";

		// Token: 0x04003EE3 RID: 16099
		[Header("Secondary Interaction (optional)")]
		[Tooltip("Second action shown as an extra row in the world prompt (e.g. 'Hide Under' / 'F' on a car that already offers 'Climb Up' / 'E'). Leave empty to show only the main row.")]
		public string secondaryActionName = "";

		// Token: 0x04003EE4 RID: 16100
		public string secondaryKeyDisplayLabel = "";

		// Token: 0x04003EE5 RID: 16101
		[Header("Actions While Using")]
		public List<UsableAction> usableActions = new List<UsableAction>();
	}
}
