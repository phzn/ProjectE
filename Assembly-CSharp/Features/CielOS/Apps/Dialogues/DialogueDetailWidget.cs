using System;
using System.Collections.Generic;
using Bozo.AnimeCharacters;
using Features.Detection;
using Features.Detection.Customization;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Features.CielOS.Apps.Dialogues
{
	// Token: 0x02001498 RID: 5272
	public class DialogueDetailWidget : MonoBehaviour
	{
		// Token: 0x0600A278 RID: 41592 RVA: 0x004371AC File Offset: 0x004353AC
		private void Awake()
		{
			if (this.backButton != null)
			{
				this.backButton.onClick.AddListener(delegate
				{
					Action onBack = this._onBack;
					if (onBack == null)
					{
						return;
					}
					onBack();
				});
			}
			if (this.duplicateButton != null)
			{
				this.duplicateButton.onClick.AddListener(delegate
				{
					Action<string> onDuplicate = this._onDuplicate;
					if (onDuplicate == null)
					{
						return;
					}
					onDuplicate(this._doc.id);
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
					onDelete(this._doc.id);
				});
			}
			if (this.renameButton != null)
			{
				this.renameButton.onClick.AddListener(delegate
				{
					this.BeginInlineRename();
				});
			}
			if (this.saveButton != null)
			{
				this.saveButton.onClick.AddListener(delegate
				{
					Action<string> onSave = this._onSave;
					if (onSave == null)
					{
						return;
					}
					onSave(this._doc.id);
				});
			}
			if (this.addSetButton != null)
			{
				DialogueDetailWidget.EnsureButton(this.addSetButton).onClick.AddListener(new UnityAction(this.OnAddSetClicked));
			}
			if (this.titleInput != null)
			{
				this.titleInput.onEndEdit.AddListener(new UnityAction<string>(this.OnTitleEndEdit));
				this.titleInput.onSelect.AddListener(delegate(string _)
				{
				});
			}
			if (this.setTemplate != null)
			{
				this.setTemplate.SetActive(false);
			}
		}

		// Token: 0x0600A279 RID: 41593 RVA: 0x0043732D File Offset: 0x0043552D
		private void BeginInlineRename()
		{
			if (this._doc.isSystem || this.titleInput == null)
			{
				return;
			}
			this.titleInput.Select();
			this.titleInput.ActivateInputField();
		}

		// Token: 0x0600A27A RID: 41594 RVA: 0x00437364 File Offset: 0x00435564
		private void OnTitleEndEdit(string value)
		{
			if (this._doc.isSystem)
			{
				return;
			}
			string text = (string.IsNullOrWhiteSpace(value) ? "New Dialogue" : value.Trim());
			if (this.titleInput != null)
			{
				this.titleInput.SetTextWithoutNotify(text);
			}
			DialogueTextLocalization.UserText(this.titleText, text);
			Action<string, string> onRename = this._onRename;
			if (onRename == null)
			{
				return;
			}
			onRename(this._doc.id, text);
		}

		// Token: 0x0600A27B RID: 41595 RVA: 0x004373D8 File Offset: 0x004355D8
		public void Open(DialogueDocView doc, Action onBack, Action<string> onDelete, Action<string> onDuplicate, Action<string, int> onAltWeightChanged, Action<string> onAltWeightReset, Action<string, string> onRename = null, Action<string, DialogueCategory> onAddSet = null, Action<string> onDeleteSet = null, Action<string, int> onEditLine = null, Action<string, int> onDeleteLine = null, Action<string, string[]> onReorderLines = null, Action<string> onSave = null, Action<string, PersonalityType, DialogueGender, DialogueActorGroup> onSetBinding = null, DialogueCategory[] expectedPhases = null)
		{
			this._doc = doc;
			this._onBack = onBack;
			this._onDelete = onDelete;
			this._onDuplicate = onDuplicate;
			this._onAltWeightChanged = onAltWeightChanged;
			this._onAltWeightReset = onAltWeightReset;
			this._onRename = onRename;
			this._onAddSet = onAddSet;
			this._onDeleteSet = onDeleteSet;
			this._onEditLine = onEditLine;
			this._onDeleteLine = onDeleteLine;
			this._onReorderLines = onReorderLines;
			this._onSave = onSave;
			this._onSetBinding = onSetBinding;
			this._expectedPhases = expectedPhases ?? Array.Empty<DialogueCategory>();
			bool flag = !doc.isSystem;
			if (this.titleInput != null)
			{
				this.titleInput.gameObject.SetActive(flag);
				if (flag)
				{
					this.titleInput.SetTextWithoutNotify(doc.DisplayLabel);
				}
			}
			if (this.titleText != null)
			{
				this.titleText.gameObject.SetActive(!flag || !(this.titleInput != null));
				DialogueTextLocalization.BindDocumentTitle(this.titleText, doc);
			}
			this.RefreshBindingChips(doc);
			if (this.deleteButton != null)
			{
				this.deleteButton.gameObject.SetActive(!doc.isSystem);
			}
			if (this.duplicateButton != null)
			{
				this.duplicateButton.gameObject.SetActive(true);
			}
			if (this.renameButton != null)
			{
				this.renameButton.gameObject.SetActive(!doc.isSystem);
			}
			if (this.saveButton != null)
			{
				this.saveButton.gameObject.SetActive(!doc.isSystem);
			}
			if (this.addSetButton != null)
			{
				this.addSetButton.SetActive(!doc.isSystem);
			}
			this.ResolveArrows();
			this.RefreshSteps();
			if (!(this._lastDocId == doc.id) || !this.TryShowPhase(this._lastCategory))
			{
				this.ShowFirstAvailablePhase();
			}
			this.RefreshValidation();
			base.gameObject.SetActive(true);
		}

		// Token: 0x0600A27C RID: 41596 RVA: 0x004375E2 File Offset: 0x004357E2
		private void OnAddSetClicked()
		{
			if (this._doc.isSystem || this._onAddSet == null)
			{
				return;
			}
			this._onAddSet(this._doc.id, this._phaseCategory);
		}

		// Token: 0x0600A27D RID: 41597 RVA: 0x00437618 File Offset: 0x00435818
		private void RefreshBindingChips(DialogueDocView doc)
		{
			bool flag = this.bindingChipsRoot != null;
			if (this.bindingText != null)
			{
				DialogueTextLocalization.Computed(this.bindingText, () => DialogueTextLocalization.Format("Binding: {0}", new object[] { DialogueDetailWidget.BindingLabel(doc) }));
				this.bindingText.gameObject.SetActive(!flag);
			}
			if (!flag)
			{
				return;
			}
			this.bindingChipsRoot.SetActive(true);
			DialogueTextLocalization.Bind(this.actorChipLabel, DialogueDetailWidget.ActorChipLabel(doc.actorGroup));
			bool flag2 = doc.actorGroup == DialogueActorGroup.Civilians;
			if (this.personalityChip != null)
			{
				this.personalityChip.SetActive(flag2);
			}
			DialogueTextLocalization.Bind(this.personalityChipLabel, doc.personality.ToString());
			if (this.genderChipLabel != null)
			{
				DialogueTextLocalization.Bind(this.genderChipLabel, (doc.gender == DialogueGender.Any) ? "All Genders" : doc.gender.ToString());
			}
			bool flag3 = !doc.isSystem && this._onSetBinding != null;
			this.WireBindingChip(this.actorChipLabel, flag3, new Action(this.CycleActor));
			this.WireBindingChip(this.personalityChipLabel, flag3 && flag2, new Action(this.CyclePersonality));
			this.WireBindingChip(this.genderChipLabel, flag3, new Action(this.CycleGender));
		}

		// Token: 0x0600A27E RID: 41598 RVA: 0x00437798 File Offset: 0x00435998
		private void WireBindingChip(TextMeshProUGUI label, bool editable, Action onClick)
		{
			if (label == null)
			{
				return;
			}
			GameObject gameObject = ((label.transform.parent != null) ? label.transform.parent.gameObject : label.gameObject);
			Button button = gameObject.GetComponent<Button>();
			if (!editable)
			{
				if (button != null)
				{
					button.onClick.RemoveAllListeners();
					button.enabled = false;
					button.interactable = false;
				}
				Image component = gameObject.GetComponent<Image>();
				if (component != null && component.color.a <= 0.02f)
				{
					component.raycastTarget = false;
				}
				return;
			}
			Image image = gameObject.GetComponent<Image>();
			if (image == null)
			{
				image = gameObject.AddComponent<Image>();
				image.color = new Color(1f, 1f, 1f, 0.01f);
			}
			image.raycastTarget = true;
			button = gameObject.GetComponent<Button>() ?? gameObject.AddComponent<Button>();
			button.targetGraphic = image;
			button.transition = Selectable.Transition.ColorTint;
			ColorBlock colors = button.colors;
			colors.normalColor = Color.white;
			colors.highlightedColor = new Color(1f, 1f, 1f, 0.1f);
			colors.pressedColor = new Color(1f, 1f, 1f, 0.2f);
			colors.fadeDuration = 0.08f;
			button.colors = colors;
			button.enabled = true;
			button.interactable = true;
			button.onClick.RemoveAllListeners();
			button.onClick.AddListener(delegate
			{
				onClick();
			});
		}

		// Token: 0x0600A27F RID: 41599 RVA: 0x00437938 File Offset: 0x00435B38
		private void CycleActor()
		{
			DialogueActorGroup dialogueActorGroup = DialogueDetailWidget.NextIn<DialogueActorGroup>(DialogueDetailWidget.ActorDomain, this._doc.actorGroup);
			this.ApplyBinding(dialogueActorGroup, this._doc.personality, this._doc.gender);
		}

		// Token: 0x0600A280 RID: 41600 RVA: 0x00437978 File Offset: 0x00435B78
		private void CyclePersonality()
		{
			PersonalityType personalityType = DialogueDetailWidget.NextIn<PersonalityType>(DialogueDetailWidget.PersonalityDomain, this._doc.personality);
			this.ApplyBinding(this._doc.actorGroup, personalityType, this._doc.gender);
		}

		// Token: 0x0600A281 RID: 41601 RVA: 0x004379B8 File Offset: 0x00435BB8
		private void CycleGender()
		{
			DialogueGender dialogueGender = DialogueDetailWidget.NextIn<DialogueGender>(DialogueDetailWidget.GenderDomain, this._doc.gender);
			this.ApplyBinding(this._doc.actorGroup, this._doc.personality, dialogueGender);
		}

		// Token: 0x0600A282 RID: 41602 RVA: 0x004379F8 File Offset: 0x00435BF8
		private static T NextIn<T>(T[] domain, T current)
		{
			int num = Array.IndexOf<T>(domain, current);
			return domain[(num + 1) % domain.Length];
		}

		// Token: 0x0600A283 RID: 41603 RVA: 0x00437A1A File Offset: 0x00435C1A
		private void ApplyBinding(DialogueActorGroup actor, PersonalityType personality, DialogueGender gender)
		{
			if (actor != DialogueActorGroup.Civilians)
			{
				personality = PersonalityType.Neutral;
			}
			Action<string, PersonalityType, DialogueGender, DialogueActorGroup> onSetBinding = this._onSetBinding;
			if (onSetBinding == null)
			{
				return;
			}
			onSetBinding(this._doc.id, personality, gender, actor);
		}

		// Token: 0x0600A284 RID: 41604 RVA: 0x00437A40 File Offset: 0x00435C40
		private static string ActorChipLabel(DialogueActorGroup g)
		{
			switch (g)
			{
			case DialogueActorGroup.Civilians:
				return "Civilian";
			case DialogueActorGroup.Police:
				return "Police";
			case DialogueActorGroup.Journalists:
				return "Journalist";
			case DialogueActorGroup.Referee:
				return "Referee";
			}
			return g.ToString();
		}

		// Token: 0x0600A285 RID: 41605 RVA: 0x00437A90 File Offset: 0x00435C90
		private void RefreshValidation()
		{
			if (this._doc.isSystem)
			{
				if (this.validationHint != null)
				{
					this.validationHint.gameObject.SetActive(false);
				}
				return;
			}
			List<DialogueCategory> missing = DialogueLibraryService.GetMissingRequiredPhases(this._doc.id);
			bool flag = missing.Count == 0;
			if (this.saveButton != null)
			{
				this.saveButton.interactable = flag;
			}
			if (this.validationHint != null)
			{
				this.validationHint.gameObject.SetActive(!flag);
				if (!flag)
				{
					DialogueTextLocalization.Computed(this.validationHint, delegate
					{
						List<string> list = new List<string>();
						foreach (DialogueCategory dialogueCategory in missing)
						{
							list.Add(DialogueTextLocalization.Get(DialogueDetailWidget.PhaseLabel(dialogueCategory)));
						}
						return DialogueTextLocalization.Format("Add a set with at least one line in: {0}", new object[] { string.Join(", ", list) });
					});
				}
			}
		}

		// Token: 0x0600A286 RID: 41606 RVA: 0x00437B4C File Offset: 0x00435D4C
		private void RefreshSteps()
		{
			if (this.stepsParent == null)
			{
				return;
			}
			for (int i = 0; i < DialogueDetailWidget.Steps.Length; i++)
			{
				DialogueDetailWidget.StepDef stepDef = DialogueDetailWidget.Steps[i];
				GameObject gameObject;
				if ((gameObject = this._stepGos[i]) == null)
				{
					gameObject = (this._stepGos[i] = this.FindStep(stepDef));
				}
				GameObject gameObject2 = gameObject;
				if (!(gameObject2 == null))
				{
					GameObject gameObject3 = ((stepDef.wrapperName != null) ? DialogueDetailWidget.ResolveWrapper(gameObject2, stepDef.wrapperName) : gameObject2);
					DialoguePhaseView dialoguePhaseView;
					int num = this.PhaseSetCount(stepDef.category, out dialoguePhaseView);
					if (!this.IsStepVisible(stepDef.category, num))
					{
						if (gameObject3 != null)
						{
							gameObject3.SetActive(false);
						}
					}
					else
					{
						if (gameObject3 != null)
						{
							gameObject3.SetActive(true);
						}
						DialogueTextLocalization.Bind(DialogueDetailWidget.FindChildTMP(gameObject2.transform, "Label"), DialogueDetailWidget.PhaseLabel(stepDef.category));
						int num2 = Mathf.Max(0, num);
						DialogueTextLocalization.BindFormat(DialogueDetailWidget.FindChildTMP(gameObject2.transform, "Count"), (num2 == 1) ? "{0} set" : "{0} sets", new object[] { num2 });
						int captured = i;
						Button component = gameObject2.GetComponent<Button>();
						if (component != null)
						{
							component.onClick.RemoveAllListeners();
							component.onClick.AddListener(delegate
							{
								this.ShowStep(captured);
							});
						}
					}
				}
			}
		}

		// Token: 0x0600A287 RID: 41607 RVA: 0x00437CC8 File Offset: 0x00435EC8
		private bool IsStepVisible(DialogueCategory category, int setCount)
		{
			return setCount >= 0 || (!this._doc.isSystem && Array.IndexOf<DialogueCategory>(this._expectedPhases, category) >= 0);
		}

		// Token: 0x0600A288 RID: 41608 RVA: 0x00437CF4 File Offset: 0x00435EF4
		private bool TryShowPhase(DialogueCategory category)
		{
			int i = 0;
			while (i < DialogueDetailWidget.Steps.Length)
			{
				if (DialogueDetailWidget.Steps[i].category == category && !(this._stepGos[i] == null))
				{
					DialoguePhaseView dialoguePhaseView;
					int num = this.PhaseSetCount(category, out dialoguePhaseView);
					if (!this.IsStepVisible(category, num))
					{
						return false;
					}
					this.ShowStep(i);
					return true;
				}
				else
				{
					i++;
				}
			}
			return false;
		}

		// Token: 0x0600A289 RID: 41609 RVA: 0x00437D58 File Offset: 0x00435F58
		private void ShowFirstAvailablePhase()
		{
			DialoguePhaseView dialoguePhaseView;
			for (int i = 0; i < DialogueDetailWidget.Steps.Length; i++)
			{
				if (!(this._stepGos[i] == null))
				{
					int num = this.PhaseSetCount(DialogueDetailWidget.Steps[i].category, out dialoguePhaseView);
					if (this.IsStepVisible(DialogueDetailWidget.Steps[i].category, num))
					{
						this.ShowStep(i);
						return;
					}
				}
			}
			this._hasPhase = false;
			this._phaseCategory = DialogueCategory.Detection;
			dialoguePhaseView = default(DialoguePhaseView);
			this.RenderSets(dialoguePhaseView, false);
			this.PaintSteps(-1);
			this.UpdatePhaseCaption(null);
		}

		// Token: 0x0600A28A RID: 41610 RVA: 0x00437DF8 File Offset: 0x00435FF8
		private void ShowStep(int stepIndex)
		{
			DialogueDetailWidget.StepDef stepDef = DialogueDetailWidget.Steps[stepIndex];
			this._phaseCategory = stepDef.category;
			this._lastDocId = this._doc.id;
			this._lastCategory = stepDef.category;
			DialoguePhaseView dialoguePhaseView;
			int num = this.PhaseSetCount(stepDef.category, out dialoguePhaseView);
			this._hasPhase = num >= 0 || this.IsStepVisible(stepDef.category, num);
			this.PaintSteps(stepIndex);
			this.RenderSets(dialoguePhaseView, num >= 0);
			this.UpdatePhaseCaption(this._hasPhase ? new DialogueCategory?(stepDef.category) : null);
		}

		// Token: 0x0600A28B RID: 41611 RVA: 0x00437E9C File Offset: 0x0043609C
		private int PhaseSetCount(DialogueCategory category, out DialoguePhaseView phase)
		{
			int i = 0;
			while (i < this._doc.phases.Length)
			{
				if (this._doc.phases[i].category == category)
				{
					phase = this._doc.phases[i];
					if (phase.alternatives == null)
					{
						return 0;
					}
					return phase.alternatives.Length;
				}
				else
				{
					i++;
				}
			}
			phase = default(DialoguePhaseView);
			return -1;
		}

		// Token: 0x0600A28C RID: 41612 RVA: 0x00437F0C File Offset: 0x0043610C
		private void UpdatePhaseCaption(DialogueCategory? category)
		{
			if (this.editingPhaseTitle != null)
			{
				DialogueTextLocalization.Computed(this.editingPhaseTitle, delegate
				{
					if (category == null)
					{
						return "";
					}
					return DialogueTextLocalization.Format("Editing Phase: {0}", new object[] { DialogueTextLocalization.Get(DialogueDetailWidget.PhaseLabel(category.Value)) });
				});
			}
			if (this.editingPhaseSubtitle != null)
			{
				DialogueTextLocalization.Bind(this.editingPhaseSubtitle, (category != null) ? DialogueDetailWidget.PhaseBlurb(category.Value) : "");
			}
		}

		// Token: 0x0600A28D RID: 41613 RVA: 0x00437F88 File Offset: 0x00436188
		private void PaintSteps(int selected)
		{
			for (int i = 0; i < DialogueDetailWidget.Steps.Length; i++)
			{
				GameObject gameObject = this._stepGos[i];
				if (!(gameObject == null))
				{
					CanvasGroup canvasGroup;
					if ((canvasGroup = this._stepGroups[i]) == null)
					{
						canvasGroup = (this._stepGroups[i] = DialogueDetailWidget.EnsureCanvasGroup(gameObject));
					}
					CanvasGroup canvasGroup2 = canvasGroup;
					if (canvasGroup2 != null)
					{
						canvasGroup2.alpha = ((i == selected) ? 1f : 0.8f);
					}
					Transform transform = gameObject.transform.Find("Underline");
					if (transform != null)
					{
						transform.gameObject.SetActive(i == selected);
					}
				}
			}
			this.PaintArrows((selected >= 0 && selected < DialogueDetailWidget.Steps.Length) ? new DialogueCategory?(DialogueDetailWidget.Steps[selected].category) : null);
		}

		// Token: 0x0600A28E RID: 41614 RVA: 0x0043805C File Offset: 0x0043625C
		private void PaintArrows(DialogueCategory? selectedCategory)
		{
			for (int i = 0; i < DialogueDetailWidget.Arrows.Length; i++)
			{
				CanvasGroup canvasGroup = this._arrowGroups[i];
				if (!(canvasGroup == null))
				{
					canvasGroup.alpha = ((selectedCategory != null && DialogueDetailWidget.Arrows[i].entersCategory == selectedCategory.Value) ? 1f : 0.8f);
				}
			}
			if (this._exitArrowGroup != null)
			{
				CanvasGroup exitArrowGroup = this._exitArrowGroup;
				DialogueCategory? dialogueCategory = selectedCategory;
				DialogueCategory dialogueCategory2 = DialogueCategory.Exit;
				exitArrowGroup.alpha = (((dialogueCategory.GetValueOrDefault() == dialogueCategory2) & (dialogueCategory != null)) ? 1f : 0.8f);
			}
		}

		// Token: 0x0600A28F RID: 41615 RVA: 0x00438100 File Offset: 0x00436300
		private void ResolveArrows()
		{
			if (this._arrowsResolved || this.stepsParent == null)
			{
				return;
			}
			this._arrowsResolved = true;
			Transform transform = this.stepsParent.Find("Arrow");
			if (transform != null)
			{
				this._exitArrowGroup = DialogueDetailWidget.EnsureCanvasGroup(transform.gameObject);
			}
			for (int i = 0; i < DialogueDetailWidget.Arrows.Length; i++)
			{
				Transform transform3;
				if (DialogueDetailWidget.Arrows[i].goName == "Arrow")
				{
					Transform transform2 = DialogueDetailWidget.FindDeep(this.stepsParent, "OptionalChaseStep");
					transform3 = ((transform2 != null) ? DialogueDetailWidget.FindDeep(transform2, "Arrow") : null);
				}
				else
				{
					transform3 = DialogueDetailWidget.FindDeep(this.stepsParent, DialogueDetailWidget.Arrows[i].goName);
				}
				if (transform3 != null)
				{
					this._arrowGroups[i] = DialogueDetailWidget.EnsureCanvasGroup(transform3.gameObject);
				}
			}
		}

		// Token: 0x0600A290 RID: 41616 RVA: 0x004381F0 File Offset: 0x004363F0
		private static CanvasGroup EnsureCanvasGroup(GameObject go)
		{
			CanvasGroup component = go.GetComponent<CanvasGroup>();
			if (!(component != null))
			{
				return go.AddComponent<CanvasGroup>();
			}
			return component;
		}

		// Token: 0x0600A291 RID: 41617 RVA: 0x00438218 File Offset: 0x00436418
		private static Button EnsureButton(GameObject go)
		{
			Button button = go.GetComponent<Button>();
			if (button == null)
			{
				button = go.AddComponent<Button>();
				Image component = go.GetComponent<Image>();
				if (component != null)
				{
					button.targetGraphic = component;
				}
			}
			return button;
		}

		// Token: 0x0600A292 RID: 41618 RVA: 0x00438254 File Offset: 0x00436454
		private void RenderSets(DialoguePhaseView phase, bool hasPhase)
		{
			int num = ((hasPhase && phase.alternatives != null) ? phase.alternatives.Length : 0);
			if (this.emptyState != null)
			{
				this.emptyState.SetActive(hasPhase && num == 0);
			}
			if (this.setsParent == null || this.setTemplate == null)
			{
				return;
			}
			DialogueDetailWidget.EnsurePool(this._setPool, this.setTemplate, this.setsParent, num);
			for (int i = 0; i < this._setPool.Count; i++)
			{
				GameObject gameObject = this._setPool[i];
				if (hasPhase && i < num)
				{
					this.BindSet(gameObject, phase.alternatives[i], i);
					gameObject.SetActive(true);
				}
				else
				{
					gameObject.SetActive(false);
				}
			}
			if (this.addSetButton != null)
			{
				this.addSetButton.transform.SetAsLastSibling();
			}
		}

		// Token: 0x0600A293 RID: 41619 RVA: 0x0043833C File Offset: 0x0043653C
		private void BindSet(GameObject go, DialogueAltView alt, int index)
		{
			DialogueTextLocalization.BindFormat(DialogueDetailWidget.FindDeepTMP(go.transform, "Title"), "Set {0}", new object[] { index + 1 });
			Transform transform = DialogueDetailWidget.FindDeep(go.transform, "TypeBadge");
			if (transform != null)
			{
				Transform transform2 = DialogueDetailWidget.FindDeep(transform, "Label");
				TextMeshProUGUI textMeshProUGUI = ((transform2 != null) ? transform2.GetComponent<TextMeshProUGUI>() : null) ?? transform.GetComponentInChildren<TextMeshProUGUI>(true);
				DialogueTextLocalization.Bind(textMeshProUGUI, alt.isSystem ? "SYSTEM" : "CUSTOM");
				Image component = transform.GetComponent<Image>();
				if (component != null)
				{
					component.color = (alt.isSystem ? new Color(0.42f, 0.52f, 0.86f, 0.3f) : new Color(0.85f, 0.33f, 0.55f, 0.3f));
				}
				if (textMeshProUGUI != null)
				{
					textMeshProUGUI.color = (alt.isSystem ? new Color(0.62f, 0.72f, 1f, 1f) : new Color(1f, 0.62f, 0.78f, 1f));
				}
			}
			Transform transform3 = DialogueDetailWidget.FindDeep(go.transform, "ResetWeight");
			if (transform3 != null)
			{
				transform3.gameObject.SetActive(alt.isSystem);
				Button component2 = transform3.GetComponent<Button>();
				if (component2 != null)
				{
					component2.onClick.RemoveAllListeners();
					string sid = alt.id;
					component2.onClick.AddListener(delegate
					{
						Action<string> onAltWeightReset = this._onAltWeightReset;
						if (onAltWeightReset == null)
						{
							return;
						}
						onAltWeightReset(sid);
					});
				}
			}
			string setIdForActions = alt.id;
			Transform transform4 = DialogueDetailWidget.FindDeep(go.transform, "Edit");
			if (transform4 != null)
			{
				transform4.gameObject.SetActive(false);
			}
			Transform transform5 = DialogueDetailWidget.FindDeep(go.transform, "Delete");
			if (transform5 != null)
			{
				transform5.gameObject.SetActive(!alt.isSystem);
				Button component3 = transform5.GetComponent<Button>();
				if (component3 != null)
				{
					component3.onClick.RemoveAllListeners();
					component3.onClick.AddListener(delegate
					{
						Action<string> onDeleteSet = this._onDeleteSet;
						if (onDeleteSet == null)
						{
							return;
						}
						onDeleteSet(setIdForActions);
					});
				}
			}
			TextMeshProUGUI textMeshProUGUI2 = DialogueDetailWidget.FindDeepTMP(go.transform, "Percent");
			Transform transform6 = DialogueDetailWidget.FindDeep(go.transform, "Weight");
			Slider slider = ((transform6 != null) ? transform6.GetComponent<Slider>() : null);
			if (slider != null)
			{
				slider.minValue = 1f;
				slider.maxValue = 100f;
				slider.wholeNumbers = true;
				slider.SetValueWithoutNotify((float)Mathf.Clamp(alt.effectiveWeight, 1, 100));
				slider.onValueChanged.RemoveAllListeners();
				string setId = alt.id;
				TextMeshProUGUI pctLocal = textMeshProUGUI2;
				slider.onValueChanged.AddListener(delegate(float v)
				{
					int num = Mathf.RoundToInt(v);
					if (pctLocal != null)
					{
						pctLocal.text = num.ToString();
					}
					Action<string, int> onAltWeightChanged = this._onAltWeightChanged;
					if (onAltWeightChanged == null)
					{
						return;
					}
					onAltWeightChanged(setId, num);
				});
			}
			if (textMeshProUGUI2 != null)
			{
				textMeshProUGUI2.text = Mathf.Clamp(alt.effectiveWeight, 1, 100).ToString();
			}
			this.RenderLines(go, alt);
		}

		// Token: 0x0600A294 RID: 41620 RVA: 0x00438684 File Offset: 0x00436884
		private void RenderLines(GameObject go, DialogueAltView alt)
		{
			Transform transform = DialogueDetailWidget.FindDeep(go.transform, "Lines");
			RectTransform linesParent = DialogueDetailWidget.FindDeep(go.transform, "LinesContainer") as RectTransform;
			Transform transform2 = DialogueDetailWidget.FindDeep(go.transform, "LineRow");
			GameObject lineTemplate = ((transform2 != null) ? transform2.gameObject : null);
			if (linesParent == null || lineTemplate == null)
			{
				return;
			}
			lineTemplate.SetActive(false);
			for (int i = linesParent.childCount - 1; i >= 0; i--)
			{
				Transform child = linesParent.GetChild(i);
				if (!(child.gameObject == lineTemplate))
				{
					Object.Destroy(child.gameObject);
				}
			}
			int num = ((alt.lines != null) ? alt.lines.Length : 0);
			bool flag = !this._doc.isSystem;
			string setId = alt.id;
			Transform transform3 = ((transform != null) ? DialogueDetailWidget.FindDeep(transform, "LinesEmpty") : null);
			if (transform3 != null)
			{
				transform3.gameObject.SetActive(num == 0);
				Button component = transform3.GetComponent<Button>();
				if (component != null)
				{
					component.onClick.RemoveAllListeners();
					component.interactable = false;
					component.enabled = false;
				}
			}
			Transform transform4 = ((transform != null) ? DialogueDetailWidget.FindDeep(transform, "LineHeader") : null);
			if (transform4 != null)
			{
				transform4.gameObject.SetActive(num > 0);
			}
			Transform transform5 = ((transform != null) ? DialogueDetailWidget.FindDeep(transform, "AddLine") : null);
			if (transform5 != null)
			{
				transform5.gameObject.SetActive(flag);
				if (flag)
				{
					Button button = DialogueDetailWidget.EnsureButton(transform5.gameObject);
					button.onClick.RemoveAllListeners();
					button.onClick.AddListener(delegate
					{
						Action<string, int> onEditLine = this._onEditLine;
						if (onEditLine == null)
						{
							return;
						}
						onEditLine(setId, -1);
					});
				}
			}
			for (int j = 0; j < num; j++)
			{
				GameObject gameObject = Object.Instantiate<GameObject>(lineTemplate, linesParent);
				gameObject.SetActive(true);
				Transform transform6 = gameObject.transform.Find("Number");
				TextMeshProUGUI textMeshProUGUI = ((transform6 != null) ? transform6.GetComponent<TextMeshProUGUI>() : null);
				Transform transform7 = gameObject.transform.Find("Text");
				TMP_Text tmp_Text = ((transform7 != null) ? transform7.GetComponent<TextMeshProUGUI>() : null);
				if (textMeshProUGUI != null)
				{
					textMeshProUGUI.text = (j + 1).ToString();
				}
				DialogueTextLocalization.BindLine(tmp_Text, (alt.lines[j] != null) ? alt.lines[j].text : null, alt.isSystem);
				Transform transform8 = gameObject.transform.Find("Handle");
				if (transform8 != null)
				{
					transform8.gameObject.SetActive(flag);
				}
				Transform transform9 = gameObject.transform.Find("EditLine");
				if (transform9 != null)
				{
					transform9.gameObject.SetActive(flag);
				}
				Transform transform10 = gameObject.transform.Find("DeleteLine");
				if (transform10 != null)
				{
					transform10.gameObject.SetActive(flag);
				}
				if (transform8 != null)
				{
					Image component2 = transform8.GetComponent<Image>();
					if (component2 != null)
					{
						component2.raycastTarget = true;
					}
				}
				if (flag)
				{
					int idx = j;
					Button component3 = gameObject.GetComponent<Button>();
					if (component3 != null)
					{
						component3.onClick.RemoveAllListeners();
						component3.enabled = false;
					}
					if (transform9 != null)
					{
						Button button2 = DialogueDetailWidget.EnsureButton(transform9.gameObject);
						button2.onClick.RemoveAllListeners();
						button2.onClick.AddListener(delegate
						{
							Action<string, int> onEditLine2 = this._onEditLine;
							if (onEditLine2 == null)
							{
								return;
							}
							onEditLine2(setId, idx);
						});
					}
					if (transform10 != null)
					{
						Button button3 = DialogueDetailWidget.EnsureButton(transform10.gameObject);
						button3.onClick.RemoveAllListeners();
						button3.onClick.AddListener(delegate
						{
							Action<string, int> onDeleteLine = this._onDeleteLine;
							if (onDeleteLine == null)
							{
								return;
							}
							onDeleteLine(setId, idx);
						});
					}
					DialogueLine[] sourceLines = alt.lines;
					RectTransform rectTransform = (RectTransform)gameObject.transform;
					GameObject gameObject2 = ((transform8 != null) ? transform8.gameObject : gameObject);
					DialogueLineRowDrag component4 = gameObject.GetComponent<DialogueLineRowDrag>();
					if (component4 != null)
					{
						Object.Destroy(component4);
					}
					(gameObject2.GetComponent<DialogueLineRowDrag>() ?? gameObject2.AddComponent<DialogueLineRowDrag>()).Init(idx, rectTransform, delegate
					{
						this.CommitLineOrder(setId, linesParent, lineTemplate, sourceLines);
					});
				}
			}
		}

		// Token: 0x0600A295 RID: 41621 RVA: 0x00438B18 File Offset: 0x00436D18
		private void CommitLineOrder(string setId, RectTransform linesParent, GameObject lineTemplate, DialogueLine[] sourceLines)
		{
			int num = ((sourceLines != null) ? sourceLines.Length : 0);
			List<string> list = new List<string>();
			for (int i = 0; i < linesParent.childCount; i++)
			{
				Transform child = linesParent.GetChild(i);
				if (!(child.gameObject == lineTemplate) && child.gameObject.activeSelf)
				{
					DialogueLineRowDrag component = child.GetComponent<DialogueLineRowDrag>();
					int num2 = ((component != null) ? component.OriginalIndex : (-1));
					if (num2 >= 0 && num2 < num)
					{
						list.Add((sourceLines[num2] != null) ? sourceLines[num2].text : "");
					}
					else
					{
						Transform transform = child.Find("Text");
						TextMeshProUGUI textMeshProUGUI = ((transform != null) ? transform.GetComponent<TextMeshProUGUI>() : null);
						list.Add((textMeshProUGUI != null) ? textMeshProUGUI.text : "");
					}
				}
			}
			Action<string, string[]> onReorderLines = this._onReorderLines;
			if (onReorderLines == null)
			{
				return;
			}
			onReorderLines(setId, list.ToArray());
		}

		// Token: 0x0600A296 RID: 41622 RVA: 0x00438C0C File Offset: 0x00436E0C
		private GameObject FindStep(DialogueDetailWidget.StepDef def)
		{
			Transform transform = DialogueDetailWidget.FindDeep(this.stepsParent, def.goName);
			if (!(transform != null))
			{
				return null;
			}
			return transform.gameObject;
		}

		// Token: 0x0600A297 RID: 41623 RVA: 0x00438C3C File Offset: 0x00436E3C
		private static GameObject ResolveWrapper(GameObject step, string wrapperName)
		{
			Transform transform = step.transform;
			while (transform != null)
			{
				if (transform.name == wrapperName)
				{
					return transform.gameObject;
				}
				transform = transform.parent;
			}
			return step;
		}

		// Token: 0x0600A298 RID: 41624 RVA: 0x00438C78 File Offset: 0x00436E78
		private static void EnsurePool(List<GameObject> pool, GameObject template, RectTransform parent, int needed)
		{
			while (pool.Count < needed)
			{
				GameObject gameObject = Object.Instantiate<GameObject>(template, parent);
				pool.Add(gameObject);
			}
		}

		// Token: 0x0600A299 RID: 41625 RVA: 0x00438CA0 File Offset: 0x00436EA0
		private static TextMeshProUGUI FindChildTMP(Transform root, string name)
		{
			Transform transform = root.Find(name);
			if (!(transform != null))
			{
				return null;
			}
			return transform.GetComponent<TextMeshProUGUI>();
		}

		// Token: 0x0600A29A RID: 41626 RVA: 0x00438CC8 File Offset: 0x00436EC8
		private static TextMeshProUGUI FindDeepTMP(Transform root, string name)
		{
			Transform transform = DialogueDetailWidget.FindDeep(root, name);
			if (!(transform != null))
			{
				return null;
			}
			return transform.GetComponent<TextMeshProUGUI>();
		}

		// Token: 0x0600A29B RID: 41627 RVA: 0x00438CF0 File Offset: 0x00436EF0
		private static Transform FindDeep(Transform root, string name)
		{
			if (root == null)
			{
				return null;
			}
			for (int i = 0; i < root.childCount; i++)
			{
				Transform child = root.GetChild(i);
				if (child.name == name)
				{
					return child;
				}
				Transform transform = DialogueDetailWidget.FindDeep(child, name);
				if (transform != null)
				{
					return transform;
				}
			}
			return null;
		}

		// Token: 0x0600A29C RID: 41628 RVA: 0x00438D48 File Offset: 0x00436F48
		private static string BindingLabel(DialogueDocView doc)
		{
			string text = DialogueTextLocalization.Get((doc.gender == DialogueGender.Any) ? "All Genders" : doc.gender.ToString());
			if (doc.actorGroup != DialogueActorGroup.Civilians)
			{
				return text;
			}
			return DialogueTextLocalization.Get(doc.personality.ToString()) + " · " + text;
		}

		// Token: 0x0600A29D RID: 41629 RVA: 0x00438DA8 File Offset: 0x00436FA8
		private static string PhaseLabel(DialogueCategory c)
		{
			switch (c)
			{
			case DialogueCategory.Detection:
				return "Pre";
			case DialogueCategory.Reaction:
				return "Reaction";
			case DialogueCategory.Shame:
				return "Shame";
			case DialogueCategory.Exhibit:
				return "Expose";
			case DialogueCategory.Exit:
				return "Exit";
			case DialogueCategory.Chasing:
				return "Chase";
			default:
				return c.ToString();
			}
		}

		// Token: 0x0600A29E RID: 41630 RVA: 0x00438E08 File Offset: 0x00437008
		private static string PhaseBlurb(DialogueCategory c)
		{
			switch (c)
			{
			case DialogueCategory.Detection:
				return "Said before the encounter, while spotting you.";
			case DialogueCategory.Reaction:
				return "The NPC's first response when they notice you.";
			case DialogueCategory.Shame:
				return "Shown if the player chooses to be ashamed.";
			case DialogueCategory.Exhibit:
				return "Shown if the player chooses to Expose.";
			case DialogueCategory.Exit:
				return "Closes the conversation as they walk away.";
			case DialogueCategory.Chasing:
				return "Spoken while chasing you, before catching you.";
			default:
				return "";
			}
		}

		// Token: 0x0400A621 RID: 42529
		private static readonly DialogueDetailWidget.StepDef[] Steps = new DialogueDetailWidget.StepDef[]
		{
			new DialogueDetailWidget.StepDef("ChaseStep", DialogueCategory.Chasing, "OptionalChaseStep"),
			new DialogueDetailWidget.StepDef("ReactionStep", DialogueCategory.Reaction, null),
			new DialogueDetailWidget.StepDef("ExposeStep", DialogueCategory.Exhibit, null),
			new DialogueDetailWidget.StepDef("ExposeStep (1)", DialogueCategory.Shame, null),
			new DialogueDetailWidget.StepDef("ExitStep", DialogueCategory.Exit, null)
		};

		// Token: 0x0400A622 RID: 42530
		private static readonly DialogueDetailWidget.ArrowDef[] Arrows = new DialogueDetailWidget.ArrowDef[]
		{
			new DialogueDetailWidget.ArrowDef("Arrow", DialogueCategory.Reaction),
			new DialogueDetailWidget.ArrowDef("FeelStrikeArrow (1)", DialogueCategory.Exhibit),
			new DialogueDetailWidget.ArrowDef("FeelStrikeArrow (3)", DialogueCategory.Shame)
		};

		// Token: 0x0400A623 RID: 42531
		private const string ExitArrowName = "Arrow";

		// Token: 0x0400A624 RID: 42532
		private const float AlphaSelected = 1f;

		// Token: 0x0400A625 RID: 42533
		private const float AlphaDimmed = 0.8f;

		// Token: 0x0400A626 RID: 42534
		[Header("Header")]
		[SerializeField]
		private TextMeshProUGUI titleText;

		// Token: 0x0400A627 RID: 42535
		[Tooltip("Optional inline title editor (custom docs): overlays titleText, commits via onRename on end-edit.")]
		[SerializeField]
		private TMP_InputField titleInput;

		// Token: 0x0400A628 RID: 42536
		[SerializeField]
		private TextMeshProUGUI bindingText;

		// Token: 0x0400A629 RID: 42537
		[SerializeField]
		private Button backButton;

		// Token: 0x0400A62A RID: 42538
		[SerializeField]
		private Button duplicateButton;

		// Token: 0x0400A62B RID: 42539
		[SerializeField]
		private Button deleteButton;

		// Token: 0x0400A62C RID: 42540
		[Tooltip("Optional 'Rename' button (custom docs only) — opens a rename prompt via onRename.")]
		[SerializeField]
		private Button renameButton;

		// Token: 0x0400A62D RID: 42541
		[Tooltip("Optional 'Save' button (custom docs only) — disabled until the doc is valid; onSave commits.")]
		[SerializeField]
		private Button saveButton;

		// Token: 0x0400A62E RID: 42542
		[Tooltip("Optional label under the header showing why Save is disabled (e.g. 'Add a set in: Exit').")]
		[SerializeField]
		private TextMeshProUGUI validationHint;

		// Token: 0x0400A62F RID: 42543
		[Header("Binding chips (read-only identity, shown for every doc)")]
		[Tooltip("Row that holds the read-only binding chips. If unwired, bindingText is used as fallback.")]
		[SerializeField]
		private GameObject bindingChipsRoot;

		// Token: 0x0400A630 RID: 42544
		[SerializeField]
		private TextMeshProUGUI actorChipLabel;

		// Token: 0x0400A631 RID: 42545
		[Tooltip("Whole personality chip GO — hidden for non-Civilian actors.")]
		[SerializeField]
		private GameObject personalityChip;

		// Token: 0x0400A632 RID: 42546
		[SerializeField]
		private TextMeshProUGUI personalityChipLabel;

		// Token: 0x0400A633 RID: 42547
		[SerializeField]
		private TextMeshProUGUI genderChipLabel;

		// Token: 0x0400A634 RID: 42548
		[Header("Editing phase caption (optional)")]
		[Tooltip("Optional 'Editing Phase: X' title shown under the steps.")]
		[SerializeField]
		private TextMeshProUGUI editingPhaseTitle;

		// Token: 0x0400A635 RID: 42549
		[Tooltip("Optional one-line description of the selected phase.")]
		[SerializeField]
		private TextMeshProUGUI editingPhaseSubtitle;

		// Token: 0x0400A636 RID: 42550
		[Header("Phase steps (pre-generated)")]
		[Tooltip("Ancestor that contains the fixed per-phase step GameObjects (ReactionStep, ExitStep, …).")]
		[SerializeField]
		private Transform stepsParent;

		// Token: 0x0400A637 RID: 42551
		[Header("Sets list")]
		[Tooltip("Vertical content that holds the set cards (SetsContent).")]
		[SerializeField]
		private RectTransform setsParent;

		// Token: 0x0400A638 RID: 42552
		[Tooltip("Disabled SetTemplate cloned per set: 'Title','Percent','Weight'(Slider),'LinesContainer' + 'LineRow' template; optional 'TypeBadge','ResetWeight','Edit','Delete'.")]
		[SerializeField]
		private GameObject setTemplate;

		// Token: 0x0400A639 RID: 42553
		[SerializeField]
		private GameObject emptyState;

		// Token: 0x0400A63A RID: 42554
		[Tooltip("Optional 'Add New Set' button (kept as the last child of setsParent).")]
		[SerializeField]
		private GameObject addSetButton;

		// Token: 0x0400A63B RID: 42555
		private DialogueDocView _doc;

		// Token: 0x0400A63C RID: 42556
		private DialogueCategory _phaseCategory;

		// Token: 0x0400A63D RID: 42557
		private bool _hasPhase;

		// Token: 0x0400A63E RID: 42558
		private Action _onBack;

		// Token: 0x0400A63F RID: 42559
		private Action<string> _onDelete;

		// Token: 0x0400A640 RID: 42560
		private Action<string> _onDuplicate;

		// Token: 0x0400A641 RID: 42561
		private Action<string, int> _onAltWeightChanged;

		// Token: 0x0400A642 RID: 42562
		private Action<string> _onAltWeightReset;

		// Token: 0x0400A643 RID: 42563
		private Action<string, string> _onRename;

		// Token: 0x0400A644 RID: 42564
		private Action<string, DialogueCategory> _onAddSet;

		// Token: 0x0400A645 RID: 42565
		private Action<string> _onDeleteSet;

		// Token: 0x0400A646 RID: 42566
		private Action<string, int> _onEditLine;

		// Token: 0x0400A647 RID: 42567
		private Action<string, int> _onDeleteLine;

		// Token: 0x0400A648 RID: 42568
		private Action<string, string[]> _onReorderLines;

		// Token: 0x0400A649 RID: 42569
		private Action<string> _onSave;

		// Token: 0x0400A64A RID: 42570
		private Action<string, PersonalityType, DialogueGender, DialogueActorGroup> _onSetBinding;

		// Token: 0x0400A64B RID: 42571
		private DialogueCategory[] _expectedPhases = Array.Empty<DialogueCategory>();

		// Token: 0x0400A64C RID: 42572
		private readonly GameObject[] _stepGos = new GameObject[DialogueDetailWidget.Steps.Length];

		// Token: 0x0400A64D RID: 42573
		private readonly CanvasGroup[] _stepGroups = new CanvasGroup[DialogueDetailWidget.Steps.Length];

		// Token: 0x0400A64E RID: 42574
		private readonly List<GameObject> _setPool = new List<GameObject>();

		// Token: 0x0400A64F RID: 42575
		private readonly CanvasGroup[] _arrowGroups = new CanvasGroup[DialogueDetailWidget.Arrows.Length];

		// Token: 0x0400A650 RID: 42576
		private CanvasGroup _exitArrowGroup;

		// Token: 0x0400A651 RID: 42577
		private bool _arrowsResolved;

		// Token: 0x0400A652 RID: 42578
		private static readonly DialogueActorGroup[] ActorDomain = new DialogueActorGroup[]
		{
			DialogueActorGroup.Civilians,
			DialogueActorGroup.Police,
			DialogueActorGroup.Journalists,
			DialogueActorGroup.Referee
		};

		// Token: 0x0400A653 RID: 42579
		private static readonly PersonalityType[] PersonalityDomain = new PersonalityType[]
		{
			PersonalityType.Neutral,
			PersonalityType.Shy,
			PersonalityType.Bold,
			PersonalityType.Curious,
			PersonalityType.Hostile,
			PersonalityType.Friendly
		};

		// Token: 0x0400A654 RID: 42580
		private static readonly DialogueGender[] GenderDomain = new DialogueGender[]
		{
			DialogueGender.Any,
			DialogueGender.Female,
			DialogueGender.Male
		};

		// Token: 0x0400A655 RID: 42581
		private string _lastDocId;

		// Token: 0x0400A656 RID: 42582
		private DialogueCategory _lastCategory;

		// Token: 0x02001499 RID: 5273
		private struct StepDef
		{
			// Token: 0x0600A2A6 RID: 41638 RVA: 0x0043902C File Offset: 0x0043722C
			public StepDef(string goName, DialogueCategory category, string wrapperName = null)
			{
				this.goName = goName;
				this.category = category;
				this.wrapperName = wrapperName;
			}

			// Token: 0x0400A657 RID: 42583
			public readonly string goName;

			// Token: 0x0400A658 RID: 42584
			public readonly DialogueCategory category;

			// Token: 0x0400A659 RID: 42585
			public readonly string wrapperName;
		}

		// Token: 0x0200149A RID: 5274
		private struct ArrowDef
		{
			// Token: 0x0600A2A7 RID: 41639 RVA: 0x00439043 File Offset: 0x00437243
			public ArrowDef(string goName, DialogueCategory entersCategory)
			{
				this.goName = goName;
				this.entersCategory = entersCategory;
			}

			// Token: 0x0400A65A RID: 42586
			public readonly string goName;

			// Token: 0x0400A65B RID: 42587
			public readonly DialogueCategory entersCategory;
		}
	}
}
