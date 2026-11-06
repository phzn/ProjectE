using System;
using System.Collections.Generic;
using UnityEngine;

namespace Features.Detection
{
	// Token: 0x0200127E RID: 4734
	[CreateAssetMenu(fileName = "New Emote Collection", menuName = "Detection/Emote Wheel/Emote Collection")]
	public class EmoteCollectionSO : ScriptableObject
	{
		// Token: 0x06009309 RID: 37641 RVA: 0x003C602A File Offset: 0x003C422A
		public List<EmoteDefinitionSO> GetEmotesForCategory(ChoiceCategory category)
		{
			switch (category)
			{
			case ChoiceCategory.Shame:
				return this.ShameEmotes;
			case ChoiceCategory.Exhibit:
				return this.ExposeEmotes;
			case ChoiceCategory.Police:
				return this.PoliceEmotes;
			default:
				return this.ShameEmotes;
			}
		}

		// Token: 0x040097A6 RID: 38822
		[Header("Shame Emotes")]
		[Tooltip("Ordered list of emotes available when the player chooses Shame")]
		public List<EmoteDefinitionSO> ShameEmotes = new List<EmoteDefinitionSO>();

		// Token: 0x040097A7 RID: 38823
		[Header("Expose Emotes")]
		[Tooltip("Ordered list of emotes available when the player chooses Expose/Exhibit")]
		public List<EmoteDefinitionSO> ExposeEmotes = new List<EmoteDefinitionSO>();

		// Token: 0x040097A8 RID: 38824
		[Header("Police Emotes (cattura Polizia)")]
		[Tooltip("Ordered list of emotes shown when the player is captured by police. Mostrate dall'EmoteWheel saltando la category choice (Shame/Expose). Tipicamente: Ammanettata, Stesa a terra, ecc.")]
		public List<EmoteDefinitionSO> PoliceEmotes = new List<EmoteDefinitionSO>();
	}
}
