using System;
using System.Collections;
using Bozo.AnimeCharacters;
using Features.MainMenu;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectExhibitionist.CharacterCustomization.Core.Runtime
{
	// Token: 0x02000B04 RID: 2820
	[DisallowMultipleComponent]
	public sealed class CustomizationUiPolishController : MonoBehaviour
	{
		// Token: 0x06004FCB RID: 20427 RVA: 0x001EE774 File Offset: 0x001EC974
		private void OnEnable()
		{
			base.StartCoroutine(this.MaintainDynamicUi());
		}

		// Token: 0x06004FCC RID: 20428 RVA: 0x001EE783 File Offset: 0x001EC983
		public void Configure(Transform root)
		{
			this.uiRoot = root;
			this.ApplyNow();
		}

		// Token: 0x06004FCD RID: 20429 RVA: 0x001EE794 File Offset: 0x001EC994
		public void ApplyNow()
		{
			foreach (Selectable selectable in ((this.uiRoot != null) ? this.uiRoot : base.transform).GetComponentsInChildren<Selectable>(true))
			{
				if (!(selectable == null) && !(selectable is Scrollbar))
				{
					if (this.improveHitTargets && (selectable is Button || selectable is Toggle))
					{
						CustomizationUiPolishController.EnsureFullRectRaycastTarget(selectable);
					}
					if (this.addInteractionFeedback && (selectable is Button || selectable is Toggle))
					{
						MainMenuButtonFx mainMenuButtonFx = selectable.GetComponent<MainMenuButtonFx>();
						if (mainMenuButtonFx == null)
						{
							mainMenuButtonFx = selectable.gameObject.AddComponent<MainMenuButtonFx>();
						}
						if (selectable.GetComponentInParent<CharacterCard>() != null)
						{
							mainMenuButtonFx.SetTintChildGraphics(false);
						}
						else if (CustomizationUiPolishController.IsColorValueControl(selectable))
						{
							mainMenuButtonFx.SetTintChildGraphics(false);
							mainMenuButtonFx.SetScaleEffectsEnabled(false);
						}
						else if (selectable.GetComponentInParent<SubCategorySelector>() != null)
						{
							mainMenuButtonFx.SetTintChildGraphics(false);
							mainMenuButtonFx.SetScaleEffectsEnabled(false);
						}
						else if (CustomizationUiPolishController.IsInsideHorizontalNavigationLayout(selectable.transform) || CustomizationUiPolishController.IsNavigationControl(selectable))
						{
							mainMenuButtonFx.SetScaleEffectsEnabled(false);
						}
					}
				}
			}
		}

		// Token: 0x06004FCE RID: 20430 RVA: 0x001EE8B4 File Offset: 0x001ECAB4
		private static bool IsInsideHorizontalNavigationLayout(Transform selectable)
		{
			Transform transform = ((selectable != null) ? selectable.parent : null);
			int num = 0;
			while (transform != null && num < 4)
			{
				if (transform.GetComponent<HorizontalLayoutGroup>() != null)
				{
					return true;
				}
				num++;
				transform = transform.parent;
			}
			return false;
		}

		// Token: 0x06004FCF RID: 20431 RVA: 0x001EE904 File Offset: 0x001ECB04
		private static bool IsColorValueControl(Selectable selectable)
		{
			if (selectable == null)
			{
				return false;
			}
			if (selectable.GetComponentInParent<ColorPreviewUI>() != null)
			{
				return true;
			}
			string text = selectable.name ?? string.Empty;
			return text.StartsWith("Color -", StringComparison.OrdinalIgnoreCase) || text.StartsWith("Body Color -", StringComparison.OrdinalIgnoreCase) || text.StartsWith("Skin Preset", StringComparison.OrdinalIgnoreCase) || text.StartsWith("Palette ", StringComparison.OrdinalIgnoreCase);
		}

		// Token: 0x06004FD0 RID: 20432 RVA: 0x001EE974 File Offset: 0x001ECB74
		private static bool IsNavigationControl(Selectable selectable)
		{
			string text = ((selectable != null) ? selectable.name : string.Empty);
			return text.IndexOf("Option", StringComparison.OrdinalIgnoreCase) >= 0 || text.IndexOf("Tab", StringComparison.OrdinalIgnoreCase) >= 0 || text.IndexOf("Category", StringComparison.OrdinalIgnoreCase) >= 0 || text.IndexOf("Part", StringComparison.OrdinalIgnoreCase) >= 0;
		}

		// Token: 0x06004FD1 RID: 20433 RVA: 0x001EE9D9 File Offset: 0x001ECBD9
		private IEnumerator MaintainDynamicUi()
		{
			yield return null;
			while (base.isActiveAndEnabled)
			{
				this.ApplyNow();
				yield return new WaitForSecondsRealtime(0.5f);
			}
			yield break;
		}

		// Token: 0x06004FD2 RID: 20434 RVA: 0x001EE9E8 File Offset: 0x001ECBE8
		private static void EnsureFullRectRaycastTarget(Selectable selectable)
		{
			Graphic component = selectable.GetComponent<Graphic>();
			if (component == null)
			{
				Image image = selectable.gameObject.AddComponent<Image>();
				image.color = new Color(1f, 1f, 1f, 0.001f);
				image.raycastTarget = true;
				return;
			}
			component.raycastTarget = true;
		}

		// Token: 0x040050C4 RID: 20676
		[SerializeField]
		private Transform uiRoot;

		// Token: 0x040050C5 RID: 20677
		[SerializeField]
		private bool improveHitTargets = true;

		// Token: 0x040050C6 RID: 20678
		[SerializeField]
		private bool addInteractionFeedback = true;
	}
}
