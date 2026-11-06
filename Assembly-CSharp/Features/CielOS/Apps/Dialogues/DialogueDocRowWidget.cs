using System;
using Features.Detection.Customization;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Features.CielOS.Apps.Dialogues
{
	// Token: 0x020014A6 RID: 5286
	public class DialogueDocRowWidget : MonoBehaviour
	{
		// Token: 0x0600A2C1 RID: 41665 RVA: 0x004392A8 File Offset: 0x004374A8
		private void Awake()
		{
			if (this.macroSlider != null)
			{
				this.macroSlider.minValue = 1f;
				this.macroSlider.maxValue = 100f;
				this.macroSlider.wholeNumbers = true;
				this.macroSlider.onValueChanged.AddListener(new UnityAction<float>(this.OnMacroChanged));
				(this.macroSlider.GetComponent<DialogueSliderDragNotifier>() ?? this.macroSlider.gameObject.AddComponent<DialogueSliderDragNotifier>()).Init(delegate
				{
					Action onWeightDragBegin = this._onWeightDragBegin;
					if (onWeightDragBegin == null)
					{
						return;
					}
					onWeightDragBegin();
				}, delegate
				{
					Action onWeightDragEnd = this._onWeightDragEnd;
					if (onWeightDragEnd == null)
					{
						return;
					}
					onWeightDragEnd();
				});
			}
			if (this.openButton != null)
			{
				this.openButton.onClick.AddListener(delegate
				{
					Action<string> onOpen = this._onOpen;
					if (onOpen == null)
					{
						return;
					}
					onOpen(this._id);
				});
			}
			if (this.editButton != null)
			{
				this.editButton.onClick.AddListener(delegate
				{
					Action<string> onEdit = this._onEdit;
					if (onEdit == null)
					{
						return;
					}
					onEdit(this._id);
				});
			}
			if (this.deleteButton != null)
			{
				this.deleteButton.onClick.AddListener(delegate
				{
					Action<string> onDelete = this._onDelete;
					if (onDelete == null)
					{
						return;
					}
					onDelete(this._id);
				});
			}
		}

		// Token: 0x0600A2C2 RID: 41666 RVA: 0x004393C8 File Offset: 0x004375C8
		public void Bind(DialogueDocView doc, Func<string> bindingLabel, Action<string> onOpen, Action<string, int> onMacroChanged, Action<string> onEdit, Action<string> onDelete, bool isDraft = false, Action onWeightDragBegin = null, Action onWeightDragEnd = null)
		{
			this._id = doc.id;
			this._onOpen = onOpen;
			this._onMacroChanged = onMacroChanged;
			this._onEdit = onEdit;
			this._onDelete = onDelete;
			this._onWeightDragBegin = onWeightDragBegin;
			this._onWeightDragEnd = onWeightDragEnd;
			DialogueTextLocalization.BindDocumentTitle(this.nameText, doc);
			DialogueTextLocalization.Computed(this.bindingText, bindingLabel);
			if (this.systemBadge != null)
			{
				this.systemBadge.SetActive(doc.isSystem);
			}
			if (this.customBadge != null)
			{
				this.customBadge.SetActive(!doc.isSystem);
			}
			if (this.draftBadge != null)
			{
				this.draftBadge.SetActive(isDraft);
			}
			if (this.customActions != null)
			{
				this.customActions.SetActive(!doc.isSystem);
			}
			int totalLines = doc.TotalLines;
			DialogueTextLocalization.BindFormat(this.linesText, (totalLines == 1) ? "{0} line" : "{0} lines", new object[] { totalLines });
			if (this.macroSlider != null)
			{
				this._suppress = true;
				this.macroSlider.SetValueWithoutNotify((float)Mathf.Clamp(doc.macroWeight, 1, 100));
				this._suppress = false;
			}
			if (this.percentText != null)
			{
				this.percentText.text = Mathf.Clamp(doc.macroWeight, 1, 100).ToString();
			}
			base.gameObject.SetActive(true);
		}

		// Token: 0x0600A2C3 RID: 41667 RVA: 0x0043954C File Offset: 0x0043774C
		private void OnMacroChanged(float v)
		{
			if (this._suppress)
			{
				return;
			}
			int num = Mathf.RoundToInt(v);
			if (this.percentText != null)
			{
				this.percentText.text = num.ToString();
			}
			Action<string, int> onMacroChanged = this._onMacroChanged;
			if (onMacroChanged == null)
			{
				return;
			}
			onMacroChanged(this._id, num);
		}

		// Token: 0x0400A672 RID: 42610
		[Header("Identity")]
		[SerializeField]
		private TextMeshProUGUI nameText;

		// Token: 0x0400A673 RID: 42611
		[SerializeField]
		private TextMeshProUGUI bindingText;

		// Token: 0x0400A674 RID: 42612
		[Header("Badges / counts")]
		[SerializeField]
		private GameObject systemBadge;

		// Token: 0x0400A675 RID: 42613
		[SerializeField]
		private GameObject customBadge;

		// Token: 0x0400A676 RID: 42614
		[Tooltip("DRAFT marker (custom doc not yet valid). Hover tooltip explains it won't play in game.")]
		[SerializeField]
		private GameObject draftBadge;

		// Token: 0x0400A677 RID: 42615
		[SerializeField]
		private TextMeshProUGUI linesText;

		// Token: 0x0400A678 RID: 42616
		[Header("Macro weight")]
		[SerializeField]
		private TextMeshProUGUI percentText;

		// Token: 0x0400A679 RID: 42617
		[SerializeField]
		private Slider macroSlider;

		// Token: 0x0400A67A RID: 42618
		[Header("Actions")]
		[SerializeField]
		private Button openButton;

		// Token: 0x0400A67B RID: 42619
		[SerializeField]
		private Button editButton;

		// Token: 0x0400A67C RID: 42620
		[SerializeField]
		private Button deleteButton;

		// Token: 0x0400A67D RID: 42621
		[SerializeField]
		private GameObject customActions;

		// Token: 0x0400A67E RID: 42622
		private string _id;

		// Token: 0x0400A67F RID: 42623
		private bool _suppress;

		// Token: 0x0400A680 RID: 42624
		private Action<string> _onOpen;

		// Token: 0x0400A681 RID: 42625
		private Action<string, int> _onMacroChanged;

		// Token: 0x0400A682 RID: 42626
		private Action<string> _onEdit;

		// Token: 0x0400A683 RID: 42627
		private Action<string> _onDelete;

		// Token: 0x0400A684 RID: 42628
		private Action _onWeightDragBegin;

		// Token: 0x0400A685 RID: 42629
		private Action _onWeightDragEnd;
	}
}
