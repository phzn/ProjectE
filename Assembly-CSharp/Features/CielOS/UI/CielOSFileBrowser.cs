using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Features.Localization;
using ProjectExhibitionist.Core;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Features.CielOS.UI
{
	// Token: 0x020013B4 RID: 5044
	public class CielOSFileBrowser : MonoBehaviour
	{
		// Token: 0x170018D2 RID: 6354
		// (get) Token: 0x06009C12 RID: 39954 RVA: 0x003FC32B File Offset: 0x003FA52B
		// (set) Token: 0x06009C13 RID: 39955 RVA: 0x003FC332 File Offset: 0x003FA532
		public static CielOSFileBrowser Instance { get; private set; }

		// Token: 0x06009C14 RID: 39956 RVA: 0x003FC33A File Offset: 0x003FA53A
		public static CielOSFileBrowser Ensure(Transform canvas)
		{
			if (CielOSFileBrowser.Instance != null)
			{
				return CielOSFileBrowser.Instance;
			}
			CielOSFileBrowser.Instance = CielOSFileBrowser.Create(canvas);
			return CielOSFileBrowser.Instance;
		}

		// Token: 0x06009C15 RID: 39957 RVA: 0x003FC360 File Offset: 0x003FA560
		public static CielOSFileBrowser Create(Transform parent)
		{
			GameObject gameObject = new GameObject("CielOSFileBrowser", new Type[] { typeof(RectTransform) });
			gameObject.transform.SetParent(parent, false);
			RectTransform rectTransform = (RectTransform)gameObject.transform;
			rectTransform.anchorMin = Vector2.zero;
			rectTransform.anchorMax = Vector2.one;
			rectTransform.offsetMin = Vector2.zero;
			rectTransform.offsetMax = Vector2.zero;
			CielOSFileBrowser cielOSFileBrowser = gameObject.AddComponent<CielOSFileBrowser>();
			cielOSFileBrowser.Build();
			return cielOSFileBrowser;
		}

		// Token: 0x06009C16 RID: 39958 RVA: 0x003FC3D8 File Offset: 0x003FA5D8
		private void Build()
		{
			CielOSTheme active = CielOSTheme.Active;
			Color color = ((active != null) ? active.controlBackground : new Color(0.2f, 0.16f, 0.26f, 1f));
			Color color2 = ((active != null) ? active.accent : new Color(0.66f, 0.38f, 0.79f, 1f));
			Image image = this.NewImage("Dim", base.transform);
			CielOSFileBrowser.Stretch((RectTransform)image.transform);
			image.color = new Color(0f, 0f, 0f, 0.55f);
			image.gameObject.AddComponent<Button>().onClick.AddListener(new UnityAction(this.Cancel));
			Image image2 = this.NewImage("Card", base.transform);
			RectTransform rectTransform = (RectTransform)image2.transform;
			RectTransform rectTransform2 = rectTransform;
			RectTransform rectTransform3 = rectTransform;
			RectTransform rectTransform4 = rectTransform;
			Vector2 vector = new Vector2(0.5f, 0.5f);
			rectTransform4.pivot = vector;
			rectTransform2.anchorMin = (rectTransform3.anchorMax = vector);
			rectTransform.sizeDelta = new Vector2(620f, 540f);
			image2.color = ((active != null) ? active.windowBackground : new Color(0.12f, 0.1f, 0.15f, 1f));
			CielOSTheme.Round(image2);
			CielOSTheme.AddBorder(rectTransform, null, 1f);
			this._title = this.MakeText(rectTransform, "Title", "Import file", 15f, (active != null) ? active.titleText : Color.white, FontStyles.Bold, TextAlignmentOptions.MidlineLeft);
			LocalizedGameText.BindSource(this._title, "cielos_shell", "Import file", null).FitTranslationToRectangle(false);
			RectTransform rectTransform5 = (RectTransform)this._title.transform;
			rectTransform5.anchorMin = new Vector2(0f, 1f);
			rectTransform5.anchorMax = new Vector2(1f, 1f);
			rectTransform5.pivot = new Vector2(0f, 1f);
			rectTransform5.sizeDelta = new Vector2(-32f, 24f);
			rectTransform5.anchoredPosition = new Vector2(16f, -10f);
			GameObject gameObject = this.MakeIconButton(rectTransform, "ViewToggle", CielOSIconId.Grid, new Action(this.ToggleView), color);
			RectTransform rectTransform6 = (RectTransform)gameObject.transform;
			vector = new Vector2(1f, 1f);
			rectTransform6.pivot = vector;
			rectTransform6.anchorMin = (rectTransform6.anchorMax = vector);
			rectTransform6.sizeDelta = new Vector2(30f, 28f);
			rectTransform6.anchoredPosition = new Vector2(-14f, -8f);
			this._viewToggleIcon = gameObject.transform.Find("Icon").GetComponent<Image>();
			GameObject gameObject2 = this.MakeIconButton(rectTransform, "Up", CielOSIconId.ChevronDown, new Action(this.GoUp), color2);
			RectTransform rectTransform7 = (RectTransform)gameObject2.transform;
			rectTransform7.anchorMin = new Vector2(0f, 1f);
			rectTransform7.anchorMax = new Vector2(0f, 1f);
			rectTransform7.pivot = new Vector2(0f, 1f);
			rectTransform7.sizeDelta = new Vector2(36f, 28f);
			rectTransform7.anchoredPosition = new Vector2(16f, -42f);
			Transform transform = gameObject2.transform.Find("Icon");
			if (transform != null)
			{
				transform.localEulerAngles = new Vector3(0f, 0f, 180f);
			}
			Image image3 = this.NewImage("PathBar", rectTransform);
			RectTransform rectTransform8 = (RectTransform)image3.transform;
			rectTransform8.anchorMin = new Vector2(0f, 1f);
			rectTransform8.anchorMax = new Vector2(1f, 1f);
			rectTransform8.pivot = new Vector2(0f, 1f);
			rectTransform8.anchoredPosition = new Vector2(60f, -42f);
			rectTransform8.offsetMax = new Vector2(-16f, -42f);
			rectTransform8.sizeDelta = new Vector2(rectTransform8.sizeDelta.x, 28f);
			rectTransform8.offsetMin = new Vector2(60f, rectTransform8.offsetMin.y);
			image3.color = color;
			CielOSTheme.Round(image3);
			this._pathLabel = this.MakeText(rectTransform8, "Path", "", 12f, (active != null) ? active.controlText : Color.white, FontStyles.Normal, TextAlignmentOptions.MidlineLeft);
			RectTransform rectTransform9 = (RectTransform)this._pathLabel.transform;
			CielOSFileBrowser.Stretch(rectTransform9);
			rectTransform9.offsetMin = new Vector2(10f, 0f);
			rectTransform9.offsetMax = new Vector2(-10f, 0f);
			this._pathLabel.overflowMode = TextOverflowModes.Ellipsis;
			this._pathLabel.enableWordWrapping = false;
			this._pathLabel.richText = false;
			Image image4 = this.NewImage("Viewport", rectTransform);
			RectTransform rectTransform10 = (RectTransform)image4.transform;
			rectTransform10.anchorMin = Vector2.zero;
			rectTransform10.anchorMax = Vector2.one;
			rectTransform10.offsetMin = new Vector2(16f, 60f);
			rectTransform10.offsetMax = new Vector2(-16f, -78f);
			image4.color = new Color(0f, 0f, 0f, 0.18f);
			CielOSTheme.Round(image4);
			image4.gameObject.AddComponent<RectMask2D>();
			ScrollRect scrollRect = image4.gameObject.AddComponent<ScrollRect>();
			scrollRect.horizontal = false;
			scrollRect.vertical = true;
			scrollRect.movementType = ScrollRect.MovementType.Clamped;
			this._listContainer = this.NewRect("ListContent", rectTransform10);
			CielOSFileBrowser.SetupScrollContent(this._listContainer);
			VerticalLayoutGroup verticalLayoutGroup = this._listContainer.gameObject.AddComponent<VerticalLayoutGroup>();
			verticalLayoutGroup.spacing = 3f;
			verticalLayoutGroup.padding = new RectOffset(6, 6, 6, 6);
			verticalLayoutGroup.childControlWidth = true;
			verticalLayoutGroup.childControlHeight = true;
			verticalLayoutGroup.childForceExpandWidth = true;
			verticalLayoutGroup.childForceExpandHeight = false;
			this._listContainer.gameObject.AddComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.PreferredSize;
			this._gridContainer = this.NewRect("GridContent", rectTransform10);
			CielOSFileBrowser.SetupScrollContent(this._gridContainer);
			GridLayoutGroup gridLayoutGroup = this._gridContainer.gameObject.AddComponent<GridLayoutGroup>();
			gridLayoutGroup.cellSize = new Vector2(128f, 116f);
			gridLayoutGroup.spacing = new Vector2(8f, 8f);
			gridLayoutGroup.padding = new RectOffset(8, 8, 8, 8);
			gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
			gridLayoutGroup.constraintCount = 4;
			gridLayoutGroup.childAlignment = TextAnchor.UpperCenter;
			this._gridContainer.gameObject.AddComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.PreferredSize;
			this._gridContainer.gameObject.SetActive(false);
			scrollRect.viewport = rectTransform10;
			scrollRect.content = this._listContainer;
			this._smoothWheel = image4.gameObject.AddComponent<CielOSSmoothWheelScroll>();
			this._smoothWheel.SetWheelStep(0.07f);
			this._smoothWheel.Bind(scrollRect);
			this.MakeButton(rectTransform, "Cancel", new Vector2(0f, 0f), new Vector2(16f, 14f), new Action(this.Cancel), color, 120f, 34f, new Vector2(0f, 0f));
			GameObject gameObject3 = this.MakeButton(rectTransform, "Select", new Vector2(1f, 0f), new Vector2(-16f, 14f), new Action(this.ConfirmSelection), color2, 120f, 34f, new Vector2(1f, 0f));
			this._selectButton = gameObject3.GetComponent<Button>();
			this.SetSelectInteractable(false);
			base.gameObject.SetActive(false);
		}

		// Token: 0x06009C17 RID: 39959 RVA: 0x003FCBBC File Offset: 0x003FADBC
		public void Open(string title, string[] extensions, Action<string> onPick, string startHint = null, string localizationScope = "cielos_shell", bool forceStartHint = false)
		{
			this._folderSelection = false;
			ControllerNavigationScope.Configure(base.gameObject, new Action(this.Cancel), null, 100, false);
			LocalizedGameText.BindComputed(this._title, () => CielOSContentText.Preferred(localizationScope, title)).FitTranslationToRectangle(false);
			this._extensions = extensions;
			this._onPick = onPick;
			this._selectedFile = null;
			this.SetSelectInteractable(false);
			base.gameObject.SetActive(true);
			base.transform.SetAsLastSibling();
			string @string = PlayerPrefs.GetString(CielOSFileBrowser.LastDirectoryKey(extensions), "");
			this.NavigateTo((forceStartHint && Directory.Exists(startHint)) ? startHint : (Directory.Exists(@string) ? @string : (Directory.Exists(startHint) ? startHint : CielOSFileBrowser.GetStartFolder(extensions))));
		}

		// Token: 0x06009C18 RID: 39960 RVA: 0x003FCC98 File Offset: 0x003FAE98
		public void OpenFolderSelection(string title, Action<string> onPick, string startHint = null, string localizationScope = "cielos_shell", bool forceStartHint = false)
		{
			this._folderSelection = true;
			this._extensions = null;
			this._onPick = onPick;
			this._selectedFile = null;
			this._lastClickedPath = null;
			LocalizedGameText.BindComputed(this._title, () => CielOSContentText.Preferred(localizationScope, title)).FitTranslationToRectangle(false);
			base.gameObject.SetActive(true);
			base.transform.SetAsLastSibling();
			string @string = PlayerPrefs.GetString("CielOS.Import.LastDirectory.Folder", "");
			this.NavigateTo((forceStartHint && Directory.Exists(startHint)) ? startHint : (Directory.Exists(@string) ? @string : (Directory.Exists(startHint) ? startHint : CielOSFileBrowser.GetStartFolder(null))));
			ControllerNavigationScope.Configure(base.gameObject, new Action(this.Cancel), (this._selectButton != null) ? this._selectButton.gameObject : null, 130, false);
		}

		// Token: 0x06009C19 RID: 39961 RVA: 0x003FCD8C File Offset: 0x003FAF8C
		private static string LastDirectoryKey(string[] extensions)
		{
			if (extensions != null)
			{
				if (extensions.Any<string>((string e) => e == ".mp3" || e == ".wav" || e == ".ogg"))
				{
					return "CielOS.Import.LastDirectory.Audio";
				}
			}
			if (extensions != null)
			{
				if (extensions.Any<string>((string e) => e == ".mp4" || e == ".mov" || e == ".avi" || e == ".webm" || e == ".m4v"))
				{
					return "CielOS.Import.LastDirectory.Video";
				}
			}
			if (extensions != null)
			{
				if (extensions.Any<string>((string e) => e == ".png" || e == ".jpg" || e == ".jpeg"))
				{
					return "CielOS.Import.LastDirectory.Image";
				}
			}
			return "CielOS.Import.LastDirectory.Other";
		}

		// Token: 0x06009C1A RID: 39962 RVA: 0x003FCE30 File Offset: 0x003FB030
		public void RememberSuccessfulDirectory(string realPath)
		{
			if (string.IsNullOrEmpty(realPath))
			{
				return;
			}
			string text = (this._folderSelection ? realPath : Path.GetDirectoryName(realPath));
			if (string.IsNullOrEmpty(text) || !Directory.Exists(text))
			{
				return;
			}
			PlayerPrefs.SetString(this._folderSelection ? "CielOS.Import.LastDirectory.Folder" : CielOSFileBrowser.LastDirectoryKey(this._extensions), text);
			PlayerPrefs.Save();
		}

		// Token: 0x06009C1B RID: 39963 RVA: 0x003FCE90 File Offset: 0x003FB090
		private static string GetStartFolder(string[] ext)
		{
			bool flag;
			if (ext != null)
			{
				flag = ext.Any<string>((string e) => e == ".mp3" || e == ".wav" || e == ".ogg");
			}
			else
			{
				flag = false;
			}
			bool flag2;
			if (ext != null)
			{
				flag2 = ext.Any<string>((string e) => e == ".png" || e == ".jpg" || e == ".jpeg");
			}
			else
			{
				flag2 = false;
			}
			bool flag3 = flag2;
			string folderPath = Environment.GetFolderPath(flag ? Environment.SpecialFolder.MyMusic : (flag3 ? Environment.SpecialFolder.MyPictures : Environment.SpecialFolder.MyDocuments));
			if (!string.IsNullOrEmpty(folderPath) && Directory.Exists(folderPath))
			{
				return folderPath;
			}
			string folderPath2 = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
			if (string.IsNullOrEmpty(folderPath2) || !Directory.Exists(folderPath2))
			{
				return Directory.GetCurrentDirectory();
			}
			return folderPath2;
		}

		// Token: 0x06009C1C RID: 39964 RVA: 0x003FCF3C File Offset: 0x003FB13C
		private void NavigateTo(string folder)
		{
			if (string.IsNullOrEmpty(folder) || !Directory.Exists(folder))
			{
				folder = CielOSFileBrowser.GetStartFolder(this._extensions);
			}
			this._current = folder;
			CielOSSmoothWheelScroll smoothWheel = this._smoothWheel;
			if (smoothWheel != null)
			{
				smoothWheel.ResetTarget();
			}
			this._selectedFile = null;
			this._lastClickedPath = null;
			this.SetSelectInteractable(this._folderSelection && Directory.Exists(folder));
			if (this._pathLabel != null)
			{
				this._pathLabel.text = folder;
			}
			this.ClearRows();
			RectTransform rectTransform = ((this._mode == CielOSFileBrowser.ViewMode.List) ? this._listContainer : this._gridContainer);
			if (this._mode == CielOSFileBrowser.ViewMode.List)
			{
				this.AddShortcuts();
				this.AddDriveShortcuts();
			}
			try
			{
				foreach (string text in from d in Directory.GetDirectories(folder)
					orderby d
					select d)
				{
					this.AddEntry(Path.GetFileName(text), true, text, rectTransform);
				}
				if (!this._folderSelection)
				{
					foreach (string text2 in from f in Directory.GetFiles(folder)
						orderby f
						select f)
					{
						if (this._extensions != null && this._extensions.Length != 0)
						{
							string text3 = Path.GetExtension(text2).ToLowerInvariant();
							if (!this._extensions.Contains(text3))
							{
								continue;
							}
						}
						this.AddEntry(Path.GetFileName(text2), false, text2, rectTransform);
					}
				}
			}
			catch (Exception ex)
			{
				this.AddInfoRow("⚠ " + ex.Message);
			}
		}

		// Token: 0x06009C1D RID: 39965 RVA: 0x003FD124 File Offset: 0x003FB324
		private void AddShortcuts()
		{
			foreach (ValueTuple<string, Environment.SpecialFolder> valueTuple in new ValueTuple<string, Environment.SpecialFolder>[]
			{
				new ValueTuple<string, Environment.SpecialFolder>("Pictures", Environment.SpecialFolder.MyPictures),
				new ValueTuple<string, Environment.SpecialFolder>("Music", Environment.SpecialFolder.MyMusic),
				new ValueTuple<string, Environment.SpecialFolder>("Documents", Environment.SpecialFolder.MyDocuments),
				new ValueTuple<string, Environment.SpecialFolder>("Desktop", Environment.SpecialFolder.Desktop)
			})
			{
				string item = valueTuple.Item1;
				string folderPath = Environment.GetFolderPath(valueTuple.Item2);
				if (!string.IsNullOrEmpty(folderPath) && Directory.Exists(folderPath))
				{
					this.AddListRow("★ " + item, true, folderPath, this._listContainer, item);
				}
			}
		}

		// Token: 0x06009C1E RID: 39966 RVA: 0x003FD1D4 File Offset: 0x003FB3D4
		private void AddDriveShortcuts()
		{
			DriveInfo[] drives;
			try
			{
				drives = DriveInfo.GetDrives();
			}
			catch (Exception ex)
			{
				Debug.LogWarning("[CielOSFileBrowser] Drive list unavailable: " + ex.Message);
				return;
			}
			foreach (DriveInfo driveInfo in drives)
			{
				try
				{
					string fullName = driveInfo.RootDirectory.FullName;
					if (driveInfo.IsReady && Directory.Exists(fullName))
					{
						this.AddListRow(fullName, true, fullName, this._listContainer, null);
					}
				}
				catch (IOException)
				{
				}
				catch (UnauthorizedAccessException)
				{
				}
			}
		}

		// Token: 0x06009C1F RID: 39967 RVA: 0x003FD278 File Offset: 0x003FB478
		private void AddEntry(string label, bool isDir, string fullPath, RectTransform parent)
		{
			if (this._mode == CielOSFileBrowser.ViewMode.Grid)
			{
				this.AddGridCell(label, isDir, fullPath, parent);
				return;
			}
			this.AddListRow(label, isDir, fullPath, parent, null);
		}

		// Token: 0x06009C20 RID: 39968 RVA: 0x003FD29C File Offset: 0x003FB49C
		private void AddListRow(string label, bool isDir, string fullPath, RectTransform parent, string authoredShortcut = null)
		{
			CielOSTheme active = CielOSTheme.Active;
			GameObject gameObject = new GameObject(label, new Type[]
			{
				typeof(RectTransform),
				typeof(Image),
				typeof(Button),
				typeof(LayoutElement)
			});
			gameObject.transform.SetParent(parent, false);
			LayoutElement component = gameObject.GetComponent<LayoutElement>();
			component.minHeight = 30f;
			component.preferredHeight = 30f;
			component.flexibleHeight = 0f;
			Image img = gameObject.GetComponent<Image>();
			img.color = ((active != null) ? active.controlBackground : new Color(0.18f, 0.14f, 0.23f, 1f));
			CielOSTheme.Round(img);
			if (!isDir)
			{
				this._rowHighlight[fullPath] = img;
			}
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
			rectTransform.sizeDelta = new Vector2(16f, 16f);
			rectTransform.anchoredPosition = new Vector2(10f, 0f);
			CielOSIcon.Apply(gameObject2.GetComponent<Image>(), this.IconFor(isDir, fullPath), (active != null) ? active.logoAccent : Color.white, 64);
			TMP_Text tmp_Text = this.MakeText(gameObject.transform, "L", label, 13f, (active != null) ? active.controlText : Color.white, FontStyles.Normal, TextAlignmentOptions.MidlineLeft);
			if (authoredShortcut != null)
			{
				LocalizedGameText.BindSource(tmp_Text, "cielos_shell", authoredShortcut, (string value) => "★ " + value);
			}
			else
			{
				tmp_Text.richText = false;
			}
			RectTransform rectTransform2 = (RectTransform)tmp_Text.transform;
			CielOSFileBrowser.Stretch(rectTransform2);
			rectTransform2.offsetMin = new Vector2(34f, 0f);
			rectTransform2.offsetMax = new Vector2(-8f, 0f);
			tmp_Text.enableWordWrapping = false;
			tmp_Text.overflowMode = TextOverflowModes.Ellipsis;
			gameObject.GetComponent<Button>().onClick.AddListener(delegate
			{
				this.OnEntryClicked(isDir, fullPath, img);
			});
			this._rows.Add(gameObject);
		}

		// Token: 0x06009C21 RID: 39969 RVA: 0x003FD56C File Offset: 0x003FB76C
		private void AddGridCell(string label, bool isDir, string fullPath, RectTransform parent)
		{
			CielOSTheme active = CielOSTheme.Active;
			GameObject gameObject = new GameObject(label, new Type[]
			{
				typeof(RectTransform),
				typeof(Image),
				typeof(Button)
			});
			gameObject.transform.SetParent(parent, false);
			Image img = gameObject.GetComponent<Image>();
			img.color = ((active != null) ? active.controlBackground : new Color(0.18f, 0.14f, 0.23f, 1f));
			CielOSTheme.Round(img);
			if (!isDir)
			{
				this._rowHighlight[fullPath] = img;
			}
			GameObject gameObject2 = new GameObject("Thumb", new Type[]
			{
				typeof(RectTransform),
				typeof(Image)
			});
			gameObject2.transform.SetParent(gameObject.transform, false);
			RectTransform rectTransform = (RectTransform)gameObject2.transform;
			rectTransform.anchorMin = new Vector2(0f, 0f);
			rectTransform.anchorMax = new Vector2(1f, 1f);
			rectTransform.offsetMin = new Vector2(8f, 22f);
			rectTransform.offsetMax = new Vector2(-8f, -8f);
			Image component = gameObject2.GetComponent<Image>();
			component.raycastTarget = false;
			component.preserveAspect = true;
			Sprite sprite = (isDir ? null : this.TryLoadThumbnail(fullPath));
			if (sprite != null)
			{
				component.sprite = sprite;
				component.color = Color.white;
			}
			else
			{
				CielOSIcon.Apply(component, this.IconFor(isDir, fullPath), (active != null) ? active.logoAccent : Color.white, 96);
			}
			TMP_Text tmp_Text = this.MakeText(gameObject.transform, "Cap", label, 10f, (active != null) ? active.controlText : Color.white, FontStyles.Normal, TextAlignmentOptions.Center);
			RectTransform rectTransform2 = (RectTransform)tmp_Text.transform;
			rectTransform2.anchorMin = new Vector2(0f, 0f);
			rectTransform2.anchorMax = new Vector2(1f, 0f);
			rectTransform2.pivot = new Vector2(0.5f, 0f);
			rectTransform2.sizeDelta = new Vector2(-6f, 18f);
			rectTransform2.anchoredPosition = new Vector2(0f, 3f);
			tmp_Text.enableWordWrapping = false;
			tmp_Text.overflowMode = TextOverflowModes.Ellipsis;
			tmp_Text.richText = false;
			gameObject.GetComponent<Button>().onClick.AddListener(delegate
			{
				this.OnEntryClicked(isDir, fullPath, img);
			});
			this._rows.Add(gameObject);
		}

		// Token: 0x06009C22 RID: 39970 RVA: 0x003FD840 File Offset: 0x003FBA40
		private void OnEntryClicked(bool isDir, string fullPath, Image rowImg)
		{
			float unscaledTime = Time.unscaledTime;
			bool flag = this._lastClickedPath == fullPath && unscaledTime - this._lastClickTime <= 0.35f;
			this._lastClickTime = unscaledTime;
			this._lastClickedPath = fullPath;
			if (isDir)
			{
				this.NavigateTo(fullPath);
				return;
			}
			if (this._folderSelection)
			{
				return;
			}
			this.Select(fullPath);
			if (flag)
			{
				this.ConfirmSelection();
			}
		}

		// Token: 0x06009C23 RID: 39971 RVA: 0x003FD8AC File Offset: 0x003FBAAC
		private void Select(string fullPath)
		{
			this._selectedFile = fullPath;
			this.SetSelectInteractable(true);
			CielOSTheme active = CielOSTheme.Active;
			Color color = ((active != null) ? active.accent : new Color(0.66f, 0.38f, 0.79f, 1f));
			Color color2 = ((active != null) ? active.controlBackground : new Color(0.18f, 0.14f, 0.23f, 1f));
			foreach (KeyValuePair<string, Image> keyValuePair in this._rowHighlight)
			{
				if (keyValuePair.Value != null)
				{
					keyValuePair.Value.color = ((keyValuePair.Key == fullPath) ? color : color2);
				}
			}
		}

		// Token: 0x06009C24 RID: 39972 RVA: 0x003FD994 File Offset: 0x003FBB94
		private void ConfirmSelection()
		{
			if (this._folderSelection)
			{
				if (!string.IsNullOrEmpty(this._current) && Directory.Exists(this._current))
				{
					this.Pick(Path.GetFullPath(this._current));
				}
				return;
			}
			if (string.IsNullOrEmpty(this._selectedFile))
			{
				return;
			}
			this.Pick(this._selectedFile);
		}

		// Token: 0x06009C25 RID: 39973 RVA: 0x003FD9F0 File Offset: 0x003FBBF0
		private Sprite TryLoadThumbnail(string realPath)
		{
			if (this._folderSelection)
			{
				return null;
			}
			string text = Path.GetExtension(realPath).ToLowerInvariant();
			if (text != ".png" && text != ".jpg" && text != ".jpeg" && text != ".bmp")
			{
				return null;
			}
			Texture2D texture2D = null;
			Sprite sprite = null;
			bool flag = false;
			Sprite sprite2;
			try
			{
				byte[] array = File.ReadAllBytes(realPath);
				texture2D = new Texture2D(2, 2)
				{
					filterMode = FilterMode.Bilinear
				};
				if (!texture2D.LoadImage(array))
				{
					sprite2 = null;
				}
				else
				{
					sprite = Sprite.Create(texture2D, new Rect(0f, 0f, (float)texture2D.width, (float)texture2D.height), new Vector2(0.5f, 0.5f), 100f);
					this._ownedThumbnails.Add(new CielOSFileBrowser.OwnedThumbnail
					{
						sprite = sprite,
						texture = texture2D
					});
					flag = true;
					sprite2 = sprite;
				}
			}
			catch
			{
				sprite2 = null;
			}
			finally
			{
				if (!flag)
				{
					CielOSFileBrowser.DestroyGeneratedObject(sprite);
					CielOSFileBrowser.DestroyGeneratedObject(texture2D);
				}
			}
			return sprite2;
		}

		// Token: 0x06009C26 RID: 39974 RVA: 0x003FDB10 File Offset: 0x003FBD10
		private void ClearRows()
		{
			foreach (GameObject gameObject in this._rows)
			{
				if (!(gameObject == null))
				{
					gameObject.SetActive(false);
					CielOSFileBrowser.DestroyGeneratedObject(gameObject);
				}
			}
			this._rows.Clear();
			this._rowHighlight.Clear();
			foreach (CielOSFileBrowser.OwnedThumbnail ownedThumbnail in this._ownedThumbnails)
			{
				CielOSFileBrowser.DestroyGeneratedObject(ownedThumbnail.sprite);
				CielOSFileBrowser.DestroyGeneratedObject(ownedThumbnail.texture);
			}
			this._ownedThumbnails.Clear();
		}

		// Token: 0x06009C27 RID: 39975 RVA: 0x003FDBE4 File Offset: 0x003FBDE4
		private void OnDisable()
		{
			if (!base.gameObject.activeSelf)
			{
				this.ClearRows();
			}
		}

		// Token: 0x06009C28 RID: 39976 RVA: 0x000A4FF8 File Offset: 0x000A31F8
		private static void DestroyGeneratedObject(Object value)
		{
			if (value == null)
			{
				return;
			}
			if (Application.isPlaying)
			{
				Object.Destroy(value);
				return;
			}
			Object.DestroyImmediate(value);
		}

		// Token: 0x06009C29 RID: 39977 RVA: 0x003FDBFC File Offset: 0x003FBDFC
		private CielOSIconId IconFor(bool isDir, string fullPath)
		{
			if (isDir)
			{
				return CielOSIconId.Folder;
			}
			string text = Path.GetExtension(fullPath).ToLowerInvariant();
			if (text == ".mp3" || text == ".wav" || text == ".ogg")
			{
				return CielOSIconId.Music;
			}
			if (text == ".png" || text == ".jpg" || text == ".jpeg" || text == ".bmp")
			{
				return CielOSIconId.Paint;
			}
			return CielOSIconId.Files;
		}

		// Token: 0x06009C2A RID: 39978 RVA: 0x003FDC7C File Offset: 0x003FBE7C
		private void ToggleView()
		{
			this._mode = ((this._mode == CielOSFileBrowser.ViewMode.List) ? CielOSFileBrowser.ViewMode.Grid : CielOSFileBrowser.ViewMode.List);
			this._listContainer.gameObject.SetActive(this._mode == CielOSFileBrowser.ViewMode.List);
			this._gridContainer.gameObject.SetActive(this._mode == CielOSFileBrowser.ViewMode.Grid);
			ScrollRect componentInParent = this._listContainer.GetComponentInParent<ScrollRect>();
			if (componentInParent != null)
			{
				componentInParent.content = ((this._mode == CielOSFileBrowser.ViewMode.List) ? this._listContainer : this._gridContainer);
			}
			CielOSSmoothWheelScroll smoothWheel = this._smoothWheel;
			if (smoothWheel != null)
			{
				smoothWheel.ResetTarget();
			}
			if (this._viewToggleIcon != null)
			{
				CielOSIcon.Apply(this._viewToggleIcon, (this._mode == CielOSFileBrowser.ViewMode.List) ? CielOSIconId.Grid : CielOSIconId.Menu, Color.white, 64);
			}
			this.NavigateTo(this._current);
		}

		// Token: 0x06009C2B RID: 39979 RVA: 0x003FDD48 File Offset: 0x003FBF48
		private void AddInfoRow(string text)
		{
			GameObject gameObject = new GameObject("info", new Type[]
			{
				typeof(RectTransform),
				typeof(LayoutElement)
			});
			gameObject.transform.SetParent((this._mode == CielOSFileBrowser.ViewMode.List) ? this._listContainer : this._gridContainer, false);
			LayoutElement component = gameObject.GetComponent<LayoutElement>();
			component.minHeight = 28f;
			component.preferredHeight = 28f;
			CielOSFileBrowser.Stretch((RectTransform)this.MakeText(gameObject.transform, "L", text, 12f, Color.gray, FontStyles.Italic, TextAlignmentOptions.MidlineLeft).transform);
			this._rows.Add(gameObject);
		}

		// Token: 0x06009C2C RID: 39980 RVA: 0x003FDDFC File Offset: 0x003FBFFC
		private void GoUp()
		{
			try
			{
				DirectoryInfo parent = Directory.GetParent(this._current);
				if (parent != null)
				{
					this.NavigateTo(parent.FullName);
				}
			}
			catch
			{
			}
		}

		// Token: 0x06009C2D RID: 39981 RVA: 0x003FDE3C File Offset: 0x003FC03C
		private void Pick(string realPath)
		{
			Action<string> onPick = this._onPick;
			this._onPick = null;
			base.gameObject.SetActive(false);
			if (onPick == null)
			{
				return;
			}
			onPick(realPath);
		}

		// Token: 0x06009C2E RID: 39982 RVA: 0x003FDE62 File Offset: 0x003FC062
		public void Cancel()
		{
			object obj = (this._folderSelection ? this._onPick : null);
			this._onPick = null;
			base.gameObject.SetActive(false);
			object obj2 = obj;
			if (obj2 == null)
			{
				return;
			}
			obj2(null);
		}

		// Token: 0x06009C2F RID: 39983 RVA: 0x003FDE93 File Offset: 0x003FC093
		private void OnDestroy()
		{
			this._onPick = null;
			this.ClearRows();
			if (CielOSFileBrowser.Instance == this)
			{
				CielOSFileBrowser.Instance = null;
			}
		}

		// Token: 0x06009C30 RID: 39984 RVA: 0x003FDEB8 File Offset: 0x003FC0B8
		private void SetSelectInteractable(bool on)
		{
			if (this._selectButton == null)
			{
				return;
			}
			this._selectButton.interactable = on;
			Image component = this._selectButton.GetComponent<Image>();
			if (component != null)
			{
				Color color = component.color;
				color.a = (on ? 1f : 0.4f);
				component.color = color;
			}
		}

		// Token: 0x06009C31 RID: 39985 RVA: 0x003FDF19 File Offset: 0x003FC119
		private RectTransform NewRect(string name, Transform parent)
		{
			GameObject gameObject = new GameObject(name, new Type[] { typeof(RectTransform) });
			gameObject.transform.SetParent(parent, false);
			return (RectTransform)gameObject.transform;
		}

		// Token: 0x06009C32 RID: 39986 RVA: 0x003FDF4C File Offset: 0x003FC14C
		private static void SetupScrollContent(RectTransform rt)
		{
			rt.anchorMin = new Vector2(0f, 1f);
			rt.anchorMax = new Vector2(1f, 1f);
			rt.pivot = new Vector2(0.5f, 1f);
			rt.offsetMin = new Vector2(0f, rt.offsetMin.y);
			rt.offsetMax = new Vector2(0f, rt.offsetMax.y);
			rt.anchoredPosition = new Vector2(0f, 0f);
		}

		// Token: 0x06009C33 RID: 39987 RVA: 0x003FDFE3 File Offset: 0x003FC1E3
		private Image NewImage(string name, Transform parent)
		{
			GameObject gameObject = new GameObject(name, new Type[]
			{
				typeof(RectTransform),
				typeof(Image)
			});
			gameObject.transform.SetParent(parent, false);
			return gameObject.GetComponent<Image>();
		}

		// Token: 0x06009C34 RID: 39988 RVA: 0x001A38AD File Offset: 0x001A1AAD
		private static void Stretch(RectTransform rt)
		{
			rt.anchorMin = Vector2.zero;
			rt.anchorMax = Vector2.one;
			rt.offsetMin = Vector2.zero;
			rt.offsetMax = Vector2.zero;
		}

		// Token: 0x06009C35 RID: 39989 RVA: 0x003FE020 File Offset: 0x003FC220
		private TMP_Text MakeText(Transform parent, string name, string content, float size, Color color, FontStyles style, TextAlignmentOptions align)
		{
			GameObject gameObject = new GameObject(name, new Type[]
			{
				typeof(RectTransform),
				typeof(TextMeshProUGUI)
			});
			gameObject.transform.SetParent(parent, false);
			TextMeshProUGUI component = gameObject.GetComponent<TextMeshProUGUI>();
			component.text = content;
			component.fontSize = size;
			component.color = color;
			component.fontStyle = style;
			component.alignment = align;
			component.raycastTarget = false;
			if (CielOSTheme.Active != null && CielOSTheme.Active.uiFont != null)
			{
				component.font = CielOSTheme.Active.uiFont;
			}
			LocalizedGameText.BindFontOnly(component);
			return component;
		}

		// Token: 0x06009C36 RID: 39990 RVA: 0x003FE0CC File Offset: 0x003FC2CC
		private GameObject MakeButton(Transform parent, string label, Vector2 anchor, Vector2 pos, Action onClick, Color color, float w, float h, Vector2 pivot)
		{
			GameObject gameObject = new GameObject(label, new Type[]
			{
				typeof(RectTransform),
				typeof(Image),
				typeof(Button)
			});
			gameObject.transform.SetParent(parent, false);
			RectTransform rectTransform = (RectTransform)gameObject.transform;
			rectTransform.anchorMax = anchor;
			rectTransform.anchorMin = anchor;
			rectTransform.pivot = pivot;
			rectTransform.sizeDelta = new Vector2(w, h);
			rectTransform.anchoredPosition = pos;
			Image component = gameObject.GetComponent<Image>();
			component.color = color;
			CielOSTheme.Round(component);
			gameObject.GetComponent<Button>().onClick.AddListener(delegate
			{
				onClick();
			});
			TMP_Text tmp_Text = this.MakeText(gameObject.transform, "L", label, 13f, Color.white, FontStyles.Bold, TextAlignmentOptions.Center);
			LocalizedGameText.BindSource(tmp_Text, "cielos_shell", label, null).FitTranslationToRectangle(false);
			CielOSFileBrowser.Stretch((RectTransform)tmp_Text.transform);
			return gameObject;
		}

		// Token: 0x06009C37 RID: 39991 RVA: 0x003FE1D8 File Offset: 0x003FC3D8
		private GameObject MakeIconButton(Transform parent, string name, CielOSIconId icon, Action onClick, Color color)
		{
			GameObject gameObject = new GameObject(name, new Type[]
			{
				typeof(RectTransform),
				typeof(Image),
				typeof(Button)
			});
			gameObject.transform.SetParent(parent, false);
			Image component = gameObject.GetComponent<Image>();
			component.color = color;
			CielOSTheme.Round(component);
			gameObject.GetComponent<Button>().onClick.AddListener(delegate
			{
				onClick();
			});
			GameObject gameObject2 = new GameObject("Icon", new Type[]
			{
				typeof(RectTransform),
				typeof(Image)
			});
			gameObject2.transform.SetParent(gameObject.transform, false);
			RectTransform rectTransform = (RectTransform)gameObject2.transform;
			rectTransform.anchorMin = Vector2.zero;
			rectTransform.anchorMax = Vector2.one;
			rectTransform.offsetMin = new Vector2(7f, 7f);
			rectTransform.offsetMax = new Vector2(-7f, -7f);
			CielOSIcon.Apply(gameObject2.GetComponent<Image>(), icon, Color.white, 64);
			return gameObject;
		}

		// Token: 0x0400A09C RID: 41116
		private RectTransform _listContainer;

		// Token: 0x0400A09D RID: 41117
		private RectTransform _gridContainer;

		// Token: 0x0400A09E RID: 41118
		private TMP_Text _pathLabel;

		// Token: 0x0400A09F RID: 41119
		private TMP_Text _title;

		// Token: 0x0400A0A0 RID: 41120
		private Button _selectButton;

		// Token: 0x0400A0A1 RID: 41121
		private Image _viewToggleIcon;

		// Token: 0x0400A0A2 RID: 41122
		private string _current;

		// Token: 0x0400A0A3 RID: 41123
		private string[] _extensions;

		// Token: 0x0400A0A4 RID: 41124
		private bool _folderSelection;

		// Token: 0x0400A0A5 RID: 41125
		private Action<string> _onPick;

		// Token: 0x0400A0A6 RID: 41126
		private CielOSFileBrowser.ViewMode _mode;

		// Token: 0x0400A0A7 RID: 41127
		private string _selectedFile;

		// Token: 0x0400A0A8 RID: 41128
		private CielOSSmoothWheelScroll _smoothWheel;

		// Token: 0x0400A0A9 RID: 41129
		private float _lastClickTime;

		// Token: 0x0400A0AA RID: 41130
		private string _lastClickedPath;

		// Token: 0x0400A0AB RID: 41131
		private readonly List<GameObject> _rows = new List<GameObject>();

		// Token: 0x0400A0AC RID: 41132
		private readonly List<CielOSFileBrowser.OwnedThumbnail> _ownedThumbnails = new List<CielOSFileBrowser.OwnedThumbnail>();

		// Token: 0x0400A0AD RID: 41133
		private readonly Dictionary<string, Image> _rowHighlight = new Dictionary<string, Image>();

		// Token: 0x0400A0AE RID: 41134
		private const string FolderDirectoryKey = "CielOS.Import.LastDirectory.Folder";

		// Token: 0x020013B5 RID: 5045
		private enum ViewMode
		{
			// Token: 0x0400A0B0 RID: 41136
			List,
			// Token: 0x0400A0B1 RID: 41137
			Grid
		}

		// Token: 0x020013B6 RID: 5046
		private struct OwnedThumbnail
		{
			// Token: 0x0400A0B2 RID: 41138
			public Sprite sprite;

			// Token: 0x0400A0B3 RID: 41139
			public Texture2D texture;
		}
	}
}
