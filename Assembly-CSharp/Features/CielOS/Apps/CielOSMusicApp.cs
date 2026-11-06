using System;
using System.Collections.Generic;
using System.IO;
using Features.CielOS.UI;
using Features.CielOS.VFS;
using Features.Localization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.CielOS.Apps
{
	// Token: 0x020013F2 RID: 5106
	public class CielOSMusicApp : MonoBehaviour
	{
		// Token: 0x06009DA9 RID: 40361 RVA: 0x0040CB14 File Offset: 0x0040AD14
		private void Awake()
		{
			foreach (object obj in base.transform)
			{
				((Transform)obj).gameObject.SetActive(false);
			}
			Canvas componentInParent = base.GetComponentInParent<Canvas>();
			if (componentInParent != null)
			{
				this._svc = CielOSAudioService.Ensure(componentInParent.transform);
			}
			this.BuildUI();
		}

		// Token: 0x06009DAA RID: 40362 RVA: 0x0040CB98 File Offset: 0x0040AD98
		private void OnEnable()
		{
			if (this._svc == null)
			{
				return;
			}
			this._svc.TrackChanged += this.OnTrackChanged;
			this._svc.StateChanged += this.OnStateChanged;
			this._svc.Rescan();
			this.OnTrackChanged();
			this.OnStateChanged();
		}

		// Token: 0x06009DAB RID: 40363 RVA: 0x0040CBF9 File Offset: 0x0040ADF9
		private void OnDisable()
		{
			if (this._svc == null)
			{
				return;
			}
			this._svc.TrackChanged -= this.OnTrackChanged;
			this._svc.StateChanged -= this.OnStateChanged;
		}

		// Token: 0x06009DAC RID: 40364 RVA: 0x0040CC38 File Offset: 0x0040AE38
		private void OnDestroy()
		{
			if (this._svc != null && !this._svc.HasPlaybackRetention)
			{
				this._svc.Stop();
			}
		}

		// Token: 0x06009DAD RID: 40365 RVA: 0x0040CC60 File Offset: 0x0040AE60
		private void BuildUI()
		{
			CielOSTheme active = CielOSTheme.Active;
			RectTransform rectTransform = (RectTransform)base.transform;
			rectTransform.anchorMin = Vector2.zero;
			rectTransform.anchorMax = Vector2.one;
			rectTransform.offsetMin = Vector2.zero;
			rectTransform.offsetMax = Vector2.zero;
			VerticalLayoutGroup verticalLayoutGroup = rectTransform.gameObject.GetComponent<VerticalLayoutGroup>();
			if (verticalLayoutGroup == null)
			{
				verticalLayoutGroup = rectTransform.gameObject.AddComponent<VerticalLayoutGroup>();
			}
			verticalLayoutGroup.padding = new RectOffset(12, 12, 10, 10);
			verticalLayoutGroup.spacing = 8f;
			verticalLayoutGroup.childControlWidth = true;
			verticalLayoutGroup.childControlHeight = true;
			verticalLayoutGroup.childForceExpandWidth = true;
			verticalLayoutGroup.childForceExpandHeight = false;
			RectTransform rectTransform2 = this.NewChild("NowPlaying", rectTransform, 22f);
			this._nowName = this.MakeText(rectTransform2, "Name", "No track", 14f, (active != null) ? active.titleText : Color.white, FontStyles.Bold, TextAlignmentOptions.MidlineLeft);
			CielOSMusicApp.Stretch((RectTransform)this._nowName.transform);
			this._nowName.overflowMode = TextOverflowModes.Ellipsis;
			this._nowName.enableWordWrapping = false;
			RectTransform rectTransform3 = this.NewChild("Transport", rectTransform, 40f);
			HorizontalLayoutGroup horizontalLayoutGroup = rectTransform3.gameObject.AddComponent<HorizontalLayoutGroup>();
			horizontalLayoutGroup.spacing = 8f;
			horizontalLayoutGroup.childAlignment = TextAnchor.MiddleCenter;
			horizontalLayoutGroup.childControlWidth = false;
			horizontalLayoutGroup.childControlHeight = false;
			horizontalLayoutGroup.childForceExpandWidth = false;
			horizontalLayoutGroup.childForceExpandHeight = false;
			this._shuffleBg = this.IconButton(rectTransform3, CielOSIconId.Shuffle, delegate
			{
				this._svc.ToggleShuffle();
			}, 34f, 0f).GetComponent<Image>();
			this.IconButton(rectTransform3, CielOSIconId.SkipPrev, delegate
			{
				this._svc.Prev();
			}, 38f, 0f);
			GameObject gameObject = this.IconButton(rectTransform3, CielOSIconId.Play, delegate
			{
				this._svc.TogglePlayPause();
			}, 46f, 0f);
			this._playIcon = gameObject.transform.Find("Icon").GetComponent<Image>();
			this.IconButton(rectTransform3, CielOSIconId.SkipNext, delegate
			{
				this._svc.Next();
			}, 38f, 0f);
			this._repeatBg = this.IconButton(rectTransform3, CielOSIconId.Repeat, delegate
			{
				this._svc.CycleRepeat();
			}, 34f, 0f).GetComponent<Image>();
			RectTransform rectTransform4 = this.NewChild("VolumeRow", rectTransform, 30f);
			HorizontalLayoutGroup horizontalLayoutGroup2 = rectTransform4.gameObject.AddComponent<HorizontalLayoutGroup>();
			horizontalLayoutGroup2.spacing = 8f;
			horizontalLayoutGroup2.childAlignment = TextAnchor.MiddleLeft;
			horizontalLayoutGroup2.childControlWidth = true;
			horizontalLayoutGroup2.childControlHeight = true;
			horizontalLayoutGroup2.childForceExpandWidth = false;
			horizontalLayoutGroup2.childForceExpandHeight = true;
			GameObject gameObject2 = new GameObject("VolIcon", new Type[]
			{
				typeof(RectTransform),
				typeof(Image)
			});
			gameObject2.transform.SetParent(rectTransform4, false);
			LayoutElement layoutElement = gameObject2.AddComponent<LayoutElement>();
			layoutElement.minWidth = 22f;
			layoutElement.preferredWidth = 22f;
			layoutElement.flexibleWidth = 0f;
			CielOSIcon.Apply(gameObject2.GetComponent<Image>(), CielOSIconId.Volume, (active != null) ? active.trayIconTint : Color.white, 64);
			this._volume = this.BuildSlider(rectTransform4, 180f, 18f);
			this._volume.value = ((this._svc != null) ? this._svc.Volume : 0.8f);
			this._volume.onValueChanged.AddListener(delegate(float v)
			{
				if (this._svc != null)
				{
					this._svc.Volume = v;
				}
			});
			this.LabeledIconButton(rectTransform4, CielOSIconId.Upload, "Import", new Action(this.ImportMusic), 86f, 28f);
			RectTransform rectTransform5 = this.NewChild("ListArea", rectTransform, 0f);
			LayoutElement layoutElement2 = rectTransform5.gameObject.AddComponent<LayoutElement>();
			layoutElement2.flexibleHeight = 1f;
			layoutElement2.minHeight = 80f;
			Image image = rectTransform5.gameObject.AddComponent<Image>();
			image.color = new Color(0f, 0f, 0f, 0.18f);
			CielOSTheme.Round(image);
			rectTransform5.gameObject.AddComponent<RectMask2D>();
			ScrollRect scrollRect = rectTransform5.gameObject.AddComponent<ScrollRect>();
			scrollRect.horizontal = false;
			scrollRect.vertical = true;
			scrollRect.movementType = ScrollRect.MovementType.Clamped;
			scrollRect.scrollSensitivity = 24f;
			this._runtimeList = this.NewChild("ListContent", rectTransform5, 0f);
			this._runtimeList.anchorMin = new Vector2(0f, 1f);
			this._runtimeList.anchorMax = new Vector2(1f, 1f);
			this._runtimeList.pivot = new Vector2(0.5f, 1f);
			this._runtimeList.offsetMin = new Vector2(0f, this._runtimeList.offsetMin.y);
			this._runtimeList.offsetMax = new Vector2(0f, this._runtimeList.offsetMax.y);
			this._runtimeList.anchoredPosition = Vector2.zero;
			VerticalLayoutGroup verticalLayoutGroup2 = this._runtimeList.gameObject.AddComponent<VerticalLayoutGroup>();
			verticalLayoutGroup2.spacing = 3f;
			verticalLayoutGroup2.padding = new RectOffset(6, 6, 6, 6);
			verticalLayoutGroup2.childControlWidth = true;
			verticalLayoutGroup2.childControlHeight = true;
			verticalLayoutGroup2.childForceExpandWidth = true;
			verticalLayoutGroup2.childForceExpandHeight = false;
			this._runtimeList.gameObject.AddComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.PreferredSize;
			scrollRect.viewport = rectTransform5;
			scrollRect.content = this._runtimeList;
		}

		// Token: 0x06009DAE RID: 40366 RVA: 0x0040D1BC File Offset: 0x0040B3BC
		private void OnTrackChanged()
		{
			if (this._nowName != null)
			{
				LocalizedGameText.BindComputed(this._nowName, delegate
				{
					string text;
					if ((text = this._svc.CurrentName) == null)
					{
						text = CielOSContentText.Get((this._svc.Tracks.Count == 0) ? "No tracks — Import audio →" : "Stopped");
					}
					return text;
				});
			}
			this.RebuildList();
		}

		// Token: 0x06009DAF RID: 40367 RVA: 0x0040D1EC File Offset: 0x0040B3EC
		private void OnStateChanged()
		{
			CielOSTheme active = CielOSTheme.Active;
			Color color = ((active != null) ? active.accent : new Color(0.66f, 0.38f, 0.79f, 1f));
			Color color2 = ((active != null) ? new Color(active.controlBackground.r * 0.7f, active.controlBackground.g * 0.7f, active.controlBackground.b * 0.7f, 1f) : new Color(0.13f, 0.1f, 0.17f, 1f));
			if (this._shuffleBg != null)
			{
				this._shuffleBg.color = (this._svc.Shuffle ? color : color2);
			}
			if (this._repeatBg != null)
			{
				this._repeatBg.color = ((this._svc.Repeat == CielOSAudioService.RepeatMode.RepeatOne) ? color : color2);
			}
			if (this._playIcon != null)
			{
				CielOSIcon.Apply(this._playIcon, this._svc.IsPlaying ? CielOSIconId.Pause : CielOSIconId.Play, (active != null) ? active.logoAccent : Color.white, 64);
			}
			if (this._volume != null && !Mathf.Approximately(this._volume.value, this._svc.Volume))
			{
				this._volume.SetValueWithoutNotify(this._svc.Volume);
			}
		}

		// Token: 0x06009DB0 RID: 40368 RVA: 0x0040D368 File Offset: 0x0040B568
		private void RebuildList()
		{
			foreach (GameObject gameObject in this._rows)
			{
				if (gameObject != null)
				{
					Object.Destroy(gameObject);
				}
			}
			this._rows.Clear();
			if (this._runtimeList == null)
			{
				return;
			}
			CielOSTheme active = CielOSTheme.Active;
			for (int i = 0; i < this._svc.Tracks.Count; i++)
			{
				int idx = i;
				string fileName = Path.GetFileName(this._svc.Tracks[i]);
				GameObject gameObject2 = new GameObject(fileName, new Type[]
				{
					typeof(RectTransform),
					typeof(Image),
					typeof(Button),
					typeof(LayoutElement)
				});
				gameObject2.transform.SetParent(this._runtimeList, false);
				LayoutElement component = gameObject2.GetComponent<LayoutElement>();
				component.minHeight = 30f;
				component.preferredHeight = 30f;
				component.flexibleHeight = 0f;
				Image component2 = gameObject2.GetComponent<Image>();
				component2.color = ((i == this._svc.Index) ? ((active != null) ? active.accent : new Color(0.66f, 0.38f, 0.79f, 1f)) : ((active != null) ? active.controlBackground : new Color(0.18f, 0.14f, 0.23f, 1f)));
				CielOSTheme.Round(component2);
				GameObject gameObject3 = new GameObject("Icon", new Type[]
				{
					typeof(RectTransform),
					typeof(Image)
				});
				gameObject3.transform.SetParent(gameObject2.transform, false);
				RectTransform rectTransform = (RectTransform)gameObject3.transform;
				rectTransform.anchorMin = new Vector2(0f, 0.5f);
				rectTransform.anchorMax = new Vector2(0f, 0.5f);
				rectTransform.pivot = new Vector2(0f, 0.5f);
				rectTransform.sizeDelta = new Vector2(15f, 15f);
				rectTransform.anchoredPosition = new Vector2(10f, 0f);
				CielOSIcon.Apply(gameObject3.GetComponent<Image>(), CielOSIconId.Music, (active != null) ? active.logoAccent : Color.white, 64);
				TMP_Text tmp_Text = this.MakeText(gameObject2.transform, "L", fileName, 13f, (active != null) ? active.controlText : Color.white, FontStyles.Normal, TextAlignmentOptions.MidlineLeft);
				RectTransform rectTransform2 = (RectTransform)tmp_Text.transform;
				CielOSMusicApp.Stretch(rectTransform2);
				rectTransform2.offsetMin = new Vector2(32f, 0f);
				rectTransform2.offsetMax = new Vector2(-8f, 0f);
				tmp_Text.enableWordWrapping = false;
				tmp_Text.overflowMode = TextOverflowModes.Ellipsis;
				gameObject2.GetComponent<Button>().onClick.AddListener(delegate
				{
					this._svc.PlayIndex(idx);
				});
				this._rows.Add(gameObject2);
			}
		}

		// Token: 0x06009DB1 RID: 40369 RVA: 0x0040D6A4 File Offset: 0x0040B8A4
		private void ImportMusic()
		{
			Canvas componentInParent = base.GetComponentInParent<Canvas>();
			if (componentInParent == null)
			{
				return;
			}
			CielOSFileBrowser browser = CielOSFileBrowser.Ensure(componentInParent.transform);
			browser.Open("Import Music", new string[] { ".mp3", ".wav", ".ogg" }, delegate(string real)
			{
				if (string.IsNullOrEmpty(real))
				{
					return;
				}
				if (CielOSFileSystem.Instance.ImportExternalFile(real, "/Music") != null)
				{
					browser.RememberSuccessfulDirectory(real);
					this._svc.Rescan();
				}
			}, CielOSMusicApp.GetDesktopImportFolder(), "cielos_shell", false);
		}

		// Token: 0x06009DB2 RID: 40370 RVA: 0x0040D728 File Offset: 0x0040B928
		public static string GetDesktopImportFolder()
		{
			string text = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
			if (!string.IsNullOrEmpty(text) && Directory.Exists(text))
			{
				return text;
			}
			text = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
			if (string.IsNullOrEmpty(text) || !Directory.Exists(text))
			{
				return null;
			}
			return text;
		}

		// Token: 0x06009DB3 RID: 40371 RVA: 0x0040D76C File Offset: 0x0040B96C
		private RectTransform NewChild(string name, Transform parent, float height)
		{
			GameObject gameObject = new GameObject(name, new Type[] { typeof(RectTransform) });
			gameObject.transform.SetParent(parent, false);
			if (height > 0f)
			{
				LayoutElement layoutElement = gameObject.AddComponent<LayoutElement>();
				layoutElement.minHeight = height;
				layoutElement.preferredHeight = height;
				layoutElement.flexibleHeight = 0f;
			}
			return (RectTransform)gameObject.transform;
		}

		// Token: 0x06009DB4 RID: 40372 RVA: 0x0040D7D4 File Offset: 0x0040B9D4
		private GameObject IconButton(Transform parent, CielOSIconId icon, Action onClick, float size, float rotate = 0f)
		{
			CielOSTheme active = CielOSTheme.Active;
			GameObject gameObject = new GameObject(icon.ToString(), new Type[]
			{
				typeof(RectTransform),
				typeof(Image),
				typeof(Button),
				typeof(LayoutElement)
			});
			gameObject.transform.SetParent(parent, false);
			LayoutElement component = gameObject.GetComponent<LayoutElement>();
			component.minWidth = size;
			component.preferredWidth = size;
			component.minHeight = size;
			component.preferredHeight = size;
			((RectTransform)gameObject.transform).sizeDelta = new Vector2(size, size);
			Image component2 = gameObject.GetComponent<Image>();
			component2.color = ((active != null) ? new Color(active.controlBackground.r * 0.7f, active.controlBackground.g * 0.7f, active.controlBackground.b * 0.7f, 1f) : new Color(0.13f, 0.1f, 0.17f, 1f));
			CielOSTheme.Circle(component2);
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
			rectTransform.offsetMin = new Vector2(size * 0.26f, size * 0.26f);
			rectTransform.offsetMax = new Vector2(-size * 0.26f, -size * 0.26f);
			if (rotate != 0f)
			{
				rectTransform.localEulerAngles = new Vector3(0f, 0f, rotate);
			}
			CielOSIcon.Apply(gameObject2.GetComponent<Image>(), icon, (active != null) ? active.logoAccent : Color.white, 64);
			return gameObject;
		}

		// Token: 0x06009DB5 RID: 40373 RVA: 0x0040D9F4 File Offset: 0x0040BBF4
		private GameObject LabeledIconButton(Transform parent, CielOSIconId icon, string label, Action onClick, float width, float height)
		{
			CielOSTheme active = CielOSTheme.Active;
			GameObject gameObject = new GameObject(label, new Type[]
			{
				typeof(RectTransform),
				typeof(Image),
				typeof(Button),
				typeof(LayoutElement),
				typeof(HorizontalLayoutGroup)
			});
			gameObject.transform.SetParent(parent, false);
			LayoutElement component = gameObject.GetComponent<LayoutElement>();
			component.minWidth = width;
			component.preferredWidth = width;
			component.flexibleWidth = 0f;
			component.minHeight = height;
			component.preferredHeight = height;
			Image component2 = gameObject.GetComponent<Image>();
			component2.color = ((active != null) ? active.accent : new Color(0.66f, 0.38f, 0.79f, 1f));
			CielOSTheme.Round(component2);
			gameObject.GetComponent<Button>().onClick.AddListener(delegate
			{
				onClick();
			});
			HorizontalLayoutGroup component3 = gameObject.GetComponent<HorizontalLayoutGroup>();
			component3.padding = new RectOffset(8, 8, 5, 5);
			component3.spacing = 5f;
			component3.childAlignment = TextAnchor.MiddleCenter;
			component3.childControlWidth = true;
			component3.childControlHeight = true;
			component3.childForceExpandWidth = false;
			component3.childForceExpandHeight = false;
			GameObject gameObject2 = new GameObject("Icon", new Type[]
			{
				typeof(RectTransform),
				typeof(Image),
				typeof(LayoutElement)
			});
			gameObject2.transform.SetParent(gameObject.transform, false);
			LayoutElement component4 = gameObject2.GetComponent<LayoutElement>();
			component4.minWidth = (component4.preferredWidth = 14f);
			component4.minHeight = (component4.preferredHeight = 14f);
			CielOSIcon.Apply(gameObject2.GetComponent<Image>(), icon, (active != null) ? active.logoAccent : Color.white, 64);
			TMP_Text tmp_Text = this.MakeText(gameObject.transform, "Label", label, 12f, (active != null) ? active.titleText : Color.white, FontStyles.Bold, TextAlignmentOptions.Center);
			tmp_Text.enableWordWrapping = false;
			tmp_Text.overflowMode = TextOverflowModes.Ellipsis;
			CielOSContentText.Bind(tmp_Text, label);
			LayoutElement layoutElement = tmp_Text.gameObject.AddComponent<LayoutElement>();
			layoutElement.minWidth = 42f;
			layoutElement.flexibleWidth = 1f;
			return gameObject;
		}

		// Token: 0x06009DB6 RID: 40374 RVA: 0x0040DC3C File Offset: 0x0040BE3C
		private Slider BuildSlider(Transform parent, float width, float height)
		{
			CielOSTheme active = CielOSTheme.Active;
			GameObject gameObject = new GameObject("Volume", new Type[]
			{
				typeof(RectTransform),
				typeof(Slider)
			});
			gameObject.transform.SetParent(parent, false);
			LayoutElement layoutElement = gameObject.AddComponent<LayoutElement>();
			layoutElement.minWidth = 72f;
			layoutElement.preferredWidth = width;
			layoutElement.flexibleWidth = 1f;
			layoutElement.minHeight = height;
			layoutElement.preferredHeight = height;
			Slider component = gameObject.GetComponent<Slider>();
			GameObject gameObject2 = new GameObject("BG", new Type[]
			{
				typeof(RectTransform),
				typeof(Image)
			});
			gameObject2.transform.SetParent(gameObject.transform, false);
			CielOSMusicApp.Stretch((RectTransform)gameObject2.transform);
			Image component2 = gameObject2.GetComponent<Image>();
			component2.color = ((active != null) ? active.barTrack : new Color(0.2f, 0.15f, 0.26f, 1f));
			CielOSTheme.Round(component2);
			GameObject gameObject3 = new GameObject("Fill Area", new Type[] { typeof(RectTransform) });
			gameObject3.transform.SetParent(gameObject.transform, false);
			RectTransform rectTransform = (RectTransform)gameObject3.transform;
			rectTransform.anchorMin = new Vector2(0f, 0.25f);
			rectTransform.anchorMax = new Vector2(1f, 0.75f);
			rectTransform.offsetMin = new Vector2(2f, 0f);
			rectTransform.offsetMax = new Vector2(-2f, 0f);
			GameObject gameObject4 = new GameObject("Fill", new Type[]
			{
				typeof(RectTransform),
				typeof(Image)
			});
			gameObject4.transform.SetParent(gameObject3.transform, false);
			RectTransform rectTransform2 = (RectTransform)gameObject4.transform;
			rectTransform2.anchorMin = Vector2.zero;
			rectTransform2.anchorMax = Vector2.one;
			rectTransform2.sizeDelta = Vector2.zero;
			Image component3 = gameObject4.GetComponent<Image>();
			component3.color = ((active != null) ? active.barFill : new Color(0.66f, 0.38f, 0.79f, 1f));
			CielOSTheme.Round(component3);
			GameObject gameObject5 = new GameObject("Handle Slide Area", new Type[] { typeof(RectTransform) });
			gameObject5.transform.SetParent(gameObject.transform, false);
			RectTransform rectTransform3 = (RectTransform)gameObject5.transform;
			CielOSMusicApp.Stretch(rectTransform3);
			rectTransform3.offsetMin = new Vector2(8f, 0f);
			rectTransform3.offsetMax = new Vector2(-8f, 0f);
			GameObject gameObject6 = new GameObject("Handle", new Type[]
			{
				typeof(RectTransform),
				typeof(Image)
			});
			gameObject6.transform.SetParent(gameObject5.transform, false);
			RectTransform rectTransform4 = (RectTransform)gameObject6.transform;
			rectTransform4.sizeDelta = new Vector2(14f, 14f);
			Image component4 = gameObject6.GetComponent<Image>();
			component4.color = ((active != null) ? active.logoAccent : Color.white);
			CielOSTheme.Circle(component4);
			component.fillRect = rectTransform2;
			component.handleRect = rectTransform4;
			component.targetGraphic = component4;
			component.direction = Slider.Direction.LeftToRight;
			component.minValue = 0f;
			component.maxValue = 1f;
			return component;
		}

		// Token: 0x06009DB7 RID: 40375 RVA: 0x0040DFA8 File Offset: 0x0040C1A8
		private void OnRectTransformDimensionsChange()
		{
			if (base.isActiveAndEnabled)
			{
				RectTransform rectTransform = base.transform as RectTransform;
				if (rectTransform != null)
				{
					LayoutRebuilder.MarkLayoutForRebuild(rectTransform);
				}
			}
		}

		// Token: 0x06009DB8 RID: 40376 RVA: 0x001A38AD File Offset: 0x001A1AAD
		private static void Stretch(RectTransform rt)
		{
			rt.anchorMin = Vector2.zero;
			rt.anchorMax = Vector2.one;
			rt.offsetMin = Vector2.zero;
			rt.offsetMax = Vector2.zero;
		}

		// Token: 0x06009DB9 RID: 40377 RVA: 0x0040DFD4 File Offset: 0x0040C1D4
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
			if (name == "Name")
			{
				CielOSContentText.Bind(component, content);
			}
			return component;
		}

		// Token: 0x0400A214 RID: 41492
		[SerializeField]
		private AudioSource audioSource;

		// Token: 0x0400A215 RID: 41493
		[SerializeField]
		private TMP_Text nowPlaying;

		// Token: 0x0400A216 RID: 41494
		[SerializeField]
		private RectTransform listContainer;

		// Token: 0x0400A217 RID: 41495
		[SerializeField]
		private Button playPauseButton;

		// Token: 0x0400A218 RID: 41496
		[SerializeField]
		private TMP_Text playPauseLabel;

		// Token: 0x0400A219 RID: 41497
		[SerializeField]
		private Button nextButton;

		// Token: 0x0400A21A RID: 41498
		[SerializeField]
		private Button prevButton;

		// Token: 0x0400A21B RID: 41499
		[SerializeField]
		private Button refreshButton;

		// Token: 0x0400A21C RID: 41500
		private CielOSAudioService _svc;

		// Token: 0x0400A21D RID: 41501
		private TMP_Text _nowName;

		// Token: 0x0400A21E RID: 41502
		private Image _playIcon;

		// Token: 0x0400A21F RID: 41503
		private Image _shuffleBg;

		// Token: 0x0400A220 RID: 41504
		private Image _repeatBg;

		// Token: 0x0400A221 RID: 41505
		private RectTransform _runtimeList;

		// Token: 0x0400A222 RID: 41506
		private Slider _volume;

		// Token: 0x0400A223 RID: 41507
		private readonly List<GameObject> _rows = new List<GameObject>();
	}
}
