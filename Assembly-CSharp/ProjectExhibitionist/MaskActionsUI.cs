using System;
using ProjectExhibitionist.CoverWall;
using ProjectExhibitionist.Interactions;
using UnityEngine;

namespace ProjectExhibitionist
{
	// Token: 0x020008E4 RID: 2276
	public class MaskActionsUI : MonoBehaviour
	{
		// Token: 0x06003D66 RID: 15718 RVA: 0x0016D358 File Offset: 0x0016B558
		private void Awake()
		{
			if (this.maskToggle == null)
			{
				this.maskToggle = base.GetComponent<MaskToggleController>();
			}
			if (this.maskToggle == null)
			{
				this.maskToggle = base.GetComponentInParent<MaskToggleController>();
			}
		}

		// Token: 0x06003D67 RID: 15719 RVA: 0x0016D38E File Offset: 0x0016B58E
		private void OnDisable()
		{
			if (this._shown != null && InteractionHUDController.Instance != null)
			{
				InteractionHUDController.Instance.HideIfCurrent(this._shown);
			}
			this._shown = null;
		}

		// Token: 0x06003D68 RID: 15720 RVA: 0x0016D3C4 File Offset: 0x0016B5C4
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

		// Token: 0x06003D69 RID: 15721 RVA: 0x0016D44C File Offset: 0x0016B64C
		private InteractionConfigSO ComputeDesiredConfig()
		{
			if (WallCoverActivityLock.ActiveController != null)
			{
				return null;
			}
			if (this.maskToggle == null || !this.maskToggle.IsMaskAvailable)
			{
				return null;
			}
			if (!this.maskToggle.IsLowered)
			{
				return this.maskUpConfig;
			}
			return this.maskDownConfig;
		}

		// Token: 0x04003D72 RID: 15730
		[Header("Controllers (auto-found if empty)")]
		[SerializeField]
		private MaskToggleController maskToggle;

		// Token: 0x04003D73 RID: 15731
		[Header("HUD Configs")]
		[Tooltip("Mostrato quando la mask è SU: Take off (R) + Lower mask (B).")]
		[SerializeField]
		private InteractionConfigSO maskUpConfig;

		// Token: 0x04003D74 RID: 15732
		[Tooltip("Mostrato quando la mask è GIU': Take off (R) + Raise mask (B).")]
		[SerializeField]
		private InteractionConfigSO maskDownConfig;

		// Token: 0x04003D75 RID: 15733
		private InteractionConfigSO _shown;
	}
}
