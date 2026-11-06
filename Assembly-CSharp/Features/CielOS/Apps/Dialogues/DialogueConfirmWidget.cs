using System;
using ProjectExhibitionist.Core;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Features.CielOS.Apps.Dialogues
{
	// Token: 0x02001497 RID: 5271
	public class DialogueConfirmWidget : MonoBehaviour
	{
		// Token: 0x0600A273 RID: 41587 RVA: 0x00437004 File Offset: 0x00435204
		private void Awake()
		{
			if (this.cancelButton != null)
			{
				this.cancelButton.onClick.AddListener(new UnityAction(this.Hide));
			}
			if (this.closeButton != null)
			{
				this.closeButton.onClick.AddListener(new UnityAction(this.Hide));
			}
			if (this.backdropButton != null)
			{
				this.backdropButton.onClick.AddListener(new UnityAction(this.Hide));
			}
			if (this.confirmButton != null)
			{
				this.confirmButton.onClick.AddListener(delegate
				{
					Action onConfirm = this._onConfirm;
					this.Hide();
					if (onConfirm == null)
					{
						return;
					}
					onConfirm();
				});
			}
		}

		// Token: 0x0600A274 RID: 41588 RVA: 0x004370BC File Offset: 0x004352BC
		public void Show(string title, string body, string footnote, Action onConfirm, Func<string> bodyResolver = null)
		{
			ControllerNavigationScope.Configure(base.gameObject, new Action(this.Hide), (this.cancelButton != null) ? this.cancelButton.gameObject : null, 100, false);
			this._onConfirm = onConfirm;
			base.transform.SetAsLastSibling();
			DialogueTextLocalization.Bind(this.titleText, title);
			if (this.bodyText != null)
			{
				if (bodyResolver != null)
				{
					DialogueTextLocalization.Computed(this.bodyText, bodyResolver);
				}
				else
				{
					DialogueTextLocalization.UserText(this.bodyText, body);
				}
				this.bodyText.gameObject.SetActive(!string.IsNullOrEmpty(body));
			}
			DialogueTextLocalization.Bind(this.footnoteText, footnote);
			base.gameObject.SetActive(true);
		}

		// Token: 0x0600A275 RID: 41589 RVA: 0x0043717D File Offset: 0x0043537D
		public void Hide()
		{
			this._onConfirm = null;
			base.gameObject.SetActive(false);
		}

		// Token: 0x0400A619 RID: 42521
		[SerializeField]
		private TextMeshProUGUI titleText;

		// Token: 0x0400A61A RID: 42522
		[SerializeField]
		private TextMeshProUGUI bodyText;

		// Token: 0x0400A61B RID: 42523
		[SerializeField]
		private TextMeshProUGUI footnoteText;

		// Token: 0x0400A61C RID: 42524
		[SerializeField]
		private Button cancelButton;

		// Token: 0x0400A61D RID: 42525
		[SerializeField]
		private Button confirmButton;

		// Token: 0x0400A61E RID: 42526
		[SerializeField]
		private Button closeButton;

		// Token: 0x0400A61F RID: 42527
		[SerializeField]
		private Button backdropButton;

		// Token: 0x0400A620 RID: 42528
		private Action _onConfirm;
	}
}
