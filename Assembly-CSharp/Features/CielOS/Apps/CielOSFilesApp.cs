using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Features.CensorBars;
using Features.CielOS.UI;
using Features.CielOS.VFS;
using Features.CielOS.WindowManager;
using Features.Localization;
using Features.PhoneCompanion;
using ProjectExhibitionist.Core;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Features.CielOS.Apps
{
	// Token: 0x020013E8 RID: 5096
	public class CielOSFilesApp : MonoBehaviour
	{
		// Token: 0x06009D4A RID: 40266 RVA: 0x004099FC File Offset: 0x00407BFC
		private void Awake()
		{
			if (string.Equals(this.startFolder, "/Photos", StringComparison.OrdinalIgnoreCase))
			{
				CielOSPhotosApp cielOSPhotosApp = base.GetComponent<CielOSPhotosApp>();
				if (cielOSPhotosApp == null)
				{
					cielOSPhotosApp = base.gameObject.AddComponent<CielOSPhotosApp>();
				}
				cielOSPhotosApp.Initialize(this.pathLabel, this.listContainer, this.upButton, this.newFolderButton, this.newFolderName);
				base.enabled = false;
				return;
			}
			CielOSContentText.BindButton(this.upButton, "Up");
			CielOSContentText.BindButton(this.newFolderButton, "New Folder");
			LocalizedGameText.BindFontOnly(this.pathLabel);
			if (this.upButton != null)
			{
				this.upButton.onClick.AddListener(new UnityAction(this.GoUp));
			}
			if (this.newFolderButton != null)
			{
				this.newFolderButton.onClick.AddListener(new UnityAction(this.CreateFolder));
				this.BuildImportButton();
			}
			if (this.newFolderName != null)
			{
				this.newFolderName.gameObject.SetActive(false);
			}
			if (!string.IsNullOrEmpty(this.startFolder) && CielOSFileSystem.Instance.Exists(this.startFolder))
			{
				this._current = this.startFolder;
			}
		}

		// Token: 0x06009D4B RID: 40267 RVA: 0x00409B34 File Offset: 0x00407D34
		private void BuildImportButton()
		{
			CielOSTheme active = CielOSTheme.Active;
			RectTransform rectTransform = (RectTransform)this.newFolderButton.transform;
			GameObject gameObject = new GameObject("ImportButton", new Type[]
			{
				typeof(RectTransform),
				typeof(Image),
				typeof(Button)
			});
			gameObject.transform.SetParent(rectTransform.parent, false);
			RectTransform rectTransform2 = (RectTransform)gameObject.transform;
			rectTransform2.anchorMin = rectTransform.anchorMin;
			rectTransform2.anchorMax = rectTransform.anchorMax;
			rectTransform2.pivot = rectTransform.pivot;
			rectTransform2.sizeDelta = rectTransform.sizeDelta;
			rectTransform2.anchoredPosition = rectTransform.anchoredPosition + new Vector2(-(rectTransform.sizeDelta.x + 8f), 0f);
			Image component = gameObject.GetComponent<Image>();
			component.color = ((active != null) ? active.accent : new Color(0.66f, 0.38f, 0.79f, 1f));
			CielOSTheme.Round(component);
			gameObject.GetComponent<Button>().onClick.AddListener(new UnityAction(this.ImportFile));
			GameObject gameObject2 = new GameObject("Icon", new Type[]
			{
				typeof(RectTransform),
				typeof(Image)
			});
			gameObject2.transform.SetParent(gameObject.transform, false);
			RectTransform rectTransform3 = (RectTransform)gameObject2.transform;
			rectTransform3.anchorMin = new Vector2(0f, 0.5f);
			rectTransform3.anchorMax = new Vector2(0f, 0.5f);
			rectTransform3.pivot = new Vector2(0f, 0.5f);
			rectTransform3.sizeDelta = new Vector2(16f, 16f);
			rectTransform3.anchoredPosition = new Vector2(10f, 0f);
			CielOSIcon.Apply(gameObject2.GetComponent<Image>(), CielOSIconId.Upload, Color.white, 64);
			GameObject gameObject3 = new GameObject("L", new Type[]
			{
				typeof(RectTransform),
				typeof(TextMeshProUGUI)
			});
			gameObject3.transform.SetParent(gameObject.transform, false);
			RectTransform rectTransform4 = (RectTransform)gameObject3.transform;
			rectTransform4.anchorMin = Vector2.zero;
			rectTransform4.anchorMax = Vector2.one;
			rectTransform4.offsetMin = new Vector2(28f, 0f);
			rectTransform4.offsetMax = new Vector2(-6f, 0f);
			TextMeshProUGUI component2 = gameObject3.GetComponent<TextMeshProUGUI>();
			component2.text = "Import";
			component2.fontSize = 13f;
			component2.alignment = TextAlignmentOptions.MidlineLeft;
			component2.color = Color.white;
			component2.raycastTarget = false;
			if (active != null && active.uiFont != null)
			{
				component2.font = active.uiFont;
			}
			CielOSContentText.Bind(component2, "Import");
		}

		// Token: 0x06009D4C RID: 40268 RVA: 0x00409E10 File Offset: 0x00408010
		private void ImportFile()
		{
			CielOSFileBrowser browser = CielOSFileBrowser.Ensure(base.GetComponentInParent<Canvas>().transform);
			browser.Open("Import file", new string[] { ".png", ".jpg", ".jpeg" }, delegate(string real)
			{
				if (string.IsNullOrEmpty(real))
				{
					return;
				}
				string text = ((CielOSFileSystem.Instance.IsProtected(this._current) && this._current == "/") ? "/Pictures" : this._current);
				if (CielOSFileSystem.Instance.ImportExternalFile(real, text) != null)
				{
					browser.RememberSuccessfulDirectory(real);
					this.Refresh();
				}
			}, null, "cielos_shell", false);
		}

		// Token: 0x06009D4D RID: 40269 RVA: 0x00409E81 File Offset: 0x00408081
		private void OnEnable()
		{
			this.Refresh();
		}

		// Token: 0x06009D4E RID: 40270 RVA: 0x00409E8C File Offset: 0x0040808C
		private void LateUpdate()
		{
			if (!this._gridMode || this.listContainer == null)
			{
				return;
			}
			float width = this.listContainer.rect.width;
			if (width > 1f && Mathf.Abs(width - this._lastGridWidth) > 0.5f)
			{
				this.LayoutGridTiles();
			}
		}

		// Token: 0x06009D4F RID: 40271 RVA: 0x00409EE8 File Offset: 0x004080E8
		private void OnDestroy()
		{
			foreach (Sprite sprite in this._thumbCache.Values)
			{
				CielOSFilesApp.DestroyRuntimeThumbnail(sprite);
			}
			this._thumbCache.Clear();
		}

		// Token: 0x06009D50 RID: 40272 RVA: 0x00409F48 File Offset: 0x00408148
		private void ApplyLayout(bool grid)
		{
			if (this.listContainer == null)
			{
				return;
			}
			this._gridMode = grid;
			VerticalLayoutGroup verticalLayoutGroup = this.listContainer.GetComponent<VerticalLayoutGroup>();
			if (verticalLayoutGroup == null)
			{
				verticalLayoutGroup = this.listContainer.gameObject.AddComponent<VerticalLayoutGroup>();
			}
			verticalLayoutGroup.spacing = 4f;
			verticalLayoutGroup.padding = new RectOffset(6, 6, 6, 6);
			verticalLayoutGroup.childAlignment = TextAnchor.UpperLeft;
			verticalLayoutGroup.childControlWidth = true;
			verticalLayoutGroup.childControlHeight = true;
			verticalLayoutGroup.childForceExpandWidth = true;
			verticalLayoutGroup.childForceExpandHeight = false;
			verticalLayoutGroup.enabled = !grid;
			ContentSizeFitter contentSizeFitter = this.listContainer.GetComponent<ContentSizeFitter>();
			if (contentSizeFitter == null)
			{
				contentSizeFitter = this.listContainer.gameObject.AddComponent<ContentSizeFitter>();
			}
			contentSizeFitter.verticalFit = (grid ? ContentSizeFitter.FitMode.Unconstrained : ContentSizeFitter.FitMode.PreferredSize);
			contentSizeFitter.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
			contentSizeFitter.enabled = !grid;
			if (grid)
			{
				this.listContainer.anchorMin = new Vector2(0f, 1f);
				this.listContainer.anchorMax = new Vector2(1f, 1f);
				this.listContainer.pivot = new Vector2(0.5f, 1f);
				this.listContainer.anchoredPosition = Vector2.zero;
				this.listContainer.sizeDelta = new Vector2(0f, this.listContainer.sizeDelta.y);
			}
		}

		// Token: 0x06009D51 RID: 40273 RVA: 0x0040A0A8 File Offset: 0x004082A8
		private void Refresh()
		{
			if (this.pathLabel != null)
			{
				this.pathLabel.text = this._current;
			}
			foreach (GameObject gameObject in this._rows)
			{
				if (gameObject != null)
				{
					Object.Destroy(gameObject);
				}
			}
			this._rows.Clear();
			if (this.listContainer == null)
			{
				return;
			}
			List<CielOSFileSystem.Entry> list = CielOSFileSystem.Instance.List(this._current);
			list.RemoveAll((CielOSFileSystem.Entry entry) => CielOSFilesApp.ShouldHidePrivateMedia(this._current, entry));
			bool flag = false;
			foreach (CielOSFileSystem.Entry entry3 in list)
			{
				if (!entry3.isDirectory && CielOSWallpaperService.IsImage(entry3.name))
				{
					flag = true;
					break;
				}
			}
			this.ApplyLayout(flag);
			foreach (CielOSFileSystem.Entry entry2 in list)
			{
				this._rows.Add(this._gridMode ? this.BuildTile(entry2) : this.BuildRow(entry2));
			}
			if (this._gridMode && base.isActiveAndEnabled)
			{
				base.StartCoroutine(this.LayoutGridTilesNextFrame());
			}
		}

		// Token: 0x06009D52 RID: 40274 RVA: 0x0040A238 File Offset: 0x00408438
		private static bool ShouldHidePrivateMedia(string currentVirtualDirectory, CielOSFileSystem.Entry entry)
		{
			if (entry.isDirectory || !PhonePhotoStorage.IsExcludedFromGallery(entry.name))
			{
				return false;
			}
			string text = (currentVirtualDirectory ?? "/").Replace('\\', '/').TrimEnd('/');
			return text.Equals("/Photos", StringComparison.OrdinalIgnoreCase) || text.StartsWith("/Photos/", StringComparison.OrdinalIgnoreCase);
		}

		// Token: 0x06009D53 RID: 40275 RVA: 0x0040A293 File Offset: 0x00408493
		private IEnumerator LayoutGridTilesNextFrame()
		{
			yield return null;
			this.LayoutGridTiles();
			yield break;
		}

		// Token: 0x06009D54 RID: 40276 RVA: 0x0040A2A4 File Offset: 0x004084A4
		private void LayoutGridTiles()
		{
			if (this.listContainer == null || !this._gridMode)
			{
				return;
			}
			float width = this.listContainer.rect.width;
			if (width < 1f)
			{
				return;
			}
			this._lastGridWidth = width;
			int num = Mathf.Max(1, Mathf.FloorToInt((width - 20f + 8f) / 104f));
			int num2 = 0;
			foreach (GameObject gameObject in this._rows)
			{
				if (!(gameObject == null))
				{
					RectTransform rectTransform = (RectTransform)gameObject.transform;
					rectTransform.anchorMin = new Vector2(0f, 1f);
					rectTransform.anchorMax = new Vector2(0f, 1f);
					rectTransform.pivot = new Vector2(0f, 1f);
					rectTransform.sizeDelta = new Vector2(96f, 112f);
					int num3 = num2 % num;
					int num4 = num2 / num;
					rectTransform.anchoredPosition = new Vector2(10f + (float)num3 * 104f, -(10f + (float)num4 * 120f));
					num2++;
				}
			}
			int num5 = (num2 + num - 1) / num;
			float num6 = 20f + (float)num5 * 112f + (float)Mathf.Max(0, num5 - 1) * 8f;
			this.listContainer.sizeDelta = new Vector2(this.listContainer.sizeDelta.x, num6);
		}

		// Token: 0x06009D55 RID: 40277 RVA: 0x0040A444 File Offset: 0x00408644
		private GameObject BuildRow(CielOSFileSystem.Entry entry)
		{
			bool locked = entry.isDirectory && CielOSFileSystem.Instance.IsProtected(entry.virtualPath);
			CielOSTheme active = CielOSTheme.Active;
			Color color = ((active != null) ? active.controlBackground : this.fileColor);
			Color color2 = ((active != null) ? active.controlText : this.textColor);
			Color color3 = ((active != null) ? active.accent : this.folderColor);
			GameObject gameObject = new GameObject(entry.name, new Type[]
			{
				typeof(RectTransform),
				typeof(Image),
				typeof(Button),
				typeof(LayoutElement),
				typeof(CielOSRightClickable)
			});
			gameObject.transform.SetParent(this.listContainer, false);
			Image component = gameObject.GetComponent<Image>();
			component.color = color;
			CielOSTheme.Round(component);
			LayoutElement component2 = gameObject.GetComponent<LayoutElement>();
			component2.minHeight = 32f;
			component2.preferredHeight = 32f;
			component2.flexibleHeight = 0f;
			GameObject gameObject2 = new GameObject("Icon", new Type[]
			{
				typeof(RectTransform),
				typeof(Image)
			});
			gameObject2.transform.SetParent(gameObject.transform, false);
			RectTransform rectTransform = (RectTransform)gameObject2.transform;
			rectTransform.anchorMin = new Vector2(0f, 0.5f);
			rectTransform.anchorMax = new Vector2(0f, 0.5f);
			rectTransform.pivot = new Vector2(0f, 0.5f);
			rectTransform.sizeDelta = new Vector2(18f, 18f);
			rectTransform.anchoredPosition = new Vector2(10f, 0f);
			CielOSIcon.Apply(gameObject2.GetComponent<Image>(), entry.isDirectory ? CielOSIconId.Folder : CielOSIconId.Files, locked ? new Color(color3.r, color3.g, color3.b, 0.6f) : color3, 64);
			GameObject gameObject3 = new GameObject("Label", new Type[]
			{
				typeof(RectTransform),
				typeof(TextMeshProUGUI)
			});
			gameObject3.transform.SetParent(gameObject.transform, false);
			RectTransform rectTransform2 = (RectTransform)gameObject3.transform;
			rectTransform2.anchorMin = Vector2.zero;
			rectTransform2.anchorMax = Vector2.one;
			rectTransform2.offsetMin = new Vector2(36f, 0f);
			rectTransform2.offsetMax = new Vector2(-10f, 0f);
			TextMeshProUGUI component3 = gameObject3.GetComponent<TextMeshProUGUI>();
			component3.text = (locked ? (entry.name + "  ·  sys") : entry.name);
			component3.fontSize = 13f;
			component3.alignment = TextAlignmentOptions.MidlineLeft;
			component3.color = color2;
			component3.raycastTarget = false;
			component3.enableWordWrapping = false;
			component3.overflowMode = TextOverflowModes.Ellipsis;
			if (active != null && active.uiFont != null)
			{
				component3.font = active.uiFont;
			}
			LocalizedGameText.BindFontOnly(component3);
			if (locked)
			{
				CielOSContentText.BindFormat(component3, "{0}  ·  sys", new object[] { entry.name });
			}
			CielOSFileSystem.Entry e = entry;
			bool flag = !entry.isDirectory && CielOSWallpaperService.IsImage(entry.name);
			bool isVideo = !entry.isDirectory && CielOSMediaLibrary.IsVideoFileName(entry.name);
			gameObject.GetComponent<Button>().onClick.AddListener(delegate
			{
				CielOSSfx.PlayClick();
				if (e.isDirectory)
				{
					this._current = e.virtualPath;
					this.Refresh();
				}
			});
			if (flag | isVideo)
			{
				gameObject.AddComponent<CielOSClickable>().OnDoubleClick = delegate(Vector2 _)
				{
					CielOSSfx.PlayClick();
					if (isVideo)
					{
						this.OpenVideoViewer(e);
						return;
					}
					this.OpenImageViewer(e);
				};
			}
			gameObject.GetComponent<CielOSRightClickable>().OnRightClick = delegate(Vector2 pos)
			{
				this.ShowContextMenu(e, locked, pos);
			};
			return gameObject;
		}

		// Token: 0x06009D56 RID: 40278 RVA: 0x0040A830 File Offset: 0x00408A30
		private GameObject BuildTile(CielOSFileSystem.Entry entry)
		{
			bool locked = entry.isDirectory && CielOSFileSystem.Instance.IsProtected(entry.virtualPath);
			bool flag = !entry.isDirectory && CielOSWallpaperService.IsImage(entry.name);
			bool flag2 = !entry.isDirectory && CielOSMediaLibrary.IsVideoFileName(entry.name);
			CielOSTheme active = CielOSTheme.Active;
			Color color = ((active != null) ? active.controlBackground : this.fileColor);
			Color color2 = ((active != null) ? active.controlText : this.textColor);
			Color color3 = ((active != null) ? active.accent : this.folderColor);
			GameObject gameObject = new GameObject(entry.name, new Type[]
			{
				typeof(RectTransform),
				typeof(Image),
				typeof(CielOSClickable),
				typeof(CielOSRightClickable)
			});
			gameObject.transform.SetParent(this.listContainer, false);
			RectTransform rectTransform = (RectTransform)gameObject.transform;
			rectTransform.anchorMin = new Vector2(0f, 1f);
			rectTransform.anchorMax = new Vector2(0f, 1f);
			rectTransform.pivot = new Vector2(0f, 1f);
			rectTransform.sizeDelta = new Vector2(96f, 112f);
			gameObject.GetComponent<Image>().color = new Color(0f, 0f, 0f, 0f);
			GameObject gameObject2 = new GameObject("Thumb", new Type[]
			{
				typeof(RectTransform),
				typeof(Image),
				typeof(RectMask2D)
			});
			gameObject2.transform.SetParent(gameObject.transform, false);
			RectTransform rectTransform2 = (RectTransform)gameObject2.transform;
			rectTransform2.anchorMin = new Vector2(0.5f, 1f);
			rectTransform2.anchorMax = new Vector2(0.5f, 1f);
			rectTransform2.pivot = new Vector2(0.5f, 1f);
			rectTransform2.sizeDelta = new Vector2(88f, 88f);
			rectTransform2.anchoredPosition = new Vector2(0f, -2f);
			gameObject2.GetComponent<Image>().color = (flag ? new Color(0.06f, 0.06f, 0.08f, 1f) : color);
			if (flag)
			{
				Sprite thumb = this.GetThumb(entry.virtualPath);
				GameObject gameObject3 = new GameObject("Img", new Type[]
				{
					typeof(RectTransform),
					typeof(RawImage)
				});
				gameObject3.transform.SetParent(gameObject2.transform, false);
				RawImage component = gameObject3.GetComponent<RawImage>();
				component.raycastTarget = false;
				RectTransform rectTransform3 = (RectTransform)gameObject3.transform;
				if (thumb != null && thumb.texture != null)
				{
					component.texture = thumb.texture;
					StreamerCensorImage.Bind(component, false);
					float num = ((thumb.texture.height > 0) ? ((float)thumb.texture.width / (float)thumb.texture.height) : 1f);
					RectTransform rectTransform4 = rectTransform3;
					RectTransform rectTransform5 = rectTransform3;
					Vector2 vector = new Vector2(0.5f, 0.5f);
					rectTransform5.anchorMax = vector;
					rectTransform4.anchorMin = vector;
					rectTransform3.pivot = new Vector2(0.5f, 0.5f);
					rectTransform3.anchoredPosition = Vector2.zero;
					float num2 = 88f;
					rectTransform3.sizeDelta = ((num >= 1f) ? new Vector2(num2 * num, num2) : new Vector2(num2, num2 / num));
				}
				else
				{
					rectTransform3.anchorMin = Vector2.zero;
					rectTransform3.anchorMax = Vector2.one;
					rectTransform3.offsetMin = Vector2.zero;
					rectTransform3.offsetMax = Vector2.zero;
					component.color = new Color(1f, 1f, 1f, 0.06f);
				}
			}
			else
			{
				GameObject gameObject4 = new GameObject("Icon", new Type[]
				{
					typeof(RectTransform),
					typeof(Image)
				});
				gameObject4.transform.SetParent(gameObject2.transform, false);
				RectTransform rectTransform6 = (RectTransform)gameObject4.transform;
				Vector2 vector = new Vector2(0.5f, 0.5f);
				rectTransform6.anchorMax = vector;
				rectTransform6.anchorMin = vector;
				rectTransform6.pivot = new Vector2(0.5f, 0.5f);
				rectTransform6.sizeDelta = new Vector2(46f, 46f);
				rectTransform6.anchoredPosition = Vector2.zero;
				CielOSIcon.Apply(gameObject4.GetComponent<Image>(), entry.isDirectory ? CielOSIconId.Folder : CielOSIconId.Files, locked ? new Color(color3.r, color3.g, color3.b, 0.6f) : color3, 64);
			}
			GameObject gameObject5 = new GameObject("Label", new Type[]
			{
				typeof(RectTransform),
				typeof(TextMeshProUGUI)
			});
			gameObject5.transform.SetParent(gameObject.transform, false);
			RectTransform rectTransform7 = (RectTransform)gameObject5.transform;
			rectTransform7.anchorMin = new Vector2(0f, 0f);
			rectTransform7.anchorMax = new Vector2(1f, 0f);
			rectTransform7.pivot = new Vector2(0.5f, 0f);
			rectTransform7.offsetMin = new Vector2(3f, 1f);
			rectTransform7.offsetMax = new Vector2(-3f, 21f);
			TextMeshProUGUI component2 = gameObject5.GetComponent<TextMeshProUGUI>();
			component2.text = entry.name;
			component2.fontSize = 11f;
			component2.alignment = TextAlignmentOptions.Top;
			component2.color = color2;
			component2.raycastTarget = false;
			component2.enableWordWrapping = false;
			component2.overflowMode = TextOverflowModes.Ellipsis;
			if (active != null && active.uiFont != null)
			{
				component2.font = active.uiFont;
			}
			LocalizedGameText.BindFontOnly(component2);
			CielOSFileSystem.Entry e = entry;
			bool eIsImg = flag;
			bool eIsVideo = flag2;
			CielOSClickable component3 = gameObject.GetComponent<CielOSClickable>();
			component3.OnClick = delegate(Vector2 _)
			{
				CielOSSfx.PlayClick();
				if (e.isDirectory)
				{
					this._current = e.virtualPath;
					this.Refresh();
				}
			};
			component3.OnDoubleClick = delegate(Vector2 _)
			{
				CielOSSfx.PlayClick();
				if (e.isDirectory)
				{
					this._current = e.virtualPath;
					this.Refresh();
					return;
				}
				if (eIsImg)
				{
					this.OpenImageViewer(e);
					return;
				}
				if (eIsVideo)
				{
					this.OpenVideoViewer(e);
				}
			};
			gameObject.GetComponent<CielOSRightClickable>().OnRightClick = delegate(Vector2 pos)
			{
				this.ShowContextMenu(e, locked, pos);
			};
			return gameObject;
		}

		// Token: 0x06009D57 RID: 40279 RVA: 0x0040AEC0 File Offset: 0x004090C0
		private Sprite GetThumb(string virtualPath)
		{
			Sprite sprite;
			if (this._thumbCache.TryGetValue(virtualPath, out sprite) && sprite != null)
			{
				return sprite;
			}
			Sprite sprite2 = CielOSWallpaperService.LoadSprite(virtualPath);
			if (sprite2 != null)
			{
				this._thumbCache[virtualPath] = sprite2;
			}
			return sprite2;
		}

		// Token: 0x06009D58 RID: 40280 RVA: 0x0040AF08 File Offset: 0x00409108
		private void RemoveCachedThumbnail(string virtualPath)
		{
			Sprite sprite;
			if (!this._thumbCache.TryGetValue(virtualPath, out sprite))
			{
				return;
			}
			this._thumbCache.Remove(virtualPath);
			CielOSFilesApp.DestroyRuntimeThumbnail(sprite);
		}

		// Token: 0x06009D59 RID: 40281 RVA: 0x0040AF3C File Offset: 0x0040913C
		private static void DestroyRuntimeThumbnail(Sprite sprite)
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

		// Token: 0x06009D5A RID: 40282 RVA: 0x0040AF70 File Offset: 0x00409170
		private void OpenImageViewer(CielOSFileSystem.Entry entry)
		{
			Sprite thumb = this.GetThumb(entry.virtualPath);
			if (thumb == null || thumb.texture == null)
			{
				return;
			}
			CielOSWindowManager cielOSWindowManager = Object.FindObjectOfType<CielOSWindowManager>();
			if (cielOSWindowManager == null)
			{
				return;
			}
			float num = ((thumb.texture.height > 0) ? ((float)thumb.texture.width / (float)thumb.texture.height) : 1f);
			float num2 = 560f;
			float num3 = Mathf.Clamp(num2 * num, 360f, 1100f);
			CielOSWindow cielOSWindow = cielOSWindowManager.LaunchTransientWindow(entry.name, null, new Vector2(num3, num2 + 40f), "imageviewer");
			if (cielOSWindow == null || cielOSWindow.Content == null)
			{
				return;
			}
			CielOSImageViewerApp.Mount(cielOSWindow.Content, entry.virtualPath);
		}

		// Token: 0x06009D5B RID: 40283 RVA: 0x0040B04C File Offset: 0x0040924C
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
			float num2 = 560f;
			float num3 = Mathf.Clamp(num2 * num, 420f, 1100f);
			CielOSWindow cielOSWindow = cielOSWindowManager.LaunchTransientWindow(entry.name, null, new Vector2(num3, num2 + 40f), "videoviewer");
			if (cielOSWindow != null && cielOSWindow.Content != null)
			{
				CielOSVideoViewerApp.Mount(cielOSWindow.Content, entry.virtualPath);
			}
		}

		// Token: 0x06009D5C RID: 40284 RVA: 0x0040B118 File Offset: 0x00409318
		private void ShowContextMenu(CielOSFileSystem.Entry e, bool locked, Vector2 pos)
		{
			if (CielOSContextMenu.Instance == null)
			{
				return;
			}
			List<CielOSContextMenu.Item> list = new List<CielOSContextMenu.Item>();
			if (e.isDirectory)
			{
				list.Add(new CielOSContextMenu.Item("Open", delegate
				{
					this._current = e.virtualPath;
					this.Refresh();
				}, true, false, null));
			}
			object obj = !e.isDirectory && CielOSWallpaperService.IsImage(e.name);
			bool isVideo = !e.isDirectory && CielOSMediaLibrary.IsVideoFileName(e.name);
			object obj2 = obj;
			if (obj2 != null)
			{
				list.Add(new CielOSContextMenu.Item("Open", delegate
				{
					this.OpenImageViewer(e);
				}, true, false, null));
				list.Add(new CielOSContextMenu.Item("Set as wallpaper", delegate
				{
					CielOSWallpaperService instance = CielOSWallpaperService.Instance;
					if (instance == null)
					{
						return;
					}
					instance.ApplyWallpaper(e.virtualPath, true);
				}, true, false, null));
				list.Add(new CielOSContextMenu.Item("Set as screensaver...", delegate
				{
					CielOSWallpaperService.ShowScreensaverTargetMenu(pos, e.virtualPath);
				}, true, false, null));
			}
			else if (isVideo)
			{
				list.Add(new CielOSContextMenu.Item("Open", delegate
				{
					this.OpenVideoViewer(e);
				}, true, false, null));
			}
			if ((obj2 | isVideo) != null)
			{
				list.Add(new CielOSContextMenu.Item("Open folder", delegate
				{
					PlayerMediaExport.OpenDirectory(Path.GetDirectoryName(CielOSFileSystem.Instance.ToReal(e.virtualPath)));
				}, true, false, "phone_story"));
			}
			list.Add(new CielOSContextMenu.Item("Rename", delegate
			{
				this.RenameEntry(e);
			}, !locked, false, null));
			list.Add(new CielOSContextMenu.Item("Delete", delegate
			{
				if (isVideo ? CielOSMediaLibrary.DeleteMedia(e.virtualPath, true) : CielOSFileSystem.Instance.Delete(e.virtualPath))
				{
					this.RemoveCachedThumbnail(e.virtualPath);
				}
				this.Refresh();
			}, !locked, false, null));
			for (int i = 0; i < list.Count; i++)
			{
				CielOSContextMenu.Item item = list[i];
				if (string.IsNullOrEmpty(item.localizationScope))
				{
					item.localizationScope = "cielos_content";
				}
				list[i] = item;
			}
			CielOSContextMenu.Instance.Show(pos, list);
		}

		// Token: 0x06009D5D RID: 40285 RVA: 0x0040B30C File Offset: 0x0040950C
		private void RenameEntry(CielOSFileSystem.Entry e)
		{
			CielOSTextPrompt.Ensure(base.GetComponentInParent<Canvas>().transform).Show("Rename", e.name, delegate(string newName)
			{
				string text;
				if ((!e.isDirectory && CielOSMediaLibrary.IsVideoFileName(e.name)) ? CielOSMediaLibrary.RenameMedia(e.virtualPath, newName, true, out text) : CielOSFileSystem.Instance.Rename(e.virtualPath, newName))
				{
					this.RemoveCachedThumbnail(e.virtualPath);
				}
				this.Refresh();
			});
		}

		// Token: 0x06009D5E RID: 40286 RVA: 0x0040B360 File Offset: 0x00409560
		private void GoUp()
		{
			if (this._current == "/" || string.IsNullOrEmpty(this._current))
			{
				return;
			}
			int num = this._current.TrimEnd('/').LastIndexOf('/');
			this._current = ((num <= 0) ? "/" : this._current.TrimEnd('/').Substring(0, num));
			this.Refresh();
		}

		// Token: 0x06009D5F RID: 40287 RVA: 0x0040B3CD File Offset: 0x004095CD
		private void CreateFolder()
		{
			CielOSTextPrompt.Ensure(base.GetComponentInParent<Canvas>().transform).Show("New Folder", "New Folder", delegate(string name)
			{
				CielOSFileSystem.Instance.CreateFolder(this._current, name);
				this.Refresh();
			});
		}

		// Token: 0x0400A1C7 RID: 41415
		[Header("References")]
		[SerializeField]
		private TMP_Text pathLabel;

		// Token: 0x0400A1C8 RID: 41416
		[SerializeField]
		private RectTransform listContainer;

		// Token: 0x0400A1C9 RID: 41417
		[SerializeField]
		private Button upButton;

		// Token: 0x0400A1CA RID: 41418
		[SerializeField]
		private Button newFolderButton;

		// Token: 0x0400A1CB RID: 41419
		[SerializeField]
		private TMP_InputField newFolderName;

		// Token: 0x0400A1CC RID: 41420
		[Header("Start location")]
		[Tooltip("Virtual folder to open at startup (e.g. \"/Photos\" for a Photos shortcut). Empty = root.")]
		[SerializeField]
		private string startFolder = "";

		// Token: 0x0400A1CD RID: 41421
		[Header("Style")]
		[SerializeField]
		private Color folderColor = new Color(0.42f, 0.22f, 0.52f, 1f);

		// Token: 0x0400A1CE RID: 41422
		[SerializeField]
		private Color fileColor = new Color(0.18f, 0.16f, 0.22f, 1f);

		// Token: 0x0400A1CF RID: 41423
		[SerializeField]
		private Color lockedColor = new Color(0.26f, 0.2f, 0.32f, 1f);

		// Token: 0x0400A1D0 RID: 41424
		[SerializeField]
		private Color textColor = new Color(0.9f, 0.92f, 0.96f, 1f);

		// Token: 0x0400A1D1 RID: 41425
		private string _current = "/";

		// Token: 0x0400A1D2 RID: 41426
		private readonly List<GameObject> _rows = new List<GameObject>();

		// Token: 0x0400A1D3 RID: 41427
		private bool _gridMode;

		// Token: 0x0400A1D4 RID: 41428
		private const float GridCellW = 96f;

		// Token: 0x0400A1D5 RID: 41429
		private const float GridCellH = 112f;

		// Token: 0x0400A1D6 RID: 41430
		private const float GridGap = 8f;

		// Token: 0x0400A1D7 RID: 41431
		private const float GridPad = 10f;

		// Token: 0x0400A1D8 RID: 41432
		private const float GridThumb = 88f;

		// Token: 0x0400A1D9 RID: 41433
		private const float GridLabelH = 20f;

		// Token: 0x0400A1DA RID: 41434
		private float _lastGridWidth = -1f;

		// Token: 0x0400A1DB RID: 41435
		private readonly Dictionary<string, Sprite> _thumbCache = new Dictionary<string, Sprite>();
	}
}
