using System;
using System.Collections.Generic;
using UnityEngine;

namespace BoZo.CharacterCreator
{
	// Token: 0x020007A5 RID: 1957
	[CreateAssetMenu(fileName = "New Blend Shape Map", menuName = "Character Creator/Blend Shape Map")]
	public class BlendShapeMapSO : ScriptableObject
	{
		// Token: 0x060033A2 RID: 13218 RVA: 0x00125660 File Offset: 0x00123860
		public List<BlendShapeMapSO.BlendShapeEntry> GetBlendShapesForSubcategory(string category, string subcategory)
		{
			return this.blendShapeMappings.FindAll((BlendShapeMapSO.BlendShapeEntry bs) => bs.category.Equals(category, StringComparison.OrdinalIgnoreCase) && bs.subcategory.Equals(subcategory, StringComparison.OrdinalIgnoreCase));
		}

		// Token: 0x060033A3 RID: 13219 RVA: 0x00125698 File Offset: 0x00123898
		public List<BlendShapeMapSO.BlendShapeEntry> GetBlendShapesForSubcategoryByType(string category, string subcategory, BlendShapeMapSO.BlendShapeType type)
		{
			return this.blendShapeMappings.FindAll((BlendShapeMapSO.BlendShapeEntry bs) => bs.category.Equals(category, StringComparison.OrdinalIgnoreCase) && bs.subcategory.Equals(subcategory, StringComparison.OrdinalIgnoreCase) && bs.blendShapeType == type);
		}

		// Token: 0x060033A4 RID: 13220 RVA: 0x001256D8 File Offset: 0x001238D8
		public List<string> GetSubcategoriesForCategory(string category)
		{
			HashSet<string> hashSet = new HashSet<string>();
			foreach (BlendShapeMapSO.BlendShapeEntry blendShapeEntry in this.blendShapeMappings)
			{
				if (blendShapeEntry.category.Equals(category, StringComparison.OrdinalIgnoreCase))
				{
					hashSet.Add(blendShapeEntry.subcategory);
				}
			}
			return new List<string>(hashSet);
		}

		// Token: 0x060033A5 RID: 13221 RVA: 0x0012574C File Offset: 0x0012394C
		public List<string> GetAllCategories()
		{
			HashSet<string> hashSet = new HashSet<string>();
			foreach (BlendShapeMapSO.BlendShapeEntry blendShapeEntry in this.blendShapeMappings)
			{
				hashSet.Add(blendShapeEntry.category);
			}
			return new List<string>(hashSet);
		}

		// Token: 0x04003438 RID: 13368
		[Tooltip("The character model this mapping applies to")]
		public string characterModelId;

		// Token: 0x04003439 RID: 13369
		[Tooltip("List of blend shape mappings")]
		public List<BlendShapeMapSO.BlendShapeEntry> blendShapeMappings = new List<BlendShapeMapSO.BlendShapeEntry>();

		// Token: 0x020007A6 RID: 1958
		[Serializable]
		public class BlendShapeEntry
		{
			// Token: 0x170009A7 RID: 2471
			// (get) Token: 0x060033A7 RID: 13223 RVA: 0x001257C7 File Offset: 0x001239C7
			public bool IsGated
			{
				get
				{
					return !string.IsNullOrWhiteSpace(this.gatedByDisplayName);
				}
			}

			// Token: 0x060033A8 RID: 13224 RVA: 0x001257D8 File Offset: 0x001239D8
			public List<string> GetAllBlendShapeNames()
			{
				List<string> list = new List<string>();
				if (this.controlMode == BlendShapeMapSO.BlendShapeEntry.BlendShapeControlMode.PairInOut)
				{
					if (!string.IsNullOrEmpty(this.pairInBlendShapeName))
					{
						list.Add(this.pairInBlendShapeName);
					}
					if (!string.IsNullOrEmpty(this.pairOutBlendShapeName))
					{
						list.Add(this.pairOutBlendShapeName);
					}
					return list;
				}
				if (this.combinedBlendShapeNames != null && this.combinedBlendShapeNames.Count > 0)
				{
					list.AddRange(this.combinedBlendShapeNames);
				}
				else if (!string.IsNullOrEmpty(this.blendShapeName))
				{
					list.Add(this.blendShapeName);
				}
				return list;
			}

			// Token: 0x060033A9 RID: 13225 RVA: 0x00125866 File Offset: 0x00123A66
			public bool IsCombined()
			{
				return this.combinedBlendShapeNames != null && this.combinedBlendShapeNames.Count > 0;
			}

			// Token: 0x060033AA RID: 13226 RVA: 0x00125880 File Offset: 0x00123A80
			public bool IsBooleanControl()
			{
				return this.controlMode == BlendShapeMapSO.BlendShapeEntry.BlendShapeControlMode.Boolean;
			}

			// Token: 0x060033AB RID: 13227 RVA: 0x0012588B File Offset: 0x00123A8B
			public bool IsPairControl()
			{
				return this.controlMode == BlendShapeMapSO.BlendShapeEntry.BlendShapeControlMode.PairInOut;
			}

			// Token: 0x060033AC RID: 13228 RVA: 0x00125896 File Offset: 0x00123A96
			public bool IsSelectControl()
			{
				return this.controlMode == BlendShapeMapSO.BlendShapeEntry.BlendShapeControlMode.Select;
			}

			// Token: 0x060033AD RID: 13229 RVA: 0x001258A1 File Offset: 0x00123AA1
			public bool IsGroupControl()
			{
				return this.controlMode == BlendShapeMapSO.BlendShapeEntry.BlendShapeControlMode.Group;
			}

			// Token: 0x060033AE RID: 13230 RVA: 0x001258AC File Offset: 0x00123AAC
			public int GetSelectOptionCount()
			{
				if (!this.IsSelectControl())
				{
					return 0;
				}
				int num = ((this.combinedBlendShapeNames != null) ? this.combinedBlendShapeNames.Count : 0);
				if (num > 0)
				{
					return num;
				}
				if (!string.IsNullOrWhiteSpace(this.blendShapeName))
				{
					return 1;
				}
				return 0;
			}

			// Token: 0x060033AF RID: 13231 RVA: 0x001258F0 File Offset: 0x00123AF0
			public string GetSelectOptionDisplayName(int index)
			{
				if (!this.IsSelectControl())
				{
					return string.Empty;
				}
				if (this.selectOptionLabels != null && index >= 0 && index < this.selectOptionLabels.Count)
				{
					string text = this.selectOptionLabels[index];
					if (!string.IsNullOrWhiteSpace(text))
					{
						return text;
					}
				}
				if (this.combinedBlendShapeNames != null && index >= 0 && index < this.combinedBlendShapeNames.Count)
				{
					string text2 = this.combinedBlendShapeNames[index];
					if (!string.IsNullOrWhiteSpace(text2))
					{
						return text2;
					}
					return "Default";
				}
				else
				{
					if (index == 0 && !string.IsNullOrWhiteSpace(this.blendShapeName))
					{
						return this.blendShapeName;
					}
					return string.Empty;
				}
			}

			// Token: 0x060033B0 RID: 13232 RVA: 0x0012598F File Offset: 0x00123B8F
			public float GetUiMinValue()
			{
				if (this.IsPairControl())
				{
					return -1f;
				}
				if (this.IsSelectControl())
				{
					return 0f;
				}
				if (this.IsBooleanControl())
				{
					return 0f;
				}
				return this.minValue;
			}

			// Token: 0x060033B1 RID: 13233 RVA: 0x001259C4 File Offset: 0x00123BC4
			public float GetUiMaxValue()
			{
				if (this.IsPairControl())
				{
					return 1f;
				}
				if (this.IsSelectControl())
				{
					int selectOptionCount = this.GetSelectOptionCount();
					if (selectOptionCount <= 1)
					{
						return 0f;
					}
					return (float)(selectOptionCount - 1);
				}
				else
				{
					if (this.IsBooleanControl())
					{
						return 1f;
					}
					return this.maxValue;
				}
			}

			// Token: 0x060033B2 RID: 13234 RVA: 0x00125A14 File Offset: 0x00123C14
			public float GetUiDefaultValue()
			{
				if (this.IsPairControl())
				{
					return Mathf.Clamp(this.defaultValue, -1f, 1f);
				}
				if (this.IsSelectControl())
				{
					return Mathf.Clamp(Mathf.Round(this.defaultValue), this.GetUiMinValue(), this.GetUiMaxValue());
				}
				if (!this.IsBooleanControl())
				{
					return this.defaultValue;
				}
				float num = (this.minValue + this.maxValue) * 0.5f;
				if (this.defaultValue < num)
				{
					return 0f;
				}
				return 1f;
			}

			// Token: 0x0400343A RID: 13370
			[Tooltip("The UI-friendly name of this blend shape")]
			public string displayName;

			// Token: 0x0400343B RID: 13371
			[Tooltip("The actual blend shape name in the model (for single blend shape)")]
			public string blendShapeName;

			// Token: 0x0400343C RID: 13372
			[Tooltip("List of blend shape names for combined blend shapes (leave empty if using single blendShapeName)")]
			public List<string> combinedBlendShapeNames = new List<string>();

			// Token: 0x0400343D RID: 13373
			[Tooltip("The category this blend shape belongs to (Head, Body, etc.)")]
			public string category;

			// Token: 0x0400343E RID: 13374
			[Tooltip("The subcategory this blend shape belongs to (Hair Front, Eyes, etc.)")]
			public string subcategory;

			// Token: 0x0400343F RID: 13375
			[Tooltip("The type of blend shape (Shape, Expression, etc.)")]
			public BlendShapeMapSO.BlendShapeType blendShapeType;

			// Token: 0x04003440 RID: 13376
			[Tooltip("How this entry is controlled in UI")]
			public BlendShapeMapSO.BlendShapeEntry.BlendShapeControlMode controlMode;

			// Token: 0x04003441 RID: 13377
			[Tooltip("Minimum value for this blend shape")]
			public float minValue;

			// Token: 0x04003442 RID: 13378
			[Tooltip("Maximum value for this blend shape")]
			public float maxValue = 100f;

			// Token: 0x04003443 RID: 13379
			[Tooltip("Default value for this blend shape")]
			public float defaultValue;

			// Token: 0x04003444 RID: 13380
			[Tooltip("Blend shape used for negative side of a paired control (In). Used only when controlMode = PairInOut")]
			public string pairInBlendShapeName;

			// Token: 0x04003445 RID: 13381
			[Tooltip("Blend shape used for positive side of a paired control (Out). Used only when controlMode = PairInOut")]
			public string pairOutBlendShapeName;

			// Token: 0x04003446 RID: 13382
			[Tooltip("Invert the On/Off logic for Boolean controls. When true, UI 'ON' sends 0 to the blend shape and 'OFF' sends 1.")]
			public bool invertBoolean;

			// Token: 0x04003447 RID: 13383
			[Tooltip("Optional labels for Select options. If provided, index-aligned labels are shown in UI instead of blend shape names.")]
			public List<string> selectOptionLabels = new List<string>();

			// Token: 0x04003448 RID: 13384
			[Tooltip("Se valorizzato, questa entry è VISIBILE solo quando l'entry-toggle col displayName indicato (nella stessa subcategory) è ON. Usato per nascondere gli slider di size finché la Futa Mode è OFF.")]
			public string gatedByDisplayName;

			// Token: 0x04003449 RID: 13385
			[Tooltip("Marca questa entry Boolean come il toggle della Futanari Mode: invece di scrivere una blend shape, pilota il FutanariModeController del modello (renderer penis/balls + fisica). Lo stato è comunque persistito.")]
			public bool isFutanariToggle;

			// Token: 0x020007A7 RID: 1959
			public enum BlendShapeControlMode
			{
				// Token: 0x0400344B RID: 13387
				Range,
				// Token: 0x0400344C RID: 13388
				Boolean,
				// Token: 0x0400344D RID: 13389
				PairInOut,
				// Token: 0x0400344E RID: 13390
				Select,
				// Token: 0x0400344F RID: 13391
				Group
			}
		}

		// Token: 0x020007A8 RID: 1960
		public enum BlendShapeType
		{
			// Token: 0x04003451 RID: 13393
			Shape,
			// Token: 0x04003452 RID: 13394
			Expression,
			// Token: 0x04003453 RID: 13395
			Color
		}
	}
}
