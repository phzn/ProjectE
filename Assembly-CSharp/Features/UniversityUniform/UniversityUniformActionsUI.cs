using System;
using Features.Flash;
using ProjectExhibitionist.CoverWall;
using ProjectExhibitionist.Interactions;
using UnityEngine;

namespace Features.UniversityUniform
{
	// Token: 0x02000BBB RID: 3003
	public sealed class UniversityUniformActionsUI : MonoBehaviour
	{
		// Token: 0x06005615 RID: 22037 RVA: 0x002173D6 File Offset: 0x002155D6
		private void Awake()
		{
			if (this.flashController == null)
			{
				this.flashController = base.GetComponent<ArmsFlashController>();
			}
		}

		// Token: 0x06005616 RID: 22038 RVA: 0x002173F2 File Offset: 0x002155F2
		private void OnDisable()
		{
			if (this._shown != null && InteractionHUDController.Instance != null)
			{
				InteractionHUDController.Instance.HideIfCurrent(this._shown);
			}
			this._shown = null;
		}

		// Token: 0x06005617 RID: 22039 RVA: 0x00217428 File Offset: 0x00215628
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

		// Token: 0x06005618 RID: 22040 RVA: 0x002174B0 File Offset: 0x002156B0
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
				return this.liftedConfig;
			}
			if (this.flashController.IsWorn)
			{
				return this.wornConfig;
			}
			return null;
		}

		// Token: 0x04005698 RID: 22168
		[SerializeField]
		private ArmsFlashController flashController;

		// Token: 0x04005699 RID: 22169
		[SerializeField]
		private InteractionConfigSO wornConfig;

		// Token: 0x0400569A RID: 22170
		[SerializeField]
		private InteractionConfigSO liftedConfig;

		// Token: 0x0400569B RID: 22171
		private InteractionConfigSO _shown;
	}
}
