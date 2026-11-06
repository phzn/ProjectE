using System;
using System.Collections.Generic;
using System.Linq;
using Bozo.AnimeCharacters;
using UnityEngine;
using UnityEngine.UI;

namespace BoZo.CharacterCreator
{
	// Token: 0x020007B7 RID: 1975
	public class RigModifiersUIController : MonoBehaviour
	{
		// Token: 0x06003444 RID: 13380 RVA: 0x0012A31C File Offset: 0x0012851C
		private void Start()
		{
			if (this.headCustomizationController != null)
			{
				this.headCustomizationController.OnSubcategoryChanged += this.HandleHeadSubcategoryChanged;
				this.headCustomizationController.OnPartChanged += this.HandleHeadPartChanged;
				this.headCustomizationController.OnConfigTabChanged += this.HandleHeadConfigTabChanged;
			}
			if (this.bodyCustomizationController != null)
			{
				this.bodyCustomizationController.OnSubcategoryChanged += this.HandleBodySubcategoryChanged;
				this.bodyCustomizationController.OnPartChanged += this.HandleBodyPartChanged;
				this.bodyCustomizationController.OnConfigTabChanged += this.HandleBodyConfigTabChanged;
			}
			this.InitializeDefaultSelections();
		}

		// Token: 0x06003445 RID: 13381 RVA: 0x0012A3D8 File Offset: 0x001285D8
		private void OnDestroy()
		{
			if (this.headCustomizationController != null)
			{
				this.headCustomizationController.OnSubcategoryChanged -= this.HandleHeadSubcategoryChanged;
				this.headCustomizationController.OnPartChanged -= this.HandleHeadPartChanged;
				this.headCustomizationController.OnConfigTabChanged -= this.HandleHeadConfigTabChanged;
			}
			if (this.bodyCustomizationController != null)
			{
				this.bodyCustomizationController.OnSubcategoryChanged -= this.HandleBodySubcategoryChanged;
				this.bodyCustomizationController.OnPartChanged -= this.HandleBodyPartChanged;
				this.bodyCustomizationController.OnConfigTabChanged -= this.HandleBodyConfigTabChanged;
			}
		}

		// Token: 0x06003446 RID: 13382 RVA: 0x0012A48B File Offset: 0x0012868B
		private void InitializeDefaultSelections()
		{
			this.currentCategory = "Head";
			this.currentSubcategory = "";
			this.currentPart = "";
			this.currentConfigTab = "";
			Debug.Log("RigModifiersUIController: Default selections initialized");
		}

		// Token: 0x06003447 RID: 13383 RVA: 0x0012A4C4 File Offset: 0x001286C4
		private void HandleHeadSubcategoryChanged(CharacterSubCategorySO subcategory)
		{
			if (subcategory == null)
			{
				return;
			}
			Debug.Log("RigModifiersUIController: Head subcategory changed to " + subcategory.subCategoryName);
			this.currentCategory = "Head";
			this.currentSubcategory = subcategory.subCategoryName;
			this.currentPart = "";
			this.ClearModSliders();
		}

		// Token: 0x06003448 RID: 13384 RVA: 0x0012A518 File Offset: 0x00128718
		private void HandleHeadPartChanged(CharacterPartSO part)
		{
			if (part == null)
			{
				return;
			}
			Debug.Log("RigModifiersUIController: Head part changed to " + part.partName);
			this.currentPart = part.partName;
			this.ClearModSliders();
		}

		// Token: 0x06003449 RID: 13385 RVA: 0x0012A54C File Offset: 0x0012874C
		private void HandleHeadConfigTabChanged(ConfigTabSO configTab)
		{
			if (configTab == null)
			{
				return;
			}
			Debug.Log("RigModifiersUIController: Head config tab changed to " + configTab.tabName);
			this.currentConfigTab = configTab.tabName;
			if (configTab.tabName.Equals("Shapes", StringComparison.OrdinalIgnoreCase))
			{
				this.CreateUIControlsForCurrentSelection();
				return;
			}
			this.ClearModSliders();
		}

		// Token: 0x0600344A RID: 13386 RVA: 0x0012A5A4 File Offset: 0x001287A4
		private void HandleBodySubcategoryChanged(CharacterSubCategorySO subcategory)
		{
			if (subcategory == null)
			{
				return;
			}
			Debug.Log("RigModifiersUIController: Body subcategory changed to " + subcategory.subCategoryName);
			this.currentCategory = "Body";
			this.currentSubcategory = subcategory.subCategoryName;
			this.currentPart = "";
			this.ClearModSliders();
		}

		// Token: 0x0600344B RID: 13387 RVA: 0x0012A5F8 File Offset: 0x001287F8
		private void HandleBodyPartChanged(CharacterPartSO part)
		{
			if (part == null)
			{
				return;
			}
			Debug.Log("RigModifiersUIController: Body part changed to " + part.partName);
			this.currentPart = part.partName;
			this.ClearModSliders();
		}

		// Token: 0x0600344C RID: 13388 RVA: 0x0012A62C File Offset: 0x0012882C
		private void HandleBodyConfigTabChanged(ConfigTabSO configTab)
		{
			if (configTab == null)
			{
				return;
			}
			Debug.Log("RigModifiersUIController: Body config tab changed to " + configTab.tabName);
			this.currentConfigTab = configTab.tabName;
			if (configTab.tabName.Equals("Shapes", StringComparison.OrdinalIgnoreCase))
			{
				this.CreateUIControlsForCurrentSelection();
				return;
			}
			this.ClearModSliders();
		}

		// Token: 0x0600344D RID: 13389 RVA: 0x0012A684 File Offset: 0x00128884
		private void CreateUIControlsForCurrentSelection()
		{
			if (this.character == null)
			{
				Debug.LogWarning("RigModifiersUIController: Character reference is not assigned!");
				return;
			}
			if (this.modSliderPrefab == null)
			{
				Debug.LogWarning("RigModifiersUIController: ModSliderPrefab is not assigned!");
				return;
			}
			this.ClearModSliders();
			List<BodyShapeModifier> list = this.character.GetMods().Values.ToList<BodyShapeModifier>();
			Transform containerByCategory = this.GetContainerByCategory(this.currentCategory);
			if (containerByCategory == null)
			{
				Debug.LogWarning("RigModifiersUIController: No container found for category " + this.currentCategory);
				return;
			}
			list = list.Where<BodyShapeModifier>((BodyShapeModifier mod) => mod.category == this.currentCategory && mod.subcategory == this.currentSubcategory && mod.part == this.currentPart).ToList<BodyShapeModifier>();
			Debug.Log(string.Format("RigModifiersUIController: Found {0} modifiers total", list.Count));
			containerByCategory.gameObject.SetActive(false);
			foreach (BodyShapeModifier bodyShapeModifier in list)
			{
				this.CreateSliderForModifier(bodyShapeModifier, containerByCategory);
			}
			containerByCategory.gameObject.SetActive(true);
			RectTransform rectTransform = containerByCategory as RectTransform;
			if (rectTransform != null)
			{
				Canvas.ForceUpdateCanvases();
				LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
			}
			Debug.Log(string.Format("RigModifiersUIController: Created {0} modifier sliders for category {1}", this.activeModSliders.Count, this.currentCategory));
		}

		// Token: 0x0600344E RID: 13390 RVA: 0x0012A7D4 File Offset: 0x001289D4
		private Transform GetContainerByCategory(string category)
		{
			if (category == "Head")
			{
				return this.faceModContainer;
			}
			return this.bodyModContainer;
		}

		// Token: 0x0600344F RID: 13391 RVA: 0x0012A7F0 File Offset: 0x001289F0
		private void CreateSliderForModifier(BodyShapeModifier modifier, Transform container)
		{
			if (this.modSliderPrefab == null || container == null)
			{
				Debug.LogWarning("RigModifiersUIController: Cannot create slider - prefab or container is null");
				return;
			}
			GameObject gameObject = Object.Instantiate<GameObject>(this.modSliderPrefab, container);
			BodyShapeSliders component = gameObject.GetComponent<BodyShapeSliders>();
			if (component == null)
			{
				Debug.LogWarning("RigModifiersUIController: ModSliderPrefab does not have BodyShapeSliders component!");
				Object.Destroy(gameObject);
				return;
			}
			component.Init(this.character, modifier);
			this.activeModSliders.Add(component);
			Debug.Log("RigModifiersUIController: Created slider for modifier " + modifier.shapeName + " in container " + container.name);
		}

		// Token: 0x06003450 RID: 13392 RVA: 0x0012A888 File Offset: 0x00128A88
		private void ClearModSliders()
		{
			foreach (BodyShapeSliders bodyShapeSliders in this.activeModSliders)
			{
				if (bodyShapeSliders != null && bodyShapeSliders.gameObject != null)
				{
					Object.Destroy(bodyShapeSliders.gameObject);
				}
			}
			this.activeModSliders.Clear();
			this.ClearContainer(this.faceModContainer);
			this.ClearContainer(this.bodyModContainer);
			Debug.Log("RigModifiersUIController: All modifier sliders cleared");
		}

		// Token: 0x06003451 RID: 13393 RVA: 0x0012A924 File Offset: 0x00128B24
		private void ClearContainer(Transform container)
		{
			if (container == null)
			{
				return;
			}
			List<GameObject> list = new List<GameObject>();
			foreach (object obj in container)
			{
				Transform transform = (Transform)obj;
				list.Add(transform.gameObject);
			}
			foreach (GameObject gameObject in list)
			{
				Object.DestroyImmediate(gameObject);
			}
			if (container is RectTransform)
			{
				Canvas.ForceUpdateCanvases();
				LayoutRebuilder.ForceRebuildLayoutImmediate(container as RectTransform);
			}
		}

		// Token: 0x06003452 RID: 13394 RVA: 0x0012A9E0 File Offset: 0x00128BE0
		public void RefreshModifierUI()
		{
			if (this.currentConfigTab.Equals("Shapes", StringComparison.OrdinalIgnoreCase))
			{
				this.CreateUIControlsForCurrentSelection();
			}
		}

		// Token: 0x06003453 RID: 13395 RVA: 0x0012A9FB File Offset: 0x00128BFB
		public void SetCharacter(OutfitSystem newCharacter)
		{
			this.character = newCharacter;
			this.RefreshModifierUI();
		}

		// Token: 0x040034B1 RID: 13489
		[Header("Character References")]
		[SerializeField]
		private OutfitSystem character;

		// Token: 0x040034B2 RID: 13490
		[Header("UI References")]
		[SerializeField]
		private HeadCustomizationController headCustomizationController;

		// Token: 0x040034B3 RID: 13491
		[SerializeField]
		private BodyCustomizationController bodyCustomizationController;

		// Token: 0x040034B4 RID: 13492
		[Header("Face and Body Containers")]
		[SerializeField]
		private Transform faceModContainer;

		// Token: 0x040034B5 RID: 13493
		[SerializeField]
		private Transform bodyModContainer;

		// Token: 0x040034B6 RID: 13494
		[Header("Prefab")]
		[SerializeField]
		private GameObject modSliderPrefab;

		// Token: 0x040034B7 RID: 13495
		private string currentCategory;

		// Token: 0x040034B8 RID: 13496
		private string currentSubcategory;

		// Token: 0x040034B9 RID: 13497
		private string currentPart;

		// Token: 0x040034BA RID: 13498
		private string currentConfigTab;

		// Token: 0x040034BB RID: 13499
		private List<BodyShapeSliders> activeModSliders = new List<BodyShapeSliders>();
	}
}
