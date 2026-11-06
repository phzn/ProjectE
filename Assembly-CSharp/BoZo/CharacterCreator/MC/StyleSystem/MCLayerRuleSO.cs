using System;
using UnityEngine;

namespace BoZo.CharacterCreator.MC.StyleSystem
{
	// Token: 0x020007DA RID: 2010
	[CreateAssetMenu(fileName = "MCLayerRule", menuName = "Character Creator/MC/Layer Rule")]
	public class MCLayerRuleSO : ScriptableObject
	{
		// Token: 0x170009B8 RID: 2488
		// (get) Token: 0x0600354A RID: 13642 RVA: 0x0012F80B File Offset: 0x0012DA0B
		public string PairKey
		{
			get
			{
				return this.occluderSlotId + ">" + this.occludedSlotId;
			}
		}

		// Token: 0x0400358E RID: 13710
		[Header("Slot coinvolti (partId)")]
		[Tooltip("partId dello slot che, se equipaggiato, applica l'effetto. Es. 'Outfit.Bottom'.")]
		public string occluderSlotId;

		// Token: 0x0400358F RID: 13711
		[Tooltip("partId dello slot bersaglio dell'effetto. Es. 'Outfit.PubicHair'.")]
		public string occludedSlotId;

		// Token: 0x04003590 RID: 13712
		[Header("Effetto")]
		[Tooltip("HideOccluded nasconde il bersaglio, ShowOccluded lo forza visibile.")]
		public MCLayerRuleSO.LayerEffect effect;

		// Token: 0x04003591 RID: 13713
		[Tooltip("Se true l'effetto si applica quando l'occluder È equipaggiato; se false quando NON lo è.")]
		public bool whenOccluderEquipped = true;

		// Token: 0x04003592 RID: 13714
		[Header("Filtro opzionale")]
		[Tooltip("Se valorizzato, la regola si applica solo quando l'occluder usa questo presetId specifico. Vuoto = qualsiasi item nello slot occluder.")]
		public string occluderPresetIdFilter;

		// Token: 0x020007DB RID: 2011
		public enum LayerEffect
		{
			// Token: 0x04003594 RID: 13716
			HideOccluded,
			// Token: 0x04003595 RID: 13717
			ShowOccluded
		}
	}
}
