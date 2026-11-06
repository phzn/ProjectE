using System;
using Features.Detection;
using Features.NPC;
using UnityEngine;

namespace ProjectExhibitionist.NPCs
{
	// Token: 0x0200099F RID: 2463
	public class SmartphoneFlashTrigger : MonoBehaviour
	{
		// Token: 0x060044AF RID: 17583 RVA: 0x0019B32C File Offset: 0x0019952C
		private void Awake()
		{
			this._seatedReaction = base.GetComponentInParent<SeatedRandomReaction>();
			if (this.smartphoneFlash == null)
			{
				this.smartphoneFlash = base.GetComponentInChildren<SmartphoneFlash>(true);
			}
			if (this.smartphoneFlash == null)
			{
				Debug.LogWarning("[SmartphoneFlashTrigger] No SmartphoneFlash found in children of " + base.gameObject.name);
			}
		}

		// Token: 0x060044B0 RID: 17584 RVA: 0x0019B388 File Offset: 0x00199588
		public void TriggerFlash()
		{
			if (!this.CanStartCapture())
			{
				return;
			}
			if (this.smartphoneFlash != null)
			{
				this.smartphoneFlash.TriggerFlash();
			}
		}

		// Token: 0x060044B1 RID: 17585 RVA: 0x0019B3AC File Offset: 0x001995AC
		public void TriggerFlashWithMultiplier(float multiplier)
		{
			if (!this.CanStartCapture())
			{
				return;
			}
			if (this.smartphoneFlash != null)
			{
				this.smartphoneFlash.TriggerFlashWithMultiplier(multiplier);
			}
		}

		// Token: 0x060044B2 RID: 17586 RVA: 0x0019B3D1 File Offset: 0x001995D1
		public void StopFlash()
		{
			if (this.smartphoneFlash != null)
			{
				this.smartphoneFlash.StopFlash();
			}
		}

		// Token: 0x060044B3 RID: 17587 RVA: 0x0019B3EC File Offset: 0x001995EC
		public void StartVideoLight()
		{
			if (!this.CanStartCapture())
			{
				return;
			}
			if (this.smartphoneFlash != null)
			{
				this.smartphoneFlash.StartVideoLight();
			}
		}

		// Token: 0x060044B4 RID: 17588 RVA: 0x0019B410 File Offset: 0x00199610
		public void StopVideoLight()
		{
			if (this.smartphoneFlash != null && this.smartphoneFlash.gameObject.activeInHierarchy)
			{
				this.smartphoneFlash.StopVideoLight();
			}
		}

		// Token: 0x060044B5 RID: 17589 RVA: 0x0019B440 File Offset: 0x00199640
		private bool CanStartCapture()
		{
			return this._seatedReaction == null || (this._seatedReaction.PresentationAllowsCapture && PlayerExposureState.IsExposed && (!this._seatedReaction.isActiveAndEnabled || this._seatedReaction.CanCapturePlayer()));
		}

		// Token: 0x04004457 RID: 17495
		[Tooltip("Reference to the SmartphoneFlash component. If null, will search in children.")]
		[SerializeField]
		private SmartphoneFlash smartphoneFlash;

		// Token: 0x04004458 RID: 17496
		private SeatedRandomReaction _seatedReaction;
	}
}
