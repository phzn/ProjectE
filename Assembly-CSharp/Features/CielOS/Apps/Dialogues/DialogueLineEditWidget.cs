using System;
using ProjectExhibitionist.Core;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Features.CielOS.Apps.Dialogues
{
	// Token: 0x020014A8 RID: 5288
	public class DialogueLineEditWidget : MonoBehaviour
	{
		// Token: 0x0600A2CE RID: 41678 RVA: 0x0043968C File Offset: 0x0043788C
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
			if (this.saveButton != null)
			{
				this.saveButton.onClick.AddListener(new UnityAction(this.Commit));
			}
			if (this.input != null)
			{
				this.input.lineType = TMP_InputField.LineType.MultiLineNewline;
				this.input.characterLimit = 100;
				this.input.onValueChanged.AddListener(new UnityAction<string>(this.UpdateCounter));
			}
		}

		// Token: 0x0600A2CF RID: 41679 RVA: 0x00439784 File Offset: 0x00437984
		private void UpdateCounter(string value)
		{
			if (this.counterText == null)
			{
				return;
			}
			int num = ((value != null) ? value.Length : 0);
			this.counterText.text = num.ToString() + "/" + 100.ToString();
			this.counterText.color = ((100 - num <= 15) ? DialogueLineEditWidget.CounterWarn : DialogueLineEditWidget.CounterNormal);
		}

		// Token: 0x0600A2D0 RID: 41680 RVA: 0x004397F4 File Offset: 0x004379F4
		public void Show(string title, string currentText, Action<string> onSave)
		{
			ControllerNavigationScope.Configure(base.gameObject, new Action(this.Hide), (this.input != null) ? this.input.gameObject : null, 100, false);
			this._onSave = onSave;
			DialogueTextLocalization.Bind(this.titleText, string.IsNullOrEmpty(title) ? "Line" : title);
			if (this.input != null)
			{
				this.input.SetTextWithoutNotify(currentText ?? "");
			}
			this.UpdateCounter(currentText ?? "");
			base.gameObject.SetActive(true);
			if (this.input != null)
			{
				this.input.Select();
				this.input.ActivateInputField();
			}
		}

		// Token: 0x0600A2D1 RID: 41681 RVA: 0x004398C0 File Offset: 0x00437AC0
		private void Commit()
		{
			string text = ((this.input != null && this.input.text != null) ? this.input.text.Trim() : "");
			if (string.IsNullOrEmpty(text))
			{
				DialogueTextLocalization.Toast("A line can't be empty.", "Nothing to save");
				return;
			}
			Action<string> onSave = this._onSave;
			this.Hide();
			if (onSave == null)
			{
				return;
			}
			onSave(text);
		}

		// Token: 0x0600A2D2 RID: 41682 RVA: 0x0043992F File Offset: 0x00437B2F
		public void Hide()
		{
			this._onSave = null;
			base.gameObject.SetActive(false);
		}

		// Token: 0x0400A687 RID: 42631
		[SerializeField]
		private TextMeshProUGUI titleText;

		// Token: 0x0400A688 RID: 42632
		[SerializeField]
		private TMP_InputField input;

		// Token: 0x0400A689 RID: 42633
		[SerializeField]
		private Button saveButton;

		// Token: 0x0400A68A RID: 42634
		[SerializeField]
		private Button cancelButton;

		// Token: 0x0400A68B RID: 42635
		[SerializeField]
		private Button closeButton;

		// Token: 0x0400A68C RID: 42636
		[SerializeField]
		private Button backdropButton;

		// Token: 0x0400A68D RID: 42637
		[Tooltip("Live '72/100' character counter shown under the input; turns amber near the limit.")]
		[SerializeField]
		private TextMeshProUGUI counterText;

		// Token: 0x0400A68E RID: 42638
		private const int MaxChars = 100;

		// Token: 0x0400A68F RID: 42639
		private const int WarnRemaining = 15;

		// Token: 0x0400A690 RID: 42640
		private static readonly Color CounterNormal = new Color(0.62f, 0.62f, 0.72f, 1f);

		// Token: 0x0400A691 RID: 42641
		private static readonly Color CounterWarn = new Color(1f, 0.68f, 0.28f, 1f);

		// Token: 0x0400A692 RID: 42642
		private Action<string> _onSave;
	}
}
