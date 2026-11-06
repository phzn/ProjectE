using System;
using Features.Localization;
using ProjectExhibitionist.Core;
using TMPro;
using UnityEngine;

namespace ProjectExhibitionist.Interactions
{
	// Token: 0x02000907 RID: 2311
	public class ActionItemUI : MonoBehaviour
	{
		// Token: 0x06003E7A RID: 15994 RVA: 0x001732B8 File Offset: 0x001714B8
		private void Awake()
		{
			if (this.keyText != null && this.keyText.GetComponent<LocalizedGameText>() == null)
			{
				ControllerKeyHint.Bind(this.keyText, this.keyText.text, null, true);
			}
			if (this.actionText != null && this.actionText.GetComponent<LocalizedGameText>() == null)
			{
				InteractionLocalization.BindAction(this.actionText, this.actionText.text);
			}
		}

		// Token: 0x06003E7B RID: 15995 RVA: 0x00173335 File Offset: 0x00171535
		public void Setup(string key, string action, InteractionConfigSO config = null)
		{
			ControllerKeyHint.Bind(this.keyText, key, config, false);
			InteractionLocalization.BindAction(this.actionText, action);
		}

		// Token: 0x04003E96 RID: 16022
		[SerializeField]
		private TextMeshProUGUI keyText;

		// Token: 0x04003E97 RID: 16023
		[SerializeField]
		private TextMeshProUGUI actionText;
	}
}
