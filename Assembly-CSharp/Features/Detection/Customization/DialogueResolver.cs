using System;
using System.Collections.Generic;
using Bozo.AnimeCharacters;
using UnityEngine;

namespace Features.Detection.Customization
{
	// Token: 0x0200133C RID: 4924
	public static class DialogueResolver
	{
		// Token: 0x060097F8 RID: 38904 RVA: 0x003E4659 File Offset: 0x003E2859
		public static DialogueLine[] Resolve(string npcGuid, PersonalityType personality, Gender gender, DialogueCategory category, DialogueActorGroup group = DialogueActorGroup.Civilians, NPCDialogueProfileSO fallbackProfile = null)
		{
			return new DialogueResolver.Session(npcGuid, personality, gender, group, fallbackProfile).Resolve(category);
		}

		// Token: 0x060097F9 RID: 38905 RVA: 0x003E466D File Offset: 0x003E286D
		public static DialogueActorGroup GroupForActor(DetectionActorType actor)
		{
			switch (actor)
			{
			case DetectionActorType.Police:
				return DialogueActorGroup.Police;
			case DetectionActorType.Reporter:
				return DialogueActorGroup.Journalists;
			case DetectionActorType.Referee:
				return DialogueActorGroup.Referee;
			default:
				return DialogueActorGroup.Civilians;
			}
		}

		// Token: 0x060097FA RID: 38906 RVA: 0x003E468C File Offset: 0x003E288C
		private static List<DialogueAltView> Alternatives(DialogueDocView doc, DialogueCategory category)
		{
			List<DialogueAltView> list = new List<DialogueAltView>();
			foreach (DialoguePhaseView dialoguePhaseView in doc.phases)
			{
				if (dialoguePhaseView.category == category)
				{
					foreach (DialogueAltView dialogueAltView in dialoguePhaseView.alternatives)
					{
						if (DialogueResolver.HasSpokenLines(dialogueAltView.lines))
						{
							list.Add(dialogueAltView);
						}
					}
				}
			}
			return list;
		}

		// Token: 0x060097FB RID: 38907 RVA: 0x003E4704 File Offset: 0x003E2904
		private static bool HasSpokenLines(DialogueLine[] lines)
		{
			if (lines != null)
			{
				foreach (DialogueLine dialogueLine in lines)
				{
					if (dialogueLine != null && !string.IsNullOrWhiteSpace(dialogueLine.text))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060097FC RID: 38908 RVA: 0x003E473C File Offset: 0x003E293C
		private static DialogueLine[] Present(DialogueAltView? alternative)
		{
			if (alternative == null)
			{
				return null;
			}
			DialogueAltView value = alternative.Value;
			List<DialogueLine> list = new List<DialogueLine>(value.lines);
			list.RemoveAll((DialogueLine line) => line == null || string.IsNullOrWhiteSpace(line.text));
			return DialogueLine.CloneForPresentation(list.ToArray(), value.isSystem ? "dialogue" : null, value.isSystem ? "dialogue_speaker" : null);
		}

		// Token: 0x060097FD RID: 38909 RVA: 0x003E47B8 File Offset: 0x003E29B8
		private static T? Pick<T>(List<T> candidates, Func<T, int> weight) where T : struct
		{
			if (candidates.Count == 0)
			{
				return null;
			}
			int num = 0;
			foreach (T t in candidates)
			{
				num += Mathf.Max(0, weight(t));
			}
			if (num <= 0)
			{
				return new T?(candidates[0]);
			}
			int num2 = Random.Range(0, num);
			foreach (T t2 in candidates)
			{
				num2 -= Mathf.Max(0, weight(t2));
				if (num2 < 0)
				{
					return new T?(t2);
				}
			}
			return new T?(candidates[candidates.Count - 1]);
		}

		// Token: 0x0200133D RID: 4925
		public sealed class Session
		{
			// Token: 0x060097FE RID: 38910 RVA: 0x003E48AC File Offset: 0x003E2AAC
			public Session(string npcGuid, PersonalityType personality, Gender gender, DialogueActorGroup group = DialogueActorGroup.Civilians, NPCDialogueProfileSO fallbackProfile = null)
			{
				this._npcGuid = npcGuid;
				this._personality = personality;
				this._gender = ((gender == Gender.Female) ? DialogueGender.Female : DialogueGender.Male);
				this._group = group;
				this._fallbackProfile = fallbackProfile;
			}

			// Token: 0x1700183C RID: 6204
			// (get) Token: 0x060097FF RID: 38911 RVA: 0x003E48E0 File Offset: 0x003E2AE0
			public bool UsesEncounterPack
			{
				get
				{
					return !this._hasAssignment && (this._resolvedDocument == null || (this._resolvedDocument.Value.isSystem && (this._fallbackProfile == null || this._resolvedDocument.Value.id == this._fallbackProfile.name)));
				}
			}

			// Token: 0x06009800 RID: 38912 RVA: 0x003E494C File Offset: 0x003E2B4C
			public DialogueLine[] Resolve(DialogueCategory category)
			{
				DialogueLibraryService.EnsureLoaded();
				this._resolvedDocument = null;
				List<DialogueAltView> assigned = this.GetAssigned(category, out this._hasAssignment);
				if (this._hasAssignment)
				{
					return DialogueResolver.Present(DialogueResolver.Pick<DialogueAltView>(assigned, (DialogueAltView a) => a.effectiveWeight));
				}
				if (this._document != null)
				{
					List<DialogueAltView> list = DialogueResolver.Alternatives(this._document.Value, category);
					if (list.Count > 0)
					{
						this._resolvedDocument = this._document;
						return DialogueResolver.Present(DialogueResolver.Pick<DialogueAltView>(list, (DialogueAltView a) => a.effectiveWeight));
					}
				}
				PersonalityType? personalityType = ((this._group == DialogueActorGroup.Civilians) ? new PersonalityType?(this._personality) : null);
				List<DialogueDocView> list2 = this.Candidates(personalityType, category);
				if (list2.Count == 0 && personalityType != null && personalityType.Value != PersonalityType.Neutral)
				{
					list2 = this.Candidates(new PersonalityType?(PersonalityType.Neutral), category);
				}
				if (list2.Count == 0 && this._fallbackProfile != null)
				{
					DialogueDocView runtimeFallbackDoc = DialogueLibraryService.GetRuntimeFallbackDoc(this._fallbackProfile, this._group);
					if (DialogueResolver.Alternatives(runtimeFallbackDoc, category).Count > 0)
					{
						list2.Add(runtimeFallbackDoc);
					}
				}
				if (list2.Count == 0)
				{
					return null;
				}
				DialogueDocView? dialogueDocView = DialogueResolver.Pick<DialogueDocView>(list2, (DialogueDocView doc) => doc.macroWeight);
				this._resolvedDocument = dialogueDocView;
				if (this._document == null)
				{
					this._document = dialogueDocView;
				}
				return DialogueResolver.Present(DialogueResolver.Pick<DialogueAltView>(DialogueResolver.Alternatives(dialogueDocView.Value, category), (DialogueAltView a) => a.effectiveWeight));
			}

			// Token: 0x06009801 RID: 38913 RVA: 0x003E4B20 File Offset: 0x003E2D20
			private List<DialogueDocView> Candidates(PersonalityType? personality, DialogueCategory category)
			{
				List<DialogueDocView> runtimeDialogueDocs = DialogueLibraryService.GetRuntimeDialogueDocs(this._group, personality, this._gender);
				runtimeDialogueDocs.RemoveAll((DialogueDocView doc) => DialogueResolver.Alternatives(doc, category).Count == 0);
				return runtimeDialogueDocs;
			}

			// Token: 0x06009802 RID: 38914 RVA: 0x003E4B60 File Offset: 0x003E2D60
			private List<DialogueAltView> GetAssigned(DialogueCategory category, out bool hasAssignment)
			{
				List<DialogueAltView> list = new List<DialogueAltView>();
				hasAssignment = false;
				string[] array = (string.IsNullOrEmpty(this._npcGuid) ? null : DialogueLibraryService.GetNpcWhitelist(this._npcGuid));
				if (array == null)
				{
					return list;
				}
				string[] array2 = array;
				for (int i = 0; i < array2.Length; i++)
				{
					DialogueView? byId = DialogueLibraryService.GetById(array2[i]);
					if (byId != null && DialogueResolver.HasSpokenLines(byId.Value.lines))
					{
						hasAssignment = true;
						DialogueView value = byId.Value;
						if (value.category == category)
						{
							list.Add(new DialogueAltView(value.id, value.isSystem, value.effectiveWeight, value.defaultWeight, value.lines));
						}
					}
				}
				return list;
			}

			// Token: 0x04009D25 RID: 40229
			private readonly string _npcGuid;

			// Token: 0x04009D26 RID: 40230
			private readonly PersonalityType _personality;

			// Token: 0x04009D27 RID: 40231
			private readonly DialogueGender _gender;

			// Token: 0x04009D28 RID: 40232
			private readonly DialogueActorGroup _group;

			// Token: 0x04009D29 RID: 40233
			private readonly NPCDialogueProfileSO _fallbackProfile;

			// Token: 0x04009D2A RID: 40234
			private DialogueDocView? _document;

			// Token: 0x04009D2B RID: 40235
			private DialogueDocView? _resolvedDocument;

			// Token: 0x04009D2C RID: 40236
			private bool _hasAssignment;
		}
	}
}
