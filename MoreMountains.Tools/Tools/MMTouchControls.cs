using System;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x0200018E RID: 398
	[RequireComponent(typeof(CanvasGroup))]
	[AddComponentMenu("More Mountains/Tools/Controls/MMTouchControls")]
	public class MMTouchControls : MonoBehaviour
	{
		// Token: 0x1700007E RID: 126
		// (get) Token: 0x0600096D RID: 2413 RVA: 0x0002B559 File Offset: 0x00029759
		// (set) Token: 0x0600096E RID: 2414 RVA: 0x0002B561 File Offset: 0x00029761
		public virtual bool IsMobile { get; protected set; }

		// Token: 0x0600096F RID: 2415 RVA: 0x0002B56C File Offset: 0x0002976C
		protected virtual void Start()
		{
			this._canvasGroup = base.GetComponent<CanvasGroup>();
			this._initialMobileControlsAlpha = this._canvasGroup.alpha;
			this.SetMobileControlsActive(false);
			this.IsMobile = false;
			bool autoMobileDetection = this.AutoMobileDetection;
			if (this.ForcedMode == MMTouchControls.InputForcedMode.Mobile)
			{
				this.SetMobileControlsActive(true);
				this.IsMobile = true;
			}
			if (this.ForcedMode == MMTouchControls.InputForcedMode.Desktop)
			{
				this.SetMobileControlsActive(false);
				this.IsMobile = false;
			}
		}

		// Token: 0x06000970 RID: 2416 RVA: 0x0002B5DC File Offset: 0x000297DC
		public virtual void SetMobileControlsActive(bool state)
		{
			if (this._canvasGroup != null)
			{
				this._canvasGroup.gameObject.SetActive(state);
				if (state)
				{
					this._canvasGroup.alpha = this._initialMobileControlsAlpha;
					return;
				}
				this._canvasGroup.alpha = 0f;
			}
		}

		// Token: 0x04000B09 RID: 2825
		[MMInformation("If you check Auto Mobile Detection, the engine will automatically switch to mobile controls when your build target is Android or iOS. You can also force mobile or desktop (keyboard, gamepad) controls using the dropdown below.\nNote that if you don't need mobile controls and/or GUI this component can also work on its own, just put it on an empty GameObject instead.", MMInformationAttribute.InformationType.Info, false)]
		[Tooltip("If you check Auto Mobile Detection, the engine will automatically switch to mobile controls when your build target is Android or iOS.You can also force mobile or desktop (keyboard, gamepad) controls using the dropdown below.Note that if you don't need mobile controls and/or GUI this component can also work on its own, just put it on an empty GameObject instead.")]
		public bool AutoMobileDetection = true;

		// Token: 0x04000B0A RID: 2826
		[Tooltip("Force desktop mode (gamepad, keyboard...) or mobile (touch controls)")]
		public MMTouchControls.InputForcedMode ForcedMode;

		// Token: 0x04000B0C RID: 2828
		protected CanvasGroup _canvasGroup;

		// Token: 0x04000B0D RID: 2829
		protected float _initialMobileControlsAlpha;

		// Token: 0x02000477 RID: 1143
		public enum InputForcedMode
		{
			// Token: 0x040020DF RID: 8415
			None,
			// Token: 0x040020E0 RID: 8416
			Mobile,
			// Token: 0x040020E1 RID: 8417
			Desktop
		}
	}
}
