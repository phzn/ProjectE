using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BoZo.CharacterCreator;
using UnityEngine;

namespace Bozo.AnimeCharacters
{
	// Token: 0x020008AE RID: 2222
	public class StyleManager : MonoBehaviour
	{
		// Token: 0x06003B7F RID: 15231 RVA: 0x001618FD File Offset: 0x0015FAFD
		private void Awake()
		{
			this.LoadAllStyles();
		}

		// Token: 0x06003B80 RID: 15232 RVA: 0x00161908 File Offset: 0x0015FB08
		private void LoadAllStyles()
		{
			this.InitializeOutfitTypeMap();
			StyleItemSO[] array = Resources.LoadAll<StyleItemSO>("CharacterCustomizationStyles");
			if (this.availableOutfitTypes != null)
			{
				foreach (OutfitType outfitType in this.availableOutfitTypes)
				{
					if (outfitType != null)
					{
						this.stylesByType[outfitType] = new List<StyleItemSO>();
					}
				}
			}
			foreach (StyleItemSO styleItemSO in array)
			{
				if (!(styleItemSO == null) && styleItemSO.showInNpcCatalog && styleItemSO.itemType != null)
				{
					if (!this.stylesByType.ContainsKey(styleItemSO.itemType))
					{
						this.stylesByType[styleItemSO.itemType] = new List<StyleItemSO>();
					}
					this.stylesByType[styleItemSO.itemType].Add(styleItemSO);
				}
			}
		}

		// Token: 0x06003B81 RID: 15233 RVA: 0x001619E0 File Offset: 0x0015FBE0
		private void InitializeOutfitTypeMap()
		{
			this.outfitTypeMap.Clear();
			if (this.availableOutfitTypes != null)
			{
				foreach (OutfitType outfitType in this.availableOutfitTypes)
				{
					if (outfitType != null && !this.outfitTypeMap.ContainsKey(outfitType.TypeName))
					{
						this.outfitTypeMap.Add(outfitType.TypeName, outfitType);
					}
				}
			}
		}

		// Token: 0x06003B82 RID: 15234 RVA: 0x00161A48 File Offset: 0x0015FC48
		public List<StyleItemSO> GetStylesForType(OutfitType type)
		{
			if (type == null)
			{
				Debug.LogWarning("[StyleManager] GetStylesForType called with null OutfitType. Returning empty list.");
				return new List<StyleItemSO>();
			}
			List<StyleItemSO> list;
			if (this.stylesByType.TryGetValue(type, out list))
			{
				return list;
			}
			return new List<StyleItemSO>();
		}

		// Token: 0x06003B83 RID: 15235 RVA: 0x00161A88 File Offset: 0x0015FC88
		public void ApplyStyle(StyleItemSO style)
		{
			if (style == null)
			{
				Debug.LogError("Impossibile applicare uno stile nullo");
				return;
			}
			if (style.itemType == null)
			{
				Debug.LogWarning("[StyleManager] Skip ApplyStyle for '" + style.itemName + "' because itemType is null.");
				return;
			}
			if (this.outfitSystem != null)
			{
				if (style.prefab == null)
				{
					this.outfitSystem.RemoveOutfit(style.itemType, true);
					this.currentStyles.Remove(style.itemType);
					if (style.additionalTypesToRemove != null)
					{
						foreach (OutfitType outfitType in style.additionalTypesToRemove)
						{
							if (outfitType != null)
							{
								this.RemoveStyle(outfitType);
							}
						}
					}
					if (style.applyBlendShapes && this.blendShapeController != null)
					{
						foreach (KeyValuePair<string, float> keyValuePair in style.GetBlendShapeValues())
						{
							this.blendShapeController.SetBlendShapeValue(keyValuePair.Key, keyValuePair.Value);
						}
					}
					return;
				}
				if (style.replaceExisting)
				{
					this.outfitSystem.RemoveOutfit(style.itemType, true);
				}
				if (style.prefab != null)
				{
					Outfit component = style.prefab.GetComponent<Outfit>();
					Outfit outfit;
					if (component == null || component.Type != style.itemType)
					{
						GameObject gameObject = new GameObject("TempOutfit");
						foreach (object obj in style.prefab.transform)
						{
							Transform transform = (Transform)obj;
							GameObject gameObject2 = Object.Instantiate<GameObject>(transform.gameObject);
							gameObject2.transform.SetParent(gameObject.transform);
							gameObject2.transform.localPosition = transform.localPosition;
							gameObject2.transform.localRotation = transform.localRotation;
							gameObject2.transform.localScale = transform.localScale;
						}
						GameObject gameObject3 = Object.Instantiate<GameObject>(gameObject, this.outfitSystem.transform);
						gameObject3.name = style.itemName;
						Object.Destroy(gameObject);
						outfit = gameObject3.AddComponent<Outfit>();
						outfit.OutfitIcon = style.icon;
						outfit.OutfitName = style.itemName;
						FieldInfo field = typeof(Outfit).GetField("Type", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
						if (!(field != null))
						{
							Debug.LogError("Non è stato possibile trovare il campo Type nella classe Outfit");
							return;
						}
						field.SetValue(outfit, style.itemType);
					}
					else
					{
						outfit = Object.Instantiate<GameObject>(style.prefab, this.outfitSystem.transform).GetComponent<Outfit>();
						outfit.OutfitIcon = style.icon;
						outfit.OutfitName = style.itemName;
					}
					this.outfitSystem.AttachOutfit(outfit);
					this.currentStyles[style.itemType] = style;
				}
			}
			else
			{
				Debug.LogWarning("OutfitSystem non disponibile. Impossibile applicare lo stile.");
			}
			if (style.applyBlendShapes && this.blendShapeController != null)
			{
				foreach (KeyValuePair<string, float> keyValuePair2 in style.GetBlendShapeValues())
				{
					this.blendShapeController.SetBlendShapeValue(keyValuePair2.Key, keyValuePair2.Value);
				}
			}
		}

		// Token: 0x06003B84 RID: 15236 RVA: 0x00161E10 File Offset: 0x00160010
		public void RemoveStyle(OutfitType type)
		{
			if (type == null)
			{
				return;
			}
			if (this.outfitSystem != null)
			{
				this.outfitSystem.RemoveOutfit(type, true);
				if (this.currentStyles.ContainsKey(type))
				{
					this.currentStyles.Remove(type);
				}
			}
		}

		// Token: 0x06003B85 RID: 15237 RVA: 0x00161E60 File Offset: 0x00160060
		public StyleItemSO GetCurrentStyle(OutfitType type)
		{
			if (type == null)
			{
				return null;
			}
			StyleItemSO styleItemSO;
			if (this.currentStyles.TryGetValue(type, out styleItemSO))
			{
				return styleItemSO;
			}
			return null;
		}

		// Token: 0x06003B86 RID: 15238 RVA: 0x00161E8B File Offset: 0x0016008B
		public bool IsStyleEquipped(StyleItemSO style)
		{
			return !(style == null) && !(style.itemType == null) && this.GetEquippedStyle(style.itemType) == style;
		}

		// Token: 0x17000A26 RID: 2598
		// (get) Token: 0x06003B87 RID: 15239 RVA: 0x00161EB8 File Offset: 0x001600B8
		public OutfitSystem OutfitSystem
		{
			get
			{
				return this.outfitSystem;
			}
		}

		// Token: 0x06003B88 RID: 15240 RVA: 0x00161EC0 File Offset: 0x001600C0
		public StyleItemSO GetEquippedStyle(OutfitType type)
		{
			if (type == null || this.outfitSystem == null)
			{
				return null;
			}
			Outfit outfit = this.outfitSystem.GetOutfit(type);
			if (outfit == null)
			{
				return this.GetStylesForType(type).FirstOrDefault<StyleItemSO>((StyleItemSO style) => style != null && style.prefab == null && (style.additionalTypesToRemove == null || style.additionalTypesToRemove.All<OutfitType>((OutfitType extra) => extra == null || this.outfitSystem.GetOutfit(extra) == null)));
			}
			string text = outfit.name.Replace("(Clone)", string.Empty).Trim();
			List<StyleItemSO> stylesForType = this.GetStylesForType(type);
			for (int i = 0; i < stylesForType.Count; i++)
			{
				StyleItemSO styleItemSO = stylesForType[i];
				if (!(styleItemSO == null))
				{
					if (styleItemSO.prefab != null && string.Equals(styleItemSO.prefab.name, text, StringComparison.OrdinalIgnoreCase))
					{
						return styleItemSO;
					}
					if (!string.IsNullOrWhiteSpace(styleItemSO.itemName) && (string.Equals(styleItemSO.itemName, outfit.OutfitName, StringComparison.OrdinalIgnoreCase) || string.Equals(styleItemSO.itemName, text, StringComparison.OrdinalIgnoreCase)))
					{
						return styleItemSO;
					}
				}
			}
			return null;
		}

		// Token: 0x04003BD3 RID: 15315
		[SerializeField]
		private OutfitSystem outfitSystem;

		// Token: 0x04003BD4 RID: 15316
		[SerializeField]
		private CharacterBlendShapeController blendShapeController;

		// Token: 0x04003BD5 RID: 15317
		[Header("Outfit Types")]
		[SerializeField]
		private OutfitType[] availableOutfitTypes;

		// Token: 0x04003BD6 RID: 15318
		private Dictionary<OutfitType, List<StyleItemSO>> stylesByType = new Dictionary<OutfitType, List<StyleItemSO>>();

		// Token: 0x04003BD7 RID: 15319
		private Dictionary<OutfitType, StyleItemSO> currentStyles = new Dictionary<OutfitType, StyleItemSO>();

		// Token: 0x04003BD8 RID: 15320
		private Dictionary<string, OutfitType> outfitTypeMap = new Dictionary<string, OutfitType>();
	}
}
