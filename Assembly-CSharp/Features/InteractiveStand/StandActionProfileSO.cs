using System;
using System.Collections.Generic;
using Features.Detection;
using UnityEngine;

namespace Features.InteractiveStand
{
	// Token: 0x0200116E RID: 4462
	[CreateAssetMenu(fileName = "StandActionProfile", menuName = "Interactive Stand/Stand Action Profile")]
	public class StandActionProfileSO : ScriptableObject
	{
		// Token: 0x04008ACC RID: 35532
		[Header("Identity")]
		[Tooltip("Display name shown at the top of the HUD menu")]
		public string StandName = "Stand";

		// Token: 0x04008ACD RID: 35533
		[Header("Actions")]
		public List<StandActionProfileSO.StandAction> Actions = new List<StandActionProfileSO.StandAction>();

		// Token: 0x0200116F RID: 4463
		[Serializable]
		public class StandAction
		{
			// Token: 0x04008ACE RID: 35534
			[Tooltip("Text shown in the HUD menu (e.g. 'Pose', 'Wave')")]
			public string DisplayName = "Action";

			// Token: 0x04008ACF RID: 35535
			[Tooltip("Key label shown next to the action (e.g. '1', '2', '3')")]
			public string KeyLabel = "1";

			// Token: 0x04008AD0 RID: 35536
			[Tooltip("Emote played when this action is selected")]
			public EmoteDefinitionSO Emote;
		}
	}
}
