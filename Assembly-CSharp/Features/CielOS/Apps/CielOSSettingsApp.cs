using System;
using System.Collections.Generic;
using System.IO;
using Features.CensorBars;
using Features.CielOS.UI;
using Features.CielOS.VFS;
using Features.Localization;
using ProjectExhibitionist.Core;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Features.CielOS.Apps
{
	// Token: 0x0200140B RID: 5131
	public class CielOSSettingsApp : MonoBehaviour
	{
		// Token: 0x06009E60 RID: 40544 RVA: 0x004123A5 File Offset: 0x004105A5
		private void Awake()
		{
			if (this.refreshButton != null)
			{
				this.refreshButton.onClick.AddListener(new UnityAction(this.Refresh));
				this.BuildImportButton();
			}
		}

		// Token: 0x06009E61 RID: 40545 RVA: 0x004123D8 File Offset: 0x004105D8
		private void BuildImportButton()
		{
			CielOSTheme active = CielOSTheme.Active;
			RectTransform rectTransform = (RectTransform)this.refreshButton.transform;
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
			gameObject.GetComponent<Button>().onClick.AddListener(new UnityAction(this.ImportImage));
			GameObject gameObject2 = new GameObject("Icon", new Type[]
			{
				typeof(RectTransform),
				typeof(Image)
			});
			gameObject2.transform.SetParent(gameObject.transform, false);
			RectTransform rectTransform3 = (RectTransform)gameObject2.transform;
			Vector2 vector = new Vector2(0.5f, 0.5f);
			rectTransform3.pivot = vector;
			rectTransform3.anchorMin = (rectTransform3.anchorMax = vector);
			rectTransform3.sizeDelta = new Vector2(18f, 18f);
			CielOSIcon.Apply(gameObject2.GetComponent<Image>(), CielOSIconId.Upload, Color.white, 64);
		}

		// Token: 0x06009E62 RID: 40546 RVA: 0x0041259C File Offset: 0x0041079C
		private void ImportImage()
		{
			CielOSFileBrowser browser = CielOSFileBrowser.Ensure(base.GetComponentInParent<Canvas>().transform);
			browser.Open("Import image", new string[] { ".png", ".jpg", ".jpeg" }, delegate(string real)
			{
				if (string.IsNullOrEmpty(real))
				{
					return;
				}
				if (CielOSFileSystem.Instance.ImportExternalFile(real, "/Pictures") != null)
				{
					browser.RememberSuccessfulDirectory(real);
					this.Refresh();
				}
			}, null, "cielos_settings", false);
		}

		// Token: 0x06009E63 RID: 40547 RVA: 0x0041260D File Offset: 0x0041080D
		private void OnEnable()
		{
			GameLocalization.LocaleChanged += this.RefreshLocale;
			this.Refresh();
		}

		// Token: 0x06009E64 RID: 40548 RVA: 0x00412626 File Offset: 0x00410826
		private void OnDisable()
		{
			GameLocalization.LocaleChanged -= this.RefreshLocale;
		}

		// Token: 0x06009E65 RID: 40549 RVA: 0x0041263C File Offset: 0x0041083C
		private void Refresh()
		{
			foreach (GameObject gameObject in this._cells)
			{
				if (gameObject != null)
				{
					Object.Destroy(gameObject);
				}
			}
			this._cells.Clear();
			CielOSFileSystem instance = CielOSFileSystem.Instance;
			int num = 0;
			foreach (string text in CielOSSettingsApp.ScanDirs)
			{
				foreach (CielOSFileSystem.Entry entry in instance.List(text))
				{
					if (!entry.isDirectory && CielOSWallpaperService.IsImage(entry.name))
					{
						this.BuildCell(entry.virtualPath, entry.name);
						num++;
					}
				}
			}
			this._imageCount = num;
			this.RefreshHeader();
		}

		// Token: 0x06009E66 RID: 40550 RVA: 0x00412748 File Offset: 0x00410948
		private void RefreshLocale()
		{
			this.RefreshHeader();
		}

		// Token: 0x06009E67 RID: 40551 RVA: 0x00412750 File Offset: 0x00410950
		private void RefreshHeader()
		{
			if (this.header == null)
			{
				return;
			}
			string text = ((this._imageCount > 0) ? "Appearance — click a picture to set wallpaper (right-click for more)" : "No images yet. Save a drawing in Paint or add images to /Pictures.");
			LocalizedGameText.BindSource(this.header, "cielos_settings", text, null).FitTranslationToRectangle(false);
		}

		// Token: 0x06009E68 RID: 40552 RVA: 0x0041279C File Offset: 0x0041099C
		private void BuildCell(string vpath, string name)
		{
			GameObject gameObject = new GameObject(name, new Type[]
			{
				typeof(RectTransform),
				typeof(Image),
				typeof(Button),
				typeof(CielOSRightClickable)
			});
			gameObject.transform.SetParent(this.gridContainer, false);
			Image component = gameObject.GetComponent<Image>();
			component.color = this.cellColor;
			CielOSTheme.Round(component);
			GameObject gameObject2 = new GameObject("Thumb", new Type[]
			{
				typeof(RectTransform),
				typeof(Image)
			});
			gameObject2.transform.SetParent(gameObject.transform, false);
			RectTransform rectTransform = (RectTransform)gameObject2.transform;
			rectTransform.anchorMin = Vector2.zero;
			rectTransform.anchorMax = Vector2.one;
			rectTransform.offsetMin = new Vector2(4f, 20f);
			rectTransform.offsetMax = new Vector2(-4f, -4f);
			Image component2 = gameObject2.GetComponent<Image>();
			component2.sprite = CielOSWallpaperService.LoadSprite(vpath);
			StreamerCensorImage.Bind(component2, false);
			component2.preserveAspect = true;
			component2.raycastTarget = false;
			component2.color = ((component2.sprite != null) ? Color.white : new Color(0.3f, 0.25f, 0.36f, 1f));
			GameObject gameObject3 = new GameObject("Caption", new Type[]
			{
				typeof(RectTransform),
				typeof(TextMeshProUGUI)
			});
			gameObject3.transform.SetParent(gameObject.transform, false);
			RectTransform rectTransform2 = (RectTransform)gameObject3.transform;
			rectTransform2.anchorMin = new Vector2(0f, 0f);
			rectTransform2.anchorMax = new Vector2(1f, 0f);
			rectTransform2.pivot = new Vector2(0.5f, 0f);
			rectTransform2.anchoredPosition = new Vector2(0f, 2f);
			rectTransform2.sizeDelta = new Vector2(-6f, 16f);
			TextMeshProUGUI component3 = gameObject3.GetComponent<TextMeshProUGUI>();
			component3.text = name;
			component3.fontSize = 10f;
			component3.alignment = TextAlignmentOptions.Center;
			component3.color = new Color(0.8f, 0.78f, 0.85f, 1f);
			component3.raycastTarget = false;
			component3.overflowMode = TextOverflowModes.Ellipsis;
			LocalizedGameText.BindFontOnly(component3);
			gameObject.GetComponent<Button>().onClick.AddListener(delegate
			{
				CielOSWallpaperService instance = CielOSWallpaperService.Instance;
				if (instance == null)
				{
					return;
				}
				instance.ApplyWallpaper(vpath, true);
			});
			Action <>9__2;
			Action <>9__4;
			gameObject.GetComponent<CielOSRightClickable>().OnRightClick = delegate(Vector2 pos)
			{
				if (CielOSContextMenu.Instance == null)
				{
					return;
				}
				CielOSContextMenu instance2 = CielOSContextMenu.Instance;
				Vector2 pos2 = pos;
				List<CielOSContextMenu.Item> list = new List<CielOSContextMenu.Item>();
				string text = "Set as wallpaper";
				Action action;
				if ((action = <>9__2) == null)
				{
					action = (<>9__2 = delegate
					{
						CielOSWallpaperService instance3 = CielOSWallpaperService.Instance;
						if (instance3 == null)
						{
							return;
						}
						instance3.ApplyWallpaper(vpath, true);
					});
				}
				list.Add(new CielOSContextMenu.Item(text, action, true, false, "cielos_settings"));
				list.Add(new CielOSContextMenu.Item("Set as screensaver...", delegate
				{
					CielOSWallpaperService.ShowScreensaverTargetMenu(pos, vpath);
				}, true, false, "cielos_settings"));
				string text2 = "Open folder";
				Action action2;
				if ((action2 = <>9__4) == null)
				{
					action2 = (<>9__4 = delegate
					{
						PlayerMediaExport.OpenDirectory(Path.GetDirectoryName(CielOSFileSystem.Instance.ToReal(vpath)));
					});
				}
				list.Add(new CielOSContextMenu.Item(text2, action2, true, false, "phone_story"));
				instance2.Show(pos2, list);
			};
			this._cells.Add(gameObject);
		}

		// Token: 0x0400A297 RID: 41623
		[SerializeField]
		private TMP_Text header;

		// Token: 0x0400A298 RID: 41624
		[SerializeField]
		private RectTransform gridContainer;

		// Token: 0x0400A299 RID: 41625
		[SerializeField]
		private Button refreshButton;

		// Token: 0x0400A29A RID: 41626
		[Header("Style")]
		[SerializeField]
		private Color cellColor = new Color(0.16f, 0.14f, 0.2f, 1f);

		// Token: 0x0400A29B RID: 41627
		private static readonly string[] ScanDirs = new string[] { "/Pictures", "/Drawings" };

		// Token: 0x0400A29C RID: 41628
		private readonly List<GameObject> _cells = new List<GameObject>();

		// Token: 0x0400A29D RID: 41629
		private int _imageCount;
	}
}
