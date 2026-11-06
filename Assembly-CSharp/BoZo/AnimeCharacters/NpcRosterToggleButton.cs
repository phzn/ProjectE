using System;
using System.Collections;
using Features.Localization;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Bozo.AnimeCharacters
{
	// Token: 0x0200089B RID: 2203
	public sealed class NpcRosterToggleButton : Button
	{
		// Token: 0x06003B48 RID: 15176 RVA: 0x00160E70 File Offset: 0x0015F070
		internal void ConfigureHint(GameObject panel, TMP_Text label)
		{
			this.hintPanel = panel;
			this.hintLabel = label;
			this.HideHint();
		}

		// Token: 0x06003B49 RID: 15177 RVA: 0x00160E86 File Offset: 0x0015F086
		internal void SetHint(string text)
		{
			if (this.hintLabel != null)
			{
				LocalizedGameText localizedGameText = LocalizedGameText.BindSource(this.hintLabel, "cielos_apps", text, null);
				if (localizedGameText == null)
				{
					return;
				}
				localizedGameText.FitTranslationToRectangle(false);
			}
		}

		// Token: 0x06003B4A RID: 15178 RVA: 0x00160EB3 File Offset: 0x0015F0B3
		public override void OnPointerEnter(PointerEventData eventData)
		{
			base.OnPointerEnter(eventData);
			this.QueueHint();
		}

		// Token: 0x06003B4B RID: 15179 RVA: 0x00160EC2 File Offset: 0x0015F0C2
		public override void OnPointerExit(PointerEventData eventData)
		{
			base.OnPointerExit(eventData);
			this.HideHint();
		}

		// Token: 0x06003B4C RID: 15180 RVA: 0x00160ED1 File Offset: 0x0015F0D1
		public override void OnSelect(BaseEventData eventData)
		{
			base.OnSelect(eventData);
			if (!(eventData is PointerEventData))
			{
				this.QueueHint();
			}
		}

		// Token: 0x06003B4D RID: 15181 RVA: 0x00160EE8 File Offset: 0x0015F0E8
		public override void OnDeselect(BaseEventData eventData)
		{
			base.OnDeselect(eventData);
			this.HideHint();
		}

		// Token: 0x06003B4E RID: 15182 RVA: 0x00160EF7 File Offset: 0x0015F0F7
		protected override void OnDisable()
		{
			this.HideHint();
			base.OnDisable();
		}

		// Token: 0x06003B4F RID: 15183 RVA: 0x00160F05 File Offset: 0x0015F105
		private void QueueHint()
		{
			this.HideHint();
			if (base.isActiveAndEnabled && this.IsInteractable() && this.hintPanel != null)
			{
				this.pendingHint = base.StartCoroutine(this.ShowHintAfterDelay());
			}
		}

		// Token: 0x06003B50 RID: 15184 RVA: 0x00160F3D File Offset: 0x0015F13D
		private IEnumerator ShowHintAfterDelay()
		{
			yield return new WaitForSecondsRealtime(0.35f);
			this.pendingHint = null;
			if (this.hintPanel != null && base.isActiveAndEnabled)
			{
				this.hintPanel.SetActive(true);
			}
			yield break;
		}

		// Token: 0x06003B51 RID: 15185 RVA: 0x00160F4C File Offset: 0x0015F14C
		private void HideHint()
		{
			if (this.pendingHint != null)
			{
				base.StopCoroutine(this.pendingHint);
				this.pendingHint = null;
			}
			if (this.hintPanel != null)
			{
				this.hintPanel.SetActive(false);
			}
		}

		// Token: 0x04003B86 RID: 15238
		private GameObject hintPanel;

		// Token: 0x04003B87 RID: 15239
		private TMP_Text hintLabel;

		// Token: 0x04003B88 RID: 15240
		private Coroutine pendingHint;
	}
}
