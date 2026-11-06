using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BoZo.CharacterCreator;
using ProjectExhibitionist.CharacterCustomization.Core;
using UnityEngine;

namespace Bozo.AnimeCharacters
{
	// Token: 0x0200086E RID: 2158
	public class CharacterStateManager : MonoBehaviour
	{
		// Token: 0x17000A07 RID: 2567
		// (get) Token: 0x060039D6 RID: 14806 RVA: 0x00153D8C File Offset: 0x00151F8C
		// (set) Token: 0x060039D7 RID: 14807 RVA: 0x00153D93 File Offset: 0x00151F93
		public static CharacterStateManager Instance { get; private set; }

		// Token: 0x14000062 RID: 98
		// (add) Token: 0x060039D8 RID: 14808 RVA: 0x00153D9C File Offset: 0x00151F9C
		// (remove) Token: 0x060039D9 RID: 14809 RVA: 0x00153DD4 File Offset: 0x00151FD4
		public event Action<string, CharacterSubCategorySO, StyleItemSO> OnStyleChanged;

		// Token: 0x060039DA RID: 14810 RVA: 0x00153E0C File Offset: 0x0015200C
		private void Awake()
		{
			if (CharacterStateManager.Instance == null)
			{
				CharacterStateManager.Instance = this;
				this.categorySubcategoryStyles = new Dictionary<string, Dictionary<string, StyleItemSO>>();
				this.categoryPartStyles = new Dictionary<string, Dictionary<string, StyleItemSO>>();
				this.categorySubcategoryStyles["Head"] = new Dictionary<string, StyleItemSO>();
				this.categorySubcategoryStyles["Body"] = new Dictionary<string, StyleItemSO>();
				this.categoryPartStyles["Head"] = new Dictionary<string, StyleItemSO>();
				this.categoryPartStyles["Body"] = new Dictionary<string, StyleItemSO>();
				return;
			}
			Object.Destroy(base.gameObject);
		}

		// Token: 0x060039DB RID: 14811 RVA: 0x00153EA4 File Offset: 0x001520A4
		public void ConfigureRuntimeBindings(StyleManager runtimeStyleManager, CharacterGeneralInfoManager runtimeGeneralInfoManager, CharacterCreatorManager runtimeCharacterCreatorManager, CharactersManager runtimeCharactersManager, bool resetCachedReferences, bool allowFindFallbackForMissing)
		{
			if (resetCachedReferences)
			{
				this.ResetRuntimeReferenceCache();
			}
			this.styleManager = runtimeStyleManager;
			this.generalInfoManager = runtimeGeneralInfoManager;
			this.characterCreatorManager = runtimeCharacterCreatorManager;
			this.charactersManager = runtimeCharactersManager;
			this.FindReferencesIfNeeded(allowFindFallbackForMissing);
		}

		// Token: 0x060039DC RID: 14812 RVA: 0x00153ED5 File Offset: 0x001520D5
		public void SetActiveContext(CustomizationTargetType targetType)
		{
			this.activeTargetContext = targetType;
		}

		// Token: 0x17000A08 RID: 2568
		// (get) Token: 0x060039DD RID: 14813 RVA: 0x00153EDE File Offset: 0x001520DE
		public CustomizationTargetType ActiveTargetContext
		{
			get
			{
				return this.activeTargetContext;
			}
		}

		// Token: 0x060039DE RID: 14814 RVA: 0x00153EE6 File Offset: 0x001520E6
		public void ConfigureBlendShapeControllerForContext(CustomizationTargetType targetType, CharacterBlendShapeController controller)
		{
			if (targetType == CustomizationTargetType.MainCharacter)
			{
				this.mainCharacterBlendShapeController = controller;
				return;
			}
			if (targetType == CustomizationTargetType.Npc)
			{
				this.npcBlendShapeController = controller;
			}
		}

		// Token: 0x060039DF RID: 14815 RVA: 0x00153EFF File Offset: 0x001520FF
		public void ResetRuntimeReferenceCache()
		{
			this.characterCreatorManager = null;
			this.generalInfoManager = null;
			this.charactersManager = null;
		}

		// Token: 0x060039E0 RID: 14816 RVA: 0x00153F18 File Offset: 0x00152118
		public void SetSubcategoryStyle(string category, CharacterSubCategorySO subcategory, StyleItemSO style)
		{
			if (subcategory == null)
			{
				Debug.LogError("Cannot set style for null subcategory");
				return;
			}
			if (!this.categorySubcategoryStyles.ContainsKey(category))
			{
				this.categorySubcategoryStyles[category] = new Dictionary<string, StyleItemSO>();
			}
			this.categorySubcategoryStyles[category][subcategory.subCategoryName] = style;
			Action<string, CharacterSubCategorySO, StyleItemSO> onStyleChanged = this.OnStyleChanged;
			if (onStyleChanged != null)
			{
				onStyleChanged(category, subcategory, style);
			}
			if (this.styleManager != null && style != null)
			{
				this.styleManager.ApplyStyle(style);
			}
			Debug.Log(string.Concat(new string[]
			{
				"Set style ",
				((style != null) ? style.itemName : null) ?? "null",
				" for category ",
				category,
				", subcategory ",
				subcategory.subCategoryName
			}));
		}

		// Token: 0x060039E1 RID: 14817 RVA: 0x00153FF4 File Offset: 0x001521F4
		public void SetPartStyle(string category, CharacterSubCategorySO subcategory, CharacterPartSO part, StyleItemSO style)
		{
			if (part == null)
			{
				Debug.LogError("Cannot set style for null part");
				return;
			}
			if (!this.categoryPartStyles.ContainsKey(category))
			{
				this.categoryPartStyles[category] = new Dictionary<string, StyleItemSO>();
			}
			string text = subcategory.subCategoryName + "." + part.partName;
			this.categoryPartStyles[category][text] = style;
			if (this.styleManager != null && style != null)
			{
				this.styleManager.ApplyStyle(style);
			}
			Debug.Log(string.Concat(new string[]
			{
				"Set style ",
				((style != null) ? style.itemName : null) ?? "null",
				" for category ",
				category,
				", subcategory ",
				subcategory.subCategoryName,
				", part ",
				part.partName
			}));
		}

		// Token: 0x060039E2 RID: 14818 RVA: 0x001540E4 File Offset: 0x001522E4
		public StyleItemSO GetSubcategoryStyle(string category, CharacterSubCategorySO subcategory)
		{
			if (subcategory == null)
			{
				return null;
			}
			if (!this.categorySubcategoryStyles.ContainsKey(category))
			{
				return null;
			}
			StyleItemSO styleItemSO;
			if (this.categorySubcategoryStyles[category].TryGetValue(subcategory.subCategoryName, out styleItemSO))
			{
				return styleItemSO;
			}
			return null;
		}

		// Token: 0x060039E3 RID: 14819 RVA: 0x0015412C File Offset: 0x0015232C
		public StyleItemSO GetPartStyle(string category, CharacterSubCategorySO subcategory, CharacterPartSO part)
		{
			if (part == null || subcategory == null)
			{
				return null;
			}
			if (!this.categoryPartStyles.ContainsKey(category))
			{
				return null;
			}
			string text = subcategory.subCategoryName + "." + part.partName;
			StyleItemSO styleItemSO;
			if (this.categoryPartStyles[category].TryGetValue(text, out styleItemSO))
			{
				return styleItemSO;
			}
			return null;
		}

		// Token: 0x060039E4 RID: 14820 RVA: 0x0015418D File Offset: 0x0015238D
		public StyleItemSO GetStyleForType(OutfitType type)
		{
			if (this.styleManager != null)
			{
				return this.styleManager.GetCurrentStyle(type);
			}
			return null;
		}

		// Token: 0x060039E5 RID: 14821 RVA: 0x001541AC File Offset: 0x001523AC
		public bool IsStyleSelected(StyleItemSO style)
		{
			if (style == null)
			{
				return false;
			}
			foreach (string text in this.categorySubcategoryStyles.Keys)
			{
				foreach (KeyValuePair<string, StyleItemSO> keyValuePair in this.categorySubcategoryStyles[text])
				{
					if (keyValuePair.Value == style)
					{
						return true;
					}
				}
			}
			foreach (string text2 in this.categoryPartStyles.Keys)
			{
				foreach (KeyValuePair<string, StyleItemSO> keyValuePair2 in this.categoryPartStyles[text2])
				{
					if (keyValuePair2.Value == style)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060039E6 RID: 14822 RVA: 0x001542FC File Offset: 0x001524FC
		public void ResetSubcategoryStyle(string category, CharacterSubCategorySO subcategory)
		{
			if (subcategory == null)
			{
				return;
			}
			if (!this.categorySubcategoryStyles.ContainsKey(category))
			{
				return;
			}
			if (this.categorySubcategoryStyles[category].ContainsKey(subcategory.subCategoryName))
			{
				StyleItemSO styleItemSO = this.categorySubcategoryStyles[category][subcategory.subCategoryName];
				this.categorySubcategoryStyles[category].Remove(subcategory.subCategoryName);
				if (this.styleManager != null && styleItemSO != null)
				{
					this.styleManager.RemoveStyle(styleItemSO.itemType);
				}
				Action<string, CharacterSubCategorySO, StyleItemSO> onStyleChanged = this.OnStyleChanged;
				if (onStyleChanged != null)
				{
					onStyleChanged(category, subcategory, null);
				}
				Debug.Log("Reset style for category " + category + ", subcategory " + subcategory.subCategoryName);
			}
		}

		// Token: 0x060039E7 RID: 14823 RVA: 0x001543C8 File Offset: 0x001525C8
		public void ResetPartStyle(string category, CharacterSubCategorySO subcategory, CharacterPartSO part)
		{
			if (subcategory == null || part == null)
			{
				return;
			}
			if (!this.categoryPartStyles.ContainsKey(category))
			{
				return;
			}
			string text = subcategory.subCategoryName + "." + part.partName;
			if (this.categoryPartStyles[category].ContainsKey(text))
			{
				StyleItemSO styleItemSO = this.categoryPartStyles[category][text];
				this.categoryPartStyles[category].Remove(text);
				if (this.styleManager != null && styleItemSO != null)
				{
					this.styleManager.RemoveStyle(styleItemSO.itemType);
				}
				Debug.Log(string.Concat(new string[] { "Reset style for category ", category, ", subcategory ", subcategory.subCategoryName, ", part ", part.partName }));
			}
		}

		// Token: 0x060039E8 RID: 14824 RVA: 0x001544B4 File Offset: 0x001526B4
		public void SaveState()
		{
			string text = JsonUtility.ToJson(this.ConvertToCharacterStateData(), true);
			GameDataPaths.EnsureRuntimeDirectories();
			File.WriteAllText(GameDataPaths.CharacterStateFilePath, text);
			Debug.Log("Stato salvato");
		}

		// Token: 0x060039E9 RID: 14825 RVA: 0x001544E8 File Offset: 0x001526E8
		public void LoadState()
		{
			string characterStateFilePath = GameDataPaths.CharacterStateFilePath;
			if (File.Exists(characterStateFilePath))
			{
				CharacterStateData characterStateData = JsonUtility.FromJson<CharacterStateData>(File.ReadAllText(characterStateFilePath));
				this.LoadFromCharacterStateData(characterStateData);
				Debug.Log("Stato caricato");
				return;
			}
			Debug.Log("Nessuno stato salvato trovato");
		}

		// Token: 0x060039EA RID: 14826 RVA: 0x0015452B File Offset: 0x0015272B
		public CharacterStateData ConvertToCharacterStateData()
		{
			return this.ConvertToCharacterStateData(this.activeTargetContext);
		}

		// Token: 0x060039EB RID: 14827 RVA: 0x0015453C File Offset: 0x0015273C
		public CharacterStateData ConvertToCharacterStateData(CustomizationTargetType targetType)
		{
			CharacterStateData characterStateData = new CharacterStateData();
			foreach (string text in this.categorySubcategoryStyles.Keys)
			{
				foreach (KeyValuePair<string, StyleItemSO> keyValuePair in this.categorySubcategoryStyles[text])
				{
					if (keyValuePair.Value != null)
					{
						characterStateData.subcategoryStyles.Add(new StyleSelection(text, keyValuePair.Key, keyValuePair.Value.itemName, ""));
					}
				}
			}
			foreach (string text2 in this.categoryPartStyles.Keys)
			{
				foreach (KeyValuePair<string, StyleItemSO> keyValuePair2 in this.categoryPartStyles[text2])
				{
					if (keyValuePair2.Value != null)
					{
						string[] array = keyValuePair2.Key.Split('.', StringSplitOptions.None);
						if (array.Length >= 2)
						{
							characterStateData.partStyles.Add(new StyleSelection(text2, array[0], keyValuePair2.Value.itemName, array[1]));
						}
					}
				}
			}
			this.CaptureBlendShapeState(characterStateData, targetType);
			return characterStateData;
		}

		// Token: 0x060039EC RID: 14828 RVA: 0x001546F0 File Offset: 0x001528F0
		private void CaptureBlendShapeState(CharacterStateData stateData, CustomizationTargetType targetType)
		{
			if (stateData == null)
			{
				return;
			}
			CharacterBlendShapeController characterBlendShapeController = this.ResolveBlendShapeController(targetType);
			if (characterBlendShapeController == null || characterBlendShapeController.BlendShapeMap == null)
			{
				return;
			}
			List<BlendShapeMapSO.BlendShapeEntry> blendShapeMappings = characterBlendShapeController.BlendShapeMap.blendShapeMappings;
			if (blendShapeMappings == null || blendShapeMappings.Count == 0)
			{
				return;
			}
			for (int i = 0; i < blendShapeMappings.Count; i++)
			{
				BlendShapeMapSO.BlendShapeEntry blendShapeEntry = blendShapeMappings[i];
				if (blendShapeEntry != null && !string.IsNullOrWhiteSpace(blendShapeEntry.displayName))
				{
					float entryUiValue = characterBlendShapeController.GetEntryUiValue(blendShapeEntry);
					stateData.blendShapes.Add(new BlendShapeValue(blendShapeEntry.category, blendShapeEntry.subcategory, blendShapeEntry.displayName, entryUiValue));
				}
			}
		}

		// Token: 0x060039ED RID: 14829 RVA: 0x0015478F File Offset: 0x0015298F
		public void LoadFromCharacterStateData(CharacterStateData stateData)
		{
			this.LoadFromCharacterStateData(stateData, this.activeTargetContext);
		}

		// Token: 0x060039EE RID: 14830 RVA: 0x001547A0 File Offset: 0x001529A0
		public void LoadFromCharacterStateData(CharacterStateData stateData, CustomizationTargetType targetType)
		{
			if (stateData == null)
			{
				return;
			}
			this.categorySubcategoryStyles.Clear();
			this.categoryPartStyles.Clear();
			this.categorySubcategoryStyles["Head"] = new Dictionary<string, StyleItemSO>();
			this.categorySubcategoryStyles["Body"] = new Dictionary<string, StyleItemSO>();
			this.categorySubcategoryStyles["Outfit"] = new Dictionary<string, StyleItemSO>();
			this.categoryPartStyles["Head"] = new Dictionary<string, StyleItemSO>();
			this.categoryPartStyles["Body"] = new Dictionary<string, StyleItemSO>();
			this.categoryPartStyles["Outfit"] = new Dictionary<string, StyleItemSO>();
			foreach (StyleSelection styleSelection in stateData.subcategoryStyles)
			{
				Debug.Log(string.Concat(new string[] { "Loading subcategory style: ", styleSelection.category, "/", styleSelection.subcategory, " -> ", styleSelection.styleItemName }));
			}
			foreach (StyleSelection styleSelection2 in stateData.partStyles)
			{
				Debug.Log(string.Concat(new string[] { "Loading part style: ", styleSelection2.category, "/", styleSelection2.subcategory, "/", styleSelection2.part, " -> ", styleSelection2.styleItemName }));
			}
			this.ApplyBlendShapesFromState(stateData, targetType);
		}

		// Token: 0x060039EF RID: 14831 RVA: 0x00154960 File Offset: 0x00152B60
		private void ApplyBlendShapesFromState(CharacterStateData stateData, CustomizationTargetType targetType)
		{
			if (stateData == null || stateData.blendShapes == null || stateData.blendShapes.Count == 0)
			{
				return;
			}
			CharacterBlendShapeController characterBlendShapeController = this.ResolveBlendShapeController(targetType);
			if (characterBlendShapeController == null || characterBlendShapeController.BlendShapeMap == null)
			{
				return;
			}
			List<BlendShapeMapSO.BlendShapeEntry> blendShapeMappings = characterBlendShapeController.BlendShapeMap.blendShapeMappings;
			if (blendShapeMappings == null || blendShapeMappings.Count == 0)
			{
				return;
			}
			for (int i = 0; i < stateData.blendShapes.Count; i++)
			{
				BlendShapeValue blendShapeValue = stateData.blendShapes[i];
				if (blendShapeValue != null && !string.IsNullOrWhiteSpace(blendShapeValue.blendShapeName))
				{
					BlendShapeMapSO.BlendShapeEntry blendShapeEntry = CharacterStateManager.FindBlendShapeEntry(blendShapeMappings, blendShapeValue);
					if (blendShapeEntry != null)
					{
						characterBlendShapeController.ApplyEntryValue(blendShapeEntry, blendShapeValue.value);
					}
					else
					{
						characterBlendShapeController.SetBlendShapeValue(blendShapeValue.blendShapeName, blendShapeValue.value);
					}
				}
			}
		}

		// Token: 0x060039F0 RID: 14832 RVA: 0x00154A20 File Offset: 0x00152C20
		private static BlendShapeMapSO.BlendShapeEntry FindBlendShapeEntry(List<BlendShapeMapSO.BlendShapeEntry> mappings, BlendShapeValue blendShape)
		{
			for (int i = 0; i < mappings.Count; i++)
			{
				BlendShapeMapSO.BlendShapeEntry blendShapeEntry = mappings[i];
				if (blendShapeEntry != null && string.Equals(blendShapeEntry.displayName, blendShape.blendShapeName, StringComparison.OrdinalIgnoreCase) && (string.IsNullOrWhiteSpace(blendShape.category) || string.Equals(blendShapeEntry.category, blendShape.category, StringComparison.OrdinalIgnoreCase)) && (string.IsNullOrWhiteSpace(blendShape.subcategory) || string.Equals(blendShapeEntry.subcategory, blendShape.subcategory, StringComparison.OrdinalIgnoreCase)))
				{
					return blendShapeEntry;
				}
			}
			return null;
		}

		// Token: 0x060039F1 RID: 14833 RVA: 0x00154AA8 File Offset: 0x00152CA8
		private CharacterBlendShapeController ResolveBlendShapeController(CustomizationTargetType targetType)
		{
			if (targetType == CustomizationTargetType.MainCharacter)
			{
				if (this.mainCharacterBlendShapeController != null)
				{
					return this.mainCharacterBlendShapeController;
				}
				if (this.blendShapeController != null)
				{
					return this.blendShapeController;
				}
			}
			else if (targetType == CustomizationTargetType.Npc && this.npcBlendShapeController != null)
			{
				return this.npcBlendShapeController;
			}
			if (this.blendShapeController == null)
			{
				this.blendShapeController = Object.FindObjectOfType<CharacterBlendShapeController>(true);
			}
			return this.blendShapeController;
		}

		// Token: 0x060039F2 RID: 14834 RVA: 0x00154B1C File Offset: 0x00152D1C
		public void SetHeadSubcategoryStyle(CharacterSubCategorySO subcategory, StyleItemSO style)
		{
			this.SetSubcategoryStyle("Head", subcategory, style);
		}

		// Token: 0x060039F3 RID: 14835 RVA: 0x00154B2C File Offset: 0x00152D2C
		public void SetHeadPartStyle(CharacterPartSO part, StyleItemSO style)
		{
			CharacterSubCategorySO characterSubCategorySO = null;
			foreach (string text in this.categorySubcategoryStyles.Keys)
			{
				using (Dictionary<string, StyleItemSO>.KeyCollection.Enumerator enumerator2 = this.categorySubcategoryStyles[text].Keys.GetEnumerator())
				{
					if (enumerator2.MoveNext())
					{
						string text2 = enumerator2.Current;
						characterSubCategorySO = new CharacterSubCategorySO();
					}
				}
				if (characterSubCategorySO != null)
				{
					break;
				}
			}
			if (characterSubCategorySO != null)
			{
				this.SetPartStyle("Head", characterSubCategorySO, part, style);
				return;
			}
			Debug.LogWarning("Could not find subcategory for part: " + part.partName);
		}

		// Token: 0x060039F4 RID: 14836 RVA: 0x00154C08 File Offset: 0x00152E08
		public StyleItemSO GetHeadSubcategoryStyle(CharacterSubCategorySO subcategory)
		{
			return this.GetSubcategoryStyle("Head", subcategory);
		}

		// Token: 0x060039F5 RID: 14837 RVA: 0x00154C18 File Offset: 0x00152E18
		public StyleItemSO GetHeadPartStyle(CharacterPartSO part)
		{
			CharacterSubCategorySO characterSubCategorySO = null;
			foreach (string text in this.categorySubcategoryStyles.Keys)
			{
				using (Dictionary<string, StyleItemSO>.KeyCollection.Enumerator enumerator2 = this.categorySubcategoryStyles[text].Keys.GetEnumerator())
				{
					if (enumerator2.MoveNext())
					{
						string text2 = enumerator2.Current;
						characterSubCategorySO = new CharacterSubCategorySO();
					}
				}
				if (characterSubCategorySO != null)
				{
					break;
				}
			}
			if (characterSubCategorySO != null)
			{
				return this.GetPartStyle("Head", characterSubCategorySO, part);
			}
			Debug.LogWarning("Could not find subcategory for part: " + part.partName);
			return null;
		}

		// Token: 0x060039F6 RID: 14838 RVA: 0x00154CF4 File Offset: 0x00152EF4
		public void ResetHeadSubcategoryStyle(CharacterSubCategorySO subcategory)
		{
			this.ResetSubcategoryStyle("Head", subcategory);
		}

		// Token: 0x060039F7 RID: 14839 RVA: 0x00154D04 File Offset: 0x00152F04
		public void ResetHeadPartStyle(CharacterPartSO part)
		{
			CharacterSubCategorySO characterSubCategorySO = null;
			foreach (string text in this.categorySubcategoryStyles.Keys)
			{
				using (Dictionary<string, StyleItemSO>.KeyCollection.Enumerator enumerator2 = this.categorySubcategoryStyles[text].Keys.GetEnumerator())
				{
					if (enumerator2.MoveNext())
					{
						string text2 = enumerator2.Current;
						characterSubCategorySO = new CharacterSubCategorySO();
					}
				}
				if (characterSubCategorySO != null)
				{
					break;
				}
			}
			if (characterSubCategorySO != null)
			{
				this.ResetPartStyle("Head", characterSubCategorySO, part);
				return;
			}
			Debug.LogWarning("Could not find subcategory for part: " + part.partName);
		}

		// Token: 0x060039F8 RID: 14840 RVA: 0x00154DDC File Offset: 0x00152FDC
		public void SaveCurrentCharacter(string characterName)
		{
			if (string.IsNullOrEmpty((characterName != null) ? characterName.Trim() : null))
			{
				Debug.LogError("Cannot save character: name is empty");
				return;
			}
			base.StartCoroutine(this.SaveCharacterCoroutine(characterName));
		}

		// Token: 0x060039F9 RID: 14841 RVA: 0x00154E0A File Offset: 0x0015300A
		private IEnumerator SaveCharacterCoroutine(string characterName)
		{
			Debug.Log("Starting save process for character: " + characterName);
			this.FindReferencesIfNeeded(true);
			CharacterDataExtended characterDataExtended = this.ResolveCharacterByName(characterName);
			if (characterDataExtended == null && this.charactersManager != null)
			{
				characterDataExtended = this.charactersManager.CreateNewCharacter(characterName, false);
			}
			CharacterDataExtended characterData = this.GetCharacterDataFromGeneralInfo(characterName, characterDataExtended);
			if (string.IsNullOrWhiteSpace(characterData.characterId))
			{
				characterData.characterId = Guid.NewGuid().ToString("N");
			}
			string storageKey = characterData.characterId;
			Debug.Log(string.Format("CharacterCreatorManager found: {0}", this.characterCreatorManager != null));
			if (this.characterCreatorManager != null)
			{
				Debug.Log(string.Format("CharacterCreatorManager.Character found: {0}", this.characterCreatorManager.Character != null));
			}
			if (this.characterCreatorManager != null)
			{
				if (this.characterCreatorManager.Character == null)
				{
					Debug.LogError("CharacterCreatorManager.Character (OutfitSystem) is not assigned! Please assign it in the Inspector.");
					Action<string, bool> onCharacterSaved = this.OnCharacterSaved;
					if (onCharacterSaved != null)
					{
						onCharacterSaved(characterName, false);
					}
					yield break;
				}
				if (this.characterCreatorManager.IconTexture == null)
				{
					Debug.LogWarning("CharacterCreatorManager.IconTexture is not assigned. Character will be saved without icon.");
				}
				yield return new WaitForEndOfFrame();
				try
				{
					Texture2D texture2D = this.characterCreatorManager.GenerateCharacterIcon();
					Debug.Log("Generated character icon: " + ((texture2D != null) ? string.Format("{0}x{1}", texture2D.width, texture2D.height) : "null"));
					CharacterGeneralInfo characterGeneralInfo = null;
					if (this.generalInfoManager != null)
					{
						characterGeneralInfo = new CharacterGeneralInfo
						{
							characterName = (this.generalInfoManager.GetCurrentName() ?? ""),
							surname = (this.generalInfoManager.GetCurrentSurname() ?? ""),
							description = (this.generalInfoManager.GetCurrentDescription() ?? ""),
							attitude = (this.generalInfoManager.GetCurrentAttitude() ?? ""),
							birthday = (this.generalInfoManager.GetCurrentBirthday() ?? "")
						};
						Debug.Log("GeneralInfo for BMAC_SaveSystem - Birthday: " + characterGeneralInfo.birthday);
					}
					BMAC_SaveSystem.SaveCharacter(this.characterCreatorManager.Character, storageKey, texture2D, characterGeneralInfo);
					Debug.Log(string.Format("BMAC_SaveSystem.SaveCharacter called with icon: {0} and generalInfo: {1}", texture2D != null, characterGeneralInfo != null));
					if (texture2D != null)
					{
						this.SaveCharacterIconAsPng(storageKey, texture2D);
					}
					if (texture2D != null)
					{
						characterData.icon = Sprite.Create(texture2D, new Rect(0f, 0f, (float)texture2D.width, (float)texture2D.height), Vector2.one * 0.5f);
					}
					if (this.charactersManager != null)
					{
						this.charactersManager.UpdateCharacter(characterData);
						Debug.Log("Character saved to CharactersManager: " + characterData.characterName);
					}
					Debug.Log("Character '" + characterName + "' saved successfully!");
					Action<string, bool> onCharacterSaved2 = this.OnCharacterSaved;
					if (onCharacterSaved2 != null)
					{
						onCharacterSaved2(characterName, true);
					}
					if (this.charactersManager != null)
					{
						this.charactersManager.OnCharacterSavedExternal();
					}
					yield break;
				}
				catch (Exception ex)
				{
					Debug.LogError("Failed to save character '" + characterName + "': " + ex.Message);
					Action<string, bool> onCharacterSaved3 = this.OnCharacterSaved;
					if (onCharacterSaved3 != null)
					{
						onCharacterSaved3(characterName, false);
					}
					yield break;
				}
			}
			Debug.LogError("CharacterCreatorManager or character not found!");
			Action<string, bool> onCharacterSaved4 = this.OnCharacterSaved;
			if (onCharacterSaved4 != null)
			{
				onCharacterSaved4(characterName, false);
			}
			yield break;
		}

		// Token: 0x060039FA RID: 14842 RVA: 0x00154E20 File Offset: 0x00153020
		public void LoadCharacterById(string characterId)
		{
			if (string.IsNullOrWhiteSpace(characterId))
			{
				Debug.LogWarning("[CharacterStateManager] Cannot load character by id: id is empty.");
				return;
			}
			CharacterDataExtended characterDataExtended = this.GetAllCharacters().FirstOrDefault<CharacterDataExtended>((CharacterDataExtended c) => c.characterId == characterId);
			if (characterDataExtended == null)
			{
				Debug.LogWarning("[CharacterStateManager] Character with id '" + characterId + "' not found.");
				return;
			}
			this.LoadCharacter(characterDataExtended.characterName);
		}

		// Token: 0x060039FB RID: 14843 RVA: 0x00154E94 File Offset: 0x00153094
		public async void LoadCharacter(string characterName)
		{
			try
			{
				Debug.Log("[CharacterStateManager] Loading character: " + characterName);
				this.FindReferencesIfNeeded(true);
				OutfitSystem targetCharacter = this.GetTargetCharacter();
				if (targetCharacter != null)
				{
					try
					{
						CharacterDataExtended characterDataExtended = this.ResolveCharacterByName(characterName);
						string text = ((!string.IsNullOrWhiteSpace((characterDataExtended != null) ? characterDataExtended.characterId : null)) ? characterDataExtended.characterId : characterName);
						CharacterData characterData = null;
						if (characterDataExtended != null)
						{
							characterData = this.ConvertToBMACData(characterDataExtended);
						}
						if (characterData == null)
						{
							characterData = BMAC_SaveSystem.GetDataFromID(text);
						}
						if (characterData == null)
						{
							Debug.LogWarning("[CharacterStateManager] Could not find BMAC data for: " + characterName);
						}
						else
						{
							await BMAC_SaveSystem.LoadCharacter(targetCharacter, characterData, false, true);
							Debug.Log("[CharacterStateManager] Character loaded from BMAC system: " + characterName);
						}
						goto IL_0172;
					}
					catch (ArgumentException ex)
					{
						Debug.LogWarning("[CharacterStateManager] ArgumentException during BMAC load (likely duplicate keys): " + ex.Message);
						Debug.LogWarning("[CharacterStateManager] Continuing with partial load...");
						goto IL_0172;
					}
					catch (Exception ex2)
					{
						Debug.LogError("[CharacterStateManager] BMAC load failed: " + ex2.Message);
						throw;
					}
				}
				Debug.LogError("[CharacterStateManager] No OutfitSystem found for character loading! Please ensure CharacterCreatorManager.Character is assigned.");
				IL_0172:
				if (this.generalInfoManager != null)
				{
					CharacterDataExtended characterDataExtended2 = this.ResolveCharacterByName(characterName);
					string text2 = ((!string.IsNullOrWhiteSpace((characterDataExtended2 != null) ? characterDataExtended2.characterId : null)) ? characterDataExtended2.characterId : characterName);
					string text3 = Path.Combine(BMAC_SaveSystem.filePath, text2 + ".json");
					if (File.Exists(text3))
					{
						try
						{
							CharacterData characterData2 = JsonUtility.FromJson<CharacterData>(File.ReadAllText(text3));
							if (characterData2 != null && characterData2.generalInfo != null)
							{
								this.generalInfoManager.SetCurrentName(characterData2.generalInfo.characterName ?? "");
								this.generalInfoManager.SetCurrentSurname(characterData2.generalInfo.surname ?? "");
								this.generalInfoManager.SetCurrentDescription(characterData2.generalInfo.description ?? "");
								this.generalInfoManager.SetCurrentAttitude(characterData2.generalInfo.attitude ?? "");
								this.generalInfoManager.SetCurrentBirthday(characterData2.generalInfo.birthday ?? "");
								Debug.Log("[CharacterStateManager] General info loaded from BMAC JSON for: " + characterName + " - Birthday: " + characterData2.generalInfo.birthday);
								if (characterDataExtended2 != null)
								{
									this.generalInfoManager.LoadCharacterGeneralInfo(characterDataExtended2);
									Debug.Log("[CharacterStateManager] Updated currentCharacter in GeneralInfoManager");
								}
							}
							else
							{
								Debug.LogWarning("[CharacterStateManager] No general info found in BMAC JSON for: " + characterName);
							}
							goto IL_0321;
						}
						catch (Exception ex3)
						{
							Debug.LogError("[CharacterStateManager] Failed to load general info from BMAC JSON: " + ex3.Message);
							goto IL_0321;
						}
					}
					Debug.LogWarning("[CharacterStateManager] BMAC JSON file not found: " + text3);
				}
				IL_0321:
				this.UpdateCharacterName();
				Action<string, bool> onCharacterLoaded = this.OnCharacterLoaded;
				if (onCharacterLoaded != null)
				{
					onCharacterLoaded(characterName, true);
				}
				Debug.Log("[CharacterStateManager] Character '" + characterName + "' loaded successfully!");
			}
			catch (Exception ex4)
			{
				Debug.LogError("[CharacterStateManager] Failed to load character '" + characterName + "': " + ex4.Message);
				Debug.LogError("[CharacterStateManager] Stack trace: " + ex4.StackTrace);
				Action<string, bool> onCharacterLoaded2 = this.OnCharacterLoaded;
				if (onCharacterLoaded2 != null)
				{
					onCharacterLoaded2(characterName, false);
				}
			}
		}

		// Token: 0x060039FC RID: 14844 RVA: 0x00154ED4 File Offset: 0x001530D4
		public void CreateNewCharacter()
		{
			Debug.Log("Creating new character...");
			this.FindReferencesIfNeeded(true);
			this.ResetCharacterName();
			if (this.generalInfoManager != null)
			{
				this.generalInfoManager.ResetToDefaults();
			}
			if (this.characterCreatorManager != null)
			{
				this.characterCreatorManager.ResetCharacter();
			}
			this.ResetAllStyles();
			Debug.Log("New character created and systems reset");
			Action onNewCharacterCreated = this.OnNewCharacterCreated;
			if (onNewCharacterCreated == null)
			{
				return;
			}
			onNewCharacterCreated();
		}

		// Token: 0x14000063 RID: 99
		// (add) Token: 0x060039FD RID: 14845 RVA: 0x00154F4C File Offset: 0x0015314C
		// (remove) Token: 0x060039FE RID: 14846 RVA: 0x00154F84 File Offset: 0x00153184
		public event Action<string, bool> OnCharacterSaved;

		// Token: 0x14000064 RID: 100
		// (add) Token: 0x060039FF RID: 14847 RVA: 0x00154FBC File Offset: 0x001531BC
		// (remove) Token: 0x06003A00 RID: 14848 RVA: 0x00154FF4 File Offset: 0x001531F4
		public event Action<string, bool> OnCharacterLoaded;

		// Token: 0x14000065 RID: 101
		// (add) Token: 0x06003A01 RID: 14849 RVA: 0x0015502C File Offset: 0x0015322C
		// (remove) Token: 0x06003A02 RID: 14850 RVA: 0x00155064 File Offset: 0x00153264
		public event Action OnNewCharacterCreated;

		// Token: 0x14000066 RID: 102
		// (add) Token: 0x06003A03 RID: 14851 RVA: 0x0015509C File Offset: 0x0015329C
		// (remove) Token: 0x06003A04 RID: 14852 RVA: 0x001550D4 File Offset: 0x001532D4
		public event Action<string> OnCharacterNameChanged;

		// Token: 0x06003A05 RID: 14853 RVA: 0x0015510C File Offset: 0x0015330C
		private void FindReferencesIfNeeded(bool allowFindFallback = true)
		{
			Debug.Log("=== FINDING REFERENCES ===");
			if (this.generalInfoManager == null && allowFindFallback)
			{
				this.generalInfoManager = Object.FindObjectOfType<CharacterGeneralInfoManager>();
				Debug.Log(string.Format("Found GeneralInfoManager: {0}", this.generalInfoManager != null));
			}
			if (this.charactersManager == null && allowFindFallback)
			{
				this.charactersManager = Object.FindObjectOfType<CharactersManager>();
				Debug.Log(string.Format("Found CharactersManager: {0}", this.charactersManager != null));
			}
			if (this.characterCreatorManager == null && allowFindFallback)
			{
				this.characterCreatorManager = Object.FindObjectOfType<CharacterCreatorManager>();
				Debug.Log(string.Format("Found CharacterCreatorManager: {0}", this.characterCreatorManager != null));
				if (this.characterCreatorManager != null)
				{
					Debug.Log(string.Format("CharacterCreatorManager.Character: {0}", this.characterCreatorManager.Character != null));
					Debug.Log(string.Format("CharacterCreatorManager.IconTexture: {0}", this.characterCreatorManager.IconTexture != null));
				}
			}
			if (this.blendShapeController == null && allowFindFallback)
			{
				this.blendShapeController = Object.FindObjectOfType<CharacterBlendShapeController>(true);
				Debug.Log(string.Format("Found CharacterBlendShapeController: {0}", this.blendShapeController != null));
			}
			Debug.Log("=== REFERENCES SEARCH COMPLETE ===");
		}

		// Token: 0x06003A06 RID: 14854 RVA: 0x00155275 File Offset: 0x00153475
		public string GetCurrentCharacterName()
		{
			if (string.IsNullOrEmpty(this.currentCharacterName))
			{
				return this.defaultCharacterName;
			}
			return this.currentCharacterName;
		}

		// Token: 0x06003A07 RID: 14855 RVA: 0x00155294 File Offset: 0x00153494
		public void UpdateCharacterName()
		{
			string text = "";
			if (this.generalInfoManager != null)
			{
				string currentName = this.generalInfoManager.GetCurrentName();
				string currentSurname = this.generalInfoManager.GetCurrentSurname();
				text = (currentName + " " + currentSurname).Trim();
			}
			if (this.currentCharacterName != text)
			{
				this.currentCharacterName = text;
				string text2 = this.GetCurrentCharacterName();
				Debug.Log("Character name updated: '" + text2 + "'");
				Action<string> onCharacterNameChanged = this.OnCharacterNameChanged;
				if (onCharacterNameChanged == null)
				{
					return;
				}
				onCharacterNameChanged(text2);
			}
		}

		// Token: 0x06003A08 RID: 14856 RVA: 0x0015531F File Offset: 0x0015351F
		public void ResetCharacterName()
		{
			this.currentCharacterName = "";
			Action<string> onCharacterNameChanged = this.OnCharacterNameChanged;
			if (onCharacterNameChanged == null)
			{
				return;
			}
			onCharacterNameChanged(this.GetCurrentCharacterName());
		}

		// Token: 0x06003A09 RID: 14857 RVA: 0x00155344 File Offset: 0x00153544
		private CharacterDataExtended GetCharacterDataFromGeneralInfo(string characterName, CharacterDataExtended existingCharacter)
		{
			CharacterDataExtended characterDataExtended = existingCharacter;
			if (existingCharacter == null)
			{
				CharacterDataExtended characterDataExtended2 = new CharacterDataExtended();
				characterDataExtended2.characterName = characterName;
				characterDataExtended2.isMainCharacter = false;
				characterDataExtended = characterDataExtended2;
				characterDataExtended2.createdFrom = "character_creator";
			}
			CharacterDataExtended characterDataExtended3 = characterDataExtended;
			if (this.generalInfoManager != null)
			{
				string text = this.generalInfoManager.GetCurrentName() ?? "";
				string text2 = this.generalInfoManager.GetCurrentSurname() ?? "";
				string text3 = this.generalInfoManager.GetCurrentDescription() ?? "";
				string text4 = this.generalInfoManager.GetCurrentAttitude() ?? "";
				string text5 = this.generalInfoManager.GetCurrentBirthday() ?? "";
				characterDataExtended3.characterName = (text + " " + text2).Trim();
				if (string.IsNullOrEmpty(characterDataExtended3.characterName))
				{
					characterDataExtended3.characterName = characterName;
				}
				characterDataExtended3.generalInfo = new CharacterGeneralInfo
				{
					characterName = text,
					surname = text2,
					description = text3,
					attitude = text4,
					birthday = text5
				};
				Debug.Log("Created character data from UI - Birthday: " + text5);
			}
			characterDataExtended3.UpdateLastModified();
			return characterDataExtended3;
		}

		// Token: 0x06003A0A RID: 14858 RVA: 0x00155464 File Offset: 0x00153664
		private CharacterDataExtended ResolveCharacterByName(string characterName)
		{
			if (this.charactersManager == null || string.IsNullOrWhiteSpace(characterName))
			{
				return null;
			}
			string selectedCharacterId = this.charactersManager.GetSelectedCharacterId();
			if (!string.IsNullOrWhiteSpace(selectedCharacterId))
			{
				CharacterDataExtended characterById = this.charactersManager.GetCharacterById(selectedCharacterId);
				if (characterById != null)
				{
					return characterById;
				}
			}
			return this.charactersManager.GetAllCharacters().FirstOrDefault<CharacterDataExtended>((CharacterDataExtended c) => c.characterName == characterName);
		}

		// Token: 0x06003A0B RID: 14859 RVA: 0x001554E0 File Offset: 0x001536E0
		private void ResetAllStyles()
		{
			this.categorySubcategoryStyles.Clear();
			this.categoryPartStyles.Clear();
			this.categorySubcategoryStyles["Head"] = new Dictionary<string, StyleItemSO>();
			this.categorySubcategoryStyles["Body"] = new Dictionary<string, StyleItemSO>();
			this.categoryPartStyles["Head"] = new Dictionary<string, StyleItemSO>();
			this.categoryPartStyles["Body"] = new Dictionary<string, StyleItemSO>();
		}

		// Token: 0x06003A0C RID: 14860 RVA: 0x00155557 File Offset: 0x00153757
		public List<CharacterDataExtended> GetAllCharacters()
		{
			this.FindReferencesIfNeeded(true);
			if (this.charactersManager != null)
			{
				return this.charactersManager.GetAllCharacters();
			}
			return this.LoadCharactersFromFiles();
		}

		// Token: 0x06003A0D RID: 14861 RVA: 0x00155580 File Offset: 0x00153780
		private List<CharacterDataExtended> LoadCharactersFromFiles()
		{
			List<CharacterDataExtended> list = new List<CharacterDataExtended>();
			GameDataPaths.EnsureCharacterSaveDirectories();
			string characterSavesDirectory = GameDataPaths.CharacterSavesDirectory;
			if (!Directory.Exists(characterSavesDirectory))
			{
				Debug.Log("[CharacterStateManager] Save directory doesn't exist: " + characterSavesDirectory);
				return list;
			}
			string[] files = Directory.GetFiles(characterSavesDirectory, "*.json");
			Debug.Log(string.Format("[CharacterStateManager] Found {0} JSON files in {1}", files.Length, characterSavesDirectory));
			foreach (string text in files)
			{
				try
				{
					string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(text);
					string text2 = File.ReadAllText(text);
					CharacterDataExtended characterDataExtended = null;
					try
					{
						characterDataExtended = JsonUtility.FromJson<CharacterDataExtended>(text2);
					}
					catch
					{
						characterDataExtended = new CharacterDataExtended
						{
							characterName = fileNameWithoutExtension,
							characterId = Guid.NewGuid().ToString(),
							isMainCharacter = false,
							createdFrom = "bmac_save_system"
						};
					}
					if (string.IsNullOrWhiteSpace(characterDataExtended.characterId))
					{
						characterDataExtended.characterId = fileNameWithoutExtension;
					}
					if (string.IsNullOrEmpty(characterDataExtended.characterName))
					{
						characterDataExtended.characterName = fileNameWithoutExtension;
					}
					Texture2D texture2D = this.LoadCharacterIconFromPng(characterDataExtended.characterId);
					if (texture2D != null)
					{
						characterDataExtended.icon = Sprite.Create(texture2D, new Rect(0f, 0f, (float)texture2D.width, (float)texture2D.height), Vector2.one * 0.5f);
					}
					list.Add(characterDataExtended);
					Debug.Log("[CharacterStateManager] Loaded character: " + characterDataExtended.characterName);
				}
				catch (Exception ex)
				{
					Debug.LogError("[CharacterStateManager] Failed to load character from " + text + ": " + ex.Message);
				}
			}
			Debug.Log(string.Format("[CharacterStateManager] Total characters loaded: {0}", list.Count));
			return list;
		}

		// Token: 0x06003A0E RID: 14862 RVA: 0x00155770 File Offset: 0x00153970
		public CharacterDataExtended GetCharacter(string characterName)
		{
			if (this.charactersManager != null)
			{
				return this.charactersManager.GetAllCharacters().FirstOrDefault<CharacterDataExtended>((CharacterDataExtended c) => c.characterName == characterName);
			}
			return null;
		}

		// Token: 0x06003A0F RID: 14863 RVA: 0x001557B8 File Offset: 0x001539B8
		public bool DeleteCharacter(string characterName)
		{
			if (string.IsNullOrEmpty(characterName))
			{
				Debug.LogError("Cannot delete character: name is empty");
				return false;
			}
			bool flag;
			try
			{
				this.FindReferencesIfNeeded(true);
				CharacterDataExtended characterDataExtended = this.ResolveCharacterByName(characterName);
				BMAC_SaveSystem.DeleteCharacter((!string.IsNullOrWhiteSpace((characterDataExtended != null) ? characterDataExtended.characterId : null)) ? characterDataExtended.characterId : characterName);
				if (this.charactersManager != null)
				{
					characterDataExtended = this.charactersManager.GetAllCharacters().FirstOrDefault<CharacterDataExtended>((CharacterDataExtended c) => c.characterName == characterName);
					if (characterDataExtended != null)
					{
						this.charactersManager.DeleteCharacter(characterDataExtended.characterId);
						Debug.Log("Character '" + characterName + "' removed from CharactersManager");
					}
				}
				Debug.Log("Character '" + characterName + "' deleted successfully!");
				flag = true;
			}
			catch (Exception ex)
			{
				Debug.LogError("Failed to delete character '" + characterName + "': " + ex.Message);
				flag = false;
			}
			return flag;
		}

		// Token: 0x06003A10 RID: 14864 RVA: 0x001558D4 File Offset: 0x00153AD4
		public bool DuplicateCharacter(string sourceCharacterName, string newCharacterName)
		{
			if (string.IsNullOrEmpty(sourceCharacterName) || string.IsNullOrEmpty(newCharacterName))
			{
				Debug.LogError("Cannot duplicate character: source or new name is empty");
				return false;
			}
			bool flag;
			try
			{
				this.LoadCharacter(sourceCharacterName);
				base.StartCoroutine(this.DuplicateCharacterCoroutine(newCharacterName));
				flag = true;
			}
			catch (Exception ex)
			{
				Debug.LogError("Failed to duplicate character '" + sourceCharacterName + "': " + ex.Message);
				flag = false;
			}
			return flag;
		}

		// Token: 0x06003A11 RID: 14865 RVA: 0x00155948 File Offset: 0x00153B48
		private IEnumerator DuplicateCharacterCoroutine(string newCharacterName)
		{
			yield return new WaitForEndOfFrame();
			this.SaveCurrentCharacter(newCharacterName);
			yield break;
		}

		// Token: 0x06003A12 RID: 14866 RVA: 0x00155960 File Offset: 0x00153B60
		public bool RenameCharacter(string oldName, string newName)
		{
			if (string.IsNullOrEmpty(oldName) || string.IsNullOrEmpty(newName))
			{
				Debug.LogError("Cannot rename character: old or new name is empty");
				return false;
			}
			bool flag;
			try
			{
				if (this.DuplicateCharacter(oldName, newName))
				{
					this.DeleteCharacter(oldName);
					Debug.Log(string.Concat(new string[] { "Character renamed from '", oldName, "' to '", newName, "'" }));
					flag = true;
				}
				else
				{
					flag = false;
				}
			}
			catch (Exception ex)
			{
				Debug.LogError("Failed to rename character '" + oldName + "': " + ex.Message);
				flag = false;
			}
			return flag;
		}

		// Token: 0x06003A13 RID: 14867 RVA: 0x00155A08 File Offset: 0x00153C08
		public bool CharacterExists(string characterName)
		{
			return !string.IsNullOrEmpty(characterName) && this.GetCharacter(characterName) != null;
		}

		// Token: 0x06003A14 RID: 14868 RVA: 0x00155A1E File Offset: 0x00153C1E
		public List<string> GetAllCharacterNames()
		{
			return (from c in this.GetAllCharacters()
				select c.characterName).ToList<string>();
		}

		// Token: 0x06003A15 RID: 14869 RVA: 0x00155A50 File Offset: 0x00153C50
		public bool ExportCharacterToPng(string characterName, string filePath)
		{
			bool flag;
			try
			{
				if (this.GetCharacter(characterName) == null)
				{
					Debug.LogError("Character '" + characterName + "' not found for export");
					flag = false;
				}
				else
				{
					this.LoadCharacter(characterName);
					CharacterCreatorManager characterCreatorManager = this.characterCreatorManager;
					Texture2D texture2D = ((characterCreatorManager != null) ? characterCreatorManager.GenerateCharacterIcon() : null);
					if (texture2D == null)
					{
						Debug.LogError("Failed to generate character icon for export");
						flag = false;
					}
					else
					{
						byte[] array = texture2D.EncodeToPNG();
						File.WriteAllBytes(filePath, array);
						Debug.Log(string.Concat(new string[] { "Character '", characterName, "' exported to '", filePath, "'" }));
						flag = true;
					}
				}
			}
			catch (Exception ex)
			{
				Debug.LogError("Failed to export character '" + characterName + "': " + ex.Message);
				flag = false;
			}
			return flag;
		}

		// Token: 0x06003A16 RID: 14870 RVA: 0x00155B28 File Offset: 0x00153D28
		private OutfitSystem GetTargetCharacter()
		{
			CharacterCreatorManager characterCreatorManager = this.characterCreatorManager;
			if (((characterCreatorManager != null) ? characterCreatorManager.Character : null) != null)
			{
				return this.characterCreatorManager.Character;
			}
			OutfitSystem outfitSystem = Object.FindObjectOfType<OutfitSystem>();
			if (outfitSystem != null)
			{
				Debug.Log("[CharacterStateManager] Found OutfitSystem fallback: " + outfitSystem.name);
				return outfitSystem;
			}
			Debug.LogError("[CharacterStateManager] No OutfitSystem found! Please ensure CharacterCreatorManager.Character is assigned or OutfitSystem exists in scene.");
			return null;
		}

		// Token: 0x06003A17 RID: 14871 RVA: 0x00155B8C File Offset: 0x00153D8C
		private async void LoadCharacterFromData(OutfitSystem targetCharacter, CharacterDataExtended characterData)
		{
			CharacterData characterData2 = this.ConvertToBMACData(characterData);
			await BMAC_SaveSystem.LoadCharacter(targetCharacter, characterData2, false, true);
		}

		// Token: 0x06003A18 RID: 14872 RVA: 0x00155BD4 File Offset: 0x00153DD4
		private CharacterData ConvertToBMACData(CharacterDataExtended extendedData)
		{
			if (extendedData == null)
			{
				return null;
			}
			string text = Path.Combine(GameDataPaths.CharacterSavesDirectory, extendedData.characterId + ".json");
			if (File.Exists(text))
			{
				try
				{
					CharacterData characterData = JsonUtility.FromJson<CharacterData>(File.ReadAllText(text));
					Debug.Log("[CharacterStateManager] Loaded BMAC data from file for: " + extendedData.characterName);
					return characterData;
				}
				catch (Exception ex)
				{
					Debug.LogWarning("[CharacterStateManager] Failed to load BMAC data from file: " + ex.Message);
				}
			}
			Debug.LogWarning("[CharacterStateManager] No BMAC data found for " + extendedData.characterName + ", using empty data");
			return new CharacterData
			{
				characterName = extendedData.characterName
			};
		}

		// Token: 0x06003A19 RID: 14873 RVA: 0x00155C84 File Offset: 0x00153E84
		private void SaveCharacterIconAsPng(string characterId, Texture2D icon)
		{
			try
			{
				GameDataPaths.EnsureCharacterProfileDirectories();
				string characterProfileIconsDirectory = GameDataPaths.CharacterProfileIconsDirectory;
				if (!Directory.Exists(characterProfileIconsDirectory))
				{
					Directory.CreateDirectory(characterProfileIconsDirectory);
					Debug.Log("Created icon folder: " + characterProfileIconsDirectory);
				}
				string text = Path.Combine(characterProfileIconsDirectory, characterId + ".png");
				byte[] array = icon.EncodeToPNG();
				File.WriteAllBytes(text, array);
				Debug.Log("Character icon saved: " + text);
			}
			catch (Exception ex)
			{
				Debug.LogError("Failed to save character icon for '" + characterId + "': " + ex.Message);
			}
		}

		// Token: 0x06003A1A RID: 14874 RVA: 0x00155D1C File Offset: 0x00153F1C
		public Texture2D LoadCharacterIconFromPng(string characterId)
		{
			Texture2D texture2D2;
			try
			{
				string text = Path.Combine(GameDataPaths.CharacterProfileIconsDirectory, characterId + ".png");
				if (File.Exists(text))
				{
					byte[] array = File.ReadAllBytes(text);
					Texture2D texture2D = new Texture2D(2, 2);
					texture2D.LoadImage(array);
					Debug.Log("Character icon loaded: " + text);
					texture2D2 = texture2D;
				}
				else
				{
					Debug.LogWarning("Character icon not found: " + text);
					texture2D2 = null;
				}
			}
			catch (Exception ex)
			{
				Debug.LogError("Failed to load character icon for '" + characterId + "': " + ex.Message);
				texture2D2 = null;
			}
			return texture2D2;
		}

		// Token: 0x06003A1B RID: 14875 RVA: 0x00155DB8 File Offset: 0x00153FB8
		[ContextMenu("Force Find References")]
		public void ForceFindReferences()
		{
			this.ResetRuntimeReferenceCache();
			this.FindReferencesIfNeeded(true);
		}

		// Token: 0x06003A1C RID: 14876 RVA: 0x00155DC8 File Offset: 0x00153FC8
		[ContextMenu("Validate System")]
		public void ValidateSystem()
		{
			Debug.Log("=== CHARACTER STATE MANAGER VALIDATION ===");
			this.FindReferencesIfNeeded(true);
			Debug.Log("GeneralInfoManager: " + ((this.generalInfoManager != null) ? "✓" : "✗"));
			Debug.Log("CharactersManager: " + ((this.charactersManager != null) ? "✓" : "✗"));
			Debug.Log("CharacterCreatorManager: " + ((this.characterCreatorManager != null) ? "✓" : "✗"));
			if (this.characterCreatorManager != null)
			{
				Debug.Log("CharacterCreatorManager.Character: " + ((this.characterCreatorManager.Character != null) ? "✓" : "✗"));
				Debug.Log("CharacterCreatorManager.IconTexture: " + ((this.characterCreatorManager.IconTexture != null) ? "✓" : "✗"));
			}
			Debug.Log("Current Character Name: '" + this.GetCurrentCharacterName() + "'");
			Debug.Log("Default Character Name: '" + this.defaultCharacterName + "'");
			List<CharacterDataExtended> allCharacters = this.GetAllCharacters();
			Debug.Log(string.Format("Total Characters in System: {0}", allCharacters.Count));
			foreach (CharacterDataExtended characterDataExtended in allCharacters)
			{
				Debug.Log(string.Format("- {0} (ID: {1}, Main: {2})", characterDataExtended.characterName, characterDataExtended.characterId, characterDataExtended.isMainCharacter));
			}
			Debug.Log("=== VALIDATION COMPLETE ===");
		}

		// Token: 0x06003A1D RID: 14877 RVA: 0x00155F8C File Offset: 0x0015418C
		[ContextMenu("Test Icon Generation")]
		public void TestIconGeneration()
		{
			Debug.Log("=== TESTING ICON GENERATION ===");
			this.FindReferencesIfNeeded(true);
			if (this.characterCreatorManager != null)
			{
				Texture2D texture2D = this.characterCreatorManager.GenerateCharacterIcon();
				if (texture2D != null)
				{
					Debug.Log(string.Format("Icon generated successfully: {0}x{1}", texture2D.width, texture2D.height));
					this.SaveCharacterIconAsPng("TestIcon", texture2D);
				}
				else
				{
					Debug.LogError("Failed to generate icon - check camera and render texture assignment");
				}
			}
			else
			{
				Debug.LogError("CharacterCreatorManager not found!");
			}
			Debug.Log("=== ICON TEST COMPLETE ===");
		}

		// Token: 0x06003A1E RID: 14878 RVA: 0x00156020 File Offset: 0x00154220
		[ContextMenu("Test Load Specific Character")]
		public void TestLoadSpecificCharacter()
		{
			List<CharacterDataExtended> allCharacters = this.GetAllCharacters();
			if (allCharacters.Count > 0)
			{
				string characterName = allCharacters[0].characterName;
				Debug.Log("=== TESTING LOAD SPECIFIC CHARACTER: " + characterName + " ===");
				this.LoadCharacter(characterName);
				Debug.Log("=== LOAD SPECIFIC CHARACTER TEST COMPLETE ===");
				return;
			}
			Debug.LogWarning("No characters found to test loading");
		}

		// Token: 0x06003A1F RID: 14879 RVA: 0x0015607C File Offset: 0x0015427C
		[ContextMenu("Test OutfitSystem Detection")]
		public void TestOutfitSystemDetection()
		{
			Debug.Log("=== TESTING OUTFIT SYSTEM DETECTION ===");
			this.FindReferencesIfNeeded(true);
			OutfitSystem targetCharacter = this.GetTargetCharacter();
			if (targetCharacter != null)
			{
				Debug.Log("✓ OutfitSystem found: " + targetCharacter.name);
				Debug.Log("✓ OutfitSystem GameObject: " + targetCharacter.gameObject.name);
				Debug.Log(string.Format("✓ OutfitSystem active: {0}", targetCharacter.gameObject.activeInHierarchy));
			}
			else
			{
				Debug.LogError("✗ No OutfitSystem found!");
			}
			Debug.Log("=== OUTFIT SYSTEM TEST COMPLETE ===");
		}

		// Token: 0x06003A20 RID: 14880 RVA: 0x00156110 File Offset: 0x00154310
		[ContextMenu("Test Load Characters From Files")]
		public void TestLoadCharactersFromFiles()
		{
			Debug.Log("=== TESTING LOAD CHARACTERS FROM FILES ===");
			List<CharacterDataExtended> list = this.LoadCharactersFromFiles();
			Debug.Log(string.Format("Loaded {0} characters:", list.Count));
			foreach (CharacterDataExtended characterDataExtended in list)
			{
				Debug.Log(string.Format("- {0} (ID: {1}, Icon: {2})", characterDataExtended.characterName, characterDataExtended.characterId, characterDataExtended.icon != null));
			}
			Debug.Log("=== LOAD TEST COMPLETE ===");
		}

		// Token: 0x06003A21 RID: 14881 RVA: 0x001561B8 File Offset: 0x001543B8
		[ContextMenu("Test Save/Load System")]
		public void TestSaveLoadSystem()
		{
			Debug.Log("=== TESTING SAVE/LOAD SYSTEM ===");
			string text = "Test Character " + DateTime.Now.ToString("HHmmss");
			Debug.Log("Testing save with name: '" + text + "'");
			this.SaveCurrentCharacter(text);
			bool flag = this.CharacterExists(text);
			Debug.Log(string.Format("Character exists after save: {0}", flag));
			if (flag)
			{
				Debug.Log("Testing load of character: '" + text + "'");
				this.LoadCharacter(text);
			}
			Debug.Log("=== TEST COMPLETE ===");
		}

		// Token: 0x04003A67 RID: 14951
		private Dictionary<string, Dictionary<string, StyleItemSO>> categorySubcategoryStyles = new Dictionary<string, Dictionary<string, StyleItemSO>>();

		// Token: 0x04003A68 RID: 14952
		private Dictionary<string, Dictionary<string, StyleItemSO>> categoryPartStyles = new Dictionary<string, Dictionary<string, StyleItemSO>>();

		// Token: 0x04003A69 RID: 14953
		[SerializeField]
		private StyleManager styleManager;

		// Token: 0x04003A6A RID: 14954
		[Header("Save/Load System")]
		[SerializeField]
		private CharacterGeneralInfoManager generalInfoManager;

		// Token: 0x04003A6B RID: 14955
		[SerializeField]
		private CharactersManager charactersManager;

		// Token: 0x04003A6C RID: 14956
		[SerializeField]
		private CharacterCreatorManager characterCreatorManager;

		// Token: 0x04003A6D RID: 14957
		[SerializeField]
		private CharacterBlendShapeController blendShapeController;

		// Token: 0x04003A6E RID: 14958
		[SerializeField]
		private CharacterBlendShapeController mainCharacterBlendShapeController;

		// Token: 0x04003A6F RID: 14959
		[SerializeField]
		private CharacterBlendShapeController npcBlendShapeController;

		// Token: 0x04003A70 RID: 14960
		[SerializeField]
		private CustomizationTargetType activeTargetContext;

		// Token: 0x04003A75 RID: 14965
		private string currentCharacterName = "";

		// Token: 0x04003A76 RID: 14966
		[Header("UI Settings")]
		public string defaultCharacterName = "New Character";
	}
}
