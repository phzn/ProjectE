using System;
using System.Collections.Generic;
using BoZo.CharacterCreator.MC.StyleSystem;
using UnityEngine;

namespace Bozo.AnimeCharacters
{
	// Token: 0x020008A2 RID: 2210
	[CreateAssetMenu(fileName = "New Character SubCategory", menuName = "Character Creator/Character SubCategory")]
	public class CharacterSubCategorySO : ScriptableObject
	{
		// Token: 0x04003B9E RID: 15262
		[Tooltip("Nome della sottocategoria")]
		public string subCategoryName;

		// Token: 0x04003B9F RID: 15263
		[Tooltip("Icona della sottocategoria (opzionale)")]
		public Sprite icon;

		// Token: 0x04003BA0 RID: 15264
		[Tooltip("Descrizione della sottocategoria (opzionale)")]
		[TextArea(2, 5)]
		public string description;

		// Token: 0x04003BA1 RID: 15265
		[Tooltip("La sottocategoria ha parti distinte?")]
		public bool hasParts;

		// Token: 0x04003BA2 RID: 15266
		[Tooltip("Parti della sottocategoria (se hasParts = true)")]
		public List<CharacterPartSO> parts = new List<CharacterPartSO>();

		// Token: 0x04003BA3 RID: 15267
		[Tooltip("Tab di configurazione (se hasParts = false)")]
		public List<ConfigTabSO> configTabs = new List<ConfigTabSO>();

		// Token: 0x04003BA4 RID: 15268
		[Header("MC Style (optional)")]
		[Tooltip("If assigned, Style tab uses the MC style customization pipeline for this subcategory.")]
		public MCStylePartDefinitionSO stylePartDefinition;

		// Token: 0x04003BA5 RID: 15269
		[Header("Camera")]
		[Tooltip("Normalized camera zoom to tween to when this subcategory is selected (-1 = no change, 0 = full body, 1 = close-up)")]
		[Range(-1f, 1f)]
		public float cameraPositionIndex = -1f;

		// Token: 0x04003BA6 RID: 15270
		[Tooltip("Vertical framing offset in world units for this subcategory.")]
		[Range(-1.5f, 1.5f)]
		public float cameraVerticalOffset;

		// Token: 0x04003BA7 RID: 15271
		[Tooltip("Optional specific camera to activate for this subcategory (null = use default)")]
		public Camera overrideCamera;
	}
}
