using System;
using UnityEngine;
using UnityEngine.Events;

namespace Bozo.AnimeCharacters
{
	// Token: 0x02000851 RID: 2129
	public class ExpressionSelect : MonoBehaviour
	{
		// Token: 0x06003870 RID: 14448 RVA: 0x00149AEC File Offset: 0x00147CEC
		private void OnEnable()
		{
			OutfitSystem outfitSystem = this.outfitSystem;
			outfitSystem.OnOutfitChanged = (UnityAction<Outfit>)Delegate.Combine(outfitSystem.OnOutfitChanged, new UnityAction<Outfit>(this.GetHead));
		}

		// Token: 0x06003871 RID: 14449 RVA: 0x00149B18 File Offset: 0x00147D18
		private void GetHead(Outfit outfit)
		{
			Outfit outfit2 = this.outfitSystem.GetOutfit("Head");
			if (outfit2)
			{
				this.animator = outfit2.GetComponentInChildren<Animator>();
			}
		}

		// Token: 0x06003872 RID: 14450 RVA: 0x00149B4A File Offset: 0x00147D4A
		private void OnDisable()
		{
			OutfitSystem outfitSystem = this.outfitSystem;
			outfitSystem.OnOutfitChanged = (UnityAction<Outfit>)Delegate.Remove(outfitSystem.OnOutfitChanged, new UnityAction<Outfit>(this.GetHead));
		}

		// Token: 0x06003873 RID: 14451 RVA: 0x00149B73 File Offset: 0x00147D73
		public void SetExpression(float value)
		{
			this.animator.SetFloat(this.parameterID, value);
		}

		// Token: 0x04003958 RID: 14680
		public OutfitSystem outfitSystem;

		// Token: 0x04003959 RID: 14681
		public Animator animator;

		// Token: 0x0400395A RID: 14682
		public string parameterID;
	}
}
