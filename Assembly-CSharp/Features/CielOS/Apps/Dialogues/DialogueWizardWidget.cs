using System;
using Bozo.AnimeCharacters;
using Features.Detection.Customization;
using Michsky.MUIP;
using ProjectExhibitionist.Core;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Features.CielOS.Apps.Dialogues
{
	// Token: 0x020014B1 RID: 5297
	public class DialogueWizardWidget : MonoBehaviour
	{
		// Token: 0x0600A303 RID: 41731 RVA: 0x0043B836 File Offset: 0x00439A36
		private void Awake()
		{
			this.EnsureWired();
		}

		// Token: 0x0600A304 RID: 41732 RVA: 0x0043B840 File Offset: 0x00439A40
		private void EnsureWired()
		{
			if (this._wired)
			{
				return;
			}
			this._wired = true;
			if (this.backButton != null)
			{
				this.backButton.onClick.AddListener(delegate
				{
					Action onCancel = this._onCancel;
					if (onCancel == null)
					{
						return;
					}
					onCancel();
				});
			}
			if (this.createButton != null)
			{
				this.createButton.onClick.AddListener(new UnityAction(this.Commit));
			}
			if (this.nameInput != null)
			{
				this.nameInput.characterLimit = 50;
				this.nameInput.onValueChanged.AddListener(delegate(string _)
				{
					this.RefreshValidation();
				});
			}
			this.WirePillRow(this.actorPillsParent, DialogueWizardWidget.Actors.Length, delegate(int i)
			{
				this._actorIndex = i;
				this.Repaint();
			});
			this.WirePillRow(this.personalityPillsParent, DialogueWizardWidget.Personalities.Length, delegate(int i)
			{
				this._personalityIndex = i;
				this.Repaint();
			});
			this.WirePillRow(this.genderPillsParent, DialogueWizardWidget.Genders.Length, delegate(int i)
			{
				this._genderIndex = i;
				this.Repaint();
			});
		}

		// Token: 0x0600A305 RID: 41733 RVA: 0x0043B948 File Offset: 0x00439B48
		public void Open(DialogueActorGroup defaultActor, Action<DialogueActorGroup, PersonalityType, DialogueGender, string> onCreate, Action onCancel)
		{
			this.EnsureWired();
			this._onCreate = onCreate;
			this._onCancel = onCancel;
			ControllerNavigationScope.Configure(base.gameObject, delegate
			{
				Action onCancel2 = this._onCancel;
				if (onCancel2 == null)
				{
					return;
				}
				onCancel2();
			}, (this.nameInput != null) ? this.nameInput.gameObject : null, 80, false);
			int num = Array.IndexOf<DialogueActorGroup>(DialogueWizardWidget.Actors, defaultActor);
			this._actorIndex = Mathf.Max(0, num);
			this._personalityIndex = 0;
			this._genderIndex = 0;
			if (this.nameInput != null)
			{
				this.nameInput.SetTextWithoutNotify("");
			}
			base.gameObject.SetActive(true);
			this.Repaint();
		}

		// Token: 0x0600A306 RID: 41734 RVA: 0x0043B9F7 File Offset: 0x00439BF7
		public void Hide()
		{
			this._onCreate = null;
			this._onCancel = null;
			base.gameObject.SetActive(false);
		}

		// Token: 0x0600A307 RID: 41735 RVA: 0x0043BA14 File Offset: 0x00439C14
		private void Commit()
		{
			string text = ((this.nameInput != null) ? this.nameInput.text.Trim() : "");
			if (string.IsNullOrEmpty(text))
			{
				return;
			}
			Action<DialogueActorGroup, PersonalityType, DialogueGender, string> onCreate = this._onCreate;
			DialogueActorGroup dialogueActorGroup = DialogueWizardWidget.Actors[this._actorIndex];
			PersonalityType personalityType = ((dialogueActorGroup == DialogueActorGroup.Civilians) ? DialogueWizardWidget.Personalities[this._personalityIndex] : PersonalityType.Neutral);
			DialogueGender dialogueGender = DialogueWizardWidget.Genders[this._genderIndex];
			this.Hide();
			if (onCreate == null)
			{
				return;
			}
			onCreate(dialogueActorGroup, personalityType, dialogueGender, text);
		}

		// Token: 0x0600A308 RID: 41736 RVA: 0x0043BA98 File Offset: 0x00439C98
		private void WirePillRow(Transform parent, int count, Action<int> onClick)
		{
			if (parent == null)
			{
				return;
			}
			int num = Mathf.Min(count, parent.childCount);
			for (int i = 0; i < num; i++)
			{
				int captured = i;
				Transform child = parent.GetChild(i);
				ButtonManager component = child.GetComponent<ButtonManager>();
				if (component != null)
				{
					component.onClick.RemoveAllListeners();
					component.onClick.AddListener(delegate
					{
						onClick(captured);
					});
				}
				else
				{
					Button component2 = child.GetComponent<Button>();
					if (!(component2 == null))
					{
						component2.onClick.RemoveAllListeners();
						component2.onClick.AddListener(delegate
						{
							onClick(captured);
						});
					}
				}
			}
		}

		// Token: 0x0600A309 RID: 41737 RVA: 0x0043BB68 File Offset: 0x00439D68
		private void Repaint()
		{
			this.PaintPillRow(this.actorPillsParent, this._actorIndex);
			this.PaintPillRow(this.personalityPillsParent, this._personalityIndex);
			this.PaintPillRow(this.genderPillsParent, this._genderIndex);
			bool civilian = DialogueWizardWidget.Actors[this._actorIndex] == DialogueActorGroup.Civilians;
			if (this.personalitySection != null)
			{
				this.personalitySection.SetActive(civilian);
			}
			if (this.summaryText != null)
			{
				DialogueTextLocalization.Computed(this.summaryText, delegate
				{
					string text = (civilian ? (" · " + DialogueTextLocalization.Get(DialogueWizardWidget.Personalities[this._personalityIndex].ToString())) : "");
					return DialogueTextLocalization.Get(DialogueWizardWidget.ActorLabels[this._actorIndex]) + text + " · " + DialogueTextLocalization.Get(DialogueWizardWidget.GenderLabel(DialogueWizardWidget.Genders[this._genderIndex]));
				});
			}
			this.RefreshValidation();
		}

		// Token: 0x0600A30A RID: 41738 RVA: 0x0043BC18 File Offset: 0x00439E18
		private void PaintPillRow(Transform parent, int selectedIndex)
		{
			if (parent == null)
			{
				return;
			}
			for (int i = 0; i < parent.childCount; i++)
			{
				Transform child = parent.GetChild(i);
				bool flag = i == selectedIndex;
				Image component = child.GetComponent<Image>();
				if (component != null)
				{
					component.color = (flag ? this.pillSelectedColor : this.pillNormalColor);
				}
				TextMeshProUGUI componentInChildren = child.GetComponentInChildren<TextMeshProUGUI>(true);
				if (componentInChildren != null)
				{
					componentInChildren.color = (flag ? this.pillSelectedText : this.pillNormalText);
				}
			}
		}

		// Token: 0x0600A30B RID: 41739 RVA: 0x0043BC9C File Offset: 0x00439E9C
		private void RefreshValidation()
		{
			string text = ((this.nameInput != null) ? this.nameInput.text.Trim() : "");
			if (this.createButton != null)
			{
				this.createButton.interactable = !string.IsNullOrEmpty(text);
			}
			if (this.nameCounter != null)
			{
				this.nameCounter.text = text.Length.ToString() + " / " + 50.ToString();
			}
		}

		// Token: 0x0600A30C RID: 41740 RVA: 0x0043BD2C File Offset: 0x00439F2C
		internal static string GenderLabel(DialogueGender g)
		{
			if (g != DialogueGender.Any)
			{
				return g.ToString();
			}
			return "All Genders";
		}

		// Token: 0x0400A6BC RID: 42684
		public static readonly DialogueActorGroup[] Actors = new DialogueActorGroup[]
		{
			DialogueActorGroup.Civilians,
			DialogueActorGroup.Police,
			DialogueActorGroup.Journalists,
			DialogueActorGroup.Referee
		};

		// Token: 0x0400A6BD RID: 42685
		public static readonly string[] ActorLabels = new string[] { "Civilian", "Police", "Journalist", "Referee" };

		// Token: 0x0400A6BE RID: 42686
		public static readonly PersonalityType[] Personalities = new PersonalityType[]
		{
			PersonalityType.Neutral,
			PersonalityType.Shy,
			PersonalityType.Bold,
			PersonalityType.Curious,
			PersonalityType.Hostile,
			PersonalityType.Friendly
		};

		// Token: 0x0400A6BF RID: 42687
		public static readonly DialogueGender[] Genders = new DialogueGender[]
		{
			DialogueGender.Any,
			DialogueGender.Female,
			DialogueGender.Male
		};

		// Token: 0x0400A6C0 RID: 42688
		private const int MaxNameChars = 50;

		// Token: 0x0400A6C1 RID: 42689
		[Header("Header")]
		[SerializeField]
		private Button backButton;

		// Token: 0x0400A6C2 RID: 42690
		[Header("Step pills (children order = value arrays)")]
		[SerializeField]
		private Transform actorPillsParent;

		// Token: 0x0400A6C3 RID: 42691
		[Tooltip("Whole 'Personality' step card — hidden when the actor is not Civilian.")]
		[SerializeField]
		private GameObject personalitySection;

		// Token: 0x0400A6C4 RID: 42692
		[SerializeField]
		private Transform personalityPillsParent;

		// Token: 0x0400A6C5 RID: 42693
		[SerializeField]
		private Transform genderPillsParent;

		// Token: 0x0400A6C6 RID: 42694
		[Header("Name + create")]
		[SerializeField]
		private TMP_InputField nameInput;

		// Token: 0x0400A6C7 RID: 42695
		[Tooltip("Optional 'N / 50' counter under the name field.")]
		[SerializeField]
		private TextMeshProUGUI nameCounter;

		// Token: 0x0400A6C8 RID: 42696
		[Tooltip("Optional recap line above Create, e.g. 'Civilian · Shy · Female'.")]
		[SerializeField]
		private TextMeshProUGUI summaryText;

		// Token: 0x0400A6C9 RID: 42697
		[SerializeField]
		private Button createButton;

		// Token: 0x0400A6CA RID: 42698
		[Header("Pill visuals (same scheme as the list filters)")]
		[SerializeField]
		private Color pillSelectedColor = new Color(0.86f, 0.34f, 0.44f, 1f);

		// Token: 0x0400A6CB RID: 42699
		[SerializeField]
		private Color pillNormalColor = Color.white;

		// Token: 0x0400A6CC RID: 42700
		[SerializeField]
		private Color pillSelectedText = Color.white;

		// Token: 0x0400A6CD RID: 42701
		[SerializeField]
		private Color pillNormalText = new Color(0.92f, 0.9f, 0.95f, 1f);

		// Token: 0x0400A6CE RID: 42702
		private int _actorIndex;

		// Token: 0x0400A6CF RID: 42703
		private int _personalityIndex;

		// Token: 0x0400A6D0 RID: 42704
		private int _genderIndex;

		// Token: 0x0400A6D1 RID: 42705
		private Action<DialogueActorGroup, PersonalityType, DialogueGender, string> _onCreate;

		// Token: 0x0400A6D2 RID: 42706
		private Action _onCancel;

		// Token: 0x0400A6D3 RID: 42707
		private bool _wired;
	}
}
