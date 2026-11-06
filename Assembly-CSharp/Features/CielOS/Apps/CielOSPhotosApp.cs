using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Features.CensorBars;
using Features.CielOS.UI;
using Features.CielOS.VFS;
using Features.CielOS.WindowManager;
using Features.Localization;
using Features.PhoneCompanion;
using ProjectExhibitionist.Core;
using ProjectExhibitionist.UI;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Features.CielOS.Apps
{
	// Token: 0x020013FF RID: 5119
	public sealed class CielOSPhotosApp : MonoBehaviour
	{
		// Token: 0x170018DE RID: 6366
		// (get) Token: 0x06009E00 RID: 40448 RVA: 0x0040F921 File Offset: 0x0040DB21
		public CielOSPhotosApp.MediaTab ActiveTab
		{
			get
			{
				return this._tab;
			}
		}

		// Token: 0x170018DF RID: 6367
		// (get) Token: 0x06009E01 RID: 40449 RVA: 0x0040F929 File Offset: 0x0040DB29
		public string CurrentDirectory
		{
			get
			{
				return this._currentDirectory;
			}
		}

		// Token: 0x06009E02 RID: 40450 RVA: 0x0040F934 File Offset: 0x0040DB34
		public void Initialize(TMP_Text pathLabel, RectTransform listContainer, Button upButton, Button newFolderButton, TMP_InputField oldFolderName)
		{
			if (this._initialized)
			{
				return;
			}
			this._pathLabel = pathLabel;
			this._listContainer = listContainer;
			this._upButton = upButton;
			this._newFolderButton = newFolderButton;
			this._oldFolderName = oldFolderName;
			if (this._oldFolderName != null)
			{
				this._oldFolderName.gameObject.SetActive(false);
			}
			this._scroll = ((this._listContainer != null) ? this._listContainer.GetComponentInParent<ScrollRect>() : null);
			this._viewport = ((this._scroll != null) ? this._scroll.viewport : null);
			if (this._viewport == null && this._scroll != null)
			{
				this._viewport = this._scroll.transform as RectTransform;
			}
			this.ConfigureContainer();
			this.ConfigureToolbar();
			this.BuildTabs();
			this.ConfigureScroll();
			this._initialized = true;
			this.Subscribe();
			CielOSPhotosSync.Ensure().SyncOnce();
			this.Refresh(true);
		}

		// Token: 0x06009E03 RID: 40451 RVA: 0x0040FA3C File Offset: 0x0040DC3C
		private void ConfigureContainer()
		{
			if (this._listContainer == null)
			{
				return;
			}
			VerticalLayoutGroup component = this._listContainer.GetComponent<VerticalLayoutGroup>();
			if (component != null)
			{
				component.enabled = false;
			}
			ContentSizeFitter component2 = this._listContainer.GetComponent<ContentSizeFitter>();
			if (component2 != null)
			{
				component2.enabled = false;
			}
			this._listContainer.anchorMin = new Vector2(0f, 1f);
			this._listContainer.anchorMax = new Vector2(1f, 1f);
			this._listContainer.pivot = new Vector2(0.5f, 1f);
		}

		// Token: 0x06009E04 RID: 40452 RVA: 0x0040FAE0 File Offset: 0x0040DCE0
		private void ConfigureToolbar()
		{
			CielOSContentText.BindButton(this._upButton, "Up");
			CielOSContentText.BindButton(this._newFolderButton, "New Folder");
			if (this._upButton != null)
			{
				this._upButton.onClick.RemoveAllListeners();
				this._upButton.onClick.AddListener(new UnityAction(this.GoUp));
				CielOSSfx.Attach(this._upButton);
			}
			if (this._newFolderButton != null)
			{
				this._newFolderButton.onClick.RemoveAllListeners();
				this._newFolderButton.onClick.AddListener(new UnityAction(this.CreateFolder));
				CielOSSfx.Attach(this._newFolderButton);
				this.BuildImportButton();
			}
		}

		// Token: 0x06009E05 RID: 40453 RVA: 0x0040FBA0 File Offset: 0x0040DDA0
		private void ConfigureScroll()
		{
			if (this._scroll == null)
			{
				return;
			}
			this._scroll.horizontal = false;
			this._scroll.vertical = true;
			this._scroll.movementType = ScrollRect.MovementType.Clamped;
			this._scroll.content = this._listContainer;
			this._smoothWheel = this._scroll.GetComponent<CielOSSmoothWheelScroll>();
			if (this._smoothWheel == null)
			{
				this._smoothWheel = this._scroll.gameObject.AddComponent<CielOSSmoothWheelScroll>();
			}
			this._smoothWheel.Bind(this._scroll);
			RectTransform rectTransform = this._scroll.transform as RectTransform;
			if (rectTransform != null)
			{
				rectTransform.anchorMin = Vector2.zero;
				rectTransform.anchorMax = Vector2.one;
				rectTransform.offsetMin = new Vector2(4f, 4f);
				rectTransform.offsetMax = new Vector2(-4f, -86f);
			}
		}

		// Token: 0x06009E06 RID: 40454 RVA: 0x0040FC94 File Offset: 0x0040DE94
		private void BuildTabs()
		{
			RectTransform rectTransform = base.transform as RectTransform;
			if (rectTransform == null)
			{
				return;
			}
			Transform transform = base.transform.Find("MediaTabs");
			if (transform != null)
			{
				Object.Destroy(transform.gameObject);
			}
			Image image = CielOSPhotosApp.NewImage("MediaTabs", rectTransform);
			RectTransform rectTransform2 = (RectTransform)image.transform;
			rectTransform2.anchorMin = new Vector2(0f, 1f);
			rectTransform2.anchorMax = new Vector2(1f, 1f);
			rectTransform2.pivot = new Vector2(0.5f, 1f);
			rectTransform2.offsetMin = new Vector2(4f, -82f);
			rectTransform2.offsetMax = new Vector2(-4f, -40f);
			CielOSTheme active = CielOSTheme.Active;
			image.color = ((active != null) ? active.windowBackground : new Color(0.11f, 0.09f, 0.13f, 1f));
			this._photosTabButton = this.MakeTab(rectTransform2, "PhotosTab", "PHOTOS", new Vector2(0f, 0f), out this._photosTabImage, out this._photosTabLabel);
			this._videosTabButton = this.MakeTab(rectTransform2, "VideosTab", "VIDEOS", new Vector2(126f, 0f), out this._videosTabImage, out this._videosTabLabel);
			this._photosTabButton.onClick.AddListener(delegate
			{
				this.SetTab(CielOSPhotosApp.MediaTab.Photos);
			});
			this._videosTabButton.onClick.AddListener(delegate
			{
				this.SetTab(CielOSPhotosApp.MediaTab.Videos);
			});
			CielOSSfx.Attach(this._photosTabButton);
			CielOSSfx.Attach(this._videosTabButton);
			this.UpdateTabVisuals();
		}

		// Token: 0x06009E07 RID: 40455 RVA: 0x0040FE4C File Offset: 0x0040E04C
		private Button MakeTab(RectTransform parent, string name, string text, Vector2 position, out Image image, out TMP_Text label)
		{
			GameObject gameObject = new GameObject(name, new Type[]
			{
				typeof(RectTransform),
				typeof(Image),
				typeof(Button)
			});
			gameObject.transform.SetParent(parent, false);
			RectTransform rectTransform = (RectTransform)gameObject.transform;
			Vector2 vector = new Vector2(0f, 0.5f);
			rectTransform.pivot = vector;
			rectTransform.anchorMin = (rectTransform.anchorMax = vector);
			rectTransform.anchoredPosition = new Vector2(12f + position.x, 0f);
			rectTransform.sizeDelta = new Vector2(116f, 30f);
			image = gameObject.GetComponent<Image>();
			CielOSTheme.Round(image);
			label = CielOSPhotosApp.MakeText(gameObject.transform, "Label", text, 12f, FontStyles.Bold, TextAlignmentOptions.Center);
			CielOSPhotosApp.Stretch((RectTransform)label.transform);
			CielOSContentText.Bind(label, text);
			return gameObject.GetComponent<Button>();
		}

		// Token: 0x06009E08 RID: 40456 RVA: 0x0040FF50 File Offset: 0x0040E150
		private void BuildImportButton()
		{
			Transform parent = this._newFolderButton.transform.parent;
			Transform transform = parent.Find("PhotosImportButton");
			if (transform != null)
			{
				Object.Destroy(transform.gameObject);
			}
			GameObject gameObject = Object.Instantiate<GameObject>(this._newFolderButton.gameObject, parent, false);
			gameObject.name = "PhotosImportButton";
			Button component = gameObject.GetComponent<Button>();
			component.onClick.RemoveAllListeners();
			component.onClick.AddListener(new UnityAction(this.ImportMedia));
			RectTransform rectTransform = (RectTransform)gameObject.transform;
			RectTransform rectTransform2 = (RectTransform)this._newFolderButton.transform;
			rectTransform.anchoredPosition = rectTransform2.anchoredPosition + new Vector2(-(rectTransform2.sizeDelta.x + 8f), 0f);
			CielOSContentText.Bind(gameObject.GetComponentInChildren<TMP_Text>(true), "Import");
			CielOSSfx.Attach(component);
		}

		// Token: 0x06009E09 RID: 40457 RVA: 0x00410032 File Offset: 0x0040E232
		private void Subscribe()
		{
			if (this._subscribed)
			{
				return;
			}
			CielOSPhotosSync.Changed += this.HandleMediaChanged;
			this._subscribed = true;
		}

		// Token: 0x06009E0A RID: 40458 RVA: 0x00410055 File Offset: 0x0040E255
		private void Unsubscribe()
		{
			if (!this._subscribed)
			{
				return;
			}
			CielOSPhotosSync.Changed -= this.HandleMediaChanged;
			this._subscribed = false;
		}

		// Token: 0x06009E0B RID: 40459 RVA: 0x00410078 File Offset: 0x0040E278
		private void OnEnable()
		{
			if (!this._initialized)
			{
				return;
			}
			this.Subscribe();
			this.Refresh(false);
		}

		// Token: 0x06009E0C RID: 40460 RVA: 0x00410090 File Offset: 0x0040E290
		private void OnDisable()
		{
			this.Unsubscribe();
			this.ClearFolderHighlight();
		}

		// Token: 0x06009E0D RID: 40461 RVA: 0x004100A0 File Offset: 0x0040E2A0
		private void OnDestroy()
		{
			this.Unsubscribe();
			foreach (Sprite sprite in this._thumbnailCache.Values)
			{
				CielOSPhotosApp.DestroyRuntimeSprite(sprite);
			}
			this._thumbnailCache.Clear();
		}

		// Token: 0x06009E0E RID: 40462 RVA: 0x00410108 File Offset: 0x0040E308
		private void LateUpdate()
		{
			if (!this._initialized || this._listContainer == null || !base.isActiveAndEnabled)
			{
				return;
			}
			float width = this._listContainer.rect.width;
			if (width > 1f && Mathf.Abs(width - this._lastLayoutWidth) > 0.5f)
			{
				this.LayoutTiles();
			}
		}

		// Token: 0x06009E0F RID: 40463 RVA: 0x0041016A File Offset: 0x0040E36A
		private void HandleMediaChanged()
		{
			if (base.isActiveAndEnabled)
			{
				this.Refresh(false);
			}
		}

		// Token: 0x06009E10 RID: 40464 RVA: 0x0041017B File Offset: 0x0040E37B
		private void SetTab(CielOSPhotosApp.MediaTab tab)
		{
			if (this._tab == tab)
			{
				return;
			}
			this.StoreCurrentDirectory();
			this._tab = tab;
			this._currentDirectory = ((tab == CielOSPhotosApp.MediaTab.Photos) ? this._photoDirectory : this._videoDirectory);
			this.UpdateTabVisuals();
			this.Refresh(true);
		}

		// Token: 0x06009E11 RID: 40465 RVA: 0x004101B8 File Offset: 0x0040E3B8
		private void UpdateTabVisuals()
		{
			CielOSTheme active = CielOSTheme.Active;
			Color color = ((active != null) ? active.accent : new Color(0.66f, 0.38f, 0.79f, 1f));
			Color color2 = ((active != null) ? active.controlBackground : new Color(0.2f, 0.16f, 0.26f, 1f));
			if (this._photosTabImage != null)
			{
				this._photosTabImage.color = ((this._tab == CielOSPhotosApp.MediaTab.Photos) ? color : color2);
			}
			if (this._videosTabImage != null)
			{
				this._videosTabImage.color = ((this._tab == CielOSPhotosApp.MediaTab.Videos) ? color : color2);
			}
			if (this._photosTabLabel != null)
			{
				this._photosTabLabel.color = ((this._tab == CielOSPhotosApp.MediaTab.Photos) ? Color.white : new Color(1f, 1f, 1f, 0.62f));
			}
			if (this._videosTabLabel != null)
			{
				this._videosTabLabel.color = ((this._tab == CielOSPhotosApp.MediaTab.Videos) ? Color.white : new Color(1f, 1f, 1f, 0.62f));
			}
		}

		// Token: 0x06009E12 RID: 40466 RVA: 0x004102EF File Offset: 0x0040E4EF
		private void StoreCurrentDirectory()
		{
			if (this._tab == CielOSPhotosApp.MediaTab.Photos)
			{
				this._photoDirectory = this._currentDirectory;
				return;
			}
			this._videoDirectory = this._currentDirectory;
		}

		// Token: 0x170018E0 RID: 6368
		// (get) Token: 0x06009E13 RID: 40467 RVA: 0x00410312 File Offset: 0x0040E512
		private string ActiveRoot
		{
			get
			{
				if (this._tab != CielOSPhotosApp.MediaTab.Photos)
				{
					return "/Videos";
				}
				return "/Photos";
			}
		}

		// Token: 0x06009E14 RID: 40468 RVA: 0x00410328 File Offset: 0x0040E528
		private void Refresh(bool resetScroll)
		{
			if (this._listContainer == null)
			{
				return;
			}
			this.ClearTiles();
			if (this._pathLabel != null)
			{
				string relative = (this._currentDirectory.Equals(this.ActiveRoot, StringComparison.OrdinalIgnoreCase) ? string.Empty : this._currentDirectory.Substring(Mathf.Min(this.ActiveRoot.Length, this._currentDirectory.Length)));
				LocalizedGameText.BindComputed(this._pathLabel, () => CielOSContentText.Get((this._tab == CielOSPhotosApp.MediaTab.Photos) ? "Photos" : "Videos") + relative);
			}
			if (this._upButton != null)
			{
				this._upButton.interactable = !this._currentDirectory.Equals(this.ActiveRoot, StringComparison.OrdinalIgnoreCase);
			}
			IEnumerable<CielOSFileSystem.Entry> enumerable = CielOSFileSystem.Instance.List(this._currentDirectory).Where<CielOSFileSystem.Entry>(new Func<CielOSFileSystem.Entry, bool>(this.ShouldShow));
			foreach (CielOSFileSystem.Entry entry in CielOSMediaLibrary.ApplyOrder(this._currentDirectory, enumerable))
			{
				this._tiles.Add(this.BuildTile(entry));
			}
			if (this._tiles.Count == 0)
			{
				this.BuildEmptyState();
			}
			if (resetScroll)
			{
				Vector2 anchoredPosition = this._listContainer.anchoredPosition;
				this._listContainer.anchoredPosition = new Vector2(anchoredPosition.x, 0f);
				if (this._scroll != null)
				{
					this._scroll.verticalNormalizedPosition = 1f;
				}
			}
			CielOSSmoothWheelScroll smoothWheel = this._smoothWheel;
			if (smoothWheel != null)
			{
				smoothWheel.ResetTarget();
			}
			this.ScheduleLayout();
		}

		// Token: 0x06009E15 RID: 40469 RVA: 0x004104E0 File Offset: 0x0040E6E0
		private bool ShouldShow(CielOSFileSystem.Entry entry)
		{
			if (entry.isDirectory)
			{
				return true;
			}
			if (this._tab != CielOSPhotosApp.MediaTab.Photos)
			{
				return CielOSMediaLibrary.IsVideoFileName(entry.name);
			}
			return CielOSPhotosSync.IsEligiblePhotoFileName(entry.name);
		}

		// Token: 0x06009E16 RID: 40470 RVA: 0x0041050B File Offset: 0x0040E70B
		private void ScheduleLayout()
		{
			if (this._layoutRoutine != null)
			{
				base.StopCoroutine(this._layoutRoutine);
			}
			if (base.isActiveAndEnabled)
			{
				this._layoutRoutine = base.StartCoroutine(this.LayoutNextFrame());
			}
		}

		// Token: 0x06009E17 RID: 40471 RVA: 0x0041053B File Offset: 0x0040E73B
		private IEnumerator LayoutNextFrame()
		{
			yield return null;
			Canvas.ForceUpdateCanvases();
			this.LayoutTiles();
			this._layoutRoutine = null;
			yield break;
		}

		// Token: 0x06009E18 RID: 40472 RVA: 0x0041054C File Offset: 0x0040E74C
		public static CielOSPhotosApp.GridMetrics CalculateGrid(float width, int itemCount)
		{
			float num = Mathf.Max(1f, width - 28f);
			int num2 = Mathf.Clamp(Mathf.RoundToInt(num / 280f), 1, 6);
			if (itemCount > 0)
			{
				num2 = Mathf.Min(num2, itemCount);
			}
			float num3 = (num - 14f * (float)Mathf.Max(0, num2 - 1)) / (float)num2;
			float num4 = Mathf.Min(300f, num3);
			float num5 = Mathf.Clamp(num4 - 12f, 112f, 288f);
			float num6 = (float)num2 * num4 + (float)Mathf.Max(0, num2 - 1) * 14f;
			return new CielOSPhotosApp.GridMetrics
			{
				columns = num2,
				cellWidth = num4,
				thumbnailSize = num5,
				cellHeight = num5 + 38f,
				gap = 14f,
				startX = Mathf.Max(14f, (width - num6) * 0.5f),
				paddingY = 14f
			};
		}

		// Token: 0x06009E19 RID: 40473 RVA: 0x0041063C File Offset: 0x0040E83C
		private void LayoutTiles()
		{
			if (this._listContainer == null)
			{
				return;
			}
			float width = this._listContainer.rect.width;
			if (width < 1f)
			{
				return;
			}
			this._lastLayoutWidth = width;
			if (this._tiles.Count == 0)
			{
				float num = ((this._viewport != null) ? this._viewport.rect.height : 160f);
				this._listContainer.sizeDelta = new Vector2(0f, Mathf.Max(160f, num));
				if (this._emptyState != null)
				{
					RectTransform rectTransform = (RectTransform)this._emptyState.transform;
					rectTransform.anchorMin = new Vector2(0f, 1f);
					rectTransform.anchorMax = new Vector2(1f, 1f);
					rectTransform.pivot = new Vector2(0.5f, 1f);
					rectTransform.anchoredPosition = new Vector2(0f, -24f);
					rectTransform.sizeDelta = new Vector2(0f, 120f);
				}
				return;
			}
			CielOSPhotosApp.GridMetrics gridMetrics = CielOSPhotosApp.CalculateGrid(width, this._tiles.Count);
			for (int i = 0; i < this._tiles.Count; i++)
			{
				CielOSPhotosApp.TileRecord tileRecord = this._tiles[i];
				int num2 = i % gridMetrics.columns;
				int num3 = i / gridMetrics.columns;
				RectTransform rect = tileRecord.rect;
				RectTransform rect2 = tileRecord.rect;
				RectTransform rect3 = tileRecord.rect;
				Vector2 vector = new Vector2(0f, 1f);
				rect3.pivot = vector;
				rect.anchorMin = (rect2.anchorMax = vector);
				tileRecord.rect.sizeDelta = new Vector2(gridMetrics.cellWidth, gridMetrics.cellHeight);
				tileRecord.rect.anchoredPosition = new Vector2(gridMetrics.startX + (float)num2 * (gridMetrics.cellWidth + gridMetrics.gap), -(gridMetrics.paddingY + (float)num3 * (gridMetrics.cellHeight + gridMetrics.gap)));
				RectTransform thumbnail = tileRecord.thumbnail;
				RectTransform thumbnail2 = tileRecord.thumbnail;
				RectTransform thumbnail3 = tileRecord.thumbnail;
				vector = new Vector2(0.5f, 1f);
				thumbnail3.pivot = vector;
				thumbnail.anchorMin = (thumbnail2.anchorMax = vector);
				tileRecord.thumbnail.anchoredPosition = new Vector2(0f, -6f);
				tileRecord.thumbnail.sizeDelta = new Vector2(gridMetrics.thumbnailSize, gridMetrics.thumbnailSize);
				tileRecord.label.anchorMin = new Vector2(0f, 0f);
				tileRecord.label.anchorMax = new Vector2(1f, 0f);
				tileRecord.label.pivot = new Vector2(0.5f, 0f);
				tileRecord.label.offsetMin = new Vector2(7f, 3f);
				tileRecord.label.offsetMax = new Vector2(-7f, 31f);
				if (tileRecord.preview != null)
				{
					CielOSPhotosApp.Cover(tileRecord.preview.rectTransform, tileRecord.preview.texture, gridMetrics.thumbnailSize, gridMetrics.thumbnailSize);
				}
				if (tileRecord.icon != null)
				{
					float num4 = Mathf.Clamp(gridMetrics.thumbnailSize * 0.36f, 42f, 104f);
					tileRecord.icon.sizeDelta = new Vector2(num4, num4);
				}
			}
			int num5 = (this._tiles.Count + gridMetrics.columns - 1) / gridMetrics.columns;
			float num6 = gridMetrics.paddingY * 2f + (float)num5 * gridMetrics.cellHeight + (float)Mathf.Max(0, num5 - 1) * gridMetrics.gap;
			float num7 = ((this._viewport != null) ? this._viewport.rect.height : 0f);
			this._listContainer.sizeDelta = new Vector2(0f, Mathf.Max(num6, num7));
		}

		// Token: 0x06009E1A RID: 40474 RVA: 0x00410A58 File Offset: 0x0040EC58
		private CielOSPhotosApp.TileRecord BuildTile(CielOSFileSystem.Entry entry)
		{
			CielOSTheme active = CielOSTheme.Active;
			bool video = !entry.isDirectory && CielOSMediaLibrary.IsVideoFileName(entry.name);
			Color color = ((active != null) ? active.controlBackground : new Color(0.18f, 0.15f, 0.22f, 1f));
			Color color2 = ((active != null) ? active.accent : new Color(0.66f, 0.38f, 0.79f, 1f));
			Color color3 = ((active != null) ? active.controlText : Color.white);
			GameObject gameObject = new GameObject(entry.name, new Type[]
			{
				typeof(RectTransform),
				typeof(Image),
				typeof(CielOSClickable),
				typeof(CielOSRightClickable)
			});
			gameObject.transform.SetParent(this._listContainer, false);
			RectTransform rectTransform = (RectTransform)gameObject.transform;
			Image component = gameObject.GetComponent<Image>();
			component.color = new Color(color.r, color.g, color.b, 0.64f);
			CielOSTheme.Round(component);
			GameObject gameObject2 = new GameObject("Thumbnail", new Type[]
			{
				typeof(RectTransform),
				typeof(Image),
				typeof(RectMask2D)
			});
			gameObject2.transform.SetParent(gameObject.transform, false);
			RectTransform rectTransform2 = (RectTransform)gameObject2.transform;
			gameObject2.GetComponent<Image>().color = (entry.isDirectory ? new Color(color2.r, color2.g, color2.b, 0.16f) : new Color(0.055f, 0.045f, 0.07f, 1f));
			RawImage rawImage = null;
			RectTransform rectTransform3 = null;
			if (!entry.isDirectory)
			{
				Sprite thumbnail = this.GetThumbnail(entry.virtualPath, video);
				GameObject gameObject3 = new GameObject("Preview", new Type[]
				{
					typeof(RectTransform),
					typeof(RawImage)
				});
				gameObject3.transform.SetParent(rectTransform2, false);
				rawImage = gameObject3.GetComponent<RawImage>();
				rawImage.texture = ((thumbnail != null) ? thumbnail.texture : null);
				StreamerCensorImage.Bind(rawImage, false);
				rawImage.raycastTarget = false;
				if (thumbnail == null)
				{
					rawImage.color = new Color(1f, 1f, 1f, 0.08f);
				}
				if (video)
				{
					this.AddVideoDecorations(rectTransform2, entry.virtualPath);
				}
			}
			else
			{
				GameObject gameObject4 = new GameObject("FolderIcon", new Type[]
				{
					typeof(RectTransform),
					typeof(Image)
				});
				gameObject4.transform.SetParent(rectTransform2, false);
				rectTransform3 = (RectTransform)gameObject4.transform;
				RectTransform rectTransform4 = rectTransform3;
				RectTransform rectTransform5 = rectTransform3;
				RectTransform rectTransform6 = rectTransform3;
				Vector2 vector = new Vector2(0.5f, 0.5f);
				rectTransform6.pivot = vector;
				rectTransform4.anchorMin = (rectTransform5.anchorMax = vector);
				rectTransform3.anchoredPosition = Vector2.zero;
				CielOSIcon.Apply(gameObject4.GetComponent<Image>(), CielOSIconId.Folder, color2, 128);
				gameObject4.GetComponent<Image>().raycastTarget = false;
			}
			TMP_Text tmp_Text = CielOSPhotosApp.MakeText(gameObject.transform, "Label", CielOSPhotosApp.DisplayName(entry), 12f, FontStyles.Normal, TextAlignmentOptions.Center);
			tmp_Text.color = color3;
			tmp_Text.enableWordWrapping = false;
			tmp_Text.overflowMode = TextOverflowModes.Ellipsis;
			CielOSPhotosApp.TileRecord tileRecord = new CielOSPhotosApp.TileRecord();
			tileRecord.gameObject = gameObject;
			tileRecord.rect = rectTransform;
			tileRecord.thumbnail = rectTransform2;
			tileRecord.preview = rawImage;
			tileRecord.icon = rectTransform3;
			tileRecord.label = (RectTransform)tmp_Text.transform;
			tileRecord.entry = entry;
			tileRecord.isVideo = video;
			tileRecord.baseColor = component.color;
			CielOSMediaTileDrag cielOSMediaTileDrag = null;
			if (!entry.isDirectory)
			{
				cielOSMediaTileDrag = gameObject.AddComponent<CielOSMediaTileDrag>();
				cielOSMediaTileDrag.Bind(this, entry, this._scroll);
			}
			CielOSFileSystem.Entry captured = entry;
			CielOSMediaTileDrag capturedDrag = cielOSMediaTileDrag;
			CielOSClickable component2 = gameObject.GetComponent<CielOSClickable>();
			component2.OnClick = delegate(Vector2 _)
			{
				if (capturedDrag != null && capturedDrag.ConsumeClickSuppression())
				{
					return;
				}
				if (captured.isDirectory)
				{
					this.OpenFolder(captured.virtualPath);
				}
			};
			component2.OnDoubleClick = delegate(Vector2 _)
			{
				if (capturedDrag != null && capturedDrag.ConsumeClickSuppression())
				{
					return;
				}
				if (captured.isDirectory)
				{
					this.OpenFolder(captured.virtualPath);
					return;
				}
				if (video)
				{
					this.OpenVideoViewer(captured);
					return;
				}
				this.OpenImageViewer(captured);
			};
			gameObject.GetComponent<CielOSRightClickable>().OnRightClick = delegate(Vector2 position)
			{
				this.ShowContextMenu(captured, video, position);
			};
			return tileRecord;
		}

		// Token: 0x06009E1B RID: 40475 RVA: 0x00410EC4 File Offset: 0x0040F0C4
		private void AddVideoDecorations(RectTransform thumbnail, string virtualPath)
		{
			TMP_Text tmp_Text = CielOSPhotosApp.MakeText(thumbnail, "Play", "▶", 34f, FontStyles.Bold, TextAlignmentOptions.Center);
			CielOSPhotosApp.Stretch((RectTransform)tmp_Text.transform);
			tmp_Text.color = new Color(1f, 1f, 1f, 0.9f);
			PhoneVideoMetadata phoneVideoMetadata = PhoneVideoStorage.LoadMetadata(CielOSFileSystem.Instance.ToReal(virtualPath));
			Image image = CielOSPhotosApp.NewImage("Duration", thumbnail);
			image.color = new Color(0f, 0f, 0f, 0.72f);
			image.raycastTarget = false;
			CielOSTheme.Round(image);
			RectTransform rectTransform = (RectTransform)image.transform;
			Vector2 vector = new Vector2(1f, 0f);
			rectTransform.pivot = vector;
			rectTransform.anchorMin = (rectTransform.anchorMax = vector);
			rectTransform.anchoredPosition = new Vector2(-7f, 7f);
			rectTransform.sizeDelta = new Vector2(64f, 22f);
			TMP_Text tmp_Text2 = CielOSPhotosApp.MakeText(rectTransform, "Label", CielOSPhotosApp.FormatDuration((phoneVideoMetadata != null) ? phoneVideoMetadata.durationSeconds : 0f), 11f, FontStyles.Bold, TextAlignmentOptions.Center);
			CielOSPhotosApp.Stretch((RectTransform)tmp_Text2.transform);
			tmp_Text2.color = Color.white;
		}

		// Token: 0x06009E1C RID: 40476 RVA: 0x00411004 File Offset: 0x0040F204
		private Sprite GetThumbnail(string virtualPath, bool video)
		{
			Sprite sprite;
			if (this._thumbnailCache.TryGetValue(virtualPath, out sprite) && sprite != null)
			{
				return sprite;
			}
			int num = this.ThumbnailPixels();
			Sprite sprite2 = (video ? CielOSPhotosApp.LoadRealImage(CielOSMediaLibrary.VideoThumbnailRealPath(virtualPath), num) : CielOSWallpaperService.LoadThumbnail(virtualPath, num));
			if (sprite2 == null && video)
			{
				sprite2 = CielOSPhotosApp.CreateVideoFallback(virtualPath);
			}
			if (sprite2 != null)
			{
				this._thumbnailCache[virtualPath] = sprite2;
			}
			return sprite2;
		}

		// Token: 0x06009E1D RID: 40477 RVA: 0x00411078 File Offset: 0x0040F278
		private int ThumbnailPixels()
		{
			Canvas componentInParent = base.GetComponentInParent<Canvas>();
			float num = ((componentInParent != null) ? componentInParent.rootCanvas.scaleFactor : 1f);
			return Mathf.CeilToInt(288f * Mathf.Max(1f, num));
		}

		// Token: 0x06009E1E RID: 40478 RVA: 0x004110C0 File Offset: 0x0040F2C0
		private static Sprite LoadRealImage(string path, int shortSide)
		{
			if (string.IsNullOrEmpty(path) || !File.Exists(path))
			{
				return null;
			}
			Sprite sprite2;
			try
			{
				Sprite sprite = CielOSWallpaperService.CreateThumbnail(File.ReadAllBytes(path), shortSide);
				if (sprite != null)
				{
					sprite.name = path;
				}
				sprite2 = sprite;
			}
			catch
			{
				sprite2 = null;
			}
			return sprite2;
		}

		// Token: 0x06009E1F RID: 40479 RVA: 0x00411118 File Offset: 0x0040F318
		private static Sprite CreateVideoFallback(string name)
		{
			Texture2D texture2D = new Texture2D(16, 16, TextureFormat.RGB24, false)
			{
				name = name + " thumbnail",
				wrapMode = TextureWrapMode.Clamp,
				filterMode = FilterMode.Bilinear
			};
			Color color = new Color(0.06f, 0.045f, 0.08f);
			Color color2 = new Color(0.22f, 0.12f, 0.28f);
			for (int i = 0; i < texture2D.height; i++)
			{
				for (int j = 0; j < texture2D.width; j++)
				{
					texture2D.SetPixel(j, i, Color.Lerp(color, color2, (float)i / (float)(texture2D.height - 1)));
				}
			}
			texture2D.Apply(false, false);
			return Sprite.Create(texture2D, new Rect(0f, 0f, (float)texture2D.width, (float)texture2D.height), new Vector2(0.5f, 0.5f), 100f);
		}

		// Token: 0x06009E20 RID: 40480 RVA: 0x004111FD File Offset: 0x0040F3FD
		private void OpenFolder(string virtualPath)
		{
			if (!CielOSFileSystem.Instance.Exists(virtualPath))
			{
				return;
			}
			this._currentDirectory = virtualPath;
			this.StoreCurrentDirectory();
			CielOSSfx.PlayClick();
			this.Refresh(true);
		}

		// Token: 0x06009E21 RID: 40481 RVA: 0x00411228 File Offset: 0x0040F428
		private void OpenImageViewer(CielOSFileSystem.Entry entry)
		{
			Sprite thumbnail = this.GetThumbnail(entry.virtualPath, false);
			if (thumbnail == null || thumbnail.texture == null)
			{
				return;
			}
			CielOSWindowManager cielOSWindowManager = Object.FindObjectOfType<CielOSWindowManager>();
			if (cielOSWindowManager == null)
			{
				return;
			}
			float num = ((thumbnail.texture.height > 0) ? ((float)thumbnail.texture.width / (float)thumbnail.texture.height) : 1f);
			float num2 = 590f;
			float num3 = Mathf.Clamp(num2 * num, 420f, 1120f);
			CielOSWindow cielOSWindow = cielOSWindowManager.LaunchTransientWindow(entry.name, null, new Vector2(num3, num2 + 34f), "imageviewer");
			if (cielOSWindow != null && cielOSWindow.Content != null)
			{
				CielOSImageViewerApp.Mount(cielOSWindow.Content, entry.virtualPath);
			}
		}

		// Token: 0x06009E22 RID: 40482 RVA: 0x00411304 File Offset: 0x0040F504
		private void OpenVideoViewer(CielOSFileSystem.Entry entry)
		{
			string text = CielOSFileSystem.Instance.ToReal(entry.virtualPath);
			if (!File.Exists(text))
			{
				return;
			}
			CielOSWindowManager cielOSWindowManager = Object.FindObjectOfType<CielOSWindowManager>();
			if (cielOSWindowManager == null)
			{
				return;
			}
			PhoneVideoMetadata phoneVideoMetadata = PhoneVideoStorage.LoadMetadata(text);
			float num = ((phoneVideoMetadata != null && phoneVideoMetadata.height > 0) ? ((float)phoneVideoMetadata.width / (float)phoneVideoMetadata.height) : 1.7777778f);
			float num2 = 590f;
			float num3 = Mathf.Clamp(num2 * num, 520f, 1180f);
			CielOSWindow cielOSWindow = cielOSWindowManager.LaunchTransientWindow(entry.name, null, new Vector2(num3, num2 + 34f), "videoviewer");
			if (cielOSWindow != null && cielOSWindow.Content != null)
			{
				CielOSVideoViewerApp.Mount(cielOSWindow.Content, entry.virtualPath);
			}
		}

		// Token: 0x06009E23 RID: 40483 RVA: 0x004113D0 File Offset: 0x0040F5D0
		private void ShowContextMenu(CielOSFileSystem.Entry entry, bool video, Vector2 position)
		{
			if (CielOSContextMenu.Instance == null)
			{
				return;
			}
			bool flag = entry.isDirectory && CielOSFileSystem.Instance.IsProtected(entry.virtualPath);
			List<CielOSContextMenu.Item> list = new List<CielOSContextMenu.Item>();
			if (entry.isDirectory)
			{
				list.Add(new CielOSContextMenu.Item("Open", delegate
				{
					this.OpenFolder(entry.virtualPath);
				}, true, false, null));
			}
			else if (!video)
			{
				list.Add(new CielOSContextMenu.Item("Open", delegate
				{
					this.OpenImageViewer(entry);
				}, true, false, null));
				list.Add(new CielOSContextMenu.Item("Set as wallpaper", delegate
				{
					CielOSWallpaperService instance = CielOSWallpaperService.Instance;
					if (instance == null)
					{
						return;
					}
					instance.ApplyWallpaper(entry.virtualPath, true);
				}, true, false, null));
				list.Add(new CielOSContextMenu.Item("Set as screensaver...", delegate
				{
					CielOSWallpaperService.ShowScreensaverTargetMenu(position, entry.virtualPath);
				}, true, false, null));
			}
			else
			{
				list.Add(new CielOSContextMenu.Item("Open", delegate
				{
					this.OpenVideoViewer(entry);
				}, true, false, null));
			}
			if (!entry.isDirectory)
			{
				list.Add(new CielOSContextMenu.Item("Save to PC", delegate
				{
					CielOSPhotosApp.SaveToPC(entry, video);
				}, true, false, "phone_story"));
				list.Add(new CielOSContextMenu.Item("Open folder", delegate
				{
					CielOSPhotosApp.OpenContainingFolder(entry);
				}, true, false, "phone_story"));
			}
			list.Add(new CielOSContextMenu.Item("Rename", delegate
			{
				this.RenameEntry(entry, video);
			}, !flag, false, null));
			list.Add(new CielOSContextMenu.Item("Delete", delegate
			{
				this.DeleteEntry(entry, video);
			}, !flag, false, null));
			for (int i = 0; i < list.Count; i++)
			{
				CielOSContextMenu.Item item = list[i];
				if (string.IsNullOrEmpty(item.localizationScope))
				{
					item.localizationScope = "cielos_content";
				}
				list[i] = item;
			}
			CielOSContextMenu.Instance.Show(position, list);
		}

		// Token: 0x06009E24 RID: 40484 RVA: 0x004115D8 File Offset: 0x0040F7D8
		private static void SaveToPC(CielOSFileSystem.Entry entry, bool video)
		{
			try
			{
				string text = CielOSFileSystem.Instance.ToReal(entry.virtualPath);
				string text2 = (video ? PlayerMediaExport.CopyPcVideo(text) : PlayerMediaExport.CopyPcPhoto(text));
				ToastService.ShowFormat("phone_story", "Saved to PC: {0}", new object[] { Path.GetFileName(text2) }, "Photos", null, -1f);
			}
			catch (Exception ex)
			{
				Debug.LogWarning("[CielOSPhotosApp] Save to PC failed: " + ex.Message);
				ToastService.ShowFormat("phone_story", "Save to PC failed: {0}", new object[] { ex.Message }, "Photos", null, -1f);
			}
		}

		// Token: 0x06009E25 RID: 40485 RVA: 0x00411684 File Offset: 0x0040F884
		private static void OpenContainingFolder(CielOSFileSystem.Entry entry)
		{
			PlayerMediaExport.OpenDirectory(Path.GetDirectoryName(CielOSFileSystem.Instance.ToReal(entry.virtualPath)));
		}

		// Token: 0x06009E26 RID: 40486 RVA: 0x004116A4 File Offset: 0x0040F8A4
		private void RenameEntry(CielOSFileSystem.Entry entry, bool video)
		{
			CielOSTextPrompt.Ensure(base.GetComponentInParent<Canvas>().transform).Show("Rename", entry.name, delegate(string requested)
			{
				bool flag;
				if (entry.isDirectory)
				{
					flag = CielOSFileSystem.Instance.Rename(entry.virtualPath, requested);
				}
				else
				{
					string text;
					flag = CielOSMediaLibrary.RenameMedia(entry.virtualPath, requested, video, out text);
				}
				if (flag)
				{
					this.RemoveCachedThumbnail(entry.virtualPath);
					this.Refresh(false);
				}
			});
		}

		// Token: 0x06009E27 RID: 40487 RVA: 0x004116FD File Offset: 0x0040F8FD
		private void DeleteEntry(CielOSFileSystem.Entry entry, bool video)
		{
			if (!(entry.isDirectory ? CielOSFileSystem.Instance.Delete(entry.virtualPath) : CielOSMediaLibrary.DeleteMedia(entry.virtualPath, video)))
			{
				return;
			}
			this.RemoveCachedThumbnail(entry.virtualPath);
			this.Refresh(false);
		}

		// Token: 0x06009E28 RID: 40488 RVA: 0x0041173B File Offset: 0x0040F93B
		private void CreateFolder()
		{
			CielOSTextPrompt.Ensure(base.GetComponentInParent<Canvas>().transform).Show("New Folder", "New Folder", delegate(string name)
			{
				if (CielOSFileSystem.Instance.CreateFolder(this._currentDirectory, name))
				{
					this.Refresh(false);
				}
			});
		}

		// Token: 0x06009E29 RID: 40489 RVA: 0x00411768 File Offset: 0x0040F968
		private void ImportMedia()
		{
			string[] array2;
			if (this._tab != CielOSPhotosApp.MediaTab.Photos)
			{
				string[] array = new string[5];
				array[0] = ".avi";
				array[1] = ".mp4";
				array[2] = ".mov";
				array[3] = ".m4v";
				array2 = array;
				array[4] = ".webm";
			}
			else
			{
				string[] array3 = new string[3];
				array3[0] = ".png";
				array3[1] = ".jpg";
				array2 = array3;
				array3[2] = ".jpeg";
			}
			string[] array4 = array2;
			string text = ((this._tab == CielOSPhotosApp.MediaTab.Photos) ? "Import photo" : "Import video");
			CielOSFileBrowser browser = CielOSFileBrowser.Ensure(base.GetComponentInParent<Canvas>().transform);
			browser.Open(text, array4, delegate(string realPath)
			{
				if (string.IsNullOrEmpty(realPath))
				{
					return;
				}
				string text2 = CielOSFileSystem.Instance.ImportExternalFile(realPath, this._currentDirectory);
				if (string.IsNullOrEmpty(text2))
				{
					return;
				}
				browser.RememberSuccessfulDirectory(realPath);
				CielOSMediaLibrary.AppendToOrder(this._currentDirectory, text2);
				this.Refresh(false);
			}, null, "cielos_shell", false);
		}

		// Token: 0x06009E2A RID: 40490 RVA: 0x00411824 File Offset: 0x0040FA24
		private void GoUp()
		{
			if (this._currentDirectory.Equals(this.ActiveRoot, StringComparison.OrdinalIgnoreCase))
			{
				return;
			}
			int num = this._currentDirectory.TrimEnd('/').LastIndexOf('/');
			string text = ((num <= 0) ? this.ActiveRoot : this._currentDirectory.Substring(0, num));
			if (!text.StartsWith(this.ActiveRoot, StringComparison.OrdinalIgnoreCase))
			{
				text = this.ActiveRoot;
			}
			this._currentDirectory = text;
			this.StoreCurrentDirectory();
			this.Refresh(true);
		}

		// Token: 0x06009E2B RID: 40491 RVA: 0x0041189F File Offset: 0x0040FA9F
		public void BeginMediaDrag(CielOSFileSystem.Entry entry)
		{
			this.ClearFolderHighlight();
		}

		// Token: 0x06009E2C RID: 40492 RVA: 0x004118A8 File Offset: 0x0040FAA8
		public void UpdateMediaDrag(CielOSFileSystem.Entry entry, Vector2 screenPosition, Camera eventCamera)
		{
			CielOSPhotosApp.TileRecord tileRecord = this.FindTileAt(screenPosition, eventCamera, true);
			if (tileRecord == this._highlightedFolder)
			{
				return;
			}
			this.ClearFolderHighlight();
			this._highlightedFolder = tileRecord;
			if (this._highlightedFolder == null)
			{
				return;
			}
			Image component = this._highlightedFolder.gameObject.GetComponent<Image>();
			CielOSTheme active = CielOSTheme.Active;
			Color color = ((active != null) ? active.accent : new Color(0.66f, 0.38f, 0.79f, 1f));
			if (component != null)
			{
				component.color = new Color(color.r, color.g, color.b, 0.62f);
			}
		}

		// Token: 0x06009E2D RID: 40493 RVA: 0x0041194C File Offset: 0x0040FB4C
		public void EndMediaDrag(CielOSFileSystem.Entry source, Vector2 screenPosition, Camera eventCamera)
		{
			CielOSPhotosApp.TileRecord tileRecord = this.FindTileAt(screenPosition, eventCamera, true);
			this.ClearFolderHighlight();
			bool flag = CielOSMediaLibrary.IsVideoFileName(source.name);
			if (tileRecord != null)
			{
				string text;
				if (CielOSMediaLibrary.MoveMedia(source.virtualPath, tileRecord.entry.virtualPath, flag, out text))
				{
					this.RemoveCachedThumbnail(source.virtualPath);
					this.Refresh(false);
				}
				return;
			}
			if (this._viewport != null && !RectTransformUtility.RectangleContainsScreenPoint(this._viewport, screenPosition, eventCamera))
			{
				return;
			}
			CielOSPhotosApp.TileRecord target = this.FindTileAt(screenPosition, eventCamera, false);
			List<string> list = (from tile in this._tiles
				where !tile.entry.isDirectory
				select tile.entry.virtualPath).ToList<string>();
			int num = list.FindIndex((string path) => path.Equals(source.virtualPath, StringComparison.OrdinalIgnoreCase));
			if (num < 0)
			{
				return;
			}
			string text2 = list[num];
			list.RemoveAt(num);
			int num2 = list.Count;
			if (target != null && !target.entry.isDirectory && !target.entry.virtualPath.Equals(source.virtualPath, StringComparison.OrdinalIgnoreCase))
			{
				num2 = list.FindIndex((string path) => path.Equals(target.entry.virtualPath, StringComparison.OrdinalIgnoreCase));
				if (num2 < 0)
				{
					num2 = list.Count;
				}
			}
			list.Insert(Mathf.Clamp(num2, 0, list.Count), text2);
			CielOSMediaLibrary.SaveOrder(this._currentDirectory, list);
			this.Refresh(false);
		}

		// Token: 0x06009E2E RID: 40494 RVA: 0x00411B00 File Offset: 0x0040FD00
		private CielOSPhotosApp.TileRecord FindTileAt(Vector2 screenPosition, Camera eventCamera, bool foldersOnly)
		{
			for (int i = this._tiles.Count - 1; i >= 0; i--)
			{
				CielOSPhotosApp.TileRecord tileRecord = this._tiles[i];
				if (foldersOnly == tileRecord.entry.isDirectory && RectTransformUtility.RectangleContainsScreenPoint(tileRecord.rect, screenPosition, eventCamera))
				{
					return tileRecord;
				}
			}
			return null;
		}

		// Token: 0x06009E2F RID: 40495 RVA: 0x00411B54 File Offset: 0x0040FD54
		private void ClearFolderHighlight()
		{
			if (this._highlightedFolder != null && this._highlightedFolder.gameObject != null)
			{
				Image component = this._highlightedFolder.gameObject.GetComponent<Image>();
				if (component != null)
				{
					component.color = this._highlightedFolder.baseColor;
				}
			}
			this._highlightedFolder = null;
		}

		// Token: 0x06009E30 RID: 40496 RVA: 0x00411BB0 File Offset: 0x0040FDB0
		private void ClearTiles()
		{
			this.ClearFolderHighlight();
			foreach (CielOSPhotosApp.TileRecord tileRecord in this._tiles)
			{
				if (!(tileRecord.gameObject == null))
				{
					tileRecord.gameObject.SetActive(false);
					Object.Destroy(tileRecord.gameObject);
				}
			}
			this._tiles.Clear();
			if (this._emptyState != null)
			{
				this._emptyState.SetActive(false);
				Object.Destroy(this._emptyState);
				this._emptyState = null;
			}
		}

		// Token: 0x06009E31 RID: 40497 RVA: 0x00411C60 File Offset: 0x0040FE60
		private void BuildEmptyState()
		{
			this._emptyState = new GameObject("EmptyState", new Type[]
			{
				typeof(RectTransform),
				typeof(TextMeshProUGUI)
			});
			this._emptyState.transform.SetParent(this._listContainer, false);
			TMP_Text component = this._emptyState.GetComponent<TMP_Text>();
			component.text = ((this._tab == CielOSPhotosApp.MediaTab.Photos) ? "No photos yet\nTake a photo with Ciel's phone" : "No videos yet\nRecord one with Ciel's phone");
			component.fontSize = 17f;
			component.alignment = TextAlignmentOptions.Center;
			component.color = new Color(1f, 1f, 1f, 0.56f);
			component.raycastTarget = false;
			CielOSTheme active = CielOSTheme.Active;
			if (active != null && active.uiFont != null)
			{
				component.font = active.uiFont;
			}
			CielOSContentText.Bind(component, component.text);
		}

		// Token: 0x06009E32 RID: 40498 RVA: 0x00411D4C File Offset: 0x0040FF4C
		private void RemoveCachedThumbnail(string virtualPath)
		{
			Sprite sprite;
			if (!this._thumbnailCache.TryGetValue(virtualPath, out sprite))
			{
				return;
			}
			this._thumbnailCache.Remove(virtualPath);
			CielOSPhotosApp.DestroyRuntimeSprite(sprite);
		}

		// Token: 0x06009E33 RID: 40499 RVA: 0x00411D80 File Offset: 0x0040FF80
		private static void DestroyRuntimeSprite(Sprite sprite)
		{
			if (sprite == null)
			{
				return;
			}
			Texture2D texture = sprite.texture;
			Object.Destroy(sprite);
			if (texture != null)
			{
				Object.Destroy(texture);
			}
		}

		// Token: 0x06009E34 RID: 40500 RVA: 0x00411DB3 File Offset: 0x0040FFB3
		private static string DisplayName(CielOSFileSystem.Entry entry)
		{
			if (!entry.isDirectory)
			{
				return Path.GetFileNameWithoutExtension(entry.name);
			}
			return entry.name;
		}

		// Token: 0x06009E35 RID: 40501 RVA: 0x00411DD0 File Offset: 0x0040FFD0
		private static string FormatDuration(float seconds)
		{
			int num = Mathf.Max(0, Mathf.FloorToInt(seconds));
			int num2 = num / 3600;
			int num3 = num / 60 % 60;
			int num4 = num % 60;
			if (num2 <= 0)
			{
				return string.Format("{0:00}:{1:00}", num3, num4);
			}
			return string.Format("{0:00}:{1:00}:{2:00}", num2, num3, num4);
		}

		// Token: 0x06009E36 RID: 40502 RVA: 0x00411E34 File Offset: 0x00410034
		private static void Cover(RectTransform rect, Texture texture, float width, float height)
		{
			if (rect == null)
			{
				return;
			}
			Vector2 vector = new Vector2(0.5f, 0.5f);
			rect.pivot = vector;
			rect.anchorMin = (rect.anchorMax = vector);
			rect.anchoredPosition = Vector2.zero;
			float num = ((texture != null && texture.height > 0) ? ((float)texture.width / (float)texture.height) : 1f);
			float num2 = ((height > 0f) ? (width / height) : 1f);
			rect.sizeDelta = ((num >= num2) ? new Vector2(height * num, height) : new Vector2(width, width / Mathf.Max(0.001f, num)));
		}

		// Token: 0x06009E37 RID: 40503 RVA: 0x00411EE2 File Offset: 0x004100E2
		private static Image NewImage(string name, Transform parent)
		{
			GameObject gameObject = new GameObject(name, new Type[]
			{
				typeof(RectTransform),
				typeof(Image)
			});
			gameObject.transform.SetParent(parent, false);
			return gameObject.GetComponent<Image>();
		}

		// Token: 0x06009E38 RID: 40504 RVA: 0x00411F1C File Offset: 0x0041011C
		private static TMP_Text MakeText(Transform parent, string name, string content, float fontSize, FontStyles style, TextAlignmentOptions alignment)
		{
			GameObject gameObject = new GameObject(name, new Type[]
			{
				typeof(RectTransform),
				typeof(TextMeshProUGUI)
			});
			gameObject.transform.SetParent(parent, false);
			TextMeshProUGUI component = gameObject.GetComponent<TextMeshProUGUI>();
			component.text = content;
			component.fontSize = fontSize;
			component.fontStyle = style;
			component.alignment = alignment;
			component.raycastTarget = false;
			CielOSTheme active = CielOSTheme.Active;
			if (active != null && active.uiFont != null)
			{
				component.font = active.uiFont;
			}
			LocalizedGameText.BindFontOnly(component);
			return component;
		}

		// Token: 0x06009E39 RID: 40505 RVA: 0x001A38AD File Offset: 0x001A1AAD
		private static void Stretch(RectTransform rect)
		{
			rect.anchorMin = Vector2.zero;
			rect.anchorMax = Vector2.one;
			rect.offsetMin = Vector2.zero;
			rect.offsetMax = Vector2.zero;
		}

		// Token: 0x0400A24E RID: 41550
		private const float TabBarHeight = 42f;

		// Token: 0x0400A24F RID: 41551
		private const float GridGap = 14f;

		// Token: 0x0400A250 RID: 41552
		private const float GridPadding = 14f;

		// Token: 0x0400A251 RID: 41553
		private const float MaximumCellWidth = 300f;

		// Token: 0x0400A252 RID: 41554
		private const float MaximumThumbnailSize = 288f;

		// Token: 0x0400A253 RID: 41555
		private TMP_Text _pathLabel;

		// Token: 0x0400A254 RID: 41556
		private RectTransform _listContainer;

		// Token: 0x0400A255 RID: 41557
		private Button _upButton;

		// Token: 0x0400A256 RID: 41558
		private Button _newFolderButton;

		// Token: 0x0400A257 RID: 41559
		private TMP_InputField _oldFolderName;

		// Token: 0x0400A258 RID: 41560
		private ScrollRect _scroll;

		// Token: 0x0400A259 RID: 41561
		private RectTransform _viewport;

		// Token: 0x0400A25A RID: 41562
		private CielOSSmoothWheelScroll _smoothWheel;

		// Token: 0x0400A25B RID: 41563
		private Button _photosTabButton;

		// Token: 0x0400A25C RID: 41564
		private Button _videosTabButton;

		// Token: 0x0400A25D RID: 41565
		private Image _photosTabImage;

		// Token: 0x0400A25E RID: 41566
		private Image _videosTabImage;

		// Token: 0x0400A25F RID: 41567
		private TMP_Text _photosTabLabel;

		// Token: 0x0400A260 RID: 41568
		private TMP_Text _videosTabLabel;

		// Token: 0x0400A261 RID: 41569
		private GameObject _emptyState;

		// Token: 0x0400A262 RID: 41570
		private Coroutine _layoutRoutine;

		// Token: 0x0400A263 RID: 41571
		private readonly List<CielOSPhotosApp.TileRecord> _tiles = new List<CielOSPhotosApp.TileRecord>();

		// Token: 0x0400A264 RID: 41572
		private readonly Dictionary<string, Sprite> _thumbnailCache = new Dictionary<string, Sprite>(StringComparer.OrdinalIgnoreCase);

		// Token: 0x0400A265 RID: 41573
		private CielOSPhotosApp.MediaTab _tab;

		// Token: 0x0400A266 RID: 41574
		private string _photoDirectory = "/Photos";

		// Token: 0x0400A267 RID: 41575
		private string _videoDirectory = "/Videos";

		// Token: 0x0400A268 RID: 41576
		private string _currentDirectory = "/Photos";

		// Token: 0x0400A269 RID: 41577
		private CielOSPhotosApp.TileRecord _highlightedFolder;

		// Token: 0x0400A26A RID: 41578
		private float _lastLayoutWidth = -1f;

		// Token: 0x0400A26B RID: 41579
		private bool _initialized;

		// Token: 0x0400A26C RID: 41580
		private bool _subscribed;

		// Token: 0x02001400 RID: 5120
		public enum MediaTab
		{
			// Token: 0x0400A26E RID: 41582
			Photos,
			// Token: 0x0400A26F RID: 41583
			Videos
		}

		// Token: 0x02001401 RID: 5121
		public struct GridMetrics
		{
			// Token: 0x0400A270 RID: 41584
			public int columns;

			// Token: 0x0400A271 RID: 41585
			public float cellWidth;

			// Token: 0x0400A272 RID: 41586
			public float cellHeight;

			// Token: 0x0400A273 RID: 41587
			public float thumbnailSize;

			// Token: 0x0400A274 RID: 41588
			public float gap;

			// Token: 0x0400A275 RID: 41589
			public float startX;

			// Token: 0x0400A276 RID: 41590
			public float paddingY;
		}

		// Token: 0x02001402 RID: 5122
		private sealed class TileRecord
		{
			// Token: 0x0400A277 RID: 41591
			public GameObject gameObject;

			// Token: 0x0400A278 RID: 41592
			public RectTransform rect;

			// Token: 0x0400A279 RID: 41593
			public RectTransform thumbnail;

			// Token: 0x0400A27A RID: 41594
			public RawImage preview;

			// Token: 0x0400A27B RID: 41595
			public RectTransform icon;

			// Token: 0x0400A27C RID: 41596
			public RectTransform label;

			// Token: 0x0400A27D RID: 41597
			public CielOSFileSystem.Entry entry;

			// Token: 0x0400A27E RID: 41598
			public bool isVideo;

			// Token: 0x0400A27F RID: 41599
			public Color baseColor;
		}
	}
}
