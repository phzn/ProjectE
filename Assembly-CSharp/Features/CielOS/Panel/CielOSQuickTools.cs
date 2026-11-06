using System;
using Features.CielOS.UI;
using Features.CielOS.WindowManager;
using Features.Localization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.CielOS.Panel
{
	// Token: 0x020013DE RID: 5086
	public class CielOSQuickTools : MonoBehaviour
	{
		// Token: 0x06009D08 RID: 40200 RVA: 0x004072CD File Offset: 0x004054CD
		private void Start()
		{
			this.Build();
		}

		// Token: 0x06009D09 RID: 40201 RVA: 0x004072D5 File Offset: 0x004054D5
		private void OnRectTransformDimensionsChange()
		{
			this._layoutDirty = true;
		}

		// Token: 0x06009D0A RID: 40202 RVA: 0x004072E0 File Offset: 0x004054E0
		private void LateUpdate()
		{
			RectTransform rectTransform = base.transform.parent as RectTransform;
			if (!this._built || rectTransform == null)
			{
				return;
			}
			if (this._layoutDirty || this._lastParentSize != rectTransform.rect.size)
			{
				this.RefreshLayout();
			}
		}

		// Token: 0x06009D0B RID: 40203 RVA: 0x0040733C File Offset: 0x0040553C
		public void Rebuild()
		{
			if (!this._built)
			{
				this.Build();
				return;
			}
			if (this.tileParent != null)
			{
				for (int i = this.tileParent.childCount - 1; i >= 0; i--)
				{
					GameObject gameObject = this.tileParent.GetChild(i).gameObject;
					gameObject.SetActive(false);
					if (Application.isPlaying)
					{
						Object.Destroy(gameObject);
					}
					else
					{
						Object.DestroyImmediate(gameObject);
					}
				}
			}
			this.PopulateTiles();
		}

		// Token: 0x06009D0C RID: 40204 RVA: 0x004073B4 File Offset: 0x004055B4
		private void Build()
		{
			if (this._built || this.catalog == null)
			{
				return;
			}
			this._built = true;
			this._preferredWidth = ((RectTransform)base.transform).rect.width;
			if (this._preferredWidth <= 0f)
			{
				this._preferredWidth = (float)this.columns * (this.cellSize.x + 8f) + 12f;
			}
			CielOSTheme active = CielOSTheme.Active;
			Image component = base.GetComponent<Image>();
			if (component != null && active != null)
			{
				component.color = active.windowBackground;
				CielOSTheme.Round(component);
				CielOSTheme.AddBorder((RectTransform)base.transform, null, 1f);
			}
			if (this.tileParent == null)
			{
				GameObject gameObject = new GameObject("Title", new Type[]
				{
					typeof(RectTransform),
					typeof(TextMeshProUGUI)
				});
				gameObject.transform.SetParent(base.transform, false);
				RectTransform rectTransform = (RectTransform)gameObject.transform;
				rectTransform.anchorMin = new Vector2(0f, 1f);
				rectTransform.anchorMax = new Vector2(1f, 1f);
				rectTransform.pivot = new Vector2(0.5f, 1f);
				rectTransform.sizeDelta = new Vector2(0f, 22f);
				rectTransform.anchoredPosition = new Vector2(0f, -8f);
				rectTransform.offsetMin = new Vector2(12f, rectTransform.offsetMin.y);
				rectTransform.offsetMax = new Vector2(-12f, rectTransform.offsetMax.y);
				TextMeshProUGUI component2 = gameObject.GetComponent<TextMeshProUGUI>();
				component2.text = "Quick Tools";
				component2.fontSize = 13f;
				component2.fontStyle = FontStyles.Bold;
				component2.alignment = TextAlignmentOptions.MidlineLeft;
				component2.color = ((active != null) ? active.panelText : Color.white);
				component2.raycastTarget = false;
				if (active != null && active.uiFont != null)
				{
					component2.font = active.uiFont;
				}
				LocalizedGameText.BindSource(component2, "cielos_shell", "Quick Tools", null).FitTranslationToRectangle(false);
				GameObject gameObject2 = new GameObject("Grid", new Type[]
				{
					typeof(RectTransform),
					typeof(GridLayoutGroup)
				});
				gameObject2.transform.SetParent(base.transform, false);
				this.tileParent = (RectTransform)gameObject2.transform;
				this.tileParent.anchorMin = new Vector2(0f, 0f);
				this.tileParent.anchorMax = new Vector2(1f, 1f);
				this.tileParent.offsetMin = new Vector2(10f, 10f);
				this.tileParent.offsetMax = new Vector2(-10f, -34f);
				GridLayoutGroup component3 = gameObject2.GetComponent<GridLayoutGroup>();
				component3.cellSize = this.cellSize;
				component3.spacing = new Vector2(8f, 8f);
				component3.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
				component3.constraintCount = this.columns;
			}
			GameObject gameObject3 = new GameObject("Viewport", new Type[]
			{
				typeof(RectTransform),
				typeof(Image),
				typeof(RectMask2D)
			});
			this._viewport = (RectTransform)gameObject3.transform;
			this._viewport.SetParent(base.transform, false);
			this._viewport.anchorMin = Vector2.zero;
			this._viewport.anchorMax = Vector2.one;
			this._viewport.offsetMin = new Vector2(10f, 10f);
			this._viewport.offsetMax = new Vector2(-10f, -34f);
			gameObject3.GetComponent<Image>().color = Color.clear;
			this.tileParent.SetParent(this._viewport, false);
			this.tileParent.anchorMin = new Vector2(0f, 1f);
			this.tileParent.anchorMax = Vector2.one;
			this.tileParent.pivot = new Vector2(0.5f, 1f);
			this.tileParent.anchoredPosition = Vector2.zero;
			this.tileParent.sizeDelta = Vector2.zero;
			this._grid = this.tileParent.GetComponent<GridLayoutGroup>();
			if (this._grid == null)
			{
				this._grid = this.tileParent.gameObject.AddComponent<GridLayoutGroup>();
			}
			this._grid.spacing = new Vector2(8f, 8f);
			this._grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
			this._grid.childAlignment = TextAnchor.UpperLeft;
			this._scroll = gameObject3.AddComponent<ScrollRect>();
			this._scroll.viewport = this._viewport;
			this._scroll.content = this.tileParent;
			this._scroll.horizontal = false;
			this._scroll.movementType = ScrollRect.MovementType.Clamped;
			this._scroll.scrollSensitivity = 24f;
			GameObject gameObject4 = new GameObject("Scrollbar", new Type[]
			{
				typeof(RectTransform),
				typeof(Image),
				typeof(Scrollbar)
			});
			RectTransform rectTransform2 = (RectTransform)gameObject4.transform;
			rectTransform2.SetParent(base.transform, false);
			rectTransform2.anchorMin = new Vector2(1f, 0f);
			rectTransform2.anchorMax = Vector2.one;
			rectTransform2.offsetMin = new Vector2(-15f, 10f);
			rectTransform2.offsetMax = new Vector2(-9f, -34f);
			gameObject4.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.08f);
			GameObject gameObject5 = new GameObject("Handle", new Type[]
			{
				typeof(RectTransform),
				typeof(Image)
			});
			RectTransform rectTransform3 = (RectTransform)gameObject5.transform;
			rectTransform3.SetParent(rectTransform2, false);
			rectTransform3.anchorMin = Vector2.zero;
			rectTransform3.anchorMax = Vector2.one;
			rectTransform3.offsetMin = (rectTransform3.offsetMax = Vector2.zero);
			Image component4 = gameObject5.GetComponent<Image>();
			component4.color = new Color(1f, 1f, 1f, 0.45f);
			CielOSTheme.Round(component4);
			Scrollbar component5 = gameObject4.GetComponent<Scrollbar>();
			component5.handleRect = rectTransform3;
			component5.targetGraphic = component4;
			component5.direction = Scrollbar.Direction.BottomToTop;
			this._scroll.verticalScrollbar = component5;
			this._scroll.verticalScrollbarVisibility = ScrollRect.ScrollbarVisibility.Permanent;
			this.PopulateTiles();
		}

		// Token: 0x06009D0D RID: 40205 RVA: 0x00407AA4 File Offset: 0x00405CA4
		private void PopulateTiles()
		{
			if (this.catalog == null || this.tileParent == null)
			{
				return;
			}
			this._tileCount = 0;
			foreach (CielOSAppDefinition cielOSAppDefinition in this.catalog.apps)
			{
				if (!(cielOSAppDefinition == null) && !cielOSAppDefinition.hidden && cielOSAppDefinition.IsVisible && CielOSQuickToolsPrefs.IsPinned(cielOSAppDefinition.appId, cielOSAppDefinition.showInQuickTools))
				{
					string id = cielOSAppDefinition.appId;
					CielOSTileFactory.BuildAppTile(this.tileParent, id, cielOSAppDefinition.displayName, 34f, true, false, cielOSAppDefinition.isCustomApp, !cielOSAppDefinition.IsPlatformSupported, true).Button.onClick.AddListener(delegate
					{
						if (this.windowManager != null)
						{
							this.windowManager.LaunchApp(id);
						}
					});
					this._tileCount++;
				}
			}
			this.RefreshLayout();
			if (this._scroll != null)
			{
				this._scroll.verticalNormalizedPosition = 1f;
			}
		}

		// Token: 0x06009D0E RID: 40206 RVA: 0x00407BE4 File Offset: 0x00405DE4
		private void RefreshLayout()
		{
			if (this._grid == null || this._viewport == null)
			{
				return;
			}
			RectTransform rectTransform = (RectTransform)base.transform;
			RectTransform rectTransform2 = rectTransform.parent as RectTransform;
			if (rectTransform2 == null)
			{
				return;
			}
			this._lastParentSize = rectTransform2.rect.size;
			float num = Mathf.Min(this._preferredWidth, Mathf.Max(80f, rectTransform2.rect.width - 48f));
			int num2 = Mathf.Clamp(Mathf.FloorToInt((num - 12f) / (this.cellSize.x + 8f)), 1, Mathf.Max(1, this.columns));
			int num3 = Mathf.CeilToInt((float)this._tileCount / (float)num2);
			float num4 = (float)num3 * this.cellSize.y + (float)Mathf.Max(0, num3 - 1) * 8f;
			float num5 = Mathf.Min(num4 + 44f, Mathf.Max(80f, rectTransform2.rect.height - 72f));
			bool flag = num4 > num5 - 44f + 0.01f;
			float num6 = (flag ? 12f : 0f);
			float num7 = Mathf.Max(1f, (num - 20f - num6 - (float)(num2 - 1) * 8f) / (float)num2);
			this._viewport.offsetMax = new Vector2(-10f - num6, -34f);
			rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, num);
			rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, num5);
			this._grid.constraintCount = num2;
			this._grid.cellSize = new Vector2(num7, this.cellSize.y);
			this.tileParent.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, num4);
			this._scroll.vertical = flag;
			this._scroll.verticalScrollbar.gameObject.SetActive(flag);
			LayoutRebuilder.ForceRebuildLayoutImmediate(this.tileParent);
			this._layoutDirty = false;
		}

		// Token: 0x0400A186 RID: 41350
		[SerializeField]
		private CielOSAppCatalogSO catalog;

		// Token: 0x0400A187 RID: 41351
		[SerializeField]
		private CielOSWindowManager windowManager;

		// Token: 0x0400A188 RID: 41352
		[Tooltip("Parent (GridLayoutGroup) for the tiles. If null, created on this object.")]
		[SerializeField]
		private RectTransform tileParent;

		// Token: 0x0400A189 RID: 41353
		[SerializeField]
		private int columns = 3;

		// Token: 0x0400A18A RID: 41354
		[SerializeField]
		private Vector2 cellSize = new Vector2(72f, 64f);

		// Token: 0x0400A18B RID: 41355
		private bool _built;

		// Token: 0x0400A18C RID: 41356
		private RectTransform _viewport;

		// Token: 0x0400A18D RID: 41357
		private ScrollRect _scroll;

		// Token: 0x0400A18E RID: 41358
		private GridLayoutGroup _grid;

		// Token: 0x0400A18F RID: 41359
		private float _preferredWidth;

		// Token: 0x0400A190 RID: 41360
		private Vector2 _lastParentSize;

		// Token: 0x0400A191 RID: 41361
		private bool _layoutDirty = true;

		// Token: 0x0400A192 RID: 41362
		private int _tileCount;
	}
}
