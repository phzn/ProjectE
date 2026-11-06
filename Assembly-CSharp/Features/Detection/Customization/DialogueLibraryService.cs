using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Bozo.AnimeCharacters;
using UnityEngine;

namespace Features.Detection.Customization
{
	// Token: 0x02001324 RID: 4900
	public static class DialogueLibraryService
	{
		// Token: 0x1400010E RID: 270
		// (add) Token: 0x06009775 RID: 38773 RVA: 0x003E16B4 File Offset: 0x003DF8B4
		// (remove) Token: 0x06009776 RID: 38774 RVA: 0x003E16E8 File Offset: 0x003DF8E8
		public static event Action Changed;

		// Token: 0x06009777 RID: 38775 RVA: 0x003E171B File Offset: 0x003DF91B
		public static void EnsureLoaded()
		{
			if (DialogueLibraryService._loaded)
			{
				return;
			}
			DialogueLibraryService.ReloadInternal();
		}

		// Token: 0x06009778 RID: 38776 RVA: 0x003E172A File Offset: 0x003DF92A
		public static void Reload()
		{
			DialogueLibraryService.ReloadInternal();
			Action changed = DialogueLibraryService.Changed;
			if (changed == null)
			{
				return;
			}
			changed();
		}

		// Token: 0x06009779 RID: 38777 RVA: 0x003E1740 File Offset: 0x003DF940
		private static void ReloadInternal()
		{
			DialogueLibraryService._systemSets = DialogueLibraryService.ProjectSystemSets();
			DialogueLibraryService._delta = DialogueCustomizationStore.Load();
			DialogueLibraryService.RebuildWeightMap();
			DialogueLibraryService._loaded = true;
		}

		// Token: 0x0600977A RID: 38778 RVA: 0x003E1764 File Offset: 0x003DF964
		private static void RebuildWeightMap()
		{
			if (DialogueLibraryService._delta == null)
			{
				DialogueLibraryService._delta = new DialogueCustomizationFile();
			}
			DialogueCustomizationFile dialogueCustomizationFile = DialogueLibraryService._delta;
			if (dialogueCustomizationFile.customEntries == null)
			{
				dialogueCustomizationFile.customEntries = new List<CustomDialogueEntry>();
			}
			dialogueCustomizationFile = DialogueLibraryService._delta;
			if (dialogueCustomizationFile.customDocs == null)
			{
				dialogueCustomizationFile.customDocs = new List<CustomDialogueDoc>();
			}
			dialogueCustomizationFile = DialogueLibraryService._delta;
			if (dialogueCustomizationFile.weightOverrides == null)
			{
				dialogueCustomizationFile.weightOverrides = new List<DialogueWeightOverride>();
			}
			dialogueCustomizationFile = DialogueLibraryService._delta;
			if (dialogueCustomizationFile.macroWeightOverrides == null)
			{
				dialogueCustomizationFile.macroWeightOverrides = new List<DialogueMacroWeightOverride>();
			}
			dialogueCustomizationFile = DialogueLibraryService._delta;
			if (dialogueCustomizationFile.npcAssignments == null)
			{
				dialogueCustomizationFile.npcAssignments = new List<NpcDialogueAssignment>();
			}
			DialogueLibraryService._weightOverrideMap = new Dictionary<string, int>();
			foreach (DialogueWeightOverride dialogueWeightOverride in DialogueLibraryService._delta.weightOverrides)
			{
				if (dialogueWeightOverride != null && !string.IsNullOrEmpty(dialogueWeightOverride.setId))
				{
					DialogueLibraryService._weightOverrideMap[dialogueWeightOverride.setId] = dialogueWeightOverride.weight;
				}
			}
			DialogueLibraryService._macroWeightOverrideMap = new Dictionary<string, int>();
			if (DialogueLibraryService._delta.macroWeightOverrides != null)
			{
				foreach (DialogueMacroWeightOverride dialogueMacroWeightOverride in DialogueLibraryService._delta.macroWeightOverrides)
				{
					if (dialogueMacroWeightOverride != null && !string.IsNullOrEmpty(dialogueMacroWeightOverride.profileId))
					{
						DialogueLibraryService._macroWeightOverrideMap[dialogueMacroWeightOverride.profileId] = dialogueMacroWeightOverride.weight;
					}
				}
			}
		}

		// Token: 0x0600977B RID: 38779 RVA: 0x003E18F4 File Offset: 0x003DFAF4
		private static int EffectiveMacroWeight(string profileId, int defaultWeight)
		{
			int num;
			if (string.IsNullOrEmpty(profileId) || DialogueLibraryService._macroWeightOverrideMap == null || !DialogueLibraryService._macroWeightOverrideMap.TryGetValue(profileId, out num))
			{
				return defaultWeight;
			}
			return num;
		}

		// Token: 0x0600977C RID: 38780 RVA: 0x003E1924 File Offset: 0x003DFB24
		private static List<DialogueLibraryService.SystemSet> ProjectSystemSets()
		{
			List<DialogueLibraryService.SystemSet> list = new List<DialogueLibraryService.SystemSet>();
			DialogueLibraryRegistrySO dialogueLibraryRegistrySO = DialogueLibraryRegistrySO.LoadDefault();
			if (dialogueLibraryRegistrySO == null)
			{
				Debug.LogWarning("[DialogueLibraryService] Nessun DialogueLibraryRegistry in Resources. Catalogo di sistema vuoto: esegui Tools/Dialoghi/Popola Library Registry.");
				return list;
			}
			foreach (DialogueLibraryRegistrySO.SystemProfileEntry systemProfileEntry in dialogueLibraryRegistrySO.systemProfiles)
			{
				if (systemProfileEntry != null && !(systemProfileEntry.profile == null))
				{
					string name = systemProfileEntry.profile.name;
					DialogueLibraryService.ProjectCategory(list, systemProfileEntry, name, DialogueCategory.Detection, systemProfileEntry.profile.detectionDialogues);
					DialogueLibraryService.ProjectCategory(list, systemProfileEntry, name, DialogueCategory.Reaction, systemProfileEntry.profile.reactionDialogues);
					DialogueLibraryService.ProjectCategory(list, systemProfileEntry, name, DialogueCategory.Shame, systemProfileEntry.profile.shameDialogues);
					DialogueLibraryService.ProjectCategory(list, systemProfileEntry, name, DialogueCategory.Exhibit, systemProfileEntry.profile.exhibitDialogues);
					DialogueLibraryService.ProjectCategory(list, systemProfileEntry, name, DialogueCategory.Exit, systemProfileEntry.profile.exitDialogues);
					DialogueLibraryService.ProjectCategory(list, systemProfileEntry, name, DialogueCategory.Chasing, systemProfileEntry.profile.chasingDialogues);
				}
			}
			return list;
		}

		// Token: 0x0600977D RID: 38781 RVA: 0x003E1A3C File Offset: 0x003DFC3C
		private static void ProjectCategory(List<DialogueLibraryService.SystemSet> into, DialogueLibraryRegistrySO.SystemProfileEntry entry, string profileName, DialogueCategory category, DialogueSet[] sets)
		{
			if (sets == null)
			{
				return;
			}
			for (int i = 0; i < sets.Length; i++)
			{
				DialogueSet dialogueSet = sets[i];
				if (dialogueSet != null)
				{
					into.Add(new DialogueLibraryService.SystemSet
					{
						setId = DialogueLibraryService.ResolveSetId(dialogueSet, profileName, category, i),
						profileId = profileName,
						macroWeight = entry.profile.macroWeight,
						personality = entry.personality,
						gender = entry.gender,
						category = category,
						defaultWeight = dialogueSet.weight,
						lines = dialogueSet.lines,
						excludeFromPool = entry.excludeFromPool,
						actorGroup = entry.actorGroup,
						displayName = ((entry.profile != null) ? entry.profile.dialogueName : null)
					});
				}
			}
		}

		// Token: 0x0600977E RID: 38782 RVA: 0x003E1B11 File Offset: 0x003DFD11
		private static string ResolveSetId(DialogueSet set, string profileName, DialogueCategory category, int index)
		{
			if (set != null && !string.IsNullOrWhiteSpace(set.setId))
			{
				return set.setId;
			}
			return string.Format("sys-{0}-{1}-{2}", profileName, category.ToString().ToLowerInvariant(), index);
		}

		// Token: 0x0600977F RID: 38783 RVA: 0x003E1B50 File Offset: 0x003DFD50
		public static List<DialogueDocView> GetRuntimeDialogueDocs(DialogueActorGroup group, PersonalityType? personality, DialogueGender gender)
		{
			DialogueLibraryService.EnsureLoaded();
			List<DialogueDocView> list = new List<DialogueDocView>();
			foreach (IGrouping<string, DialogueLibraryService.SystemSet> grouping in from s in DialogueLibraryService._systemSets
				where s.actorGroup == @group && (@group != DialogueActorGroup.Civilians || !s.excludeFromPool) && (personality == null || s.personality == personality.Value) && DialogueLibraryService.GenderMatches(s.gender, gender)
				group s by s.profileId)
			{
				DialogueLibraryService.SystemSet systemSet = grouping.First<DialogueLibraryService.SystemSet>();
				DialoguePhaseView[] array = (from s in grouping
					group s by s.category into phase
					select new DialoguePhaseView(phase.Key, phase.Select<DialogueLibraryService.SystemSet, DialogueAltView>((DialogueLibraryService.SystemSet s) => new DialogueAltView(s.setId, true, DialogueLibraryService.EffectiveWeight(s.setId, s.defaultWeight), s.defaultWeight, s.lines)).ToArray<DialogueAltView>())).ToArray<DialoguePhaseView>();
				list.Add(new DialogueDocView(systemSet.profileId, true, systemSet.displayName, systemSet.personality, systemSet.gender, group, DialogueLibraryService.EffectiveMacroWeight(systemSet.profileId, systemSet.macroWeight), array));
			}
			if (DialogueLibraryService._delta.customDocs != null)
			{
				foreach (CustomDialogueDoc customDialogueDoc in DialogueLibraryService._delta.customDocs)
				{
					if (customDialogueDoc != null && customDialogueDoc.actorGroup == group && (personality == null || customDialogueDoc.personality == personality.Value) && DialogueLibraryService.GenderMatches(customDialogueDoc.gender, gender))
					{
						list.Add(DialogueLibraryService.BuildDocFromCustom(customDialogueDoc));
					}
				}
			}
			if (group == DialogueActorGroup.Civilians)
			{
				foreach (CustomDialogueEntry customDialogueEntry in DialogueLibraryService._delta.customEntries)
				{
					if (customDialogueEntry != null && (personality == null || customDialogueEntry.personality == personality.Value) && DialogueLibraryService.GenderMatches(customDialogueEntry.gender, gender))
					{
						int num = DialogueLibraryService.EffectiveWeight(customDialogueEntry.id, customDialogueEntry.weight);
						list.Add(new DialogueDocView(customDialogueEntry.id, false, customDialogueEntry.displayName, customDialogueEntry.personality, customDialogueEntry.gender, group, num, new DialoguePhaseView[]
						{
							new DialoguePhaseView(customDialogueEntry.category, new DialogueAltView[]
							{
								new DialogueAltView(customDialogueEntry.id, false, num, customDialogueEntry.weight, customDialogueEntry.lines)
							})
						}));
					}
				}
			}
			return list;
		}

		// Token: 0x06009780 RID: 38784 RVA: 0x003E1E4C File Offset: 0x003E004C
		internal static DialogueDocView GetRuntimeFallbackDoc(NPCDialogueProfileSO profile, DialogueActorGroup group)
		{
			return DialogueLibraryService.BuildDoc(new DialogueLibraryRegistrySO.SystemProfileEntry
			{
				profile = profile,
				actorGroup = group,
				gender = profile.targetGender
			});
		}

		// Token: 0x06009781 RID: 38785 RVA: 0x003E1E74 File Offset: 0x003E0074
		public static List<DialogueView> GetEntries(PersonalityType personality, DialogueGender gender, DialogueCategory category, bool forPool = true)
		{
			DialogueLibraryService.EnsureLoaded();
			List<DialogueView> list = new List<DialogueView>();
			foreach (DialogueLibraryService.SystemSet systemSet in DialogueLibraryService._systemSets)
			{
				if (systemSet.personality == personality && systemSet.category == category && DialogueLibraryService.GenderMatches(systemSet.gender, gender) && (!forPool || !systemSet.excludeFromPool))
				{
					list.Add(new DialogueView(systemSet.setId, true, systemSet.personality, systemSet.gender, systemSet.category, DialogueLibraryService.EffectiveWeight(systemSet.setId, systemSet.defaultWeight), systemSet.defaultWeight, systemSet.lines, systemSet.excludeFromPool, systemSet.actorGroup, systemSet.displayName));
				}
			}
			foreach (CustomDialogueEntry customDialogueEntry in DialogueLibraryService._delta.customEntries)
			{
				if (customDialogueEntry != null && customDialogueEntry.personality == personality && customDialogueEntry.category == category && DialogueLibraryService.GenderMatches(customDialogueEntry.gender, gender))
				{
					list.Add(new DialogueView(customDialogueEntry.id, false, customDialogueEntry.personality, customDialogueEntry.gender, customDialogueEntry.category, DialogueLibraryService.EffectiveWeight(customDialogueEntry.id, customDialogueEntry.weight), customDialogueEntry.weight, customDialogueEntry.lines, false, DialogueActorGroup.Civilians, customDialogueEntry.displayName));
				}
			}
			if (DialogueLibraryService._delta.customDocs != null)
			{
				foreach (CustomDialogueDoc customDialogueDoc in DialogueLibraryService._delta.customDocs)
				{
					if (customDialogueDoc != null && customDialogueDoc.actorGroup == DialogueActorGroup.Civilians && customDialogueDoc.personality == personality && DialogueLibraryService.GenderMatches(customDialogueDoc.gender, gender) && customDialogueDoc.phases != null)
					{
						foreach (CustomDialoguePhase customDialoguePhase in customDialogueDoc.phases)
						{
							if (customDialoguePhase != null && customDialoguePhase.category == category && customDialoguePhase.sets != null)
							{
								foreach (CustomDialogueSet customDialogueSet in customDialoguePhase.sets)
								{
									if (customDialogueSet != null)
									{
										list.Add(new DialogueView(customDialogueSet.id, false, customDialogueDoc.personality, customDialogueDoc.gender, category, DialogueLibraryService.EffectiveWeight(customDialogueSet.id, customDialogueSet.weight), customDialogueSet.weight, customDialogueSet.lines, false, customDialogueDoc.actorGroup, customDialogueDoc.displayName));
									}
								}
							}
						}
					}
				}
			}
			return list;
		}

		// Token: 0x06009782 RID: 38786 RVA: 0x003E21D8 File Offset: 0x003E03D8
		public static List<DialogueView> GetEntriesByGroup(DialogueActorGroup group, PersonalityType? personality, DialogueGender? gender, DialogueCategory category)
		{
			DialogueLibraryService.EnsureLoaded();
			List<DialogueView> list = new List<DialogueView>();
			foreach (DialogueLibraryService.SystemSet systemSet in DialogueLibraryService._systemSets)
			{
				if (systemSet.actorGroup == group && systemSet.category == category && (personality == null || systemSet.personality == personality.Value) && (gender == null || DialogueLibraryService.GenderMatches(systemSet.gender, gender.Value)))
				{
					list.Add(new DialogueView(systemSet.setId, true, systemSet.personality, systemSet.gender, systemSet.category, DialogueLibraryService.EffectiveWeight(systemSet.setId, systemSet.defaultWeight), systemSet.defaultWeight, systemSet.lines, systemSet.excludeFromPool, systemSet.actorGroup, systemSet.displayName));
				}
			}
			if (group == DialogueActorGroup.Civilians)
			{
				foreach (CustomDialogueEntry customDialogueEntry in DialogueLibraryService._delta.customEntries)
				{
					if (customDialogueEntry != null && customDialogueEntry.category == category && (personality == null || customDialogueEntry.personality == personality.Value) && (gender == null || DialogueLibraryService.GenderMatches(customDialogueEntry.gender, gender.Value)))
					{
						list.Add(new DialogueView(customDialogueEntry.id, false, customDialogueEntry.personality, customDialogueEntry.gender, customDialogueEntry.category, DialogueLibraryService.EffectiveWeight(customDialogueEntry.id, customDialogueEntry.weight), customDialogueEntry.weight, customDialogueEntry.lines, false, DialogueActorGroup.Civilians, customDialogueEntry.displayName));
					}
				}
			}
			return list;
		}

		// Token: 0x06009783 RID: 38787 RVA: 0x003E23C0 File Offset: 0x003E05C0
		[return: TupleElementNames(new string[] { "total", "system", "custom" })]
		public static ValueTuple<int, int, int> GetGroupCounts(DialogueActorGroup group)
		{
			DialogueLibraryService.EnsureLoaded();
			int num = DialogueLibraryService._systemSets.Count<DialogueLibraryService.SystemSet>((DialogueLibraryService.SystemSet s) => s.actorGroup == group);
			int num2;
			if (group != DialogueActorGroup.Civilians)
			{
				num2 = 0;
			}
			else
			{
				num2 = DialogueLibraryService._delta.customEntries.Count<CustomDialogueEntry>((CustomDialogueEntry c) => c != null);
			}
			int num3 = num2;
			return new ValueTuple<int, int, int>(num + num3, num, num3);
		}

		// Token: 0x06009784 RID: 38788 RVA: 0x003E243C File Offset: 0x003E063C
		public static List<DialogueActorGroup> GetPopulatedGroups()
		{
			DialogueLibraryService.EnsureLoaded();
			return (from g in DialogueLibraryService._systemSets.Select<DialogueLibraryService.SystemSet, DialogueActorGroup>((DialogueLibraryService.SystemSet s) => s.actorGroup).Distinct<DialogueActorGroup>()
				orderby (int)g
				select g).ToList<DialogueActorGroup>();
		}

		// Token: 0x1700183B RID: 6203
		// (get) Token: 0x06009785 RID: 38789 RVA: 0x003E24A5 File Offset: 0x003E06A5
		private static DialogueLibraryRegistrySO Registry
		{
			get
			{
				if (!(DialogueLibraryService._registryCache != null))
				{
					return DialogueLibraryService._registryCache = DialogueLibraryRegistrySO.LoadDefault();
				}
				return DialogueLibraryService._registryCache;
			}
		}

		// Token: 0x06009786 RID: 38790 RVA: 0x003E24C8 File Offset: 0x003E06C8
		public static List<DialogueDocView> GetDialogueDocs(DialogueActorGroup group, PersonalityType? personality, DialogueGender? gender)
		{
			DialogueLibraryService.EnsureLoaded();
			List<DialogueDocView> list = new List<DialogueDocView>();
			if (DialogueLibraryService.Registry == null)
			{
				return list;
			}
			foreach (DialogueLibraryRegistrySO.SystemProfileEntry systemProfileEntry in DialogueLibraryService.Registry.systemProfiles)
			{
				if (systemProfileEntry != null && !(systemProfileEntry.profile == null) && systemProfileEntry.actorGroup == group && (personality == null || systemProfileEntry.personality == personality.Value) && (gender == null || DialogueLibraryService.GenderMatches(systemProfileEntry.gender, gender.Value)))
				{
					DialogueDocView dialogueDocView = DialogueLibraryService.BuildDoc(systemProfileEntry);
					if (dialogueDocView.phases.Length != 0)
					{
						list.Add(dialogueDocView);
					}
				}
			}
			if (DialogueLibraryService._delta.customDocs != null)
			{
				foreach (CustomDialogueDoc customDialogueDoc in DialogueLibraryService._delta.customDocs)
				{
					if (customDialogueDoc != null && customDialogueDoc.actorGroup == group && (personality == null || customDialogueDoc.personality == personality.Value) && (gender == null || DialogueLibraryService.GenderMatches(customDialogueDoc.gender, gender.Value)))
					{
						list.Add(DialogueLibraryService.BuildDocFromCustom(customDialogueDoc));
					}
				}
			}
			return list;
		}

		// Token: 0x06009787 RID: 38791 RVA: 0x003E263C File Offset: 0x003E083C
		public static DialogueDocView? GetDialogueDocById(string id)
		{
			DialogueLibraryService.EnsureLoaded();
			if (string.IsNullOrEmpty(id))
			{
				DialogueDocView? dialogueDocView = null;
				return dialogueDocView;
			}
			CustomDialogueDoc customDialogueDoc = DialogueLibraryService.FindCustomDoc(id);
			if (customDialogueDoc != null)
			{
				return new DialogueDocView?(DialogueLibraryService.BuildDocFromCustom(customDialogueDoc));
			}
			if (DialogueLibraryService.Registry == null)
			{
				DialogueDocView? dialogueDocView = null;
				return dialogueDocView;
			}
			foreach (DialogueLibraryRegistrySO.SystemProfileEntry systemProfileEntry in DialogueLibraryService.Registry.systemProfiles)
			{
				if (systemProfileEntry != null && !(systemProfileEntry.profile == null) && systemProfileEntry.profile.name == id)
				{
					return new DialogueDocView?(DialogueLibraryService.BuildDoc(systemProfileEntry));
				}
			}
			return null;
		}

		// Token: 0x06009788 RID: 38792 RVA: 0x003E2710 File Offset: 0x003E0910
		public static List<DialogueDocView> GetAllCustomDocs()
		{
			DialogueLibraryService.EnsureLoaded();
			List<DialogueDocView> list = new List<DialogueDocView>();
			if (DialogueLibraryService._delta.customDocs != null)
			{
				foreach (CustomDialogueDoc customDialogueDoc in DialogueLibraryService._delta.customDocs)
				{
					if (customDialogueDoc != null)
					{
						list.Add(DialogueLibraryService.BuildDocFromCustom(customDialogueDoc));
					}
				}
			}
			return list;
		}

		// Token: 0x06009789 RID: 38793 RVA: 0x003E2788 File Offset: 0x003E0988
		private static DialogueDocView BuildDoc(DialogueLibraryRegistrySO.SystemProfileEntry entry)
		{
			NPCDialogueProfileSO profile = entry.profile;
			List<DialoguePhaseView> list = new List<DialoguePhaseView>();
			foreach (ValueTuple<DialogueCategory, Func<NPCDialogueProfileSO, DialogueSet[]>> valueTuple in DialogueLibraryService.PhaseMap)
			{
				DialogueCategory item = valueTuple.Item1;
				DialogueSet[] array = valueTuple.Item2(profile);
				if (array != null && array.Length != 0)
				{
					List<DialogueAltView> list2 = new List<DialogueAltView>();
					for (int j = 0; j < array.Length; j++)
					{
						DialogueSet dialogueSet = array[j];
						if (dialogueSet != null)
						{
							string text = DialogueLibraryService.ResolveSetId(dialogueSet, profile.name, item, j);
							list2.Add(new DialogueAltView(text, true, DialogueLibraryService.EffectiveWeight(text, dialogueSet.weight), dialogueSet.weight, dialogueSet.lines));
						}
					}
					if (list2.Count > 0)
					{
						list.Add(new DialoguePhaseView(item, list2.ToArray()));
					}
				}
			}
			return new DialogueDocView(profile.name, true, profile.dialogueName, entry.personality, entry.gender, entry.actorGroup, DialogueLibraryService.EffectiveMacroWeight(profile.name, profile.macroWeight), list.ToArray());
		}

		// Token: 0x0600978A RID: 38794 RVA: 0x003E28A3 File Offset: 0x003E0AA3
		public static DialogueCategory[] ExpectedPhases(DialogueActorGroup group)
		{
			if (group == DialogueActorGroup.Journalists)
			{
				return new DialogueCategory[]
				{
					DialogueCategory.Reaction,
					DialogueCategory.Exhibit,
					DialogueCategory.Shame,
					DialogueCategory.Exit,
					DialogueCategory.Chasing
				};
			}
			return new DialogueCategory[]
			{
				DialogueCategory.Reaction,
				DialogueCategory.Exhibit,
				DialogueCategory.Shame,
				DialogueCategory.Exit
			};
		}

		// Token: 0x0600978B RID: 38795 RVA: 0x003E28CC File Offset: 0x003E0ACC
		private static DialogueDocView BuildDocFromCustom(CustomDialogueDoc doc)
		{
			List<DialoguePhaseView> list = new List<DialoguePhaseView>();
			if (doc.phases != null)
			{
				foreach (CustomDialoguePhase customDialoguePhase in doc.phases)
				{
					if (customDialoguePhase != null && customDialoguePhase.sets != null && customDialoguePhase.sets.Count != 0)
					{
						List<DialogueAltView> list2 = new List<DialogueAltView>();
						foreach (CustomDialogueSet customDialogueSet in customDialoguePhase.sets)
						{
							if (customDialogueSet != null)
							{
								list2.Add(new DialogueAltView(customDialogueSet.id, false, DialogueLibraryService.EffectiveWeight(customDialogueSet.id, customDialogueSet.weight), customDialogueSet.weight, customDialogueSet.lines));
							}
						}
						if (list2.Count > 0)
						{
							list.Add(new DialoguePhaseView(customDialoguePhase.category, list2.ToArray()));
						}
					}
				}
			}
			return new DialogueDocView(doc.id, false, doc.displayName, doc.personality, doc.gender, doc.actorGroup, DialogueLibraryService.EffectiveMacroWeight(doc.id, doc.macroWeight), list.ToArray());
		}

		// Token: 0x0600978C RID: 38796 RVA: 0x003E2A28 File Offset: 0x003E0C28
		private static CustomDialogueDoc FindCustomDoc(string id)
		{
			if (string.IsNullOrEmpty(id) || DialogueLibraryService._delta.customDocs == null)
			{
				return null;
			}
			return DialogueLibraryService._delta.customDocs.FirstOrDefault<CustomDialogueDoc>((CustomDialogueDoc d) => d != null && d.id == id);
		}

		// Token: 0x0600978D RID: 38797 RVA: 0x003E2A78 File Offset: 0x003E0C78
		public static void SetMacroWeight(string profileId, int weight)
		{
			DialogueLibraryService.EnsureLoaded();
			if (string.IsNullOrEmpty(profileId))
			{
				return;
			}
			weight = Mathf.Clamp(weight, 1, 100);
			if (DialogueLibraryService._delta.macroWeightOverrides == null)
			{
				DialogueLibraryService._delta.macroWeightOverrides = new List<DialogueMacroWeightOverride>();
			}
			DialogueMacroWeightOverride dialogueMacroWeightOverride = DialogueLibraryService._delta.macroWeightOverrides.FirstOrDefault<DialogueMacroWeightOverride>((DialogueMacroWeightOverride o) => o != null && o.profileId == profileId);
			if (dialogueMacroWeightOverride == null)
			{
				dialogueMacroWeightOverride = new DialogueMacroWeightOverride
				{
					profileId = profileId
				};
				DialogueLibraryService._delta.macroWeightOverrides.Add(dialogueMacroWeightOverride);
			}
			dialogueMacroWeightOverride.weight = weight;
			DialogueLibraryService._macroWeightOverrideMap[profileId] = weight;
			DialogueLibraryService.Persist();
		}

		// Token: 0x0600978E RID: 38798 RVA: 0x003E2B2C File Offset: 0x003E0D2C
		[return: TupleElementNames(new string[] { "total", "system", "custom" })]
		public static ValueTuple<int, int, int> GetDialogueDocCounts(DialogueActorGroup group)
		{
			DialogueLibraryService.EnsureLoaded();
			if (DialogueLibraryService.Registry == null)
			{
				return new ValueTuple<int, int, int>(0, 0, 0);
			}
			int num = 0;
			foreach (DialogueLibraryRegistrySO.SystemProfileEntry systemProfileEntry in DialogueLibraryService.Registry.systemProfiles)
			{
				if (systemProfileEntry != null && !(systemProfileEntry.profile == null) && systemProfileEntry.actorGroup == group && DialogueLibraryService.HasAnyPhase(systemProfileEntry.profile))
				{
					num++;
				}
			}
			int num2 = ((DialogueLibraryService._delta.customDocs != null) ? DialogueLibraryService._delta.customDocs.Count<CustomDialogueDoc>((CustomDialogueDoc d) => d != null && d.actorGroup == group) : 0);
			return new ValueTuple<int, int, int>(num + num2, num, num2);
		}

		// Token: 0x0600978F RID: 38799 RVA: 0x003E2C10 File Offset: 0x003E0E10
		private static bool HasAnyPhase(NPCDialogueProfileSO p)
		{
			ValueTuple<DialogueCategory, Func<NPCDialogueProfileSO, DialogueSet[]>>[] phaseMap = DialogueLibraryService.PhaseMap;
			for (int i = 0; i < phaseMap.Length; i++)
			{
				DialogueSet[] array = phaseMap[i].Item2(p);
				if (array != null && array.Length != 0)
				{
					DialogueSet[] array2 = array;
					for (int j = 0; j < array2.Length; j++)
					{
						if (array2[j] != null)
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x06009790 RID: 38800 RVA: 0x003E2C68 File Offset: 0x003E0E68
		public static DialogueView? GetById(string id)
		{
			DialogueLibraryService.EnsureLoaded();
			if (string.IsNullOrEmpty(id))
			{
				return null;
			}
			DialogueLibraryService.SystemSet systemSet = DialogueLibraryService._systemSets.FirstOrDefault<DialogueLibraryService.SystemSet>((DialogueLibraryService.SystemSet x) => x.setId == id);
			if (systemSet != null)
			{
				return new DialogueView?(new DialogueView(systemSet.setId, true, systemSet.personality, systemSet.gender, systemSet.category, DialogueLibraryService.EffectiveWeight(systemSet.setId, systemSet.defaultWeight), systemSet.defaultWeight, systemSet.lines, systemSet.excludeFromPool, systemSet.actorGroup, systemSet.displayName));
			}
			CustomDialogueEntry customDialogueEntry = DialogueLibraryService._delta.customEntries.FirstOrDefault<CustomDialogueEntry>((CustomDialogueEntry x) => x != null && x.id == id);
			if (customDialogueEntry != null)
			{
				return new DialogueView?(new DialogueView(customDialogueEntry.id, false, customDialogueEntry.personality, customDialogueEntry.gender, customDialogueEntry.category, DialogueLibraryService.EffectiveWeight(customDialogueEntry.id, customDialogueEntry.weight), customDialogueEntry.weight, customDialogueEntry.lines, false, DialogueActorGroup.Civilians, customDialogueEntry.displayName));
			}
			CustomDialogueDoc customDialogueDoc;
			DialogueCategory dialogueCategory;
			CustomDialogueSet customDialogueSet = DialogueLibraryService.FindCustomSetWithDoc(id, out customDialogueDoc, out dialogueCategory);
			if (customDialogueSet != null && customDialogueDoc != null)
			{
				return new DialogueView?(new DialogueView(customDialogueSet.id, false, customDialogueDoc.personality, customDialogueDoc.gender, dialogueCategory, DialogueLibraryService.EffectiveWeight(customDialogueSet.id, customDialogueSet.weight), customDialogueSet.weight, customDialogueSet.lines, false, customDialogueDoc.actorGroup, customDialogueDoc.displayName));
			}
			return null;
		}

		// Token: 0x06009791 RID: 38801 RVA: 0x003E2DE4 File Offset: 0x003E0FE4
		private static CustomDialogueSet FindCustomSetWithDoc(string setId, out CustomDialogueDoc doc, out DialogueCategory category)
		{
			doc = null;
			category = DialogueCategory.Detection;
			if (string.IsNullOrEmpty(setId) || DialogueLibraryService._delta.customDocs == null)
			{
				return null;
			}
			Func<CustomDialogueSet, bool> <>9__0;
			foreach (CustomDialogueDoc customDialogueDoc in DialogueLibraryService._delta.customDocs)
			{
				if (((customDialogueDoc != null) ? customDialogueDoc.phases : null) != null)
				{
					foreach (CustomDialoguePhase customDialoguePhase in customDialogueDoc.phases)
					{
						if (((customDialoguePhase != null) ? customDialoguePhase.sets : null) != null)
						{
							IEnumerable<CustomDialogueSet> sets = customDialoguePhase.sets;
							Func<CustomDialogueSet, bool> func;
							if ((func = <>9__0) == null)
							{
								func = (<>9__0 = (CustomDialogueSet x) => x != null && x.id == setId);
							}
							CustomDialogueSet customDialogueSet = sets.FirstOrDefault<CustomDialogueSet>(func);
							if (customDialogueSet != null)
							{
								doc = customDialogueDoc;
								category = customDialoguePhase.category;
								return customDialogueSet;
							}
						}
					}
				}
			}
			return null;
		}

		// Token: 0x06009792 RID: 38802 RVA: 0x003E2F10 File Offset: 0x003E1110
		private static bool GenderMatches(DialogueGender setGender, DialogueGender query)
		{
			return setGender == DialogueGender.Any || query == DialogueGender.Any || setGender == query;
		}

		// Token: 0x06009793 RID: 38803 RVA: 0x003E2F20 File Offset: 0x003E1120
		private static int EffectiveWeight(string setId, int defaultWeight)
		{
			int num;
			if (string.IsNullOrEmpty(setId) || !DialogueLibraryService._weightOverrideMap.TryGetValue(setId, out num))
			{
				return defaultWeight;
			}
			return num;
		}

		// Token: 0x06009794 RID: 38804 RVA: 0x003E2F48 File Offset: 0x003E1148
		public static void SetWeight(string setId, int weight)
		{
			DialogueLibraryService.EnsureLoaded();
			if (string.IsNullOrEmpty(setId))
			{
				return;
			}
			weight = Mathf.Clamp(weight, 1, 100);
			CustomDialogueEntry customDialogueEntry = DialogueLibraryService._delta.customEntries.FirstOrDefault<CustomDialogueEntry>((CustomDialogueEntry x) => x != null && x.id == setId);
			CustomDialogueSet customDialogueSet = ((customDialogueEntry == null) ? DialogueLibraryService.FindCustomSet(setId) : null);
			if (customDialogueEntry != null)
			{
				customDialogueEntry.weight = weight;
				DialogueLibraryService._weightOverrideMap.Remove(setId);
				DialogueLibraryService._delta.weightOverrides.RemoveAll((DialogueWeightOverride o) => o != null && o.setId == setId);
			}
			else if (customDialogueSet != null)
			{
				customDialogueSet.weight = weight;
				DialogueLibraryService._weightOverrideMap.Remove(setId);
				DialogueLibraryService._delta.weightOverrides.RemoveAll((DialogueWeightOverride o) => o != null && o.setId == setId);
			}
			else
			{
				DialogueWeightOverride dialogueWeightOverride = DialogueLibraryService._delta.weightOverrides.FirstOrDefault<DialogueWeightOverride>((DialogueWeightOverride o) => o != null && o.setId == setId);
				if (dialogueWeightOverride == null)
				{
					dialogueWeightOverride = new DialogueWeightOverride
					{
						setId = setId
					};
					DialogueLibraryService._delta.weightOverrides.Add(dialogueWeightOverride);
				}
				dialogueWeightOverride.weight = weight;
				DialogueLibraryService._weightOverrideMap[setId] = weight;
			}
			DialogueLibraryService.Persist();
		}

		// Token: 0x06009795 RID: 38805 RVA: 0x003E3084 File Offset: 0x003E1284
		public static void ResetWeight(string setId)
		{
			DialogueLibraryService.EnsureLoaded();
			if (string.IsNullOrEmpty(setId))
			{
				return;
			}
			int num = DialogueLibraryService._delta.weightOverrides.RemoveAll((DialogueWeightOverride o) => o != null && o.setId == setId);
			DialogueLibraryService._weightOverrideMap.Remove(setId);
			if (num > 0)
			{
				DialogueLibraryService.Persist();
			}
		}

		// Token: 0x06009796 RID: 38806 RVA: 0x003E30E8 File Offset: 0x003E12E8
		public static string CreateCustom(PersonalityType personality, DialogueGender gender, DialogueCategory category, DialogueLine[] lines, int weight = 10, string displayName = null)
		{
			DialogueLibraryService.EnsureLoaded();
			CustomDialogueEntry customDialogueEntry = new CustomDialogueEntry
			{
				id = Guid.NewGuid().ToString("N"),
				displayName = (displayName ?? ""),
				personality = personality,
				gender = gender,
				category = category,
				weight = Mathf.Clamp(weight, 1, 100),
				lines = (lines ?? Array.Empty<DialogueLine>())
			};
			DialogueLibraryService._delta.customEntries.Add(customDialogueEntry);
			DialogueLibraryService.Persist();
			return customDialogueEntry.id;
		}

		// Token: 0x06009797 RID: 38807 RVA: 0x003E317C File Offset: 0x003E137C
		public static bool UpdateCustom(string id, PersonalityType personality, DialogueGender gender, DialogueCategory category, DialogueLine[] lines, int weight, string displayName = null)
		{
			DialogueLibraryService.EnsureLoaded();
			CustomDialogueEntry customDialogueEntry = DialogueLibraryService._delta.customEntries.FirstOrDefault<CustomDialogueEntry>((CustomDialogueEntry x) => x != null && x.id == id);
			if (customDialogueEntry == null)
			{
				Debug.LogWarning("[DialogueLibraryService] UpdateCustom: id '" + id + "' non e' un'entry custom (sistema = read-only).");
				return false;
			}
			if (displayName != null)
			{
				customDialogueEntry.displayName = displayName;
			}
			customDialogueEntry.personality = personality;
			customDialogueEntry.gender = gender;
			customDialogueEntry.category = category;
			customDialogueEntry.lines = lines ?? Array.Empty<DialogueLine>();
			customDialogueEntry.weight = Mathf.Clamp(weight, 1, 100);
			DialogueLibraryService.Persist();
			return true;
		}

		// Token: 0x06009798 RID: 38808 RVA: 0x003E3220 File Offset: 0x003E1420
		public static bool DeleteCustom(string id)
		{
			DialogueLibraryService.EnsureLoaded();
			if (DialogueLibraryService._delta.customEntries.RemoveAll((CustomDialogueEntry x) => x != null && x.id == id) == 0)
			{
				Debug.LogWarning("[DialogueLibraryService] DeleteCustom: id '" + id + "' non e' un'entry custom (sistema non cancellabile).");
				return false;
			}
			Func<string, bool> <>9__2;
			foreach (NpcDialogueAssignment npcDialogueAssignment in DialogueLibraryService._delta.npcAssignments)
			{
				if (npcDialogueAssignment != null && npcDialogueAssignment.entryIds != null && npcDialogueAssignment.entryIds.Contains(id))
				{
					NpcDialogueAssignment npcDialogueAssignment2 = npcDialogueAssignment;
					IEnumerable<string> entryIds = npcDialogueAssignment.entryIds;
					Func<string, bool> func;
					if ((func = <>9__2) == null)
					{
						func = (<>9__2 = (string e) => e != id);
					}
					npcDialogueAssignment2.entryIds = entryIds.Where<string>(func).ToArray<string>();
				}
			}
			DialogueLibraryService._delta.weightOverrides.RemoveAll((DialogueWeightOverride o) => o != null && o.setId == id);
			DialogueLibraryService._weightOverrideMap.Remove(id);
			DialogueLibraryService.Persist();
			return true;
		}

		// Token: 0x06009799 RID: 38809 RVA: 0x003E3344 File Offset: 0x003E1544
		private static CustomDialogueSet FindCustomSet(string setId)
		{
			if (string.IsNullOrEmpty(setId) || DialogueLibraryService._delta.customDocs == null)
			{
				return null;
			}
			Func<CustomDialogueSet, bool> <>9__0;
			foreach (CustomDialogueDoc customDialogueDoc in DialogueLibraryService._delta.customDocs)
			{
				if (((customDialogueDoc != null) ? customDialogueDoc.phases : null) != null)
				{
					foreach (CustomDialoguePhase customDialoguePhase in customDialogueDoc.phases)
					{
						if (((customDialoguePhase != null) ? customDialoguePhase.sets : null) != null)
						{
							IEnumerable<CustomDialogueSet> sets = customDialoguePhase.sets;
							Func<CustomDialogueSet, bool> func;
							if ((func = <>9__0) == null)
							{
								func = (<>9__0 = (CustomDialogueSet x) => x != null && x.id == setId);
							}
							CustomDialogueSet customDialogueSet = sets.FirstOrDefault<CustomDialogueSet>(func);
							if (customDialogueSet != null)
							{
								return customDialogueSet;
							}
						}
					}
				}
			}
			return null;
		}

		// Token: 0x0600979A RID: 38810 RVA: 0x003E345C File Offset: 0x003E165C
		private static CustomDialoguePhase GetOrAddPhase(CustomDialogueDoc doc, DialogueCategory category)
		{
			CustomDialoguePhase customDialoguePhase = doc.phases.FirstOrDefault<CustomDialoguePhase>((CustomDialoguePhase p) => p != null && p.category == category);
			if (customDialoguePhase == null)
			{
				customDialoguePhase = new CustomDialoguePhase
				{
					category = category
				};
				doc.phases.Add(customDialoguePhase);
			}
			return customDialoguePhase;
		}

		// Token: 0x0600979B RID: 38811 RVA: 0x003E34B0 File Offset: 0x003E16B0
		public static string CreateCustomDoc(string displayName, PersonalityType personality, DialogueGender gender, DialogueActorGroup actorGroup = DialogueActorGroup.Civilians, int macroWeight = 10)
		{
			DialogueLibraryService.EnsureLoaded();
			CustomDialogueDoc customDialogueDoc = new CustomDialogueDoc
			{
				id = Guid.NewGuid().ToString("N"),
				displayName = (displayName ?? ""),
				personality = personality,
				gender = gender,
				actorGroup = actorGroup,
				macroWeight = Mathf.Clamp(macroWeight, 1, 100),
				phases = new List<CustomDialoguePhase>()
			};
			DialogueLibraryService._delta.customDocs.Add(customDialogueDoc);
			DialogueLibraryService.Persist();
			return customDialogueDoc.id;
		}

		// Token: 0x0600979C RID: 38812 RVA: 0x003E353C File Offset: 0x003E173C
		public static string DuplicateToCustomDoc(string sourceId, string newName = null)
		{
			DialogueLibraryService.EnsureLoaded();
			DialogueDocView? dialogueDocById = DialogueLibraryService.GetDialogueDocById(sourceId);
			if (dialogueDocById == null)
			{
				return null;
			}
			DialogueDocView value = dialogueDocById.Value;
			CustomDialogueDoc customDialogueDoc = new CustomDialogueDoc
			{
				id = Guid.NewGuid().ToString("N"),
				displayName = (string.IsNullOrEmpty(newName) ? (value.DisplayLabel + " (Copy)") : newName),
				personality = value.personality,
				gender = value.gender,
				actorGroup = value.actorGroup,
				macroWeight = Mathf.Clamp(value.macroWeight, 1, 100),
				phases = new List<CustomDialoguePhase>()
			};
			foreach (DialoguePhaseView dialoguePhaseView in value.phases)
			{
				CustomDialoguePhase customDialoguePhase = new CustomDialoguePhase
				{
					category = dialoguePhaseView.category
				};
				foreach (DialogueAltView dialogueAltView in dialoguePhaseView.alternatives)
				{
					customDialoguePhase.sets.Add(new CustomDialogueSet
					{
						id = Guid.NewGuid().ToString("N"),
						weight = Mathf.Clamp(dialogueAltView.effectiveWeight, 1, 100),
						lines = DialogueLibraryService.CloneLines(dialogueAltView.lines)
					});
				}
				customDialogueDoc.phases.Add(customDialoguePhase);
			}
			DialogueLibraryService._delta.customDocs.Add(customDialogueDoc);
			DialogueLibraryService.Persist();
			return customDialogueDoc.id;
		}

		// Token: 0x0600979D RID: 38813 RVA: 0x003E36CC File Offset: 0x003E18CC
		private static DialogueLine[] CloneLines(DialogueLine[] src)
		{
			if (src == null)
			{
				return Array.Empty<DialogueLine>();
			}
			DialogueLine[] array = new DialogueLine[src.Length];
			for (int i = 0; i < src.Length; i++)
			{
				DialogueLine[] array2 = array;
				int num = i;
				DialogueLine dialogueLine = new DialogueLine();
				DialogueLine dialogueLine2 = src[i];
				dialogueLine.speakerName = ((dialogueLine2 != null) ? dialogueLine2.speakerName : null);
				DialogueLine dialogueLine3 = src[i];
				dialogueLine.text = ((dialogueLine3 != null) ? dialogueLine3.text : null);
				array2[num] = dialogueLine;
			}
			return array;
		}

		// Token: 0x0600979E RID: 38814 RVA: 0x003E372C File Offset: 0x003E192C
		public static bool RenameCustomDoc(string docId, string newName)
		{
			DialogueLibraryService.EnsureLoaded();
			CustomDialogueDoc customDialogueDoc = DialogueLibraryService.FindCustomDoc(docId);
			if (customDialogueDoc == null)
			{
				return false;
			}
			customDialogueDoc.displayName = newName ?? "";
			DialogueLibraryService.Persist();
			return true;
		}

		// Token: 0x0600979F RID: 38815 RVA: 0x003E3760 File Offset: 0x003E1960
		public static bool SetCustomDocBinding(string docId, PersonalityType personality, DialogueGender gender, DialogueActorGroup? actorGroup = null)
		{
			DialogueLibraryService.EnsureLoaded();
			CustomDialogueDoc customDialogueDoc = DialogueLibraryService.FindCustomDoc(docId);
			if (customDialogueDoc == null)
			{
				return false;
			}
			customDialogueDoc.personality = personality;
			customDialogueDoc.gender = gender;
			if (actorGroup != null)
			{
				customDialogueDoc.actorGroup = actorGroup.Value;
			}
			DialogueLibraryService.Persist();
			return true;
		}

		// Token: 0x060097A0 RID: 38816 RVA: 0x003E37A8 File Offset: 0x003E19A8
		public static bool DeleteCustomDoc(string docId)
		{
			DialogueLibraryService.EnsureLoaded();
			CustomDialogueDoc customDialogueDoc = DialogueLibraryService.FindCustomDoc(docId);
			if (customDialogueDoc == null)
			{
				return false;
			}
			HashSet<string> setIds = new HashSet<string>();
			if (customDialogueDoc.phases != null)
			{
				foreach (CustomDialoguePhase customDialoguePhase in customDialogueDoc.phases)
				{
					if (((customDialoguePhase != null) ? customDialoguePhase.sets : null) != null)
					{
						foreach (CustomDialogueSet customDialogueSet in customDialoguePhase.sets)
						{
							if (customDialogueSet != null && !string.IsNullOrEmpty(customDialogueSet.id))
							{
								setIds.Add(customDialogueSet.id);
							}
						}
					}
				}
			}
			DialogueLibraryService._delta.customDocs.Remove(customDialogueDoc);
			using (HashSet<string>.Enumerator enumerator3 = setIds.GetEnumerator())
			{
				while (enumerator3.MoveNext())
				{
					string sid = enumerator3.Current;
					DialogueLibraryService._delta.weightOverrides.RemoveAll((DialogueWeightOverride o) => o != null && o.setId == sid);
					DialogueLibraryService._weightOverrideMap.Remove(sid);
				}
			}
			Func<string, bool> <>9__1;
			foreach (NpcDialogueAssignment npcDialogueAssignment in DialogueLibraryService._delta.npcAssignments)
			{
				if (npcDialogueAssignment != null && npcDialogueAssignment.entryIds != null && npcDialogueAssignment.entryIds.Any<string>(new Func<string, bool>(setIds.Contains)))
				{
					NpcDialogueAssignment npcDialogueAssignment2 = npcDialogueAssignment;
					IEnumerable<string> entryIds = npcDialogueAssignment.entryIds;
					Func<string, bool> func;
					if ((func = <>9__1) == null)
					{
						func = (<>9__1 = (string e) => !setIds.Contains(e));
					}
					npcDialogueAssignment2.entryIds = entryIds.Where<string>(func).ToArray<string>();
				}
			}
			DialogueLibraryService.Persist();
			return true;
		}

		// Token: 0x060097A1 RID: 38817 RVA: 0x003E39C8 File Offset: 0x003E1BC8
		public static string AddSet(string docId, DialogueCategory category, int weight = 10)
		{
			DialogueLibraryService.EnsureLoaded();
			CustomDialogueDoc customDialogueDoc = DialogueLibraryService.FindCustomDoc(docId);
			if (customDialogueDoc == null)
			{
				return null;
			}
			CustomDialoguePhase orAddPhase = DialogueLibraryService.GetOrAddPhase(customDialogueDoc, category);
			CustomDialogueSet customDialogueSet = new CustomDialogueSet
			{
				id = Guid.NewGuid().ToString("N"),
				weight = Mathf.Clamp(weight, 1, 100),
				lines = Array.Empty<DialogueLine>()
			};
			orAddPhase.sets.Add(customDialogueSet);
			DialogueLibraryService.Persist();
			return customDialogueSet.id;
		}

		// Token: 0x060097A2 RID: 38818 RVA: 0x003E3A3C File Offset: 0x003E1C3C
		public static bool DeleteSet(string docId, string setId)
		{
			DialogueLibraryService.EnsureLoaded();
			CustomDialogueDoc customDialogueDoc = DialogueLibraryService.FindCustomDoc(docId);
			if (customDialogueDoc == null || customDialogueDoc.phases == null)
			{
				return false;
			}
			bool flag = false;
			Predicate<CustomDialogueSet> <>9__1;
			foreach (CustomDialoguePhase customDialoguePhase in customDialogueDoc.phases)
			{
				if (((customDialoguePhase != null) ? customDialoguePhase.sets : null) != null)
				{
					List<CustomDialogueSet> sets = customDialoguePhase.sets;
					Predicate<CustomDialogueSet> predicate;
					if ((predicate = <>9__1) == null)
					{
						predicate = (<>9__1 = (CustomDialogueSet s) => s != null && s.id == setId);
					}
					if (sets.RemoveAll(predicate) > 0)
					{
						flag = true;
					}
				}
			}
			if (!flag)
			{
				return false;
			}
			DialogueLibraryService._delta.weightOverrides.RemoveAll((DialogueWeightOverride o) => o != null && o.setId == setId);
			DialogueLibraryService._weightOverrideMap.Remove(setId);
			Func<string, bool> <>9__2;
			foreach (NpcDialogueAssignment npcDialogueAssignment in DialogueLibraryService._delta.npcAssignments)
			{
				if (npcDialogueAssignment != null && npcDialogueAssignment.entryIds != null && npcDialogueAssignment.entryIds.Contains(setId))
				{
					NpcDialogueAssignment npcDialogueAssignment2 = npcDialogueAssignment;
					IEnumerable<string> entryIds = npcDialogueAssignment.entryIds;
					Func<string, bool> func;
					if ((func = <>9__2) == null)
					{
						func = (<>9__2 = (string e) => e != setId);
					}
					npcDialogueAssignment2.entryIds = entryIds.Where<string>(func).ToArray<string>();
				}
			}
			DialogueLibraryService.Persist();
			return true;
		}

		// Token: 0x060097A3 RID: 38819 RVA: 0x003E3BC8 File Offset: 0x003E1DC8
		public static int AddLine(string docId, string setId, string text)
		{
			DialogueLibraryService.EnsureLoaded();
			CustomDialogueSet customDialogueSet = DialogueLibraryService.FindCustomSetInDoc(docId, setId);
			if (customDialogueSet == null)
			{
				return -1;
			}
			List<DialogueLine> list = new List<DialogueLine>(customDialogueSet.lines ?? Array.Empty<DialogueLine>());
			list.Add(new DialogueLine
			{
				text = (text ?? "")
			});
			customDialogueSet.lines = list.ToArray();
			DialogueLibraryService.Persist();
			return list.Count - 1;
		}

		// Token: 0x060097A4 RID: 38820 RVA: 0x003E3C30 File Offset: 0x003E1E30
		public static bool UpdateLine(string docId, string setId, int index, string text)
		{
			DialogueLibraryService.EnsureLoaded();
			CustomDialogueSet customDialogueSet = DialogueLibraryService.FindCustomSetInDoc(docId, setId);
			if (customDialogueSet == null || customDialogueSet.lines == null || index < 0 || index >= customDialogueSet.lines.Length)
			{
				return false;
			}
			if (customDialogueSet.lines[index] == null)
			{
				customDialogueSet.lines[index] = new DialogueLine();
			}
			customDialogueSet.lines[index].text = text ?? "";
			DialogueLibraryService.Persist();
			return true;
		}

		// Token: 0x060097A5 RID: 38821 RVA: 0x003E3C9C File Offset: 0x003E1E9C
		public static bool DeleteLine(string docId, string setId, int index)
		{
			DialogueLibraryService.EnsureLoaded();
			CustomDialogueSet customDialogueSet = DialogueLibraryService.FindCustomSetInDoc(docId, setId);
			if (customDialogueSet == null || customDialogueSet.lines == null || index < 0 || index >= customDialogueSet.lines.Length)
			{
				return false;
			}
			List<DialogueLine> list = new List<DialogueLine>(customDialogueSet.lines);
			list.RemoveAt(index);
			customDialogueSet.lines = list.ToArray();
			DialogueLibraryService.Persist();
			return true;
		}

		// Token: 0x060097A6 RID: 38822 RVA: 0x003E3CF8 File Offset: 0x003E1EF8
		public static bool ReorderLine(string docId, string setId, int from, int to)
		{
			DialogueLibraryService.EnsureLoaded();
			CustomDialogueSet customDialogueSet = DialogueLibraryService.FindCustomSetInDoc(docId, setId);
			if (customDialogueSet == null || customDialogueSet.lines == null)
			{
				return false;
			}
			int num = customDialogueSet.lines.Length;
			if (from < 0 || from >= num || to < 0 || to >= num || from == to)
			{
				return false;
			}
			List<DialogueLine> list = new List<DialogueLine>(customDialogueSet.lines);
			DialogueLine dialogueLine = list[from];
			list.RemoveAt(from);
			list.Insert(to, dialogueLine);
			customDialogueSet.lines = list.ToArray();
			DialogueLibraryService.Persist();
			return true;
		}

		// Token: 0x060097A7 RID: 38823 RVA: 0x003E3D74 File Offset: 0x003E1F74
		private static CustomDialogueSet FindCustomSetInDoc(string docId, string setId)
		{
			CustomDialogueDoc customDialogueDoc = DialogueLibraryService.FindCustomDoc(docId);
			if (((customDialogueDoc != null) ? customDialogueDoc.phases : null) == null)
			{
				return null;
			}
			Func<CustomDialogueSet, bool> <>9__0;
			foreach (CustomDialoguePhase customDialoguePhase in customDialogueDoc.phases)
			{
				if (((customDialoguePhase != null) ? customDialoguePhase.sets : null) != null)
				{
					IEnumerable<CustomDialogueSet> sets = customDialoguePhase.sets;
					Func<CustomDialogueSet, bool> func;
					if ((func = <>9__0) == null)
					{
						func = (<>9__0 = (CustomDialogueSet x) => x != null && x.id == setId);
					}
					CustomDialogueSet customDialogueSet = sets.FirstOrDefault<CustomDialogueSet>(func);
					if (customDialogueSet != null)
					{
						return customDialogueSet;
					}
				}
			}
			return null;
		}

		// Token: 0x060097A8 RID: 38824 RVA: 0x003E3E30 File Offset: 0x003E2030
		public static bool IsCustomDoc(string docId)
		{
			return DialogueLibraryService.FindCustomDoc(docId) != null;
		}

		// Token: 0x060097A9 RID: 38825 RVA: 0x003E3E3C File Offset: 0x003E203C
		public static List<DialogueCategory> GetMissingRequiredPhases(string docId)
		{
			DialogueLibraryService.EnsureLoaded();
			CustomDialogueDoc customDialogueDoc = DialogueLibraryService.FindCustomDoc(docId);
			DialogueCategory[] array = DialogueLibraryService.ExpectedPhases((customDialogueDoc != null) ? customDialogueDoc.actorGroup : DialogueActorGroup.Civilians);
			List<DialogueCategory> list = new List<DialogueCategory>();
			DialogueCategory[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				DialogueCategory cat = array2[i];
				CustomDialoguePhase customDialoguePhase;
				if (customDialogueDoc == null)
				{
					customDialoguePhase = null;
				}
				else
				{
					List<CustomDialoguePhase> phases = customDialogueDoc.phases;
					customDialoguePhase = ((phases != null) ? phases.FirstOrDefault<CustomDialoguePhase>((CustomDialoguePhase p) => p != null && p.category == cat) : null);
				}
				CustomDialoguePhase customDialoguePhase2 = customDialoguePhase;
				bool flag;
				if (customDialoguePhase2 != null && customDialoguePhase2.sets != null)
				{
					flag = customDialoguePhase2.sets.Any<CustomDialogueSet>(delegate(CustomDialogueSet s)
					{
						if (s != null && s.lines != null)
						{
							return s.lines.Any<DialogueLine>((DialogueLine l) => l != null && !string.IsNullOrWhiteSpace(l.text));
						}
						return false;
					});
				}
				else
				{
					flag = false;
				}
				if (!flag)
				{
					list.Add(cat);
				}
			}
			return list;
		}

		// Token: 0x060097AA RID: 38826 RVA: 0x003E3F04 File Offset: 0x003E2104
		public static bool IsCustomDocValid(string docId)
		{
			return DialogueLibraryService.GetMissingRequiredPhases(docId).Count == 0;
		}

		// Token: 0x060097AB RID: 38827 RVA: 0x003E3F14 File Offset: 0x003E2114
		public static string[] GetNpcWhitelist(string npcGuid)
		{
			DialogueLibraryService.EnsureLoaded();
			NpcDialogueAssignment npcDialogueAssignment = DialogueLibraryService._delta.npcAssignments.FirstOrDefault<NpcDialogueAssignment>((NpcDialogueAssignment x) => x != null && x.npcGuid == npcGuid);
			if (npcDialogueAssignment == null)
			{
				return null;
			}
			return npcDialogueAssignment.entryIds;
		}

		// Token: 0x060097AC RID: 38828 RVA: 0x003E3F5C File Offset: 0x003E215C
		public static void SetNpcWhitelist(string npcGuid, IEnumerable<string> entryIds)
		{
			DialogueLibraryService.EnsureLoaded();
			if (string.IsNullOrEmpty(npcGuid))
			{
				return;
			}
			string[] array;
			if (entryIds == null)
			{
				array = null;
			}
			else
			{
				array = entryIds.Where<string>((string s) => !string.IsNullOrEmpty(s)).Distinct<string>().ToArray<string>();
			}
			string[] array2 = array;
			DialogueLibraryService._delta.npcAssignments.RemoveAll((NpcDialogueAssignment x) => x != null && x.npcGuid == npcGuid);
			if (array2 != null && array2.Length != 0)
			{
				DialogueLibraryService._delta.npcAssignments.Add(new NpcDialogueAssignment
				{
					npcGuid = npcGuid,
					entryIds = array2
				});
			}
			DialogueLibraryService.Persist();
		}

		// Token: 0x060097AD RID: 38829 RVA: 0x003E4010 File Offset: 0x003E2210
		public static void ClearNpcWhitelist(string npcGuid)
		{
			DialogueLibraryService.EnsureLoaded();
			if (DialogueLibraryService._delta.npcAssignments.RemoveAll((NpcDialogueAssignment x) => x != null && x.npcGuid == npcGuid) > 0)
			{
				DialogueLibraryService.Persist();
			}
		}

		// Token: 0x060097AE RID: 38830 RVA: 0x003E4054 File Offset: 0x003E2254
		public static string[] GetDocSetIds(string docId)
		{
			DialogueLibraryService.EnsureLoaded();
			DialogueDocView? dialogueDocById = DialogueLibraryService.GetDialogueDocById(docId);
			if (dialogueDocById == null)
			{
				return Array.Empty<string>();
			}
			List<string> list = new List<string>();
			foreach (DialoguePhaseView dialoguePhaseView in dialogueDocById.Value.phases)
			{
				if (dialoguePhaseView.alternatives != null)
				{
					foreach (DialogueAltView dialogueAltView in dialoguePhaseView.alternatives)
					{
						if (!string.IsNullOrEmpty(dialogueAltView.id))
						{
							list.Add(dialogueAltView.id);
						}
					}
				}
			}
			return list.ToArray();
		}

		// Token: 0x060097AF RID: 38831 RVA: 0x003E40F7 File Offset: 0x003E22F7
		public static void AssignDocToNpc(string npcGuid, string docId)
		{
			if (string.IsNullOrEmpty(npcGuid))
			{
				return;
			}
			if (string.IsNullOrEmpty(docId))
			{
				DialogueLibraryService.ClearNpcWhitelist(npcGuid);
				return;
			}
			DialogueLibraryService.SetNpcWhitelist(npcGuid, DialogueLibraryService.GetDocSetIds(docId));
		}

		// Token: 0x060097B0 RID: 38832 RVA: 0x003E4120 File Offset: 0x003E2320
		public static string GetAssignedDocId(string npcGuid)
		{
			string[] npcWhitelist = DialogueLibraryService.GetNpcWhitelist(npcGuid);
			if (npcWhitelist == null || npcWhitelist.Length == 0)
			{
				return null;
			}
			HashSet<string> hashSet = new HashSet<string>(npcWhitelist);
			foreach (CustomDialogueDoc customDialogueDoc in DialogueLibraryService._delta.customDocs)
			{
				if (customDialogueDoc != null)
				{
					string[] docSetIds = DialogueLibraryService.GetDocSetIds(customDialogueDoc.id);
					if (docSetIds.Length == hashSet.Count && docSetIds.All<string>(new Func<string, bool>(hashSet.Contains)))
					{
						return customDialogueDoc.id;
					}
				}
			}
			return null;
		}

		// Token: 0x060097B1 RID: 38833 RVA: 0x003E41C8 File Offset: 0x003E23C8
		private static void Persist()
		{
			DialogueCustomizationStore.Save(DialogueLibraryService._delta);
			Action changed = DialogueLibraryService.Changed;
			if (changed == null)
			{
				return;
			}
			changed();
		}

		// Token: 0x04009CE9 RID: 40169
		private static List<DialogueLibraryService.SystemSet> _systemSets;

		// Token: 0x04009CEA RID: 40170
		private static DialogueCustomizationFile _delta;

		// Token: 0x04009CEB RID: 40171
		private static Dictionary<string, int> _weightOverrideMap;

		// Token: 0x04009CEC RID: 40172
		private static bool _loaded;

		// Token: 0x04009CEE RID: 40174
		private static Dictionary<string, int> _macroWeightOverrideMap;

		// Token: 0x04009CEF RID: 40175
		private static DialogueLibraryRegistrySO _registryCache;

		// Token: 0x04009CF0 RID: 40176
		[TupleElementNames(new string[] { "cat", "sets" })]
		private static readonly ValueTuple<DialogueCategory, Func<NPCDialogueProfileSO, DialogueSet[]>>[] PhaseMap = new ValueTuple<DialogueCategory, Func<NPCDialogueProfileSO, DialogueSet[]>>[]
		{
			new ValueTuple<DialogueCategory, Func<NPCDialogueProfileSO, DialogueSet[]>>(DialogueCategory.Detection, (NPCDialogueProfileSO p) => p.detectionDialogues),
			new ValueTuple<DialogueCategory, Func<NPCDialogueProfileSO, DialogueSet[]>>(DialogueCategory.Reaction, (NPCDialogueProfileSO p) => p.reactionDialogues),
			new ValueTuple<DialogueCategory, Func<NPCDialogueProfileSO, DialogueSet[]>>(DialogueCategory.Exhibit, (NPCDialogueProfileSO p) => p.exhibitDialogues),
			new ValueTuple<DialogueCategory, Func<NPCDialogueProfileSO, DialogueSet[]>>(DialogueCategory.Shame, (NPCDialogueProfileSO p) => p.shameDialogues),
			new ValueTuple<DialogueCategory, Func<NPCDialogueProfileSO, DialogueSet[]>>(DialogueCategory.Exit, (NPCDialogueProfileSO p) => p.exitDialogues),
			new ValueTuple<DialogueCategory, Func<NPCDialogueProfileSO, DialogueSet[]>>(DialogueCategory.Chasing, (NPCDialogueProfileSO p) => p.chasingDialogues)
		};

		// Token: 0x02001325 RID: 4901
		private sealed class SystemSet
		{
			// Token: 0x04009CF1 RID: 40177
			public string setId;

			// Token: 0x04009CF2 RID: 40178
			public string profileId;

			// Token: 0x04009CF3 RID: 40179
			public int macroWeight;

			// Token: 0x04009CF4 RID: 40180
			public PersonalityType personality;

			// Token: 0x04009CF5 RID: 40181
			public DialogueGender gender;

			// Token: 0x04009CF6 RID: 40182
			public DialogueCategory category;

			// Token: 0x04009CF7 RID: 40183
			public int defaultWeight;

			// Token: 0x04009CF8 RID: 40184
			public DialogueLine[] lines;

			// Token: 0x04009CF9 RID: 40185
			public bool excludeFromPool;

			// Token: 0x04009CFA RID: 40186
			public DialogueActorGroup actorGroup;

			// Token: 0x04009CFB RID: 40187
			public string displayName;
		}
	}
}
