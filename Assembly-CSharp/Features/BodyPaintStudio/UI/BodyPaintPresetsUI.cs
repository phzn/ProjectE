using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using Features.CielOS.UI;
using Features.Localization;
using ProjectExhibitionist.Core;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Features.BodyPaintStudio.UI
{
	// Token: 0x0200151A RID: 5402
	public sealed class BodyPaintPresetsUI : MonoBehaviour
	{
		// Token: 0x170019BE RID: 6590
		// (get) Token: 0x0600A700 RID: 42752 RVA: 0x0045CF09 File Offset: 0x0045B109
		public bool IsDialogOpen
		{
			get
			{
				return this._busy || (this._dialog != null && this._dialog.activeSelf) || this._dismissFrame == Time.frameCount;
			}
		}

		// Token: 0x0600A701 RID: 42753 RVA: 0x0045CF40 File Offset: 0x0045B140
		private void Awake()
		{
			if (this.saveButton != null)
			{
				this.saveButton.onClick.AddListener(new UnityAction(this.ShowSaveDialog));
			}
			if (this.libraryButton != null)
			{
				this.libraryButton.onClick.AddListener(new UnityAction(this.ShowLibrary));
			}
			if (this.exportButton != null)
			{
				this.exportButton.onClick.AddListener(new UnityAction(this.ExportCurrent));
			}
			this.BindSectionLabels();
			if (this.section != null)
			{
				this.section.gameObject.SetActive(false);
			}
		}

		// Token: 0x0600A702 RID: 42754 RVA: 0x0045CFF0 File Offset: 0x0045B1F0
		private void BindSectionLabels()
		{
			BodyPaintPresetsUI.BindButton(this.saveButton, "SAVE PRESET");
			BodyPaintPresetsUI.BindButton(this.libraryButton, "MY PRESETS");
			BodyPaintPresetsUI.BindButton(this.exportButton, "EXPORT PNG");
			Transform transform = ((this.section != null) ? this.section.Find("PresetHeading") : null);
			if (transform != null)
			{
				LocalizedGameText localizedGameText = LocalizedGameText.BindSource(transform.GetComponent<TMP_Text>(), "bodypaint_presets", "PRESETS", null);
				if (localizedGameText == null)
				{
					return;
				}
				localizedGameText.FitTranslationToRectangle(true);
			}
		}

		// Token: 0x0600A703 RID: 42755 RVA: 0x0045D079 File Offset: 0x0045B279
		private static void BindButton(Button button, string source)
		{
			if (button != null)
			{
				LocalizedGameText localizedGameText = LocalizedGameText.BindSource(button.GetComponentInChildren<TMP_Text>(true), "bodypaint_presets", source, null);
				if (localizedGameText == null)
				{
					return;
				}
				localizedGameText.FitTranslationToRectangle(true);
			}
		}

		// Token: 0x0600A704 RID: 42756 RVA: 0x0045D0A4 File Offset: 0x0045B2A4
		public void Configure(BodyPaintCanvasService canvas, Camera previewCamera)
		{
			base.StopAllCoroutines();
			this._busy = false;
			if (this._canvas != null)
			{
				this._canvas.OnPaintStateChanged -= this.HandlePaintState;
			}
			this.CloseDialog();
			this._canvas = canvas;
			this.BindSectionLabels();
			if (this._canvas != null)
			{
				this._canvas.OnPaintStateChanged += this.HandlePaintState;
			}
			if (this.section != null)
			{
				this.section.gameObject.SetActive(this._canvas != null);
				RectTransform rectTransform = this.section.parent as RectTransform;
				if (rectTransform != null)
				{
					rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (this._canvas != null) ? 1032f : 900f);
				}
			}
			if (this.statusLabel != null)
			{
				LocalizedGameText.BindSource(this.statusLabel, "bodypaint_presets", string.Empty, null);
			}
			this.RefreshButtons();
		}

		// Token: 0x0600A705 RID: 42757 RVA: 0x0045D1AB File Offset: 0x0045B3AB
		private void OnDisable()
		{
			base.StopAllCoroutines();
			this._busy = false;
			this.CloseDialog();
		}

		// Token: 0x0600A706 RID: 42758 RVA: 0x0045D1C0 File Offset: 0x0045B3C0
		private void OnDestroy()
		{
			if (this._canvas != null)
			{
				this._canvas.OnPaintStateChanged -= this.HandlePaintState;
			}
			this.ReleaseThumbnails();
		}

		// Token: 0x0600A707 RID: 42759 RVA: 0x0045D1ED File Offset: 0x0045B3ED
		private void HandlePaintState(bool hasPaint)
		{
			this.RefreshButtons();
		}

		// Token: 0x0600A708 RID: 42760 RVA: 0x0045D1F8 File Offset: 0x0045B3F8
		private void RefreshButtons()
		{
			bool flag = this._canvas != null && !this._busy;
			if (this.saveButton != null)
			{
				this.saveButton.interactable = flag && this._canvas.HasAnyPaint;
			}
			if (this.exportButton != null)
			{
				this.exportButton.interactable = flag && this._canvas.HasAnyPaint;
			}
			if (this.libraryButton != null)
			{
				this.libraryButton.interactable = flag;
			}
			if (this._importConfirmButton != null)
			{
				this._importConfirmButton.interactable = flag && !string.IsNullOrEmpty(this._importFolder);
			}
		}

		// Token: 0x0600A709 RID: 42761 RVA: 0x0045D2BC File Offset: 0x0045B4BC
		public bool TryCloseDialog()
		{
			if (!this.IsDialogOpen)
			{
				return false;
			}
			if (this._busy || this._dialog == null)
			{
				return true;
			}
			if (this._importBrowser != null && this._importBrowser.gameObject.activeInHierarchy)
			{
				this._importBrowser.Cancel();
				this._dismissFrame = Time.frameCount;
				return true;
			}
			if (this._confirmation || this._importDialog)
			{
				this.ShowLibrary();
			}
			else
			{
				this.CloseDialog();
			}
			return true;
		}

		// Token: 0x0600A70A RID: 42762 RVA: 0x0045D344 File Offset: 0x0045B544
		private void CloseDialog()
		{
			this._dismissFrame = Time.frameCount;
			TMP_InputField nameInput = this._nameInput;
			this._nameInput = null;
			if (nameInput != null)
			{
				nameInput.DeactivateInputField(false);
			}
			this._dialogStatus = null;
			this._confirmation = false;
			this._importDialog = false;
			this._importFolder = null;
			this._importFolderLabel = null;
			this._importConfirmButton = null;
			this._importBrowser = null;
			if (this._dialog != null)
			{
				if (EventSystem.current != null && EventSystem.current.currentSelectedGameObject != null && EventSystem.current.currentSelectedGameObject.transform.IsChildOf(this._dialog.transform))
				{
					EventSystem.current.SetSelectedGameObject(null);
				}
				this._dialog.SetActive(false);
				Object.Destroy(this._dialog);
			}
			this._dialog = null;
			this.ReleaseThumbnails();
		}

		// Token: 0x0600A70B RID: 42763 RVA: 0x0045D428 File Offset: 0x0045B628
		private void ReleaseThumbnails()
		{
			foreach (Texture2D texture2D in this._thumbnails)
			{
				if (texture2D != null)
				{
					Object.Destroy(texture2D);
				}
			}
			this._thumbnails.Clear();
		}

		// Token: 0x0600A70C RID: 42764 RVA: 0x0045D490 File Offset: 0x0045B690
		private void ShowSaveDialog()
		{
			if (this._busy || !this.HasPaintToSave())
			{
				return;
			}
			RectTransform rectTransform = this.CreateDialog("SavePresetDialog", "SAVE PRESET", 680f, 350f);
			this.Label(rectTransform, "SaveHint", "Keep this body paint in your personal library.", 32f, 76f, 616f, 30f, 18f, BodyPaintPresetsUI.Muted, true);
			this.Label(rectTransform, "NameCaption", "PRESET NAME", 32f, 122f, 616f, 24f, 13f, BodyPaintPresetsUI.Muted, true);
			this._nameInput = this.CreateInput(rectTransform, 32f, 152f, 616f, 50f);
			this._nameInput.text = BodyPaintPresetsUI.DefaultName();
			TMP_InputField input = this._nameInput;
			input.onSubmit.AddListener(delegate(string _)
			{
				if (!input.wasCanceled)
				{
					this.SaveCurrent();
				}
			});
			input.onEndEdit.AddListener(delegate(string _)
			{
				if (this._nameInput == input && input.wasCanceled && !this._busy)
				{
					this.CloseDialog();
				}
			});
			this._dialogStatus = this.Label(rectTransform, "DialogStatus", string.Empty, 32f, 214f, 616f, 45f, 15f, BodyPaintPresetsUI.Muted, false);
			this.ActionButton(rectTransform, "Cancel", "CANCEL", 364f, 280f, 134f, 42f, BodyPaintPresetsUI.ButtonColor, new UnityAction(this.CloseDialog));
			this.ActionButton(rectTransform, "Save", "SAVE PRESET", 510f, 280f, 138f, 42f, BodyPaintPresetsUI.Accent, new UnityAction(this.SaveCurrent));
			this._nameInput.ActivateInputField();
			this._nameInput.Select();
			ControllerNavigationScope.Configure(this._dialog, delegate
			{
				this.TryCloseDialog();
			}, this._nameInput.gameObject, 120, false);
		}

		// Token: 0x0600A70D RID: 42765 RVA: 0x0045D690 File Offset: 0x0045B890
		private void SaveCurrent()
		{
			if (this._busy || !this.HasPaintToSave())
			{
				return;
			}
			string text = ((this._nameInput != null) ? this._nameInput.text.Trim() : BodyPaintPresetsUI.DefaultName());
			if (string.IsNullOrWhiteSpace(text))
			{
				this.SetStatus("Choose a name for this preset.", true);
				return;
			}
			this.Begin(BodyPaintPresetsUI.Operation.Save, null, text, null);
		}

		// Token: 0x0600A70E RID: 42766 RVA: 0x0045D6F3 File Offset: 0x0045B8F3
		private void ExportCurrent()
		{
			if (this._busy || !this.HasPaintToSave())
			{
				return;
			}
			this.Begin(BodyPaintPresetsUI.Operation.ExportCurrent, null, BodyPaintPresetsUI.DefaultName(), null);
		}

		// Token: 0x0600A70F RID: 42767 RVA: 0x0045D714 File Offset: 0x0045B914
		private bool HasPaintToSave()
		{
			if (this._canvas == null)
			{
				return false;
			}
			this._canvas.RefreshCoverage();
			if (this._canvas.HasAnyPaint)
			{
				return true;
			}
			this.SetStatus("There is no body paint to save yet.", true);
			this.RefreshButtons();
			return false;
		}

		// Token: 0x0600A710 RID: 42768 RVA: 0x0045D754 File Offset: 0x0045B954
		private void ShowLibrary()
		{
			if (this._busy || this._canvas == null)
			{
				return;
			}
			RectTransform rectTransform = this.CreateDialog("PresetLibraryDialog", "MY PRESETS", 1060f, 760f);
			this.ActionButton(rectTransform, "Import", "IMPORT BODYPAINT", 766f, 28f, 262f, 42f, BodyPaintPresetsUI.Accent, new UnityAction(this.ShowImportDialog));
			rectTransform.Find("DialogTitle").GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 710f);
			this.Label(rectTransform, "LibraryHint", "Load a look, export it, or make room for a new one.", 32f, 76f, 996f, 32f, 18f, BodyPaintPresetsUI.Muted, true);
			this._dialogStatus = this.Label(rectTransform, "DialogStatus", string.Empty, 32f, 680f, 796f, 56f, 15f, BodyPaintPresetsUI.Muted, false);
			this.ActionButton(rectTransform, "Close", "CLOSE", 858f, 686f, 170f, 42f, BodyPaintPresetsUI.ButtonColor, new UnityAction(this.CloseDialog));
			List<BodyPaintPresetInfo> list;
			try
			{
				list = BodyPaintPresetStore.ListPresets(null);
			}
			catch (Exception ex)
			{
				this.ReportFailure("Could not open your presets.", ex);
				return;
			}
			if (list.Count == 0)
			{
				this.Label(rectTransform, "EmptyTitle", "YOUR FIRST LOOK STARTS HERE", 80f, 275f, 900f, 48f, 25f, Color.white, true).alignment = TextAlignmentOptions.Center;
				this.Label(rectTransform, "EmptyHint", "Paint something you love, then choose Save Preset.", 80f, 333f, 900f, 60f, 19f, BodyPaintPresetsUI.Muted, true).alignment = TextAlignmentOptions.Center;
				return;
			}
			RectTransform rectTransform2 = BodyPaintPresetsUI.Rect("PresetViewport", rectTransform);
			BodyPaintPresetsUI.TopLeft(rectTransform2, 32f, 130f, 974f, 526f);
			rectTransform2.gameObject.AddComponent<RectMask2D>();
			rectTransform2.gameObject.AddComponent<Image>().color = new Color(0f, 0f, 0f, 0.12f);
			RectTransform rectTransform3 = BodyPaintPresetsUI.Rect("Presets", rectTransform2);
			rectTransform3.anchorMin = new Vector2(0f, 1f);
			rectTransform3.anchorMax = Vector2.one;
			rectTransform3.pivot = new Vector2(0.5f, 1f);
			rectTransform3.anchoredPosition = Vector2.zero;
			rectTransform3.sizeDelta = new Vector2(0f, (float)list.Count * 158f - 12f);
			for (int i = 0; i < list.Count; i++)
			{
				this.BuildPresetRow(rectTransform3, list[i], (float)i * 158f);
			}
			ScrollRect scrollRect = rectTransform2.gameObject.AddComponent<ScrollRect>();
			scrollRect.viewport = rectTransform2;
			scrollRect.content = rectTransform3;
			scrollRect.horizontal = false;
			scrollRect.vertical = true;
			scrollRect.movementType = ScrollRect.MovementType.Clamped;
			scrollRect.scrollSensitivity = 32f;
			RectTransform rectTransform4 = BodyPaintPresetsUI.Rect("ScrollTrack", rectTransform);
			BodyPaintPresetsUI.TopLeft(rectTransform4, 1016f, 130f, 12f, 526f);
			rectTransform4.gameObject.AddComponent<Image>().color = BodyPaintPresetsUI.Card;
			RectTransform rectTransform5 = BodyPaintPresetsUI.Rect("HandleArea", rectTransform4);
			BodyPaintPresetsUI.Stretch(rectTransform5, 2f);
			RectTransform rectTransform6 = BodyPaintPresetsUI.Rect("Handle", rectTransform5);
			BodyPaintPresetsUI.Stretch(rectTransform6, 0f);
			Image image = rectTransform6.gameObject.AddComponent<Image>();
			image.color = BodyPaintPresetsUI.Muted;
			Scrollbar scrollbar = rectTransform4.gameObject.AddComponent<Scrollbar>();
			scrollbar.handleRect = rectTransform6;
			scrollbar.targetGraphic = image;
			scrollbar.direction = Scrollbar.Direction.BottomToTop;
			scrollRect.verticalScrollbar = scrollbar;
			scrollRect.verticalNormalizedPosition = 1f;
		}

		// Token: 0x0600A711 RID: 42769 RVA: 0x0045DB24 File Offset: 0x0045BD24
		private void ShowImportDialog()
		{
			if (this._busy || this._canvas == null)
			{
				return;
			}
			RectTransform rectTransform = this.CreateDialog("ImportPresetDialog", "IMPORT BODYPAINT", 740f, 530f);
			this._importDialog = true;
			this.Label(rectTransform, "ImportHint", "Choose the folder containing body.png and head.png.", 32f, 78f, 676f, 40f, 18f, BodyPaintPresetsUI.Muted, true);
			this.Label(rectTransform, "FolderCaption", "FOLDER", 32f, 126f, 676f, 24f, 13f, BodyPaintPresetsUI.Muted, true);
			RectTransform rectTransform2 = BodyPaintPresetsUI.Rect("FolderPath", rectTransform);
			BodyPaintPresetsUI.TopLeft(rectTransform2, 32f, 155f, 456f, 42f);
			rectTransform2.gameObject.AddComponent<Image>().color = BodyPaintPresetsUI.Card;
			this._importFolderLabel = this.Label(rectTransform2, "FolderValue", "No folder selected.", 12f, 4f, 432f, 34f, 16f, BodyPaintPresetsUI.Muted, true);
			this._importFolderLabel.enableWordWrapping = false;
			this.ActionButton(rectTransform, "ChooseFolder", "CHOOSE FOLDER", 500f, 155f, 208f, 42f, BodyPaintPresetsUI.ButtonColor, new UnityAction(this.ChooseImportFolder));
			this.Label(rectTransform, "NameCaption", "PRESET NAME", 32f, 214f, 676f, 24f, 13f, BodyPaintPresetsUI.Muted, true);
			this._nameInput = this.CreateInput(rectTransform, 32f, 244f, 676f, 50f);
			this._suggestedImportName = GameLocalization.GetSource("bodypaint_presets", "Imported body paint");
			this._nameInput.text = this._suggestedImportName;
			TMP_InputField input = this._nameInput;
			input.onSubmit.AddListener(delegate(string _)
			{
				if (!input.wasCanceled)
				{
					this.ImportSelectedFolder();
				}
			});
			this.Label(rectTransform, "ImportExplanation", "Import adds a new preset to your library. Your current paint stays as it is.", 32f, 314f, 676f, 52f, 16f, BodyPaintPresetsUI.Muted, true);
			this._dialogStatus = this.Label(rectTransform, "DialogStatus", string.Empty, 32f, 370f, 676f, 72f, 16f, BodyPaintPresetsUI.Muted, false);
			this.ActionButton(rectTransform, "Cancel", "CANCEL", 408f, 460f, 140f, 42f, BodyPaintPresetsUI.ButtonColor, new UnityAction(this.ShowLibrary));
			this._importConfirmButton = this.ActionButton(rectTransform, "Import", "IMPORT BODYPAINT", 560f, 460f, 148f, 42f, BodyPaintPresetsUI.Accent, new UnityAction(this.ImportSelectedFolder));
			this.RefreshButtons();
		}

		// Token: 0x0600A712 RID: 42770 RVA: 0x0045DE10 File Offset: 0x0045C010
		private void ChooseImportFolder()
		{
			if (this._busy || !this._importDialog || this._dialog == null)
			{
				return;
			}
			if (this._importBrowser == null)
			{
				this._importBrowser = CielOSFileBrowser.Create(this._dialog.transform);
			}
			string text = this._importFolder ?? this._lastImportFolder;
			if (string.IsNullOrEmpty(text))
			{
				string exportRoot = BodyPaintPresetStore.ExportRoot;
				text = (Directory.Exists(exportRoot) ? exportRoot : Path.GetDirectoryName(exportRoot));
			}
			this._importBrowser.OpenFolderSelection("CHOOSE FOLDER", delegate(string selected)
			{
				this._dismissFrame = Time.frameCount;
				if (string.IsNullOrEmpty(selected) || !this._importDialog || this._dialog == null)
				{
					return;
				}
				this._importFolder = selected;
				this._importFolderLabel.text = selected;
				LocalizedGameText.BindFontOnly(this._importFolderLabel);
				this._importFolderLabel.color = Color.white;
				string text2 = BodyPaintPresetsUI.ImportNameFromFolder(selected);
				if (this._nameInput != null && (string.IsNullOrWhiteSpace(this._nameInput.text) || this._nameInput.text == this._suggestedImportName))
				{
					this._nameInput.text = text2;
				}
				this._suggestedImportName = text2;
				this.SetStatus(string.Empty, false);
				this.RefreshButtons();
			}, text, "bodypaint_presets", true);
		}

		// Token: 0x0600A713 RID: 42771 RVA: 0x0045DEB4 File Offset: 0x0045C0B4
		private static string ImportNameFromFolder(string folder)
		{
			string text = Path.GetFileName(folder.TrimEnd(new char[]
			{
				Path.DirectorySeparatorChar,
				Path.AltDirectorySeparatorChar
			}));
			if (text.StartsWith("Bodypaint_", StringComparison.Ordinal))
			{
				text = text.Substring("Bodypaint_".Length);
				text = Regex.Replace(text, "_\\d{8}_\\d{6}_[0-9a-f]{8}$", string.Empty);
			}
			if (string.IsNullOrWhiteSpace(text))
			{
				text = GameLocalization.GetSource("bodypaint_presets", "Imported body paint");
			}
			if (text.Length <= 64)
			{
				return text;
			}
			return text.Substring(0, 64);
		}

		// Token: 0x0600A714 RID: 42772 RVA: 0x0045DF40 File Offset: 0x0045C140
		private void ImportSelectedFolder()
		{
			if (this._busy || this._canvas == null)
			{
				return;
			}
			if (string.IsNullOrEmpty(this._importFolder))
			{
				this.SetStatus("Choose a bodypaint folder first.", true);
				return;
			}
			string text = ((this._nameInput != null) ? this._nameInput.text.Trim() : string.Empty);
			if (string.IsNullOrWhiteSpace(text))
			{
				this.SetStatus("Choose a name for this preset.", true);
				return;
			}
			this.Begin(BodyPaintPresetsUI.Operation.Import, null, text, this._importFolder);
		}

		// Token: 0x0600A715 RID: 42773 RVA: 0x0045DFC8 File Offset: 0x0045C1C8
		private void BuildPresetRow(RectTransform parent, BodyPaintPresetInfo preset, float y)
		{
			RectTransform rectTransform = BodyPaintPresetsUI.Rect("Preset_" + preset.id, parent);
			BodyPaintPresetsUI.TopLeft(rectTransform, 0f, y, 974f, 146f);
			rectTransform.gameObject.AddComponent<Image>().color = BodyPaintPresetsUI.Card;
			RectTransform rectTransform2 = BodyPaintPresetsUI.Rect("Preview", rectTransform);
			BodyPaintPresetsUI.TopLeft(rectTransform2, 16f, 18f, 176f, 88f);
			RawImage rawImage = rectTransform2.gameObject.AddComponent<RawImage>();
			rawImage.color = Color.white;
			rawImage.raycastTarget = false;
			try
			{
				Texture2D texture2D = BodyPaintPresetsUI.DecodePreview(BodyPaintPresetStore.LoadPreview(preset.id, null));
				if (texture2D != null)
				{
					this._thumbnails.Add(texture2D);
					rawImage.texture = texture2D;
				}
				else
				{
					rawImage.color = BodyPaintPresetsUI.ButtonColor;
				}
			}
			catch (Exception)
			{
				rawImage.color = BodyPaintPresetsUI.ButtonColor;
			}
			this.Label(rectTransform, "MapCaption", "BODY / FACE", 16f, 112f, 176f, 20f, 11f, BodyPaintPresetsUI.Muted, true).alignment = TextAlignmentOptions.Center;
			this.Label(rectTransform, "PresetName", preset.name, 214f, 14f, 738f, 34f, 23f, Color.white, false);
			string text = preset.createdUtc;
			DateTime dateTime;
			if (DateTime.TryParse(preset.createdUtc, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out dateTime))
			{
				text = dateTime.ToLocalTime().ToString("g", CultureInfo.CurrentCulture);
			}
			this.Label(rectTransform, "CreatedDate", text, 214f, 50f, 738f, 25f, 14f, BodyPaintPresetsUI.Muted, false);
			this.ActionButton(rectTransform, "Load", "LOAD", 214f, 92f, 126f, 38f, BodyPaintPresetsUI.Accent, delegate
			{
				this.RequestLoad(preset);
			});
			this.ActionButton(rectTransform, "Export", "EXPORT PNG", 350f, 92f, 206f, 38f, BodyPaintPresetsUI.ButtonColor, delegate
			{
				this.Begin(BodyPaintPresetsUI.Operation.ExportSaved, preset, preset.name, null);
			});
			this.ActionButton(rectTransform, "Delete", "DELETE", 832f, 92f, 120f, 38f, BodyPaintPresetsUI.ButtonColor, delegate
			{
				this.ShowConfirmation(preset, true);
			});
		}

		// Token: 0x0600A716 RID: 42774 RVA: 0x0045E260 File Offset: 0x0045C460
		private void RequestLoad(BodyPaintPresetInfo preset)
		{
			if (this._busy || this._canvas == null)
			{
				return;
			}
			this._canvas.RefreshCoverage();
			if (this._canvas.HasAnyPaint)
			{
				this.ShowConfirmation(preset, false);
				return;
			}
			this.Begin(BodyPaintPresetsUI.Operation.Load, preset, null, null);
		}

		// Token: 0x0600A717 RID: 42775 RVA: 0x0045E2B0 File Offset: 0x0045C4B0
		private void ShowConfirmation(BodyPaintPresetInfo preset, bool delete)
		{
			if (this._busy)
			{
				return;
			}
			RectTransform rectTransform = this.CreateDialog(delete ? "DeletePresetDialog" : "LoadPresetDialog", delete ? "DELETE PRESET?" : "REPLACE CURRENT PAINT?", 680f, 350f);
			this._confirmation = true;
			this.Label(rectTransform, "PresetName", preset.name, 32f, 82f, 616f, 42f, 23f, Color.white, false);
			this.Label(rectTransform, "ConfirmationHint", delete ? "This removes the preset from your library. Exported copies are kept." : "Loading replaces the paint on your body and face. Save your current look first if you want to keep it.", 32f, 136f, 616f, 76f, 18f, BodyPaintPresetsUI.Muted, true);
			this._dialogStatus = this.Label(rectTransform, "DialogStatus", string.Empty, 32f, 220f, 616f, 45f, 15f, BodyPaintPresetsUI.Muted, false);
			this.ActionButton(rectTransform, "Cancel", "CANCEL", 352f, 280f, 142f, 42f, BodyPaintPresetsUI.ButtonColor, new UnityAction(this.ShowLibrary));
			this.ActionButton(rectTransform, "Confirm", delete ? "DELETE" : "LOAD PRESET", 506f, 280f, 142f, 42f, BodyPaintPresetsUI.Accent, delegate
			{
				this.Begin(delete ? BodyPaintPresetsUI.Operation.Delete : BodyPaintPresetsUI.Operation.Load, preset, null, null);
			});
		}

		// Token: 0x0600A718 RID: 42776 RVA: 0x0045E44C File Offset: 0x0045C64C
		private void Begin(BodyPaintPresetsUI.Operation operation, BodyPaintPresetInfo preset, string name, string importFolder = null)
		{
			if (this._busy || this._canvas == null)
			{
				return;
			}
			this._busy = true;
			this.RefreshButtons();
			if (this._dialog != null)
			{
				Selectable[] componentsInChildren = this._dialog.GetComponentsInChildren<Selectable>();
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					componentsInChildren[i].interactable = false;
				}
			}
			this.SetStatus((operation == BodyPaintPresetsUI.Operation.Import) ? "Importing..." : ((operation == BodyPaintPresetsUI.Operation.Load) ? "Loading..." : ((operation == BodyPaintPresetsUI.Operation.Delete) ? "Deleting..." : "Saving...")), false);
			base.StartCoroutine(this.PerformOperation(operation, preset, name, importFolder));
		}

		// Token: 0x0600A719 RID: 42777 RVA: 0x0045E4ED File Offset: 0x0045C6ED
		private IEnumerator PerformOperation(BodyPaintPresetsUI.Operation operation, BodyPaintPresetInfo preset, string name, string importFolder)
		{
			yield return null;
			bool flag = false;
			string text = string.Empty;
			Texture2D texture2D = null;
			Texture2D texture2D2 = null;
			Texture2D texture2D3 = null;
			try
			{
				if (operation == BodyPaintPresetsUI.Operation.Import)
				{
					Vector2Int canvasSize = this._canvas.GetCanvasSize(BodyPaintCanvasService.PaintChannel.Body);
					Vector2Int canvasSize2 = this._canvas.GetCanvasSize(BodyPaintCanvasService.PaintChannel.Head);
					BodyPaintPresetFiles bodyPaintPresetFiles = BodyPaintPresetStore.ReadImportFolder(importFolder, canvasSize.x, canvasSize.y, canvasSize2.x, canvasSize2.y);
					texture2D = BodyPaintPresetsUI.DecodePaint(bodyPaintPresetFiles.BodyPng);
					texture2D2 = BodyPaintPresetsUI.DecodePaint(bodyPaintPresetFiles.HeadPng);
					texture2D3 = BodyPaintPresetsUI.CreateMapPreview(texture2D, texture2D2);
					BodyPaintPresetStore.Save(name, bodyPaintPresetFiles.BodyPng, bodyPaintPresetFiles.HeadPng, texture2D3.EncodeToPNG(), null);
					this._lastImportFolder = importFolder;
					text = "Body paint imported. Find it in My Presets.";
				}
				else if (operation == BodyPaintPresetsUI.Operation.Delete)
				{
					BodyPaintPresetStore.Delete(preset.id, null);
					text = "Preset deleted.";
				}
				else if (operation == BodyPaintPresetsUI.Operation.Load)
				{
					BodyPaintPresetFiles bodyPaintPresetFiles2 = BodyPaintPresetStore.Load(preset.id, null);
					texture2D = BodyPaintPresetsUI.DecodePaint(bodyPaintPresetFiles2.BodyPng);
					texture2D2 = BodyPaintPresetsUI.DecodePaint(bodyPaintPresetFiles2.HeadPng);
					this._canvas.RestorePaintTextures(texture2D, texture2D2);
					text = "Preset loaded. Keep painting to make it your own.";
				}
				else
				{
					byte[] array = null;
					byte[] array2;
					byte[] array3;
					if (operation == BodyPaintPresetsUI.Operation.ExportSaved)
					{
						BodyPaintPresetFiles bodyPaintPresetFiles3 = BodyPaintPresetStore.Load(preset.id, null);
						array2 = bodyPaintPresetFiles3.BodyPng;
						array3 = bodyPaintPresetFiles3.HeadPng;
					}
					else
					{
						texture2D = this._canvas.CapturePaintTexture(BodyPaintCanvasService.PaintChannel.Body);
						texture2D2 = this._canvas.CapturePaintTexture(BodyPaintCanvasService.PaintChannel.Head);
						if (texture2D == null || texture2D2 == null)
						{
							throw new InvalidOperationException("Paint textures are not available.");
						}
						array2 = texture2D.EncodeToPNG();
						array3 = texture2D2.EncodeToPNG();
						if (operation == BodyPaintPresetsUI.Operation.Save)
						{
							texture2D3 = BodyPaintPresetsUI.CreateMapPreview(texture2D, texture2D2);
							array = texture2D3.EncodeToPNG();
						}
					}
					if (operation == BodyPaintPresetsUI.Operation.Save)
					{
						BodyPaintPresetStore.Save(name, array2, array3, array, null);
						text = "Preset saved. Find it in My Presets.";
					}
					else
					{
						string text2 = BodyPaintPresetStore.ExportToGameFolder(name, array2, array3);
						Debug.Log("[BodyPaintPresets] Exported to " + text2, this);
						text = "Body paint saved in the game's Bodypaints folder.";
					}
				}
				flag = true;
			}
			catch (BodyPaintImportException ex)
			{
				switch (ex.Code)
				{
				case BodyPaintImportError.MissingFiles:
					this.SetStatus("This folder needs both body.png and head.png.", true);
					goto IL_02A9;
				case BodyPaintImportError.IncompatibleSize:
					this.SetStatus("These images do not match the body and face canvas sizes.", true);
					goto IL_02A9;
				case BodyPaintImportError.UnreadableFolder:
					this.SetStatus("This folder could not be read. Choose another folder.", true);
					goto IL_02A9;
				}
				this.SetStatus("The images must be valid PNG files with transparency.", true);
				IL_02A9:;
			}
			catch (Exception ex2)
			{
				this.ReportFailure((operation == BodyPaintPresetsUI.Operation.Import) ? "Could not import this body paint." : ((operation == BodyPaintPresetsUI.Operation.Load) ? "Could not load this preset." : ((operation == BodyPaintPresetsUI.Operation.Delete) ? "Could not delete this preset." : ((operation == BodyPaintPresetsUI.Operation.Save) ? "Could not save this preset." : "Could not export this body paint."))), ex2);
			}
			finally
			{
				if (texture2D != null)
				{
					Object.Destroy(texture2D);
				}
				if (texture2D2 != null)
				{
					Object.Destroy(texture2D2);
				}
				if (texture2D3 != null)
				{
					Object.Destroy(texture2D3);
				}
				this._busy = false;
				if (this._dialog != null)
				{
					Selectable[] componentsInChildren = this._dialog.GetComponentsInChildren<Selectable>();
					for (int i = 0; i < componentsInChildren.Length; i++)
					{
						componentsInChildren[i].interactable = true;
					}
				}
				this.RefreshButtons();
			}
			if (!flag)
			{
				yield break;
			}
			if (operation == BodyPaintPresetsUI.Operation.Delete || operation == BodyPaintPresetsUI.Operation.Import)
			{
				this.ShowLibrary();
			}
			else if (operation == BodyPaintPresetsUI.Operation.Save || operation == BodyPaintPresetsUI.Operation.Load)
			{
				this.CloseDialog();
			}
			this.SetStatus(text, false);
			yield break;
		}

		// Token: 0x0600A71A RID: 42778 RVA: 0x0045E519 File Offset: 0x0045C719
		private void ReportFailure(string source, Exception exception)
		{
			Debug.LogWarning("[BodyPaintPresets] " + source + " " + exception.Message, this);
			this.SetStatus(source + " Please try again.", true);
		}

		// Token: 0x0600A71B RID: 42779 RVA: 0x0045E54C File Offset: 0x0045C74C
		private void SetStatus(string source, bool error = false)
		{
			Color color = (error ? BodyPaintPresetsUI.Error : BodyPaintPresetsUI.Success);
			if (this.statusLabel != null)
			{
				LocalizedGameText localizedGameText = LocalizedGameText.BindSource(this.statusLabel, "bodypaint_presets", source, null);
				if (localizedGameText != null)
				{
					localizedGameText.FitTranslationToRectangle(false);
				}
				this.statusLabel.color = color;
			}
			if (this._dialogStatus != null)
			{
				LocalizedGameText localizedGameText2 = LocalizedGameText.BindSource(this._dialogStatus, "bodypaint_presets", source, null);
				if (localizedGameText2 != null)
				{
					localizedGameText2.FitTranslationToRectangle(false);
				}
				this._dialogStatus.color = color;
			}
		}

		// Token: 0x0600A71C RID: 42780 RVA: 0x0045E5DC File Offset: 0x0045C7DC
		private static string DefaultName()
		{
			return GameLocalization.GetSource("bodypaint_presets", "My body paint") + " " + DateTime.Now.ToString("yyyy-MM-dd HH-mm");
		}

		// Token: 0x0600A71D RID: 42781 RVA: 0x0045E614 File Offset: 0x0045C814
		private static Texture2D DecodePaint(byte[] png)
		{
			Texture2D texture2D = new Texture2D(2, 2, TextureFormat.RGBA32, false, false);
			if (png == null || !texture2D.LoadImage(png, false))
			{
				Object.Destroy(texture2D);
				throw new InvalidOperationException("This preset has an unreadable paint image.");
			}
			texture2D.wrapMode = TextureWrapMode.Clamp;
			return texture2D;
		}

		// Token: 0x0600A71E RID: 42782 RVA: 0x0045E654 File Offset: 0x0045C854
		private static Texture2D DecodePreview(byte[] png)
		{
			if (png == null || png.Length == 0)
			{
				return null;
			}
			Texture2D texture2D = new Texture2D(2, 2, TextureFormat.RGBA32, false, false);
			if (texture2D.LoadImage(png, true))
			{
				return texture2D;
			}
			Object.Destroy(texture2D);
			return null;
		}

		// Token: 0x0600A71F RID: 42783 RVA: 0x0045E688 File Offset: 0x0045C888
		private static Texture2D CreateMapPreview(Texture2D body, Texture2D head)
		{
			Texture2D texture2D = new Texture2D(400, 200, TextureFormat.RGB24, false, false);
			Color32[] array = new Color32[80000];
			for (int i = 0; i < 200; i++)
			{
				for (int j = 0; j < 400; j++)
				{
					Color pixelBilinear = ((j < 200) ? body : head).GetPixelBilinear(((float)(j % 200) + 0.5f) / 200f, ((float)i + 0.5f) / 200f);
					float num = (((j / 12 + i / 12) % 2 == 0) ? 0.16f : 0.21f);
					Color color = Color.Lerp(new Color(num, num, num), pixelBilinear, pixelBilinear.a);
					color.a = 1f;
					array[i * 200 * 2 + j] = color;
				}
			}
			texture2D.SetPixels32(array);
			texture2D.Apply(false, false);
			return texture2D;
		}

		// Token: 0x0600A720 RID: 42784 RVA: 0x0045E778 File Offset: 0x0045C978
		private RectTransform CreateDialog(string name, string title, float width, float height)
		{
			this.CloseDialog();
			RectTransform rectTransform = BodyPaintPresetsUI.Rect(name, base.transform);
			BodyPaintPresetsUI.Stretch(rectTransform, 0f);
			this._dialog = rectTransform.gameObject;
			rectTransform.gameObject.AddComponent<Image>().color = new Color(0f, 0f, 0f, 0.74f);
			RectTransform rectTransform2 = BodyPaintPresetsUI.Rect("Card", rectTransform);
			RectTransform rectTransform3 = rectTransform2;
			RectTransform rectTransform4 = rectTransform2;
			RectTransform rectTransform5 = rectTransform2;
			Vector2 vector = new Vector2(0.5f, 0.5f);
			rectTransform5.pivot = vector;
			rectTransform3.anchorMin = (rectTransform4.anchorMax = vector);
			rectTransform2.sizeDelta = new Vector2(width, height);
			rectTransform2.gameObject.AddComponent<Image>().color = BodyPaintPresetsUI.Panel;
			RectTransform rectTransform6 = BodyPaintPresetsUI.Rect("Accent", rectTransform2);
			BodyPaintPresetsUI.TopLeft(rectTransform6, 0f, 0f, width, 4f);
			rectTransform6.gameObject.AddComponent<Image>().color = BodyPaintPresetsUI.Accent;
			this.Label(rectTransform2, "DialogTitle", title, 32f, 26f, width - 64f, 42f, 29f, Color.white, true);
			ControllerNavigationScope.Configure(this._dialog, delegate
			{
				this.TryCloseDialog();
			}, null, 120, false);
			return rectTransform2;
		}

		// Token: 0x0600A721 RID: 42785 RVA: 0x0045E8B0 File Offset: 0x0045CAB0
		private TMP_InputField CreateInput(RectTransform parent, float x, float y, float width, float height)
		{
			RectTransform rectTransform = BodyPaintPresetsUI.Rect("PresetNameInput", parent);
			BodyPaintPresetsUI.TopLeft(rectTransform, x, y, width, height);
			Image image = rectTransform.gameObject.AddComponent<Image>();
			image.color = BodyPaintPresetsUI.Card;
			RectTransform rectTransform2 = BodyPaintPresetsUI.Rect("TextArea", rectTransform);
			BodyPaintPresetsUI.Stretch(rectTransform2, 0f);
			rectTransform2.offsetMin = new Vector2(12f, 4f);
			rectTransform2.offsetMax = new Vector2(-12f, -4f);
			rectTransform2.gameObject.AddComponent<RectMask2D>();
			TMP_Text tmp_Text = this.Label(rectTransform2, "Value", string.Empty, 0f, 0f, width - 24f, height - 8f, 21f, Color.white, false);
			BodyPaintPresetsUI.Stretch(tmp_Text.rectTransform, 0f);
			tmp_Text.overflowMode = TextOverflowModes.Overflow;
			tmp_Text.GetComponent<LocalizedGameText>().SetSourceLayout(false, false, 21f, 21f);
			TMP_Text tmp_Text2 = this.Label(rectTransform2, "Placeholder", "Name your look", 0f, 0f, width - 24f, height - 8f, 21f, BodyPaintPresetsUI.Muted, true);
			BodyPaintPresetsUI.Stretch(tmp_Text2.rectTransform, 0f);
			LocalizedGameText component = tmp_Text2.GetComponent<LocalizedGameText>();
			component.SetSourceLayout(true, false, 14f, 21f);
			component.FitTranslationToRectangle(true);
			TMP_InputField tmp_InputField = rectTransform.gameObject.AddComponent<TMP_InputField>();
			tmp_InputField.targetGraphic = image;
			tmp_InputField.textViewport = rectTransform2;
			tmp_InputField.textComponent = (TextMeshProUGUI)tmp_Text;
			tmp_InputField.placeholder = tmp_Text2;
			tmp_InputField.lineType = TMP_InputField.LineType.SingleLine;
			tmp_InputField.characterLimit = 64;
			tmp_InputField.richText = false;
			tmp_InputField.customCaretColor = true;
			tmp_InputField.caretColor = Color.white;
			tmp_InputField.selectionColor = new Color(BodyPaintPresetsUI.Accent.r, BodyPaintPresetsUI.Accent.g, BodyPaintPresetsUI.Accent.b, 0.5f);
			return tmp_InputField;
		}

		// Token: 0x0600A722 RID: 42786 RVA: 0x0045EA8C File Offset: 0x0045CC8C
		private TMP_Text Label(Transform parent, string name, string value, float x, float y, float width, float height, float size, Color color, bool translate = true)
		{
			RectTransform rectTransform = BodyPaintPresetsUI.Rect(name, parent);
			BodyPaintPresetsUI.TopLeft(rectTransform, x, y, width, height);
			TextMeshProUGUI textMeshProUGUI = rectTransform.gameObject.AddComponent<TextMeshProUGUI>();
			if (this.font != null)
			{
				textMeshProUGUI.font = this.font;
			}
			textMeshProUGUI.text = value;
			textMeshProUGUI.fontSize = size;
			textMeshProUGUI.color = color;
			textMeshProUGUI.raycastTarget = false;
			textMeshProUGUI.richText = false;
			textMeshProUGUI.alignment = TextAlignmentOptions.MidlineLeft;
			textMeshProUGUI.enableWordWrapping = true;
			textMeshProUGUI.overflowMode = TextOverflowModes.Ellipsis;
			if (translate)
			{
				LocalizedGameText localizedGameText = LocalizedGameText.BindSource(textMeshProUGUI, "bodypaint_presets", value, null);
				if (localizedGameText != null)
				{
					localizedGameText.FitTranslationToRectangle(false);
				}
			}
			else
			{
				LocalizedGameText.BindFontOnly(textMeshProUGUI);
			}
			return textMeshProUGUI;
		}

		// Token: 0x0600A723 RID: 42787 RVA: 0x0045EB38 File Offset: 0x0045CD38
		private Button ActionButton(Transform parent, string name, string caption, float x, float y, float width, float height, Color color, UnityAction callback)
		{
			RectTransform rectTransform = BodyPaintPresetsUI.Rect(name, parent);
			BodyPaintPresetsUI.TopLeft(rectTransform, x, y, width, height);
			Image image = rectTransform.gameObject.AddComponent<Image>();
			image.color = color;
			Button button = rectTransform.gameObject.AddComponent<Button>();
			button.targetGraphic = image;
			ColorBlock colors = button.colors;
			colors.normalColor = Color.white;
			colors.highlightedColor = new Color(1.18f, 1.18f, 1.18f);
			colors.pressedColor = new Color(0.8f, 0.8f, 0.8f);
			colors.selectedColor = Color.white;
			colors.disabledColor = new Color(1f, 1f, 1f, 0.4f);
			button.colors = colors;
			button.onClick.AddListener(delegate
			{
				if (!this._busy)
				{
					callback();
				}
			});
			this.Label(rectTransform, "Label", caption, 8f, 2f, width - 16f, height - 4f, 15f, Color.white, true).alignment = TextAlignmentOptions.Center;
			return button;
		}

		// Token: 0x0600A724 RID: 42788 RVA: 0x0045EC65 File Offset: 0x0045CE65
		private static RectTransform Rect(string name, Transform parent)
		{
			RectTransform component = new GameObject(name, new Type[] { typeof(RectTransform) })
			{
				layer = 5
			}.GetComponent<RectTransform>();
			component.SetParent(parent, false);
			return component;
		}

		// Token: 0x0600A725 RID: 42789 RVA: 0x0045EC94 File Offset: 0x0045CE94
		private static void TopLeft(RectTransform rect, float x, float y, float width, float height)
		{
			Vector2 vector = new Vector2(0f, 1f);
			rect.pivot = vector;
			rect.anchorMin = (rect.anchorMax = vector);
			rect.anchoredPosition = new Vector2(x, -y);
			rect.sizeDelta = new Vector2(width, height);
		}

		// Token: 0x0600A726 RID: 42790 RVA: 0x0045ECE5 File Offset: 0x0045CEE5
		private static void Stretch(RectTransform rect, float inset = 0f)
		{
			rect.anchorMin = Vector2.zero;
			rect.anchorMax = Vector2.one;
			rect.offsetMin = Vector2.one * inset;
			rect.offsetMax = Vector2.one * -inset;
		}

		// Token: 0x0400ABCA RID: 43978
		private const string Scope = "bodypaint_presets";

		// Token: 0x0400ABCB RID: 43979
		private static readonly Color Panel = new Color(0.075f, 0.075f, 0.095f, 1f);

		// Token: 0x0400ABCC RID: 43980
		private static readonly Color Card = new Color(0.14f, 0.14f, 0.17f, 1f);

		// Token: 0x0400ABCD RID: 43981
		private static readonly Color ButtonColor = new Color(0.22f, 0.22f, 0.27f, 1f);

		// Token: 0x0400ABCE RID: 43982
		private static readonly Color Accent = new Color(0.88f, 0.26f, 0.52f, 1f);

		// Token: 0x0400ABCF RID: 43983
		private static readonly Color Muted = new Color(0.72f, 0.72f, 0.78f, 1f);

		// Token: 0x0400ABD0 RID: 43984
		private static readonly Color Success = new Color(0.63f, 0.89f, 0.74f, 1f);

		// Token: 0x0400ABD1 RID: 43985
		private static readonly Color Error = new Color(1f, 0.65f, 0.65f, 1f);

		// Token: 0x0400ABD2 RID: 43986
		[SerializeField]
		private RectTransform section;

		// Token: 0x0400ABD3 RID: 43987
		[SerializeField]
		private Button saveButton;

		// Token: 0x0400ABD4 RID: 43988
		[SerializeField]
		private Button libraryButton;

		// Token: 0x0400ABD5 RID: 43989
		[SerializeField]
		private Button exportButton;

		// Token: 0x0400ABD6 RID: 43990
		[SerializeField]
		private TMP_Text statusLabel;

		// Token: 0x0400ABD7 RID: 43991
		[SerializeField]
		private TMP_FontAsset font;

		// Token: 0x0400ABD8 RID: 43992
		private BodyPaintCanvasService _canvas;

		// Token: 0x0400ABD9 RID: 43993
		private GameObject _dialog;

		// Token: 0x0400ABDA RID: 43994
		private TMP_Text _dialogStatus;

		// Token: 0x0400ABDB RID: 43995
		private TMP_InputField _nameInput;

		// Token: 0x0400ABDC RID: 43996
		private CielOSFileBrowser _importBrowser;

		// Token: 0x0400ABDD RID: 43997
		private TMP_Text _importFolderLabel;

		// Token: 0x0400ABDE RID: 43998
		private Button _importConfirmButton;

		// Token: 0x0400ABDF RID: 43999
		private string _importFolder;

		// Token: 0x0400ABE0 RID: 44000
		private string _lastImportFolder;

		// Token: 0x0400ABE1 RID: 44001
		private string _suggestedImportName;

		// Token: 0x0400ABE2 RID: 44002
		private bool _importDialog;

		// Token: 0x0400ABE3 RID: 44003
		private readonly List<Texture2D> _thumbnails = new List<Texture2D>();

		// Token: 0x0400ABE4 RID: 44004
		private bool _busy;

		// Token: 0x0400ABE5 RID: 44005
		private bool _confirmation;

		// Token: 0x0400ABE6 RID: 44006
		private int _dismissFrame = -1;

		// Token: 0x0200151B RID: 5403
		private enum Operation
		{
			// Token: 0x0400ABE8 RID: 44008
			Save,
			// Token: 0x0400ABE9 RID: 44009
			ExportCurrent,
			// Token: 0x0400ABEA RID: 44010
			ExportSaved,
			// Token: 0x0400ABEB RID: 44011
			Load,
			// Token: 0x0400ABEC RID: 44012
			Delete,
			// Token: 0x0400ABED RID: 44013
			Import
		}
	}
}
