using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Features.CensorBars;
using Features.Localization;
using Features.PhoneCompanion;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace ProjektSumperk
{
	// Token: 0x020003FC RID: 1020
	public class PhotoGallery : MonoBehaviour
	{
		// Token: 0x170004EF RID: 1263
		// (get) Token: 0x060019DB RID: 6619 RVA: 0x000A3DC8 File Offset: 0x000A1FC8
		public bool ShowingVideos
		{
			get
			{
				return this._showingVideos;
			}
		}

		// Token: 0x170004F0 RID: 1264
		// (get) Token: 0x060019DC RID: 6620 RVA: 0x000A3DD0 File Offset: 0x000A1FD0
		public Transform VideoParent
		{
			get
			{
				return this._videoParent;
			}
		}

		// Token: 0x060019DD RID: 6621 RVA: 0x000A3DD8 File Offset: 0x000A1FD8
		private void Start()
		{
			this._photoPath = PhonePhotoStorage.ImagesFolder;
			this._started = true;
			this.SetupMediaTabs();
			this.ResetToDefault(true);
		}

		// Token: 0x060019DE RID: 6622 RVA: 0x000A3DF9 File Offset: 0x000A1FF9
		private void OnEnable()
		{
			if (this._started)
			{
				this.ResetToDefault(true);
			}
		}

		// Token: 0x060019DF RID: 6623 RVA: 0x000A3E0A File Offset: 0x000A200A
		private void OnDisable()
		{
			this.CloseFullscreenPanel();
		}

		// Token: 0x060019E0 RID: 6624 RVA: 0x000A3E14 File Offset: 0x000A2014
		public void GetPhotoImages(Sprite sprite)
		{
			if (this.FullScreenPanel == null)
			{
				return;
			}
			this.FullScreenPanel.SetActive(true);
			Image component = this.FullScreenPanel.GetComponent<Image>();
			if (component != null)
			{
				component.sprite = sprite;
			}
			StreamerCensorImage.Bind(component, false);
		}

		// Token: 0x060019E1 RID: 6625 RVA: 0x000A3E5F File Offset: 0x000A205F
		public void ShowImages()
		{
			this.SetupMediaTabs();
			this.SetMediaTab(false);
		}

		// Token: 0x060019E2 RID: 6626 RVA: 0x000A3E6E File Offset: 0x000A206E
		public void ShowVideos()
		{
			this.SetupMediaTabs();
			this.SetMediaTab(true);
			this.RefreshVideos();
		}

		// Token: 0x060019E3 RID: 6627 RVA: 0x000A3E84 File Offset: 0x000A2084
		public void ResetToDefault(bool refreshMedia = true)
		{
			if (string.IsNullOrEmpty(this._photoPath))
			{
				this._photoPath = PhonePhotoStorage.ImagesFolder;
			}
			this.SetupMediaTabs();
			this.CloseFullscreenPanel();
			this.SetMediaTab(false);
			if (this._scrollRect != null)
			{
				this._scrollRect.StopMovement();
				this._scrollRect.horizontalNormalizedPosition = 0f;
				this._scrollRect.verticalNormalizedPosition = 1f;
			}
			if (!refreshMedia)
			{
				return;
			}
			if (!base.isActiveAndEnabled || !base.gameObject.activeInHierarchy)
			{
				return;
			}
			this.RefreshPhotos();
			this.RefreshVideos();
		}

		// Token: 0x060019E4 RID: 6628 RVA: 0x000A3F1C File Offset: 0x000A211C
		private void SetMediaTab(bool videos)
		{
			this._showingVideos = videos;
			if (this.parent != null)
			{
				this.parent.gameObject.SetActive(!videos);
			}
			if (this._videoParent != null)
			{
				this._videoParent.gameObject.SetActive(videos);
			}
			if (this._scrollRect != null)
			{
				this._scrollRect.StopMovement();
				this._scrollRect.content = (videos ? this._videoParent : this.parent) as RectTransform;
				this._scrollRect.vertical = true;
				this._scrollRect.scrollSensitivity = 12f;
				PhoneGalleryScrollInput.Ensure(this._scrollRect);
				this._scrollRect.verticalNormalizedPosition = 1f;
			}
			this.SetTabVisuals(videos);
			this.UpdateEmptyState();
		}

		// Token: 0x060019E5 RID: 6629 RVA: 0x000A3FF0 File Offset: 0x000A21F0
		private void CloseFullscreenPanel()
		{
			if (this.FullScreenPanel == null)
			{
				return;
			}
			Image component = this.FullScreenPanel.GetComponent<Image>();
			if (component != null)
			{
				component.enabled = true;
			}
			if (this.FullScreenPanel.activeSelf)
			{
				this.FullScreenPanel.SetActive(false);
			}
		}

		// Token: 0x060019E6 RID: 6630 RVA: 0x000A4044 File Offset: 0x000A2244
		public void RefreshPhotos()
		{
			if (this.parent == null || this.imagePrefab == null)
			{
				return;
			}
			if (string.IsNullOrEmpty(this._photoPath))
			{
				this._photoPath = PhonePhotoStorage.ImagesFolder;
			}
			if (!base.isActiveAndEnabled || !base.gameObject.activeInHierarchy)
			{
				return;
			}
			int num = this._photoLoadGeneration + 1;
			this._photoLoadGeneration = num;
			int num2 = num;
			PhotoGallery.ClearContent(this.parent);
			this._hasPhotos = false;
			if (!Directory.Exists(this._photoPath))
			{
				Directory.CreateDirectory(this._photoPath);
				this.UpdateEmptyState();
				return;
			}
			List<FileInfo> list = new List<FileInfo>(new DirectoryInfo(this._photoPath).GetFiles("*.png"));
			list.Sort((FileInfo a, FileInfo b) => b.LastWriteTimeUtc.CompareTo(a.LastWriteTimeUtc));
			foreach (FileInfo fileInfo in list)
			{
				string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileInfo.Name);
				if (!fileNameWithoutExtension.EndsWith(".censored", StringComparison.OrdinalIgnoreCase) && !PhonePhotoStorage.IsExcludedFromGallery(fileInfo.Name) && !int.TryParse(fileNameWithoutExtension, out num))
				{
					this._hasPhotos = true;
					GameObject gameObject = Object.Instantiate<GameObject>(this.imagePrefab, this.parent, false);
					gameObject.name = fileNameWithoutExtension;
					gameObject.transform.localPosition = Vector3.zero;
					gameObject.transform.localEulerAngles = Vector3.zero;
					PhonePhotoGalleryItem phonePhotoGalleryItem = gameObject.AddComponent<PhonePhotoGalleryItem>();
					Image image = ((gameObject.transform.childCount > 0) ? gameObject.transform.GetChild(0).GetComponent<Image>() : gameObject.GetComponentInChildren<Image>());
					if (image != null)
					{
						base.StartCoroutine(this.LoadImage(fileInfo.FullName, image, phonePhotoGalleryItem, num2));
					}
				}
			}
			this.UpdateEmptyState();
		}

		// Token: 0x060019E7 RID: 6631 RVA: 0x000A4240 File Offset: 0x000A2440
		public void RefreshVideos()
		{
			this.SetupMediaTabs();
			if (this._videoParent == null || this.imagePrefab == null)
			{
				return;
			}
			PhotoGallery.ClearContent(this._videoParent);
			string videosFolder = PhoneVideoStorage.VideosFolder;
			string[] array = (Directory.Exists(videosFolder) ? Directory.GetFiles(videosFolder, "*.avi") : Array.Empty<string>());
			Array.Sort<string>(array, (string a, string b) => File.GetLastWriteTimeUtc(b).CompareTo(File.GetLastWriteTimeUtc(a)));
			this._hasVideos = array.Length != 0;
			for (int i = 0; i < array.Length; i++)
			{
				this.BuildVideoCell(array[i]);
			}
			this.UpdateEmptyState();
			if (this._showingVideos)
			{
				this.RebuildActiveScrollContent(this._videoParent as RectTransform);
			}
		}

		// Token: 0x060019E8 RID: 6632 RVA: 0x000A4304 File Offset: 0x000A2504
		private void SetupMediaTabs()
		{
			if (this.parent == null)
			{
				return;
			}
			if (this._scrollRect == null)
			{
				this._scrollRect = this.parent.GetComponentInParent<ScrollRect>(true);
			}
			if (this._scrollRect != null)
			{
				this._scrollRect.horizontal = false;
				this._scrollRect.vertical = true;
				this._scrollRect.scrollSensitivity = 12f;
			}
			if (this._tabsReady)
			{
				return;
			}
			Transform transform = PhotoGallery.FindDescendant(base.transform, "SelectedBar");
			if (transform != null)
			{
				this._imagesTab = transform.Find("Images");
				this._videosTab = transform.Find("Videos");
			}
			if (this._videosTab != null)
			{
				this._videosTab.gameObject.SetActive(true);
				PhotoGallery.ReplaceWithRuntimeButton(this._videosTab.gameObject, new UnityAction(this.ShowVideos));
				Transform transform2 = this._videosTab.Find("Selected");
				if (transform2 != null)
				{
					this._videosSelected = transform2.gameObject;
				}
			}
			if (this._imagesTab != null)
			{
				PhotoGallery.ReplaceWithRuntimeButton(this._imagesTab.gameObject, new UnityAction(this.ShowImages));
				Transform transform3 = this._imagesTab.Find("Selected");
				if (transform3 != null)
				{
					this._imagesSelected = transform3.gameObject;
				}
			}
			RectTransform rectTransform = this._imagesTab as RectTransform;
			RectTransform rectTransform2 = this._videosTab as RectTransform;
			if (rectTransform != null && rectTransform2 != null)
			{
				HorizontalLayoutGroup component = transform.GetComponent<HorizontalLayoutGroup>();
				if (component != null)
				{
					component.enabled = false;
				}
				rectTransform.anchorMin = Vector2.zero;
				rectTransform.anchorMax = new Vector2(0.5f, 1f);
				rectTransform.offsetMin = Vector2.zero;
				rectTransform.offsetMax = Vector2.zero;
				rectTransform2.anchorMin = new Vector2(0.5f, 0f);
				rectTransform2.anchorMax = Vector2.one;
				rectTransform2.offsetMin = Vector2.zero;
				rectTransform2.offsetMax = Vector2.zero;
			}
			PhotoGallery.TintSelection(this._imagesSelected);
			PhotoGallery.TintSelection(this._videosSelected);
			PhotoGallery.SetTabLabel(this._imagesTab, "PHOTOS");
			PhotoGallery.SetTabLabel(this._videosTab, "VIDEO");
			RectTransform rectTransform3 = ((this._scrollRect != null) ? this._scrollRect.viewport : null);
			if (rectTransform3 != null)
			{
				this._emptyPhotos = PhotoGallery.CreateEmptyOverlay("NoPhotos", rectTransform3, "No photos yet\nTake a photo with Ciel's phone", "cielos_content");
				this._emptyVideos = PhotoGallery.CreateEmptyOverlay("NoVideos", rectTransform3, "No videos yet\nOpen Camera and press H", "phone_story");
			}
			GameObject gameObject = Object.Instantiate<GameObject>(this.parent.gameObject, this.parent.parent, false);
			gameObject.name = "VideosContent";
			for (int i = gameObject.transform.childCount - 1; i >= 0; i--)
			{
				gameObject.transform.GetChild(i).gameObject.SetActive(false);
				PhotoGallery.DestroyImmediateSafe(gameObject.transform.GetChild(i).gameObject);
			}
			this._videoParent = gameObject.transform;
			this._videoParent.gameObject.SetActive(false);
			this._tabsReady = true;
			this.SetTabVisuals(false);
		}

		// Token: 0x060019E9 RID: 6633 RVA: 0x000A4660 File Offset: 0x000A2860
		private void RebuildActiveScrollContent(RectTransform content)
		{
			if (content == null || this._scrollRect == null)
			{
				return;
			}
			LayoutRebuilder.ForceRebuildLayoutImmediate(content);
			Canvas.ForceUpdateCanvases();
			this._scrollRect.content = content;
			this._scrollRect.StopMovement();
			this._scrollRect.verticalNormalizedPosition = 1f;
		}

		// Token: 0x060019EA RID: 6634 RVA: 0x000A46B8 File Offset: 0x000A28B8
		private static Transform FindDescendant(Transform root, string objectName)
		{
			if (root == null)
			{
				return null;
			}
			foreach (Transform transform in root.GetComponentsInChildren<Transform>(true))
			{
				if (transform.name == objectName)
				{
					return transform;
				}
			}
			return null;
		}

		// Token: 0x060019EB RID: 6635 RVA: 0x000A46FC File Offset: 0x000A28FC
		private static void ReplaceWithRuntimeButton(GameObject host, UnityAction action)
		{
			Button[] components = host.GetComponents<Button>();
			Button button = ((components.Length != 0) ? components[0] : host.AddComponent<Button>());
			for (int i = 1; i < components.Length; i++)
			{
				components[i].enabled = false;
			}
			button.enabled = true;
			button.onClick = new Button.ButtonClickedEvent();
			button.targetGraphic = host.GetComponent<Graphic>() ?? host.GetComponentInChildren<Graphic>();
			button.transition = Selectable.Transition.ColorTint;
			button.onClick.AddListener(action);
		}

		// Token: 0x060019EC RID: 6636 RVA: 0x000A4774 File Offset: 0x000A2974
		private static void SetTabLabel(Transform tab, string value)
		{
			if (tab == null)
			{
				return;
			}
			TMP_Text componentInChildren = tab.GetComponentInChildren<TMP_Text>(true);
			if (componentInChildren != null)
			{
				LocalizedGameText.BindSource(componentInChildren, "phone_story", value, null).FitTranslationToRectangle(false);
			}
			Text componentInChildren2 = tab.GetComponentInChildren<Text>(true);
			if (componentInChildren2 != null)
			{
				LocalizedLegacyGameText.BindSource(componentInChildren2, "phone_story", value).FitTranslationToRectangle();
			}
		}

		// Token: 0x060019ED RID: 6637 RVA: 0x000A47D4 File Offset: 0x000A29D4
		private void SetTabVisuals(bool videos)
		{
			if (this._imagesSelected != null)
			{
				this._imagesSelected.SetActive(!videos);
			}
			if (this._videosSelected != null)
			{
				this._videosSelected.SetActive(videos);
			}
			PhotoGallery.SetTabLabelColor(this._imagesTab, videos ? new Color(1f, 1f, 1f, 0.48f) : Color.white);
			PhotoGallery.SetTabLabelColor(this._videosTab, videos ? Color.white : new Color(1f, 1f, 1f, 0.48f));
		}

		// Token: 0x060019EE RID: 6638 RVA: 0x000A4874 File Offset: 0x000A2A74
		private static void SetTabLabelColor(Transform tab, Color color)
		{
			if (tab == null)
			{
				return;
			}
			TMP_Text componentInChildren = tab.GetComponentInChildren<TMP_Text>(true);
			if (componentInChildren != null)
			{
				componentInChildren.color = color;
			}
			Text componentInChildren2 = tab.GetComponentInChildren<Text>(true);
			if (componentInChildren2 != null)
			{
				componentInChildren2.color = color;
			}
		}

		// Token: 0x060019EF RID: 6639 RVA: 0x000A48BC File Offset: 0x000A2ABC
		private static void TintSelection(GameObject selected)
		{
			Image image;
			if (selected != null && selected.TryGetComponent<Image>(out image))
			{
				image.color = new Color(0.59f, 0.29f, 0.78f, 1f);
				image.raycastTarget = false;
			}
		}

		// Token: 0x060019F0 RID: 6640 RVA: 0x000A4904 File Offset: 0x000A2B04
		private static GameObject CreateEmptyOverlay(string name, RectTransform viewport, string source, string scope)
		{
			GameObject gameObject = new GameObject(name, new Type[]
			{
				typeof(RectTransform),
				typeof(Text)
			});
			RectTransform rectTransform = (RectTransform)gameObject.transform;
			rectTransform.SetParent(viewport, false);
			rectTransform.anchorMin = new Vector2(0.08f, 0.36f);
			rectTransform.anchorMax = new Vector2(0.92f, 0.64f);
			rectTransform.offsetMin = Vector2.zero;
			rectTransform.offsetMax = Vector2.zero;
			Text component = gameObject.GetComponent<Text>();
			component.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
			component.fontSize = 18;
			component.alignment = TextAnchor.MiddleCenter;
			component.color = new Color(1f, 1f, 1f, 0.72f);
			component.raycastTarget = false;
			LocalizedLegacyGameText.BindSource(component, scope, source).FitTranslationToRectangle();
			return gameObject;
		}

		// Token: 0x060019F1 RID: 6641 RVA: 0x000A49E0 File Offset: 0x000A2BE0
		private void UpdateEmptyState()
		{
			if (this._emptyPhotos != null)
			{
				this._emptyPhotos.SetActive(!this._showingVideos && !this._hasPhotos);
			}
			if (this._emptyVideos != null)
			{
				this._emptyVideos.SetActive(this._showingVideos && !this._hasVideos);
			}
		}

		// Token: 0x060019F2 RID: 6642 RVA: 0x000A4A47 File Offset: 0x000A2C47
		private IEnumerator LoadImage(string filePath, Image image, PhonePhotoGalleryItem marker, int generation)
		{
			string absoluteUri = new Uri(filePath).AbsoluteUri;
			using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(absoluteUri, true))
			{
				yield return request.SendWebRequest();
				if (request.result != UnityWebRequest.Result.Success)
				{
					Debug.LogWarning("[PhotoGallery] Cannot load " + filePath + ": " + request.error);
					yield break;
				}
				Texture2D content = DownloadHandlerTexture.GetContent(request);
				if (content == null)
				{
					yield break;
				}
				if (generation != this._photoLoadGeneration || image == null || marker == null)
				{
					Object.Destroy(content);
					yield break;
				}
				Sprite sprite = Sprite.Create(content, new Rect(0f, 0f, (float)content.width, (float)content.height), new Vector2(0.5f, 0.5f), 100f);
				sprite.name = filePath;
				marker.runtimeSprite = sprite;
				image.sprite = sprite;
				StreamerCensorImage.Bind(image, false);
			}
			UnityWebRequest request = null;
			yield break;
			yield break;
		}

		// Token: 0x060019F3 RID: 6643 RVA: 0x000A4A74 File Offset: 0x000A2C74
		private void BuildVideoCell(string videoPath)
		{
			GameObject gameObject = Object.Instantiate<GameObject>(this.imagePrefab, this._videoParent, false);
			gameObject.name = Path.GetFileNameWithoutExtension(videoPath);
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.localEulerAngles = Vector3.zero;
			PhoneVideoGalleryItem phoneVideoGalleryItem = gameObject.AddComponent<PhoneVideoGalleryItem>();
			phoneVideoGalleryItem.videoPath = videoPath;
			Image image = ((gameObject.transform.childCount > 0) ? gameObject.transform.GetChild(0).GetComponent<Image>() : gameObject.GetComponentInChildren<Image>());
			Sprite sprite = PhotoGallery.LoadVideoThumbnail(videoPath);
			if (image != null)
			{
				image.sprite = sprite;
				StreamerCensorImage.Bind(image, false);
				image.color = Color.white;
			}
			phoneVideoGalleryItem.runtimeSprite = sprite;
			GetPhoto component = gameObject.GetComponent<GetPhoto>();
			if (component != null)
			{
				component.enabled = false;
			}
			Button[] components = gameObject.GetComponents<Button>();
			for (int i = 0; i < components.Length; i++)
			{
				components[i].enabled = false;
			}
			phoneVideoGalleryItem.videoPath = videoPath;
			PhotoGallery.AddVideoDecorations(gameObject.transform, PhoneVideoStorage.LoadMetadata(videoPath));
		}

		// Token: 0x060019F4 RID: 6644 RVA: 0x000A4B80 File Offset: 0x000A2D80
		private static void AddVideoDecorations(Transform item, PhoneVideoMetadata metadata)
		{
			GameObject gameObject = new GameObject("Play", new Type[]
			{
				typeof(RectTransform),
				typeof(Text)
			});
			RectTransform rectTransform = (RectTransform)gameObject.transform;
			rectTransform.SetParent(item, false);
			Vector2 vector = new Vector2(0.5f, 0.5f);
			rectTransform.anchorMax = vector;
			rectTransform.anchorMin = vector;
			rectTransform.pivot = new Vector2(0.5f, 0.5f);
			rectTransform.anchoredPosition = Vector2.zero;
			rectTransform.sizeDelta = new Vector2(42f, 42f);
			Text component = gameObject.GetComponent<Text>();
			component.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
			component.text = "▶";
			component.fontSize = 27;
			component.alignment = TextAnchor.MiddleCenter;
			component.color = new Color(1f, 1f, 1f, 0.9f);
			component.raycastTarget = false;
			GameObject gameObject2 = new GameObject("Duration", new Type[]
			{
				typeof(RectTransform),
				typeof(Image)
			});
			RectTransform rectTransform2 = (RectTransform)gameObject2.transform;
			rectTransform2.SetParent(item, false);
			RectTransform rectTransform3 = rectTransform2;
			RectTransform rectTransform4 = rectTransform2;
			vector = new Vector2(1f, 0f);
			rectTransform4.anchorMax = vector;
			rectTransform3.anchorMin = vector;
			rectTransform2.pivot = new Vector2(1f, 0f);
			rectTransform2.anchoredPosition = new Vector2(-5f, 5f);
			rectTransform2.sizeDelta = new Vector2(54f, 20f);
			Image component2 = gameObject2.GetComponent<Image>();
			component2.sprite = PhoneUiSpriteFactory.Rounded;
			component2.type = Image.Type.Sliced;
			component2.color = new Color(0f, 0f, 0f, 0.68f);
			component2.raycastTarget = false;
			GameObject gameObject3 = new GameObject("Label", new Type[]
			{
				typeof(RectTransform),
				typeof(Text)
			});
			RectTransform rectTransform5 = (RectTransform)gameObject3.transform;
			rectTransform5.SetParent(rectTransform2, false);
			rectTransform5.anchorMin = Vector2.zero;
			rectTransform5.anchorMax = Vector2.one;
			rectTransform5.offsetMin = Vector2.zero;
			rectTransform5.offsetMax = Vector2.zero;
			Text component3 = gameObject3.GetComponent<Text>();
			component3.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
			component3.fontSize = 12;
			component3.alignment = TextAnchor.MiddleCenter;
			component3.color = new Color(1f, 1f, 1f, 0.92f);
			component3.text = PhotoGallery.FormatDuration((metadata != null) ? metadata.durationSeconds : 0f);
			component3.raycastTarget = false;
		}

		// Token: 0x060019F5 RID: 6645 RVA: 0x000A4E18 File Offset: 0x000A3018
		private static Sprite LoadVideoThumbnail(string videoPath)
		{
			string text = PhoneVideoStorage.ThumbnailPathFor(videoPath);
			if (File.Exists(text))
			{
				try
				{
					Texture2D texture2D = new Texture2D(2, 2, TextureFormat.RGB24, false);
					if (texture2D.LoadImage(File.ReadAllBytes(text)))
					{
						Sprite sprite = Sprite.Create(texture2D, new Rect(0f, 0f, (float)texture2D.width, (float)texture2D.height), new Vector2(0.5f, 0.5f), 100f);
						sprite.name = videoPath;
						return sprite;
					}
					Object.Destroy(texture2D);
				}
				catch (Exception ex)
				{
					Debug.LogWarning("[PhotoGallery] Cannot load video thumbnail " + text + ": " + ex.Message);
				}
			}
			Texture2D texture2D2 = new Texture2D(8, 8, TextureFormat.RGB24, false);
			Color color = new Color(0.055f, 0.06f, 0.08f);
			Color color2 = new Color(0.12f, 0.13f, 0.16f);
			for (int i = 0; i < texture2D2.height; i++)
			{
				for (int j = 0; j < texture2D2.width; j++)
				{
					texture2D2.SetPixel(j, i, Color.Lerp(color, color2, (float)i / (float)texture2D2.height));
				}
			}
			texture2D2.Apply();
			Sprite sprite2 = Sprite.Create(texture2D2, new Rect(0f, 0f, 8f, 8f), new Vector2(0.5f, 0.5f), 100f);
			sprite2.name = videoPath;
			return sprite2;
		}

		// Token: 0x060019F6 RID: 6646 RVA: 0x000A4F94 File Offset: 0x000A3194
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

		// Token: 0x060019F7 RID: 6647 RVA: 0x000A4FF8 File Offset: 0x000A31F8
		private static void DestroyImmediateSafe(GameObject target)
		{
			if (target == null)
			{
				return;
			}
			if (Application.isPlaying)
			{
				Object.Destroy(target);
				return;
			}
			Object.DestroyImmediate(target);
		}

		// Token: 0x060019F8 RID: 6648 RVA: 0x000A5018 File Offset: 0x000A3218
		private static void ClearContent(Transform content)
		{
			if (content == null)
			{
				return;
			}
			for (int i = content.childCount - 1; i >= 0; i--)
			{
				GameObject gameObject = content.GetChild(i).gameObject;
				gameObject.SetActive(false);
				PhotoGallery.DestroyImmediateSafe(gameObject);
			}
		}

		// Token: 0x04001B0D RID: 6925
		public const float GalleryScrollSensitivity = 12f;

		// Token: 0x04001B0E RID: 6926
		public GameObject imagePrefab;

		// Token: 0x04001B0F RID: 6927
		public Transform parent;

		// Token: 0x04001B10 RID: 6928
		public GameObject FullScreenPanel;

		// Token: 0x04001B11 RID: 6929
		private string _photoPath;

		// Token: 0x04001B12 RID: 6930
		private Transform _videoParent;

		// Token: 0x04001B13 RID: 6931
		private ScrollRect _scrollRect;

		// Token: 0x04001B14 RID: 6932
		private Transform _imagesTab;

		// Token: 0x04001B15 RID: 6933
		private Transform _videosTab;

		// Token: 0x04001B16 RID: 6934
		private GameObject _imagesSelected;

		// Token: 0x04001B17 RID: 6935
		private GameObject _videosSelected;

		// Token: 0x04001B18 RID: 6936
		private GameObject _emptyPhotos;

		// Token: 0x04001B19 RID: 6937
		private GameObject _emptyVideos;

		// Token: 0x04001B1A RID: 6938
		private bool _hasPhotos;

		// Token: 0x04001B1B RID: 6939
		private bool _hasVideos;

		// Token: 0x04001B1C RID: 6940
		private bool _showingVideos;

		// Token: 0x04001B1D RID: 6941
		private bool _tabsReady;

		// Token: 0x04001B1E RID: 6942
		private bool _started;

		// Token: 0x04001B1F RID: 6943
		private int _photoLoadGeneration;
	}
}
