using System;
using Features.Flash;
using ProjectExhibitionist.CoverWall;
using ProjectExhibitionist.Interactions;
using UnityEngine;

namespace Features.RainCoat
{
	// Token: 0x02000DBC RID: 3516
	public class RainCoatActionsUI : MonoBehaviour
	{
		// Token: 0x060067AD RID: 26541 RVA: 0x002971B8 File Offset: 0x002953B8
		private void Awake()
		{
			if (this.flashController == null)
			{
				foreach (ArmsFlashController armsFlashController in base.GetComponents<ArmsFlashController>())
				{
					if (armsFlashController.Config != null && armsFlashController.Config.flashLayerName == this.flashLayerName)
					{
						this.flashController = armsFlashController;
						return;
					}
				}
			}
		}

		// Token: 0x060067AE RID: 26542 RVA: 0x00297214 File Offset: 0x00295414
		private void OnDisable()
		{
			if (this._shown != null && InteractionHUDController.Instance != null)
			{
				InteractionHUDController.Instance.HideIfCurrent(this._shown);
			}
			this._shown = null;
		}

		// Token: 0x060067AF RID: 26543 RVA: 0x0029724C File Offset: 0x0029544C
		private void LateUpdate()
		{
			InteractionHUDController instance = InteractionHUDController.Instance;
			if (instance == null)
			{
				return;
			}
			InteractionConfigSO interactionConfigSO = this.ComputeDesiredConfig();
			if (interactionConfigSO == this._shown)
			{
				if (interactionConfigSO != null && instance.CurrentConfig == null)
				{
					instance.Show(interactionConfigSO);
				}
				return;
			}
			if (interactionConfigSO == null)
			{
				if (this._shown != null)
				{
					instance.HideIfCurrent(this._shown);
				}
			}
			else
			{
				instance.Show(interactionConfigSO);
			}
			this._shown = interactionConfigSO;
		}

		// Token: 0x060067B0 RID: 26544 RVA: 0x002972D4 File Offset: 0x002954D4
		private InteractionConfigSO ComputeDesiredConfig()
		{
			if (WallCoverActivityLock.ActiveController != null)
			{
				return null;
			}
			if (this.flashController == null)
			{
				return null;
			}
			if (this.flashController.IsOpen)
			{
				return this.openConfig;
			}
			if (this.flashController.IsWorn)
			{
				return this.wornConfig;
			}
			return null;
		}

		// Token: 0x04006A2B RID: 27179
		[Header("Controllers (auto-found if empty)")]
		[SerializeField]
		private ArmsFlashController flashController;

		// Token: 0x04006A2C RID: 27180
		[Tooltip("Layer name che identifica il flash del coat fra i controller su questo GO.")]
		[SerializeField]
		private string flashLayerName = "RainCoatFlash";

		// Token: 0x04006A2D RID: 27181
		[Header("HUD Configs")]
		[Tooltip("Mostrato col coat indossato e chiuso: Open Coat / Toggle Hood.")]
		[SerializeField]
		private InteractionConfigSO wornConfig;

		// Token: 0x04006A2E RID: 27182
		[Tooltip("Mostrato mentre il coat è flashato aperto: Close Coat.")]
		[SerializeField]
		private InteractionConfigSO openConfig;

		// Token: 0x04006A2F RID: 27183
		private InteractionConfigSO _shown;
	}
}
