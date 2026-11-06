using System;
using System.Collections.Generic;
using Features.Detection;
using Features.Detection.Customization;
using Michsky.MUIP;
using ProjectExhibitionist.CharacterCustomization.Core;
using ProjectExhibitionist.CharacterCustomization.Core.Runtime;
using UnityEngine;
using UnityEngine.Events;

namespace Bozo.AnimeCharacters
{
	// Token: 0x02000895 RID: 2197
	[RequireComponent(typeof(CustomDropdown))]
	public class DialogueOverrideSelector : MonoBehaviour
	{
		// Token: 0x17000A21 RID: 2593
		// (get) Token: 0x06003B25 RID: 15141 RVA: 0x0015F844 File Offset: 0x0015DA44
		private CustomDropdown Dropdown
		{
			get
			{
				if (!(this._dropdown != null))
				{
					return this._dropdown = base.GetComponent<CustomDropdown>();
				}
				return this._dropdown;
			}
		}

		// Token: 0x06003B26 RID: 15142 RVA: 0x0015F875 File Offset: 0x0015DA75
		private void OnEnable()
		{
			CustomizationDropdownLocalization.Bind(this.Dropdown, true, "cielos_apps");
			this.Dropdown.onValueChanged.AddListener(new UnityAction<int>(this.HandleSelectionChanged));
			this.Rebuild();
		}

		// Token: 0x06003B27 RID: 15143 RVA: 0x0015F8AA File Offset: 0x0015DAAA
		private void OnDisable()
		{
			if (this._dropdown != null)
			{
				this._dropdown.onValueChanged.RemoveListener(new UnityAction<int>(this.HandleSelectionChanged));
			}
		}

		// Token: 0x06003B28 RID: 15144 RVA: 0x0015F8D6 File Offset: 0x0015DAD6
		private void HandleSelectionChanged(int index)
		{
			this._hasPendingSelection = false;
		}

		// Token: 0x06003B29 RID: 15145 RVA: 0x0015F8E0 File Offset: 0x0015DAE0
		public void Rebuild()
		{
			CustomizationCoreOrchestrator customizationCoreOrchestrator = Object.FindObjectOfType<CustomizationCoreOrchestrator>(true);
			this._editingActorGroup = DialogueOverrideSelector.ResolveEditingActorGroup((customizationCoreOrchestrator != null) ? customizationCoreOrchestrator.CurrentSession : null);
			this.RebuildOptions(DialogueLibraryService.GetAllCustomDocs());
		}

		// Token: 0x06003B2A RID: 15146 RVA: 0x0015F91C File Offset: 0x0015DB1C
		private static DialogueActorGroup ResolveEditingActorGroup(CustomizationSessionContext session)
		{
			if (session != null && session.targetType == CustomizationTargetType.Npc && (session.presetId == "sys_Stadium_Referee_01" || (session.runtimeInstance != null && session.runtimeInstance.GetComponentInChildren<RefereeChaser>(true) != null)))
			{
				return DialogueActorGroup.Referee;
			}
			return DialogueActorGroup.Civilians;
		}

		// Token: 0x06003B2B RID: 15147 RVA: 0x0015F96C File Offset: 0x0015DB6C
		private void RebuildOptions(IEnumerable<DialogueDocView> documents)
		{
			CustomDropdown dropdown = this.Dropdown;
			if (dropdown == null)
			{
				return;
			}
			string selection = this.GetSelection();
			this._docIds.Clear();
			dropdown.items = new List<CustomDropdown.Item>();
			dropdown.CreateNewItem("Automatic (none)", false);
			this._docIds.Add("");
			foreach (DialogueDocView dialogueDocView in documents)
			{
				if (dialogueDocView.actorGroup == DialogueActorGroup.Civilians || dialogueDocView.actorGroup == this._editingActorGroup)
				{
					dropdown.CreateNewItem(dialogueDocView.DisplayLabel, false);
					this._docIds.Add(dialogueDocView.id);
				}
			}
			this._hasBuiltOptions = true;
			int num = this._docIds.IndexOf(selection ?? string.Empty);
			dropdown.selectedItemIndex = ((num >= 0) ? num : 0);
			this._hasPendingSelection = false;
			dropdown.SetupDropdown();
		}

		// Token: 0x06003B2C RID: 15148 RVA: 0x0015FA68 File Offset: 0x0015DC68
		public string GetSelection()
		{
			if (this._hasPendingSelection)
			{
				return this._pendingSelection ?? string.Empty;
			}
			CustomDropdown dropdown = this.Dropdown;
			if (dropdown == null)
			{
				return "";
			}
			int selectedItemIndex = dropdown.selectedItemIndex;
			if (selectedItemIndex < 0 || selectedItemIndex >= this._docIds.Count)
			{
				return "";
			}
			return this._docIds[selectedItemIndex];
		}

		// Token: 0x06003B2D RID: 15149 RVA: 0x0015FAD0 File Offset: 0x0015DCD0
		public void SetSelection(string docId)
		{
			this._pendingSelection = docId ?? string.Empty;
			this._hasPendingSelection = true;
			CustomDropdown dropdown = this.Dropdown;
			if (!this._hasBuiltOptions || dropdown == null)
			{
				return;
			}
			int num = this._docIds.IndexOf(this._pendingSelection);
			if (num < 0)
			{
				return;
			}
			this._hasPendingSelection = false;
			dropdown.SetDropdownIndex(num, true);
		}

		// Token: 0x04003B57 RID: 15191
		private CustomDropdown _dropdown;

		// Token: 0x04003B58 RID: 15192
		private readonly List<string> _docIds = new List<string>();

		// Token: 0x04003B59 RID: 15193
		private string _pendingSelection;

		// Token: 0x04003B5A RID: 15194
		private bool _hasPendingSelection;

		// Token: 0x04003B5B RID: 15195
		private bool _hasBuiltOptions;

		// Token: 0x04003B5C RID: 15196
		private DialogueActorGroup _editingActorGroup;
	}
}
