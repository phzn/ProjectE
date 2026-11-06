using System;
using Bozo.AnimeCharacters;
using UnityEngine;
using UnityEngine.Events;

namespace Features.NPCs
{
	// Token: 0x02000EBA RID: 3770
	public class PoliceHatTagInjector : MonoBehaviour
	{
		// Token: 0x06007100 RID: 28928 RVA: 0x002DE720 File Offset: 0x002DC920
		private void Start()
		{
			this._outfitSystem = base.GetComponentInParent<OutfitSystem>();
			if (this._outfitSystem == null)
			{
				Debug.LogWarning("[PoliceHatTagInjector] No OutfitSystem found in parent hierarchy.", this);
				return;
			}
			OutfitSystem outfitSystem = this._outfitSystem;
			outfitSystem.OnOutfitChanged = (UnityAction<Outfit>)Delegate.Combine(outfitSystem.OnOutfitChanged, new UnityAction<Outfit>(this.OnOutfitChanged));
			this._outfitSystem.AddTag("Hat");
		}

		// Token: 0x06007101 RID: 28929 RVA: 0x002DE78A File Offset: 0x002DC98A
		private void OnDestroy()
		{
			if (this._outfitSystem != null)
			{
				OutfitSystem outfitSystem = this._outfitSystem;
				outfitSystem.OnOutfitChanged = (UnityAction<Outfit>)Delegate.Remove(outfitSystem.OnOutfitChanged, new UnityAction<Outfit>(this.OnOutfitChanged));
			}
		}

		// Token: 0x06007102 RID: 28930 RVA: 0x002DE7C1 File Offset: 0x002DC9C1
		private void OnOutfitChanged(Outfit _)
		{
			this._outfitSystem.AddTag("Hat");
		}

		// Token: 0x040073C0 RID: 29632
		private OutfitSystem _outfitSystem;
	}
}
