using System;
using Features.DebugTools;
using Invector.vCover;
using ProjectExhibitionist.Core;
using ProjectExhibitionist.Core.SceneManagement;
using UnityEngine;

namespace ProjectExhibitionist.CoverWall
{
	// Token: 0x020009F8 RID: 2552
	public class CoverCrouchToggle : MonoBehaviour
	{
		// Token: 0x17000C54 RID: 3156
		// (get) Token: 0x060048B7 RID: 18615 RVA: 0x001BB2B9 File Offset: 0x001B94B9
		// (set) Token: 0x060048B8 RID: 18616 RVA: 0x001BB2C0 File Offset: 0x001B94C0
		public static bool StanceInverted { get; private set; }

		// Token: 0x060048B9 RID: 18617 RVA: 0x001BB2C8 File Offset: 0x001B94C8
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetStaticState()
		{
			CoverCrouchToggle.StanceInverted = false;
		}

		// Token: 0x060048BA RID: 18618 RVA: 0x001BB2D0 File Offset: 0x001B94D0
		private void OnDisable()
		{
			this.ResetToggle();
			this.controller = null;
		}

		// Token: 0x060048BB RID: 18619 RVA: 0x001BB2E0 File Offset: 0x001B94E0
		private void Update()
		{
			if (this.controller == null)
			{
				Transform player = PlayerRegistry.Player;
				this.controller = ((player != null) ? player.GetComponentInChildren<vBasicCoverController>(true) : null);
				if (this.controller == null)
				{
					return;
				}
			}
			if (!this.controller.inCover)
			{
				this.ResetToggle();
				return;
			}
			CoverWallSetup coverWallSetup = ((this.controller.currentCoverPoint != null) ? this.controller.currentCoverPoint.GetComponentInParent<CoverWallSetup>() : null);
			if (coverWallSetup != this.activeSetup)
			{
				this.activeSetup = coverWallSetup;
				CoverCrouchToggle.StanceInverted = false;
			}
			if (FlyCamera.IsActive)
			{
				return;
			}
			if (!TextInputFocus.BlocksGameplayInput && coverWallSetup != null && coverWallSetup.allowCrouchToggle && GameplayInput.GetKeyDown(this.toggleKey))
			{
				CoverCrouchToggle.StanceInverted = !CoverCrouchToggle.StanceInverted;
			}
		}

		// Token: 0x060048BC RID: 18620 RVA: 0x001BB3B9 File Offset: 0x001B95B9
		private void ResetToggle()
		{
			CoverCrouchToggle.StanceInverted = false;
			this.activeSetup = null;
		}

		// Token: 0x0400489F RID: 18591
		public KeyCode toggleKey = KeyCode.C;

		// Token: 0x040048A0 RID: 18592
		private vBasicCoverController controller;

		// Token: 0x040048A1 RID: 18593
		private CoverWallSetup activeSetup;
	}
}
